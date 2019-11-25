using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Linq;
using System.Text;

namespace Gekko
{
    
    public enum EFreq 
    {
        //========================================================================================================
        //                          FREQUENCY LOCATION, indicates where to implement more frequencies
        //========================================================================================================
        A,
        Q,
        M,        
        U,        //also called 'u' in Eviews, called 'n' in TSP, but undated has no name in AREMOS (uses 'periodic')     
        None,     //used to signal non-freq variable, for instance a VAL   
        D,        //daily        
        Empty2,   // --------> this and the following can be filled/changed
        Empty3,
        Empty4,
        Empty5,
        Empty6,
        Empty7,
        Empty8,
        Empty9,
        Empty10,
        Empty11,
        Empty12,
        Empty13,
        Empty14,
        Empty15,
        Empty16,
        Empty17,
        Empty18,
        Empty19, 
        Empty20
    }

    public enum ESeriesMissing
    {
        Error,
        M,
        Zero,
        Skip,
        Ignore
    }
    
    public class GekkoTimeStuff
    {
        public static readonly int numberOfQuarters = 4;
        public static readonly int numberOfMonths = 12;
    }

    [Serializable]
    [ProtoContract]
    //GekkoTime is an immutable struct for fast looping. Structs should be < 16 bytes to be effective (we have 3 x 4 = 12 bytes here)
    public struct GekkoTime
    {
        //use IsNull() to check for null. The fields are short, to reduce size since this is a struct (that also gets saved in GBK files).
        [ProtoMember(1)]
        public readonly short super;   //year, null object is emulated by setting super to -12345
        [ProtoMember(2)]
        public readonly short sub;     //quarter, month, week, etc. (not day)
        [ProtoMember(3)]
        public readonly short subsub;  //day, if sub is month
        [ProtoMember(4)]
        public readonly EFreq freq;

        public static GekkoTime tNull = new GekkoTime(EFreq.A, -12345, 1);  //think of it as a 'null' object (but it is a struct)
                
        //Note: using "new GekkoTime()" without arguments is not intended to be used, even
        //      though it is valid to do. Such a struct cannot have its fields changed anyway, so
        //      it will be unusable.
        public GekkoTime(EFreq freq2, int super2, int sub2, int subsub2)
        {
            freq = freq2;
            super = (short)super2;
            sub = (short)sub2;
            subsub = (short)subsub2;
            FreqCheck();
        }

        public GekkoTime(EFreq freq2, int super2, int sub2)
        {
            freq = freq2;
            super = (short)super2;
            sub = (short)sub2;
            subsub = (short)1;
            FreqCheck();
        }        

        private void FreqCheck()
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //Sanity checks to follow
            //Problem is that TIME 2010m13 2012m0 can probably parse. If not, the check below is not necessary.            
            
