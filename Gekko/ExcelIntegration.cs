using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDna.Integration;
using ProtoBuf;
using ProtoBuf.Meta;
using SevenZip;

using System.Threading.Tasks;

using System.Text.RegularExpressions;

using System.IO;

using System.Collections;
using System.Globalization;

using System.Threading;

using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;

using System.Data;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
//using SevenZip;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
//using ProtoBuf;
//using ProtoBuf.Meta;
//using System.Diagnostics;
using System.Collections.ObjectModel;

//using System.Linq;
using Gekko;
using ProtoBuf;

namespace Gekko
{
        
    public static class MyFunctions
    {
        [ExcelFunction(Name = "GEKKO_Roundtrip", Description = "Roundtrip")]
        public static double GEKKO_Roundtrip()
        {


            //NOTE: See c:\Thomas\slet regarding files. 7z.dll needs to be in same folder.

            //Data
            Program.databanks.storage.Add(new Databank("Work"));
            Program.databanks.storage.Add(new Databank("Ref"));

            Series ts2 = new Series(EFreq.A, "testing1234" + "!a");
            ts2.SetData(new GekkoTime(EFreq.A, 2020, 1), 1234d);
            Program.databanks.GetFirst().AddIVariable("testing1234" + "!a", ts2);

            string fileName = "test.gbk";
            string name = "y";
            double v = 54321d;
            int year = 2000;
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            string path = Path.Combine(Globals.excelDnaPath, fileName);
            double d = double.NaN;

            try
            {
                //this actually works, but when inside the call, it is a blank Gekko with no Work and Ref
                //databanks etc. So no state is transferred.
                Program.obeyCommandCalledFromGUI(@"edit 'c:\tools\dok.bat';", new P());
            }
            catch (Exception e)
            {

            }

            if (false)  //this works
            {

                //Setup Work databanks with series inside (used for writing)            
                Series ts5 = new Series(EFreq.A, name + "!a");
                ts5.SetData(new GekkoTime(EFreq.A, year, 1), v);
                Program.databanks.GetFirst().AddIVariable(name + "!a", ts5);

                //Write gbk file
                int i = Program.WriteGbk(Program.databanks.GetFirst(), new GekkoTime(EFreq.A, 1999, 1), new GekkoTime(EFreq.A, 2001, 1), path, false, null, null, true, false);

                //Read gbk file
                ReadOpenMulbkHelper oRead = new ReadOpenMulbkHelper();
                Program.ReadInfo info = new Program.ReadInfo();
                string tsdxFile = null;
                string tempTsdxPath = null;
                int NaNCounter = 0;
                Databank db = Program.GetDatabankFromFile(null, oRead, info, path, path, oRead.dateformat, oRead.datetype, ref tsdxFile, ref tempTsdxPath, ref NaNCounter);
                Series ts = db.GetIVariable(name + "!a") as Series;
                GekkoTime gt = new GekkoTime(EFreq.A, 2000, 1);
                d = ts.GetDataSimple(gt);
            }
            return d;
            
        }
    }
}
