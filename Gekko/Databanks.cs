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
using System.Drawing;

namespace Gekko
{
    public class Databanks
    {
        public List<Databank> storage;
        public Databank local = new Databank("Local");
        public Databank global = new Databank("Global");
        public LocalGlobal localGlobal = new LocalGlobal();  //see also LocalCode1() and LocalCode2()
        public Databank optionBank = null;
        public Databank optionRef = null;

        public bool swappingIsActive = false;        
        
        public Databanks()
        {
            this.storage = new List<Databank>();
        }

        public IVariable GetVariableWithSearch(string varName, bool canSearch)
        {
            //check local bank
            IVariable rv = this.GetLocal().GetIVariable(varName);
            if (rv != null)
            {                
                return rv;
            }

            if (this.optionBank != null)
            {                
                return this.optionBank.GetIVariable(varName);
            }

            for (int i = 0; i < this.storage.Count; i++)
            {
                if (i == 1) continue;  //The Ref databank IS NEVER SEARCHED!!
                if (canSearch == false && i > 1) break;  //without seach, we never search normal OPENed databanks
                Databank db2 = this.storage[i];
                rv = db2.GetIVariable(varName);
                if (rv != null)
                {                    
                    return rv;
                }
            }            
            return this.GetGlobal().GetIVariable(varName);
        }


        public Databank GetFirst()
        {
            if (Program.databanks.optionBank != null) return Program.databanks.optionBank;
            return this.storage[0];
        }

        public Databank GetRef()
        {
            if (Program.databanks.optionRef != null) return Program.databanks.optionRef;
            return this.storage[1];
        }

        public Databank GetLocal()
        {
            return this.local;
        }

        public Databank GetGlobal()
        {
            return this.global;
        }

        public Databank GetDatabank(string databank)
        {
            return GetDatabank(databank, false);
        }

        public Databank GetDatabank(string databank, bool reportError)
        {
            string databank_lower = databank.ToLower();

            if (!G.IsSimpleToken(databank))
            {
                new Error("Databank name '" + databank + "' is not a simple name");
            }

            switch (databank_lower)
            {
                case "@":                
                case Globals.ref_name:
                    {
                        return this.GetRef();
                    }
                    break;                
                case Globals.local_name:
                    {
                        return this.GetLocal();
                    }
                    break;                
                case Globals.global_name:
                    {
                        return this.GetGlobal();
                    }
                    break;
                case Globals.first_name:
                    {
                        return this.GetFirst();
                    }
                    break;                
                default:
                    {
                        foreach (Databank db in this.storage)
                        {
                            if (G.Equal(db.name, databank_lower)) return db;
                        }
                    }
                    break;
            }
            
            if (reportError)
            {
                new Error("Databank '" + databank + "' does not seem to be open");
            }

            return null;
            
        }

        public Databank OpenDatabankNew(string name, Databank databankTemp, EOpenType openType, int openPosition, int existI, int workI, int refI, bool create)
        {
            Databank rv = null;
            if (openType == EOpenType.Pos)
            {
                if (openPosition > this.storage.Count)
                {
                    new Error("There are " + (this.storage.Count - 1) + " numbered databanks in the databank list (F2). Opening in position " + openPosition + " is not possible.");
                }
            }
            
            if (openType == EOpenType.Ref)
            {
                new Warning("OPEN<ref> is for advanced users, and will put the existing " + Globals.Ref + " in the list of 'normal' databanks");
            }
            //bool readFromFile = false;
            //Does not read the actual bank, but just arranges for the bank to be read into the right 'slot'
            //If <first/edit> or <ref>, the bank in the [0] or [1] slot is pushed down to [2].
            if (G.Equal(name, Globals.Work))
            {
                if (openType == EOpenType.Normal || openType == EOpenType.Last)
                {
                    new Error("The 'Work' databank cannot be opened or closed (it is always open).");
                }
                else if (openType == EOpenType.First)
                {
                    new Error("You cannot use OPEN<first> with the 'Work' databank. If Work is not first and it needs to be, you must CLOSE the present first databank. After that, Work will be first.");
                }
                else if (openType == EOpenType.Edit)
                {
                    new Error("You cannot use OPEN<edit> with the 'Work' databank. Work is always editable, and if Work is not first on the F2 databank list, you must CLOSE the present first databank. After that, Work will become first (and editable).");
                }
                else if (openType == EOpenType.Ref)
                {
                    new Error("You cannot use OPEN<ref> with the 'Work' databank. It is not legal to set Work as the ref databank. Use the Ref databank for such purposes.");                    
                }
            }
            else if (G.Equal(name, Globals.Ref))  //Ref
            {
                if (openType == EOpenType.Normal)
                {
                    new Error("The '" + Globals.Ref + "' databank cannot be opened or closed (it is always open).");
                }
                else if (openType == EOpenType.First)
                {
                    new Error("You cannot use OPEN<first> with the '" + Globals.Ref + "' databank.");
                }
                else if (openType == EOpenType.Last)
                {
                    new Error("You cannot use OPEN<last> with the '" + Globals.Ref + "' databank.");
                }
                else if (openType == EOpenType.Edit)
                {
                    new Error("You cannot use OPEN<edit> with the '" + Globals.Ref + "' databank.");
                }
                else if (openType == EOpenType.Ref)
                {
                    new Error("You cannot use OPEN<ref> with the '" + Globals.Ref + "' databank.");
                }
            }
            else if (G.Equal(name, Globals.First))
            {
                new Error("The databank name 'First' is reserved and cannot be used.");
            }
            else if (G.Equal(name, Globals.All))
            {
                new Error("The databank name 'All' is reserved and cannot be used.");
            }
            else if (G.Equal(name, Globals.Local))
            {
                new Error("The databank name 'Local' is reserved and cannot be used.");
            }
            else if (G.Equal(name, Globals.Global))
            {
                new Error("The databank name 'Global' is reserved and cannot be used.");
            }

            //string name = databank.name;
            //int existI; int WorkI; int BaseI; FindBanksI(name, out existI, out WorkI, out BaseI);

            List<Databank> m = new List<Databank>(this.storage.Count);
            if (existI != -12345)  //the databank name already exists. No actual file reading, just rearrange the banks
            {
                rv = DatabankLogicExistingBankNew(name, databankTemp, openType, openPosition, existI, workI, refI, m);
            }
            else  //the databank name does not exist already
            {
                //readFromFile = true;                
                rv = DatabankLogicDefaultNew(name, databankTemp, openType, openPosition, m, create);                
            }
            this.storage = m;
            return rv;
        }
        
