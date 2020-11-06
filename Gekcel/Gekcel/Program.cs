﻿using System;
using System.Windows.Forms;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Microsoft.Vbe.Interop;
using System.IO;
using Gekko;
using ExcelDna.IntelliSense;
using ExcelDna.ComInterop;
using Extensibility;
//Test DFG



namespace Gekcel
{
    // Running Gekcel:
    //
    // Note when compiling a new gekko.exe that this must be 32-bit. This and ANTLR.dll etc. should be put in the \Diverse\ExternalDllFiles folder.
    //
    // In this setup, we inject some VBA code into Excel, creating a Gekxel.xlsm file with this VBA code.
    // All this is done automatically, but you need to setup Excel to allow this kind of injection. This
    // is what you have to do (one time, not every time):
    //
    //   Go to Excel options (File --> More --> Options) (Filer --> Mere --> Indstillinger)
    //   Go to Trust Center ("Center for sikkerhed")
    //   Go to Trust Center Settings
    //   Go to Macro settings ("Indstillinger for makro")
    //   Check this: Trust access to the VBA object model ("Hav tillid til VBA-projektobjektmodellen")
    //
    // Next, do the following if setting up
    //
    // 1. Double-click Gekcel.xll
    // 2. If there is a security warning, click "activate"
    // 3. Now there should be a "Gekko" tab on the ribbon. Click this tab and click the "Setup" button
    //    You should get a message that the Gekko environment is set up.
    // 4. In a cell, type this: =Gekko_GetData1("demo"; "x1"; 2020)
    //    This should show the value 11
    //
    //
    // Else when deployed:
    //
    // 1. Open up Gekcel.xlsm (activate content)
    // 2. Drag Gekcel.xll on it
    // 3. If there is a security warning, click "activate"
    // 4. In a cell, type this: =Gekko_GetData1("demo"; "x1"; 2020)
    //    This should show the value 11. This requires that demo.gbk is in the same folder as the Gekcel.xlsm file.

    //
    //
    // https://stackoverflow.com/questions/14896215/how-do-you-set-the-value-of-a-cell-using-excel-dna

    /*
     http://www.eviews.com/download/whitepapers/EViews_COM_Automation.pdf

Run("%x = 100;")
Index("x*!*", "series", returnType), returnType = strings, array (1-dim). You cannot index across banks here.
ListToArray(nameString). Convert a comma-separated string into an array
ArrayToList(nameArray). Reverse
GetSeries(db, names, freq, per1, per2)     db, freq can be null. Names comma-separated. If freq!=null, a "!f" is added to names if no "!" in search. You cannot index across banks here.
  it returns a 2d area with names as rows and dates as cols and names.
  Operator!
SetSeries(db, names, freq, per1, per2, array)



     * 
     * */

    //
    //

    //TT: Inserting and calling VB code via the Ribbon ("play" button)
    //To do this, you must have this activated in Excel:
    //-------------------------------------------------------------

    //-------------------------------------------------------------

    [ComVisible(true)]
    public class RibbonController : ExcelRibbon
    {
        static void Main(string[] args)
        {         
        }

        public override string GetCustomUI(string RibbonID)
        {
            //This is the Ribbon interface (a 'Gekko' tab) that will show up in Excel
            return @"
<customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
  <ribbon>
    <tabs>
      <tab id='tab1' label='Gekko'>        
        <group id='group3' label='Setup'>   
          <button id='button3a' imageMso='MacroPlay' size='large' label='Setup' onAction='OnButtonPressed3' />  
        </group>            
<!-- 
        <group id='group1' label='Gekko reading'>              
          <button id='button1a' imageMso='FileOpen' size='large' label='Read' onAction='OnButtonPressed1' />                   
        </group>
        <group id='group2' label='Gekko writing'>                            
          <button id='button2a' imageMso='FileSave' size='large' label='Write' onAction='OnButtonPressed2' />
        </group>            
-->
      </tab>
    </tabs>
  </ribbon>
</customUI>";
        }

        public void OnButtonPressed1(IRibbonControl control)
        {
            MessageBox.Show("Read Gekko data // sets cell B2 = 12345");
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;                  
            Worksheet ws = (Worksheet)app.ActiveSheet;
            if (ws != null)
            {
                Microsoft.Office.Interop.Excel.Range cells = (Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 2];
                cells.Value2 = 12345d;
            }            
        }

