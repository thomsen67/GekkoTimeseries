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
    public class Databank
    {

        /*
         * 
        This should be cleaned up at some point. In Add, the freq should be taken from the timeseries itself.
        
        Regarding frequencies: in .storage hash, the vars are kept with %q, %m, %u suffix for Q, M and U, whereas A does not
        have a suffix.

        --------
        
        ContainsVariable(string variable)
          adds global freq, for instance fy%q
        
        ContainsVariable(bool freqAddToName, string variable)
          maybe adds global freq, for instance fy%q
        
        ----------
        
        GetVariable(string variable)        

        GetVariable(bool freqAddToName, string variable)        

        GetVariable(EFreq eFreq, string variable)
        
        -----------
          
        AddVariable(TimeSeries ts)
          adds global freq, for instance fy%q
        
        AddVariable(string frequency, TimeSeries ts)   --> freq should be taken from ts!
          adds given freq, for instance fy%q     
         
        --------  
         
        RemoveVariable(string variable)
          adds global freq, for instance fy%q        

        RemoveVariable(bool freqAddToName, string variable)
          maybe add globals freq, for instance fy%q                             
         
        RemoveVariable(EFreq eFreq, string variable)
          adds given freq, for instance fy%q        
        
        */

        //Note the .isDirty field, so methods that change anything must set isDirty = true!
        //Remember new fields in Clear() method and also in G.CloneDatabank()        
        [ProtoMember(1)]
        public GekkoDictionary<string, TimeSeries> storage;
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
                if (G.equal(this.aliasName, Globals.Work) || G.equal(this.aliasName, Globals.Ref))
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
            this.storage = new GekkoDictionary<string, TimeSeries>(StringComparer.OrdinalIgnoreCase);            
        }
        
        public Databank(string aliasName)
        {
            this.storage = new GekkoDictionary<string, TimeSeries>(StringComparer.OrdinalIgnoreCase);            
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
            foreach (TimeSeries ts in this.storage.Values) 
            {                
                ts.Trim();                
            }
            G.WritelnGray("TRIM: " + G.Seconds(t0));
            //This does not change the databank, so this.hasBeenChanged is not touched!!
        }        

        public void AddVariable(TimeSeries ts)
        {             
            AddVariable(true, null, ts);            
        }        

        public void AddVariable(string frequency, TimeSeries ts)
        {
            AddVariable(false, frequency, ts);
        }


        //generic method, not for outside use
        private void AddVariable(bool freqAddToName, string frequency, TimeSeries ts)
        {            
            if (this.protect) Program.ProtectError("You cannot add a timeseries to a non-editable databank, see OPEN<edit> or UNLOCK");
            string variable = ts.variableName;
            if (!G.IsSimpleToken(variable, true))  //also checks for null and "" and '¤'
            {
                G.Writeln2("*** ERROR in databank: the name '" + variable + "' is not a simple variable name");
                throw new GekkoException();
            }
            if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable); 
            else variable = Program.AddFreqAtEndOfVariableName(variable, frequency);                
            this.storage.Add(variable, ts);
            ts.parentDatabank = this;
            this.isDirty = true;
        }

        public TimeSeries GetVariable(string variable)
        {            
            return GetVariable(true, variable);
        }

        public TimeSeries GetVariable(bool freqAddToName, string variable)
        {            
            if (freqAddToName) variable = Program.AddFreqAtEndOfVariableName(variable);
            TimeSeries x = null; this.storage.TryGetValue(variable, out x);
            return x;            
        }

        public TimeSeries GetVariable(EFreq eFreq, string variable)
        {
            if (eFreq != EFreq.Annual) variable = Program.AddFreqAtEndOfVariableName(variable, eFreq);  //we do this IF here because it is speed critical code. Else a new string object will be created.
            TimeSeries x = null; this.storage.TryGetValue(variable, out x);
            return x;
        }        
    }
}
