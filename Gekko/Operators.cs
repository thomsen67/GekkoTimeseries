using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public static class Operators
    {
        //This class stores methods that perform an operation on two different IVariable types. For instance, adding
        //VAL 1.2 and a STRING 'a' produces 1.2a, whereas adding STRING 'a' and VAL 1.2 produces a1.2. But the internal
        //logic is the same, so we provide a StringVal() method with optional swap parameter. This way, the logic can be
        //stored in one place, instead of two places (in the ScalarString and ScalarVal classes).

        public static class DateVal
        {
            public static IVariable Add(ScalarDate t, ScalarVal d)
            {
                int i = O.ConvertToInt(d);
                GekkoTime t2 = t.date.Add(i);
                ScalarDate z = new ScalarDate(t2);
                return z;
            }

            public static IVariable Subtract(ScalarDate t, ScalarVal d)
            {
                //HMMM, what about rounding up and down here??
                int i = O.ConvertToInt(d);
                GekkoTime t2 = t.date.Add(-i);
                ScalarDate z = new ScalarDate(t2);
                return z;
            }
        }

        public static class StringVal
        {
            public static IVariable Add(ScalarString s, ScalarVal d, bool invert)
            {
                double dd = d.GetValOLD(null);
                string i = null;
                if (G.isNumericalError(dd))
                {
                    i = "M";
                }
                else
                {
                    i = dd.ToString();
                }                
                string z = null;
                if (invert)
                {
                    z = i + s.string2;
                }
                else
                {
                    z = s.string2 + i;
                }
                return new ScalarString(z);
            }            
        }

        public static class StringDate
        {
            public static IVariable Add(ScalarString s, ScalarDate d, bool invert)
            {
                GekkoTime gt = O.ConvertToDate(d);
                string z = null;
                if (invert)
                {
                    z = G.FromDateToString(gt) + s.string2;
                }
                else
                {
                    z = s.string2 + G.FromDateToString(gt);
                }
                return new ScalarString(z);
            }
        }

        public static class ScalarList
        {
            public static IVariable Add(GekkoSmpl smpl, IVariable scalar, IVariable list, bool swap)
            {
                List<IVariable> newList = new List<IVariable>();
                List<IVariable> l = O.ConvertToList(list);
                foreach (IVariable iv in l)
                {
                    IVariable temp = null;
                    if (swap)
                    {
                        temp = iv.Add(smpl, scalar);
                    }
                    else
                    {
                        temp = scalar.Add(smpl, iv);
                    }
                    newList.Add(temp);
                }

                return new List(newList);
            }
        }

        public static class StringList
        {
            public static IVariable Add(ScalarString s, List l, bool swap)
            {
                List<string> m = Program.GetListOfStringsFromList(l);
                List<string> newList = new List<string>();
                if (!swap)
                {
                    newList.Add(s.string2);
                    newList.AddRange(m);
                }
                else
                {
                    newList.AddRange(m);
                    newList.Add(s.string2);
                }
                return new List(newList);
            }
        }
    
    }
}
