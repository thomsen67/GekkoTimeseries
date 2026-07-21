/* 
    Gekko Timeseries Software (www.t-t.dk/gekko). 
    Copyright (C) 2025, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class Databank : IBank
    {
        //Databanks: version 1.0 is tsd inside zip, version 1.1 is using protobuffers,
        //           version 1.2 is for Gekko 3.0.        

        //Note the .isDirty field, so methods that change anything must set isDirty = true!
        //Remember new fields in Clear() method and also in G.CloneDatabank()
        //
        [ProtoMember(1)]
        public GekkoDictionary<string, IVariable> storage;

        [ProtoMember(2)]
        public DatabankCacheParams cacheParameters = null;

        [ProtoMember(3)]
        public List<Trace2> traces = null; //when writing, this is where all the Trace's go.       

        public string name = null;

        //TODO:
        //TODO:
        //TODO: For now, these fileNameWithPath are a bit of a mess. In Gekko 4.0, clean it up.
        //TODO: But in general, FileNameWithPath may be a link to a temp file, whereas
        //TODO: FileNameWithPathPretty should "go through" zip paths etc. Beware that
        //TODO: FileNameWithPath is used for MD5.
        //TODO:
        //TODO:
        private string fileNameWithPath = null;  //will be constructed when reading: do not protobuf it        
        public string FileNameWithPath
        {
            //?????????
            //?????????
            //????????? Could this be avoided by means of testing boolean open == true/false, in Program.OpenOrRead()?
            //?????????
            //?????????
            get
            {
                return this.fileNameWithPath;
            }
            set
            {
                if (G.Equal(this.name, Globals.Work) || G.Equal(this.name, Globals.Ref))
                {
                    this.fileNameWithPath = value;  //overwrite filename with latest bank read or merged into Work/Ref
                }
                else
                {
                    //If the bank is not Work or Ref, it must have been opened with OPEN
                    //If there is no filename, put it in. But if there is a filename already, always keep it.
                    //  This may happen in the IMPORT here: OPEN<edit>bank; IMPORT<xlsx>data;
                    //  An IMPORT or READ statement should not alter the filename.
                    if (this.fileNameWithPath == null) this.fileNameWithPath = value;
                    else
                    {
                        //do nothing, keep the first filename encountered. This is the filename that the OPEN databank
                        //is tied to, and that it will be trying to write to when the bank is closed.
                    }
                }
            }
        }

        private string fileNameWithPathPretty = null;  //will be constructed when reading: do not protobuf it        
        public string FileNameWithPathPretty
        {
            //?????????
            //?????????
            //????????? Could this be avoided by means of testing boolean open == true/false, in Program.OpenOrRead()?
            //?????????
            //?????????
            get
            {
                return this.fileNameWithPathPretty;
            }
            set
            {
                if (G.Equal(this.name, Globals.Work) || G.Equal(this.name, Globals.Ref))
                {
                    this.fileNameWithPathPretty = value;  //overwrite filename with latest bank read or merged into Work/Ref
                }
                else
                {
                    //If the bank is not Work or Ref, it must have been opened with OPEN
                    //If there is no filename, put it in. But if there is a filename already, always keep it.
                    //  This may happen in the IMPORT here: OPEN<edit>bank; IMPORT<xlsx>data;
                    //  An IMPORT or READ statement should not alter the filename.
                    if (this.fileNameWithPathPretty == null) this.fileNameWithPathPretty = value;
                    else
                    {
                        //do nothing, keep the first filename encountered. This is the filename that the OPEN databank
                        //is tied to, and that it will be trying to write to when the bank is closed.
                    }
                }
            }
        }

        public bool save = true;  //Don't use protobuffer on this field.
        public int yearStart = -12345;  //only set when reading a bank, not afterwards if timeseries change. Not meant for making loops etc. or critical, only static information about the bank        
        public int yearEnd = -12345;  //only set when reading a bank, not afterwards if timeseries change. Not meant for making loops etc. or critical, only static information about the bank        
        public string info1 = null; //must be taken from DatabankInfo.xml, don't use protobuffer        
        public string date = null; //must be taken from DatabankInfo.xml, don't use protobuffer
        public bool isDirty = false;  //used to see if en OPEN databank must be re-written. Don't use protobuffer on this field.
        public bool editable = true;  //used to set an OPEN databank as editable. Don't use protobuffer on this field.        
        public string fileHash = null; //do not store this in protobuf
        public string databankVersion = null; //do not store this in protobuf

        private Databank()
        {
            //This is ONLY because protobuf-net needs it
            //without line below, protobuf probably crashes
            this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
        }

        public Databank(string name)
        {
            this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            this.name = name;
        }

        /// <summary>
        /// Not exactly clear why this is not just done manually when reading a protobuf Databank object. Well, if it works, don't fix it.
        /// It is done manually for parallel gbk read/write.
        /// </summary>
        [ProtoBeforeSerialization]
        public void BeforeProtobufWrite()
        {
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                Program.ProtobufWalker(kvp.Value, true);
            }
        }

        /// <summary>
        /// Not exactly clear why this is not just done manually after reading a protobuf Databank object. Well, if it works, don't fix it.
        /// It is done manually for parallel gbk read/write.
        /// </summary>
        [ProtoAfterDeserialization]
        public void AfterProtobufRead()
        {
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                Program.ProtobufWalker(kvp.Value, false);
            }
        }

        public GekkoDictionary<string, IVariable> StorageFlattenedArrayTimeseries()
        {
            GekkoDictionary<string, IVariable> rv = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                if (kvp.Value.Type() == EVariableType.Series)
                {
                    Series ts = kvp.Value as Series;
                    if (ts.type == ESeriesType.ArraySuper)
                    {
                        foreach (KeyValuePair<MultidimElement, IVariable> kvp2 in ts.dimensionsStorage.storage)
                        {
                            Series ts2 = kvp2.Value as Series;
                            rv.Add(ts2.GetName(), ts2);
                        }
                    }
                    else
                    {
                        rv.Add(kvp.Key, kvp.Value);
                    }
                }
                else
                {
                    rv.Add(kvp.Key, kvp.Value);
                }
            }
            return rv;
        }

        public void Clear()
        {
            if (!this.editable) Program.ProtectError("You cannot clear a non-editable databank, see OPEN<edit> or UNLOCK");
            this.yearStart = -12345;
            this.yearEnd = -12345;
            this.info1 = null;
            this.date = null;                                    
            this.traces = null; //probably null already
            this.storage.Clear();
            this.isDirty = true;
        }

        public void Trim()
        {
            //Used to save some RAM, or just before serializing the databank via protobuf-net.
            DateTime t0 = DateTime.Now;
            foreach (IVariable iv in this.storage.Values)
            {
                iv.DeepTrim();
            }
            G.WritelnGray("TRIM: " + G.Seconds(t0));
        }

        /// <summary>
        /// Default, when it is conceptually a non-LHS (left-hand side) variable, like the x in y = 2 * x.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IVariable GetIVariable(string variable)
        {
            return GetIVariable(variable, false);
        }

        /// <summary>
        /// Get IVariable from databank. May return null.
        /// Can choose if it is conceptually a LHS (left-hand side) variable, like the y in y = 2 * x.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IVariable GetIVariable(string variable, bool isLhs)
        {
            //Most and maybe all variable access goes through here (see also #jslej48djsd9)
            //Handles array-subseries too.
            IVariable iv = null;
            if (Series.IsArraySubSeriesName(variable))
            {                
                string dbName, varName, freq; string[] indexes; char firstChar;
                O.Chop(variable, out dbName, out varName, out freq, out indexes);
                if (this.storage.Count > 0)
                {
                    string atsName = O.UnChop(dbName, varName, freq, null, null);
                    IVariable iv2 = null; this.storage.TryGetValue(atsName, out iv2);
                    if (iv2 == null)
                    {
                        new Error("Cannot find variable '" + atsName + "'");
                    }
                    else
                    {
                        Series ats = iv2 as Series;
                        if (ats == null) new Error("Internal error #873k4j744734");
                        iv = ats.FindArraySeries(null, indexes, false, false, null);
                    }
                }
            }
            else
            {
                if (this.storage.Count > 0)
                {
                    this.storage.TryGetValue(variable, out iv);
                }
            }
            //What about             
            Program.RegisterANewTracePrecedent(iv, this, isLhs, true); //both precedents for DECOMP and data tracing
            return iv;
        }

        /// <summary>
        /// See overload.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IVariable GetIVariableMayCreateSeries(string variable)
        {
            return GetIVariableMayCreateSeries(variable, false);
        }

        /// <summary>
        /// If sigil variable (% or #), it will get the IVariable like GetIVariable(). 
        /// If series name, and if not present and a timeseries, the timeseries will be created (array or non-array).
        /// If x[a,b] and not even x exists, both the x parent series and the x[a,b] subseries are created.
        /// </summary>        
        public IVariable GetIVariableMayCreateSeries(string variable, bool isLhs)
        {
            if (G.StartsWithSigil(variable)) return GetIVariable(variable, isLhs);
            if (this.ContainsIVariable(variable))
            {
                Series ts = this.GetIVariable(variable, isLhs) as Series;
                if (!Series.IsArraySubSeriesName(variable) && ts.type == ESeriesType.ArraySuper)
                {
                    new Error("Missing []-index on " + ts.dimensions + "-dimensional array-timeseries " + G.GetNameAndFreqPretty(ts.name));
                }
                return ts;
            }
            else
            {
                Series ts = new Series(G.ConvertFreq(G.Chop_GetFreq(variable)), null);
                this.AddIVariable(variable, ts);                
                return ts;
            }
        }


        /// <summary>
        /// Overload. May write a list file.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        public void AddIVariableWithOverwrite(string name, IVariable x)
        {
            if (name != null && Program.IsListfileArtificialName(name))
            {
                O.WriteListFile(name, x);
            }
            else
            {
                if (!this.editable) Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");
                if (x.Type() == EVariableType.Series && ((Series)x).type == ESeriesType.Light)
                {
                    throw new GekkoException(); //only intended for non-series
                }
                if (this.ContainsIVariable(name))
                {
                    this.RemoveIVariable(name);
                }
                this.AddIVariable(name, x);
            }
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="x"></param>
        public void AddIVariableWithOverwrite(IVariable x)
        {
            if (!this.editable) Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");
            Series x_series = x as Series;
            if (x_series != null)
            {
                if (this.ContainsIVariable(x_series.name))
                {
                    this.RemoveIVariable(x_series.name);
                }
                this.AddIVariable(x_series.name, x);
            }
            else throw new GekkoException();  //only intended for series            
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        public void AddIVariable(string name, IVariable x)
        {
            AddIVariable(name, x, false);
        }

        /// <summary>
        /// Main central method for adding a new IVariable. The other adding methods here go through this.
        /// When it is known that isSimpleName == true, checking the name can be skipped for speed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="isSimpleName"></param>
        public void AddIVariable(string name, IVariable x, bool isSimpleName)
        {
            if (!this.editable) Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");

            if (!isSimpleName) G.CheckIVariableNameAndType(x, G.CheckIVariableName(name));

            Series ts = x as Series;
            if (ts != null && ts.type == ESeriesType.Light) throw new GekkoException(); //this check can be removed at some point

            if (ts != null)
            {
                //Series type
                if (Series.IsArraySubSeriesName(name))
                {                    
                    string dbName, varName, freq; string[] indexes; char firstChar;
                    O.Chop(name, out dbName, out varName, out freq, out indexes);
                    string atsName = O.UnChop(dbName, varName, freq, null, null);                                        
                    IVariable iv2 = null; this.storage.TryGetValue(atsName, out iv2);
                    Series ats = iv2 as Series;
                    if (ats == null)
                    {
                        //must construct it first                        
                        ats = new Series(G.ConvertFreq(freq), atsName);
                        ats.SetArrayTimeseries(indexes.Length + 1, true);
                        AddIvariableHelper(atsName, ats);
                    }
                    else
                    {
                        if (ats.dimensions != indexes.Length) new Error(indexes.Length + " dimensional index " + ats.name + Stringlist.GetIndexWithCommas(indexes) + " used on " + ats.dimensions + "-dimensional array-timeseries " + G.GetNameAndFreqPretty(ats.name));
                    }
                    ats.SetDirty(true);
                    ats.dimensionsStorage.AddIVariableWithOverwrite(new MultidimElement(indexes, ats), ts);
                    ts.name = Globals.seriesArraySubName + Globals.freqIndicator + freq;  //We have to overwrite it here, else it would be "x[a,b]!a"

                }
                else
                {
                    ts.meta.parentDatabank = this;
                    ts.SetDirty(true);
                    //for instance when cloning from x to y, the y object will have x as name. Therefor we set the name here.                
                    //often this name is already correct here, but for cloning (COPY command) etc. we need to set the name right.
                    ts.name = name;
                    AddIvariableHelper(name, x);
                }
            }
            else
            {
                //Non-series
                AddIvariableHelper(name, x);
            }
            Program.RegisterANewTracePrecedent(x, this, true, false);
        }

        /// <summary>
        /// What is the purpose of this method?
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        private void AddIvariableHelper(string name, IVariable x)
        {
            //See also #0893543895, here the name is set outside this helper method
            this.storage.Add(name, x);
        }

        /// <summary>
        /// Remove a variable
        /// </summary>
        /// <param name="name"></param>
        public void RemoveIVariable(string name)
        {
            if (this.storage.ContainsKey(name)) this.storage.Remove(name);
            this.isDirty = true;
        }

        /// <summary>
        /// Check existence of a variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsIVariable(string variable)
        {
            if (Series.IsArraySubSeriesName(variable))
            {                
                string dbName, varName, freq; string[] indexes; char firstChar;
                O.Chop(variable, out dbName, out varName, out freq, out indexes);
                string atsName = O.UnChop(dbName, varName, freq, null, null);
                IVariable iv2 = null; this.storage.TryGetValue(atsName, out iv2);
                if (iv2 == null)
                {
                    return false;
                }
                else
                {
                    Series ats = iv2 as Series;
                    if (ats == null) new Error("Internal error #873k4j744734");
                    LookupSettings settings = new LookupSettings();
                    settings.create = O.ECreatePossibilities.NoneReturnNullAlways;
                    IVariable iv = ats.FindArraySeries(null, indexes, false, false, settings);
                    if (iv == null) return false;
                    else return true;
                }
            }
            else
            {
                return this.storage.ContainsKey(variable);
            }
        }

        public string Message()
        {
            return "databank " + "'" + this.name + "'";
        }

        /// <summary>
        /// Get name of databank.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.name;
        }

        public EBankType BankType()
        {
            return EBankType.Normal;
        }        

        public string GetFileNameWithPath()
        {            
            return this.fileNameWithPathPretty;  //has no filename
        }

        public string GetStamp()
        {
            return this.date;  //has no stamp
        }
    }

    [ProtoContract]
    public class DatabankCacheParams
    {
        //Excel ---------------------------------------------------------------------------

        [ProtoMember(1)]
        public string cols;

        [ProtoMember(2)]
        public string sheet;

        [ProtoMember(3)]
        public string cell;

        [ProtoMember(4)]
        public string datecell;

        [ProtoMember(5)]
        public string namecell;

        //At the moment, IMPORT<collapse=... method=...> does not use cache at all, so the following two are just placeholders
        public string collapse;
        public string method;

        [ProtoMember(6)]
        public string dateformat;

        [ProtoMember(7)]
        public string datetype;

        //Px ---------------------------------------------------------------------------

        [ProtoMember(8)]
        public bool variablecode;

        //Gdx ---------------------------------------------------------------------------       

        [ProtoMember(9)]
        public string option_gams_time_freq;

        [ProtoMember(10)]
        public string option_gams_time_set;

        [ProtoMember(11)]
        public string option_gams_time_prefix;

        [ProtoMember(12)]
        public double option_gams_time_offset;

        [ProtoMember(13)]
        public bool option_gams_time_detect_auto;

        [ProtoMember(14)]
        public int option_gams_trim;

        //Gbk ---------------------------------------------------------------------------       

        [ProtoMember(15)]
        public bool trace;

        //The following are not used in IsSame(). They are values from XML file inside .gbk and are stored here. When loading from cache, they are copied from here to the ReadInfo object.
        //See also ReadInfo class.

        [ProtoMember(16)]
        public string databankVersion = "";
        [ProtoMember(17)]
        public string info1 = null;
        [ProtoMember(18)]
        public string date;  
        [ProtoMember(19)]
        public string modelName;
        [ProtoMember(20)]
        public string modelInfo;
        [ProtoMember(21)]
        public string modelDate;
        [ProtoMember(22)]
        public string modelSignature;
        [ProtoMember(23)]
        public string modelHash;
        [ProtoMember(24)]
        public string modelLastSimPeriod;
        [ProtoMember(25)]
        public string modelLastSimStamp; 
        [ProtoMember(26)]
        public string modelLargestLag; 
        [ProtoMember(27)]
        public string modelLargestLead;

        [ProtoMember(28)]
        public string user;
        [ProtoMember(29)]
        public string branch;
        [ProtoMember(30)]
        public string commit;
        [ProtoMember(31)]
        public string gcm;

        [ProtoMember(32)]
        public int nTraces;

        [ProtoMember(33)]
        public string dataHash;

        // ================= COMPARE =======================================================

        /// <summary>
        /// Tests if one object is equal (equal fields) to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSame(DatabankCacheParams other)
        {
            //??? should this also compare user, branch, commit and gcm ??? NO!
            //    dataHash should not be necessary to add, since data changes affects the datafile hash anyway
            
            if (other == null) return false;
            //xlsx
            if (!G.Equal(this.cols, other.cols)) return false;
            if (!G.Equal(this.sheet, other.sheet)) return false;
            if (!G.Equal(this.cell, other.cell)) return false;
            if (!G.Equal(this.datecell, other.datecell)) return false;
            if (!G.Equal(this.namecell, other.namecell)) return false;
            if (!G.Equal(this.collapse, other.collapse)) return false;
            if (!G.Equal(this.method, other.method)) return false;
            if (!G.Equal(this.dateformat, other.dateformat)) return false;
            if (!G.Equal(this.datetype, other.datetype)) return false;
            //px
            if (this.variablecode != other.variablecode) return false;
            //gdx
            if (!G.Equal(this.option_gams_time_freq, other.option_gams_time_freq)) return false;
            if (!G.Equal(this.option_gams_time_set, other.option_gams_time_set)) return false;
            if (!G.Equal(this.option_gams_time_prefix, other.option_gams_time_prefix)) return false;
            if (this.option_gams_time_offset != other.option_gams_time_offset) return false;
            if (this.option_gams_time_detect_auto != other.option_gams_time_detect_auto) return false;
            if (this.option_gams_trim != other.option_gams_trim) return false;
            //gbk
            if (this.trace != other.trace) return false;
            return true;
        }
    }
}