        public void OnButtonPressed2(IRibbonControl control)
        {            
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;            
            Worksheet ws = (Worksheet)app.ActiveSheet;
            double d = double.NaN;
            if (ws != null)
            {
                Microsoft.Office.Interop.Excel.Range cells = (Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 2];
                d = cells.Value2;                
            }
            MessageBox.Show("Write Gekko data // value of cell D2 is: " + d);
        }

        public void OnButtonPressed3(IRibbonControl control)
        {
            InternalHelperMethods.Setup();
        }        
    }
    
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class COMLibrary
    {
        //DFG: links regarding COM interface
        //https://github.com/Excel-DNA/Samples/tree/master/DnaComServer
        //https://brooklynanalyticsinc.com/2019/04/09/excel-dna-or-why-are-you-still-using-vba/
        //http://mikejuniperhill.blogspot.com/2014/03/interfacing-c-and-vba-with-exceldna_16.html

        //TT: These methods just mirror the ones in the class ExcelFunctionCalls
        //TT: They are version that can be called from inside of VBA

        public double Gekko_GetData2(string gbkFile, string variableWithFreq, string date)
        {
            return ExcelFunctionCalls.Gekko_GetData1(gbkFile, variableWithFreq, date);
        }
                
        public double Gekko_SetData2(string gbkFile, string variableWithFreq, string date, double d)
        {
            return ExcelFunctionCalls.Gekko_SetData1(gbkFile, variableWithFreq, date, d);
        }

        public object[,] Gekko_GetSeries2(string gbkFile, string names, string freq, string t1, string t2)
        {
            return ExcelFunctionCalls.Gekko_GetSeries1(gbkFile, names, freq, t1, t2);
        }
        
    }

    public static class ExcelFunctionCalls
    {
        //TT: Type this in cell: =Gekko_Setup()
        [ExcelFunction(Name = "Gekko_Setup", Description = "Sets up Gekko environment")]
        public static double Gekko_Setup()
        {
            InternalHelperMethods.Setup();
            return 1d;
        }

