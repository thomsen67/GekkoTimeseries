using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Controls.Primitives;
using static GAMS.GAMSOptions;
using static Gekko.O;
using System.Management;
using static Gekko.Program;

namespace Gekko
{
    public class GamsScalarHelper
    {
        public string zip_name = null;
        public string raw_path = null;
        public object[] raw_ignore = null;
        public string variable = null;
        public string model = null;
        public string counts1 = "**** counts do not match";
        public string counts2 = "**** unmatched free variables";
        public string counts3 = "**** number of unmatched =e= rows";
        public int? t1 = null;
        public int? t2 = null;
        public object[] cmd_lines = null;
        public object[] gms_lines = null;
    }
    public class RootHelper
    {
        public string rootFileName = null;
        public List<string> roots = new List<string>();
    }

    public class Functions
    {
        //NOTE:
        //NOTE: All function names should be with lower-case only!!
        //NOTE: All function helpers should be PRIVATE!!
        //        --> or if the function must have access from CS code,
        //            call it HELPER_methodname().
        //NOTE:

        public enum EElementByElementType
        {
            Times,
            Divide
        }

        public enum ESumDim
        {
            Rows,
            Cols
        }

        public enum ESumType
        {
            Min,
            Max,
            Sum,
            Avg
        }

        // ===========================================================================================================================
        // ========================= functions to manipulate dates start =============================================================
        // ===========================================================================================================================

        public static IVariable date(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable iv1, IVariable iv2)
        {
            //%d.date('m');
            GekkoTime dd = O.ConvertToDate(iv1);
            string ff = O.ConvertToString(iv2);
            GekkoTime gt = Program.ConvertFreq(dd, G.ConvertFreq(ff), null);
            return new ScalarDate(gt);
        }
        public static void arraypack(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable iv1, IVariable iv2)
        {
            string aName = O.ConvertToString(iv1);
            string lName = O.ConvertToString(iv2);
            List<string> m = new List<string>();
            EFreq freq = EFreq.None;
            List<KeyValuePair<string, IVariable>> x = new List<KeyValuePair<string, IVariable>>();
            foreach (KeyValuePair<string, IVariable> kvp in Program.databanks.GetFirst().storage)
            {
                if (kvp.Value.Type() != EVariableType.Series) continue;
                Series ts = kvp.Value as Series;
                if (ts.dimensions > 0) continue;
                x.Add(kvp);
                if (freq == EFreq.None) freq = ts.freq;
                else
                {
                    if (freq != ts.freq) new Error("Not all non-arrayseries in the first-position databank have the same frequency");
                }
            }
            if (freq == EFreq.None) new Error("No non-arrayseries found in the first-position databank");
            Series ats = new Series(freq, G.Chop_AddFreq(aName, freq));
            ats.meta.domains = new string[] { lName };
            ats.SetArrayTimeseries(2, true);
            foreach (KeyValuePair<string, IVariable> kvp in x)
            {                
                ats.dimensionsStorage.AddIVariableWithOverwrite(new MultidimItem(new string[] { G.Chop_RemoveFreq(kvp.Key) }, ats), kvp.Value);
                Program.databanks.GetFirst().RemoveIVariable(kvp.Key);
                m.Add(G.Chop_RemoveFreq(kvp.Key));
            }
            Program.databanks.GetFirst().Clear();
            Program.databanks.GetFirst().AddIVariableWithOverwrite(ats);
            List mm = new List(m);
            if (lName != "*") Program.databanks.GetFirst().AddIVariableWithOverwrite(lName, mm);            
        }

        public static void arrayunpack(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable iv1)
        {
            string aName = O.ConvertToString(iv1);
            IVariable iv = Program.databanks.GetFirst().GetIVariable(G.Chop_AddFreq(aName, Program.options.freq));
            if (iv == null) new Error("Cound not find variable '" + aName + "' in the first-position databank");
            Series ats = iv as Series;
            if (ats == null) new Error("The variable '" + aName + "' is not a timeseries");
            if (ats.type != ESeriesType.ArraySuper) new Error("The series '" + aName + "' is not an array-timeseries");
            if (ats.dimensions != 1) new Error("Expected series '" + aName + "' to be 1-dimensional");
            List<KeyValuePair<string, IVariable>> x = new List<KeyValuePair<string, IVariable>>();
            foreach (KeyValuePair<MultidimItem, IVariable> kvp in ats.dimensionsStorage.storage)
            {
                MultidimItem map = kvp.Key;
                string s = map.storage[0];
                Series ts = kvp.Value as Series;
                x.Add(new KeyValuePair<string, IVariable>(G.Chop_AddFreq(s, ts.freq), ts));
            }
            Program.databanks.GetFirst().Clear();
            foreach (KeyValuePair<string, IVariable> kvp in x)
            {
                Program.databanks.GetFirst().AddIVariable(kvp.Key, kvp.Value);
            }
        }

        public static IVariable date(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable iv1, IVariable iv2, IVariable iv3)
        {
            if (iv1.Type() == EVariableType.Val)
            {
                //%d = date(2020, 'q', 2); //2020q2
                int yy = O.ConvertToInt(iv1);
                string ff = O.ConvertToString(iv2);
                int ss = O.ConvertToInt(iv3);
                GekkoTime gt = new GekkoTime(G.ConvertFreq(ff), yy, ss);
                return new ScalarDate(gt);
            }
            else
            {
                //%d.date('m', 'start');
                GekkoTime dd = O.ConvertToDate(iv1);
                string ff = O.ConvertToString(iv2);
                string startEnd2 = O.ConvertToString(iv3);
                GekkoTime gt = Program.ConvertFreq(dd, G.ConvertFreq(ff), startEnd2);
                return new ScalarDate(gt);
            }
        }

        public static IVariable date(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable year, IVariable m, IVariable month, IVariable d, IVariable day)
        {
            if (year.Type() == EVariableType.Val)
            {
                //%d = date(2020, 'm', 2, 'd', 4); //2020m2d4
                int year_int = O.ConvertToInt(year);
                string m_string = O.ConvertToString(m);
                if (!G.Equal(m_string, "m"))
                {
                    new Error("date(): expected year, month and day");
                }
                int month_int = O.ConvertToInt(month);

                string day_string = O.ConvertToString(d);
                if (!G.Equal(day_string, "d"))
                {
                    new Error("date(): expected year, month and day");
                }
                int day_int = O.ConvertToInt(day);
                GekkoTime gt = new GekkoTime(EFreq.D, year_int, month_int, day_int);
                return new ScalarDate(gt);
            }
            else
            {
                new Error("date(): expected year, month and day"); return null;
            }
        }

        public static IVariable getyear(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                new Error("getyear() expects date input");
                //throw new GekkoException();
            }

            GekkoTime gt = (ths as ScalarDate).date;
            return new ScalarVal(gt.super);
        }

        public static IVariable getweek(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            //%d.getWeek() is only legal for a %d of weekly frequency.
            //This is to notify that there may be a year problem.
            //But one can use %d.date('w').getWeek().
            if (ths.Type() == EVariableType.Date && ((ScalarDate)ths).date.freq == EFreq.W)
            {
                int week = ((ScalarDate)ths).date.sub;
                return new ScalarVal(week);
            }
            else
            {
                using (Error txt = new Error())
                {
                    txt.MainAdd("The getWeek() function only accepts a date with weekly frequency as input, so you cannot use %d.getWeek() directly ");
                    txt.MainAdd("on for instance a daily date %d. This is to remind you that when getting a week number from a daily date, ");
                    txt.MainAdd("the year may change. You may use %d.date('w').getWeek() instead, cf. the explanation in the link. ");
                    // ---
                    txt.MoreAdd("Week numbers are special around New Year. For instance, December 31, 2019 is in week 1, 2020. And January 1, 2021, is in week 53, 2020.");
                    txt.MoreAdd("So the year may change, and the user should be aware of this. Therefore, getWeek() cannot be ");
                    txt.MoreAdd("used directly on a daily date. Instead, you may first convert the daily date to a weekly date with the ");
                    txt.MoreAdd("date('w') function. Hence, to find the week number of a given daily date %d, you can use %d.date('w').getWeek(). Note ");
                    txt.MoreAdd("here that %d.date('w').getYear() will get the year of the weekly date, and this year may be different from %d.getYear()!");
                }

                return null;
            }
        }

        public static IVariable getsubper(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            //returns month for daily freq
            if (ths.Type() != EVariableType.Date)
            {
                new Error("getsubper() expects date input");
                //throw new GekkoException();
            }

            GekkoTime gt = (ths as ScalarDate).date;
            return new ScalarVal(gt.sub);
        }

        public static IVariable getquarter(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                new Error("getquarter() expects date input");
                //throw new GekkoException();
            }

