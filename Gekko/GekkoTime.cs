using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Linq;
using System.Text;

namespace Gekko
{
    
    public class FreqHelper
    {
        public GekkoTime t1;
        public int offset = 0;
    }

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
        W,        //weekly
        Empty3,   // --------> this and the following can be filled/changed
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

    public enum EHolidayName
    {
        Null,  //null
        New_Years_Day, //Nytaarsdag
        Leap_Day, //Skuddag
        Maundy_Thursday, //Skaertorsdag
        Good_Friday, //Langfredag
        Easter_Sunday, //Paaskedag
        Easter_Monday, //Anden_paaskedag
        Labour_Day, //Foerste_maj
        Constitution_Day, //Grundlovsdag
        General_Prayer_Day, //Store_bededag
        Ascension_Day, //Kristi_himmelfartsdag
        Whit_Sunday, //Pinsedag
        Whit_Monday, //Anden_pinsedag
        Christmas_Eve, //Juleaften
        Christmas_Day, //Foerste_juledag
        Boxing_Day, //Anden_juledag
        New_Years_Eve, //Nytaarsaften
    }

    public static class Extensions
    {
        /// <summary>
        /// Extension method for EFreq enum, pretty "printing" the freq. Convert to pretty string like "Annual", "Quarterly" etc.
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static string Pretty(this EFreq freq)        {

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            string f = "";
            if (freq == EFreq.A)
            {
                f = "Annual";
            }
            else if (freq == EFreq.Q)
            {
                f = "Quarterly";
            }
            else if (freq == EFreq.M)
            {
                f = "Monthly";
            }
            else if (freq == EFreq.W)
            {
                f = "Weekly";
            }
            else if (freq == EFreq.D)
            {
                f = "Daily";
            }
            else if (freq == EFreq.U)
            {
                f = "Undated";
            }
            else
            {
                new Error("Strange error regarding freq");
            }
            return f;
        }
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
        public static readonly int numberOfDaysInAWeek = 7;
    }

    [Serializable]
    [ProtoContract]
    //GekkoTime is an immutable struct for fast looping. Structs should be < 16 bytes to be effective (we have 3 x 4 = 12 bytes here)
    public struct GekkoTime : IComparable<GekkoTime>
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

        public static GekkoTime tNull = new GekkoTime(EFreq.A, -12345, 1);  //DO NOT CHANGE NAME! Think of it as a 'null' object (but it is a struct)
        public static GekkoTime tNotNull = new GekkoTime(EFreq.A, -23456, 1);  //DO NOT CHANGE NAME! Think of it as 'yes' (for instance in PRT <i>... which is different from PRT <i=2010q3>).
        public static DateTime unixTimeOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private static List<HolidayNames> holidayNames = null;  //use HolidayNames!!
        public static List<HolidayNames> HolidayNames
        {
            get
            {
                if (GekkoTime.holidayNames != null) return GekkoTime.holidayNames;
                List<HolidayNames> holidays = new List<HolidayNames>();
                holidays.Add(new HolidayNames(EHolidayName.New_Years_Day, "New_Years_Day", "Nytaarsdag"));
                holidays.Add(new HolidayNames(EHolidayName.Leap_Day, "Leap_Day", "Skuddag"));
                holidays.Add(new HolidayNames(EHolidayName.Maundy_Thursday, "Maundy_Thursday", "Skaertorsdag"));
                holidays.Add(new HolidayNames(EHolidayName.Good_Friday, "Good_Friday", "Langfredag"));
                holidays.Add(new HolidayNames(EHolidayName.Easter_Sunday, "Easter_Sunday", "Paaskedag"));
                holidays.Add(new HolidayNames(EHolidayName.Easter_Monday, "Easter_Monday", "Anden_paaskedag"));
                holidays.Add(new HolidayNames(EHolidayName.Labour_Day, "Labour_Day", "Foerste_maj"));
                holidays.Add(new HolidayNames(EHolidayName.General_Prayer_Day, "General_Prayer_Day", "Store_bededag"));
                holidays.Add(new HolidayNames(EHolidayName.Ascension_Day, "Ascension_Day", "Kristi_himmelfartsdag"));
                holidays.Add(new HolidayNames(EHolidayName.Whit_Sunday, "Whit_Sunday", "Pinsedag"));
                holidays.Add(new HolidayNames(EHolidayName.Whit_Monday, "Whit_Monday", "Anden_pinsedag"));
                holidays.Add(new HolidayNames(EHolidayName.Constitution_Day, "Constitution_Day", "Grundlovsdag"));
                holidays.Add(new HolidayNames(EHolidayName.Christmas_Eve, "Christmas_Eve", "Juleaften"));
                holidays.Add(new HolidayNames(EHolidayName.Christmas_Day, "Christmas_Day", "Foerste_juledag"));
                holidays.Add(new HolidayNames(EHolidayName.Boxing_Day, "Boxing_Day", "Anden_juledag"));
                holidays.Add(new HolidayNames(EHolidayName.New_Years_Eve, "New_Years_Eve", "Nytaarsaften"));
                GekkoTime.holidayNames = holidays;
                return holidays;
            }
        }

        private static List<WeekDayNames> weekdayNames = null;  //use WeekdayNames!!
        public static List<WeekDayNames> WeekdayNames
        {
            get
            {
                if (GekkoTime.weekdayNames != null) return GekkoTime.weekdayNames;
                List<WeekDayNames> weekdays = new List<WeekDayNames>();
                weekdays.Add(new WeekDayNames(1, "Monday", "Mandag"));
                weekdays.Add(new WeekDayNames(2, "Tuesday", "Tirsdag"));
                weekdays.Add(new WeekDayNames(3, "Wednesday", "Onsdag"));
                weekdays.Add(new WeekDayNames(4, "Thursday", "Torsdag"));
                weekdays.Add(new WeekDayNames(5, "Friday", "Fredag"));
                weekdays.Add(new WeekDayNames(6, "Saturday", "Lørdag"));
                weekdays.Add(new WeekDayNames(7, "Sunday", "Søndag"));
                GekkoTime.weekdayNames = weekdays;
                return weekdays;
            }
        }

