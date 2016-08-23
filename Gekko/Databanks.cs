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
using System.Drawing;

namespace Gekko
{
    public class Databanks
    {
        public List<Databank> storage;
        public bool swappingIsActive = false;        
        
        public Databanks()
        {
            this.storage = new List<Databank>();
        }

        public Databank GetFirst()
        {
            return this.storage[0];
        }

        public Databank GetRef()
        {
            return this.storage[1];
        }

        public Databank GetDatabank(string databank) {            
            if (databank == "@")
            {
                return this.GetRef();
            }
            else
            {
                foreach (Databank db in this.storage)
                {
                    if (G.equal(db.aliasName, databank)) return db;
                }
            }
            return null;            
        }

        public bool OpenDatabank(ref Databank databank, EOpenType openType, int openPosition)
        {
            if (openType == EOpenType.Pos)
            {
                if (openPosition > this.storage.Count)
                {
                    G.Writeln2("*** ERROR: There are " + (this.storage.Count - 1) + " numbered databanks in the databank list (F2).");
                    G.Writeln("           Opening in position " + openPosition + " is not possible.", Color.Red);
                    throw new GekkoException();
                }
            }
            
            if (openType == EOpenType.Ref)
            {
                G.Writeln2("+++ WARNING: OPEN<ref> is for advanced users, and will put the existing " + Globals.Ref + " in the list of 'normal' databanks", Globals.warningColor);
            }
            bool readFromFile = false;
            //Does not read the actual bank, but just arranges for the bank to be read into the right 'slot'
            //If <first/edit> or <ref>, the bank in the [0] or [1] slot is pushed down to [2].
            if (G.equal(databank.aliasName, Globals.Work))
            {
                if (openType == EOpenType.Normal || openType == EOpenType.Last)
                {
                    G.Writeln2("*** ERROR: The 'Work' databank cannot be opened or closed (it is always open).");
                    throw new GekkoException();
                }                
                else if (openType == EOpenType.First)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<first> with the 'Work' databank.");
                    G.Writeln("           If Work is not first and it needs to be, you must CLOSE");
                    G.Writeln("           the present first databank. After that, Work will be first.");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Edit)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<edit> with the 'Work' databank.");
                    G.Writeln("           Work is always editable, and if Work is not first on");
                    G.Writeln("           the F2 databank list, you must CLOSE the present first");
                    G.Writeln("           databank. After that, Work will become first (and editable).");
                    
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Ref)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<ref> with the 'Work' databank.");
                    G.Writeln("           It is not legal to set Work as the ref databank.");
                    G.Writeln("           Use the Ref databank for such purposes.");
                    throw new GekkoException();
                }     
            }
            else if (G.equal(databank.aliasName, Globals.Ref))  //Ref
            {
                if (openType == EOpenType.Normal)
                {
                    G.Writeln2("*** ERROR: The '" + Globals.Ref + "' databank cannot be opened or closed (it is always open).");                    
                    throw new GekkoException();
                }
                else if (openType == EOpenType.First)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<first> with the '" + Globals.Ref + "' databank.");                    
                    throw new GekkoException();                    
                }
                else if (openType == EOpenType.Last)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<last> with the '" + Globals.Ref + "' databank.");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Edit)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<edit> with the '" + Globals.Ref + "' databank.");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Ref)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN<ref> with the '" + Globals.Ref + "' databank.");                    
                    throw new GekkoException();
                }     
            }
            string name = databank.aliasName;
            int existI; int WorkI; int BaseI; FindBanksI(name, out existI, out WorkI, out BaseI);

            List<Databank> m = new List<Databank>(this.storage.Count);
            if (existI != -12345)  //the databank name already exists. No actual file reading, just rearrange the banks
            {                
                databank = this.storage[existI];  //now points to the existing databank, and no longer the empty databank the method was called with
                readFromFile = false;
                if (openType == EOpenType.Normal || openType == EOpenType.Last || (openType == EOpenType.Pos && openPosition != 1))
                {
                    G.Writeln2("*** ERROR: Databank '" + databank.aliasName + "' is already open. Use CLOSE to close it first.");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Edit || openType == EOpenType.First || (openType == EOpenType.Pos && openPosition == 1))
                {
                    if (existI == 0)
                    {
                        //Note: OPEN<edit> could be used to unlock an OPEN<first>...
                        //this.storage[0].protect = false;  //this is set elsewhere
                        if (openType == EOpenType.Edit)
                        {
                            if (databank.protect == false)
                            {
                                G.Writeln2("Databank '" + databank.aliasName + "' is already editable in first position.");
                            }
                            else
                            {
                                databank.protect = false;
                                G.Writeln2("Databank '" + databank.aliasName + "' set editable.");
                            }
                        }
                        m.AddRange(this.storage);  //just copied, and put back again later on
                    }
                    else if (existI == 1)  //Trying an OPEN<edit>db on a db that is already ref (opened with OPEN<ref>db).
                    {
                        m.Add(this.storage[existI]);    //first, = former sec
                        m.Add(this.storage[BaseI]);     //ref, = Ref databank, to aviod empty slot
                        m.Add(this.storage[0]);         //former first ends here
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == BaseI) continue;
                            m.Add(this.storage[i]);
                        }
                    }
                    else  //Trying an OPEN<edit>db on a db that is already there in slot [2] or below
                    {
                        m.Add(this.storage[existI]);         //first
                        m.Add(this.storage[1]);              //ref, same
                        m.Add(this.storage[0]);
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == existI) continue;
                            m.Add(this.storage[i]);
                        }
                    }
                    if (openType == EOpenType.Edit) G.Writeln2("Databank '" + name + "' set as editable databank, put first position.");
                    else G.Writeln2("Databank '" + name + "' put in first position.");
                }
                else if (openType == EOpenType.Ref)
                {
                    if (existI == 0) //Trying an OPEN<sec>db on a db that is already first/editable (opened with OPEN<first> or OPEN<edit>)
                    {
                        m.Add(this.storage[WorkI]);    //first, = Work databank, to aviod empty slot
                        m.Add(this.storage[existI]);   //ref, = former first
                        m.Add(this.storage[1]);        //former ref ends here
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == WorkI) continue;
                            m.Add(this.storage[i]);
                        }
                    }
                    else if (existI == 1)
                    {
                        G.Writeln2("*** ERROR: Databank '" + databank.aliasName + "' is already open as ref bank");
                        throw new GekkoException();
                    }
                    else  //Trying an OPEN<edit/first>db on a db that is already there in slot [2] or below
                    {
                        m.Add(this.storage[0]);         //first, same
                        m.Add(this.storage[existI]);    //ref
                        m.Add(this.storage[1]);
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == existI) continue;
                            m.Add(this.storage[i]);
                        }
                    }
                    G.Writeln2("Databank '" + name + "' set as ref bank");
                }
            }
            else  //the databank name does not exist, so it is new and will be read from file later on
            {
                readFromFile = true;
                if (openType == EOpenType.Normal || (openType == EOpenType.Pos && openPosition == 2))
                {                    
                    m.Add(this.storage[0]);  //first
                    m.Add(this.storage[1]);  //ref
                    m.Add(databank);
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                    G.Writeln2("Databank '" + name + "' opened");                
                }
                else if (openType == EOpenType.First || openType == EOpenType.Edit || (openType == EOpenType.Pos && openPosition == 1))
                {
                    bool edit = false;
                    if (openType == EOpenType.Edit) edit = true;
                    m.Add(databank);         //first
                    m.Add(this.storage[1]);  //ref
                    m.Add(this.storage[0]);
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);                    
                    if (openType == EOpenType.Edit) G.Writeln2("Databank '" + name + "' opened as editable in first position");
                    else G.Writeln2("Databank '" + name + "' opened in first position");
                }
                else if (openType == EOpenType.Ref)
                {
                    m.Add(this.storage[0]);         //first
                    m.Add(databank);                //ref
                    m.Add(this.storage[1]);
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                    G.Writeln2("Databank '" + name + "' opened as ref");
                }
                else if (openType == EOpenType.Last || (openType == EOpenType.Pos && openPosition == this.storage.Count + 1))
                {
                    m.Add(this.storage[0]);  //first
                    m.Add(this.storage[1]);  //ref                
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                    m.Add(databank);
                    G.Writeln2("Databank '" + name + "' opened");
                }
                else if (openType == EOpenType.Pos)
                {
                    //pos is not 1., 2. or count+1 ===> so 3, 4, ..., up to count.
                    if (openPosition < 1)
                    {
                        G.Writeln2("*** ERROR: OPEN<pos=...> cannot be 0 or negative");
                        throw new GekkoException();
                    }
                    m.Add(this.storage[0]);  //first
                    m.Add(this.storage[1]);  //ref
                    for (int i = 2; i < openPosition; i++)
                    {
                        m.Add(this.storage[i]);
                    }
                    m.Add(databank);
                    for (int i = openPosition; i < this.storage.Count; i++)
                    {
                        m.Add(this.storage[i]);
                    }
                    G.Writeln2("Databank '" + name + "' opened in position " + openPosition);
                }
                else
                {
                    G.Writeln("*** ERROR: Internal error ¤89435734");
                    throw new GekkoException();
                }
            }
            this.storage = m;
            return readFromFile;
        }

        private static void FindBanksI(string name, out int existI, out int WorkI, out int BaseI)
        {
            existI = -12345;
            WorkI = -12345;
            BaseI = -12345;
            for (int i = 0; i < Program.databanks.storage.Count; i++)
            {
                if (G.equal(Program.databanks.storage[i].aliasName, name))
                {
                    existI = i;
                }
                if (G.equal(Program.databanks.storage[i].aliasName, Globals.Work))
                {
                    WorkI = i;
                }
                if (G.equal(Program.databanks.storage[i].aliasName, Globals.Ref))
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
                if (G.equal(db1.aliasName, this.storage[i].aliasName))
                {
                    this.storage[i] = db2; break;
                }
            }
            if (!ok)
            {
                G.Writeln2("*** ERROR: Could not replace databank " + db2.aliasName);
                throw new GekkoException();
            }
        }

        public Databank RemoveDatabank(string name)
        {
            if (G.equal(name, Globals.Work))
            {
                G.Writeln2("*** ERROR: " + Globals.Work + " databank cannot be closed");
                throw new GekkoException();
            }
            if (G.equal(name, Globals.Ref))
            {
                G.Writeln2("*** ERROR: " + Globals.Ref + " databank cannot be closed");
                throw new GekkoException();
            }

            int existI; int WorkI; int BaseI; FindBanksI(name, out existI, out WorkI, out BaseI);
            if (existI == -12345)
            {
                G.Writeln2("*** ERROR: Could not close databank '" + name + "': the bank is not open");
                throw new GekkoException();
            }

            Databank databankToRemove = Program.databanks.storage[existI];

            List<Databank> m = new List<Databank>(this.storage.Count - 1);
            if (existI == 0) //found as first
            {
                m.Add(this.storage[WorkI]);  //[0]: Work is put back
                m.Add(this.storage[1]);  //[1]: not touched
                for (int i = 2; i < this.storage.Count; i++)
                {
                    if (i == WorkI) continue;
                    m.Add(this.storage[i]);
                }
            }
            else if (existI == 1) //found as secondary
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
            G.Writeln2("*** ERROR: Swapping is not working at the moment.");
            throw new GekkoException();
            Databank db1 = Program.databanks.GetDatabank(old1);
            Databank db2 = Program.databanks.GetDatabank(old2);
            Program.databanks.RemoveDatabank(old1);
            Program.databanks.RemoveDatabank(old2);
            db1.aliasName = new1;
            db2.aliasName = new2;            
        }

        public static void Unswap()
        {
            List<Databank> dbList = new List<Databank>();
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.equal(databank.aliasName, Globals.Work)) dbList.Add(databank);
            }
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.equal(databank.aliasName, Globals.Ref)) dbList.Add(databank);
            }
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.equal(databank.aliasName, Globals.Work)) continue;
                if (G.equal(databank.aliasName, Globals.Ref)) continue;
                dbList.Add(databank);
            }
            Program.databanks.storage = dbList;            
        }
    }
}
