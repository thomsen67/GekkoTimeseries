/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

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


        //Note the .isDirty field, so methods that change anything must set isDirty = true!
        //Remember new fields in Clear() method and also in G.CloneDatabank()        
        [ProtoMember(1)]
        public GekkoDictionary<string, IVariable> storage;
        public string aliasName = null;
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
                if (G.Equal(this.aliasName, Globals.Work) || G.Equal(this.aliasName, Globals.Ref))
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
        public bool save = true;  //Don't use protobuffer on this field.
        public int yearStart = -12345;  //only set when reading a bank, not afterwards if timeseries change. Not meant for making loops etc. or critical, only static information about the bank        
        public int yearEnd = -12345;  //only set when reading a bank, not afterwards if timeseries change. Not meant for making loops etc. or critical, only static information about the bank        
        public string info1 = null; //must be taken from DatabankInfo.xml, don't use protobuffer        
        public string date = null; //must be taken from DatabankInfo.xml, don't use protobuffer
        public bool isDirty = false;  //used to see if en OPEN databank must be re-written. Don't use protobuffer on this field.
        public bool protect = false;  //used to set an OPEN databank as protected. Don't use protobuffer on this field.
        //public GekkoDictionary<string, string> tmptmpVars;
        public Program.ReadInfo readInfo = null; //contains info from reading the file, among other things info from the XML file. NOTE: do not store it in protobuf!
        public string fileHash = null; //do not store this in protobuf

        private Databank()
        {
            //This is ONLY because protobuf-net needs it
            this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
        }

        public Databank(string aliasName)
        {
            this.storage = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);
            this.aliasName = aliasName;
            //this.aliasNameOriginal = aliasName;
        }

        public void Clear() {
            if (this.protect) Program.ProtectError("You cannot clear a non-editable databank, see OPEN<edit> or UNLOCK");
            //aliasName = null; --> keep that name when clearing
            //fileNameWithPath = null;  --> keep that name when clearing
            yearStart = -12345;
            yearEnd = -12345;
            info1 = null;
            date = null;
            this.storage.Clear();
            //this.fileNameWithPath = null;  --> NO! This would be ok regarding READ, but not regarding OPEN<edit/first> for instance --> we need a bank to write back to!
            //this.readInfo = null;  //must be ok to remove, just contains stuff for printing --> but let us keep it for ultra safety for now
            this.isDirty = true;
        }

        public bool ContainsVariable(string variable) {
            return ContainsVariable(true, variable);
        }
        public bool ContainsVariable(bool freqAddToName, string variable)
        {
            if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable);
            return this.storage.ContainsKey(variable);
        }

        public void RemoveVariable(string variable)
        {
            RemoveVariable(true, variable);
        }

        public void RemoveVariable(EFreq eFreq, string variable)
        {
            if (this.protect) Program.ProtectError("You cannot remove a timeseries in a non-editable databank, see OPEN<edit> or UNLOCK");
            variable = Program.AddFreqAtEndOfVariableName(variable, eFreq);
            if (ContainsVariable(false, variable))  //do not add freq at the end (has just been added)
            {
                this.storage.Remove(variable);
            }
            this.isDirty = true;
            return;
        }

        public void RemoveVariable(bool freqAddToName, string variable)
        {
            if (this.protect) Program.ProtectError("You cannot remove a timeseries in a non-editable databank, see OPEN<edit> or UNLOCK");
            if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable);
            if (ContainsVariable(false, variable))  //do not add freq at the end (has just been added)
            {
                this.storage.Remove(variable);
            }
            this.isDirty = true;
            return;
        }

        //Generic method, not for outside use!
        private void RemoveVariable(bool freqAddToName, string freq, string variable)
        {
            if (this.protect) Program.ProtectError("You cannot remove a timeseries in a non-editable databank, see OPEN<edit> or UNLOCK");
            if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable, freq);
            if (ContainsVariable(false, variable))  //do not add freq at the end (has just been added)
            {
                this.storage.Remove(variable);
            }
            this.isDirty = true;
            return;
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
            //This does not change the databank, so this.hasBeenChanged is not touched!!
        }

        public IVariable GetIVariable(string variable)
        {
            IVariable iv = null;
            this.storage.TryGetValue(variable, out iv);
            return iv;
        }

        public void AddIVariable(IVariable x)
        {
            TimeSeries ts = x as TimeSeries;
            if (ts != null) AddIVariable(ts.name, x);
            else
            {
                G.Writeln2("***ERROR: Internal error: please use AddIvariable(name, x)");
                throw new GekkoException();
            }
        }

        public void AddIVariableWithOverwrite(string name, IVariable x)
        {
            if (this.protect) Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");
            if (this.ContainsIVariable(name))
            {
                this.RemoveIVariable(name);
                this.AddIVariable(name, x);
            }
        }

        public void AddIVariable(string name, IVariable x)
        {
            if (this.protect) Program.ProtectError("You cannot add a variable to a non-editable databank, see OPEN<edit> or UNLOCK");
            //For TimeSeries, use AddIVariable
            bool hasTilde = false;
            foreach (char c in name)
            {
                //The good thing is that this is only checked when putting stuff INTO the databank, and not
                //when retrieving from the databank. A ScalarVal will for instance just have its contents replaced,
                //if inside a loop.
                //The name may still be strange, but that will be caught in the Chop() method.
                //it seems "SER {'1x'} = ... " will be legal, but never mind. It can never be called with "PRT 1x" anyway.
                if (G.IsLetterOrDigitOrUnderscore(c) || c == Globals.symbolMemvar || c == Globals.symbolList || c == Globals.freqIndicator)
                {
                    //good
                }
                else
                {
                    G.Writeln2("***ERROR: Malformed name: '" + name + "'");
                    throw new GekkoException();
                }
                if (c == Globals.freqIndicator) hasTilde = true;
            }
            TimeSeries ts = x as TimeSeries;
            if (ts != null)
            {
                if (ts.name != name || !hasTilde)
                {
                    G.Writeln2("*** ERROR: #763209485");  //use AddIVariable(x), remember tilde in x.variableName.
                    throw new GekkoException();
                }                
                ts.meta.parentDatabank = this;
                ts.SetDirty(true);
            }
            this.storage.Add(name, x);
        }

        public void RemoveIVariable(string name)
        {
            if (this.storage.ContainsKey(name)) this.storage.Remove(name);
        }

        public bool ContainsIVariable(string name)
        {
            return this.storage.ContainsKey(name);
        }

        public string Message()
        {
            return "databank " + "'" + this.aliasName + "'";
        }

        //OLD OLD OLD
        //OLD OLD OLD
        //OLD OLD OLD
        //OLD OLD OLD
        //OLD OLD OLD
        //OLD OLD OLD

        public void AddVariable(TimeSeries ts)
        {
            G.Writeln2("*** ERROR: #743297326");
            throw new GekkoException();
            AddVariable(true, ts, true);
        }

        public void AddVariable(TimeSeries ts, bool variableNameCheck)
        {
            G.Writeln2("*** ERROR: #743297325");
            throw new GekkoException();
            AddVariable(true, ts, variableNameCheck);
        }

        //generic method, not for outside use
        private void AddVariable(bool freqAddToName, TimeSeries ts, bool variableNameCheck)
        {
            G.Writeln2("*** ERROR: #743297324");
            throw new GekkoException();
            if (this.protect) Program.ProtectError("You cannot add a timeseries to a non-editable databank, see OPEN<edit> or UNLOCK");
            string variable = ts.name;
            if (variableNameCheck && !G.IsSimpleToken(variable, true))  //also checks for null and "" and '¤'
            {
                G.Writeln2("*** ERROR in databank: the name '" + variable + "' is not a simple variable name");
                throw new GekkoException();
            }
            //if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable); 
            //else variable = Program.AddFreqAtEndOfVariableName(variable, G.GetFreq(ts.freq));                
            this.storage.Add(variable, ts);
            ts.meta.parentDatabank = this;
            this.isDirty = true;
        }

        public TimeSeries GetVariable(string variable)
        {
            G.Writeln2("*** ERROR: #743297321");
            throw new GekkoException();
            return GetVariable(true, variable);
        }

        public TimeSeries GetVariable(bool freqAddToName, string variable)
        {
            G.Writeln2("*** ERROR: #743297322");
            throw new GekkoException();
            if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable);
            IVariable x = GetIVariable(variable);
            return (TimeSeries)x;            
        }

        public TimeSeries GetVariable(EFreq eFreq, string variable)
        {
            G.Writeln2("*** ERROR: #743297323");
            throw new GekkoException();
            if (eFreq != EFreq.Annual) variable = Program.AddFreqAtEndOfVariableName(variable, eFreq);  //we do this IF here because it is speed critical code. Else a new string object will be created.
            IVariable x = GetIVariable(variable);
            return (TimeSeries)x;
        }        
    }
}
