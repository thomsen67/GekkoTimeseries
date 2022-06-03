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

        //Note the .isDirty field, so methods that change anything must set isDirty = true!
        //Remember new fields in Clear() method and also in G.CloneDatabank()        
        [ProtoMember(1)]
        public GekkoDictionary<string, IVariable> storage;
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
        /// Get IVariable from databank. May return null.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IVariable GetIVariable(string variable)
        {
            //Most and maybe all variable access goes through here (see also #jslej48djsd9)
            //Beware that an array-superseries is accessed here, but its sub-series are 
            IVariable iv = null;
            if (this.storage.Count > 0)
            {
                this.storage.TryGetValue(variable, out iv);
            }
            //What about 
            Program.PrecedentsHelper(null, iv, this);
            bool isLhs = true;
            Program.Trace(variable, this, iv, isLhs);  
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
            Program.Trace(name, this, x, true);
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
        
        public EBankType BankType()
        {
            return EBankType.Normal;
        }  
    }
}