        private static List<MonthNames> monthNames = null;  //use WeekdayNames!!
        public static List<MonthNames> MonthNames
        {
            get
            {
                if (GekkoTime.monthNames != null) return GekkoTime.monthNames;
                GekkoTime.monthNames = new List<MonthNames>();
                GekkoTime.monthNames.Add(new MonthNames(1, "January", "Januar"));
                GekkoTime.monthNames.Add(new MonthNames(2, "February", "Februar"));
                GekkoTime.monthNames.Add(new MonthNames(3, "March", "Marts"));
                GekkoTime.monthNames.Add(new MonthNames(4, "April", "April"));
                GekkoTime.monthNames.Add(new MonthNames(5, "May", "Maj"));
                GekkoTime.monthNames.Add(new MonthNames(6, "June", "Juni"));
                GekkoTime.monthNames.Add(new MonthNames(7, "July", "Juli"));
                GekkoTime.monthNames.Add(new MonthNames(7, "August", "August"));
                GekkoTime.monthNames.Add(new MonthNames(7, "September", "September"));
                GekkoTime.monthNames.Add(new MonthNames(7, "October", "Oktober"));
                GekkoTime.monthNames.Add(new MonthNames(7, "November", "November"));
                GekkoTime.monthNames.Add(new MonthNames(7, "December", "December"));
                return GekkoTime.monthNames;
            }
        }

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
                    new Error("Freq 'a' cannot have subperiod > 1");
                }
            }
            else if (freq == EFreq.Q)
            {
                if (sub < 1 || sub > 4 || subsub > 1)
                {
                    new Error("Freq 'q' wrong quarter: " + sub);
                }
            }
            else if (freq == EFreq.M)
            {
                if (sub < 1 || sub > 12 || subsub > 1)
                {
                    new Error("Freq 'm' wrong month: " + sub);
                }
            }
            else if (freq == EFreq.W)
            {
                if (subsub > 1) new Error("Internal error #78745728309472");
                G.CheckWeekNumberAndMaybePrintErrorMessage(this.super, this.sub, true);
            }
            else if (freq == EFreq.D)
            {
                if (sub < 1 || sub > 12)
                {
                    new Error("Freq 'd' wrong month: " + sub);
                }
                int maxDays = GekkoTime.DaysInMonth(super, sub);  //see also #9832453429857
                if (subsub < 1 || subsub > maxDays)
                {
                    new Error("Freq 'd' wrong day: " + subsub);
                }
            }
            else if (freq == EFreq.U)
            {
                if (sub > 1 || subsub > 1)
                {
                    new Error("Freq 'u' cannot have subperiod > 1");
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

        public static int FromDateTimeToUnixDays(DateTime t)
        {
            //note this possibility for seconds: long unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
            int days = (t - GekkoTime.unixTimeOrigin).Days;
            return days;
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
            else if (freq == EFreq.W)
            {
                return ISOWeek.ToGekkoTime(dt);
            }
            else if (freq == EFreq.D)
            {
                sub = dt.Month;
                subsub = dt.Day;
            }
            else if (freq == EFreq.U)
            {
                new Error("Cannot convert date to undated frequency");
            }

            return new GekkoTime(freq, super, sub, subsub);
        }

        public static DateTime FromExcelDateToDateTime(double data)
        {
            return System.DateTime.FromOADate(data);
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

            int y = gt.super;  //Note: may be changed for W freq
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
                    else { m = 3; d = GekkoTime.DaysInMonth(y, m); }
                }
                else if (gt.sub == 2)
                {
                    if (first) { m = 4; d = 1; }
                    else { m = 6; d = GekkoTime.DaysInMonth(y, m); }
                }
                else if (gt.sub == 3)
                {
                    if (first) { m = 7; d = 1; }
                    else { m = 9; d = GekkoTime.DaysInMonth(y, m); }
                }
                else
                {
                    if (first) { m = 10; d = 1; }
                    else { m = 12; d = GekkoTime.DaysInMonth(y, m); }
                }
            }
            else if (gt.freq == EFreq.M)
            {
                if (format == null) f = "yyyy-mm";
                m = gt.sub;
                if (first) d = 1;
                else d = GekkoTime.DaysInMonth(y, m);
            }
            else if (gt.freq == EFreq.W)
            {
                if (format == null) f = "yyyy-ww-dd";
                if (first)
                {
                    DateTime dtw1 = ISOWeek.ToDateTime(gt, Globals.weeklyFirstDayWhenPrinting);
                    y = dtw1.Year;
                    m = dtw1.Month;
                    d = dtw1.Day;
                }
                else
                {
                    DateTime dtw2 = ISOWeek.ToDateTime(gt, Globals.weeklyLastDayWhenPrinting);
                    y = dtw2.Year;
                    m = dtw2.Month;
                    d = dtw2.Day;
                }
            }
            else if (gt.freq == EFreq.D)
            {
                if (format == null) f = "yyyy-mm-dd";
                m = gt.sub;
                d = gt.subsub;
            }
            else if (gt.freq == EFreq.U)
            {
                new Error("You cannot use dateformat together with an undated frequency");
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

            dt = GekkoTime.DateTime(y, m, d);
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
                dt = System.DateTime.ParseExact(s, format.ToLower().Replace("m", "M"), null);
            }
            catch (Exception e)
            {
                new Error("The date '" + s + "' does not comply with the format '" + format + "'");
            }
            return dt;
        }

        /// <summary>
        /// Converts from a string like "2020q2" or "2020m04d02" to a GekkoTime struct. Trailing "a" or "a1" or "u" or "u1" allowed. A string like "98" will be understood as 1998. Using "k" for quarters is not allowed, see overloads.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static GekkoTime FromStringToGekkoTime(string s)
        {
            return FromStringToGekkoTime(s, false);
        }

        /// <summary>
        /// Converts from a string like "2020q2" or "2020k2" or "2020m04d02" to a GekkoTime struct. Trailing "a" or "a1" or "u" or "u1" allowed. A string like "98" will be understood as 1998.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="allowKForQuartersAndUForWeeks"></param>
        /// <returns></returns>
        public static GekkoTime FromStringToGekkoTime(string s, bool allowKForQuartersAndUForWeeks)
        {
            return FromStringToGekkoTime(s, allowKForQuartersAndUForWeeks, true);
        }
        public static GekkoTime FromStringToGekkoTime(string s, bool allowKForQuartersAndUForWeeks, bool reportError)
        {
            return FromStringToGekkoTime(s, allowKForQuartersAndUForWeeks, reportError, true);
        }

        /// <summary>
        /// Converts from a string like "2020q2" or "2020k2" or "2020m04d02" to a GekkoTime struct. Trailing "a" or "a1" or "u" or "u1" allowed. With allowTwoDigits==true, a string like "98" will be understood as 1998.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="allowKForQuartersAndUForWeeks"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>
        public static GekkoTime FromStringToGekkoTime(string s, bool allowKForQuartersAndUForWeeks, bool reportError, bool allowTwoDigits)
        {
            //To do the reverse: see G.FromDateToString()

            //trailing a or a1 accepted for annual: 2001a, 2001a1
            //trailing u accepted for undated: 2001u, 2001u1
            //k{i} may be accepted for dates (allowK...)
            //u[i} may be accepted for dates (allowK...)
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
                    return new GekkoTime(EFreq.A, G.FindYear(i, allowTwoDigits), 1);
                }
            }

            if (s.EndsWith("a1", StringComparison.OrdinalIgnoreCase))
            {
                int i = -12345;
                bool b = int.TryParse(s.Substring(0, s.Length - 2), out i);
                if (b)
                {
                    return new GekkoTime(EFreq.A, G.FindYear(i, allowTwoDigits), 1);
                }
                else
                {
                    if (reportError)
                    {
                        new Error("Could not parse " + s + " as an annual date");
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
                    return new GekkoTime(EFreq.A, G.FindYear(i, allowTwoDigits), 1);
                }
                else
                {
                    if (reportError)
                    {
                        new Error("Could not parse " + s + " as an annual date");
                    }
                    else return GekkoTime.tNull;
                }
            }

            //now other freqs
            //now other freqs
            //now other freqs
                        
            bool weekU = allowKForQuartersAndUForWeeks && (s.Contains("u") || s.Contains("U")) && char.IsDigit(s[s.Length - 1]);

            if (s.Contains("q") || s.Contains("Q"))
            {
                string[] temp1 = s.Split(new char[] { 'q', 'Q' });
                int y1 = -12345;
                int q1 = -12345;
                try
                {
                    y1 = G.FindYear(int.Parse(temp1[0]), allowTwoDigits);
                    q1 = int.Parse(temp1[1]);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a quarterly date");
                    else return GekkoTime.tNull;
                }
                if (q1 < 1 || q1 > 4)
                {
                    if (reportError) new Error("Should have quarters from 1 to and including 4");
                    else return GekkoTime.tNull;
                }
                t = new GekkoTime(EFreq.Q, y1, q1);
            }
            else if (allowKForQuartersAndUForWeeks && (s.Contains("k") || s.Contains("K")))
            {
                string[] temp1 = s.Split(new char[] { 'k', 'K' });
                int y1 = -12345;
                int q1 = -12345;
                try
                {
                    y1 = G.FindYear(int.Parse(temp1[0]), allowTwoDigits);
                    q1 = int.Parse(temp1[1]);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a quarterly date");
                    else return GekkoTime.tNull;
                }
                if (q1 < 1 || q1 > 4)
                {
                    if (reportError) new Error("Should have quarters from 1 to and including 4");
                    else return GekkoTime.tNull;
                }
                t = new GekkoTime(EFreq.Q, y1, q1);
            }
            else if (s.Contains("d") || s.Contains("D"))  //must be before 'm'
            {

                int y1 = -12345;
                int m1 = -12345;
                int d = -12345;

                try
                {
                    string[] temp1 = s.Split(new char[] { 'm', 'M' });  //2019m12d24
                    string[] temp2 = temp1[1].Split(new char[] { 'd', 'D' });
                    y1 = G.FindYear(int.Parse(temp1[0]), allowTwoDigits);
                    m1 = int.Parse(temp2[0]);
                    d = int.Parse(temp2[1]);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a daily date");
                    else return GekkoTime.tNull;
                }

                if (m1 < 1 || m1 > 12)
                {
                    if (reportError) new Error("Should have months from 1 to and including 12");
                    else return GekkoTime.tNull;
                }

                int maxDays = GekkoTime.DaysInMonth(y1, m1); //see also #9832453429857
                if (d < 1 || d > maxDays)
                {
                    if (reportError) new Error("Illegal day in daily date");
                    else return GekkoTime.tNull;
                }
                t = new GekkoTime(EFreq.D, y1, m1, d);
            }
            else if (s.Contains("m") || s.Contains("M"))
            {

                int y1 = -12345;
                int m1 = -12345;
                string[] temp1 = s.Split(new char[] { 'm', 'M' });
                try
                {
                    y1 = G.FindYear(int.Parse(temp1[0]), allowTwoDigits);
                    m1 = int.Parse(temp1[1]);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a monthly date");
                    else return GekkoTime.tNull;
                }

                if (m1 < 1 || m1 > 12)
                {
                    if (reportError) new Error("Should have months from 1 to and including 12");
                    else return GekkoTime.tNull;
                }
                t = new GekkoTime(EFreq.M, y1, m1);
            }
            else if (s.Contains("w") || s.Contains("W"))
            {
                string[] temp1 = s.Split(new char[] { 'w', 'W' });
                int y1 = -12345;
                int w1 = -12345;
                try
                {
                    y1 = G.FindYear(int.Parse(temp1[0]), allowTwoDigits);
                    w1 = int.Parse(temp1[1]);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a weekly date");
                    else return GekkoTime.tNull;
                }
                bool error = G.CheckWeekNumberAndMaybePrintErrorMessage(y1, w1, reportError);
                if (error) return GekkoTime.tNull;  //if there is an error and reportError = true above, we will not get here.                
                t = new GekkoTime(EFreq.W, y1, w1);
            }
            else if (weekU)
            {
                string[] temp1 = s.Split(new char[] { 'u', 'U' });
                int y1 = -12345;
                int w1 = -12345;
                try
                {
                    y1 = G.FindYear(int.Parse(temp1[0]), allowTwoDigits);
                    w1 = int.Parse(temp1[1]);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a weekly date");
                    else return GekkoTime.tNull;
                }
                bool error = G.CheckWeekNumberAndMaybePrintErrorMessage(y1, w1, reportError);
                if (error) return GekkoTime.tNull;  //if there is an error and reportError = true above, we will not get here.                
                t = new GekkoTime(EFreq.W, y1, w1);
            }
            else if (s.Contains("u") || s.Contains("U"))
            {
                string s2 = s;
                //u1 not allowed for undated anymore: can be confused with weeks...
                //if (s.EndsWith("u1", StringComparison.OrdinalIgnoreCase))
                //{
                //    s2 = s.Substring(0, s.Length - 2);
                //}
                if (s.EndsWith("u", StringComparison.OrdinalIgnoreCase))
                {
                    s2 = s.Substring(0, s.Length - 1);
                }

                try
                {
                    t = new GekkoTime(EFreq.U, int.Parse(s2), 1);
                }
                catch
                {
                    if (reportError) new Error("Could not parse " + s + " as a undated date");
                    else return GekkoTime.tNull;
                }
            }
            else
            {
                if (reportError) new Error("Could not parse the timeperiod: " + s);
                else return GekkoTime.tNull;
            }
            return t;
        }

        /// <summary>
        /// Tries to convert a scalar string/date/val into a GekkoTime. Reason why O.ConvertToDate() is not
        /// used is that it does not accept strings.
        /// </summary>
        /// <param name="iv2"></param>
        /// <returns></returns>
        public static GekkoTime FromIVariableToGekkoTime(IVariable iv2)
        {
            GekkoTime oneTime;
            if (iv2.Type() == EVariableType.String) oneTime = GekkoTime.FromStringToGekkoTime(iv2.ConvertToString());
            else oneTime = O.ConvertToDate(iv2);
            return oneTime;
        }

        public static DateTime FromGekkoTimeToDateTime(GekkoTime gt, O.GetDateChoices firstLast)
        {
            //See also #09834753425, converting from GekkoTime to GekkoTime

            if (gt.freq == EFreq.A)
            {
                if (firstLast == O.GetDateChoices.FlexibleStart)
                {
                    return GekkoTime.DateTime(gt.super, 1, 1);
                }
                else if (firstLast == O.GetDateChoices.FlexibleEnd)
                {
                    return GekkoTime.DateTime(gt.super, 12, 31);
                }
                else
                {
                    new Error("Freq convertion problem"); return System.DateTime.MinValue;
                }
            }
            else if (gt.freq == EFreq.Q)
            {
                if (firstLast == O.GetDateChoices.FlexibleStart)
                {
                    return GekkoTime.DateTime(gt.super, GekkoTime.FromQuarterToMonthStart(gt.sub), 1);
                }
                else if (firstLast == O.GetDateChoices.FlexibleEnd)
                {
                    int month = GekkoTime.FromQuarterToMonthEnd(gt.sub);
                    return GekkoTime.DateTime(gt.super, month, GekkoTime.DaysInMonth(gt.super, month));
                }
                else
                {
                    new Error("Freq convertion problem"); return System.DateTime.MinValue;
                }
            }
            else if (gt.freq == EFreq.M)
            {
                if (firstLast == O.GetDateChoices.FlexibleStart)
                {
                    return GekkoTime.DateTime(gt.super, gt.sub, 1);
                }
                else if (firstLast == O.GetDateChoices.FlexibleEnd)
                {
                    return GekkoTime.DateTime(gt.super, gt.sub, GekkoTime.DaysInMonth(gt.super, gt.sub));
                }
                else
                {
                    new Error("Freq convertion problem"); return System.DateTime.MinValue;
                }
            }
            else if (gt.freq == EFreq.D)
            {
                //for daily, firstLast has no effect
                return GekkoTime.DateTime(gt.super, gt.sub, gt.subsub);
            }
            else
            {
                new Error("Freq convertion problem"); return System.DateTime.MinValue;
            }            
        }

        /// <summary>
        /// Just makes sure an error message is issued if date makes no sense
        /// </summary>
        /// <returns></returns>
        public static DateTime DateTime(int year, int month, int day)
        {
            DateTime dt = System.DateTime.MinValue;
            try
            {
                dt = new DateTime(year, month, day);
            }
            catch
            {
                Globals.numberOfDateErrors++;
                new Error("Invalid date encountered"); //we do not show this: it is typically with -12345 which would just confuse users. : year = " + year + ", month = " + month + ", day = " + day + ". Note that years < 1 or > 9999 are invalid.");                
                throw;  //probably not necessary
            }
            return dt;
        }

        /// <summary>
        /// How many days does a certain month contain
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int DaysInMonth(int y, int m)
        {
            if (y >= 1 && y <= 9999)
            {
                return System.DateTime.DaysInMonth(y, m);
            }
            else
            {
                return 30;  //does not make any sense anyway, but undated freq can have periods outside 1..9999
            }
        }


        /// <summary>
        /// Subtract a t, returns integer. For annual freq, the subtraction corresponds to "normal" integer subtraction, for instance 2020 with subtract 2015 is = 5.
        /// The method Observations() is very similar.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Subtract(GekkoTime t)
        {
            return Observations(t, this) - 1;
        }

        /// <summary>
        /// Finds the number of observations that a time range t1 to t2 spans. If t1 = 2020 and t2 = 2022, it will return 3. So note that it is the difference + 1.
        /// The method *can* return 0 or a negative number! If both GekkoTimes are null, 0 is returned. If one but not the other is null, an error is issued.
        /// The method Subtract() is very similar.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static int Observations(GekkoTime t1, GekkoTime t2)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //BEWARE: Can return 0 or a negative number!
            //Also checks that freqs are the same and are not null.
            //Returns 0 if both are null.

            if (t1.freq != t2.freq)
            {
                new Error("Frequency mismatch in observations(): " + t1.freq.Pretty() + " vs. " + t2.freq.Pretty());
            }

            if ((t1.IsNull() && !t2.IsNull()) || ((!t1.IsNull() && t2.IsNull())))
            {
                new Error("Trying to compute the number of observations between two dates, where one is null and the other is not null.");
            }

            EFreq efreq = t1.freq;

            if (efreq == EFreq.A)
            {
                //for a fast return in case of A freq
                return t2.super - t1.super + 1;  //Subpers are ignored. It is tacitly assumed that the subperiods are = 1 here, else this is nonsense
            }
            else if (efreq == EFreq.D)
            {
                //see also #98032743029847
                if (t1.super < 1 || t1.super > 9999 || t2.super < 1 || t2.super > 9999)
                {
                    return int.MaxValue;  //does not make any sense anyhow
                }
                else
                {
                    DateTime dt1 = GekkoTime.DateTime(t1.super, t1.sub, t1.subsub);
                    DateTime dt2 = GekkoTime.DateTime(t2.super, t2.sub, t2.subsub);
                    return (dt2 - dt1).Days + 1;
                }
            }
            else if (efreq == EFreq.W)
            {
                if (t1.super < 1 || t1.super > 9999 || t2.super < 1 || t2.super > 9999)
                {
                    return int.MaxValue;  //does not make any sense anyhow
                }
                else
                {
                    //Could be done with knowledge of maxweeks in year and some modulo.
                    //But this is probably more robust, and a little slower.
                    DateTime dt1 = ISOWeek.ToDateTime(t1, DayOfWeek.Monday);  //could be any day...
                    DateTime dt2 = ISOWeek.ToDateTime(t2, DayOfWeek.Monday);  //as long as the same
                    //dt1 and dt2 are now both Mondays
                    return (dt2 - dt1).Days / GekkoTimeStuff.numberOfDaysInAWeek + 1;
                }
            }
            else
            {
                int subPeriods = 1;
                if (efreq == EFreq.Q) subPeriods = 4;
                else if (efreq == EFreq.M) subPeriods = 12;
                else if (efreq == EFreq.U) subPeriods = 1;
                else
                {
                    new Error("Error regarding frequency");
                }
                int obs = subPeriods * (t2.super - t1.super) + t2.sub - t1.sub + 1;  //can be 0 or negative                
                return obs;
            }
        }

        /// <summary>
        /// Central method regarding conversion of frequencies.
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="tt1"></param>
        /// <param name="tt2"></param>
        public static void ConvertFreqs(EFreq freq, GekkoTime t1, GekkoTime t2, ref GekkoTime tt1, ref GekkoTime tt2)
        {
            tt1 = ConvertFreqsFirst(freq, t1, null);
            tt2 = ConvertFreqsLast(freq, t2);
        }

        /// <summary>
        /// Central method regarding conversion of frequencies (here using GekkoSmplSimple).
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="tt1"></param>
        /// <param name="tt2"></param>
        public static GekkoSmplSimple ConvertFreqs(EFreq freq, GekkoSmplSimple input)
        {
            return new GekkoSmplSimple(ConvertFreqsFirst(freq, input.t1, null), ConvertFreqsLast(freq, input.t2));
        }

        /// <summary>
        /// Convert from a GekkoTime in one frequency to another frequency (possibly use start of period). Cf. ConvertFreqsLast().
        /// The freqHelper object is only for offsetting and the lag problem (can be null).
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="t"></param>
        /// <param name="freqHelper"></param>
        /// <returns></returns>
        public static GekkoTime ConvertFreqsFirst(EFreq freq, GekkoTime t, FreqHelper freqHelper)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //See also #09834753425, converting from GekkoTime to DateTime

            GekkoTime tt = t;

            int offset = 0;
            if (freqHelper != null)
            {
                //This object is only <> null when we are converting smpl.t0 to some foreign frequency.
                //So conversion of smpl.t1 is exempt from the following code.
                //For instance, if smpl.t0 = 2020m9 and smpl.t1 = 2020m12, this corresponds to
                //to 3 lags to account for "the lag problem", for instance in y!a = pch(x!a + 0), where the pch()
                //triggers a lag (and 2 lags are always put in), but where the global frequency is set to for instance monthly.
                //Naively, converting smpl.t0 til annual would become 2020, and the 3 lags would disappear.
                //So instead, when there is frequency conversion for smpl.t0, we transform .t1 (not .t0) to annual (result is still 2020), and
                //then subtract 3 periods from this annual date.
                //Below, we start setting tt = freqHelper.t1, and then do the offset at the very end.
                //
                //All this could be handled more elegantly if .t0 was an offset instead of a date.
                tt = freqHelper.t1;
            }
            
            if (freq == t.freq)
            {
                //do nothing
            }
            else
            {

                if (freq == EFreq.A)
                {
                    //From Q or M or D to A freq is just the annual part of the date. W is less simple.

                    if (t.freq == EFreq.Q)
                    {
                        tt = new GekkoTime(EFreq.A, t.super, 1);
                    }
                    else if (t.freq == EFreq.M || t.freq == EFreq.D)
                    {
                        tt = new GekkoTime(EFreq.A, t.super, 1);
                    }
                    else if (t.freq == EFreq.W)
                    {                        
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Monday);
                        tt = new GekkoTime(EFreq.A, dt.Year, 1);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.A, t.super, 1);
                    }

                }
                else if (freq == EFreq.Q)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to Q sets q1 for start year and q4 for end year
                        tt = new GekkoTime(EFreq.Q, t.super, 1);
                    }
                    else if (t.freq == EFreq.M || t.freq == EFreq.D)
                    {
                        //from M or D to Q finds corresponding Q
                        tt = new GekkoTime(EFreq.Q, t.super, GekkoTime.FromMonthToQuarter(t.sub));  //first m                        
                    }
                    else if (t.freq == EFreq.W)
                    {
                        //from W to Q
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Monday);
                        tt = new GekkoTime(EFreq.Q, dt.Year, GekkoTime.FromMonthToQuarter(dt.Month));
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.Q, t.super, 1);
                    }
                }
                else if (freq == EFreq.M)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to M sets m1 for start year, and m12 for end year
                        tt = new GekkoTime(EFreq.M, t.super, 1);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        //from Q to M sets mx for start q, and my for end q
                        tt = new GekkoTime(EFreq.M, t.super, GekkoTime.FromQuarterToMonthStart(t.sub));
                    }
                    else if (t.freq == EFreq.D)
                    {
                        //from D to M sets month directly
                        tt = new GekkoTime(EFreq.M, t.super, t.sub);
                    }
                    else if (t.freq == EFreq.W)
                    {
                        //from W to M
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Monday);
                        tt = new GekkoTime(EFreq.M, dt.Year, dt.Month);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.M, t.super, 1);
                    }
                }




                //WEEKLY START

                else if (freq == EFreq.W)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to W                        
                        tt = ISOWeekFirstDayInYear(t.super);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        //from Q to W
                        DateTime dt = GekkoTime.DateTime(t.super, GekkoTime.FromQuarterToMonthStart(t.sub), 1);
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                    else if (t.freq == EFreq.M)
                    {
                        //from M to W 
                        DateTime dt = GekkoTime.DateTime(t.super, t.sub, 1);
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                    else if (t.freq == EFreq.D)
                    {
                        //from D to W
                        DateTime dt = GekkoTime.DateTime(t.super, t.sub, t.subsub);
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        //from U to W
                        DateTime dt = GekkoTime.DateTime(t.super, 1, 1);
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                }

                //WEEKLY END



                else if (freq == EFreq.D)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to D sets first day
                        tt = new GekkoTime(EFreq.D, t.super, 1, 1);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        //from Q to D sets first day
                        tt = new GekkoTime(EFreq.D, t.super, GekkoTime.FromQuarterToMonthStart(t.sub), 1);
                    }
                    else if (t.freq == EFreq.M)
                    {
                        //from M to D sets first day
                        tt = new GekkoTime(EFreq.D, t.super, t.sub, 1);
                    }
                    else if (t.freq == EFreq.W)
                    {
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Monday);
                        tt = new GekkoTime(EFreq.D, dt.Year, dt.Month, dt.Day);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.D, t.super, 1, 1);
                    }
                }
                else if (freq == EFreq.U)
                {
                    //From Q or M to U freq is just the annual part of the date

                    if (t.freq == EFreq.A)
                    {
                        tt = new GekkoTime(EFreq.U, t.super, 1);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        tt = new GekkoTime(EFreq.U, t.super, 1);
                    }
                    else if (t.freq == EFreq.M || t.freq == EFreq.D)
                    {
                        tt = new GekkoTime(EFreq.U, t.super, 1);
                    }
                    else if (t.freq == EFreq.W)
                    {
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Monday);
                        tt = new GekkoTime(EFreq.U, dt.Year, 1);
                    }
                }
            }

            if (freqHelper != null)
            {
                tt = tt.Add(-freqHelper.offset);
            }

            return tt;
        }        

        /// <summary>
        /// Convert from a GekkoTime in one frequency to another frequency (possibly use start of period). Cf. ConvertFreqsFirst().
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static GekkoTime ConvertFreqsLast(EFreq freq, GekkoTime t)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            GekkoTime tt = t;

            if (freq == t.freq)
            {
                //do nothing
            }
            else
            {

                if (freq == EFreq.A)
                {
                    //From Q or M to A freq is just the annual part of the date. W is less simple.

                    if (t.freq == EFreq.Q)
                    {
                        tt = new GekkoTime(EFreq.A, t.super, 1);
                    }
                    else if (t.freq == EFreq.M || t.freq == EFreq.D)
                    {
                        tt = new GekkoTime(EFreq.A, t.super, 1);
                    }
                    else if (t.freq == EFreq.W)
                    {
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Sunday);
                        tt = new GekkoTime(EFreq.A, dt.Year, 1);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.A, t.super, 1);
                    }

                }
                else if (freq == EFreq.Q)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to Q sets q1 for start year and q4 for end year                        
                        tt = new GekkoTime(EFreq.Q, t.super, GekkoTimeStuff.numberOfQuarters);
                    }
                    else if (t.freq == EFreq.M || t.freq == EFreq.D)
                    {
                        //from M to Q finds corresponding Q                        
                        tt = new GekkoTime(EFreq.Q, t.super, GekkoTime.FromMonthToQuarter(t.sub));  //last m                 
                    }
                    else if (t.freq == EFreq.W)
                    {
                        //from W to Q
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Sunday);
                        tt = new GekkoTime(EFreq.Q, dt.Year, GekkoTime.FromMonthToQuarter(dt.Month));
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.Q, t.super, GekkoTimeStuff.numberOfQuarters);
                    }
                }
                else if (freq == EFreq.M)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to M sets m1 for start year, and m12 for end year                        
                        tt = new GekkoTime(EFreq.M, t.super, GekkoTimeStuff.numberOfMonths);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        //from Q to M sets mx for start q, and my for end q                        
                        tt = new GekkoTime(EFreq.M, t.super, GekkoTime.FromQuarterToMonthEnd(t.sub));
                    }
                    else if (t.freq == EFreq.D)
                    {
                        //from D to M sets month directly
                        tt = new GekkoTime(EFreq.M, t.super, t.sub);
                    }
                    else if (t.freq == EFreq.W)
                    {
                        //from W to M
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Sunday);
                        tt = new GekkoTime(EFreq.M, dt.Year, dt.Month);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        tt = new GekkoTime(EFreq.M, t.super, GekkoTimeStuff.numberOfMonths);
                    }
                }



                //WEEKLY START

                else if (freq == EFreq.W)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to W                        
                        tt = ISOWeekLastDayInYear(t.super);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        //from Q to W
                        int month = GekkoTime.FromQuarterToMonthEnd(t.sub);
                        DateTime dt = GekkoTime.DateTime(t.super, month, GekkoTime.DaysInMonth(t.super, month));
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                    else if (t.freq == EFreq.M)
                    {
                        //from M to W                         
                        DateTime dt = GekkoTime.DateTime(t.super, t.sub, GekkoTime.DaysInMonth(t.super, t.sub));
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                    else if (t.freq == EFreq.D)
                    {
                        //from D to W
                        DateTime dt = GekkoTime.DateTime(t.super, t.sub, t.subsub);
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        //from U to W                        
                        DateTime dt = GekkoTime.DateTime(t.super, GekkoTimeStuff.numberOfMonths, 31);
                        tt = ISOWeek.ToGekkoTime(dt);
                    }
                }

                //WEEKLY END





                else if (freq == EFreq.D)
                {
                    if (t.freq == EFreq.A)
                    {
                        //from A to D sets last day of year
                        tt = new GekkoTime(EFreq.D, t.super, GekkoTimeStuff.numberOfMonths, 31);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        //from Q to D sets last day of quarter
                        int month = GekkoTime.FromQuarterToMonthEnd(t.sub);
                        tt = new GekkoTime(EFreq.D, t.super, month, GekkoTime.DaysInMonth(t.super, month));
                    }
                    else if (t.freq == EFreq.M)
                    {
                        //from M to D sets last day of month                        
                        tt = new GekkoTime(EFreq.D, t.super, t.sub, GekkoTime.DaysInMonth(t.super, t.sub));
                    }
                    else if (t.freq == EFreq.W)
                    {
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Sunday);
                        tt = new GekkoTime(EFreq.D, dt.Year, dt.Month, dt.Day);
                    }
                    else if (t.freq == EFreq.U)
                    {
                        //from U to D sets last day of year
                        tt = new GekkoTime(EFreq.D, t.super, GekkoTimeStuff.numberOfMonths, 31);
                    }
                }
                else if (freq == EFreq.U)
                {
                    //From Q or M or D to U freq is just the annual part of the date

                    if (t.freq == EFreq.A)
                    {
                        tt = new GekkoTime(EFreq.U, t.super, 1);
                    }
                    else if (t.freq == EFreq.Q)
                    {
                        tt = new GekkoTime(EFreq.U, t.super, 1);
                    }
                    else if (t.freq == EFreq.M || t.freq == EFreq.D)
                    {
                        tt = new GekkoTime(EFreq.U, t.super, 1);
                    }
                    else if (t.freq == EFreq.W)
                    {
                        DateTime dt = ISOWeek.ToDateTime(t, DayOfWeek.Sunday);
                        tt = new GekkoTime(EFreq.U, dt.Year, 1);
                    }
                }
            }

            return tt;
        }

        /// <summary>
        /// Helper method to keeps this code together. Also used in ScalarVal class. The year of the returned GekkoTime
        /// is not necessarily the same is the input year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static GekkoTime ISOWeekFirstDayInYear(int year)
        {            
            DateTime dt = GekkoTime.DateTime(year, 1, 1);
            GekkoTime tt = ISOWeek.ToGekkoTime(dt);
            return tt;
        }

        /// <summary>
        /// Helper method to keeps this code together. Also used in ScalarVal class. The year of the returned GekkoTime
        /// is not necessarily the same is the input year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static GekkoTime ISOWeekLastDayInYear(int year)
        {            
            DateTime dt = GekkoTime.DateTime(year, GekkoTimeStuff.numberOfMonths, 31);
            GekkoTime tt = ISOWeek.ToGekkoTime(dt);
            return tt;
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
                    new Error("Incompatible freqs: " + G.ConvertFreq(this.freq) + " vs. " + G.ConvertFreq(gt2.freq));
                }
            }
        }

        // Regarding GekkoTime comparisons, remember that all these are False in C#:
        // In Gekko we have these:
        // (double.NaN > double.NaN)
        // (double.NaN > 100d)
        // (100d > double.NaN)
        // (double.NaN <double.NaN)
        // (double.NaN < 100d)
        // (100d < double.NaN)
        // (double.NaN == double.NaN)
        // (double.NaN == 100d)         
        // (100d == double.NaN)         

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
            if (gt2.IsNull()) return true;  //what if this.IsNull() is true??
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
                    //same year, compare weeks/months/quarters
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
            if (gt2.IsNull()) return true;  //what if this.IsNull() is true??
            if (EqualsGekkoTime(gt2, false)) return true;
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
            if (gt2.IsNull()) return true;  //what if this.IsNull() is true??
            if (EqualsGekkoTime(gt2, false)) return true;
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
            if (gt2.IsNull()) return true;  //what if this.IsNull() is true??
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
                    //same year, compare weeks/months/quarters
                    if (this.sub < gt2.sub) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// True if the two GekkoTimes are same period. Because of Equals() C# method, too dangerous to use the former IsSamePeriod()
        /// </summary>
        public bool EqualsGekkoTime(GekkoTime gt2)
        {
            return EqualsGekkoTime(gt2, true);
        }

        /// <summary>
        /// True if the two GekkoTimes are same period. Because of Equals() C# method, too dangerous to name it IsSamePeriod()
        /// </summary>
        public bool EqualsGekkoTime(GekkoTime gt2, bool check)
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

            if (this.freq == EFreq.A)
            {
                //Simple: make it run fast!                
                return new GekkoTime(this.freq, this.super + addedPeriods, this.sub, false);  //call the fast constructor
            }
            else if (this.freq == EFreq.D)
            {
                //see also #98032743029847
                DateTime dt1 = GekkoTime.DateTime(this.super, this.sub, this.subsub);
                DateTime dt2 = dt1.AddDays(addedPeriods);
                GekkoTime gt = GekkoTime.FromDateTimeToGekkoTime(this.freq, dt2);
                return gt;
            }
            else if (this.freq == EFreq.W)
            {
                //This could probably be done in a more efficient way, using knowledge of the number of
                //weeks in different years. But this is pretty simple and therefore stable!
                DateTime dt1 = ISOWeek.ToDateTime(this, DayOfWeek.Monday); //could be any weekday
                DateTime dt2 = dt1.AddDays(addedPeriods * GekkoTimeStuff.numberOfDaysInAWeek);
                GekkoTime gt = GekkoTime.FromDateTimeToGekkoTime(this.freq, dt2);
                return gt;
            }
            else
            {
                int subPeriods = 1;                
                if (this.freq == EFreq.Q) subPeriods = 4;
                else if (this.freq == EFreq.M) subPeriods = 12;
                else if (this.freq == EFreq.U) subPeriods = 1;
                else new Error("Error regarding frequencies");

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

        /// <summary>
        /// For t0=2001 and t1=2002, this will return "+1" (or "-1" if t1=2000). If same period, null is returned.
        /// </summary>
        /// <param name="t0"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetLagString(GekkoTime t0, GekkoTime t)
        {
            int lag = t.Subtract(t0);
            string slag = null;
            if (lag < 0) slag = lag.ToString();
            if (lag > 0) slag = "+" + lag.ToString();
            return slag;
        }

        /// <summary>
        /// Human-readable version for Gekko.
        /// </summary>
        /// <returns></returns>
        public override string ToString()  //can just as well implement it, better than nasty surprises with object ToString()
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (this.IsNull()) return "[null]";
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
            else if (this.freq == EFreq.W)
            {
                return super + "w" + sub;
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
                new Error("Problem with freq"); return null;
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

        public static void Convert12(GekkoSmpl smpl, EFreq desiredFreq, out GekkoTime t1, out GekkoTime t2)
        {
            t1 = smpl.t1;
            t2 = smpl.t2;
            if (t1.freq != desiredFreq || t2.freq != desiredFreq)
            {
                //for instance x!q = ... ; where global freq is !a
                t1 = GekkoTime.ConvertFreqsFirst(desiredFreq, t1, null);
                t2 = GekkoTime.ConvertFreqsLast(desiredFreq, t2);
            }
        }

        public static void Convert03(GekkoSmpl smpl, EFreq desiredFreq, out GekkoTime t0, out GekkoTime t3)
        {            
            t0 = smpl.t0; //is returned         
            t3 = smpl.t3; //is returned
            GekkoTime t1 = smpl.t1;

            if (t0.freq != desiredFreq || t3.freq != desiredFreq)
            {
                FreqHelper freqHelper = null;
                int offset = t1.Subtract(t0); //lag before conversion
                if (offset > 0)  //cannot be negative
                {
                    freqHelper = new FreqHelper();  //This object is not created if were are not using flexible freqs
                    freqHelper.offset = offset;
                    freqHelper.t1 = t1;
                }

                //for instance x!q = ... ; where global freq is !a
                t0 = GekkoTime.ConvertFreqsFirst(desiredFreq, t0, freqHelper);
                t3 = GekkoTime.ConvertFreqsLast(desiredFreq, t3);                               
            }
        }        

        public static GekkoTime GetSpecialDay(int year, EHolidayName name)
        {
            GekkoTime gt = GekkoTime.tNull;
            if (name == EHolidayName.New_Years_Day)
            {
                gt = new GekkoTime(EFreq.D, year, 1, 1);
            }
            else if (name == EHolidayName.Leap_Day)
            {
                if (System.DateTime.IsLeapYear(year))
                {
                    gt = new GekkoTime(EFreq.D, year, 2, 29);
                }
                else
                {
                    gt = GekkoTime.tNull;
                }
            }
            else if (name == EHolidayName.Maundy_Thursday)
            {
                gt = GetEaster(year).Add(-3);
            }
            else if (name == EHolidayName.Good_Friday)
            {
                gt = GetEaster(year).Add(-2);
            }
            else if (name == EHolidayName.Easter_Sunday)
            {
                gt = GetEaster(year);
            }
            else if (name == EHolidayName.Easter_Monday)
            {
                gt = GetEaster(year).Add(1);
            }
            else if (name == EHolidayName.Labour_Day)
            {
                gt = new GekkoTime(EFreq.D, year, 5, 1);
            }
            else if (name == EHolidayName.Constitution_Day)
            {
                gt = new GekkoTime(EFreq.D, year, 6, 5);
            }
            else if (name == EHolidayName.General_Prayer_Day)
            {
                gt = GetEaster(year).Add(5 + (7 * 3));                
            }
            else if (name == EHolidayName.Ascension_Day)
            {
                gt = GetEaster(year).Add(4 + (7 * 5));
            }
            else if (name == EHolidayName.Whit_Sunday)
            {
                gt = GetEaster(year).Add(7 * 7);
            }
            else if (name == EHolidayName.Whit_Monday)
            {
                gt = GetEaster(year).Add((7 * 7) + 1);
            }
            else if (name == EHolidayName.Christmas_Eve)
            {
                gt = new GekkoTime(EFreq.D, year, 12, 24);
            }
            else if (name == EHolidayName.Christmas_Day)
            {
                gt = new GekkoTime(EFreq.D, year, 12, 25);
            }
            else if (name == EHolidayName.Boxing_Day)
            {
                gt = new GekkoTime(EFreq.D, year, 12, 26);
            }
            else if (name == EHolidayName.New_Years_Eve)
            {
                gt = new GekkoTime(EFreq.D, year, 12, 31);
            }
            else
            {
                new Error("Unexpected error #jklhfs7af78f");
            }
            return gt;
        }

        public static GekkoTime GetSpecialDay(int year, string s)
        {            
            EHolidayName name = EHolidayName.Null;
            foreach (HolidayNames h in GekkoTime.HolidayNames)
            {
                if (G.Equal(s, h.en) || G.Equal(s, h.da))
                {
                    name = h.name;
                    break;
                }
            }

            if (name == EHolidayName.Null)
            {
                using (Error txt = new Error())
                {
                    int y = Program.ComputerYear();
                    txt.MainAdd("The holiday name '" + s + "' was not recognized. Click the link to see legal names.");
                    txt.MoreAdd("The following is a list of recognized holiday names (English and Danish). Dates are shown for the current year: these are not necessarily consequtive as shown here.");
                    txt.MoreNewLine();
                    foreach (HolidayNames h in GekkoTime.HolidayNames)
                    {                        
                        txt.MoreAdd($"{GetSpecialDay(y, h.name).ToString(),-15}" + $"{h.en,-25}" + $"{ h.da,-25}");
                        txt.MoreNewLineTight();
                    }
                }
            }

            return GetSpecialDay(year, name);
        }

        /// <summary>
        /// Internal method. Work out the date for Easter Sunday for specified year. Quite complicated.
        /// </summary>
        /// <param name="year">The year as an integer</param>
        /// <returns>Returns a datetime of Easter Sunday.</returns>
        private static GekkoTime GetEaster(int year)
        {
            //Taken from here: https://github.com/martinjw/Holiday/blob/efafc0d6d7f5ef56017f7d47b830a8d1c820de9b/src/PublicHoliday/HolidayCalculator.cs#L15
            //should be
            //Easter Monday  28 Mar 2005  17 Apr 2006  9 Apr 2007  24 Mar 2008
            //Oudin's Algorithm - http://www.smart.net/~mmontes/oudin.html      
            //Note: dates for Eastern Orthodox Church may be different!
            var g = year % 19;
            var c = year / 100;
            var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            var i = h - (h / 28) * (1 - (h / 28) * (29 / (h + 1)) * ((21 - g) / 11));
            var j = (year + year / 4 + i + 2 - c + c / 4) % 7;
            var p = i - j;
            var easterDay = 1 + (p + 27 + (p + 6) / 40) % 31;
            var easterMonth = 3 + (p + 26) / 30;
            return new GekkoTime(EFreq.D, year, easterMonth, easterDay);
        }

        // NOTE: It seems reflection is used and therefore Equals() checks the fields.
        //       Using default probably means use of reflection and boxing, so could be slow
        //       if used in a tight loop or something. For slower things probably ok.
        //public override bool Equals(Object obj)
        //{            
        //}
        //public override int GetHashCode()
        //{         
        //}

        public int CompareTo(GekkoTime other)
        {         
            if (this.EqualsGekkoTime(other)) return 0;
            else if (this.StrictlySmallerThan(other)) return -1;
            else return 1;
        }
    }

    public class GekkoTimeIterator : IEnumerable<GekkoTime>
    {
        private GekkoTime _StartDate;
        private GekkoTime _EndDate;
        private EFreq _freq;

        public GekkoTimeIterator(GekkoSmplSimple tuple) : this(tuple.t1, tuple.t2) { }        

        public GekkoTimeIterator(GekkoTime t1, GekkoTime t2)
        {
            _StartDate = t1;
            _EndDate = t2;
            if (t1.freq != t2.freq)
            {
                new Error("Mismatch of frequencies in time iterator");
            }
            _freq = t1.freq;
        }

        public GekkoTimeIterator(EFreq convertToThisFreq, GekkoSmplSimple tuple) : this(convertToThisFreq, tuple.t1, tuple.t2) { }

        public GekkoTimeIterator(EFreq convertToThisFreq, GekkoTime t1, GekkoTime t2)
        {
            //This is for the case where we want to "cast" to another freq than the freq of t1 and t2
            _StartDate = GekkoTime.ConvertFreqsFirst(convertToThisFreq, t1, null);
            _EndDate = GekkoTime.ConvertFreqsLast(convertToThisFreq, t2);
            _freq = convertToThisFreq;
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
            new Error("Iterator problem"); throw new GekkoException();  //must throw explicitly here, else C# whines.
        }
    }    

    public class AllFreqsHelper
    {
        //========================================================================================================
        //                          FREQUENCY LOCATION, indicates where to implement more frequencies
        //========================================================================================================

        public GekkoTime t1Annual;
        public GekkoTime t2Annual;
        public GekkoTime t1Quarterly;
        public GekkoTime t2Quarterly;
        public GekkoTime t1Monthly;
        public GekkoTime t2Monthly;
        public GekkoTime t1Weekly;
        public GekkoTime t2Weekly;
        public GekkoTime t1Daily;
        public GekkoTime t2Daily;
        public GekkoTime t1Undated;
        public GekkoTime t2Undated;

        public GekkoSmplSimple GetPeriods(EFreq desiredFreq)
        {
            GekkoTime t1 = GekkoTime.tNull;
            GekkoTime t2 = GekkoTime.tNull;
            if (desiredFreq == EFreq.U)
            {
                t1 = this.t1Undated;
                t2 = this.t2Undated;
            }
            else if (desiredFreq == EFreq.A)
            {
                t1 = this.t1Annual;
                t2 = this.t2Annual;
            }
            if (desiredFreq == EFreq.Q)
            {
                t1 = this.t1Quarterly;
                t2 = this.t2Quarterly;
            }
            if (desiredFreq == EFreq.M)
            {
                t1 = this.t1Monthly;
                t2 = this.t2Monthly;
            }
            if (desiredFreq == EFreq.W)
            {
                t1 = this.t1Weekly;
                t2 = this.t2Weekly;
            }
            if (desiredFreq == EFreq.D)
            {
                t1 = this.t1Daily;
                t2 = this.t2Daily;
            }
            return new GekkoSmplSimple(t1, t2);
        }
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
                new Error("Internal error, mismatch of frequencies");
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
            new Error("Iterator problem"); throw new GekkoException();  //must throw explicit exception, else C# whines
        }
    }

    [ProtoContract]
    //For use with VPRT, and fixing in timeseries
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
                if (gts.tStart.EqualsGekkoTime(gts.tEnd))
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

    [ProtoContract]
    public class GekkoTimeSpansSimple
    {
        [ProtoMember(1)]
        private List<GekkoTimeSpanSimple> storage = new List<GekkoTimeSpanSimple>();

        public GekkoTimeSpanSimple this[int i]
        {
            get { return this.storage[i]; }            
        }

        public int Count() 
        {
            return this.storage.Count;
        }

        public void Add(GekkoTimeSpanSimple gts)
        {
            //qwerty
            if (gts == null)
            {

            }
            this.storage.Add(gts);
        }

        public void AddRange(GekkoTimeSpansSimple gtss)
        {
            //qwerty
            foreach (GekkoTimeSpanSimple xx in gtss.GetStorage())
            {                
                if (xx == null)
                {

                }
            }
            
            this.storage.AddRange(gtss.storage);
        }

        /// <summary>
        /// Use for iterators only.
        /// </summary>
        /// <returns></returns>
        public List<GekkoTimeSpanSimple> GetStorage()
        {
            return this.storage;
        }

        public void SetStorage(List<GekkoTimeSpanSimple> input)
        {
            //qwerty
            foreach (GekkoTimeSpanSimple xx in input)
            {
                if (xx == null)
                {

                }
            }
            this.storage = input;
        }

        public override string ToString()
        {
            string s = null;
            foreach (GekkoTimeSpanSimple x in this.storage)
            {
                s += x.ToString() + ", ";
            }
            if (s.EndsWith(", ")) s = s.Substring(0, s.Length - ", ".Length);
            return s;
        }
    }


    [ProtoContract]
    /// <summary>
    /// For use with Trace. Like GekkoTime it is immutable, but it is not a struct. Should it be??
    /// </summary>
    public class GekkoTimeSpanSimple
    {
        [ProtoMember(1)]
        public readonly GekkoTime t1;
        [ProtoMember(2)]
        public readonly GekkoTime t2;

        public GekkoTimeSpanSimple()
        {
            //only because of protobuf
        }

        public GekkoTimeSpanSimple(GekkoTime t1, GekkoTime t2)
        {
            if (t1.IsNull() && t2.IsNull())
            {
                //ok, signifies no period(s)
            }
            else
            {
                if (t1.StrictlyLargerThan(t2)) new Error("Time span problem");
                if (t1.IsNull() || t2.IsNull()) new Error("Time span problem");
            }
            this.t1 = t1;
            this.t2 = t2;
        }

        public bool IsNull()
        {
            return this.t1.IsNull() && this.t2.IsNull();
        }

        public override string ToString()
        {
            return this.t1.ToString() + "-" + this.t2.ToString();
        }
    }

    
    /// <summary>
    /// Taken from .NET Core here: https://github.com/dotnet/runtime/blob/b41f1c5f2fde25d752d857a54c3af24145060cdd/src/libraries/System.Private.CoreLib/src/System/Globalization/ISOWeek.cs
    /// </summary>
    public static class ISOWeek
    {
        private const int WeeksInLongYear = 53;
        private const int WeeksInShortYear = 52;

        private const int MinWeek = 1;
        private const int MaxWeek = WeeksInLongYear;

        /// <summary>
        /// Convert from DateTime to a weekly GekkoTime that encompasses the DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static GekkoTime ToGekkoTime(DateTime dt)
        {
            GekkoTime gt = new GekkoTime(EFreq.W, GetYear(dt), GetWeekOfYear(dt));
            return gt;
        }        

        // The year parameter represents an ISO week-numbering year (also called ISO year informally).
        // Each week's year is the Gregorian year in which the Thursday falls.
        // The first week of the year, hence, always contains 4 January.
        // ISO week year numbering therefore slightly deviates from the Gregorian for some days close to 1 January.
        public static DateTime GetYearStart(int year)
        {
            GekkoTime gt = new GekkoTime(EFreq.W, year, MinWeek);
            return ToDateTime(gt, DayOfWeek.Monday);
        }

        // The year parameter represents an ISO week-numbering year (also called ISO year informally).
        // Each week's year is the Gregorian year in which the Thursday falls.
        // The first week of the year, hence, always contains 4 January.
        // ISO week year numbering therefore slightly deviates from the Gregorian for some days close to 1 January.
        public static DateTime GetYearEnd(int year)
        {
            GekkoTime gt = new GekkoTime(EFreq.W, year, GetWeeksInYear(year));
            return ToDateTime(gt, DayOfWeek.Sunday);
        }

        // From https://en.wikipedia.org/wiki/ISO_week_date#Weeks_per_year:
        //
        // The long years, with 53 weeks in them, can be described by any of the following equivalent definitions:
        //
        // - Any year starting on Thursday and any leap year starting on Wednesday.
        // - Any year ending on Thursday and any leap year ending on Friday.
        // - Years in which 1 January and 31 December (in common years) or either (in leap years) are Thursdays.
        //
        // All other week-numbering years are short years and have 52 weeks.
        public static int GetWeeksInYear(int year)
        {
            int year2 = G.FindYear(year);  //TTH change: also checks reasonable value            

            //static int P(int y) => (y + (y / 4) - (y / 100) + (y / 400)) % 7;  //TTH change: moved to method

            if (P(year2) == 4 || P(year2 - 1) == 3)
            {
                return WeeksInLongYear;
            }

            return WeeksInShortYear;
        }

        // From https://en.wikipedia.org/wiki/ISO_week_date#Calculating_a_date_given_the_year,_week_number_and_weekday:
        //
        // This method requires that one know the weekday of 4 January of the year in question.
        // Add 3 to the number of this weekday, giving a correction to be used for dates within this year.
        //
        // Multiply the week number by 7, then add the weekday. From this sum subtract the correction for the year.
        // The result is the ordinal date, which can be converted into a calendar date.
        //
        // If the ordinal date thus obtained is zero or negative, the date belongs to the previous calendar year.
        // If greater than the number of days in the year, to the following year.
        public static DateTime ToDateTime(GekkoTime gt, DayOfWeek dayOfWeek)
        {
            //TTH change: removed parameters int year and int week and put a GekkoTime instead.
            if (gt.IsNull()) new Error("ISOWeek:ToDateTime(): GekkoTime is set to null");
            if (gt.freq != EFreq.W) new Error("ISOWeek:ToDateTime(): GekkoTime must be W freq");
            int year = gt.super;            
            int year2 = G.FindYear(year);  //TTH change: also checks reasonable value            
            int week = gt.sub;

            G.CheckWeekNumberAndMaybePrintErrorMessage(year2, week, true);

            // We allow 7 for convenience in cases where a user already has a valid ISO
            // day of week value for Sunday. This means that both 0 and 7 will map to Sunday.
            // The GetWeekday method will normalize this into the 1-7 range required by ISO.
            if ((int)dayOfWeek < 0 || (int)dayOfWeek > 7)
            {                
                new Error("The day of week must be >=0 and <= 7"); //TTH change
            }

            var jan4 = GekkoTime.DateTime(year2, month: 1, day: 4);
            int correction = GetWeekday(jan4.DayOfWeek) + 3;
            int ordinal = (week * 7) + GetWeekday(dayOfWeek) - correction;
            return GekkoTime.DateTime(year2, month: 1, day: 1).AddDays(ordinal - 1);
        }

        private static int GetWeekOfYear(DateTime date)
        {
            //TTH changed: made private

            int week = GetWeekNumber(date);

            if (week < MinWeek)
            {
                // If the week number obtained equals 0, it means that the
                // given date belongs to the preceding (week-based) year.
                return GetWeeksInYear(date.Year - 1);
            }

            if (week > GetWeeksInYear(date.Year))
            {
                // If a week number of 53 is obtained, one must check that
                // the date is not actually in week 1 of the following year.
                return MinWeek;
            }

            return week;
        }

        /// <summary>
        /// May return y-1, y or y+1, where i is date.Year.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static int GetYear(DateTime date)
        {
            //TTH changed: made private

            int week = GetWeekNumber(date);

            if (week < MinWeek)
            {
                // If the week number obtained equals 0, it means that the
                // given date belongs to the preceding (week-based) year.
                return date.Year - 1;
            }

            if (week > GetWeeksInYear(date.Year))
            {
                // If a week number of 53 is obtained, one must check that
                // the date is not actually in week 1 of the following year.
                return date.Year + 1;
            }

            return date.Year;
        }

        // From https://en.wikipedia.org/wiki/ISO_week_date#Calculating_the_week_number_of_a_given_date:
        //
        // Using ISO weekday numbers (running from 1 for Monday to 7 for Sunday),
        // subtract the weekday from the ordinal date, then add 10. Divide the result by 7.
        // Ignore the remainder; the quotient equals the week number.
        //
        // If the week number thus obtained equals 0, it means that the given date belongs to the preceding (week-based) year.
        // If a week number of 53 is obtained, one must check that the date is not actually in week 1 of the following year.
        private static int GetWeekNumber(DateTime date)
        {
            return (date.DayOfYear - GetWeekday(date.DayOfWeek) + 10) / 7;
        }

        // Day of week in ISO is represented by an integer from 1 through 7, beginning with Monday and ending with Sunday.
        // This matches the underlying values of the DayOfWeek enum, except for Sunday, which needs to be converted.
        private static int GetWeekday(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 7 : (int)dayOfWeek;
        }

        private static int P(int y)
        {
            return (y + (y / 4) - (y / 100) + (y / 400)) % 7;  //TTH change: moved to method
        }
    }
}
