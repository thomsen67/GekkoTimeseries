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
                int i = O.GetInt(d);
                GekkoTime t2 = t.date.Add(i);
                ScalarDate z = new ScalarDate(t2);
                return z;
            }

            public static IVariable Subtract(ScalarDate t, ScalarVal d)
            {
                //HMMM, what about rounding up and down here??
                int i = O.GetInt(d);
                GekkoTime t2 = t.date.Add(-i);
                ScalarDate z = new ScalarDate(t2);
                return z;
            }
        }

        public static class StringVal
        {
            public static IVariable Add(ScalarString s, ScalarVal d, bool invert)
            {
                double dd = d.GetVal(null);
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
                    z = i + s._string2;
                }
                else
                {
                    z = s._string2 + i;
                }
                return new ScalarString(z);
            }            
        }

        public static class StringDate
        {
            public static IVariable Add(ScalarString s, ScalarDate d, bool invert)
            {
                GekkoTime gt = O.GetDate(d);
                string z = null;
                if (invert)
                {
                    z = G.FromDateToString(gt) + s._string2;
                }
                else
                {
                    z = s._string2 + G.FromDateToString(gt);
                }
                return new ScalarString(z);
            }
        }

        public static class ValTimeSeries
        {
            public static IVariable Add(ScalarVal x, MetaTimeSeries ats, GekkoSmpl t)
            {
                //no need to implement swap
                if (t == null) throw new GekkoException();                
                TimeSeries ts = ats.ts;
                double val1 = x.val;
                double val2 = ts.GetData(t.t1.Add(ats.offset));  //uuu
                return new ScalarVal(val1 + val2);
            }
        }

        public static class StringList
        {
            public static IVariable Add(ScalarString s, MetaList l, bool swap)
            {
                List<string> m = O.GetStringList(l);
                List<string> newList = new List<string>();
                if (!swap)
                {
                    newList.Add(s._string2);
                    newList.AddRange(m);
                }
                else
                {
                    newList.AddRange(m);
                    newList.Add(s._string2);
                }
                return new MetaList(newList);
            }
        }
    
    }
}