            if (freq == EFreq.A)
            {
                //sub and subsub can be anything <= 1
                if (sub > 1 || subsub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'a' cannot have subperiod > 1");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.Q)
            {
                if (sub < 1 || sub > 4 || subsub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'q' wrong quarter: " + sub);
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.M)
            {
                if (sub < 1 || sub > 12 || subsub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'm' wrong month: " + sub);
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.D)
            {
                if (sub < 1 || sub > 12)
                {
                    G.Writeln2("*** ERROR: freq 'd' wrong month: " + sub);
                    throw new GekkoException();
                }
                int maxDays = G.DaysInMonth(super, sub);  //see also #9832453429857
                if (subsub < 1 || subsub > maxDays)
                {
                    G.Writeln2("*** ERROR: freq 'd' wrong day: " + subsub);
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.U)
            {
                if (sub > 1 || subsub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'u' cannot have subperiod > 1");
                    throw new GekkoException();
                }
            }
        }

        private GekkoTime(EFreq freq2, int super2, int sub2, bool check)
        {
            //The check argument is not actually used, just can call the method with 'false'.
            //This method is only used for looping and adding internally, not for creating
            //a fresh new GekkoTime from command input. Hence the method is private.
            freq = freq2;
            super = (short)super2;
            sub = (short)sub2;
            subsub = (short)1;
        }

        public bool IsNull()
        {
            if (this.super == -12345) return true;
            return false;
        }

        public static GekkoTime FromDateTimeToGekkoTime(EFreq freq, DateTime dt)
        {
            int sub = 1;
            int subsub = 1;
            int super = dt.Year;
            if (freq == EFreq.A)
            {                
            }
            else if (freq == EFreq.Q)
            {
                sub = GekkoTime.FromMonthToQuarter(dt.Month);
            }
            else if (freq == EFreq.M)
            {
                sub = dt.Month;
            }
            else if (freq == EFreq.D)
            {
                sub = dt.Month;
                subsub = dt.Day;
            }
            else if (freq == EFreq.U)
            {
                G.Writeln2("*** ERROR: Cannot convert date to undated frequency");
                throw new GekkoException();
            }

            return new GekkoTime(freq, super, sub, subsub);
        }

        public static DateTime FromExcelDateToDateTime(double data)
        {
            return DateTime.FromOADate(data);
        }

        public static void FromGekkoTimeToDifferentFormatsForWriting(GekkoTime gt, bool first, string format, out DateTime dt, out string f, out string date_as_string)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //format can be null, 'gekko' or 'yyyy-dd-mm'-style

            dt = new DateTime();

            f = null;
            //f = format;

            date_as_string = null;

            int y = gt.super;
            int m = -12345;
            int d = -12345;

            if (gt.freq == EFreq.A)
            {
                if (format == null) f = "yyyy";
                if (first) { m = 1; d = 1; }
                else { m = 12; d = 31; }
            }
            else if (gt.freq == EFreq.Q)
            {
                if (format == null) f = "yyyy-mm";
                if (gt.sub == 1)
                {
                    if (first) { m = 1; d = 1; }
                    else { m = 3; d = G.DaysInMonth(y, m); }
                }
                else if (gt.sub == 2)
                {
                    if (first) { m = 4; d = 1; }
                    else { m = 6; d = G.DaysInMonth(y, m); }
                }
                else if (gt.sub == 3)
                {
                    if (first) { m = 7; d = 1; }
                    else { m = 9; d = G.DaysInMonth(y, m); }
                }
                else
                {
                    if (first) { m = 10; d = 1; }
                    else { m = 12; d = G.DaysInMonth(y, m); }
                }
            }
            else if (gt.freq == EFreq.M)
            {
                if (format == null) f = "yyyy-mm";
                m = gt.sub;
                if (first) d = 1;
                else d = G.DaysInMonth(y, m);
            }
            else if (gt.freq == EFreq.D)
            {
                if (format == null) f = "yyyy-mm-dd";
                m = gt.sub;
                d = gt.subsub;
            }
            else if (gt.freq == EFreq.U)
            {
                G.Writeln2("*** ERROR: You cannot use dateformat together with an undated frequency");
                throw new GekkoException();
            }

            //Now: three possibilities regarding format:
            //format == null                   --> f has 'yyyy-mm-dd'-style value (custom)
            //format == 'gekko'                --> f = null
            //format == 'yyyy-mm-dd'-style     --> f = null

            if (format != null && !G.Equal(format, "gekko")) f = format;

            //Now: three possibilities regarding format:
            //format == null                   --> f = 'yyyy-mm-dd'-style value (custom)
            //format == 'gekko'                --> f = null
            //format == 'yyyy-mm-dd'-style     --> f = 'yyyy-mm-dd'-style

            dt = new DateTime(y, m, d);
            if (format == null)
            {
                //Not sure if this is right, using f?? Probably ok.
                date_as_string = G.DateHelper3(f, dt); //lowercase 'm' is understood as minutes in C#
            }
            else if (G.Equal(format, "gekko"))
            {
                date_as_string = gt.ToString();
            }
            else
            {
                date_as_string = G.DateHelper3(format, dt); //lowercase 'm' is understood as minutes in C#
            }
        }

        public static DateTime FromYYYYMMDDToDateTime(string format, string s)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.ParseExact(s, format.ToLower().Replace("m", "M"), null);
            }
            catch (Exception e)
            {
                G.Writeln2("*** ERROR: The date '" + s + "' does not comply with the format '" + format + "'");
                throw new GekkoException();
            }
            return dt;
        }

        public static GekkoTime FromStringToGekkoTime(string s)
        {
            return FromStringToGekkoTime(s, false);
        }

        public static GekkoTime FromStringToGekkoTime(string s, bool allowKForQuarters)
        {
            return FromStringToGekkoTime(s, allowKForQuarters, true);
        }

        public static GekkoTime FromStringToGekkoTime(string s, bool allowKForQuarters, bool reportError)
        {
            //To do the reverse: see G.FromDateToString()   

            //trailing a or a1 accepted: 2001a, 2001a1
            //trailing u or u1 accepted: 2001u, 2001u1
            //k may be accepted for dates (allowK...)
            //two digits like 98 are understood as annual 1998
            //else: 2001, 2001q1, 2001m1, 2001m1d15

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            GekkoTime t = GekkoTime.tNull;

            if (true)
            {
                int i = -12345;
                bool b = int.TryParse(s, out i);
                if (b)
                {
                    //happens often, so we do it fast
                    return new GekkoTime(EFreq.A, G.findYear(i), 1);
                }
            }

            if (s.EndsWith("a1", StringComparison.OrdinalIgnoreCase))
            {
                int i = -12345;
                bool b = int.TryParse(s.Substring(0, s.Length - 2), out i);
                if (b)
                {
                    return new GekkoTime(EFreq.A, G.findYear(i), 1);
                }
                else
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }
            else if (s.EndsWith("a", StringComparison.OrdinalIgnoreCase))
            {
                int i = -12345;
                bool b = int.TryParse(s.Substring(0, s.Length - 1), out i);
                if (b)
                {
                    return new GekkoTime(EFreq.A, G.findYear(i), 1);
                }
                else
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }


            if (s.Contains("q") || s.Contains("Q"))
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'q', 'Q' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int q1 = int.Parse(temp1[1]);
                    if (q1 < 1 || q1 > 4)
                    {
                        if (reportError)
                        {
                            G.Writeln("*** ERROR: should have quarters from 1 to and including 4");
                            throw new GekkoException();
                        }
                        else return GekkoTime.tNull;
                    }
                    t = new GekkoTime(EFreq.Q, y1, q1);
                }
                catch (Exception e)
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }
            else if (allowKForQuarters && (s.Contains("k") || s.Contains("K")))
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'k', 'K' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int q1 = int.Parse(temp1[1]);
                    if (q1 < 1 || q1 > 4)
                    {
                        if (reportError)
                        {
                            G.Writeln("*** ERROR: should have quarters from 1 to and including 4");
                            throw new GekkoException();
                        }
                        else return GekkoTime.tNull;
                    }
                    t = new GekkoTime(EFreq.Q, y1, q1);
                }
                catch (Exception e)
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }
            else if (s.Contains("d") || s.Contains("d"))  //must be before 'm'
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'm', 'M' });  //2019m12d24
                    string[] temp2 = temp1[1].Split(new char[] { 'd', 'd' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int m1 = int.Parse(temp2[0]);
                    if (m1 < 1 || m1 > 12)
                    {
                        if (reportError)
                        {
                            G.Writeln("*** ERROR: should have months from 1 to and including 12");
                            throw new GekkoException();
                        }
                        else return GekkoTime.tNull;
                    }
                    int d = int.Parse(temp2[1]);
                    int maxDays = G.DaysInMonth(y1, m1); //see also #9832453429857
                    if (d < 1 || d > maxDays)
                    {
                        G.Writeln("*** ERROR: illegal day in daily date");
                        throw new GekkoException();
                    }
                    t = new GekkoTime(EFreq.D, y1, m1, d);
                }
                catch (Exception e)
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }
            else if (s.Contains("m") || s.Contains("M"))
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'm', 'M' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int m1 = int.Parse(temp1[1]);
                    if (m1 < 1 || m1 > 12)
                    {
                        if (reportError)
                        {
                            G.Writeln("*** ERROR: should have months from 1 to and including 12");
                            throw new GekkoException();
                        }
                        else return GekkoTime.tNull;
                    }
                    t = new GekkoTime(EFreq.M, y1, m1);
                }
                catch (Exception e)
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }
            else if (s.Contains("u") || s.Contains("U")) 
            {
                string s2 = s;
                if (s.EndsWith("u1", StringComparison.OrdinalIgnoreCase))
                {
                    s2 = s.Substring(0, s.Length - 2);
                }
                else if (s.EndsWith("u", StringComparison.OrdinalIgnoreCase))
                {
                    s2 = s.Substring(0, s.Length - 1);
                }

                try
                {
                    t = new GekkoTime(EFreq.U, int.Parse(s2), 1);
                }
                catch (Exception e)
                {
                    if (reportError)
                    {
                        G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                        throw new GekkoException();
                    }
                    else return GekkoTime.tNull;
                }
            }
            else
            {
                if (reportError)
                {
                    G.Writeln2("*** ERROR: Could not understand the timeperiod: " + s);
                    throw new GekkoException();
                }
                else return GekkoTime.tNull;
            }
            return t;
        }

        public static DateTime FromGekkoTimeToDateTime(GekkoTime gt, O.GetDateChoices firstLast)
        {
            //See also #09834753425, converting from GekkoTime to GekkoTime

            if (gt.freq == EFreq.A)
            {
                if (firstLast == O.GetDateChoices.FlexibleStart)
                {
                    return new DateTime(gt.super, 1, 1);
                }
                else if (firstLast == O.GetDateChoices.FlexibleEnd)
                {
                    return new DateTime(gt.super, 12, 31);
                }
                else
                {
                    G.Writeln2("*** ERROR: Freq convertion problem");
                    throw new GekkoException();
                }
            }
            else if (gt.freq == EFreq.Q)
            {
                if (firstLast == O.GetDateChoices.FlexibleStart)
                {
                    return new DateTime(gt.super, GekkoTime.FromQuarterToMonthStart(gt.sub), 1);
                }
                else if (firstLast == O.GetDateChoices.FlexibleEnd)
                {
                    int month = GekkoTime.FromQuarterToMonthEnd(gt.sub);
                    return new DateTime(gt.super, month, G.DaysInMonth(gt.super, month));
                }
                else
                {
                    G.Writeln2("*** ERROR: Freq convertion problem");
                    throw new GekkoException();
                }
            }
            else if (gt.freq == EFreq.M)
            {
                if (firstLast == O.GetDateChoices.FlexibleStart)
                {
                    return new DateTime(gt.super, gt.sub, 1);
                }
                else if (firstLast == O.GetDateChoices.FlexibleEnd)
                {
                    return new DateTime(gt.super, gt.sub, G.DaysInMonth(gt.super, gt.sub));
                }
                else
                {
                    G.Writeln2("*** ERROR: Freq convertion problem");
                    throw new GekkoException();
                }
            }
            else if (gt.freq == EFreq.D)
            {
                return new DateTime(gt.super, gt.sub, gt.subsub);
            }
            else
            {
                G.Writeln2("*** ERROR: Freq convertion problem");
                throw new GekkoException();
            }
        }

        public static int Observations(GekkoTime t1, GekkoTime t2)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //BEWARE: Can return 0 or a negative number!
            //Also checks that freqs are the same
            if (t1.freq != t2.freq)
            {
                G.Writeln2("*** ERROR: Frequency mismatch: " + G.GetFreqString(t1.freq) + " vs. " + G.GetFreqString(t2.freq));
                throw new GekkoException();
            }
            EFreq efreq = t1.freq;

            if (efreq == EFreq.D)
            {
                //see also #98032743029847
                DateTime dt1 = new DateTime(t1.super, t1.sub, t1.subsub);
                DateTime dt2 = new DateTime(t2.super, t2.sub, t2.subsub);
                return (dt2 - dt1).Days + 1;
            }
            else
            {
                int subPeriods = 1;
                if (efreq == EFreq.A)
                {
                    //fast return
                    return t2.super - t1.super + 1;  //Subpers are ignored. It is tacitly assumed that the subperiods are = 1 here, else this is nonsense
                }
                else if (efreq == EFreq.Q) subPeriods = 4;
                else if (efreq == EFreq.M) subPeriods = 12;
                else if (efreq == EFreq.U) subPeriods = 1;
                else
                {
                    G.Writeln2("*** ERROR: Error regarding frequency");
                    throw new GekkoException();
                }

                int obs = subPeriods * (t2.super - t1.super) + t2.sub - t1.sub + 1;
                if (obs < 0)
                {
                    //This should not normally be possible, maybe with PRT<2010 2009> or the like?
                }
                return obs;
            }
        }

        public static void ConvertFreqs(EFreq freq, GekkoTime t1, GekkoTime t2, ref GekkoTime tt1, ref GekkoTime tt2)
        {
            tt1 = ConvertFreqsFirst(freq, t1);
            tt2 = ConvertFreqsLast(freq, t2);
        }

        public static GekkoTime ConvertFreqsFirst(EFreq freq, GekkoTime t1)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //See also #09834753425, converting from GekkoTime to DateTime

            GekkoTime tt1 = t1;
            if (freq == t1.freq)
            {
                //do nothing
            }
            else
            {

                if (freq == EFreq.A)
                {
                    //From Q or M or D to A freq is just the annual part of the date

                    if (t1.freq == EFreq.Q)
                    {
                        tt1 = new GekkoTime(EFreq.A, t1.super, 1);
                    }
                    else if (t1.freq == EFreq.M || t1.freq == EFreq.D)
                    {
                        tt1 = new GekkoTime(EFreq.A, t1.super, 1);
                    }
                    else if (t1.freq == EFreq.U)
                    {
                        tt1 = new GekkoTime(EFreq.A, t1.super, 1);
                    }

                }
                else if (freq == EFreq.Q)
                {
                    if (t1.freq == EFreq.A)
                    {
                        //from A to Q sets q1 for start year and q4 for end year
                        tt1 = new GekkoTime(EFreq.Q, t1.super, 1);
                    }
                    else if (t1.freq == EFreq.M || t1.freq == EFreq.D)
                    {
                        //from M or D to Q finds corresponding Q
                        tt1 = new GekkoTime(EFreq.Q, t1.super, GekkoTime.FromMonthToQuarter(t1.sub));  //first m                        
                    }                    
                    else if (t1.freq == EFreq.U)
                    {
                        tt1 = new GekkoTime(EFreq.Q, t1.super, 1);
                    }
                }
                else if (freq == EFreq.M)
                {
                    if (t1.freq == EFreq.A)
                    {
                        //from A to M sets m1 for start year, and m12 for end year
                        tt1 = new GekkoTime(EFreq.M, t1.super, 1);
                    }
                    else if (t1.freq == EFreq.Q)
                    {
                        //from Q to M sets mx for start q, and my for end q
                        tt1 = new GekkoTime(EFreq.M, t1.super, GekkoTime.FromQuarterToMonthStart(t1.sub));
                    }
                    else if (t1.freq == EFreq.D)
                    {
                        //from D to M sets month directly
                        tt1 = new GekkoTime(EFreq.M, t1.super, t1.sub);
                    }
                    else if (t1.freq == EFreq.U)
                    {
                        tt1 = new GekkoTime(EFreq.M, t1.super, 1);
                    }
                }
                else if (freq == EFreq.D)
                {
                    if (t1.freq == EFreq.A)
                    {
                        //from A to D sets first day
                        tt1 = new GekkoTime(EFreq.D, t1.super, 1, 1);
                    }
                    else if (t1.freq == EFreq.Q)
                    {
                        //from Q to D sets first day
                        tt1 = new GekkoTime(EFreq.D, t1.super, GekkoTime.FromQuarterToMonthStart(t1.sub), 1);
                    }
                    else if (t1.freq == EFreq.M)
                    {
                        //from M to D sets first day
                        tt1 = new GekkoTime(EFreq.D, t1.super, t1.sub, 1);
                    }                    
                    else if (t1.freq == EFreq.U)
                    {
                        tt1 = new GekkoTime(EFreq.D, t1.super, 1, 1);
                    }
                }
                else if (freq == EFreq.U)
                {
                    //From Q or M to U freq is just the annual part of the date

                    if (t1.freq == EFreq.A)
                    {
                        tt1 = new GekkoTime(EFreq.U, t1.super, 1);
                    }
                    else if (t1.freq == EFreq.Q)
                    {
                        tt1 = new GekkoTime(EFreq.U, t1.super, 1);
                    }
                    else if (t1.freq == EFreq.M || t1.freq == EFreq.D)
                    {
                        tt1 = new GekkoTime(EFreq.U, t1.super, 1);
                    }
                }
            }

            return tt1;
        }

        public static GekkoTime ConvertFreqsLast(EFreq freq, GekkoTime t2)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            GekkoTime tt2 = t2;

            if (freq == t2.freq)
            {
                //do nothing
            }
            else
            {

                if (freq == EFreq.A)
                {
                    //From Q or M to A freq is just the annual part of the date

                    if (t2.freq == EFreq.Q)
                    {
                        tt2 = new GekkoTime(EFreq.A, t2.super, 1);
                    }
                    else if (t2.freq == EFreq.M || t2.freq == EFreq.D)
                    {
                        tt2 = new GekkoTime(EFreq.A, t2.super, 1);
                    }
                    else if (t2.freq == EFreq.U)
                    {
                        tt2 = new GekkoTime(EFreq.A, t2.super, 1);
                    }

                }
                else if (freq == EFreq.Q)
                {
                    if (t2.freq == EFreq.A)
                    {
                        //from A to Q sets q1 for start year and q4 for end year                        
                        tt2 = new GekkoTime(EFreq.Q, t2.super, GekkoTimeStuff.numberOfQuarters);
                    }
                    else if (t2.freq == EFreq.M || t2.freq == EFreq.D)
                    {
                        //from M to Q finds corresponding Q                        
                        tt2 = new GekkoTime(EFreq.Q, t2.super, GekkoTime.FromMonthToQuarter(t2.sub));  //last m                 
                    }
                    else if (t2.freq == EFreq.U)
                    {
                        tt2 = new GekkoTime(EFreq.Q, t2.super, GekkoTimeStuff.numberOfQuarters);
                    }
                }
                else if (freq == EFreq.M)
                {
                    if (t2.freq == EFreq.A)
                    {
                        //from A to M sets m1 for start year, and m12 for end year                        
                        tt2 = new GekkoTime(EFreq.M, t2.super, GekkoTimeStuff.numberOfMonths);
                    }
                    else if (t2.freq == EFreq.Q)
                    {
                        //from Q to M sets mx for start q, and my for end q                        
                        tt2 = new GekkoTime(EFreq.M, t2.super, GekkoTime.FromQuarterToMonthEnd(t2.sub));
                    }
                    else if (t2.freq == EFreq.D)
                    {
                        //from D to M sets month directly
                        tt2 = new GekkoTime(EFreq.M, t2.super, t2.sub);
                    }
                    else if (t2.freq == EFreq.U)
                    {
                        tt2 = new GekkoTime(EFreq.M, t2.super, GekkoTimeStuff.numberOfMonths);
                    }
                }
                else if (freq == EFreq.D)
                {
                    if (t2.freq == EFreq.A)
                    {
                        //from A to D sets last day of year
                        tt2 = new GekkoTime(EFreq.D, t2.super, GekkoTimeStuff.numberOfMonths, 31);
                    }
                    else if (t2.freq == EFreq.Q)
                    {
                        //from Q to D sets last day of quarter
                        int month = GekkoTime.FromQuarterToMonthEnd(t2.sub);
                        tt2 = new GekkoTime(EFreq.D, t2.super, month, G.DaysInMonth(t2.super, month));
                    }
                    else if (t2.freq == EFreq.M)
                    {
                        //from M to D sets last day of month                        
                        tt2 = new GekkoTime(EFreq.D, t2.super, t2.sub, G.DaysInMonth(t2.super, t2.sub));
                    }
                    else if (t2.freq == EFreq.U)
                    {
                        //from U to D sets last day of year
                        tt2 = new GekkoTime(EFreq.D, t2.super, GekkoTimeStuff.numberOfMonths, 31);
                    }
                }
                else if (freq == EFreq.U)
                {
                    //From Q or M or D to U freq is just the annual part of the date

                    if (t2.freq == EFreq.A)
                    {
                        tt2 = new GekkoTime(EFreq.U, t2.super, 1);
                    }
                    else if (t2.freq == EFreq.Q)
                    {
                        tt2 = new GekkoTime(EFreq.U, t2.super, 1);
                    }
                    else if (t2.freq == EFreq.M || t2.freq == EFreq.D)
                    {
                        tt2 = new GekkoTime(EFreq.U, t2.super, 1);
                    }
                }
            }

            return tt2;
        }

        private void CheckSameFreq(GekkoTime gt2)
        {
            if (this.freq != gt2.freq)
            {
                if (this.IsNull() || gt2.IsNull())
                {
                    //ok
                }
                else
                {
                    G.Writeln2("*** ERROR: Comparing two different frequencies");
                    throw new GekkoException();
                }
            }
        }

        public bool StrictlyLargerThan(GekkoTime gt2)
        {
            return StrictlyLargerThan(gt2, true);
        }

        public bool StrictlyLargerThan(GekkoTime gt2, bool check)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if(check) CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (this.super > gt2.super) return true; //gt1 har larger year
            else if (this.super == gt2.super)
            {
                //same year
                if (this.freq == EFreq.D)
                {
                    if (this.sub > gt2.sub) return true; //same year, gt1 has larger month
                    else if (this.sub == gt2.sub)
                    {
                        //same year and same month -> gt1.day must be > gt2.day
                        if (this.subsub > gt2.subsub) return true;
                    }
                }
                else
                {
                    //same year, compare months/quarters
                    if (this.sub > gt2.sub) return true;
                }
            }
            return false;
        }

        public bool LargerThanOrEqual(GekkoTime gt2)
        {
            return LargerThanOrEqual(gt2, true);
        }

        public bool LargerThanOrEqual(GekkoTime gt2, bool check)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (check) CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (IsSamePeriod(gt2, false)) return true;
            if (StrictlyLargerThan(gt2, false)) return true;
            return false;
        }

        public bool SmallerThanOrEqual(GekkoTime gt2)
        {
            return SmallerThanOrEqual(gt2, true);
        }

        public bool SmallerThanOrEqual(GekkoTime gt2, bool check)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (check) CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if(IsSamePeriod(gt2, false)) return true;
            if (StrictlySmallerThan(gt2, false)) return true;
            return false;
        }

        public bool StrictlySmallerThan(GekkoTime gt2)
        {
            return StrictlySmallerThan(gt2, true);
        }            

        public bool StrictlySmallerThan(GekkoTime gt2, bool check)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (check) CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (this.super < gt2.super) return true; //gt1 har smaller year
            else if (this.super == gt2.super)
            {
                //same year
                if (this.freq == EFreq.D)
                {
                    if (this.sub < gt2.sub) return true; //same year, gt1 has smaller month
                    else if (this.sub == gt2.sub)
                    {
                        //same year and same month -> gt1.day must be < gt2.day
                        if (this.subsub < gt2.subsub) return true;
                    }
                }
                else
                {
                    //same year, compare months/quarters
                    if (this.sub < gt2.sub) return true;
                }
            }
            return false;
        }

        public bool IsSamePeriod(GekkoTime gt2)
        {
            return IsSamePeriod(gt2, true);
        }

        public bool IsSamePeriod(GekkoTime gt2, bool check)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //will handle null ok, two null --> true
            if (check) CheckSameFreq(gt2);
            if (this.freq == EFreq.D)
            {
                if (this.super == gt2.super && this.sub == gt2.sub && this.subsub == gt2.subsub) return true;                
            }
            else
            {
                if (this.super == gt2.super && this.sub == gt2.sub) return true;                
            }
            return false;
        }

        public GekkoTime Add(int addedPeriods)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //seems to work for negative adds
            //see also GetPeriod()
            //Could probably use DateTime functions, but we like to keep it fast and simple

            if (addedPeriods == 0) return this;

            if (this.freq == EFreq.D)
            {
                //see also #98032743029847
                DateTime dt1 = new DateTime(this.super, this.sub, this.subsub);  
                DateTime dt2 = dt1.AddDays(addedPeriods);
                GekkoTime gt = GekkoTime.FromDateTimeToGekkoTime(this.freq, dt2);
                return gt;
            }
            else
            {
                int subPeriods = 1;
                if (this.freq == EFreq.A)
                {
                    //Simple: make it run fast!                
                    return new GekkoTime(this.freq, this.super + addedPeriods, this.sub, false);  //call the fast constructor
                }
                else if (this.freq == EFreq.Q) subPeriods = 4;
                else if (this.freq == EFreq.M) subPeriods = 12;
                else if (this.freq == EFreq.U) subPeriods = 1;  
                else throw new GekkoException("Error regarding frequencies");

                int subs = (this.sub - 1) + addedPeriods; //a lot easier if first converting from quarters 1,2,3,4 into 0,1,2,3
                int supers = subs / subPeriods;  //divisor
                int subs2 = subs % subPeriods;  //modulo

                if (subs2 < 0)
                {
                    //the C# modulo is a bit strange, since it allows negative modulo's
                    //well, if so: it is corrected into a positive modulo (subtracting 1 from the divisor)
                    supers--;
                    subs2 += subPeriods;
                }
                return new GekkoTime(this.freq, this.super + supers, subs2 + 1, false);  //+1: just the same one that was subtracted at the start. 'false': call the fast constructor.
            }
        }

        public override string ToString()  //can just as well implement it, better than nasty surprises with object ToString()
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (this.IsNull()) return "[unknown]";
            if (this.freq == EFreq.A)
            {
                if (super >= Globals.timeStringsStart && super <= Globals.timeStringsEnd)
                {
                    return Globals.timeStrings[super - Globals.timeStringsStart];  //faster, and works like a string cache
                }
                return super.ToString();
            }
            else if (this.freq == EFreq.Q)
            {
                return super + "q" + sub;
            }
            else if (this.freq == EFreq.M)
            {
                return super + "m" + sub;
            }
            else if (this.freq == EFreq.D)
            {
                return super + "m" + sub + "d" + subsub;
            }
            else if (this.freq == EFreq.U)  
            {
                return "" + super;  
            }
            else
            {
                G.Writeln2("*** ERROR: Problem with freq");
                throw new GekkoException();
            }
        }

        public static int FromQuarterToMonthStart(int q)
        {
            return (GekkoTimeStuff.numberOfMonths / GekkoTimeStuff.numberOfQuarters) * q - 2;          
        }

        public static int FromQuarterToMonthEnd(int q)
        {
            return (GekkoTimeStuff.numberOfMonths / GekkoTimeStuff.numberOfQuarters) * q;
        }

        public static int FromMonthToQuarter(int m)
        {
            return (m - 1) / (GekkoTimeStuff.numberOfMonths / GekkoTimeStuff.numberOfQuarters) + 1;
        }
    }

    public class GekkoTimeIterator : IEnumerable<GekkoTime>
    {
        private GekkoTime _StartDate;
        private GekkoTime _EndDate;
        private EFreq _freq;

        public GekkoTimeIterator(Tuple<GekkoTime, GekkoTime> tuple) : this(tuple.Item1, tuple.Item2) { }       

        public GekkoTimeIterator(GekkoTime startDate, GekkoTime endDate)
        {
            _StartDate = startDate;
            _EndDate = endDate;
            if (startDate.freq != endDate.freq)
            {
                G.Writeln2("*** ERROR: Mismatch of frequencies in time iterator");
                throw new GekkoException();
            }
            _freq = startDate.freq;
        }

        public IEnumerator<GekkoTime> GetEnumerator()
        {
            GekkoTime currentDate = _StartDate;
            do
            {
                yield return currentDate;
                currentDate = currentDate.Add(1);
            }
            while (!(currentDate.StrictlyLargerThan(_EndDate))); // Note that our Iterator is inclusive of endDate behaving like 'between'
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            G.Writeln("*** ERROR: iterator problem");
            throw new GekkoException();
        }
    }

    

    public class AllFreqsHelper
    {
        public GekkoTime t1Annual;
        public GekkoTime t2Annual;
        public GekkoTime t1Quarterly;
        public GekkoTime t2Quarterly;
        public GekkoTime t1Monthly;
        public GekkoTime t2Monthly;
        public GekkoTime t1Daily;
        public GekkoTime t2Daily;
        public GekkoTime t1Undated;
        public GekkoTime t2Undated;
    }

    public class GekkoTimeIteratorBackwards : IEnumerable<GekkoTime>
    {
        private GekkoTime _StartDate;
        private GekkoTime _EndDate;
        private EFreq _freq;

        public GekkoTimeIteratorBackwards(GekkoTime startDate, GekkoTime endDate)
        {
            _StartDate = startDate;
            _EndDate = endDate;
            if (startDate.freq != endDate.freq)
            {
                G.Writeln2("*** ERROR: Internal error, mismatch of frequencies");
                throw new GekkoException();
            }
            _freq = startDate.freq;
        }

        public IEnumerator<GekkoTime> GetEnumerator()
        {
            GekkoTime currentDate = _StartDate;
            do
            {
                yield return currentDate;
                currentDate = currentDate.Add(-1);
            }
            while (!(currentDate.StrictlySmallerThan(_EndDate))); // Note that our Iterator is inclusive of endDate behaving like 'between'
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            G.Writeln("*** ERROR: iterator problem");
            throw new GekkoException();
        }
    }

    [ProtoContract]
    //For use with VPRT, and fixing in timeseriesries
    public class GekkoTimeSpan
    {
        [ProtoMember(1)]
        public GekkoTime tStart;
        [ProtoMember(2)]
        public GekkoTime tEnd;
        [ProtoMember(3)]
        public int by = 1;

        public GekkoTimeSpan()
        {
            //only because of protobuf
        }

        public GekkoTimeSpan(GekkoTime t1, GekkoTime t2)
        {
            this.tStart = t1;
            this.tEnd = t2;
        }
        public GekkoTimeSpan(GekkoTime t1, GekkoTime t2, int by)
        {
            this.tStart = t1;
            this.tEnd = t2;
            this.by = by;
        }
    }

    [ProtoContract]    
    //For use with VPRT, and fixing in timeseries
    public class GekkoTimeSpans
    {
        public GekkoTimeSpans()
        {
            //only because of protobuf
        }

        [ProtoMember(1)]
        public List<GekkoTimeSpan> data = new List<GekkoTimeSpan>();

        public string ToString()
        {
            string s = null;
            foreach (GekkoTimeSpan gts in this.data)
            {
                if (gts.tStart.IsSamePeriod(gts.tEnd))
                {
                    s += gts.tStart.ToString() + ", ";
                }
                else
                {
                    s += gts.tStart.ToString() + "-" + gts.tEnd.ToString() + ", ";
                }
            }
            s = s.Substring(0, s.Length - ", ".Length);
            return s;
        }
    }
}