        private Databank DatabankLogicExistingBankNew(string name, Databank databank, EOpenType openType, int openPosition, int existI, int WorkI, int RefI, List<Databank> m)
        {
            databank = this.storage[existI];  //the databank at the slot where the new databank is to be put in
            Databank rv = databank;
            if (openType == EOpenType.Normal || openType == EOpenType.Last || (openType == EOpenType.Pos && openPosition != 1))
            {
                new Error("Databank '" + rv.name + "' is already open. Use CLOSE to close it first.");
                //throw new GekkoException();
            }
            else if (openType == EOpenType.Edit || openType == EOpenType.First || (openType == EOpenType.Pos && openPosition == 1))
            {
                if (existI == 0)
                {
                    //Note: OPEN<edit> could be used to unlock an OPEN<first>...
                    //this.storage[0].protect = false;  //this is set elsewhere
                    if (openType == EOpenType.Edit)
                    {
                        if (rv.editable)
                        {
                            new Writeln("Databank '" + name + "' is already editable in first position.");
                        }
                    }
                    m.AddRange(this.storage);  //just copied, and put back again later on
                }
                else if (existI == 1)  //Trying an OPEN<edit>db on a db that is already ref (opened with OPEN<ref>db).
                {
                    m.Add(rv);    //first, = former sec
                    m.Add(this.storage[RefI]);     //ref, = Ref databank, to aviod empty slot
                    m.Add(this.storage[0]);         //former first ends here
                    for (int i = 2; i < this.storage.Count; i++)
                    {
                        if (i == RefI) continue;
                        m.Add(this.storage[i]);
                    }
                }
                else  //Trying an OPEN<edit>db on a db that is already there in slot [2] or below
                {
                    m.Add(rv);         //first
                    m.Add(this.storage[1]);              //ref, same
                    m.Add(this.storage[0]);
                    for (int i = 2; i < this.storage.Count; i++)
                    {
                        if (i == existI) continue;
                        m.Add(this.storage[i]);
                    }
                }

                if (openType == EOpenType.Edit)
                {
                    if (openType == EOpenType.Edit) rv.editable = true;
                    new Writeln("Databank '" + name + "' set as editable databank, put in first position.");
                }
                else new Writeln("Databank '" + name + "' put in first position.");
            }
            else if (openType == EOpenType.Ref)
            {
                new Error("OPEN <ref> not allowed.");        
            }
            return rv;
        }

        private Databank DatabankLogicDefaultNew(string name, Databank databank, EOpenType openType, int openPosition, List<Databank> m, bool create)
        {
            Databank rv = databank;
            if ((openType == EOpenType.Edit || create) && rv == null)
            {
                //OPEN <edit/create> b, where b does not exist as a file, and where b is not opened already
                rv = new Databank(name);
            }

            //default logic                                
            if (openType == EOpenType.Sec)
            {
                new Error("OPEN <sec> not allowed.");
                //throw new GekkoException();                
            }
            else if (openType == EOpenType.First || openType == EOpenType.Edit || (openType == EOpenType.Pos && openPosition == 1))
            {                
                m.Add(rv);         //first
                m.Add(this.storage[1]);  //ref
                m.Add(this.storage[0]);
                for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                if (openType == EOpenType.Edit) new Writeln("Opening databank '" + name + "' as editable in first position");
                else new Writeln("Opening databank '" + name + "' in first position");
            }
            else if (openType == EOpenType.Ref)
            {
                new Error("OPEN <ref> not allowed.");
            }
            else if (ShouldPutBankLast(openType, openPosition))
            {
                m.Add(this.storage[0]);  //first
                m.Add(this.storage[1]);  //ref                
                for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                m.Add(rv);
                new Writeln("Opening databank '" + name + "'");
            }
            else if (openType == EOpenType.Pos)
            {
                //pos is not 1., 2. or count+1 ===> so 3, 4, ..., up to count.
                if (openPosition < 1)
                {
                    new Error("OPEN <pos = ...> cannot be 0 or negative");
                    //throw new GekkoException();
                }
                m.Add(this.storage[0]);  //first
                m.Add(this.storage[1]);  //ref
                for (int i = 2; i < openPosition; i++)
                {
                    m.Add(this.storage[i]);
                }
                m.Add(rv);
                for (int i = openPosition; i < this.storage.Count; i++)
                {
                    m.Add(this.storage[i]);
                }
                new Writeln("Opening databank '" + name + "' in position " + openPosition);
            }
            else
            {
                new Error("Internal error #89435735");
            }

            return rv;
        }
        
