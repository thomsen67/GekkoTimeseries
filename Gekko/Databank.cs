/* 
    Gekko Timeseries Software (www.t-t.dk/gekko). 
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

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
        //version 1.2 is for Gekko 3.0.

        [ProtoBeforeSerialization]
        public void BeforeProtobufWrite()
        {
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                Recurse(kvp.Value, true);
            }
        }

        [ProtoAfterDeserialization]
        public void AfterProtobufRead()
        {
            foreach (KeyValuePair<string, IVariable> kvp in this.storage)
            {
                Recurse(kvp.Value, false);
            }
        }

        /// <summary>
        /// We have to recurse here, because [ProtoBeforeSerialization] and
        /// [ProtoAfterDeserialization] do not work inside an object tree structure,
        /// but only at the uppermost object. Before, this was put in Matrix.cs only.
        /// </summary>
        /// <param name="iv"></param>
        /// <param name="b"></param>
        public void Recurse(IVariable iv, bool b)
        {
            if (iv.Type() == EVariableType.Matrix)
            {
                Matrix m = (Matrix)iv;
                if (b)
                {
                    m.BeforeProtobufWrite();
                }
                else
                {
                    m.AfterProtobufRead();
                }
            }
            else if (iv.Type() == EVariableType.List)
            {
                List thisList = (List)iv;
                foreach (IVariable iv2 in thisList.list)
                {
                    Recurse(iv2, b);
                }
            }
            else if (iv.Type() == EVariableType.Map)
            {
                Map thisMap = (Map)iv;
                foreach (KeyValuePair<string, IVariable> kvp in thisMap.storage)
                {
                    Recurse(kvp.Value, b);
                }
            }
        }

        //Note the .isDirty field, so methods that change anything must set isDirty = true!
        //Remember new fields in Clear() method and also in G.CloneDatabank()        
        [ProtoMember(1)]
        public GekkoDictionary<string, IVariable> storage;

        [ProtoMember(2)]
        public DatabankCacheParams cacheParameters = null;

        [ProtoMember(3)]
        public Dictionary<Trace, Precedents> traces = null;

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
        public Program.ReadInfo readInfo = null; //contains info from reading the file, among other things info from the XML file. NOTE: do not store it in protobuf!
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

        public void Clear() {
            if (!this.editable) Program.ProtectError("You cannot clear a non-editable databank, see OPEN<edit> or UNLOCK");            
            yearStart = -12345;
            yearEnd = -12345;
            info1 = null;
            date = null;
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
            //Beware that an array-superseries is accessed here, but its sub-series are 
            IVariable iv = null;
            if (this.storage.Count > 0)
            {
                this.storage.TryGetValue(variable, out iv);
            }
            //What about             
            Program.Trace(iv, this, variable, isLhs, true); //both precedents for DECOMP and data tracing
            return iv;
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
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="isSimpleName"></param>
        public void AddIVariable(string name, IVariable x, bool isSimpleName)
        {
            if (!this.editable) Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");

            if (!isSimpleName) G.CheckIVariableNameAndType(x, G.CheckIVariableName(name));

            Series ts = x as Series;
            if (ts != null)
            {
                if (ts.type == ESeriesType.Light)
                {
                    throw new GekkoException(); //this check can be removed at some point
                }
                ts.meta.parentDatabank = this;
                ts.SetDirty(true);
                //for instance when cloning from x to y, the y object will have x as name. Therefor we set the name here.                
                //often this name is already correct here, but for cloning (COPY command) etc. we need to set the name right.
                ts.name = name;
            }
            AddIvariableHelper(name, x);
            Program.Trace(x, this, name, true, false);
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
        }

        /// <summary>
        /// Check existence of a variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsIVariable(string name)
        {
            return this.storage.ContainsKey(name);
        }

        public string Message()
        {
            return "databank " + "'" + this.name + "'";
        }

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

        /// <summary>
        /// Tests if one object is equal (equal fields) to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSame(DatabankCacheParams other)
        {
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
            return true;
        }
    }
}
