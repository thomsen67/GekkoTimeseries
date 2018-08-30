using System;
using System.Collections.Generic;
using ProtoBuf;
using System.Linq;
using System.Text;

namespace Gekko
{
    public enum EFreq  //search for 'ttfreq' to see places with freq that should be enumerated
    {
        Annual,
        Quarterly,
        Monthly,
        Undated,      //also called 'u' in Eviews, called 'n' in TSP, but undated has no name in AREMOS (uses 'periodic')     
        None          //used to signal non-freq variable, for instance a VAL   
    }  
    
    public class GekkoTimeStuff
    {
        public static readonly int numberOfQuarters = 4;
        public static readonly int numberOfMonths = 12;
    }

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

        public static GekkoTime tNull = new GekkoTime(EFreq.Annual, -12345, 1);  //think of it as a 'null' object (but it is a struct)

        //Note: using "new GekkoTime()" without arguments is not intended to be used, even
        //      though it is valid to do. Such a struct cannot have its fields changed anyway, so
        //      it will be unusable.
        public GekkoTime(EFreq freq2, int super2, int sub2)
        {
            freq = freq2;
            super = (short)super2;
            sub = (short)sub2;
            //Sanity checks to follow
            //Problem is that TIME 2010m13 2012m0 can probably parse. If not, the check below is not necessary.            
            if (sub < 1)
            {
                G.Writeln2("*** ERROR: subperiod < 1");
                throw new GekkoException();
            }
            if (freq == EFreq.Annual)
            {
                if (sub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'a' cannot have subperiod > 1");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.Quarterly)
            {
                if (sub > 4)
                {
                    G.Writeln2("*** ERROR: freq 'q' cannot have subperiod > 4");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.Monthly)
            {
                if (sub > 12)
                {
                    G.Writeln2("*** ERROR: freq 'm' cannot have subperiod > 12");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq.Undated)
            {
                if (sub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'u' cannot have subperiod > 1");
                    throw new GekkoException();
                }
            }
            subsub = 0;
        }

        private GekkoTime(EFreq freq2, int super2, int sub2, bool check)
        {
            //The check argument is not actually used, just can call the method with 'false'.
            //This method is only used for looping and adding internally, not for creating
            //a fresh new GekkoTime from command input. Hence the method is private.
            freq = freq2;
            super = (short)super2;
            sub = (short)sub2;
            subsub = 0;
        }

        public bool IsNull()
        {
            if (this.super == -12345) return true;
            return false;
        }

        public static int Observations(GekkoTime t1, GekkoTime t2)
        {
            //BEWARE: Can return 0 or a negative number!
            //Also checks that freqs are the same
            if (t1.freq != t2.freq)
            {
                G.Writeln2("*** ERROR: Frequency mismatch: " + G.GetFreqString(t1.freq) + " vs. " + G.GetFreqString(t2.freq));
                throw new GekkoException();
            }
            EFreq efreq = t1.freq;
            int subPeriods = 1;
            if (efreq == EFreq.Annual)
            {
                //fast return
                return t2.super - t1.super + 1;  //Subpers are ignored. It is tacitly assumed that the subperiods are = 1 here, else this is nonsense
            }
            else if (efreq == EFreq.Quarterly) subPeriods = 4;
            else if (efreq == EFreq.Monthly) subPeriods = 12;
            else if (efreq == EFreq.Undated) subPeriods = 1;  //ttfreq
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

        public static void ConvertFreqs(EFreq freq, GekkoTime t1, GekkoTime t2, ref GekkoTime tt1, ref GekkoTime tt2)
        {
            tt1 = t1;
            tt2 = t2;

            if (t1.freq != t2.freq)
            {
                G.Writeln2("*** ERROR: Frequencies do not match: " + G.GetFreqString(t1.freq) + " vs. " + G.GetFreqString(t2.freq));
                throw new GekkoException();
            }

            if (freq == t1.freq)
            {
                //do nothing
            }
            else
            {

                if (freq == EFreq.Annual)
                {
                    if (t1.freq == EFreq.Quarterly)
                    {
                        tt1 = new GekkoTime(EFreq.Annual, t1.super, 1);  
                        tt2 = new GekkoTime(EFreq.Annual, t2.super, 1);  
                    }
                    else if (t1.freq == EFreq.Monthly)
                    {
                        tt1 = new GekkoTime(EFreq.Annual, t1.super, 1);  
                        tt2 = new GekkoTime(EFreq.Annual, t2.super, 1);  
                    }

                }
                else if (freq == EFreq.Quarterly)
                {
                    if (t1.freq == EFreq.Annual)
                    {
                        tt1 = new GekkoTime(EFreq.Quarterly, t1.super, 1);
                        tt2 = new GekkoTime(EFreq.Quarterly, t2.super, GekkoTimeStuff.numberOfQuarters);
                    }

                    else if (t1.freq == EFreq.Monthly)
                    {
                        tt1 = new GekkoTime(EFreq.Quarterly, t1.super, GekkoTime.FromMonthToQuarter(t1.sub));  //first m
                        tt2 = new GekkoTime(EFreq.Quarterly, t2.super, GekkoTime.FromMonthToQuarter(t2.sub));  //last m                 
                    }
                }
                else if (freq == EFreq.Monthly)
                {
                    if (t1.freq == EFreq.Annual)
                    {
                        tt1 = new GekkoTime(EFreq.Monthly, t1.super, 1);
                        tt2 = new GekkoTime(EFreq.Monthly, t2.super, GekkoTimeStuff.numberOfMonths);
                    }                
                    else if (t1.freq == EFreq.Quarterly)
                    {
                        tt1 = new GekkoTime(EFreq.Monthly, t1.super, GekkoTime.FromQuarterToMonthStart(t1.sub));
                        tt2 = new GekkoTime(EFreq.Monthly, t2.super, GekkoTime.FromQuarterToMonthEnd(t2.sub));
                    }
                }
            }
        }

        public bool StrictlyLargerThan(GekkoTime gt2)
        {
            CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (this.super > gt2.super) return true;
            else if (this.super == gt2.super)
            {
                if (this.sub > gt2.sub) return true;
            }
            return false;
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

        public bool LargerThanOrEqual(GekkoTime gt2)
        {
            CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (this.super == gt2.super && this.sub == gt2.sub) return true;
            if (StrictlyLargerThan(gt2)) return true;
            return false;
        }

        public bool SmallerThanOrEqual(GekkoTime gt2)
        {
            CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (this.super == gt2.super && this.sub == gt2.sub) return true;
            if (StrictlySmallerThan(gt2)) return true;
            return false;
        }

        public bool StrictlySmallerThan(GekkoTime gt2)
        {
            CheckSameFreq(gt2);
            if (gt2.IsNull()) return true;
            if (this.super < gt2.super) return true;
            else if (this.super == gt2.super)
            {
                if (this.sub < gt2.sub) return true;
            }
            return false;
        }

        public bool IsSamePeriod(GekkoTime gt2)
        {
            //will handle null ok, two null --> true
            CheckSameFreq(gt2);            
            if (this.super == gt2.super)
                if (this.sub == gt2.sub)
                    return true;
            return false;
        }

        public GekkoTime Add(int addedPeriods)
        {
            //seems to work for negative adds
            //see also GetPeriod()
            //Could probably use DateTime functions, but we like to keep it fast and simple

            if (addedPeriods == 0) return this;

            int subPeriods = 1;

            if (this.freq == EFreq.Annual)
            {
                //Simple: make it run fast!                
                return new GekkoTime(this.freq, this.super + addedPeriods, this.sub, false);  //call the fast constructor
            }
            else if (this.freq == EFreq.Quarterly) subPeriods = 4;
            else if (this.freq == EFreq.Monthly) subPeriods = 12;
            else if (this.freq == EFreq.Undated) subPeriods = 1;  //ttfreq
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

        public override string ToString()  //can just as well implement it, better than nasty surprises with object ToString()
        {
            if (this.IsNull()) return "[unknown]";
            if (this.freq == EFreq.Annual)
            {
                if (super >= Globals.timeStringsStart && super <= Globals.timeStringsEnd)
                {
                    return Globals.timeStrings[super - Globals.timeStringsStart];  //faster, and works like a string cache
                }
                return super.ToString();
            }
            else if (this.freq == EFreq.Quarterly)
            {
                return super + "q" + sub;
            }
            else if (this.freq == EFreq.Monthly)
            {
                return super + "m" + sub;
            }
            else if (this.freq == EFreq.Undated)  //ttfreq
            {
                return "" + super;  //ttfreq
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

        public GekkoTimeIterator(GekkoTime startDate, GekkoTime endDate)
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

    //For use with VPRT
    public class GekkoTimeSpan
    {
        public GekkoTime tStart;
        public GekkoTime tEnd;
        public int by = 1;
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

    //For use with VPRT
    public class GekkoTimeSpans
    {
        public List<GekkoTimeSpan> data = new List<GekkoTimeSpan>();
    }
}