            GekkoTime gt = (ths as ScalarDate).date;
            if (gt.freq != EFreq.Q)
            {
                new Error("getquarter() expects quarterly date");
                //throw new GekkoException();
            }
            return new ScalarVal(gt.sub);
        }

        public static IVariable getmonth(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            return getmonth(smpl, _t1, _t2, ths, null);
        }

        public static IVariable getmonth(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable lang)
        {
            if (ths.Type() != EVariableType.Date)
            {
                new Error("getmonth() expects date input");
            }

            GekkoTime gt = (ths as ScalarDate).date;
            if (gt.freq != EFreq.D && gt.freq != EFreq.M)
            {
                new Error("getmonth() expects monthly or daily date");
            }

            int month = gt.sub;            

            if (lang == null)
            {
                return new ScalarVal(month);
            }
            else
            {
                string language = O.ConvertToString(lang);
                string s = null;
                foreach (MonthNames m in GekkoTime.MonthNames)
                {
                    if (G.Equal(language, Globals.languageEn))
                    {
                        if (m.number == month) return new ScalarString(m.en);
                    }
                    else if (G.Equal(language, Globals.languageDa))
                    {
                        if (m.number == month) return new ScalarString(m.da);
                    }
                    else new Error("Language '" + language + "' not recognized.");
                }
                new Error("Unexpected error #623uikhd7af1");  //should not be possible
                return null;
            }
        }

        public static IVariable getday(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() != EVariableType.Date)
            {
                new Error("getday() expects date input");
            }

            GekkoTime gt = (ths as ScalarDate).date;
            if (gt.freq != EFreq.D)
            {
                new Error("getday() expects daily date");
            }
            return new ScalarVal(gt.subsub);
        }

        public static IVariable getweekday(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            return getweekday(smpl, _t1, _t2, ths, null);
        }

        public static IVariable getweekday(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable lang)
        {
            if (ths.Type() != EVariableType.Date)
            {
                new Error("getweekday() expects date input");
            }

            GekkoTime gt = (ths as ScalarDate).date;
            if (gt.freq != EFreq.D)
            {
                new Error("getweekday() expects daily date");
            }

            DateTime dt1 = GekkoTime.FromGekkoTimeToDateTime(gt, O.GetDateChoices.Strict);
            int day = (int)dt1.DayOfWeek;  //sunday = 0, monday = 1, ... , saturday = 6.
            if (day == 0) day = 7;  //now day is: monday = 1, ... , saturday = 6, sunday = 7.

            if (lang == null)
            {
                return new ScalarVal(day);
            }
            else
            {
                string language = O.ConvertToString(lang);
                string s = null;
                foreach (WeekDayNames w in GekkoTime.WeekdayNames)
                {
                    if (G.Equal(language, Globals.languageEn))
                    {
                        if (w.number == day) return new ScalarString(w.en);
                    }
                    else if (G.Equal(language, Globals.languageDa))
                    {
                        if (w.number == day) return new ScalarString(w.da);
                    }
                    else new Error("Language '" + language + "' not recognized.");
                }
                new Error("Unexpected error #623uikhd7af6");  //should not be possible
                return null;
            }
        }

        public static IVariable getparent(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() != EVariableType.Series)
            {
                new Error("getparent() expects series input");
            }

            Series ts = ths as Series;

            if (ts.type == ESeriesType.ArraySuper)
            {
                new Error("getparent(): this is already an array parent-series");
                //throw new GekkoException();
            }
            else if (ts.type == ESeriesType.Light)
            {
                new Error("getparent(): an expression cannot have a parent series");
                //throw new GekkoException();
            }
            if (ts.mmi == null)
            {
                new Error("getparent(): this series is not an array subseries");
                //throw new GekkoException();
            }

            if (ts.mmi.parent == null)
            {
                new Error("getparent(): this array subseries does not have a parent series assigned to it");
                //throw new GekkoException();
            }

            return ts.mmi.parent;

        }

        // ===========================================================================================================================
        // ========================= functions to manipulate dates end ===============================================================
        // ===========================================================================================================================





        // ===========================================================================================================================
        // ========================= functions to manipulate bankvarnames with indexes start =========================================
        // ===========================================================================================================================


        //See equivalent method in G.cs
        public static IVariable getbank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getbank(smpl, _t1, _t2, item));
                return rv;
            }
            if (ths.Type() == EVariableType.Series)
            {
                return new ScalarString((ths as Series).GetParentDatabank().name);
            }
            else
            {
                string ss = G.Chop_GetBank(O.ConvertToString(ths));
                return new ScalarString(ss);
            }
        }


        //See equivalent method in G.cs
        public static IVariable getname(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getname(smpl, _t1, _t2, item));
                return rv;
            }
            if (ths.Type() == EVariableType.Series)
            {
                return new ScalarString(G.Chop_GetName((ths as Series).GetName()));
            }
            else
            {
                return new ScalarString(G.Chop_GetName(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getfreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getfreq(smpl, _t1, _t2, item));
                return rv;
            }
            else if (ths.Type() == EVariableType.Date)
            {
                GekkoTime gt = (ths as ScalarDate).date;
                return new ScalarString(G.ConvertFreq(gt.freq));
            }
            if (ths.Type() == EVariableType.Series)
            {
                return new ScalarString(G.ConvertFreq((ths as Series).freq));
            }
            else
            {
                return new ScalarString(G.Chop_GetFreq(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getnameandfreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getnameandfreq(smpl, _t1, _t2, item));
                return rv;
            }
            if (ths.Type() == EVariableType.Series)
            {
                return new ScalarString((ths as Series).GetName());
            }
            else
            {
                return new ScalarString(G.Chop_GetNameAndFreq(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getindex(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(getindex(smpl, _t1, _t2, item));
                return rv;
            }
            else
            {
                return new List(G.Chop_GetIndex(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable getfullname(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ivbank, IVariable ivname, IVariable ivfreq)
        {
            return getfullname(smpl, _t1, _t2, ivbank, ivname, ivfreq, null);
        }

        //See equivalent method in G.cs
        public static IVariable getfullname(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ivbank, IVariable ivname, IVariable ivfreq, IVariable ivindex)
        {
            string bank = O.ConvertToString(ivbank);
            string name = O.ConvertToString(ivname);
            string freq = O.ConvertToString(ivfreq);
            string[] index = null;
            if (ivindex != null) index = Stringlist.GetListOfStringsFromListOfIvariables(O.ConvertToList(ivindex).ToArray());
            string s = G.Chop_GetFullName(bank, name, freq, index);
            return new ScalarString(s);
        }

        //See equivalent method in G.cs
        public static IVariable addbank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(addbank(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_AddBank(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable setbank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setbank(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetBank(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removebank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removebank(smpl, _t1, _t2, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveBank(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removebank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removebank(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveBank(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable replacebank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(replacebank(smpl, _t1, _t2, item, x2, x3));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_ReplaceBank(O.ConvertToString(ths), O.ConvertToString(x2), O.ConvertToString(x3)));
            }
        }



        //See equivalent method in G.cs
        public static IVariable addfreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(addfreq(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_AddFreq(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable setfreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setfreq(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetFreq(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removefreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removefreq(smpl, _t1, _t2, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveFreq(O.ConvertToString(ths)));
            }
        }

        public static IVariable removeindex(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removeindex(smpl, _t1, _t2, item));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveIndex(O.ConvertToString(ths)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable removefreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(removefreq(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_RemoveFreq(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        //See equivalent method in G.cs
        public static IVariable replacefreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(replacefreq(smpl, _t1, _t2, item, x2, x3));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_ReplaceFreq(O.ConvertToString(ths), O.ConvertToString(x2), O.ConvertToString(x3)));
            }
        }

        //See equivalent method in G.cs
        public static IVariable setname(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setname(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetName(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }

        public static IVariable setnameprefix(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setnameprefix(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetNamePrefix(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }

        public static IVariable setnamesuffix(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2)
        {
            if (ths.Type() == EVariableType.List)
            {
                List rv = new List();
                foreach (IVariable item in (ths as List).list) rv.Add(setnamesuffix(smpl, _t1, _t2, item, x2));
                return rv;
            }
            else
            {
                return new ScalarString(G.Chop_SetNameSuffix(O.ConvertToString(ths), O.ConvertToString(x2)));
            }
        }


        // ===========================================================================================================================
        // ========================= functions to manipulate bankvarnames with indexes end ===========================================
        // ===========================================================================================================================


        public static IVariable rotate(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable dim)
        {
            int iDim = O.ConvertToInt(dim);

            Series ts = x1 as Series;
            if (ts == null || ts.type != ESeriesType.ArraySuper)
            {
                new Error("You must use a array-timeseries variable");
                //throw new GekkoException();
            }

            if (iDim > ts.dimensions || iDim < 1)
            {
                new Error("Array-series does not have a dimension #" + iDim);
                //throw new GekkoException();
            }

            //For Gekko 4.0, set name to G.GetArraySeriesTempName(G.ConvertFreq(EFreq.U))
            Series tsRotated = new Series(EFreq.U, G.Chop_SetFreq(ts.name, G.ConvertFreq(EFreq.U)));
            tsRotated.meta.label = ts.meta.label;
            tsRotated.SetArrayTimeseries(ts.dimensions + 1, true);

            foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
            {
                //foreach array-subseries, for instance x[#age] over the ages 18-100
                //must be converted into y[#t] where #t is for instance 1950-2100, and the timeperiod is undated 18-100

                MultidimItem map = kvp.Key;
                string s = map.storage[iDim - 1];
                int a = G.IntParse(s);
                if (a == -12345)
                {
                    new Note("Could not parse '" + s + "' as an integer, skipped");
                    continue;
                }

                Series tsSub = kvp.Value as Series;
                if (tsSub == null)
                {
                    new Error("Element is not a series");  //should not be possible
                    //throw new GekkoException();
                }

                if (tsSub.type == ESeriesType.Timeless)
                {
                    new Error("Sub-series is timeless ... conversion will be fixed later on");
                    //throw new GekkoException();
                }

                foreach (GekkoTime t in new GekkoTimeIterator(tsSub.GetRealDataPeriodFirst(), tsSub.GetRealDataPeriodLast()))
                {
                    MultidimItem mapRotated = map.Clone();
                    map.parent = tsRotated;
                    mapRotated.storage[iDim - 1] = t.ToString();

                    Series tsRotatedSub = null;
                    IVariable iv2 = null; tsRotated.dimensionsStorage.TryGetValue(mapRotated, out iv2);
                    if (iv2 == null)
                    {
                        tsRotatedSub = new Series(EFreq.U, null);
                        tsRotated.dimensionsStorage.AddIVariableWithOverwrite(mapRotated, tsRotatedSub);
                    }
                    else
                    {
                        tsRotatedSub = iv2 as Series;
                    }
                    GekkoTime tu = new GekkoTime(EFreq.U, a, 1);
                    tsRotatedSub.SetData(tu, tsSub.GetVal(t));
                }
            }

            return tsRotated;
        }

        public static IVariable bankname(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (x1.Type() == EVariableType.String)
            {
                string s = x1.ConvertToString();
                if (G.Equal(s, Globals.First))
                {
                    return new ScalarString(Program.databanks.GetFirst().name);
                }
                else if (G.Equal(s, Globals.Ref))
                {
                    return new ScalarString(Program.databanks.GetRef().name);
                }
                else
                {
                    new Error("bankname() accepts strings 'first' or 'ref'"); return null;
                    //throw new GekkoException();
                }
            }
            else if (x1.Type() == EVariableType.Val)
            {

                int x = O.ConvertToInt(x1);
                Databank db = null;
                if (x < 0)
                {
                    new Error("bankname() must be called with value >= 0"); return null;
                    //throw new GekkoException();
                }
                else if (x == 0)
                {
                    return new ScalarVal(Program.databanks.storage.Count - 1);  //number of open banks (except Ref)
                }
                else if (x >= Program.databanks.storage.Count)
                {
                    new Error("bankname() must be called with < " + Program.databanks.storage.Count); return null;
                    //throw new GekkoException();
                }
                else if (x == 1) return new ScalarString(Program.databanks.GetFirst().name);
                else
                {
                    return new ScalarString(Program.databanks.storage[x].name);
                }
            }
            else
            {
                new Error("bankname() only accepts string or val"); return null;
                //throw new GekkoException();
            }

        }

        public static IVariable getendoexo(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            Databank databank = Program.databanks.GetFirst();

            List<string> fixes = new List<string>();
            foreach (string name in new string[] { "endo", "exo" })
            {
                foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
                {
                    if (kvp.Key.StartsWith(name + "_", StringComparison.OrdinalIgnoreCase) && kvp.Key.EndsWith(Globals.freqIndicator + G.ConvertFreq(Program.options.freq), StringComparison.OrdinalIgnoreCase))
                    {
                        //starts with endo_ or exo_ and is of annual type
                        fixes.Add(G.Chop_RemoveFreq(kvp.Key));
                    }
                }
            }
            fixes.Sort();
            List fix = new List(fixes);
            return fix;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x1"></param>
        /// <returns></returns>
        public static IVariable bankfilename(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            return bankfilename(smpl, _t1, _t2, x1, new ScalarString(""));
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static IVariable bankfilename(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            string y1 = x1.ConvertToString();
            string rv = null;
            Databank db = Program.databanks.GetDatabank(y1);
            if (db == null)
            {
                new Error("No open databank has the name '" + y1 + "'");
                //throw new GekkoException();
            }

            string y2 = x2.ConvertToString();
            if (G.Equal(y2, "fullpath"))
            {
                rv = db.FileNameWithPathPretty;
            }
            else
            {
                rv = Program.GetDatabankFilename(db);
            }
            return new ScalarString(rv);
        }

        //!NOTE: do not delete, use for unit tests. Must be lowercase, used in a unit test .gcm file.
        public static IVariable helper_error(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string s = O.ConvertToString(x);
            if (s == Globals.errorHelper)
            {
                new Error("ErrorHelper #" + s);
            }
            return Globals.scalarVal0;
        }

        //!NOTE: do not delete, used for unit tests (maybe not...)
        public static IVariable helper_period(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            //y <2004 2005> = f(<2003 2006>, f(<2002 2007>, x));
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);
            Series ts = new Series(ESeriesType.Light, t1, t2);
            return ts;
        }

        public static IVariable isopen(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            if (G.Equal(s1, Globals.Ref) || G.Equal(s1, Globals.Work) || G.Equal(s1, Globals.Global) || G.Equal(s1, Globals.Local)) return Globals.scalarVal1;

            //Work is typically at pos = 0, but not always so. Ref is always at pos = 1, but we just probe storage[1] for simplicity
            for (int i = 0; i < Program.databanks.storage.Count; i++)
            {
                Databank db = Program.databanks.storage[i];
                if (G.Equal(db.name, s1)) return Globals.scalarVal1;
            }
            return Globals.scalarVal0;
        }

        public static IVariable islibraryloaded(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            foreach (Library lib in Program.libraries.GetLibrariesIncludingLocal())
            {
                if (G.Equal(lib.GetName(), s1)) return Globals.scalarVal1;
            }
            return Globals.scalarVal0;
        }

        public static IVariable concat(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            //same as %s1 + %s2 anyway.
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            return new ScalarString(s1 + s2);
        }

        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        public static IVariable piece(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2, IVariable x3)
        {
            new Error("Rename: please use substring() instead of piece()"); return null;
            //throw new GekkoException();
        }

        public static IVariable substring(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2, IVariable x3)
        {
            string s = null;
            string s1 = O.ConvertToString(x1);
            int i2 = O.ConvertToInt(x2);
            int i3 = O.ConvertToInt(x3);
            if (i3 < 0)
            {
                //AREMOS supports this...
                new Error("piece(): size < 0 not supported.");
                //throw new GekkoException();
            }
            int a, b = 0;
            if (i2 < 0)
            {
                a = s1.Length + i2;
                b = i3;
            }
            else
            {
                a = i2 - 1;
                b = i3;
            }
            if (a < 0 || a > s1.Length - 1 || a + b > s1.Length)
            {
                new Error("piece() function with start " + (a + 1) + " and size " + b + " is not possible on a string of size " + s1.Length + ".");

                //throw new GekkoException();
            }
            s = s1.Substring(a, b);
            return new ScalarString(s);
        }

        public static IVariable laspchain(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable plist, IVariable xlist, IVariable date, IVariable options)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);
            IVariable result = Program.Laspeyres("laspchain", plist, xlist, null, null, date.ConvertToDate(O.GetDateChoices.Strict), options, t1, t2);
            return result;
        }

        public static IVariable laspchain(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable plist, IVariable xlist, IVariable date)
        {
            return laspchain(smpl, _t1, _t2, plist, xlist, date, null);
        }

        //legacy: do not delete yet
        public static IVariable laspchain(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable t1, IVariable t2, IVariable plist, IVariable xlist, IVariable date)
        {
            return laspchain(smpl, t1, t2, plist, xlist, date);
        }

        public static IVariable laspfixed(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable plist, IVariable xlist, IVariable date, IVariable options)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);
            IVariable result = Program.Laspeyres("laspfixed", plist, xlist, null, null, date.ConvertToDate(O.GetDateChoices.Strict), options, t1, t2);
            return result;
        }

        public static IVariable laspfixed(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable plist, IVariable xlist, IVariable date)
        {
            return laspfixed(smpl, _t1, _t2, plist, xlist, date, null);
        }

        //legacy: do not delete yet
        public static IVariable laspfixed(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable t1, IVariable t2, IVariable plist, IVariable xlist, IVariable date)
        {
            return laspfixed(smpl, t1, t2, plist, xlist, date);
        }

        //legacy: do not delete yet
        public static IVariable hpfilter(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable rightSide, IVariable per1, IVariable per2, IVariable ilambda)
        {
            return hpfilter(smpl, per1, per2, rightSide, ilambda, Globals.scalarVal0);
        }

        //legacy: do not delete fyet
        public static IVariable hpfilter(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable rightSide, IVariable per1, IVariable per2, IVariable ilambda, IVariable ilog)
        {
            return hpfilter(smpl, per1, per2, rightSide, ilambda, ilog);
        }

        public static IVariable hpfilter(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable rightSide, IVariable ilambda)
        {
            return hpfilter(smpl, _t1, _t2, rightSide, ilambda, Globals.scalarVal0);
        }

        public static IVariable hpfilter(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable rightSide, IVariable ilambda, IVariable ilog)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);

            int obs = GekkoTime.Observations(t1, t2);

            double lambda = O.ConvertToVal(ilambda);
            double log = O.ConvertToVal(ilog);

            Series rhs = O.ConvertToSeries(rightSide) as Series;

            //Series lhs = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
            Series lhs = new Series(ESeriesType.Light, t1, t2);

            bool isLog = false;
            if (log == 0d)
            {
                isLog = false;
            }
            else if (log == 1d)
            {
                isLog = true;
            }
            else
            {
                new Error("hpfilter() logarithm argument should be 0 or 1");
                //throw new GekkoException();
            }

            if (obs < 2)
            {
                new Error("hpfilter() needs at least two observations to make sense");
                //throw new GekkoException();
            }

            double[] input = new double[obs];

            int counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(t1, t2))
            {
                counter++;
                if (isLog)
                {
                    input[counter] = Math.Log(rhs.GetData(smpl, gt));
                }
                else
                {
                    input[counter] = rhs.GetData(smpl, gt);
                }
            }

            HPfilter hpf = new HPfilter();
            double[] output = hpf.HPFilter(input, lambda);

            counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(t1, t2))
            {
                counter++;
                if (isLog)
                {
                    lhs.SetData(gt, Math.Exp(output[counter]));
                }
                else
                {
                    lhs.SetData(gt, output[counter]);
                }
            }

            return lhs;
        }

        /// <summary>
        /// Helper for timings.
        /// </summary>
        public static IVariable tic(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            Globals.tictoc = DateTime.Now;
            return GekkoNull.gekkoNull;
        }


        /// <summary>
        /// Helper for timings.
        /// </summary>
        public static IVariable toc(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((DateTime.Now - Globals.tictoc).TotalSeconds);
        }

        // ====================== matrix stuff ===============================
        // ====================== matrix stuff ===============================
        // ====================== matrix stuff ===============================

        public static IVariable t(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (x1.Type() == EVariableType.Matrix)
            {
                Matrix x = (Matrix)x1;
                int d1 = x.data.GetLength(0);
                int d2 = x.data.GetLength(1);
                Matrix y = new Matrix(d2, d1);
                for (int i = 0; i < d1; i++)
                {
                    for (int j = 0; j < d2; j++)
                    {
                        y.data[j, i] = x.data[i, j];
                    }
                }
                if (x.colnames != null) y.rownames = new List<string>(x.colnames);  //better to clone
                if (x.rownames != null) y.colnames = new List<string>(x.rownames);  //better to clone
                return y;
            }
            else if (x1.Type() == EVariableType.List)
            {
                List m = x1 as List;
                int max = 0;
                foreach (IVariable iv in m.list)
                {
                    List mm = iv as List;
                    if (mm == null)
                    {
                        new Error("t(): transpose on list only when it contains sublists");
                    }
                    max = Math.Max(max, mm.list.Count);
                }
                List rv = new List();
                for (int i = 0; i < max; i++)
                {
                    List tmp = new List();
                    for (int j = 0; j < m.list.Count; j++)
                    {
                        tmp.list.Add(GekkoNull.gekkoNull);
                    }
                    rv.list.Add(tmp);
                }

                for (int j = 0; j < m.list.Count; j++)
                {
                    for (int i = 0; i < (m.list[j] as List).list.Count; i++)
                    {
                        (rv.list[i] as List).list[j] = (m.list[j] as List).list[i]; //no need to clone, since assignments alway clone anyway
                    }
                }
                return rv;
            }
            else
            {
                new Error("t(): transpose can only be used on list or matrix"); return null;
            }
        }

        //Converts timeseries to matrix
        public static IVariable pack(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] vars)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);
            int offset = 0;
            if (_t1 == null && _t2 == null && ((vars[0].Type() == EVariableType.Date || vars[0].Type() == EVariableType.Val) && (vars[1].Type() == EVariableType.Date || vars[1].Type() == EVariableType.Val)))
            {
                //legacy: do not delete yet
                //seems like two dates first
                offset = 2;
                t1 = O.ConvertToDate(vars[0]);
                t2 = O.ConvertToDate(vars[1]);
            }

            int obs = GekkoTime.Observations(t1, t2);

            List<IVariable> temp = new List<IVariable>();
            for (int i = offset; i < vars.Length; i++)
            {
                temp.Add(vars[i]);
            }

            List<Series> tss = Program.UnfoldAsSeries(smpl, temp);

            int n = tss.Count;
            if (n < 1)
            {
                new Error("Number of items is " + n);
            }

            Matrix m = new Matrix(obs, n);

            //SLACK: could use array-copy...?
            int varcount = -1;
            foreach (Series ts in tss)
            {
                varcount++;
                int counter = -1;
                foreach (GekkoTime gt in new GekkoTimeIterator(t1, t2))
                {
                    counter++;
                    m.data[counter, varcount] = ts.GetData(smpl, gt);
                }
            }

            return m;
        }

        public static IVariable det(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Matrix m = O.ConvertToMatrix(x);
            double d = alglib.rmatrixdet(m.data);
            return new ScalarVal(d);
        }

        public static IVariable rows(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            Matrix m = O.ConvertToMatrix(x1);
            return new ScalarVal(m.data.GetLength(0));
        }

        public static IVariable cols(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            Matrix m = O.ConvertToMatrix(x1);
            return new ScalarVal(m.data.GetLength(1));
        }

        public static IVariable multiply(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            return ElementByElementHelper(EElementByElementType.Times, smpl, x1, x2);
        }

        public static IVariable divide(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            return ElementByElementHelper(EElementByElementType.Divide, smpl, x1, x2);
        }

        public static IVariable zeroes(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            return zeros(smpl, _t1, _t2, x1, x2);
        }


        public static IVariable series(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //series() normal series with current freq
            //series(0) normal series with current freq
            //series('a') normal annual series
            //series(3) 3-dim series with current freq
            //series('a', 3) 3-dim annual series            
            Series ts = helper_seriesAndTimeless("series", x);
            return ts;
        }

        public static IVariable timeless(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //timeless() normal timeless with current freq
            //timeless(200) normal timeless with value 200
            //timeless('a') normal annual timeless            
            //timeless('a', 200) annual timeless with value 200
            Series ts = helper_seriesAndTimeless("timeless", x);
            return ts;
        }

        private static Series helper_seriesAndTimeless(string type, IVariable[] x)
        {
            Series ts = null;
            if (G.Equal(type, "series"))
            {
                if (x.Length == 0)
                {
                    ts = new Series(Program.options.freq, null);
                }
                else if (x.Length == 1)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.ConvertFreq(O.ConvertToString(x[0]));
                        ts = new Series(freq, null);
                    }
                    else if (x[0].Type() == EVariableType.Val)
                    {
                        ts = new Series(Program.options.freq, null);
                        ts.dimensionsStorage = new Multidim();
                        ts.dimensions = O.ConvertToInt(x[0]);
                        ts.type = ESeriesType.ArraySuper;
                        ts.meta = new SeriesMetaInformation();
                    }
                    else
                    {
                        new Error("Expected argument 1 in series() to be value or string");
                        //throw new GekkoException();
                    }
                }
                else if (x.Length == 2)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.ConvertFreq(O.ConvertToString(x[0]));
                        ts = new Series(freq, null);
                        ts.dimensions = O.ConvertToInt(x[1]);
                    }
                    else
                    {
                        new Error("series() with 2 arguments must have string as first argument");
                        //throw new GekkoException();
                    }
                }
                else
                {

                    new Error("series() does not accept > 2 arguments");
                    //throw new GekkoException();
                }

            }
            else
            {
                if (x.Length == 0)
                {
                    ts = new Series(ESeriesType.Timeless, Program.options.freq, null, double.NaN);
                }
                else if (x.Length == 1)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.ConvertFreq(O.ConvertToString(x[0]));
                        ts = new Series(ESeriesType.Timeless, freq, null, double.NaN);
                    }
                    else if (x[0].Type() == EVariableType.Val)
                    {
                        ts = new Series(ESeriesType.Timeless, Program.options.freq, null, x[0].ConvertToVal());
                    }
                    else
                    {
                        new Error("Expected argument 1 in timeless() to be value or string");
                        //throw new GekkoException();
                    }
                }
                else if (x.Length == 2)
                {
                    if (x[0].Type() == EVariableType.String)
                    {
                        //frequency
                        EFreq freq = G.ConvertFreq(O.ConvertToString(x[0]));
                        double d = x[1].ConvertToVal();
                        ts = new Series(ESeriesType.Timeless, freq, null, d);
                    }
                    else
                    {
                        new Error("timeless() with 2 arguments must have string as first argument");
                        //throw new GekkoException();
                    }
                }
                else
                {

                    new Error("series() does not accept > 2 arguments");
                    //throw new GekkoException();
                }
            }

            return ts;
        }

        public static IVariable i(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            int n = O.ConvertToInt(x);
            Matrix m = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                m.data[i, i] = 1d;
            }
            return m;
        }

        public static IVariable diag(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Matrix m = null;
            Matrix xx = O.ConvertToMatrix(x);
            //for a 1x1 matrix, either of the two cases below yields the same!
            if (xx.data.GetLength(1) == 1)
            {
                int n = xx.data.GetLength(0);  //rows
                m = new Matrix(n, n);
                for (int i = 0; i < n; i++)
                {
                    m.data[i, i] = xx.data[i, 0];
                }
            }
            else
            {
                int n = CheckSquare(xx);
                m = new Matrix(n, 1);
                for (int i = 0; i < n; i++)
                {
                    m.data[i, 0] = xx.data[i, i];
                }
            }
            return m;
        }

        public static IVariable trace(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Matrix m = O.ConvertToMatrix(x);
            int n = CheckSquare(m);
            double d = 0d;
            for (int i = 0; i < n; i++)
            {
                d += m.data[i, i];
            }
            return new ScalarVal(d);
        }

        private static int CheckSquare(Matrix m)
        {
            if (m.data.GetLength(0) != m.data.GetLength(1))
            {
                new Error("The matrix is not square (rows = " + m.data.GetLength(0) + ", cols = " + m.data.GetLength(1));
            }
            return m.data.GetLength(0);
        }

        public static IVariable inv(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Matrix m = O.ConvertToMatrix(x);
            int n = CheckSquare(m);
            Matrix clone = m.Clone();
            clone.data = Program.InvertMatrix(clone.data);
            return clone;
        }

        public static IVariable zeros(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            int n1 = O.ConvertToInt(x1);
            int n2 = O.ConvertToInt(x2);
            Matrix m = new Matrix(n1, n2);
            return m;
        }

        public static IVariable truncate(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            //overlapping windows of dates
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);
            GekkoTime dx1 = x1.ConvertToDate(O.GetDateChoices.Strict);
            GekkoTime dx2 = x2.ConvertToDate(O.GetDateChoices.Strict);
            if (dx1.StrictlyLargerThan(dx2))
            {
                new Error("First date must be smaller than or equal to second date");
                //throw new GekkoException();
            }
            GekkoTime dy1 = t1;
            GekkoTime dy2 = t2;
            GekkoTime dt1 = dx1;
            if (dy1.StrictlyLargerThan(dt1)) dt1 = dy1;
            GekkoTime dt2 = dx2;
            if (dy2.StrictlySmallerThan(dt2)) dt2 = dy2;
            List m = new List();
            m.list = new List<IVariable>();
            if (dt1.StrictlyLargerThan(dt2))  //no overlap at all
            {
                m.list.Add(new GekkoNull());
                m.list.Add(new GekkoNull());
            }
            else
            {
                m.list.Add(new ScalarDate(dt1));
                m.list.Add(new ScalarDate(dt2));
            }
            return m;
        }


        /// <summary>
        /// Helper method to deal with two-argument function, where arguments may be scalar or series or even 1x1 matrix.
        /// The function must be given normal and "swapped", for instance (x1, x2) => x1 - x2, followed by (x1, x2) => x2 - x1.
        /// Just swap varnames on rhs only in the swapped function (pure syntactics, no thinking needed). For symmetrical functions swapping yields the same, but must still be stated.
        /// Note: See also the methods O.ConvertToSeriesMaybeConstant() and Program.UnfoldAsSeries().
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="iv1">x1</param>
        /// <param name="iv2">x2</param>
        /// <param name="a">Normal function, like (x1, x2) => x1 - x2</param>
        /// <param name="aSwapped">Swapped function, like (x1, x2) => x2 - x1. Just swap varnames on rhs only.</param>
        /// <returns></returns>
        public static IVariable helper_GeneralFunction(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable iv1, IVariable iv2, Func<double, double, double> a, Func<double, double, double> aSwapped)
        {
            if (iv1.Type() == EVariableType.Series)
            {
                //series, series
                //series, val
                Series iv1_series = iv1 as Series;
                Series x1_series, x2_series; double x2_val;
                iv1_series.PrepareInput(smpl, iv2, out x1_series, out x2_series, out x2_val);
                Series rv_series = null;
                if (x2_series != null) rv_series = Series.ArithmeticsSeriesSeries(smpl, x1_series, x2_series, a);
                else rv_series = Series.ArithmeticsSeriesVal(smpl, x1_series, x2_val, a);
                return rv_series;
            }
            else if ((iv1.Type() == EVariableType.Val || (iv1.Type() == EVariableType.Matrix && ((Matrix)iv1).data.Length == 1)) && iv2.Type() == EVariableType.Series)
            {
                //val, series
                double x1_val = iv1.ConvertToVal();
                Series x2_series = (Series)iv2;
                Series rv_series = Series.ArithmeticsSeriesVal(smpl, x2_series, x1_val, aSwapped);  //Note: swapped!
                return rv_series;
            }
            else
            {
                //val, val
                double x1_val = iv1.ConvertToVal();
                double x2_val = iv2.ConvertToVal();
                double d = a(x1_val, x2_val);
                return new ScalarVal(d);
            }
        }

        public static IVariable min(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] items)
        {
            if (items.Length < 2)
            {
                new Error("Expected 2 or more arguments");
            }

            bool hasDate = false;
            foreach (IVariable item in items)
            {
                if (item.Type() == EVariableType.Date)
                {
                    hasDate = true; break;
                }
            }

            if (hasDate)
            {
                //date comparisions
                GekkoTime gt = GekkoTime.tNull;
                foreach (IVariable item in items)
                {
                    GekkoTime item_date = item.ConvertToDate(O.GetDateChoices.Strict);
                    if (gt.IsNull()) gt = item_date;
                    else
                    {
                        if (item_date.StrictlySmallerThan(gt)) gt = item_date;
                    }
                }
                return new ScalarDate(gt);
            }
            else
            {
                IVariable total = items[0];
                for (int i = 1; i < items.Length; i++)
                {
                    //accumulate
                    total = helper_GeneralFunction(smpl, _t1, _t2, total, items[i], (x1, x2) => Math.Min(x1, x2), (x1, x2) => Math.Min(x2, x1));
                }
                return total;
            }
        }

        public static IVariable max(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] items)
        {
            if (items.Length < 2)
            {
                new Error("Expected 2 or more arguments");
                //throw new GekkoException();
            }

            bool hasDate = false;
            foreach (IVariable item in items)
            {
                if (item.Type() == EVariableType.Date)
                {
                    hasDate = true; break;
                }
            }

            if (hasDate)
            {
                //date comparisions
                GekkoTime gt = GekkoTime.tNull;
                foreach (IVariable item in items)
                {
                    GekkoTime item_date = item.ConvertToDate(O.GetDateChoices.Strict);
                    if (gt.IsNull()) gt = item_date;
                    else
                    {
                        if (item_date.StrictlyLargerThan(gt)) gt = item_date;
                    }
                }
                return new ScalarDate(gt);
            }
            else
            {
                IVariable total = items[0];
                for (int i = 1; i < items.Length; i++)
                {
                    //accumulate
                    total = helper_GeneralFunction(smpl, _t1, _t2, total, items[i], (x1, x2) => Math.Max(x1, x2), (x1, x2) => Math.Max(x2, x1));
                }
                return total;
            }
        }

        public static IVariable sumr(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Rows, ESumType.Sum);
        }

        public static IVariable sumc(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Cols, ESumType.Sum);
        }

        public static IVariable avgr(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Rows, ESumType.Avg);
        }

        public static IVariable avgc(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Cols, ESumType.Avg);
        }

        public static IVariable minr(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Rows, ESumType.Min);
        }

        public static IVariable minc(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Cols, ESumType.Min);
        }

        public static IVariable maxr(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Rows, ESumType.Max);
        }

        public static IVariable null2(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return GekkoNull.gekkoNull;
        }

        public static IVariable isnull(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return Globals.scalarVal1;
            else return Globals.scalarVal0;
        }

        public static IVariable m(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            //alias
            return miss(smpl, _t1, _t2);
        }

        public static IVariable m(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            //alias
            return miss(smpl, _t1, _t2, x1, x2);
        }

        //missing value
        public static IVariable miss(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return Globals.scalarValMissing;
        }

        public static IVariable miss(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            int n1 = O.ConvertToInt(x1);
            int n2 = O.ConvertToInt(x2);
            Matrix m = new Matrix(n1, n2, double.NaN);
            return m;
        }

        public static IVariable ismiss(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return ismiss(smpl, _t1, _t2, x, null);
        }

        public static IVariable ismiss(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable option)
        {
            if (x.Type() == EVariableType.Series)
            {
                Series x_series = x as Series;

                GekkoTime t1 = GekkoTime.tNull;
                GekkoTime t2 = GekkoTime.tNull;

                if (option == null)
                {
                    t1 = x_series.GetRealDataPeriodFirst();
                    t2 = x_series.GetRealDataPeriodLast();
                }
                else
                {
                    string option_string = O.ConvertToString(option);
                    if (!G.Equal(option_string, "all"))
                    {
                        new Error("Expected 'all' option");
                        //throw new GekkoException();
                    }
                    helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);
                }

                Series rv = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
                {
                    bool b = G.isNumericalError(x_series.GetDataSimple(t));
                    if (b) rv.SetData(t, 1d);
                    else rv.SetData(t, 0d);
                }
                return rv;
            }
            else
            {
                bool b = G.isNumericalError(x.ConvertToVal());
                if (b) return Globals.scalarVal1;
                return Globals.scalarVal0;
            }
        }

        public static IVariable maxc(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return SumHelper(smpl, _t1, _t2, x, ESumDim.Cols, ESumType.Max);
        }

        private static IVariable SumHelper(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, ESumDim dim, ESumType type)
        {
            Matrix m = O.ConvertToMatrix(x);
            int rows = m.data.GetLength(0);
            int cols = m.data.GetLength(1);

            int max = -12345;
            if (dim == ESumDim.Rows) max = rows;
            else if (dim == ESumDim.Rows) max = cols;

            Matrix m2 = null;
            if (dim == ESumDim.Cols)
            {
                if (type == ESumType.Min)
                {
                    m2 = new Matrix(1, cols, double.MaxValue);
                }
                else if (type == ESumType.Max)
                {
                    m2 = new Matrix(1, cols, double.MinValue);
                }
                else if (type == ESumType.Sum || type == ESumType.Avg)
                {
                    m2 = new Matrix(1, cols);
                }
                else throw new GekkoException();
            }
            else if (dim == ESumDim.Rows)
            {
                if (type == ESumType.Min)
                {
                    m2 = new Matrix(rows, 1, double.MaxValue);
                }
                else if (type == ESumType.Max)
                {
                    m2 = new Matrix(rows, 1, double.MinValue);
                }
                else if (type == ESumType.Sum || type == ESumType.Avg)
                {
                    m2 = new Matrix(rows, 1);
                }
                else throw new GekkoException();
            }
            else throw new GekkoException();

            if (dim == ESumDim.Cols)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (type == ESumType.Min)
                        {
                            if (m2.data[0, j] > m.data[i, j]) m2.data[0, j] = m.data[i, j];
                        }
                        else if (type == ESumType.Max)
                        {
                            if (m2.data[0, j] < m.data[i, j]) m2.data[0, j] = m.data[i, j];
                        }
                        else if (type == ESumType.Sum || type == ESumType.Avg)
                        {
                            m2.data[0, j] += m.data[i, j];
                        }
                        else throw new GekkoException();
                    }
                }
                if (type == ESumType.Avg)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        m2.data[0, j] = m2.data[0, j] / (double)rows;
                    }
                }
            }
            else if (dim == ESumDim.Rows)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (type == ESumType.Min)
                        {
                            if (m2.data[i, 0] > m.data[i, j]) m2.data[i, 0] = m.data[i, j];
                        }
                        else if (type == ESumType.Max)
                        {
                            if (m2.data[i, 0] < m.data[i, j]) m2.data[i, 0] = m.data[i, j];
                        }
                        else if (type == ESumType.Sum || type == ESumType.Avg)
                        {
                            m2.data[i, 0] += m.data[i, j];
                        }
                        else throw new GekkoException();
                    }
                }
                if (type == ESumType.Avg)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        m2.data[i, 0] = m2.data[i, 0] / (double)cols;
                    }
                }
            }
            else throw new GekkoException();
            return m2;
        }

        public static IVariable ones(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            int n1 = O.ConvertToInt(x1);
            int n2 = O.ConvertToInt(x2);
            Matrix m = new Matrix(n1, n2, 1d);
            return m;
        }

        //Multiplication element by element
        private static IVariable ElementByElementHelper(EElementByElementType type, GekkoSmpl smpl, IVariable x1, IVariable x2)
        {
            Matrix m1 = O.ConvertToMatrix(x1);
            Matrix m2 = O.ConvertToMatrix(x2);
            bool fixedRow1 = false;
            bool fixedCol1 = false;
            if (m1.data.GetLength(0) != m2.data.GetLength(0))
            {
                if (m2.data.GetLength(0) != 1)
                {
                    new Error("" + type.ToString() + "(): There are " + m1.data.GetLength(0) + " and " + m2.data.GetLength(0) + " rows in the matrices");
                    //throw new GekkoException();
                }
                else
                {
                    //for instance div ( 2x3 , 1x3 ) --> use fixed row 1 instead of i
                    fixedRow1 = true;
                }
            }
            if (m1.data.GetLength(1) != m2.data.GetLength(1))
            {
                if (m2.data.GetLength(1) != 1)
                {
                    new Error("" + type.ToString() + "(): There are " + m1.data.GetLength(1) + " and " + m2.data.GetLength(1) + " cols in the matrices");
                    //throw new GekkoException();
                }
                else
                {
                    //for instance div ( 2x3 , 2x1 ) --> use fixed column 1 instead of j
                    fixedCol1 = true;
                }
            }
            Matrix m = new Matrix(m1.data.GetLength(0), m1.data.GetLength(1));
            for (int i = 0; i < m1.data.GetLength(0); i++)
            {
                for (int j = 0; j < m1.data.GetLength(1); j++)
                {
                    int ii = i;
                    int jj = j;
                    if (fixedRow1) ii = 0; //internal arrays are 0-based
                    if (fixedCol1) jj = 0; //internal arrays are 0-based
                    if (type == EElementByElementType.Times)
                    {
                        m.data[i, j] = m1.data[i, j] * m2.data[ii, jj];
                    }
                    else if (type == EElementByElementType.Divide)
                    {
                        m.data[i, j] = m1.data[i, j] / m2.data[ii, jj];
                    }
                }
            }
            return m;
        }

        //legacy, do not delete yet
        public static IVariable unpack(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable per1, IVariable per2)
        {
            return unpack(smpl, per1, per2, x);
        }

        //Converts matrix to timeseries
        public static IVariable unpack(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);

            //from matrix to timeseries            

            int obs = GekkoTime.Observations(t1, t2);

            Matrix m = O.ConvertToMatrix(x);

            if (m.data.GetLength(1) > 1)
            {
                new Error("The matrix provided should have 1 column only");
                //throw new GekkoException();
            }

            if (m.data.GetLength(0) != obs)
            {
                new Error("You provided " + obs + " periods for a matrix with " + m.data.GetLength(0) + " rows");
                //throw new GekkoException();
            }

            return O.CreateTimeSeriesFromMatrix(new GekkoSmpl(t1, t2), m);


        }

        private static int helper_Pack(IVariable[] vars, ref GekkoTime gt1, ref GekkoTime gt2, ref int offset)
        {
            if (vars.Length > 2)  //must be at least 1 variable
            {
                if ((vars[0].Type() == EVariableType.Date || vars[0].Type() == EVariableType.Val) || (vars[1].Type() == EVariableType.Date || vars[1].Type() == EVariableType.Val))
                {
                    //seems like two dates first
                    offset = 2;
                    gt1 = O.ConvertToDate(vars[0]);
                    gt2 = O.ConvertToDate(vars[1]);
                }
            }
            int obs = GekkoTime.Observations(gt1, gt2);
            if (obs < 1)
            {
                new Error("Number of observations is " + obs);
            }
            return obs;
        }

        public static IVariable len(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            return length(smpl, _t1, _t2, x1);
        }

        public static IVariable length(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (x1.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(x1);
                return new ScalarVal(s1.Length);
            }
            else if (x1.Type() == EVariableType.List)
            {
                List l1 = x1 as List;
                return new ScalarVal(l1.list.Count);
            }
            else
            {
                FunctionError("length", x1);
                return null;
            }
        }

        public static IVariable chol(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            return chol(smpl, _t1, _t2, x, new ScalarString("upper"));
        }

        public static IVariable chol(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable type)
        {
            if (x.Type() != EVariableType.Matrix)
            {
                new Error("Chol() only accepts a matrix");
                //throw new GekkoException();
            }

            if (type.Type() != EVariableType.String)
            {
                new Error("Chol() only accepts a string as type");
                //throw new GekkoException();
            }

            double[,] y = ((Matrix)x).data;

            string s = ((ScalarString)type).string2;

            bool upper = true;
            if (G.Equal(s, "upper"))
            {

            }
            else if (G.Equal(s, "lower"))
            {
                upper = false;
            }
            else
            {
                new Error("Type must be 'upper' or 'lower'");
                //throw new GekkoException();
            }

            double[,] z = Cholesky(y, upper);
            Matrix rv = new Matrix();
            rv.data = z;
            return rv;
        }

        public static IVariable rseed(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable seed)
        {
            double seed2 = O.ConvertToVal(seed);
            int i = (int)seed2;
            Globals.random = new Random(i);
            return new ScalarVal(i);
        }

        public static void flush(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            Program.Flush(true);  //removes cached models
            new Writeln("Gekko cache files deleted");
        }

        //Expect time dimension to be last for each "record", like (('a', 'b', '2001'), ('a', 'c', '2002'))
        public static IVariable binary(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            IVariable rv = null;

            List x_list = x as List;
            if (x_list == null) new Error("binary() expects list input");
            int n = x_list.list.Count;
            if (n < 1) new Error("Expected >= 1 elements");
            int cols = -12345;
            for (int i = 0; i < x_list.list.Count; i++)
            {
                IVariable iv = x_list.list[i];
                List y_list = iv as List;
                if (y_list == null) new Error("Sub-element #" + (i + 1) + " is not a list");
                if (cols == -12345) cols = y_list.list.Count;
                else
                {
                    if (cols != y_list.list.Count) new Error("Expected sub-list #" + (i + 1) + " to have " + cols + " elements");
                }
            }

            if (cols < 2) new Error("Expected sub-lists to have at least two elements each");
            int time = cols - 1;  //TODO, choose column (now last)
            IVariable iv2 = ((List)x_list.list[0]).list[time];
            GekkoTime oneTime = GekkoTime.FromIVariableToGekkoTime(iv2);
            EFreq freq = oneTime.freq;

            Series z = new Series(freq, G.GetArraySeriesTempName(freq));  //without this "name", precedents etc. will crash
            z.meta.label = null;
            z.SetArrayTimeseries(cols, true);

            for (int i = 0; i < x_list.list.Count; i++)
            {
                List row = x_list.list[i] as List;
                string[] ss = new string[cols - 1];
                GekkoTime gt = GekkoTime.tNull;
                int count = -1;
                for (int j = 0; j < cols; j++)
                {
                    if (j == time)
                    {
                        gt = GekkoTime.FromIVariableToGekkoTime(row.list[j]);
                        continue;
                    }
                    string s2 = row.list[j].ConvertToString();
                    count++;
                    ss[count] = s2;
                }
                if (gt.IsNull()) new Writeln("Time not found for row #" + (i + 1));
                MultidimItem map = new MultidimItem(ss, z);
                IVariable tsSub = null;
                z.dimensionsStorage.TryGetValue(map, out tsSub);
                if (tsSub == null)
                {
                    tsSub = new Series(freq, null);
                    z.dimensionsStorage.AddIVariableWithOverwrite(map, tsSub);
                }
                Series tsSub_series = (Series)tsSub;
                tsSub_series.SetData(gt, 1d);
            }

            rv = z;
            return rv;
        }        

        public static IVariable rnorm(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable means, IVariable vcov)
        {
            //Maybe it is stupid that we are using stddev versus matrix of covariance

            if (means.Type() == EVariableType.Matrix && vcov.Type() == EVariableType.Matrix)
            {
                //This will be SLOW, because of cholesky decomp for each drawing
                //To circumvent this, we would have to define an object to store to fixed cover matrix

                //Otherwise, we could output a n x sims matrix instead of n x 1.

                Matrix m = (Matrix)vcov;
                int n = m.data.GetLength(0);
                if (m.data.GetLength(1) != n)
                {
                    new Error("Covar matrix is not square");
                    //throw new GekkoException();
                }

                Matrix mean = (Matrix)means;
                if (mean.data.GetLength(0) != n || mean.data.GetLength(1) != 1)
                {
                    new Error("Mean matrix does not correspond to covar matrix");
                    //throw new GekkoException();
                }

                double[,] tmp = Cholesky(m.data, false);
                //after this, #m = t(#tmp)*#tmp

                double[,] randoms = new double[n, 1];
                //https://en.wikipedia.org/wiki/Multivariate_normal_distribution#Drawing_values_from_the_distribution
                for (int i = 0; i < n; i++)
                {
                    double random = O.ConvertToVal(rnorm(smpl, null, null, Globals.scalarVal0, Globals.scalarVal1)); //could be sped up by interfacing to the interior of the method
                    randoms[i, 0] = random;
                }

                Matrix rv = new Matrix();
                rv.data = Program.AddMatrixMatrix(mean.data, Program.MultiplyMatrices(tmp, randoms), n, 1);

                return rv;


            }
            else
            {
                double mean = O.ConvertToVal(means);
                double stdDev = Math.Sqrt(O.ConvertToVal(vcov));
                Random rand = new Random(); //reuse this if you are generating many
                double u1 = Globals.random.NextDouble(); //these are uniform(0,1) random doubles
                double u2 = Globals.random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
                return new ScalarVal(randNormal);
            }
        }

        private static double[,] Cholesky(double[,] m, bool upper)
        {
            if (m.GetLength(0) != m.GetLength(1))
            {
                new Error("Matrix must be square");
                //throw new GekkoException();
            }
            int n = m.GetLength(0);
            double[,] tmp = new double[n, n];

            if (upper)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = i; j < n; j++)
                    {
                        tmp[i, j] = m[i, j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = j; i < n; i++)
                    {
                        tmp[i, j] = m[i, j];
                    }
                }
            }

            bool result = alglib.spdmatrixcholesky(ref tmp, n, upper);

            if (!result)
            {
                new Error("Could not perform Cholesky decomposition");
                //throw new GekkoException();
            }

            return tmp;
        }

        public static IVariable runif(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            double u2 = Globals.random.NextDouble();
            return new ScalarVal(u2);
        }

        public static IVariable contains(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable y, IVariable x)
        {
            string x_string = x.ConvertToString();
            List<IVariable> y_list = y.ConvertToList();
            foreach (IVariable iv in y_list)
            {
                ScalarString temp = iv as ScalarString;
                if (temp != null && G.Equal(temp.string2, x_string)) return Globals.scalarVal1;
            }
            return Globals.scalarVal0;
        }

        public static void setdomains(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable m)
        {
            Series x_series = x as Series;
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                new Error("setdomains(): Expected array-series");
            }
            List m_list = m as List;
            if (m_list == null)
            {
                new Error("setdomains(): Expected list of strings");
            }
            string[] ss = Stringlist.GetListOfStringsFromListOfIvariables(m_list.list.ToArray());
            if (ss == null)
            {
                new Error("setdomains(): Error regarding the list elements. These should be strings.");
            }
            if (x_series.dimensions != ss.Length)
            {
                new Error("setdomains(): The array-timeseries has " + x_series.dimensions + ", whereas the provided list of domain names has " + ss.Length + " dimensions.");
            }
            foreach (string s in ss)
            {
                if (s.StartsWith(Globals.symbolCollection.ToString()) || s == "*")
                {
                    //good
                }
                else new Error("setdomains(): Problem in domain name '" + s + "'. Only the string '*' or strings starting with '#' are allowed.");
            }
            x_series.meta.domains = ss;
        }

        /// <summary>
        /// Tests if a variable is an array-series
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IVariable isarrayseries(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {            
            Series x_series = x as Series;
            if (x_series == null) return Globals.scalarVal0;
            if (x_series.type == ESeriesType.ArraySuper) return Globals.scalarVal1;            
            return Globals.scalarVal0;
        }

        /// <summary>
        /// Tests if a variable is a timeless series
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IVariable istimelessseries(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {            
            Series x_series = x as Series;
            if (x_series == null) return Globals.scalarVal0;
            if (x_series.type == ESeriesType.Timeless) return Globals.scalarVal1;            
            return Globals.scalarVal0;
        }

        /// <summary>
        /// Gets the domain lists that each dimension must comply with (corresponding to GAMS domains)
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List getdomains(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Series x_series = x as Series;
            if (x_series.meta.domains == null) return new List(new List<string>());  //empty
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                new Error("setdomains(): Expected array-series");
                //throw new GekkoException();
            }
            List<string> ss = new List<string>();
            for (int i = 0; i < x_series.meta.domains.Length; i++) ss.Add(x_series.meta.domains[i]);  //cloning for safety
            return new List(ss);
        }

        /// <summary>
        /// If x[a, d] = 1, x[a, e] = 1, x[b, d] = 1, we get (('a', 'b'), ('d', 'e'))
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List getelements(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Series x_series = x as Series;
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                new Error("getelements(): Expected array-series");
            }

            double dimCount2 = 1d;
            string dimCount = null;
            List<List<string>> elements = new List<List<string>>();
            List<string> domains = new List<string>();
            List<MultidimItem> keys = null;
            GekkoDictionary<string, string>[] temp = null;
            keys = x_series.dimensionsStorage.storage.Keys.ToList();
            keys.Sort(Multidim.CompareMultidimItems);
            temp = new GekkoDictionary<string, string>[x_series.dimensions];
            Program.DispHelperArraySeries2(x_series, keys, ref dimCount2, ref dimCount, elements, domains);

            List mm = new List();
            for (int i = 0; i < x_series.dimensions; i++)
            {
                List m = new List();
                foreach (string s in elements[i])
                {
                    m.list.Add(new ScalarString(s));
                }
                mm.Add(m);
            }
            return mm;
        }

        /// <summary>
        /// Info on GAMS gdx type: is it variable or parameter?
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IVariable getfixtype(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            Series x_series = x as Series;
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                new Error("getFixType(): Expected array-series");
            }
            if (x_series.meta != null && x_series.meta.fix == EFixedType.Parameter) return new ScalarString("parameter");
            return new ScalarString("variable");
        }

        /// <summary>
        /// Sets GAMS gdx type (parameter or variable)
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void setfixtype(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable type)
        {
            Series x_series = x as Series;
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                new Error("setFixType(): Expected array-series");
            }
            string type_string = O.ConvertToString(type);
            if (G.Equal(type_string, "parameter"))
            {
                x_series.meta.fix = EFixedType.Parameter;
            }
            else if (G.Equal(type_string, "variable"))
            {
                x_series.meta.fix = EFixedType.Normal;  //what about timeless?? Timeless vars are probably almost always parameters, since we do not solve for them.
            }
            else new Error("setFixType() expects argument 'parameter' or 'variable'");
        }
        
        /// <summary>
        /// Gets info on subseries inside an array-series:
        /// - len/length: the number of subseries
        /// - names: 'x[a, b]', x[a, c]'
        /// - elements: ('a', 'b'), ('a', 'c')
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x"></param>
        /// <param name="option">len, length, names, elements</param>
        /// <returns></returns>
        public static IVariable subseries(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable option)
        {
            Series x_series = x as Series;
            if (x_series == null || x_series.type != ESeriesType.ArraySuper)
            {
                new Error("subseries(): Expected array-series");
            }

            ScalarString ss = option as ScalarString;
            if (ss == null)
            {
                new Error("subseries(): Expected string as option, argument 2");
                //throw new GekkoException();
            }
            string s = ss.ConvertToString();

            double dimCount2 = 1d;
            string dimCount = null;
            List<List<string>> elements = new List<List<string>>();
            List<string> domains = new List<string>();
            List<MultidimItem> keys = null;
            GekkoDictionary<string, string>[] temp = null;
            keys = x_series.dimensionsStorage.storage.Keys.ToList();
            keys.Sort(Multidim.CompareMultidimItems);

            IVariable mm = null;

            bool ok = false;
            if (G.Equal(s, "len") || G.Equal(s, "length"))
            {
                ok = true;
                mm = new ScalarVal(keys.Count);
            }
            else if (G.Equal(s, "dimensions"))
            {
                ok = true;
                mm = new ScalarVal(x_series.dimensions);
            }
            else
            {

                mm = new List();
                foreach (MultidimItem mmi in keys)
                {
                    List m = new List();
                    if (G.Equal(s, "elements"))
                    {
                        ok = true;
                        foreach (string e in mmi.storage)
                        {
                            m.Add(new ScalarString(e));
                        }
                        (mm as List).Add(m);
                    }
                    else if (G.Equal(s, "names"))
                    {
                        ok = true;
                        (mm as List).Add(new ScalarString(mmi.GetName()));
                    }
                }
                if (!ok)
                {
                    new Error("subseries(): Expected string as option to be 'length', 'names' or 'elements'");
                    //throw new GekkoException();
                }
            }

            return mm;
        }

        public static IVariable count(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable y)
        {
            string s = O.ConvertToString(y);
            if (ths.Type() != EVariableType.List)
            {
                FunctionError("count", ths);
            }
            int c = 0;
            foreach (IVariable iv in (ths as List).list)
            {
                ScalarString ss = iv as ScalarString;
                if (ss != null)
                {
                    string s2 = O.ConvertToString(ss);
                    if (G.Equal(s, s2)) c++;
                }
            }
            return new ScalarVal(c);
        }

        public static IVariable remove(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable y)
        {
            string s = O.ConvertToString(y);
            if (ths.Type() != EVariableType.List)
            {
                FunctionError("remove", ths);
            }
            List m = new List();

            foreach (IVariable iv in (ths as List).list)
            {
                ScalarString ss = iv as ScalarString;
                if (ss != null)
                {
                    string s2 = O.ConvertToString(ss);
                    if (G.Equal(s, s2))
                    {
                        //discard                        
                    }
                    else
                    {
                        m.Add(iv);
                    }
                }
            }
            return m;
        }

        public static IVariable pop(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable i)
        {
            int ii = O.ConvertToInt(i);
            return PopHelper(ths, ii);
        }

        public static IVariable pop(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            return PopHelper(ths, -12345);
        }

        private static IVariable PopHelper(IVariable ths, int ii)
        {
            //ii is 1-based
            if (ths.Type() != EVariableType.List)
            {
                FunctionError("pop", ths);
            }
            List m = new List();

            if (ii == -12345)
            {
                ii = (ths as List).Count();
            }

            if (ii < 1 || ii > (ths as List).Count())
            {
                new Error("Cannot pop() at index " + ii);
                //throw new GekkoException();
            }

            int c = -1;  //0-based
            foreach (IVariable iv in (ths as List).list)
            {
                c++;
                if (ii != c + 1) m.Add(iv);
            }
            return m;
        }

        public static IVariable sum(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] items)
        {
            IVariable tsl2 = helper_Sum(smpl.t0, smpl.t3, items, false);
            return tsl2;
        }

        //Legacy, do not delete yet
        public static IVariable sumt(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable d1, IVariable d2)
        {
            return sumt(smpl, d1, d2, x);
        }

        public static IVariable sumt(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);

            GekkoSmpl smplHere = new GekkoSmpl(t1, t2);
            IVariable iv = O.ConvertToSeriesMaybeConstant(smplHere, x);
            double d = 0d;
            foreach (GekkoTime t in new GekkoTimeIterator(smplHere.t1, smplHere.t2))
            {
                d += (iv as Series).GetData(smpl, t);
            }
            return new ScalarVal(d);
        }

        //Legacy, do not delete yet
        public static IVariable avgt(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x, IVariable d1, IVariable d2)
        {
            return avgt(smpl, d1, d2, x);
        }

        public static IVariable avgt(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);

            GekkoSmpl smplHere = new GekkoSmpl(t1, t2);
            IVariable iv = O.ConvertToSeriesMaybeConstant(smplHere, x);
            double d = 0d;
            foreach (GekkoTime t in new GekkoTimeIterator(smplHere.t1, smplHere.t2))
            {
                d += (iv as Series).GetData(smpl, t);
            }
            return new ScalarVal(d / GekkoTime.Observations(smplHere.t1, smplHere.t2));
        }

        private static void helper_TimeOptionField(GekkoSmpl smpl, IVariable _t1, IVariable _t2, out GekkoTime t1, out GekkoTime t2)
        {
            t1 = smpl.t1;
            t2 = smpl.t2;
            if (_t1 != null && _t2 != null)
            {
                t1 = O.ConvertToDate(_t1);
                t2 = O.ConvertToDate(_t2);
                if (t1.StrictlyLargerThan(t2))
                {
                    new Error("Local time period <...> must be increasing");
                }
            }
        }

        public static IVariable avg(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] items)
        {
            IVariable tsl2 = helper_Sum(smpl.t0, smpl.t3, items, true);
            return tsl2;
        }

        private static IVariable helper_Sum(GekkoTime t0, GekkoTime t3, IVariable[] items, bool avg)
        {
            List m = O.FlattenIVariables(new List(items));

            bool hasSeries = false;
            foreach (IVariable item in m.list)
            {
                if (item.Type() == EVariableType.Series)
                {
                    hasSeries = true;
                    break;
                }
            }

            IVariable rv = null;
            if (hasSeries)
            {
                rv = new Series(ESeriesType.Light, t0, t3); //will have small dataarray            
                foreach (GekkoTime t in new GekkoTimeIterator(t0, t3)) (rv as Series).SetData(t, 0d);
            }
            else
            {
                rv = new ScalarVal(0d);
            }

            GekkoSmpl smpl = new GekkoSmpl(t0, t3);
            foreach (IVariable item in m.list)
            {
                IVariable xx = null;
                if (hasSeries) xx = O.ConvertToSeriesMaybeConstant(smpl, item);
                else xx = new ScalarVal(O.ConvertToVal(item));  //will not happen often
                rv = O.Add(smpl, rv, xx);
            }

            if (avg) rv = O.Divide(smpl, rv, new ScalarVal(m.list.Count));

            return rv;
        }


        public static IVariable percentile(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable percent)
        {
            //Mimics Excel's percentile function, see unit tests
            if (G.IsGekkoNull(x1)) return x1;
            GekkoTime t1 = Globals.globalPeriodStart;
            GekkoTime t2 = Globals.globalPeriodEnd;

            Series ts = O.ConvertToSeries(x1) as Series;
            double percent2 = O.ConvertToVal(percent);

            int index1 = -12345;
            int index2 = -12345;
            double[] data = ts.GetDataSequenceBEWARE(out index1, out index2, t1, t2);

            //double[] data2 = new double[index2 - index1 + 1];  //we copy the array, to avoid mishaps if it is altered in the median method (= a little bit slack)
            //for (int i = index1; i <= index2; i++)
            //{
            //    data2[i - index1] = data[i];
            //}

            double median = Program.Percentile(data, percent2);

            ScalarVal z2 = new ScalarVal(median);
            return z2;
        }

        public static IVariable abs(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                rv = new ScalarVal(Math.Abs(d));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[1]); // (x1) => Math.Abs(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Abs(m.data[i, j]);
                    }
                }
                rv = m2;
            }
            else
            {
                new Error("abs(): type " + x1.Type().ToString() + " not supported");
            }
            return rv;
        }


        public static IVariable helper_time(GekkoTime t)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (t.freq == EFreq.A || t.freq == EFreq.U)
            {
                return new ScalarVal(t.super); //Annual: for plots, 0.5 will be added to align them with the higher frequencies
            }
            else if (t.freq == EFreq.Q)
            {
                return new ScalarVal(t.super + 1d / GekkoTimeStuff.numberOfQuarters / 2d + 1d / GekkoTimeStuff.numberOfQuarters * (t.sub - 1));  //2001q1 --> 0.125, 2001q2 --> 0.375, ...
            }
            else if (t.freq == EFreq.M)
            {
                return new ScalarVal(t.super + 1d / GekkoTimeStuff.numberOfMonths / 2d + 1d / GekkoTimeStuff.numberOfMonths * (t.sub - 1));
            }
            else if (t.freq == EFreq.W)
            {
                double numberOfWeeks = ISOWeek.GetWeeksInYear(t.super);
                return new ScalarVal(t.super + 1d / numberOfWeeks / 2d + 1d / numberOfWeeks * (t.sub - 1));
            }
            else if (t.freq == EFreq.D)
            {
                int year = t.super;
                int month = t.sub;
                int day = t.subsub;
                int daysInMonth = GekkoTime.DaysInMonth(year, month);
                double positionInMonth = 1d / daysInMonth / 2d + 1d / daysInMonth * (day - 1d);
                double positionInYear = (month - 1d + positionInMonth) / 12d;
                return new ScalarVal(year + positionInYear);
            }
            throw new GekkoException();
        }

        public static IVariable time(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            if (_t1 != null || _t2 != null) new Error("time() function does not accept local time period");
            Series x = new Series(ESeriesType.Light, smpl.t0, smpl.t2);
            foreach (GekkoTime t in new GekkoTimeIterator(smpl.t0, smpl.t2))
            {
                x.SetData(t, helper_time(t).ConvertToVal());
            }
            return x;
        }

        public static IVariable iif(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable i1, IVariable op, IVariable i2, IVariable o1, IVariable o2)
        {
            if (_t1 != null || _t2 != null) new Error("iif() function does not accept local time period");            
            Series result = new Series(ESeriesType.Light, smpl.t0, smpl.t2);

            if (!IsValOrTimeseries(i1))
            {
                new Error("iif(): arg 1, type " + i1.Type().ToString() + " not supported");
            }
            if (!IsValOrTimeseries(i2))
            {
                new Error("iif(): arg 3, type " + i2.Type().ToString() + " not supported");
            }
            if (!IsValOrTimeseries(o1))
            {
                new Error("iif(): arg 4, type " + o1.Type().ToString() + " not supported");
            }
            if (!IsValOrTimeseries(o2))
            {
                new Error("iif(): arg 5, type " + o2.Type().ToString() + " not supported");
            }

            Series di1 = new Series(ESeriesType.Light, smpl.t0, smpl.t2);
            Series di2 = new Series(ESeriesType.Light, smpl.t0, smpl.t2);
            Series do1 = new Series(ESeriesType.Light, smpl.t0, smpl.t2);
            Series do2 = new Series(ESeriesType.Light, smpl.t0, smpl.t2);

            foreach (GekkoTime gt in new GekkoTimeIterator(smpl.t0, smpl.t2))
            {
                if (i1.Type() == EVariableType.Series) di1.SetData(gt, ((Series)i1).GetData(smpl, gt));
                else di1.SetData(gt, ((ScalarVal)i1).val);

                if (i2.Type() == EVariableType.Series) di2.SetData(gt, ((Series)i2).GetData(smpl, gt));
                else di2.SetData(gt, ((ScalarVal)i2).val);

                if (o1.Type() == EVariableType.Series) do1.SetData(gt, ((Series)o1).GetData(smpl, gt));
                else do1.SetData(gt, ((ScalarVal)o1).val);

                if (o2.Type() == EVariableType.Series) do2.SetData(gt, ((Series)o2).GetData(smpl, gt));
                else do2.SetData(gt, ((ScalarVal)o2).val);
            }

            string x = O.ConvertToString(op).Trim();

            foreach (GekkoTime gt in new GekkoTimeIterator(smpl.t0, smpl.t2))
            {
                if (x == "==")
                {
                    if (G.Equals(di1.GetData(smpl, gt), di2.GetData(smpl, gt)))
                    {
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == "<>")
                {
                    if (!G.Equals(di1.GetData(smpl, gt), di2.GetData(smpl, gt)))
                    {
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == ">")
                {
                    if (di1.GetData(smpl, gt) > di2.GetData(smpl, gt))
                    {
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == ">=")
                {
                    if (di1.GetData(smpl, gt) >= di2.GetData(smpl, gt))
                    {
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == "<")
                {
                    if (di1.GetData(smpl, gt) < di2.GetData(smpl, gt))
                    {
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else if (x == "<=")
                {
                    if (di1.GetData(smpl, gt) <= di2.GetData(smpl, gt))
                    {
                        result.SetData(gt, do1.GetData(smpl, gt));
                    }
                    else
                    {
                        result.SetData(gt, do2.GetData(smpl, gt));
                    }
                }
                else
                {
                    new Error("iif(): Expected operator '==', '<>', '<', '<=', '>' or '>='");
                }
            }
            return result;
        }

        public static IVariable data(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string s = O.ConvertToString(x);
            string[] ss = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List m = new List();
            foreach (string s2 in ss)
            {
                double d = Functions.helper_ValConvertFromString(s2);
                m.Add(new ScalarVal(d));
            }
            return m;
        }

        //!! practical for empty lists and singletons, for instance #m = list(); or #m = list('a');
        //!! for other cases, ('a', 'b') is shorter than list('a', 'b') but yields the same.
        public static IVariable list(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            if (x == null || x.Length == 0)
            {
                //empty list
                List rv = new List();
                rv.list = new List<IVariable>();
                return rv;
            }
            else
            {
                List rv = new List(new List<IVariable>(x));
                return rv;
            }
        }

        public static IVariable map(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            //empty map
            Map rv = new Map();
            rv.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            return rv;
        }

        public static IVariable log(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                rv = new ScalarVal(Math.Log(x1.ConvertToVal()));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[2]); // (x1) => Math.Log(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Log(m.data[i, j]);
                    }
                }
                rv = m2;
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("log(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return rv;
        }

        public static IVariable tanh(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                rv = new ScalarVal(Math.Tanh(x1.ConvertToVal()));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[8]); // (x1) => Math.Tanh(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Tanh(m.data[i, j]);
                    }
                }
                rv = m2;
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("tanh(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return rv;
        }

        public static IVariable exp(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                rv = new ScalarVal(Math.Exp(d));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[3]); // (x1) => Math.Exp(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Exp(m.data[i, j]);
                    }
                }
                rv = m2;
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("exp(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return rv;
        }

        public static IVariable sqrt(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            IVariable rv = null;
            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                rv = new ScalarVal(Math.Sqrt(d));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x1 as Series, Globals.arithmentics1[4]); // (x1) => Math.Sqrt(x1);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                rv = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        ((Matrix)rv).data[i, j] = Math.Sqrt(m.data[i, j]);
                    }
                }
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("sqrt(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return rv;
        }

        private static bool IsValOrTimeseries(IVariable x)
        {
            return x.Type() == EVariableType.Val || x.Type() == EVariableType.Series;
        }

        //same as power()
        public static IVariable pow(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            if (G.IsGekkoNull(x1)) return x1;
            return O.Power(smpl, x1, x2);
        }

        //same as pow()
        public static IVariable power(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            return pow(smpl, _t1, _t2, x1, x2);
        }

        //public static IVariable nothing(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable x1)
        //{            
        //    Program.RevertSmpl(smplOriginal, smpl);
        //    return x1;
        //}

        [MyCustom(Lag = "lag=1")]
        public static IVariable pch(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesLag(smpl, x1 as Series, Globals.arithmentics[10], 1);  //(x, x.1) => (x / x.1 - 1d) * 100d;                
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("pch(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return null;
        }

        public static IVariable observations(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            GekkoTime t1 = O.ConvertToDate(x1);
            GekkoTime t2 = O.ConvertToDate(x2);
            int obs = GekkoTime.Observations(t1, t2); //can be 0 or negative, we allow this                        
            return new ScalarVal(obs);
        }

        public static IVariable seq(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            List<IVariable> mm = new List<IVariable>();
            if (x1.Type() == EVariableType.Val && x2.Type() == EVariableType.Val)
            {
                int i1 = O.ConvertToInt(x1);
                int i2 = O.ConvertToInt(x2);
                //List m = new List()                
                for (int i = i1; i <= i2; i++)
                {
                    ScalarVal d = new ScalarVal(i);
                    mm.Add(d);
                }
            }
            else if (x1.Type() == EVariableType.Date && x2.Type() == EVariableType.Date)
            {
                GekkoTime i1 = O.ConvertToDate(x1);
                GekkoTime i2 = O.ConvertToDate(x2);
                if (GekkoTime.Observations(i1, i2) > 0)
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(i1, i2))  //fails with error if different freqs
                    {
                        mm.Add(new ScalarDate(t));
                    }
                }
            }
            else
            {
                new Error("seq() must be fed with two values or two dates");
                //throw new GekkoException();
            }
            return new List(mm);
        }

        [MyCustom(Lag = "lag=1")]
        public static IVariable dlog(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesLag(smpl, x1 as Series, Globals.arithmentics[11], 1); // (x, x.1) => Math.Log(x / x.1);
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("dlog(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return null;
        }

        [MyCustom(Lag = "lag=1")]
        public static IVariable diff(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            return dif(smplOriginal, smpl, _t1, _t2, x1);
        }


        [MyCustom(Lag = "lag=1")]
        public static IVariable dif(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesLag(smpl, x1 as Series, Globals.arithmentics[2], 1); // (x, x.1) => x - x.1;
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("dif(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return null;
        }

        [MyCustom(Lag = "lag=[4]")]  //remember Program.RevertSmpl(), remember: -1-based, starts at -1, then 0, then 1, ...
        public static IVariable lag(GekkoSmpl2 smpl2, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable ilag)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smpl2, smpl);
            return O.Indexer(smpl2, smpl, O.EIndexerType.IndexerLag, x1, O.Negate(smpl, ilag));
        }

        [MyCustom(Lag = "lag=[4]-1")]  //remember Program.RevertSmpl(), remember: -1-based, starts at -1, then 0, then 1, ...
        public static IVariable movsum(GekkoSmpl2 smpl2, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable ilags)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smpl2, smpl);
            return MovAvgSum(smpl, x1, ilags, false);
        }

        [MyCustom(Lag = "lag=[4]-1")]  //remember Program.RevertSmpl(), remember: -1-based, starts at -1, then 0, then 1, ...
        public static IVariable movavg(GekkoSmpl2 smpl2, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable ilags)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smpl2, smpl);
            return MovAvgSum(smpl, x1, ilags, true);
        }
                
        public static IVariable asbrename(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable name, IVariable file, IVariable decorate)
        {
            string s = null;
            string sname = O.ConvertToString(name);
            string sfile = O.ConvertToString(file);
            string sdecorate = O.ConvertToString(decorate);
            bool d = false;
            if (G.Equal(sdecorate, "yes")) d = true;
            bool debug = false;
            if (G.Equal(sdecorate, "debug")) debug = true;

            if (debug || Globals.asbRecode_dict1 == null)
            {

                string txt = null;
                try
                {
                    FindFileHelper ffh = Program.FindFile(sfile, null, true, false, true, true, null);
                    txt = Program.GetTextFromFileWithWait(ffh.realPathAndFileName);
                }
                catch
                {
                    new Error("Problem finding/reading file: " + sfile);
                }

                List<string> ss = Stringlist.ExtractLinesFromText(txt);

                if (debug)
                {
                    GekkoDictionary<string, int> dict1a = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    GekkoDictionary<string, int> dict2a = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    foreach (string line in ss)
                    {
                        if (line == null || line.Trim() == "") continue;
                        string[] linex = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (linex.Length != 2)
                        {
                            new Writeln("NOT2: " + line);
                            continue;
                        }
                        if (linex[0].StartsWith(".") || linex[0].EndsWith(".") || linex[1].StartsWith(".") || linex[1].EndsWith("."))
                        {
                            new Writeln("DOT: " + line);
                        }
                        if (!dict1a.ContainsKey(linex[0])) dict1a.Add(linex[0], 1);
                        else dict1a[linex[0]] = dict1a[linex[0]] + 1;
                        if (!dict2a.ContainsKey(linex[1])) dict2a.Add(linex[1], 1);
                        else dict2a[linex[1]] = dict2a[linex[1]] + 1;
                    }

                    foreach (KeyValuePair<string, int> kvp in dict1a)
                    {
                        if (kvp.Value > 1) new Writeln(kvp.Value + " --1-- " + kvp.Key);
                    }

                    foreach (KeyValuePair<string, int> kvp in dict2a)
                    {
                        if (kvp.Value > 1) new Writeln(kvp.Value + " --2-- " + kvp.Key);
                    }

                    new Writeln("-------------------------------------------------------");
                    new Writeln("DEBUGGED " + ss.Count + " lines");
                    new Writeln("-------------------------------------------------------");

                    return new ScalarString("");
                }

                int dubletCounter1 = 0;
                int blanksCounter = 0;
                int dotCounter = 0;

                Globals.asbRecode_dict1 = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (string line in ss)
                {
                    if (line == null || line.Trim() == "") continue;
                    string[] linex = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (linex.Length != 2)
                    {
                        blanksCounter++;
                        continue;
                    }
                    try
                    {
                        if (Globals.asbRecode_dict1.ContainsKey(linex[1].Trim()))
                        {
                            dubletCounter1++;
                            continue;
                        }
                        Globals.asbRecode_dict1.Add(linex[1].Trim(), linex[0].Trim());
                    }
                    catch
                    {
                        new Warning("This line could not be put into dictionary: " + line + ", skipping...");
                    }
                }

                //if (dubletCounter1 > 0)
                //{
                //    new Warning("In the recode file, there were " + dubletCounter1 + " dublets on the right-hand side.");
                //}

                //if (blanksCounter > 0)
                //{
                //    new Warning("In the recode file, there were " + blanksCounter + " lines where blanks did not separate exactly two parts/names");
                //}

            }

            string s5 = null;
            try
            {
                s5 = Globals.asbRecode_dict1[sname];
            }
            catch
            {
                new Error("Could not find name '" + sname + "' as right-hand side in " + sfile);
            }

            string s6 = null;
            try
            {
                s6 = Program.DstCodes(s5, d);
            }
            catch
            {
                new Error("Could not understend name '" + s5 + "' as an asb name");
            }            

            ScalarString rv = new ScalarString(s6);
            return rv;
        }

        private static IVariable MovAvgSum(GekkoSmpl smpl, IVariable x, IVariable ilags, bool avg)
        {
            IVariable rv = null;
            int d = O.ConvertToInt(ilags);
            double divide = 1d;
            if (avg) divide = (double)d;
            if (x.Type() == EVariableType.Series)
            {
                Series ts = (Series)x;
                Series z = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                foreach (GekkoTime gt in smpl.Iterate03())
                {
                    double sum = 0d;
                    for (int i = 0; i < d; i++)   //movsum(x, 2) is m + x[-1], so d is always the number of elements.
                    {
                        sum += ts.GetData(smpl, gt.Add(-i));
                    }
                    z.SetData(gt, sum / divide);
                }
                rv = z;
            }
            else if (x.Type() == EVariableType.Val)
            {
                rv = new ScalarVal(d / divide * ((ScalarVal)x).val);
            }
            else
            {
                string s = null;
                if (x.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("movsum(): type " + x.Type().ToString() + " not supported" + s);
            }

            return rv;
        }

        public static IVariable smooth(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //The functions collapse(), interpolate(), rebase() and smooth() are essentially timeless, operating
            //on the full sample. Therefore, _t1 and _t2 are ignored.

            Series input = null;
            Series overlay = null;
            string method = "linear";  //default

            if (x.Length == 0)
            {
                new Error("smooth(): did not expect 0 arguments.");
            }

            input = O.ConvertToSeries(x[0]) as Series;

            GekkoSmpl smplHere = new GekkoSmpl(input.GetRealDataPeriodFirst(), input.GetRealDataPeriodLast());

            if (x.Length == 1)
            {
                method = Program.options.smooth_method;  //default is "linear"
            }
            else if (x.Length == 2)
            {
                if (x[1].Type() == EVariableType.String)
                {
                    method = x[1].ConvertToString();
                }
                else
                {
                    //overlay, here we expect a series, scalar or 1x1 matrix
                    method = "overlay";
                    overlay = O.ConvertToSeriesMaybeConstant(smplHere, x[1]);
                }
            }
            else if (x.Length == 3)
            {
                if (x[1].Type() == EVariableType.String)
                {
                    method = x[1].ConvertToString();
                    overlay = O.ConvertToSeriesMaybeConstant(smplHere, x[2]);
                    if (!G.Equal(method, "overlay")) new Error("Expected 'overlay' as 2. argument if there are 3 arguments.");
                }
                else
                {
                    new Error("Expected 'overlay' as 2. argument if there are 3 arguments.");
                }
            }
            else
            {
                new Error("smooth(): did not expect > 3 arguments.");
            }

            ESmoothTypes method2 = ESmoothTypes.Linear;  //spline is the default in AREMOS, but linear is simpler and we are certain that the average of the filled in holes is meaningful
            if (G.Equal(method, "geometric")) method2 = ESmoothTypes.Geometric;
            else if (G.Equal(method, "linear")) method2 = ESmoothTypes.Linear;
            else if (G.Equal(method, "spline")) method2 = ESmoothTypes.Spline;
            else if (G.Equal(method, "repeat")) method2 = ESmoothTypes.Repeat;
            else if (G.Equal(method, "overlay")) method2 = ESmoothTypes.Overlay;
            else
            {
                new Error("Expected smooth() method to be 'linear', 'geometric', 'spline', 'repeat' or 'overlay' -- not '" + method + "'"); ;
            }

            Series lhs = new Series(input.freq, null);  //could this be light?
            Program.SmoothHelper(lhs, input, method2, overlay);
            return lhs;
        }


        public static IVariable rebase(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //The functions collapse(), interpolate(), rebase() and smooth() are essentially timeless, operating
            //on the full sample. Therefore, _t1 and _t2 are ignored.

            // x, i         --> rebase(x, 2020)
            // x, i, v      --> rebase(x, 2020, 1)
            // Does not support rebasing over a window of periods --> use REBASE command            

            //hmmm, what if we are doing a PLOT <2010 2030> pch(rebase(x, 2020)) --> we will get a missing in 2010...?

            if (x.Length < 2) new Error("Expected rebase() with >= 2 arguments.");
            if (x.Length > 3) new Error("Expected rebase() with <= 3 arguments.");

            IVariable iv = x[0];
            if (G.IsGekkoNull(iv)) return iv;

            GekkoTime gti = O.ConvertToDate(x[1], O.GetDateChoices.Strict);
            double indexValue = 100d;

            if (x.Length == 3) indexValue = O.ConvertToVal(x[2]);

            Series ts; double sum; double n;
            Program.RebaseHelper1(gti, gti, iv, out ts, out sum, out n);

            Series tsNew = ts.DeepClone(null, null) as Series;

            Program.RebaseHelper2(tsNew, sum, n, indexValue);

            return tsNew;
        }

        public static IVariable collapse(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //The functions collapse(), interpolate(), rebase() and smooth() are essentially timeless, operating
            //on the full sample. Therefore, _t1 and _t2 are ignored.

            // collapse(x)
            // collapse(x, 'avg')
            // collapse(x, 'q')
            // collapse(x, 'q', 'avg')            

            //The two last are only used when collapsing from m --> a.
            //Otherwise, we collapse per default from d --> m, m --> q, q --> a.            

            //hmmm, what if we are doing a PLOT <2010 2030> pch(rebase(x, 2020)) --> we will get a missing in 2010...?

            if (x.Length < 1) new Error("Expected collapse() with >= 1 arguments.");
            if (x.Length > 3) new Error("Expected collapse() with <= 3 arguments.");

            IVariable iv = x[0];
            if (G.IsGekkoNull(iv)) return iv;
            Series ts = iv as Series;
            if (ts == null) new Error("Expected a timeseries as first argument, got " + G.GetTypeString(iv) + " type");

            string missing = null;
            string freq_destination = null;
            string method = Program.options.collapse_method.ToLower(); //starts as "total"

            if (x.Length > 1)
            {
                string s = O.ConvertToString(x[1]);
                if (s.ToLower().StartsWith("total") || s.ToLower().StartsWith("avg") || s.ToLower().StartsWith("first") || s.ToLower().StartsWith("last") || s.ToLower().StartsWith("strict") || s.ToLower().StartsWith("flex"))
                {
                    // collapse(x!d, 'total') or collapse(x!d, 'avg-strict') or collapse(x!d, 'strict'), etc.
                    // Not allowed to do collapse(x!d, 'total', 'a')

                    string[] ss = s.Split('-');
                    if (ss.Length == 2)
                    {
                        method = ss[0];
                        missing = ss[1];
                    }
                    else if (s.ToLower().StartsWith("strict") || s.ToLower().StartsWith("flex"))
                    {
                        missing = s;
                    }
                    else
                    {
                        method = s;
                    }
                    if (x.Length >= 3) new Error("If you state a method as second argument, you cannot use further arguments. Alternatively, indicate the destination frequency first, and then the method.");
                }
                else
                {
                    // collapse(x!d, 'a') or collapse(x!d, 'q'), etc.
                    freq_destination = s;
                    if (x.Length == 3) method = O.ConvertToString(x[2]);
                    if (x.Length > 3) new Error("Collapse() function has too many arguments.");
                }
            }

            if (freq_destination == null)
            {
                //state defaults                
                if (ts.freq == EFreq.Q) freq_destination = "a";
                else if (ts.freq == EFreq.M) freq_destination = "q";
                else if (ts.freq == EFreq.W) freq_destination = "m";  //will use fractions...
                else if (ts.freq == EFreq.D) freq_destination = "m";  //not W!
                else
                {
                    new Error("The frequency of the input timeseries should be D, M or Q for collapse().");
                }
            }

            Series tsNew = new Series(G.ConvertFreq(freq_destination, false), null);  //the name will not be used for anything --> the series is temporary

            CollapseHelper helper = new CollapseHelper();
            if (method != null) helper.method = method;
            if (missing != null) helper.collapse_missing = missing;
            Program.CollapseHelper(tsNew, ts, helper);

            return tsNew;
        }

        public static IVariable splice(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //The functions collapse(), interpolate(), rebase(), smooth() and splice are essentially timeless, operating
            //on the full sample. Therefore, _t1 and _t2 are ignored.

            // splice(x1, 2010, x2)
            // splice('first', x1, 2010, x2)
            // splice('2', x1, 2010, x2)
            // splice('first-2', x1, 2010, x2)            

            //Corresponds to SPLICE, but truncates the returned series to the time period  

            string opt_type = null;
            string opt_first = null;
            string opt_last = null;
            double opt_n = double.NaN;

            if (x.Length < 1) new Error("Expected splice() with >= 1 arguments.");

            int hasStringStart = 0;
            if (x[0].Type() == EVariableType.String)
            {
                hasStringStart = 1;
                string temp = x[0].ConvertToString();
                string[] ss = temp.Split('-');
                if (ss.Length > 2) new Error("splice(): You can onlyt use one '-'");
                if (ss.Length == 1)
                {
                    if (G.Equal(temp, "rel1"))
                    {
                        opt_type = "rel1";
                    }
                    else if (G.Equal(temp, "rel2"))
                    {
                        opt_type = "rel2";
                    }
                    else if (G.Equal(temp, "rel3"))
                    {
                        opt_type = "rel3";
                    }
                    else if (G.Equal(temp, "abs"))
                    {
                        opt_type = "abs";
                    }
                    else if (G.Equal(temp, "first"))
                    {
                        opt_first = "yes";
                    }
                    else if (G.Equal(temp, "last"))
                    {
                        opt_last = "yes";
                    }
                    else if (G.IntParse(temp) != -12345)
                    {
                        opt_n = G.IntParse(temp);
                    }
                    else new Error("In splice(), could not understand first parameter '" + temp + "'");
                }
                else
                {
                    if (G.Equal(ss[0], "rel1"))
                    {
                        opt_type = "rel1";
                    }
                    else if (G.Equal(ss[0], "rel2"))
                    {
                        opt_type = "rel2";
                    }
                    else if (G.Equal(ss[0], "rel3"))
                    {
                        opt_type = "rel3";
                    }
                    else if (G.Equal(ss[0], "abs"))
                    {
                        opt_type = "abs";
                    }
                    else new Error("In splice(), could not understand first parameter '" + temp + "'");

                    if (G.Equal(ss[1], "first"))
                    {
                        opt_first = "yes";
                    }
                    else if (G.Equal(ss[1], "last"))
                    {
                        opt_last = "yes";
                    }
                    else if (G.IntParse(ss[1]) != -12345)
                    {
                        opt_n = G.IntParse(ss[1]);
                    }
                    else new Error("In splice(), could not understand first parameter '" + temp + "'");
                }
            }

            List rhs = new List();
            for (int i = hasStringStart; i < x.Length; i++)
            {
                rhs.Add(x[i]);
            }

            Series tsNew = O.Splice.SpliceHelper(null, rhs, opt_type, opt_first, opt_last, opt_n, true);
            return tsNew;
        }

        /// <summary>
        /// Work out the date for Easter Sunday for specified year
        /// </summary>
        /// <param name="year">The year as an integer</param>
        /// <returns>Returns a datetime of Easter Sunday.</returns>
        public static IVariable getspecialday(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable date, IVariable name)
        {
            GekkoTime t = O.ConvertToDate(date);
            if (t.freq != EFreq.A) new Error("getSpecialDay() function was called with a date of " + t.freq.Pretty() + " frequency. Only Annual dates or integers can be used.");
            GekkoTime t2 = GekkoTime.GetSpecialDay(t.super, O.ConvertToString(name));
            if (t2.IsNull()) return GekkoNull.gekkoNull;
            return new ScalarDate(t2);
        }

        public static IVariable interpolate(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] x)
        {
            //The functions collapse(), interpolate(), rebase() and smooth() are essentially timeless, operating
            //on the full sample. Therefore, _t1 and _t2 are ignored.

            // 'm' is destination frequency, y is indicator for 'dentona1'
            // --------------------------------------------------------------
            // interpolate(x)
            // interpolate(x, 'repeat')
            // interpolate(x, 'm')
            // interpolate(x, 'm', 'repeat')
            // -  -  -  -  -  -  -  -  -  -  -  -
            // interpolate(x, y)
            // interpolate(x, y, 'repeat')
            // interpolate(x, y, 'm')
            // interpolate(x, y, 'm', 'repeat')
            // --------------------------------------------------------------

            //either a --> q, a --> m, or q --> m.
            //if freq not stated: a --> q, q --> m. Else fail.                        

            if (x.Length < 1) new Error("Expected interpolate() with >= 1 arguments.");
            if (x.Length > 4) new Error("Expected interpolate() with <= 4 arguments.");

            IVariable iv = x[0];
            if (G.IsGekkoNull(iv)) return iv;
            Series ts = iv as Series;
            if (ts == null) new Error("Expected a timeseries as first argument, got " + G.GetTypeString(iv) + " type");

            string freq_destination = null;
            string method = Program.options.interpolate_method;  // "repeat" is default

            Series tsIndicator = null;
            int offset = 0;  //0 or 1, 1 if we use indicator series   
            if (x.Length > 1 && x[1].Type() == EVariableType.Series)
            {                
                tsIndicator = x[1] as Series;
                offset = 1;
            }
            else if (x.Length > 1 && x[1].Type() == EVariableType.Val)
            {
                //TODO: allow it to be a constant (value)?
                //TODO: allow it to be a constant (value)?
                //TODO: allow it to be a constant (value)?
                //TODO: allow it to be a constant (value)?
                //TODO: allow it to be a constant (value)?
                tsIndicator = x[1] as Series;
                offset = 1;
            }            

            if (x.Length > 1 + offset)
            {
                string s = O.ConvertToString(x[1 + offset]);
                if (G.Equal(s, "repeat") || G.Equal(s, "prorate") || G.Equal(s, "dentona1"))
                {
                    method = s;
                    if (x.Length == 3 + offset) new Error("If you state a method as argument #(" + (3 + offset - 1) + "), you cannot use further arguments. Alternatively, indicate the frequency first, and then the method.");
                }
                else
                {
                    freq_destination = s;
                    if (x.Length == 3 + offset) method = O.ConvertToString(x[2 + offset]);
                }
            }

            if (freq_destination == null)
            {
                //state defaults                
                if (ts.freq == EFreq.A) freq_destination = "q";
                else if (ts.freq == EFreq.Q) freq_destination = "m";
                else if (ts.freq == EFreq.M) freq_destination = "d";
                else if (ts.freq == EFreq.W) freq_destination = "d";
                else if (ts.freq == EFreq.D) new Error("You cannot input a daily series for interpolate().");
            }

            Series tsNew = new Series(G.ConvertFreq(freq_destination, false), null);  //the name will not be used for anything --> the series is temporary

            if (method == null) method = "repeat";  //never happens...?
            Program.InterpolateHelper(tsNew, ts, tsIndicator, method);

            return tsNew;
        }


        [MyCustom(Lag = "lag=13")]  //12+1, good enough for months, overkill for quarters but never mind
        public static IVariable pchy(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                Series x1_series = x1 as Series;
                return Series.ArithmeticsSeriesLag(smpl, x1_series, Globals.arithmentics[10], SeriesLagYNumber(x1_series));  //(x, x.1) => (x / x.1 - 1d) * 100d;                
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("pchy(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return null;
        }

        [MyCustom(Lag = "lag=13")]  //12+1, good enough for months, overkill for quarters but never mind
        public static IVariable diffy(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            return dify(smplOriginal, smpl, _t1, _t2, x1);
        }

        [MyCustom(Lag = "lag=13")]  //12+1, good enough for months, overkill for quarters but never mind
        public static IVariable dify(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                Series x1_series = x1 as Series;
                return Series.ArithmeticsSeriesLag(smpl, x1_series, Globals.arithmentics[2], SeriesLagYNumber(x1_series));  // (x1, x2) => x1 - x2;
            }
            else
            {
                new Error("dify() function only valid for time series arguments");
                //throw new GekkoException();
            }
            return null;
        }

        [MyCustom(Lag = "lag=13")]  //12+1, good enough for months, overkill for quarters but never mind
        public static IVariable dlogy(GekkoSmpl2 smplOriginal, GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            Program.RevertSmpl(smplOriginal, smpl);
            if (x1.Type() == EVariableType.Series)
            {
                Series x1_series = x1 as Series;
                return Series.ArithmeticsSeriesLag(smpl, x1_series, Globals.arithmentics[11], SeriesLagYNumber(x1_series));  // Math.Log(x1 / x2);
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("dlogy(): type " + x1.Type().ToString() + " not supported" + s);
            }
            return null;
        }

        private static int SeriesLagYNumber(Series x1_series)
        {
            int i = 1;
            if (x1_series.freq == EFreq.A || x1_series.freq == EFreq.U) i = Globals.freqASubperiods;
            else if (x1_series.freq == EFreq.Q) i = Globals.freqQSubperiods;
            else if (x1_series.freq == EFreq.M) i = Globals.freqMSubperiods;
            return i;
        }

        //Used as function, but also for string interplation, like TELL 'Number is {%v}'.
        //See also #83490837432, these should be merged/fusioned
        public static IVariable format(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            string format3 = O.ConvertToString(x2);
            string[] two = format3.Split('=');
            string format2 = two[0];
            string culture = null;
            if (two.Length > 1) culture = two[1];

            try
            {
                if (x1.Type() == EVariableType.Val)
                {
                    double d = O.ConvertToVal(x1); //#875324397                
                    string x = Program.NumberFormat(d, format2, culture);
                    return new ScalarString(x);
                }
                else if (x1.Type() == EVariableType.Date)
                {
                    string s = G.FromDateToString((x1 as ScalarDate).date);
                    string x = Program.StringFormat(s, format2);
                    return new ScalarString(x);
                }
                else if (x1.Type() == EVariableType.String)
                {
                    string s = O.ConvertToString(x1);
                    string x = Program.StringFormat(s, format2);
                    return new ScalarString(x);
                }
                else
                {
                    //new Error("format() expects val, date or string type");
                    throw new GekkoException();
                }
            }
            catch (Exception e)
            {
                new Error("Format '" + format2 + "' failed"); return null;
                //throw new GekkoException();
            }
        }

        public static IVariable round(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable round)
        {
            if (G.IsGekkoNull(x1)) return x1;
            double d2 = O.ConvertToVal(round);
            int aaa1 = 0;
            if (!G.ConvertToInt(out aaa1, d2))
            {
                new Error("Could not convert decimals variable to integer");
            }
            int decimals = aaa1;
            if (decimals < 0)
            {
                new Error("number of decimals in round() must be positive");
            }

            if (x1.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x1);
                return new ScalarVal(Math.Round(d, decimals, MidpointRounding.AwayFromZero));
            }
            else if (x1.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeriesVal(smpl, x1 as Series, decimals, Globals.arithmentics[12]);  //(x1, x2) => Math.Round(x1, (int)x2);                
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Round(m.data[i, j], decimals, MidpointRounding.AwayFromZero);
                    }
                }
                return m2;
            }
            else
            {
                string s = null;
                if (x1.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("round(): type " + x1.Type().ToString() + " not supported" + s); return null;
            }
        }

        public static IVariable mod(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable iv1, IVariable iv2)
        {
            return helper_GeneralFunction(smpl, _t1, _t2, iv1, iv2, (x1, x2) => x1 % x2, (x1, x2) => x2 % x1);
        }


        public static IVariable int2(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            if (G.IsGekkoNull(x)) return x;

            if (x.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x);
                return new ScalarVal(Math.Truncate(d));
            }
            else if (x.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x as Series, Globals.arithmentics1[5]); //(x) => Math.Truncate(x1);                
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Truncate(m.data[i, j]);
                    }
                }
                return m2;
            }
            else
            {
                string s = null;
                if (x.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("dlogy(): type " + x.Type().ToString() + " not supported" + s); return null;
            }
        }

        public static IVariable floor(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            if (G.IsGekkoNull(x)) return x;

            if (x.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x);
                return new ScalarVal(Math.Floor(d));
            }
            else if (x.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x as Series, Globals.arithmentics1[6]); //(x) => Math.Floor(x1);                
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Floor(m.data[i, j]);
                    }
                }
                return m2;
            }
            else
            {
                string s = null;
                if (x.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("floor(): type " + x.Type().ToString() + " not supported" + s); return null;
            }
        }

        public static IVariable ceiling(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            if (G.IsGekkoNull(x)) return x;

            if (x.Type() == EVariableType.Val)
            {
                double d = O.ConvertToVal(x);
                return new ScalarVal(Math.Ceiling(d));
            }
            else if (x.Type() == EVariableType.Series)
            {
                return Series.ArithmeticsSeries(smpl, x as Series, Globals.arithmentics1[7]); //(x) => Math.Ceiling(x1);                
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.ConvertToMatrix(x);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Ceiling(m.data[i, j]);
                    }
                }
                return m2;
            }
            else
            {
                string s = null;
                if (x.Type() == EVariableType.String) s += ". " + Globals.stringConversionNote;
                new Error("ceiling(): type " + x.Type().ToString() + " not supported" + s); return null;
            }
        }

        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        public static IVariable search(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            new Error("search() is now index() in Gekko 3.0"); return null;
            //throw new GekkoException();
        }

        public static IVariable index(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO also for lists
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO

            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int i = s1.IndexOf(s2, StringComparison.CurrentCultureIgnoreCase);
            return new ScalarVal(i + 1);  //return 0 if not found
        }

        public static IVariable startswith(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int i = 0;
            if (s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase)) i = 1;
            return new ScalarVal(i);
        }

        public static IVariable endswith(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int i = 0;
            if (s1.EndsWith(s2, StringComparison.OrdinalIgnoreCase)) i = 1;
            return new ScalarVal(i);
        }

        public static IVariable yesno(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            double d = O.ConvertToVal(x1);
            ScalarString rv = null;
            if (d == 0d)
            {
                rv = new ScalarString("no");
            }
            else if (d == 1d)
            {
                rv = new ScalarString("yes");
            }
            else
            {
                new Error("yesno(): expected input 0 or 1, got " + d);
            }
            return rv;
        }

        public static IVariable readfile(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(O.ReplaceSlash(x1));
            FindFileHelper ffh = Program.FindFile(s1, null, true, true, false, true, smpl.p);
            if (ffh.realPathAndFileName == null) new Error("Could not find file: " + s1);
            string txt = Program.GetTextFromFileWithWait(ffh.realPathAndFileName);
            return new ScalarString(txt);
        }

        public static IVariable readscreen(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            string txt = CrossThreadStuff.GetTextBoxMainTabUpper();
            return new ScalarString(txt);
        }

        public static IVariable isutf8file(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(O.ReplaceSlash(x1));
            FindFileHelper ffh = Program.FindFile(s1, null, true, true, false, true, smpl.p);
            if (ffh.realPathAndFileName == null) new Error("Could not find file: " + s1);
            bool b = Utf8Checker.IsUtf8(ffh.realPathAndFileName);
            if (b) return Globals.scalarVal1;
            else return Globals.scalarVal0;
        }

        public static IVariable existfile(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(O.ReplaceSlash(x1));
            FindFileHelper ffh = Program.FindFile(s1, null, true, true, false, true, smpl.p);
            if (ffh.realPathAndFileName == null) return Globals.scalarVal0;
            return Globals.scalarVal1;
        }

        public static void writefile(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable file1, IVariable x1)
        {
            Program.WriteFile(file1, O.ReplaceSlash(x1));
        }

        public static IVariable nl(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(G.NL2.ToString());
        }

        public static IVariable split(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2, IVariable removeEmpty, IVariable strip)
        {
            string s1 = O.ConvertToString(x1);
            string s2 = O.ConvertToString(x2);
            int removeEmpty_int = O.ConvertToInt(removeEmpty);
            int strip_int = O.ConvertToInt(strip);
            string[] ss = null;
            if (removeEmpty_int == 1)
            {
                ss = s1.Split(new string[] { s2 }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (removeEmpty_int == 0)
            {
                ss = s1.Split(new string[] { s2 }, StringSplitOptions.None);
            }
            else
            {
                new Error("removeempty must be yes or no");
            }

            if (strip_int == 1)
            {
                for (int i = 0; i < ss.Length; i++)
                {
                    ss[i] = ss[i].Trim();
                }
            }
            else if (strip_int == 0)
            {
                //do nothing
            }
            else
            {
                new Error("strip must be yes or no");
                //throw new GekkoException();
            }

            List m = null;
            if (removeEmpty_int == 1)
            {
                List<string> ss2 = new List<string>();
                for (int i = 0; i < ss.Length; i++)
                {
                    if (ss[i] == "") continue;
                    ss2.Add(ss[i]);
                }
                m = Stringlist.CreateListFromStrings(ss2.ToArray());
            }
            else
            {
                m = Stringlist.CreateListFromStrings(ss);
            }

            return m;
        }

        public static IVariable split(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2, IVariable x3)
        {
            return split(smpl, _t1, _t2, x1, x2, x3, new ScalarVal(1));  //stripblanks = yes
        }

        public static IVariable split(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            return split(smpl, _t1, _t2, x1, x2, new ScalarVal(1), new ScalarVal(1));  //removeempty = yes, stripblanks = yes
        }

        public static IVariable type(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string t = null;
            if (x.Type() == EVariableType.Val)
            {
                t = "val";
            }
            else if (x.Type() == EVariableType.Date)
            {
                t = "date";
            }
            else if (x.Type() == EVariableType.String)
            {
                t = "string";
            }
            else if (x.Type() == EVariableType.Series)
            {
                t = "series";
            }
            else if (x.Type() == EVariableType.List)
            {
                t = "list";
            }
            else if (x.Type() == EVariableType.Map)
            {
                t = "map";
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                t = "matrix";
            }
            else if (x.Type() == EVariableType.Null)
            {
                t = "null";
            }
            else
            {
                new Error("Unknown type");
                //throw new GekkoException();
            }
            ScalarString rv = new ScalarString(t);
            return rv;
        }


        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        //OBSOLETE
        public static IVariable trim(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            new Error("trim() is now strip() in Gekko 3.0."); return null;
            //throw new GekkoException();
        }

        public static IVariable strip(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            return new ScalarString(s1.Trim());
        }

        public static IVariable stripstart(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            return new ScalarString(s1.TrimStart());
        }

        public static IVariable stripend(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            string s1 = O.ConvertToString(x1);
            return new ScalarString(s1.TrimEnd());
        }

        public static IVariable upper(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            //does not exist in AREMOS

            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                string s4 = s1.ToUpper();
                return new ScalarString(s4);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {
                    if (iv.Type() == EVariableType.String)
                    {
                        tmp.Add(new ScalarString(iv.ConvertToString().ToUpper()));
                    }
                    else
                    {
                        tmp.Add(iv);
                    }
                }
                return tmp;
            }
            else
            {
                FunctionError("upper", ths);  //throws exception
                return null;
            }
        }

        public static IVariable upperfirst(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            //Only first char is "uppered". 

            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                if (s1.Length > 1) return new ScalarString(char.ToUpper(s1[0]) + s1.Substring(1));
                return new ScalarString(s1.ToUpper());
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {
                    if (iv.Type() == EVariableType.String)
                    {
                        string s1 = O.ConvertToString(iv);
                        if (s1.Length > 1) tmp.Add(new ScalarString(char.ToUpper(s1[0]) + s1.Substring(1)));
                        else tmp.Add(new ScalarString(s1.ToUpper()));
                    }
                    else
                    {
                        tmp.Add(iv);
                    }
                }
                return tmp;
            }
            else
            {
                FunctionError("upper", ths);  //throws exception
                return null;
            }
        }

        public static IVariable isupper(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                bool b = helper_IsUpper(s1);
                if (b) return new ScalarVal(1d);
                else return new ScalarVal(0d);
            }
            else
            {
                FunctionError("isUpper", ths);  //throws exception
                return null;
            }
        }

        public static IVariable islower(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                bool b = helper_IsLower(s1);
                if (b) return new ScalarVal(1d);
                else return new ScalarVal(0d);
            }
            else
            {
                FunctionError("isLower", ths);  //throws exception
                return null;
            }
        }

        public static IVariable isalpha(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                bool b = helper_IsLetter(s1);
                if (b) return new ScalarVal(1d);
                else return new ScalarVal(0d);
            }
            else
            {
                FunctionError("isAlpha", ths);  //throws exception
                return null;
            }
        }

        public static IVariable isnumeric(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                bool b = helper_IsDigit(s1);
                if (b) return new ScalarVal(1d);
                else return new ScalarVal(0d);
            }
            else
            {
                FunctionError("isNumeric", ths);  //throws exception
                return null;
            }
        }

        public static bool helper_IsUpper(string value)
        {
            // Consider string to be uppercase if it has no lowercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsLower(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool helper_IsLower(string value)
        {
            // Consider string to be lowercase if it has no uppercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool helper_IsLetter(string value)
        {
            // Consider string to be lowercase if it has no uppercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsLetter(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool helper_IsDigit(string value)
        {
            // Consider string to be lowercase if it has no uppercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsDigit(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static IVariable lower(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths)
        {
            //does not exist in AREMOS

            if (ths.Type() == EVariableType.String)
            {
                string s1 = O.ConvertToString(ths);
                string s4 = s1.ToLower();
                return new ScalarString(s4);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {
                    if (iv.Type() == EVariableType.String)
                    {
                        tmp.Add(new ScalarString(iv.ConvertToString().ToLower()));
                    }
                    else
                    {
                        tmp.Add(iv);
                    }
                }
                return tmp;
            }
            else
            {
                FunctionError("upper", ths);  //throws exception
                return null;
            }
        }

        public static IVariable sort(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2 = null)
        {
            List<IVariable> tmp = O.ConvertToList(x1);
            List<string> xx = new List<string>(Stringlist.GetListOfStringsFromListOfIvariables(tmp.ToArray()));

            string sort = null;
            if (x2 != null)
            {
                sort = O.ConvertToString(x2);
                if (!G.Equal(sort, "natural"))
                {
                    new Error("Expected 'natural' argument");
                    //throw new GekkoException();
                }
            }

            if (G.Equal(sort, "natural")) xx.Sort(G.CompareNaturalIgnoreCase);
            else xx.Sort(StringComparer.OrdinalIgnoreCase);

            List m = new List();
            foreach (string s in xx)
            {
                m.Add(new ScalarString(s));
            }
            return m;
        }

        public static IVariable flatten(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (x1.Type() == EVariableType.List)
            {
                return O.FlattenIVariables(x1);
            }
            else
            {
                return x1;  //not touched
            }
        }

        public static IVariable unique(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            List<IVariable> tmp = O.ConvertToList(x1);
            List<string> xx = new List<string>(Stringlist.GetListOfStringsFromListOfIvariables(tmp.ToArray()));
            List m = new List();
            GekkoDictionary<string, string> dict = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in xx)
            {
                if (!dict.ContainsKey(s))
                {
                    dict.Add(s, null);
                    m.Add(new ScalarString(s));
                }
                else
                {
                    //ignore
                }
            }

            return m;
        }

        public static IVariable strip(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            return replace(smpl, _t1, _t2, x1, x2, new ScalarString(""));
        }

        public static IVariable tostring(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)  //'string' not allowed as method name
        {
            string s = null;
            if (x.Type() == EVariableType.Val)
            {
                double v = ((ScalarVal)x).val;
                if (G.isNumericalError(v)) s = "M";
                else s = v.ToString();
            }
            else if (x.Type() == EVariableType.Date)
            {
                GekkoTime gt = ((ScalarDate)x).date;
                s = G.FromDateToString(gt);
            }
            else if (x.Type() == EVariableType.String)
            {
                s = ((ScalarString)x).string2;  //maybe could just return x here, but maybe that is not safe
            }
            else if (x.Type() == EVariableType.List)
            {
                List<IVariable> l = ((List)x).list;
                foreach (IVariable iv in l)
                {
                    IVariable iv2 = tostring(smpl, _t1, _t2, iv);
                    string ss = O.ConvertToString(iv2);
                    s += ss + ", ";
                }
                if (s.EndsWith(", ")) s = s.Substring(0, s.Length - 2);
            }
            else if (x.Type() == EVariableType.Series)
            {
                new Error("Cannot convert a SERIES to a string");
                //throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable strings(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string s = null;
            if (x.Type() == EVariableType.List)
            {
                List m = x as List;
                List<IVariable> m2 = new List<IVariable>();
                foreach (IVariable iv in m.list)
                {
                    IVariable iv2 = tostring(smpl, _t1, _t2, iv);
                    m2.Add(iv2);
                }
                return new List(m2);
            }
            else
            {
                new Error("Expected a LIST variable as argument");
                //throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable vals(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string s = null;
            if (x.Type() == EVariableType.List)
            {
                List m = x as List;
                List<IVariable> m2 = new List<IVariable>();
                foreach (IVariable iv in m.list)
                {
                    IVariable iv2 = val(smpl, _t1, _t2, iv);
                    m2.Add(iv2);
                }
                return new List(m2);
            }
            else
            {
                new Error("Expected a LIST variable as argument");
                //throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable dates(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string s = null;
            if (x.Type() == EVariableType.List)
            {
                List m = x as List;
                List<IVariable> m2 = new List<IVariable>();
                foreach (IVariable iv in m.list)
                {
                    IVariable iv2 = date(smpl, _t1, _t2, iv);
                    m2.Add(iv2);
                }
                return new List(m2);
            }
            else
            {
                new Error("Expected a LIST variable as argument");
            }
            return new ScalarString(s);
        }

        public static IVariable date(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            GekkoTime d = GekkoTime.tNull;
            if (x.Type() == EVariableType.Val)
            {
                d = O.ConvertToDate(x);  //already has auto-conversion from VAL to DATE
            }
            else if (x.Type() == EVariableType.Date)
            {
                d = ((ScalarDate)x).date;
            }
            else if (x.Type() == EVariableType.String)
            {
                string s = ((ScalarString)x).string2;
                d = GekkoTime.FromStringToGekkoTime(s);
            }
            else if (x.Type() == EVariableType.List)
            {
                new Error("Cannot convert a LIST to a date");
                //throw new GekkoException();
            }
            else if (x.Type() == EVariableType.Series)
            {
                new Error("Cannot convert a SERIES to a date");
                //throw new GekkoException();
            }
            return new ScalarDate(d);
        }

        public static IVariable val(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            if (G.IsGekkoNull(x1)) return x1;
            double v = double.NaN;
            if (x1.Type() == EVariableType.Val)
            {
                v = ((ScalarVal)x1).val;
            }
            else if (x1.Type() == EVariableType.Date)
            {
                ScalarDate sd = (ScalarDate)x1;
                if (sd.date.freq == EFreq.A || sd.date.freq == EFreq.U)
                {
                    v = sd.date.super;
                }
                else
                {
                    new Error("Cannot only convert annual or undated date to value");
                    //throw new GekkoException();
                }
            }
            else if (x1.Type() == EVariableType.String)
            {
                string s = ((ScalarString)x1).string2;
                v = helper_ValConvertFromString(s);
            }
            else if (x1.Type() == EVariableType.List)
            {
                new Error("Cannot convert a LIST to a value");
                //throw new GekkoException();
            }
            else if (x1.Type() == EVariableType.Series)
            {
                Series x1_series = x1 as Series;
                if (x1_series.type == ESeriesType.Timeless)
                {
                    v = x1_series.GetTimelessData();
                }
                else
                {
                    new Error("Cannot convert a non-timeless SERIES to a value");
                    //throw new GekkoException();
                }
            }
            return new ScalarVal(v);
        }

        public static double helper_ValConvertFromString(string s)
        {
            double v;
            s = s.Trim();
            if (G.Equal(s, "m") || G.Equal(s, "m()") || G.Equal(s, "miss()") || G.Equal(s, "nan"))
            {
                v = double.NaN;
            }
            else
            {
                if (G.TryParseIntoDouble(s, out v))
                {
                    //ok
                }
                else
                {
                    new Error("Could not convert string '" + s + "' into a value");
                }
            }

            return v;
        }

        public static IVariable replaceinside(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3, IVariable max)
        {
            return replace(smpl, _t1, _t2, ths, x2, x3, true, max);
        }

        public static IVariable replaceinside(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3)
        {
            return replace(smpl, _t1, _t2, ths, x2, x3, true, Globals.scalarVal0);
        }

        public static IVariable replace(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3, IVariable max)
        {
            return replace(smpl, _t1, _t2, ths, x2, x3, false, max);
        }

        public static IVariable replace(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3)
        {
            return replace(smpl, _t1, _t2, ths, x2, x3, false, Globals.scalarVal0);
        }

        public static IVariable replace(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x2, IVariable x3, bool isInside, IVariable max)
        {
            int imax = O.ConvertToInt(max);

            if (ths.Type() == EVariableType.String)
            {
                if (isInside)
                {
                    new Error("replaceinside() is for list argument only");
                    //throw new GekkoException();
                }
                string s1 = O.ConvertToString(ths);
                string s2 = O.ConvertToString(x2);
                string s3 = O.ConvertToString(x3);
                string s4 = G.Replace(s1, s2, s3, StringComparison.OrdinalIgnoreCase, imax);
                //string s4 = Regex.Replace(s1, s2, s3, RegexOptions.IgnoreCase);
                return new ScalarString(s4);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List m = ths as List;
                string s2 = O.ConvertToString(x2);
                string s3 = O.ConvertToString(x3);
                List tmp = new List();
                foreach (IVariable iv in m.list)
                {
                    bool hit = false;
                    if (iv.Type() == EVariableType.String)
                    {
                        string s = O.ConvertToString(iv);
                        if (isInside)
                        {
                            if (G.Contains(s, s2))
                            {
                                hit = true;
                                tmp.Add(new ScalarString(G.Replace(s, s2, s3, StringComparison.OrdinalIgnoreCase, imax)));
                                //tmp.Add(new ScalarString(Regex.Replace(s, s2, s3, RegexOptions.IgnoreCase)));                                
                            }
                        }
                        else
                        {
                            if (imax <= 0 || imax == int.MaxValue)
                            {
                                //good
                            }
                            else
                            {
                                new Error("You cannot use max argument with replace() on a list");
                                //throw new GekkoException();
                            }
                            if (G.Equal(s, s2))
                            {
                                hit = true;
                                tmp.Add(new ScalarString(s3));
                            }
                        }
                    }
                    if (!hit) tmp.Add(iv);
                }
                return tmp;
            }
            else if (ths.Type() == EVariableType.Series)
            {
                if (isInside == false && max.ConvertToVal() == 0d)
                {
                    Series ths_series = ths as Series;

                    //good
                    double d2 = O.ConvertToVal(x2);
                    double d3 = O.ConvertToVal(x3);

                    Series lhs = null;

                    if (ths_series.type == ESeriesType.ArraySuper)
                    {
                        lhs = new Series(ths_series.freq, null);
                        lhs.dimensionsStorage = new Multidim();
                        lhs.dimensions = ths_series.dimensions;
                        lhs.type = ESeriesType.ArraySuper;
                        lhs.meta = new SeriesMetaInformation();

                        foreach (KeyValuePair<MultidimItem, IVariable> kvp in ths_series.dimensionsStorage.storage)
                        {
                            Series sub = kvp.Value as Series;
                            Series sub2 = replace(smpl, _t1, _t2, sub, x2, x3, isInside, max) as Series;
                            sub2.type = ESeriesType.Normal;
                            lhs.dimensionsStorage.AddIVariableWithOverwrite(kvp.Key, sub2);
                        }
                    }
                    else if (ths_series.type == ESeriesType.Timeless)
                    {
                        lhs = new Series(ESeriesType.Timeless, ths_series.freq, null, double.NaN);
                        double d_existing = ths_series.GetTimelessData();
                        double rv = Helper_Replace(d_existing, d2, d3);
                        lhs.SetTimelessData(rv);
                    }
                    else
                    {
                        lhs = new Series(ESeriesType.Light, smpl.t0, smpl.t3);
                        foreach (GekkoTime t in smpl.Iterate12())
                        {
                            //will only replace for current sample, not outside of it! So PRT dif(replace(x, m(), 1)) may spring a surprise since relpace does not replace before sample start. But not intended for that though.
                            double d_existing = ths_series.GetDataSimple(t);
                            double rv = Helper_Replace(d_existing, d2, d3);
                            lhs.SetData(t, rv);
                        }
                    }
                    return lhs;

                }
                else
                {
                    new Error("Replace(): you cannot use series type with 'inside' or 'max'"); return null;
                }
            }
            else
            {
                FunctionError("replace", ths);  //throws exception
                return null;
            }
        }

        private static double Helper_Replace(double d_existing, double d2, double d3)
        {
            double rv = double.NaN;
            if (double.IsNaN(d_existing) && double.IsNaN(d2))
            {
                rv = d3;
            }
            else if (d_existing == d2)
            {
                rv = d3;
            }
            else
            {
                //replicate
                rv = d_existing;
            }

            return rv;
        }

        public static IVariable gekkoversion(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(Globals.gekkoVersion);
        }

        public static IVariable gekkobitness(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(Program.Get64Bitness(1));
        }

        public static IVariable gekkoinfo(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            string s = O.ConvertToString(x);
            string rv = G.GekkoInfo(s);
            return new ScalarString(rv);
        }


        public static IVariable currentfreq(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(G.ConvertFreq(Program.options.freq));
        }

        public static IVariable currentperstart(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarDate(Globals.globalPeriodStart);
        }

        public static IVariable currentperend(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarDate(Globals.globalPeriodEnd);
        }

        public static IVariable currentdatetime(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(Program.GetDateTimePretty(DateTime.Now));
        }

        public static IVariable currenttime(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(Program.GetTimeStamp());
        }

        public static IVariable currentdate(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            //See also #80927435209843
            return new ScalarString(Program.GetDateStamp());
        }

        public static IVariable currentdate2(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            //See also #80927435209843
            DateTime dt = DateTime.Now;
            GekkoTime gt = GekkoTime.FromDateTimeToGekkoTime(EFreq.D, dt);
            return new ScalarDate(gt);
        }

        //The following are val-based, not string based --------

        public static IVariable currentyear(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((double)DateTime.Now.Year);
        }

        public static IVariable currentmonth(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((double)DateTime.Now.Month);
        }

        public static IVariable currentday(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((double)DateTime.Now.Day);
        }

        public static IVariable currenthour(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((double)DateTime.Now.Hour);
        }

        public static IVariable currentminute(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((double)DateTime.Now.Minute);
        }

        public static IVariable currentsecond(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarVal((double)DateTime.Now.Second);
        }

        public static IVariable toexceldate(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable d)
        {
            GekkoTime gt = O.ConvertToDate(d);
            if (gt.freq != EFreq.D)
            {
                new Error("toExcelDate() expects a daily date as input");
            }
            int iy = gt.super;
            int im = gt.sub;
            int id = gt.subsub;
            DateTime dt = GekkoTime.DateTime(iy, im, id);
            double ed = dt.ToOADate();
            return new ScalarVal(ed);
        }        

        public static void tracestats2(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            tracestats2(smpl, _t1, _t2, new ScalarString(Program.databanks.GetFirst().GetName()));
        }

        public static void tracestats2(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            //NOTE: Does not include the invisible traces assigned to each series object
            Databank db = Program.databanks.GetDatabank(x.ConvertToString());
            TraceHelper th = Trace2.CollectAllTraces(db, ETraceHelper.GetAllMetasAndTraces);
            int max = 10000;
            int[] depths = new int[max];
            foreach (KeyValuePair<Trace2, PrecedentsAndDepth> kvp in th.tracesDepth2)
            {                
                depths[Math.Min(kvp.Value.depth, max - 1)]++;
            }

            using (Writeln txt = new Writeln())
            {
                txt.MainAdd("Databank " + x.ConvertToString() + ":" + " " + th.seriesObjectCount + " series with " + (th.traces.Count) + " traces in total.");
            }
            using (Writeln txt = new Writeln())
            {
                txt.MainOmitVeryFirstNewLine();
                for (int i = 1; i < max; i++)
                {
                    string extra = null;
                    if (depths[i] == 0) break;
                    if (Globals.runningOnTTComputer || G.IsUnitTesting())
                    {
                        if (i == 0) extra = "    <--- only TTH";
                    }
                    else
                    {
                        if (i == 0) continue;  //do not print that
                    }                    
                    
                    txt.MainAdd("depth: " + (i - 1) + ", traces: " + depths[i] + extra);
                    txt.MainNewLineTight();
                }
                txt.MainAdd("");
            }
            if (Globals.runningOnTTComputer) new Writeln("TTH: Counted " + th.seriesObjectCount + " series, with " + th.metas.Count + " trace starts, " + th.traces.Count + " unique traces, and " + th.traces.Count + " trace combinations.");
        }

        public static void gamsscalar(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] input)
        {
            //To test: start up here: c:\Thomas\Gekko\regres\MAKRO\test3_BACKUP2\klon\Model
            if (input.Length != 1) new Error("Expected 1 argument for gamsscalar()");
            string input1 = O.ConvertToString(input[0]);
            if (G.Equal(input1, "pack"))
            {
                helper_GamsScalar(0, 0, null);
            }
            else if (G.Equal(input1, "info"))
            {
                GekkoTime t = new GekkoTime(EFreq.A, 2030, 1);
                if (Program.model?.modelGamsScalar.dict_FromANumberToVarName != null)
                {
                    Zipper zipper = new Zipper("info.zip");
                    string[] x = Program.model.modelGamsScalar.dict_FromANumberToVarName.OrderBy(s => s, new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToArray();
                    using (FileStream fs = Program.WaitForFileStream(Path.Combine(zipper.tempFolder, "vars.txt"), null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        foreach (string s in x)
                        {
                            sw.WriteLine(s);
                        }
                    }

                    using (FileStream fs = Program.WaitForFileStream(Path.Combine(zipper.tempFolder, "eqs.txt"), null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        foreach (string eq in Program.model.modelGamsScalar.dict_FromEqNumberToEqName)
                        {
                            if (!eq.Contains(t.ToString())) continue;
                            string eqText = Program.model.modelGamsScalar.GetEquationTextUnfolded(eq, false, t);
                            sw.WriteLine(eqText);
                            sw.WriteLine();
                        }
                    }
                    zipper.ZipAndCleanup();
                    new Writeln("Created info.zip with vars.txt and eqs.txt inside. Equations are from the year " + t.ToString());
                }
                else new Error("It does not seem like a GAMS scalar model is loaded");
            }
            else new Error("For gamsscalar(), did not recognize argument '" + input1 + "'");            
        }

        private static void helper_GamsScalar(int depth, int dif0, GamsScalarHelper settings)
        {
            string path = Program.options.folder_working;
            bool optionsFileExists = false;
            try
            {
                if (depth > 1) new Error("GAMS solver called > 2 times in gamsscalar() function. This should not be necessary: report this to the Gekko editor.");
                int dif = 0;                
                if (depth == 0)
                {
                    if (File.Exists(Path.Combine(path, "convert.opt"))) optionsFileExists = true;
                    if (!optionsFileExists)
                    {
                        File.WriteAllText(Path.Combine(path, "convert.opt"), " "); //create a convert.opt file to avoid GAMS error message (will be deleted again at the end). To avoid a warning, " " is used.
                    }
                    string jsonCode = G.RemoveComments(Program.GetTextFromFileWithWait(path + "\\" + "gamsconvert.json"));
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    Dictionary<string, object> jsonTree = null;
                    try
                    {
                        jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);
                    }
                    catch (Exception e)
                    {
                        new Error("The .json file does not seem correctly formatted. " + e.Message);
                    }

                    // -------------------------------------------------------------

                    settings = new GamsScalarHelper();
                    try { settings.zip_name = (string)jsonTree["zip_name"]; } catch { }
                    try { settings.raw_path = (string)jsonTree["raw_path"]; } catch { }
                    try { settings.raw_ignore = (object[])jsonTree["raw_ignore"]; } catch { }
                    try { settings.variable = (string)jsonTree["variable"]; } catch { }
                    try { settings.model = (string)jsonTree["model"]; } catch { }
                    try { settings.counts1 = (string)jsonTree["counts1"]; } catch { }
                    try { settings.counts2 = (string)jsonTree["counts2"]; } catch { }
                    try { settings.counts3 = (string)jsonTree["counts3"]; } catch { }
                    try { settings.t1 = (int)jsonTree["t1"]; } catch { }
                    try { settings.t2 = (int)jsonTree["t2"]; } catch { }
                    try { settings.cmd_lines = (object[])jsonTree["cmd_lines"]; } catch { }
                    try { settings.gms_lines = (object[])jsonTree["gms_lines"]; } catch { }

                    // ============================================

                    if (settings.zip_name == null) new Error("You must indicate zip_name in gamsconvert.json");
                    if (settings.raw_path == null) new Error("You must indicate raw_path in gamsconvert.json");
                    if (settings.raw_ignore == null) settings.raw_ignore = new object[0]; //will not ignore anything if omitted.
                    if (settings.variable == null) new Error("You must indicate variable in gamsconvert.json");
                    if (settings.model == null) new Error("You must indicate model in gamsconvert.json");
                    //if (settings.counts1 == null) new Error("");
                    //if (settings.counts2 == null) new Error("");
                    //if (settings.counts3 == null) new Error("");
                    if (settings.t1 == null) new Error("You must indicate t1 in gamsconvert.json");
                    if (settings.t2 == null) new Error("You must indicate t2 in gamsconvert.json");
                    //if (settings.a1 == null) Error("");
                    //if (settings.a2 == null) Error("");
                    if (settings.cmd_lines == null) new Error("You must indicate cmd_lines in gamsconvert.json");
                    if (settings.gms_lines == null) new Error("You must indicate gms_lines in gamsconvert.json");

                    // -------------------------------------------------------------

                    using (Writeln txt = new Writeln())
                    {
                        txt.MainAdd("The gamsConvert() function is a helper function for GAMS to produce a ");
                        txt.MainAdd("scalar model for use in the Gekko DECOMP command.");
                        txt.MainAdd("A GAMS scalar model is produced by the GAMS CONVERT command.");
                    }
                    using (Writeln txt = new Writeln())
                    {
                        txt.MainAdd("Settings:");
                    }
                    using (Writeln txt = new Writeln("- ", int.MaxValue, System.Drawing.Color.Empty, false, ETabs.Main))
                    {
                        txt.MainOmitVeryFirstNewLine();
                        txt.MainAdd("working folder (model folder) = " + Program.options.folder_working); txt.MainNewLineTight();
                        txt.MainAdd("zip_name = " + settings.zip_name); txt.MainNewLineTight();
                        txt.MainAdd("raw_path = " + settings.raw_path); txt.MainNewLineTight();
                        txt.MainAdd("raw_ignore = "); txt.MainNewLineTight();
                        foreach (string ox in settings.raw_ignore)
                        {
                            string x = ox as string;
                            if (x == null) new Error("Expected raw_ignore elements to be all strings");
                            txt.MainAdd("- " + x); txt.MainNewLineTight();
                        }
                        txt.MainAdd("variable = " + settings.variable); txt.MainNewLineTight();
                        txt.MainAdd("model = " + settings.variable); txt.MainNewLineTight();
                        txt.MainAdd("counts1 = " + settings.counts1); txt.MainNewLineTight();
                        txt.MainAdd("counts2 = " + settings.counts2); txt.MainNewLineTight();
                        txt.MainAdd("counts3 = " + settings.counts3); txt.MainNewLineTight();
                        txt.MainAdd("t1 = " + settings.t1); txt.MainNewLineTight();
                        txt.MainAdd("t2 = " + settings.t2); txt.MainNewLineTight();
                        txt.MainAdd("cmd_lines = "); txt.MainNewLineTight();
                        foreach (string ox in settings.cmd_lines)
                        {
                            string x = ox as string;
                            if (x == null) new Error("Expected cmd_lines elements to be all strings");
                            txt.MainAdd("- " + x); txt.MainNewLineTight();
                        }
                        txt.MainAdd("gms_lines = "); txt.MainNewLineTight();
                        foreach (string ox in settings.gms_lines)
                        {
                            string x = ox as string;
                            if (x == null) new Error("Expected gms_lines elements to be all strings");
                            txt.MainAdd("- " + x); txt.MainNewLineTight();
                        }
                    }
                    Helper_GamsConvert2(path);
                }

                using (Writeln txt = new Writeln())
                {
                    txt.MainAdd(""); txt.MainNewLineTight();
                    txt.MainAdd(""); txt.MainNewLineTight(); ;
                    txt.MainAdd("=========================================================================="); txt.MainNewLineTight();
                    txt.MainAdd("DIFF = " + (dif0 + dif) + ", depth = " + depth); txt.MainNewLineTight();
                    txt.MainAdd("=========================================================================="); txt.MainNewLineTight();
                    txt.MainAdd(""); txt.MainNewLineTight();
                }

                new Writeln("-------------------- calling GAMS start ---------------------------------------");

                using (Writeln txt = new Writeln())
                {
                    using (FileStream fs = Program.WaitForFileStream(Path.Combine(path, "gamsconvert" + depth + ".cmd"), null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        foreach (string s5 in settings.cmd_lines)
                        {
                            string s6 = (s5 as string).Replace("gamsconvert.gms", "gamsconvert" + depth + ".gms");
                            sw.WriteLine(s6);
                        }
                    }

                    string fail = null;

                    using (FileStream fs = Program.WaitForFileStream(Path.Combine(path, "gamsconvert" + depth + ".gms"), null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    {
                        foreach (string s5 in settings.gms_lines)
                        {
                            string s6 = (s5 as string).Replace("{t1}", settings.t1.ToString()).Replace("{t2}", settings.t2.ToString()).Replace("{model}", settings.model);
                            if (dif0 > 0 && s6.Trim().ToLower().StartsWith("solve "))
                            {
                                string s7 = null;
                                for (int i = 0; i < dif0 + dif; i++)
                                {
                                    string s8 = "extra" + i;
                                    s7 += " , " + s8;
                                    sw.WriteLine("equation " + s8 + "[t];");
                                    sw.WriteLine(s8 + "['" + settings.t1.ToString() + "'] .. " + settings.variable + "['" + settings.t1.ToString() + "'] =E= 0;");
                                }
                                sw.WriteLine("model " + settings.model + "_temp / " + settings.model + s7 + " /;");
                                sw.WriteLine("model " + settings.model + " / " + settings.model + "_temp /;");
                            }
                            sw.WriteLine(s6);
                        }
                    }
                }

                DateTime t0 = DateTime.Now;

                string s1 = CrossThreadStuff.GetOutputWindowText();
                Program.ExecuteShellCommand("gamsconvert" + depth + ".cmd", false, null);
                string s2 = CrossThreadStuff.GetOutputWindowText();
                string s = s2.Substring(s1.Length);

                new Writeln("-------------------- calling GAMS end ---------------------------------------");

                List<string> ss = Stringlist.ExtractLinesFromText(s);
                try
                {
                    for (int i = 0; i < ss.Count; i++)
                    {
                        if (G.Contains(ss[i], settings.counts1))
                        {
                            if (G.Contains(ss[i + 1], settings.counts2) && G.Contains(ss[i + 2], settings.counts3))
                            {
                                string[] ss1 = ss[i + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                string[] ss2 = ss[i + 2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                string x1 = ss1[ss1.Length - 1];
                                string x2 = ss2[ss2.Length - 1];
                                int i1 = int.Parse(x1);
                                int i2 = int.Parse(x2);
                                dif = i1 - i2;
                            }
                            else
                            {
                                new Error("It seems the GAMS rows/cols do not match, but Gekko cannot extract the difference from the GAMS output, from the two lines following the line containing '" + settings.counts1 + "'.");
                            }
                        }
                    }
                }
                catch { }

                //Test date/time of gams.gms and dict.txt --> must be after start of ...

                if (dif == 0)
                {                    
                    new Writeln("Created scalar model. Now packing: " + settings.zip_name + "...");
                    if (!File.Exists(Path.Combine(path, "gams.gms"))) new Error("The file gams.gms does not exist in the working folder.");
                    if (!File.Exists(Path.Combine(path, "dict.txt"))) new Error("The file dict.txt does not exist in the working folder.");
                    if (File.GetLastWriteTime(Path.Combine(path, "gams.gms")) < t0) new Error("The file gams.gms does not seem to be newly created.");
                    if (File.GetLastWriteTime(Path.Combine(path, "dict.txt")) < t0) new Error("The file dict.txt does not seem to be newly created.");
                    try
                    {
                        Zipper zipper = new Zipper(settings.zip_name);
                        Program.WaitForFileCopy(Path.Combine(path, "gams.gms"), Path.Combine(zipper.tempFolder, "gams.gms"));
                        Program.WaitForFileCopy(Path.Combine(path, "dict.txt"), Path.Combine(zipper.tempFolder, "dict.txt"));

                        bool isFolder = false;
                        string rawpath = Program.CreateFullPathAndFileName(settings.raw_path);
                        string temp = Path.GetFileName(rawpath);
                        if (G.Equal(temp, "*.gms")) isFolder = true;
                        if (temp.Contains("*") && !isFolder) new Error("Expected raw_path to use '*.gms' not '" + temp + "'");

                        if (isFolder)
                        {
                            StringBuilder sb = new StringBuilder();
                            string[] files = Directory.GetFiles(Path.GetDirectoryName(rawpath), temp, SearchOption.AllDirectories);
                            if (files.Length == 0)
                            {
                                new Warning("Did not find any " + temp + " files in '" + Path.GetDirectoryName(rawpath) + "' folder or sub-folders.");
                            }
                            else
                            {
                                foreach (string file in files)
                                {
                                    if (Path.GetFileNameWithoutExtension(file).StartsWith("gams", StringComparison.OrdinalIgnoreCase)) continue; //drop gams*.gms
                                    if (!G.Equal(Path.GetExtension(temp), Path.GetExtension(file))) continue;  //must be same extension
                                    bool ignore = false;
                                    foreach (object osi in settings.raw_ignore)
                                    {
                                        string si = osi as string;
                                        if (G.Equal(Path.GetFileName(file), si))
                                        {
                                            ignore = true;
                                            break;
                                        }
                                    }
                                    if (ignore) continue;
                                    string fileTxt = Program.GetTextFromFileWithWait(file);
                                    if (!fileTxt.Contains("..")) continue;
                                    sb.Append(fileTxt);
                                    sb.AppendLine();
                                }
                                using (FileStream fs = WaitForFileStream(Path.Combine(zipper.tempFolder, "raw.gms"), null, GekkoFileReadOrWrite.Write))
                                using (StreamWriter res = G.GekkoStreamWriter(fs))
                                {
                                    res.Write(sb);
                                }
                            }
                        }
                        else
                        {
                            if (File.Exists(rawpath))
                            {
                                Program.WaitForFileCopy(rawpath, Path.Combine(zipper.tempFolder, "raw.gms"));
                            }
                            else
                            {
                                new Warning("Did not find raw.gms as this file path: " + rawpath);
                            }
                        }
                        zipper.ZipAndCleanup();
                        new Writeln("Zipping of " + settings.zip_name + " finished");
                    }
                    catch
                    {
                        new Error("Something went wrong when zipping " + settings.zip_name);
                    }                    
                    Program.WaitForFileDelete(Path.Combine(path, "gams.gms"));
                    Program.WaitForFileDelete(Path.Combine(path, "dict.txt"));
                }
                else
                {

                    if (dif0 + dif >= 0)
                    {
                        new Writeln("");
                        using (Writeln txt = new Writeln())
                        {
                            txt.MainAdd("The model row/columns differ with " + dif + " --> Gekko will try to remedy this.");
                            txt.MainAdd("");
                        }
                        new Writeln("");
                        helper_GamsScalar(depth + 1, dif0 + dif, settings);
                    }
                    else
                    {
                        new Error("It seems there are " + dif + " more equations than variables, so some variables need to be unfixed. Gekko does not support that scenario (yet).");
                    }
                }
                if (depth == 0) new Writeln("Successfully packed GAMS scalar model files for Gekko: " + settings.zip_name + ".");
            }
            finally
            {
                if (depth == 0)
                {
                    try { Helper_GamsConvert2(path); } catch { };
                    try { if (!optionsFileExists) File.Delete(Path.Combine(path, "convert.opt")); } catch { };
                }
            }            
        }

        private static void Helper_GamsConvert2(string path)
        {            
            int counter = 0;
            foreach (string f in Directory.EnumerateFiles(path, "gamsconvert*.cmd"))
            {
                File.Delete(f);
            }

            foreach (string f in Directory.EnumerateFiles(path, "gamsconvert*.gms"))
            {
                File.Delete(f);
            }            
        }

        public static IVariable fromexceldate(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x)
        {
            double xx = O.ConvertToVal(x);
            DateTime dt = DateTime.FromOADate(xx);
            GekkoTime gt = GekkoTime.FromDateTimeToGekkoTime(EFreq.D, dt);
            IVariable rv = new ScalarDate(gt);
            return rv;
        }

        // --------------------------------

        public static IVariable currentfolder(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        {
            return new ScalarString(Program.options.folder_working);
        }

        public static IVariable filteredperiods(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            GekkoTime t1 = GekkoTime.tNull;
            GekkoTime t2 = GekkoTime.tNull;
            try
            {
                t1 = O.ConvertToDate(x1);
                t2 = O.ConvertToDate(x2);
            }
            catch
            {
                new Error("Function 'filteredperiods' takes two dates as arguments.");
            }

            ScalarVal z2 = null;

            if (Globals.globalPeriodTimeFilters2.Count == 0)
            {
                //for a quick return in most cases
                z2 = Globals.scalarVal0;
            }
            else
            {
                int counter = 0;
                foreach (GekkoTime gt in new GekkoTimeIterator(t1, t2))
                {
                    if (Program.ShouldFilterPeriod(gt)) counter++;
                }
                z2 = new ScalarVal((double)counter);
            }
            return z2;
        }

        public static IVariable exist(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1)
        {
            double d = 0d;
            IVariable y = O.Lookup(smpl, null, x1, null, new LookupSettings(O.ELookupType.RightHandSide, O.ECreatePossibilities.NoneReturnNullButErrorForParentArraySeries, true), EVariableType.Var, false, null); //will use search settings (data, sim mode) if not bank is given
            if (y != null) d = 1d;
            ScalarVal v = new ScalarVal(d);
            return v;
        }

        public static IVariable fromseries(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            GekkoTime t1, t2; helper_TimeOptionField(smpl, _t1, _t2, out t1, out t2);

            Series ts = null;

            if (x1.Type() == EVariableType.Series)
            {
                ts = x1 as Series;
            }
            else if (x1.Type() == EVariableType.String)
            {
                ts = O.GetIVariableFromString(x1.ConvertToString(), O.ECreatePossibilities.NoneReportError, true) as Series;
                //ts = O.Lookup(null, null, x1, null, O.ELookupType.RightHandSide, EVariableType.Var, null) as Series;
            }
            else
            {
                new Error("fromseries(): expected first argument to be series or string type");
                //throw new GekkoException();
            }

            string s2 = O.ConvertToString(x2);

            if (ts == null)
            {
                new Error("Variable is not of series type");
                //throw new GekkoException();
            }

            if (G.Equal(s2, "name"))
            {
                return new ScalarString(ts.name);
            }
            else if (G.Equal(s2, "bank"))
            {
                string s = "";
                string ss = ts?.meta?.parentDatabank.name;
                if (ss != null) s = ss;
                return new ScalarString(ss);
            }
            else if (G.Equal(s2, "label"))
            {
                return new ScalarString(ts.meta.label);
            }
            else if (G.Equal(s2, "source"))
            {
                return new ScalarString(ts.meta.source);
            }
            else if (G.Equal(s2, "stamp"))
            {
                return new ScalarString(ts.meta.stamp);
            }
            else if (G.Equal(s2, "stamp2"))
            {
                return new ScalarDate(GekkoTime.FromDateTimeToGekkoTime(EFreq.D, Program.GetDateTimePrettyInverse(ts.meta.stamp)));
            }
            else if (G.Equal(s2, "units"))
            {
                return new ScalarString(ts.meta.units);
            }
            else if (G.Equal(s2, "perStart"))
            {
                using (Error e = new Error())
                {
                    e.MainAdd("From Gekko 3.1.4 and onwards, the use of fromSeries('perStart') is obsolete.");
                    e.MainAdd("Please use fromSeries('dataStart') instead. This is more precise, but");
                    e.MainAdd("beware that in some cases, fromSeries('dataStart') may return a different (tighter)");
                    e.MainAdd("date. In existing programs, changing 'perStart' to 'dataStart' may change the results,");
                    e.MainAdd("but in all imaginable cases, this is a change for the better. The use of");
                    e.MainAdd("fromSeries('dataStart') has a small speed penalty compared to 'perStart', but in");
                    e.MainAdd("most cases, this should not be noticable. The hazard of 'perStart' compared to ");
                    e.MainAdd("'dataStart' is no longer deemed worth the speed reward of the former, which is");
                    e.MainAdd("the reason for this change. Apologies for the inconvenience.");
                }
                return null;
            }
            else if (G.Equal(s2, "dataStart"))
            {
                GekkoTime gt = ts.GetRealDataPeriodFirst();
                if (gt.IsNull())
                {
                    new Error("'dataStart': The series has no data or is timeless");
                    //throw new GekkoException();
                }
                return new ScalarDate(gt);
            }
            else if (G.Equal(s2, "dataStartTruncate"))
            {
                //   . . . . d d d d d d d . . . . . (data)
                //          xx1         xx2
                //
                //   . . . . . . . p p p p p . . . . . (period)
                //                yy1     yy2

                GekkoTime xx1 = ts.GetRealDataPeriodFirst();
                GekkoTime xx2 = ts.GetRealDataPeriodLast();

                if (xx1.IsNull())
                {
                    new Error("'dataStartTruncate': The series has no data or is timeless");
                    //throw new GekkoException();
                }

                GekkoTime yy1 = t1;
                GekkoTime yy2 = t2;

                IVariable z = new ScalarDate(xx1);
                if (yy1.StrictlyLargerThan(xx1)) z = new ScalarDate(yy1);
                if (yy1.StrictlyLargerThan(xx2))
                {
                    z = new GekkoNull();
                }

                return z;
            }
            else if (G.Equal(s2, "perEnd"))
            {
                using (Error e = new Error())
                {
                    e.MainAdd("From Gekko 3.1.4 and onwards, the use of fromSeries('perEnd') is obsolete.");
                    e.MainAdd("Please use fromSeries('dataEnd') instead. This is more precise, but");
                    e.MainAdd("beware that in some cases, fromSeries('dataEnd') may return a different (tighter)");
                    e.MainAdd("date. In existing programs, changing 'perEnd' to 'dataEnd' may change the results,");
                    e.MainAdd("but in all imaginable cases, this is a change for the better. The use of");
                    e.MainAdd("fromSeries('dataEnd') has a small speed penalty compared to 'perEnd', but in");
                    e.MainAdd("most cases, this should not be noticable. The hazard of 'perEnd' compared to ");
                    e.MainAdd("'dataEnd' is no longer deemed worth the speed reward of the former, which is");
                    e.MainAdd("the reason for this change. Apologies for the inconvenience.");
                }
                return null;
            }
            else if (G.Equal(s2, "dataEnd"))
            {
                GekkoTime gt = ts.GetRealDataPeriodLast();
                if (gt.IsNull())
                {
                    new Error("'dataEnd': The series has no data or is timeless");
                    //throw new GekkoException();
                }
                return new ScalarDate(gt);
            }
            else if (G.Equal(s2, "dataEndTruncate"))
            {
                //   . . . . d d d d d d d . . . . . (data)
                //          xx1         xx2
                //
                //   . . . p p p p p . . . . . . . . (period)
                //        yy1     yy2

                GekkoTime xx1 = ts.GetRealDataPeriodFirst();
                GekkoTime xx2 = ts.GetRealDataPeriodLast();

                if (xx2.IsNull())
                {
                    new Error("'dataEndTruncate': The series has no data or is timeless");
                    //throw new GekkoException();
                }

                GekkoTime yy1 = t1;
                GekkoTime yy2 = t2;

                IVariable z = new ScalarDate(xx2);
                if (yy2.StrictlySmallerThan(xx2)) z = new ScalarDate(yy2);
                if (yy2.StrictlySmallerThan(xx1))
                {
                    z = new GekkoNull();
                }

                return z;
            }
            else if (G.Equal(s2, "freq"))
            {
                return new ScalarString(G.ConvertFreq(ts.freq));
            }
            else
            {
                new Error("fromSeries(): Argument '" + s2 + "' not recognized."); return null;
            }
        }

        /// <summary>
        /// This should obsolete bankfilename() and banktime(), whereas bankname() could stay.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static IVariable frombank(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            string bank = null;
            if (x1.Type() == EVariableType.String)
            {
                bank = x1.ConvertToString();
            }
            else
            {
                new Error("frombank(): expected first argument to be string type");
                //throw new GekkoException();
            }

            string s2 = O.ConvertToString(x2);

            Databank db = Program.databanks.GetDatabank(bank, true);

            if (G.Equal(s2, "filename"))
            {
                string rv = Program.GetDatabankFilename(db);
                return new ScalarString(rv);                
            }
            else if (G.Equal(s2, "fullpath"))
            {
                string rv = db.FileNameWithPathPretty;
                return new ScalarString(rv);
            }
            else if (G.Equal(s2, "label"))
            {
                return new ScalarString(db.info1);
            }
            else if (G.Equal(s2, "stamp"))
            {
                return new ScalarString(db.date);
            }
            else if (G.Equal(s2, "stamp2"))
            {
                if (G.NullOrEmpty(db.date)) return new GekkoNull();
                return new ScalarDate(GekkoTime.FromDateTimeToGekkoTime(EFreq.D, Program.GetDateTimePrettyInverse(db.date)));
            }
            else if (G.Equal(s2, "format"))
            {
                string s = Globals.currentGbkVersion;
                if (!G.NullOrEmpty(db.databankVersion))
                {
                    s = db.databankVersion;
                }
                return new ScalarString(s);
            }            
            else if (G.Equal(s2, "count"))
            {
                return new ScalarVal(db.storage.Count);
            }
            else
            {
                new Error("Unrecognized frombank() option '" + s2 + "'");
                throw new GekkoException();
            }
        }

        // -----------------------------------
        // LIST functions start
        // -----------------------------------

        public static IVariable union(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og tilføjer dem fra b (uden dubletter). Hvis dubletter i b skal med, skal der bruges komma...

            if (x1.Type() == EVariableType.List && x2.Type() == EVariableType.List)
            {
                //good
            }
            else
            {
                new Error("You can only use the union() function with two lists");
                //throw new GekkoException();
            }

            List<string> lx1 = Stringlist.GetListOfStringsFromList(x1);
            List<string> lx2 = Stringlist.GetListOfStringsFromList(x2);
            List<string> union = new List<string>();
            union.AddRange(lx1);
            GekkoDictionary<string, bool> result = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in lx1)
            {
                if (!result.ContainsKey(s)) result.Add(s, true);
            }
            foreach (string s in lx2)
            {
                if (!result.ContainsKey(s))
                {
                    result.Add(s, true);
                    union.Add(s);
                }
            }
            //union.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new List(union);
        }

        public static IVariable except(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og retainer dem hvis ikke i b.
            List<string> lx1 = Stringlist.GetListOfStringsFromList(x1);
            List<string> lx2 = Stringlist.GetListOfStringsFromList(x2);
            List<string> difference = new List<string>();
            GekkoDictionary<string, bool> temp = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in lx2)
            {
                if (!temp.ContainsKey(s)) temp.Add(s, true);
            }
            foreach (string s in lx1)
            {
                if (!temp.ContainsKey(s))
                {
                    difference.Add(s);
                }
            }
            //difference.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new List(difference);
        }

        public static IVariable intersect(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable x1, IVariable x2)
        {
            if (x1.Type() == EVariableType.List && x2.Type() == EVariableType.List)
            {
                //good
            }
            else
            {
                new Error("You can only use the intersect() function with two lists");
                //throw new GekkoException();
            }

            //tager dem der nu er i a (inkl. dubletter) og retainer dem hvis også i b.
            List<string> lx1 = Stringlist.GetListOfStringsFromList(x1);
            List<string> lx2 = Stringlist.GetListOfStringsFromList(x2);
            List<string> intersection = new List<string>();
            if (lx1.Count > lx2.Count)  //for speedup, we do the heaviest looping on the smaller list.
            {
                ListMultiplyHelper(lx1, lx2, intersection);
            }
            else
            {
                ListMultiplyHelper(lx2, lx1, intersection);
            }
            //intersection.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new List(intersection);
        }

        private static void ListMultiplyHelper(List<string> x1, List<string> x2, List<string> intersection)
        {
            GekkoDictionary<string, bool> temp = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in x1)
            {
                if (!temp.ContainsKey(s))  //there can be dublets
                {
                    temp.Add(s, true);
                }
            }
            GekkoDictionary<string, bool> result = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in x2)
            {
                if (temp.ContainsKey(s))
                {
                    if (!result.ContainsKey(s))
                    {
                        result.Add(s, true);
                    }
                }
            }
            intersection.AddRange(result.Keys);
        }

        // -----------------------------------
        // LIST functions end
        // -----------------------------------



        /// <summary>
        /// Inbuilt function to search for the file root.ini upwards in the directory structure.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="_t1"></param>
        /// <param name="_t2"></param>
        /// <returns></returns>
        public static IVariable root(GekkoSmpl smpl, IVariable _t1, IVariable _t2, params IVariable[] vars)
        {
            if (vars.Length > 1) new Error("Funtion root() only accepts 0 or 1 arguments");
                        
            string rootFileName = "root.ini";
            if (vars.Length == 1)
            {
                string s = O.ConvertToString(vars[0]);
                if (G.Equal(s, "gekko")) rootFileName = "gekko.ini";
                else if (G.Equal(s, "root")) rootFileName = "root.ini";
                else new Error("Expected argument to be 'root' or 'gekko'");
            }
            
            string folder1 = Program.options.folder_working;

            //From working folder
            RootHelper rootHelper1 = new RootHelper();
            rootHelper1.rootFileName = rootFileName;
            helper_root(new DirectoryInfo(folder1), rootHelper1);

            //From gcm file

            if (rootHelper1.roots.Count == 0)
            {
                new Error("Could not find a " + rootFileName + " file in the folder '" + folder1 + "' or any parent folders");
            }
            else if (rootHelper1.roots.Count == 1)
            {
                string fileAndFolder1 = rootHelper1.roots[0];
                //seems to work ok on UNC path, for instance "\\localhost\b$\xx\root.ini" --> "\\localhost\b$\xx"
                string dir1 = Path.GetDirectoryName(fileAndFolder1);
                //if we have "g:\root.ini", this will return "g:\" (note the backslash that is normally omitted)
                //whereas "g:\sub\root.ini" will return "g:\sub". 
                //So for the root we remove the backslash to be consistent, so we get "g:" instead of "g:\".
                //The thing is that we prefer to use {root()}\xx\yy and not {root()}xx\yy.
                //NOTE: Someting like RUN c:x.gcm is always interpreted as a (malformed) library call (library names must be > 1 char).
                if (dir1.EndsWith("\\")) dir1 = dir1.Remove(dir1.Length - 1);

                //now we test that the executing gcm (if any) is consistent with this root
                P p = smpl.p;
                string gcm = null; if (p != null) gcm = p.GetExecutingGcmFile(false); //p may be null, and method may return null                    
                if (gcm != null)
                {
                    string folder2 = Path.GetDirectoryName(gcm);

                    if (!G.Equal(folder1, folder2)) //if working folder and gcm folder is the same, no need to check further (this is often the case)
                    {
                        //From gcm folder
                        RootHelper rootHelper2 = new RootHelper();
                        rootHelper2.rootFileName = rootFileName;
                        helper_root(new DirectoryInfo(folder2), rootHelper2);

                        if (rootHelper2.roots.Count >= 1)
                        {
                            string fileAndFolder2 = rootHelper2.roots[0];  //the first and deepest one
                            if (!G.Equal(fileAndFolder1, fileAndFolder2))
                            {
                                new Error("The " + rootFileName + " file determined from the Gekko working folder is '" + fileAndFolder1 + "', while the " + rootFileName + " file determined from the currently running gcm file is '" + fileAndFolder2 + "'. " + Globals.rootError1);
                            }
                        }
                    }
                }

                return new ScalarString(dir1);
            }
            else
            {
                using (Error error = new Error())
                {
                    error.MainAdd("When searching for a " + rootFileName + " file in the folder '" + folder1 + "' or any parent folders, several files were found. This is illegal, since it is bound to produce confusion and perhaps errors. The files found are the following:");
                    error.MainNewLineTight();
                    int counter = 0;
                    foreach (string s in rootHelper1.roots)
                    {
                        counter++;
                        error.MainAdd("File #" + counter + " of " + rootHelper1.roots.Count + ": " + s);
                        error.MainNewLineTight();
                    }
                }
            }
            return null;  //because of errors we never get here
        }        

        private static void helper_root(DirectoryInfo directoryInfo, RootHelper rootHelper)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (G.Equal(file.Name.Trim(), rootHelper.rootFileName.Trim()))
                {
                    rootHelper.roots.Add(file.FullName.Trim());
                    break;  //no need to carry on, cannot have dublets
                }
            }
            DirectoryInfo parent = directoryInfo.Parent;
            if (parent == null) return;
            helper_root(parent, rootHelper);
        }


        //public static IVariable checkroot(GekkoSmpl smpl, IVariable _t1, IVariable _t2)
        //{

        //    WalkFolderHelper(new DirectoryInfo(""));

        //    void WalkFolderHelper(DirectoryInfo directoryInfo)
        //    {
        //        foreach (FileInfo file in directoryInfo.GetFiles())
        //        {

        //        }
        //        foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
        //        {
        //            WalkFolderHelper(subfolder);
        //        }
        //    }
        //}



        //SOME HARDCODED FUNCTIONS FOR MODELS:
        //See #09875209837532

        public static double CES_UC(double p1rel, double p2rel, double theta, double sigma)
        {
            double c = Math.Pow(theta * Math.Pow(p1rel, 1 - sigma) + (1 - theta) * Math.Pow(p2rel, 1 - sigma), 1 / (1 - sigma));
            return c;
        }

        public static double CES_XL(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p1rel, sigma);
        }

        public static double CES_XR(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p2rel, sigma);
        }

        //See also #9823750983752

        public static double ces_costs(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return y / kappa * Math.Pow(Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
        }

        public static double ces_ac(double p1, double p2, double kappa, double phi, double sigma)
        {
            return 1d / kappa * Math.Pow(Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
        }

        public static double ces_factor1(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), (sigma / (1 - sigma))) * y / kappa * Math.Pow((Math.Pow((p2 / p1), (1 - sigma)) * Math.Pow(((1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))) / (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))), sigma) + 1), (sigma / (1 - sigma)));
        }

        public static double ces_factor2(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return Math.Pow((1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))), (sigma / (1 - sigma))) * y / kappa * Math.Pow((Math.Pow((p1 / p2), (1 - sigma)) * Math.Pow((((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))) / (1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))))), sigma) + 1), (sigma / (1 - sigma)));
        }

        public static IVariable prepend(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x)
        {
            return append(smpl, _t1, _t2, ths, Globals.scalarVal1, x);
        }

        //see also the other append method
        public static IVariable append(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x)
        {
            //FIX: type checks etc.!
            List temp = ths.DeepClone(null, null) as List;
            temp.Add(x);
            return temp;
        }

        //see also the other append method
        public static IVariable append(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable index, IVariable x)
        {
            //FIX: type checks etc.!
            int i = O.ConvertToInt(index, true);
            List temp = ths.DeepClone(null, null) as List;
            if (i - 1 < 0 || i - 1 > temp.list.Count)
            {
                new Error("Cannot insert at position " + i);
                //throw new GekkoException();
            }
            temp.list.Insert(i - 1, x);
            return temp;
        }


        public static IVariable preextend(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable index, IVariable x)
        {
            return extend(smpl, _t1, _t2, ths, Globals.scalarVal1, x);
        }


        //see also the other extend() method
        public static IVariable extend(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable index, IVariable x)
        {
            if (ths.Type() != EVariableType.List) FunctionError("extend", x);
            int i = O.ConvertToInt(index, true);
            List temp = ths.DeepClone(null, null) as List;
            if (i - 1 < 0 || i - 1 > temp.list.Count)
            {
                new Error("Cannot insert at position " + i);
                //throw new GekkoException();
            }
            if (x.Type() == EVariableType.List)
            {
                List x_list = x as List;
                temp = temp.DeepClone(null, null) as List;
                temp.list.InsertRange(i - 1, x_list.list);
            }
            else
            {
                FunctionError("extend", x);
            }
            return temp;
        }

        //see also the other extend() method
        public static IVariable extend(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x)
        {
            if (ths.Type() != EVariableType.List) FunctionError("extend", x);
            List temp = ths as List;
            if (x.Type() == EVariableType.List)
            {
                List x_list = x as List;
                temp = temp.DeepClone(null, null) as List;
                temp.list.AddRange(x_list.list);
            }
            else
            {
                FunctionError("extend", x);
            }
            return temp;
        }

        //See also suffix()
        public static IVariable prefix(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x)
        {
            IVariable rv = null;
            string x_string = O.ConvertToString(x);
            if (ths.Type() == EVariableType.String)
            {
                string s = O.ConvertToString(ths);
                rv = new ScalarString(x_string + s);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List<string> xx = new List<string>(Stringlist.GetListOfStringsFromListOfIvariables((ths as List).list.ToArray()));
                List rv2 = new List();
                foreach (string s in xx)
                {
                    rv2.Add(new ScalarString(x_string + s));
                }
                rv = rv2;
            }
            else
            {
                FunctionError("prefix", ths);
                throw new GekkoException();
            }
            return rv;
        }

        //See also prefix()
        public static IVariable suffix(GekkoSmpl smpl, IVariable _t1, IVariable _t2, IVariable ths, IVariable x)
        {
            IVariable rv = null;
            string x_string = O.ConvertToString(x);
            if (ths.Type() == EVariableType.String)
            {
                string s = O.ConvertToString(ths);
                rv = new ScalarString(s + x_string);
            }
            else if (ths.Type() == EVariableType.List)
            {
                List<string> xx = new List<string>(Stringlist.GetListOfStringsFromListOfIvariables((ths as List).list.ToArray()));
                List rv2 = new List();
                foreach (string s in xx)
                {
                    rv2.Add(new ScalarString(s + x_string));
                }
                rv = rv2;
            }
            else
            {
                FunctionError("prefix", ths);
                throw new GekkoException();
            }
            return rv;
        }

        private static void FunctionError(string s, IVariable x)
        {
            new Error("Function " + s + "() does not allow a " + G.GetTypeString(x) + " variable");
        }
    }
}