        public bool ShouldPutBankLast(EOpenType openType, int openPosition)
        {
            return openType == EOpenType.Normal || openType == EOpenType.Last || (openType == EOpenType.Pos && openPosition == this.storage.Count + 1);
        }

        public static void FindBanksI(string name, out int existI, out int WorkI, out int BaseI)
        {
            existI = -12345;
            WorkI = -12345;
            BaseI = -12345;
            for (int i = 0; i < Program.databanks.storage.Count; i++)
            {
                if (G.Equal(Program.databanks.storage[i].name, name))
                {
                    existI = i;
                }
                if (G.Equal(Program.databanks.storage[i].name, Globals.Work))
                {
                    WorkI = i;
                }
                if (G.Equal(Program.databanks.storage[i].name, Globals.Ref))
                {
                    BaseI = i;
                }
            }
        }        

        public void ReplaceDatabank(Databank db1, Databank db2)
        {
            bool ok = true;
            for (int i = 0; i < this.storage.Count; i++)
            {
                if (G.Equal(db1.name, this.storage[i].name))
                {
                    this.storage[i] = db2; break;
                }
            }
            if (!ok)
            {
                new Error("Could not replace databank " + db2.name);
                //throw new GekkoException();
            }
        }

        public Databank RemoveDatabank(string name)
        {
            if (G.Equal(name, Globals.Work))
            {
                new Error("" + Globals.Work + " databank cannot be closed");
                //throw new GekkoException();
            }
            if (G.Equal(name, Globals.Ref))
            {
                new Error("" + Globals.Ref + " databank cannot be closed");
                //throw new GekkoException();
            }

            int existI; int WorkI; int BaseI; FindBanksI(name, out existI, out WorkI, out BaseI);
            if (existI == -12345)
            {
                new Error("Could not close databank '" + name + "': the bank is not open");
                //throw new GekkoException();
            }

            Databank databankToRemove = Program.databanks.storage[existI];

            List<Databank> m = new List<Databank>(this.storage.Count - 1);
            if (existI == 0) //found as first
            {

                //Default
                //Closing a bank in first position (not Work)
                m.Add(this.storage[2]);  //[0]: gets #2 that is, number 2 on the non-ref databank list. 
                m.Add(this.storage[1]);  //[1]: ref is not touched
                for (int i = 3; i < this.storage.Count; i++)
                {
                    //add the rest
                    m.Add(this.storage[i]);
                }

            }
            else if (existI == 1) //found as ref
            {
                m.Add(this.storage[0]);  //[0]: not touched
                m.Add(this.storage[BaseI]);  //[1]: Base is put back
                for (int i = 2; i < this.storage.Count; i++)
                {
                    if (i == BaseI) continue;
                    m.Add(this.storage[i]);
                }
            }
            else //found as a normal open bank (position 2 or larger)
            {
                m.Add(this.storage[0]);  //[0]: not touched
                m.Add(this.storage[1]);  //[1]: not touched
                for (int i = 2; i < this.storage.Count; i++)
                {
                    if (i == existI) continue;
                    m.Add(this.storage[i]);
                }
            }            
            this.storage = m;
            return databankToRemove;
        }        

        public static void SwapBankAliases(string old1, string new1, string old2, string new2)
        {
            new Error("Swapping is not working at the moment.");
            //throw new GekkoException();
            //Databank db1 = Program.databanks.GetDatabank(old1);
            //Databank db2 = Program.databanks.GetDatabank(old2);
            //Program.databanks.RemoveDatabank(old1);
            //Program.databanks.RemoveDatabank(old2);
            //db1.name = new1;
            //db2.name = new2;            
        }

        public static void Unswap()
        {
            List<Databank> dbList = new List<Databank>();
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.Equal(databank.name, Globals.Work)) dbList.Add(databank);
            }
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.Equal(databank.name, Globals.Ref)) dbList.Add(databank);
            }
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.Equal(databank.name, Globals.Work)) continue;
                if (G.Equal(databank.name, Globals.Ref)) continue;
                dbList.Add(databank);
            }
            Program.databanks.storage = dbList;
            new Writeln("Moved Work and Ref back to their normal positions in the databank list (F2)");                     
        }
    }
}
