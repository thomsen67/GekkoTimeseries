using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class List : IVariable
    {
        public bool isNameList = true; //no protobuf. See #76328234 for similar functionality for ScalarVal
        
        [ProtoMember(1)]
        public List<IVariable> list = null;  

        public List()
        {
            this.list = new List<IVariable>(); //empty list with 0 items
        }

        public List(List<IVariable> list)
        {
            this.list = list;
        }

        public List(IVariable[] list)
        {
            this.list = list.ToList<IVariable>();
        }

        public List(List<string> list)
        {
            List<IVariable> m = new List<Gekko.IVariable>();
            foreach (string s in list)
            {
                m.Add(new ScalarString(s));
            }
            this.list = m;
        }        

        //!!!This has nothing to do #m1+#m2 etc., see Add(GekkoSmpl t, IVariable x) instead.
        //   This method is just to avoid x.list.Add(...)
        public void Add(IVariable x)
        {
            if (this.list == null) this.list = new List<IVariable>();
            this.list.Add(x);
        }

        public int Count()
        {
            return this.list.Count;
        }

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                //Indices run from 1, 2, 3, ... n. Element 0 is length of list.
                if (index.Type() == EVariableType.Val)
                {
                    int ival = O.ConvertToInt(index);
                    if (ival < 0)
                    {
                        new Error("Illegal LIST indexer [" + ival + "]: negative number not allowed");
                    }
                    else if (ival == 0)
                    {
                        new Error("Illegal [0] list indexing. Use #m.len() or len(#m) instead of #m[0] to get the length of list #m.");
                    }
                    else if (ival > this.list.Count)
                    {
                        new Error("Illegal LIST indexer [" + ival + "]: larger than length of list (" + this.list.Count + ")");
                    }

                    return this.list[ival - 1];
                    //return this.list[ival - 1].DeepClone(); --> not necessary to clone: probably %y = #y[3] gets cloned anyway when assigning with '='

                }
                else if (index.Type() == EVariableType.Range)
                {
                    Range index_range = index as Range;

                    if (index_range.first != null && index_range.first.Type() == EVariableType.String && index_range.last != null && index_range.last.Type() == EVariableType.String)
                    {
                        string s1 = O.ConvertToString(index_range.first);
                        string s2 = O.ConvertToString(index_range.last);

                        List<string> m = new List<string>();
                        foreach (IVariable x in this.list)
                        {
                            string s = O.ConvertToString(x);
                            Program.AddIfInRange(null, null, s, s1, s2, m);
                        }
                        return new List(m);
                    }
                    else
                    {
                        //slice like #m[2..5], or #m[2..] or #m[..5] or even #m[..]
                        int ival1 = 1;
                        int ival2 = this.list.Count;
                        if (index_range.first != null) ival1 = O.ConvertToInt(index_range.first);
                        if (index_range.last != null) ival2 = O.ConvertToInt(index_range.last);
                        CheckRange(ival1, ival2, false);
                        List<IVariable> tmp = new List<Gekko.IVariable>();
                        for (int i = ival1 - 1; i <= ival2 - 1; i++)
                        {
                            tmp.Add(this.list[i].DeepClone(null));
                        }
                        List m = new List(tmp);
                        m.isNameList = this.isNameList;
                        return m;
                    }
                }
                else if (index.Type() == EVariableType.String)
                {
                    string s5 = ((ScalarString)index).string2;
                    if (s5.Contains("?") || s5.Contains("*"))
                    {
                        //Wildcard: return a list of those
                        List<string> found = Program.MatchWildcard(s5, new List<string>(Stringlist.GetListOfStringsFromListOfIvariables(this.list.ToArray()))); //the list items are sorted, not like a dict                        
                        return new List(found);
                    }
                    else
                    {
                        //Request if the list has the item
                        foreach (IVariable x in this.list)
                        {
                            ScalarString x_string = x as ScalarString;
                            if (x_string != null)
                            {
                                if (G.Equal(x_string.string2, s5)) return Globals.scalarVal1;
                            }
                        }
                        return Globals.scalarVal0;
                    }
                }
                else
                {
                    new Error("Type mismatch regarding []-index"); return null;
                }
            }
            else if (indexes.Length == 2)
            {
                if (indexes[0].Type() == EVariableType.Val)
                {
                    //#m[3, 5] or #m[3, 4..5] or #m[1..2, 4..5]
                    return this.Indexer(t, indexerType, indexes[0]).Indexer(t, indexerType, indexes[1]);
                }
                else if (indexes[0].Type() == EVariableType.Range)
                {
                    //#m[1..2, 4]
                    //this corresponds to selecting a column. Note that #m[1..2, 4] is different from #m[1..2][4]!
                                        
                    Range index_range1 = indexes[0] as Range;
                    int ival1 = 1;
                    int ival2 = this.list.Count;
                    if (index_range1.first != null) ival1 = O.ConvertToInt(index_range1.first);
                    if (index_range1.last != null) ival2 = O.ConvertToInt(index_range1.last);
                    CheckRange(ival1, ival2, true);

                    List m = null;
                    if (indexes[1].Type() == EVariableType.Val)
                    {
                        int j = O.ConvertToInt(indexes[1]);
                        m = GetColumnSlice(ival1, ival2, j);
                    }
                    else if (indexes[1].Type() == EVariableType.Range)
                    {
                        int jval1 = 1;
                        int jval2 = this.list.Count;
                        Range index_range2 = indexes[1] as Range;
                        if (index_range2.first != null) jval1 = O.ConvertToInt(index_range2.first);
                        if (index_range2.last != null) jval2 = O.ConvertToInt(index_range2.last);
                        m = new List();
                        for (int j = jval1; j <= jval2; j++)
                        {
                            List m3 = GetColumnSlice(ival1, ival2, j);
                            m.list.Add(m3);
                        }
                        m = Functions.t(null, null, null, m) as List;
                    }
                    else
                    {
                        new Error("Expected value or range for second dimension of indexer"); return null;
                    }
                    return m;
                }
                else
                {
                    new Error("Invalid use of [..., ...] indexer on list"); return null;
                }
            }
            else
            {
                new Error("Cannot use " + indexes.Length + "-dimensional indexer on LIST"); return null;
            }
        }

        private List GetColumnSlice(int ival1, int ival2, int j)
        {
            List m = new List();
            for (int i = ival1; i <= ival2; i++)
            {
                List m2 = this.list[i - 1] as List;
                if (m2 == null)
                {
                    new Error("Element " + i + " of list is not a (sub)list");
                }
                if (j < 1 || j > m2.list.Count)
                {
                    new Error("Sublist " + i + " of list has illegal index (" + j + ")");
                }
                m.list.Add(m2.list[j - 1]);
            }

            return m;
        }

        private void CheckRange(int ival1, int ival2, bool twod)
        {
            if (ival1 > this.list.Count || ival2 > this.list.Count || ival2 < ival1 || ival1 < 1 || ival2 < 1)
            {
                if (twod) new Error("Invalid range in first dimension, [" + ival1 + " .. " + ival2 + ", ...]");
                else new Error("Invalid range, [" + ival1 + " .. " + ival2 + "]");
            }
        }

        public IVariable Negate(GekkoSmpl t)
        {
            new Error("You cannot use minus with lists"); return null;                
        }

        
        public double GetValOLD(GekkoSmpl t)
        {
            new Error("Type mismatch: you are trying to extract a VAL from a list. Maybe you need an []-indexer on the list, for instance #mylist[2]?"); return double.NaN;
        }

        public double GetVal(GekkoTime t)
        {
            new Error("Type mismatch: you are trying to extract a VAL from a list."); return double.NaN;
        }

        public double ConvertToVal()
        {
            new Error("Cannot extract a val from " + G.GetTypeString(this) + " type"); return double.NaN;
        }

        public string ConvertToString()
        {
            new Error("Trying to convert a LIST into a STRING."); return null;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            new Error("Type mismatch: you are trying to extract a DATE from a list."); return GekkoTime.tNull;
        }

        public List<IVariable> ConvertToList()
        {
            return this.list;
        }

        public EVariableType Type()
        {
            return EVariableType.List;
        }

        public IVariable Add(GekkoSmpl smpl, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        return Functions.extend(smpl, null, null, this, x);
                    }
                    break;
                case EVariableType.String:
                case EVariableType.Val:
                case EVariableType.Date:
                    {
                        using (Error txt = new Error())
                        {
                            txt.MainAdd("Adding a list and a scalar with the '+' operator like for instance #x + %y is not legal.");
                            txt.MainAdd("If you want to append a scalar to a list, you may use for instance #z = #x.append(%y);.");                            
                            if (x.Type() == EVariableType.String)
                            {
                                txt.MainAdd("For scalar strings, another possibility is to use a {a{naked list¤i_naked_list.htm}a} definintion with comma: #z = {#x}, {%y}; or #z = {#x}, a;.");
                                txt.MainAdd("The latter example appends the string 'a' to the list.");
                            }
                            //
                            txt.MoreAdd("In general, you may use the append() function, for instance #z = #x.append(%y);.");
                            txt.MoreAdd("These can be nested, like for instance #z = #x.append(%y1).append(%y2);.");
                            txt.MoreAdd("Note: to prepend a scalar to a list, you can use #x.prepend(%y).");
                            txt.MoreAdd("Another possibility is for instance #z = #x + (%y,);, putting the %y variable into a 1-element list definition.");                            
                            txt.MoreNewLine();                            
                            //
                            ListAndScalarErrorMessage(txt);
                        }
                    }
                    break;
                default:
                    {
                        new Error("Add to list not allowed for this type: " + G.GetTypeString(x));
                    }
                    break;
            }

            return Functions.extend(smpl, null, null, this, x);
        }

        /// <summary>
        /// An error message shown when adding for instance #x + %y
        /// </summary>
        /// <param name="txt"></param>
        public static void ListAndScalarErrorMessage(Error txt)
        {
            txt.MoreAdd("Regarding scalar strings: in Gekko 2.x it was possible to write for instance #x + %y on the right-hand side of a list definition,");
            txt.MoreAdd("and have the string appended to the list. This is disallowed in Gekko 3.x, because it is potentially confusing and error-prone.");
            txt.MoreAdd("Consider #y = 'a' + #x + 'b';. In Gekko 2.x, the #z list would contain 'a' as the first element, then the elements of #x,");
            txt.MoreAdd("and finally 'b' as the last element. Now imagine that we want to move 'b' to be added before the elements of #x. This would");
            txt.MoreAdd("correspond to #y = 'a' + 'b' + #x;, but the 'a' + 'b' part of this would be evaluated first and would return 'ab', since adding strings generally works");
            txt.MoreAdd("that way in Gekko, and hence the first element of #y would become 'ab'. There is no way out if this problem, and it could");
            txt.MoreAdd("easily produce hard-to-catch bugs. But then couldn't at least #y = #x + 'a' + 'b'; be legal, appending first 'a' and then 'b' to the list #x?");
            txt.MoreAdd("But this could easily produce other kinds of bugs, for instance if the user first writes #y = #x + (%s1 + %s2);,");
            txt.MoreAdd("adding the concatenated string to the list, but afterwards deletes the parentheses because they seems superfluous.");
            txt.MoreAdd("Because of these kinds of potential bugs, in Gekko 3.x you cannot add a list and a scalar using the '+' operator.");
        }

        /// <summary>
        /// An error message shown when adding for instance %y + #x
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="isString"></param>
        public static void ScalarAndListErrorMessage(Error txt, bool isString)
        {
            txt.MainAdd("Adding a scalar and a list with the '+' operator like for instance %y + #x is not legal.");
            txt.MainAdd("If you want to prepend a scalar to a list, you may use for instance #z = #x.prepend(%y);.");
            if (isString)
            {
                txt.MainAdd("For scalar strings, another possibility is to use a {a{naked list¤i_naked_list.htm}a} definintion with comma: #z = {%y}, {#x}; or #z = a, {#x}.");
                txt.MainAdd("The latter example prepends the string 'a' to the list.");
            }
            //
            txt.MoreAdd("In general, you may use the prepend() function, for instance #z = #x.prepend(%y);.");
            txt.MoreAdd("These can be nested, like for instance #z = #x.prepend(%y2).prepend(%y1);, beware of the inversed insertion order.");
            txt.MoreAdd("Note: to append a scalar to a list, you can use #x.append(%y).");
            txt.MoreAdd("Another possibility is for instance #z = (%y,) + #x;, putting the %y variable into a 1-element list definition.");
            //
            txt.MoreNewLine();
            List.ListAndScalarErrorMessage(txt);
        }

        public IVariable Concat(GekkoSmpl smpl, IVariable x)
        {
            
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        //should this be allowed, as in COPY {#b}:{#v} ... ? Does it even work?
                        List rv = new List();
                        List x_list = x as List;
                        foreach (IVariable iv1 in this.list)
                        {
                            foreach (IVariable iv2 in x_list.list)
                            {
                                rv.list.Add(iv1.Concat(smpl, iv2));
                            }
                        }
                        return rv;
                    }
                    break;
                case EVariableType.String:
                case EVariableType.Val:
                case EVariableType.Date:
                    {
                        //This is only allowed for stuff like COPY b:{#m}!q etc.
                        //See also #786592387654
                        return Operators.ScalarList.Add(smpl, x, this, true);  //note: swapping
                    }
                    break;
                default:
                    {
                        new Error("Concat to list not allowed for this type: " + G.GetTypeString(x));
                    }
                    break;
            }

            return Functions.extend(smpl, null, null, this, x);
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        return Functions.except(t, null, null, this, x);
                    }
                    break;
                case EVariableType.String:
                    {
                        return Functions.except(t, null, null, this, new List(new List<IVariable> { x }));
                    }
                    break;
                default:
                    {
                        new Error("Subtract from list not allowed for this type: " + G.GetTypeString(x)); return null;
                    }
                    break;               
            }
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {
                case EVariableType.List:
                    {
                        return Functions.intersect(t, null, null, this, x);
                    }
                    break;
                case EVariableType.String:
                    {
                        return Functions.intersect(t, null, null, this, new List(new List<IVariable> { x }));
                    }
                    break;
                default:
                    {
                        new Error("Intersect with list not allowed for this type: " + G.GetTypeString(x)); return null;
                    }
                    break;
            }
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use divide with lists"); return null;                
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use power function with lists"); return null;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            if (dims.Length == 1 && dims[0].Type() == EVariableType.Val)
            {
                int i = O.ConvertToInt(dims[0]);
                if (i < 1 || i > this.list.Count)
                {
                    new Error("List index [" + i + "] out of range 1.." + this.list.Count);
                }
                this.list[i - 1] = rhsExpression.DeepClone(null);
            }
            else
            {
                new Error("Expected indexer type VAL on LIST object (left-hand side)");
            }
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            List<IVariable> temp = new List<IVariable>();
            if (this.list == null)
            {
                //should not happen, but just in case
            }
            else
            {
                foreach (IVariable iv in this.list)
                {
                    if (!Object.ReferenceEquals(this, iv))  //avoid problems if the list contains itself
                    {                        
                        temp.Add(iv.DeepClone(truncate));
                    }
                    else
                    {
                        //a list containing itself
                        temp.Add(iv);
                    }
                }
            }
            List l = new List(temp); //l.isFromNakedList should not be set, not relevant when cloning
            return l;
        }

        public void DeepTrim()
        {            
            foreach (IVariable iv in this.list)
            {
                if (!Object.ReferenceEquals(this, iv))  //avoid problems if the list contains itself
                {                    
                    iv.DeepTrim();
                }
            }         
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            if (this.list == null) this.list = new List<IVariable>();
            foreach (IVariable iv in this.list)
            {
                if (!Object.ReferenceEquals(this, iv))  //avoid problems if the list contains itself
                {
                    iv.DeepCleanup(yearMinMax);
                }
            }
        }
    }
}