        //TT: Type this in cell: =Gekko_GetData1("demo.gbk"; "x1!a"; "2020")
        [ExcelFunction(Name = "Gekko_GetData1", Description = "Gets a data value from a timeseries in a gbk databank file")]
        public static double Gekko_GetData1(
            [ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile,
            [ExcelArgument(Name = "variableWithFreq", Description = "Name of timeseries including frequency, for instance x!a or y!q")] string variableWithFreq,
            [ExcelArgument(Name = "date", Description = "Date, for instance 2020, 2020q2 or 2020m7")] string date)
        {            
            double d = double.NaN;            
            Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.          
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
            variableWithFreq = G.Chop_AddFreq(variableWithFreq, "a");
            Gekko.Series ts = db.GetIVariable(variableWithFreq) as Gekko.Series;
            if (ts == null)
            {
                MessageBox.Show("*** ERROR: Could not find timeseries '" + variableWithFreq + "' in '" + gbkFile + "' databank");
            }
            else
            {
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(date, true, true);
                d = ts.GetDataSimple(gt);
            }
            return d;
        }

        //TT: Type this in cell: =Gekko_SetData1("demo.gbk"; "x1!a"; "2020"; 777)
        [ExcelFunction(Name = "Gekko_SetData1", Description = "Sets a data value in a timeseries in a gbk databank file")]
        public static double Gekko_SetData1(
            [ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile, 
            [ExcelArgument(Name = "variableWithFreq", Description = "Name of timeseries including frequency, for instance x!a or y!q")] string variableWithFreq, 
            [ExcelArgument(Name = "date", Description = "Date, for instance 2020, 2020q2 or 2020m7")] string date, 
            [ExcelArgument(Name = "value", Description = "Value of observation")] double value)
        {            
            Program.PrepareExcelDna(Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.            
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);            
            variableWithFreq = G.Chop_AddFreq(variableWithFreq, "a");
            double rv = 0;
            Gekko.Series ts = db.GetIVariable(variableWithFreq) as Gekko.Series;            
            if (ts == null)
            {
                MessageBox.Show("*** ERROR: Could not find timeseries '" + variableWithFreq + "' in '" + gbkFile + "' databank");
            }
            else
            {
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(date, true, true);
                ts.SetData(gt, value);
                InternalHelperMethods.WriteGbkDatabankToFile(gbkFile, db);
                rv = 1d;
            }
            return rv; //1 for good, 0 for problem
        }
                
        [ExcelFunction(Name = "Gekko_GetSeries1", Description = "Gets a 2d array with rows of names and colums of periods")]
        public static object[,] Gekko_GetSeries1(
            [ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile,
            [ExcelArgument(Name = "names", Description = "Name of timeseries, can include wildcards and frequency, for instance 'x*!q' or 'x, y, z'")] string names,
            [ExcelArgument(Name = "freq", Description = "Frequency (optional), for instance 'a' or 'q'")] string freq,
            [ExcelArgument(Name = "t1", Description = "Starting date, for instance 2020, 2020q2 or 2020m7")] string t1,
            [ExcelArgument(Name = "t1", Description = "Ending date, for instance 2020, 2020q2 or 2020m7")] string t2)
        {
            Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.          
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
                        
            string[] ss = names.Split(',');
            int m = ss.Length;
            GekkoTime gt1 = new GekkoTime(EFreq.A, int.Parse(t1), 1);
            GekkoTime gt2 = new GekkoTime(EFreq.A, int.Parse(t2), 1);
            int n = GekkoTime.Observations(gt1, gt2);

            object[,] o = new object[m, n];

            int i = -1;            
            foreach (string s in ss)
            {
                i++;
                string varnameWithFreq = G.Chop_AddFreq(s.Trim(), freq);
                //o[j, i + 1] = d;
                Gekko.Series ts = db.GetIVariable(varnameWithFreq) as Gekko.Series;
                if (ts == null)
                {
                    MessageBox.Show("*** ERROR: Series '" + varnameWithFreq + "' was not found in databank '" + gbkFile + "'");
                }
                int j = -1;
                foreach (GekkoTime gt in new GekkoTimeIterator(gt1, gt2))
                {
                    j++;
                    double d = ts.GetDataSimple(gt);
                    o[i, j] = d;
                }
            }
            return o;             
        }
    }

    public static class InternalHelperMethods
    {
        public static void Setup()
        {
            //To recreate or alter demo.gbk, use the following .gcm code.
            //
            //    RESET;
            //    TIME 2020 2043;
            //    x1 = 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42;
            //    x2 = 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82;
            //    OPTION freq q; TIME 2020q1 2025q4;
            //    x1 = 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142;
            //    x2 = 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182;
            //    OPTION freq m; TIME 2020m1 2021m12;
            //    x1 = 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242;
            //    x2 = 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282;
            //    WRITE demo;
                        
            string demo = Path.GetDirectoryName(ExcelDnaUtil.XllPath) + "\\demo.gbk";
            string demo_orig = (new DirectoryInfo(ExcelDnaUtil.XllPath)).Parent.Parent.Parent.FullName + "\\Diverse\\ExternalDllFiles\\demo.gbk";
            
            if (File.Exists(demo))
            {
                try
                {
                    File.Delete(demo);
                }
                catch
                {
                    MessageBox.Show("*** ERROR: Could not delete file: " + demo);
                    throw new Exception();
                }
            }
            File.Copy(demo_orig, demo);

            //Next, handle the Gekcel.xlsm file, and inject VBA code into it.
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;
            Worksheet ws = (Worksheet)app.ActiveSheet;
            string excelFile = Path.GetDirectoryName(ExcelDnaUtil.XllPath) + "\\Gekcel.xlsm";
            string excelFile_orig = (new DirectoryInfo(ExcelDnaUtil.XllPath)).Parent.Parent.Parent.FullName + "\\Diverse\\ExternalDllFiles\\Gekcel_orig.xlsm";
            if (File.Exists(excelFile))
            {
                try
                {
                    File.Delete(excelFile);
                }
                catch
                {
                    MessageBox.Show("*** ERROR: Could not delete file: " + excelFile);
                    throw new Exception();
                }
            }
            File.Copy(excelFile_orig, excelFile);

            Workbook targetExcelFile = app.Workbooks.Open(excelFile);
            VBComponent newStandardModule = targetExcelFile.VBProject.VBComponents.Add(vbext_ComponentType.vbext_ct_StdModule);
            CodeModule codeModule = newStandardModule.CodeModule;

            string codeText = null;
            int lineNum = codeModule.CountOfLines + 1;
            // ---

            //
            // TT: The following code will be injected into an Excel sheet. It can just be formatted normally. Only exception is that
            //     double quotes like " must be doubled: "".
            //     Aligning at the left-most margin is intentional.
            //
            //     The functions starting with 'Gekko_' just mirror the methods in the class COMLibrary
            //
            // -----------------------------------------------------
            codeText +=
            @"
Public Sub h(word)
  MsgBox word
End Sub

Public Function Gekko_GetData2(gbkFile As String, variableWithFreq As String, date2 As String) As Double
  dim gekko as Object
  set gekko = createobject(""Gekcel.COMLibrary"")
  Gekko_GetData2 = gekko.Gekko_GetData2(gbkFile, variableWithFreq, date2)
End Function

Public Function Gekko_SetData2(gbkFile As String, variableWithFreq As String, date2 As String, d As Double) As Double
  dim gekko as Object
  set gekko = createobject(""Gekcel.COMLibrary"")
  Gekko_SetData2 = gekko.Gekko_SetData2(gbkFile, variableWithFreq, date2, d)  
End Function

Public Function Gekko_GetSeries2(gbkFile As String, names As String, freq As String, t1 as String, t2 as String) As Variant()
  dim gekko as Object
  set gekko = createobject(""Gekcel.COMLibrary"")
  Gekko_GetSeries2 = gekko.Gekko_GetSeries2(gbkFile, names, freq, t1, t2)
End Function

Sub Gekko_Populate()
  o = Gekko_GetSeries2(""demo"", ""x1"", ""a"", ""2020"", ""2021"")
  nrows = UBound(o, 1) - LBound(o, 1) + 1
  ncols = UBound(o, 2) - LBound(o, 2) + 1  
  Set rValues = Application.Range(""A1:A1"").Resize(nrows, ncols)
  rValues.ClearContents
  rValues.Value = o  
End Sub

";

            codeText += "\r\n";
            // -----------------------------------------------------

            codeModule.InsertLines(lineNum, codeText);
            targetExcelFile.Save();  //saves file

            if (true)
            {
                app.Run("h", "Gekko environment is set up and ready");
            }

            //app.Quit();
        }

        //These helper methods are not shown in Excel
        public static Databank ReadGbkDatabankFromFile(string gbkFile)
        {
            //Read gbk file
            SetWorkingFolderIfNullOrEmpty();
            gbkFile = GetFullPathNameWithExtension(gbkFile, ".gbk");
            ReadOpenMulbkHelper oRead = new ReadOpenMulbkHelper();
            Program.ReadInfo info = new Program.ReadInfo();
            string tsdxFile = null;
            string tempTsdxPath = null;
            int NaNCounter = 0;
            Databank db = null;            
            db = Program.GetDatabankFromFile(null, oRead, info, gbkFile, gbkFile, oRead.dateformat, oRead.datetype, ref tsdxFile, ref tempTsdxPath, ref NaNCounter);
            return db;
        }

        public static void WriteGbkDatabankToFile(string gbkFile, Databank db)
        {
            SetWorkingFolderIfNullOrEmpty();
            gbkFile = GetFullPathNameWithExtension(gbkFile, ".gbk");
            //TT: hmmm, the two following seem necessary...
            Program.databanks.storage = new System.Collections.Generic.List<Databank>();
            Program.databanks.storage.Add(db);
            int i = Program.WriteGbk(db, GekkoTime.tNull, GekkoTime.tNull, gbkFile, false, null, null, true, false);
        }

        private static string GetFullPathNameWithExtension(string gbkFile, string extension)
        {            
            gbkFile = Gekko.Program.AddExtension(gbkFile, extension);
            gbkFile = Gekko.Program.CreateFullPathAndFileName(gbkFile);
            return gbkFile;
        }


        private static void SetWorkingFolderIfNullOrEmpty()
        {
            if (string.IsNullOrEmpty(Program.options.folder_working)) Program.options.folder_working = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
        }


    }


    [ComVisible(true)]
    [Guid("a407a925-2ebf-4452-9c42-e431a382276f")]
    [ProgId("GekkoExcelComAddIn.ComAddIn")]
    public class GekkoExcelComAddIn : ExcelComAddIn
    {
        public void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom)
        {
            try
            {
                dynamic addIn = AddInInst;
                addIn.Object = new COMLibrary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
    }

    [ComVisible(false)]
    internal class ExcelAddin : IExcelAddIn
    {
        private GekkoExcelComAddIn com_addin;
        public void AutoOpen()
        {
            try
            {
                com_addin = new GekkoExcelComAddIn();
                ExcelComAddInHelper.LoadComAddIn(com_addin);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading COM AddIn: " + ex);
            }

            ComServer.DllRegisterServer();
            IntelliSenseServer.Install();
        }

        public void AutoClose()
        {
            ComServer.DllUnregisterServer();
            IntelliSenseServer.Uninstall();
        }
    }


}
