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

        public Databank GetPrim()
        {
            return this.storage[0];
        }

        public Databank GetSec()
        {
            return this.storage[1];
        }

        public Databank GetDatabank(string databank) {            
            if (databank == "@")
            {
                return this.GetSec();
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

        public bool OpenDatabank(ref Databank databank, EOpenType openType)
        {
            if (openType == EOpenType.Sec)
            {
                G.Writeln2("+++ WARNING: OPEN<ref> is possible in principle, but READ<ref> is strongly advised instead.");
                G.Writeln("             OPEN<ref> is for advanced users, and will put the existing " + Globals.Base + " in the list of 'normal' databanks", Globals.warningColor);                
            }
            bool readFromFile = false;
            //Does not read the actual bank, but just arranges for the bank to be read into the right 'slot'
            //If <prim> or <sec>, the bank in the [0] or [1] slot is pushed down to [2].
            if (G.equal(databank.aliasName, Globals.Work))
            {
                if (openType == EOpenType.Normal)
                {
                    G.Writeln2("*** ERROR: The 'Work' databank cannot be opened or closed (it is always open).");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Prim)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN <prim> with the 'Work' databank.");
                    G.Writeln("           If Work is not primary and it needs to be, you must CLOSE");
                    G.Writeln("           the present primary databank. After that, Work will be primary.");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Sec)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN <ref> with the 'Work' databank.");
                    G.Writeln("           It is not legal to set Work as the reference databank.");
                    G.Writeln("           Use the Ref databank for such purposes.");
                    throw new GekkoException();
                }     
            }
            else if (G.equal(databank.aliasName, Globals.Base))  //Ref
            {
                if (openType == EOpenType.Normal)
                {
                    G.Writeln2("*** ERROR: The '" + Globals.Base + "' databank cannot be opened or closed (it is always open).");                    
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Prim)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN <prim> with the '" + Globals.Base + "' databank.");                    
                    throw new GekkoException();                    
                }
                else if (openType == EOpenType.Sec)
                {
                    G.Writeln2("*** ERROR: You cannot use OPEN <ref> with the '" + Globals.Base + "' databank.");                    
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
                if (openType == EOpenType.Normal)
                {
                    G.Writeln2("*** ERROR: Databank '" + databank.aliasName + "' is already open. Use CLOSE to close it first.");
                    throw new GekkoException();
                }
                else if (openType == EOpenType.Prim)
                {
                    if (existI == 0) 
                    {
                        G.Writeln2("*** ERROR: Databank '" + databank.aliasName + "' is already open as primary.");
                        throw new GekkoException();
                    }
                    else if (existI == 1)  //Trying an OPEN<prim>db on a db that is already secondary (opened with OPEN<sec>db).
                    {                        
                        m.Add(this.storage[existI]);    //prim, = former sec
                        m.Add(this.storage[BaseI]);     //sec, = Base databank, to aviod empty slot
                        m.Add(this.storage[0]);         //former prim ends here
                        for (int i = 2; i < this.storage.Count; i++)
                        {                            
                            if (i == BaseI) continue;
                            m.Add(this.storage[i]);
                        }                        
                    }
                    else  //Trying an OPEN<prim>db on a db that is already there in slot [2] or below
                    {                        
                        m.Add(this.storage[existI]);         //prim
                        m.Add(this.storage[1]);              //sec, same
                        m.Add(this.storage[0]);
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == existI) continue;
                            m.Add(this.storage[i]);
                        }                                        
                    }
                    G.Writeln2("Databank '" + name + "' set as primary bank");
                }
                else if (openType == EOpenType.Sec)
                {
                    if (existI == 0) //Trying an OPEN<sec>db on a db that is already primary (opened with OPEN<prim>db).
                    {
                        m.Add(this.storage[WorkI]);    //prim, = Work databank, to aviod empty slot
                        m.Add(this.storage[existI]);   //sec, = former prim
                        m.Add(this.storage[1]);        //former sec ends here
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == WorkI) continue;
                            m.Add(this.storage[i]);
                        }
                    }
                    else if (existI == 1)
                    {
                        G.Writeln2("*** ERROR: Databank '" + databank.aliasName + "' is already open as reference bank");
                        throw new GekkoException();
                    }
                    else  //Trying an OPEN<prim>db on a db that is already there in slot [2] or below
                    {
                        m.Add(this.storage[0]);         //prim, same
                        m.Add(this.storage[existI]);    //sec
                        m.Add(this.storage[1]);
                        for (int i = 2; i < this.storage.Count; i++)
                        {
                            if (i == existI) continue;
                            m.Add(this.storage[i]);
                        }
                    }
                    G.Writeln2("Databank '" + name + "' set as reference bank");
                }
            }
            else  //the databank name does not exist, so it is new and will be read from file later on
            {
                readFromFile = true;
                if (openType == EOpenType.Normal)
                {                    
                    m.Add(this.storage[0]);  //prim
                    m.Add(this.storage[1]);  //sec
                    m.Add(databank);
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                    G.Writeln2("Databank '" + name + "' opened");                
                }
                else if (openType == EOpenType.Prim)
                {                    
                    m.Add(databank);         //prim
                    m.Add(this.storage[1]);  //sec
                    m.Add(this.storage[0]);
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                    G.Writeln2("Databank '" + name + "' opened as primary");
                }
                else if (openType == EOpenType.Sec)
                {
                    m.Add(this.storage[0]);         //prim
                    m.Add(databank);                //sec
                    m.Add(this.storage[1]);
                    for (int i = 2; i < this.storage.Count; i++) m.Add(this.storage[i]);
                    G.Writeln2("Databank '" + name + "' opened as reference");
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
                if (G.equal(Program.databanks.storage[i].aliasName, Globals.Base))
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
            if (G.equal(name, Globals.Base))
            {
                G.Writeln2("*** ERROR: " + Globals.Base + " databank cannot be closed");
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
            if (existI == 0) //found as primary
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
                if (G.equal(databank.aliasName, Globals.Base)) dbList.Add(databank);
            }
            foreach (Databank databank in Program.databanks.storage)
            {
                if (G.equal(databank.aliasName, Globals.Work)) continue;
                if (G.equal(databank.aliasName, Globals.Base)) continue;
                dbList.Add(databank);
            }
            Program.databanks.storage = dbList;            
        }
    }
}
