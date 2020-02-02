using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDna.Integration;
using ProtoBuf;
using ProtoBuf.Meta;
using SevenZip;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
//using System;

using System.IO;
//using System.Text;
//using System.Collections.Generic;
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
    
    [ProtoContract]
    [ProtoInclude(1, typeof(Series))]
    public class PTest
    {     

        [ProtoMember(1)]
        public GekkoDictionary<string, PTest2> xx = new GekkoDictionary<string, PTest2>(StringComparer.OrdinalIgnoreCase);
    }

    
    [ProtoContract]
    [ProtoInclude(1, typeof(Series))]
    public class PTest2
    {
        [ProtoMember(2)]
        public GekkoDictionary<string, IVariable> xx = new GekkoDictionary<string, IVariable>(StringComparer.OrdinalIgnoreCase);

    }

    public static class MyFunctions
    {

        [ExcelFunction(Name = "GEKKO_ReadData", Description = "Get data from Gekko databank")]
        public static double GEKKO_ReadData(string s)
        {



            ReadOpenMulbkHelper oRead = new ReadOpenMulbkHelper();
            //oRead.Type = EDataFormat.Tsd;
            Program.ReadInfo info = new Program.ReadInfo();
            //string file = @"c:\Thomas\Desktop\gekko\testing\sim.gbk";
            string file1 = @"c:\Thomas\Desktop\gekko\testing\jul05.gbk";
            string file = @"c:\Thomas\Desktop\gekko\testing\test.gbk";
            string tsdxFile = null;
            string tempTsdxPath = null;
            int NaNCounter = 0;


            Program.databanks = new Databanks();
            Program.databanks.storage.Add(new Databank("Work"));
            Program.databanks.storage.Add(new Databank("Ref"));
            Series ts2 = new Series(EFreq.A, s + "!a");
            ts2.SetData(new GekkoTime(EFreq.A, 2000, 1), 777d);
            Program.databanks.GetFirst().AddIVariable(s + "!a", ts2);
            
            int i = Program.WriteGbk(Program.databanks.GetFirst(), new GekkoTime(EFreq.A, 1999, 1), new GekkoTime(EFreq.A, 2001, 1), file, false, null, null, true, false);
            
            double d = double.NaN;

            if (true)
            {
                Databank db = Program.GetDatabankFromFile(null, oRead, info, file, file, oRead.dateformat, oRead.datetype, ref tsdxFile, ref tempTsdxPath, ref NaNCounter);
                Series ts = db.GetIVariable(s + "!a") as Series;
                GekkoTime gt = new GekkoTime(EFreq.A, 2006, 1);
                double x = ts.GetDataSimple(gt);
                return x;
            }

        }
    }
}
