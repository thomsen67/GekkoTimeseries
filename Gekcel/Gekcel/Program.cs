using System;
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
    // Note when compiling a new gekko.exe that this SEEMS to have to be 
    // Debug|x86 (that is, debug 32-bit). Then gekko.exe, gekko.pdb and ANTLR.dll should be put in 
    // the \Diverse\ExternalDllFiles folder.
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


        CLS: See https://stackoverflow.com/questions/10203349/use-vba-to-clear-immediate-window


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

        public object[,] Gekko_GetGroup2(string gbkFile, string names, string freq, string t1, string t2)
        {
            return ExcelFunctionCalls.Gekko_GetGroup1(gbkFile, names, freq, t1, t2);
        }

        public string Gekko_SetGroup2(string gbkFile, string names, string freq, string t1, string t2, object[,] cells)
        {
            return ExcelFunctionCalls.Gekko_SetGroup1(gbkFile, names, freq, t1, t2, cells);
        }

        public double Gekko_Test2(object[,] cells)
        {
            return ExcelFunctionCalls.Gekko_Test1(cells);
        }

        public object[,] Gekko_Fetch2()
        {
            return ExcelFunctionCalls.Gekko_Fetch1();
        }

        public string Gekko_Run2(string commands)
        {
            return ExcelFunctionCalls.Gekko_Run1(commands);
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
                
        [ExcelFunction(Name = "Gekko_Run1", Description = "Run Gekko command(s)")]
        public static string Gekko_Run1(string commands)
        {            
            return InternalHelperMethods.Run(commands);
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
                
        [ExcelFunction(Name = "Gekko_GetGroup1", Description = "Gets a 2d array with rows of names and colums of periods")]
        public static object[,] Gekko_GetGroup1(
            [ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile,
            [ExcelArgument(Name = "names", Description = "Name of timeseries, can include wildcards and frequency, for instance 'x*!q' or 'x, y, z'")] string names,
            [ExcelArgument(Name = "freq", Description = "Frequency (optional), for instance 'a' or 'q'")] string freq,
            [ExcelArgument(Name = "t1", Description = "Starting date, for instance 2020, 2020q2 or 2020m7")] string t1,
            [ExcelArgument(Name = "t2", Description = "Ending date, for instance 2020, 2020q2 or 2020m7")] string t2)
        {
            Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.          
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
                        
            string[] ss = names.Split(',');
            int m = ss.Length;
            GekkoTime gt1 = new GekkoTime(EFreq.A, int.Parse(t1), 1);
            GekkoTime gt2 = new GekkoTime(EFreq.A, int.Parse(t2), 1);
            int n = GekkoTime.Observations(gt1, gt2);

            object[,] o = new object[m + 1, n + 1];
                        
            int i = -1;            
            foreach (string s in ss)
            {
                i++;                
                string varnameWithFreq = G.Chop_AddFreq(s.Trim(), freq);
                o[i + 1, 0] = varnameWithFreq;
                Gekko.Series ts = db.GetIVariable(varnameWithFreq) as Gekko.Series;
                if (ts == null)
                {
                    MessageBox.Show("*** ERROR: Series '" + varnameWithFreq + "' was not found in databank '" + gbkFile + "'");
                }
                int j = -1;
                foreach (GekkoTime gt in new GekkoTimeIterator(gt1, gt2))
                {
                    j++;
                    string date = gt.super.ToString();  //TODO!!
                    if (i == 0) o[0, j + 1] = date;
                    double d = ts.GetDataSimple(gt);
                    o[i + 1, j + 1] = d;
                }
            }
            return o;             
        }

        [ExcelFunction(Name = "Gekko_SetGroup1", Description = "Sets a 2d array with rows of names and colums of periods")]
        public static string Gekko_SetGroup1(
            [ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile,
            [ExcelArgument(Name = "names", Description = "Name of timeseries, can include wildcards and frequency, for instance 'x*!q' or 'x, y, z'")] string names,
            [ExcelArgument(Name = "freq", Description = "Frequency (optional), for instance 'a' or 'q'")] string freq,
            [ExcelArgument(Name = "t1", Description = "Starting date, for instance 2020, 2020q2 or 2020m7")] string t1,
            [ExcelArgument(Name = "t2", Description = "Ending date, for instance 2020, 2020q2 or 2020m7")] string t2,
            [ExcelArgument(Name = "cells", Description = "Excel cells as Variant array")] object[,] o)
        {
            Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.          
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);

            MessageBox.Show("hejsa");
            return "hejsa2";
        }

        [ExcelFunction(Name = "Gekko_Fetch1", Description = "Fetch")]
        public static object[,] Gekko_Fetch1()
        {
            var xxx = Globals.excelDnaData.clipData.data;
            object[,] xx = new object[2, 2];
            xx[0, 0] = 1d;
            xx[0, 1] = 2d;
            xx[1, 0] = 3d;
            xx[1, 1] = 4d;
            return xx;
            
        }

        [ExcelFunction(Name = "Gekko_Test1", Description = "Sets a 2d array with rows of names and colums of periods")]        
        public static double Gekko_Test1(object[,] cells)
        {
            //double sum = 0d;
            //The object starts with index 1 for both dimensions

            Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.          

            TableLight matrix = new TableLight();

            for (int i = 1; i < cells.GetLength(0) + 1; i++)
            {                
                for (int j = 1; j < cells.GetLength(1) + 1; j++)
                {                    

                    object cell = cells[i, j];

                    if(i==1 && j == 1)
                    {

                    }
                    else if (i == 1)
                    {
                        //dates                        
                        if (cell.GetType() == typeof(string))
                        {
                            string date = (string)cell;                            
                            CellLight cellLight = new CellLight(date); //as string
                            matrix.Add(i, j, cellLight);
                        }
                        else
                        {
                            MessageBox.Show("*** ERROR: expected date");
                            return 0;
                        }
                    }
                    else if (j == 1)
                    {
                        //names
                        if (cell.GetType() == typeof(string))
                        {
                            string name = (string)cell;
                            CellLight cellLight = new CellLight(name); //as string
                            matrix.Add(i, j, cellLight);
                        }
                        else
                        {
                            MessageBox.Show("*** ERROR: expected variable name");
                            return 0;
                        }
                    }
                    else
                    {

                        if (cell == null) continue;
                        if (cell.GetType() == typeof(double))
                        {
                            //MessageBox.Show("Added " + i + " " + j + "   " + (double)cell);
                            //sum += (double)cell;
                            double d = (double)cell;
                            CellLight cellLight = new CellLight(d); //as double
                            matrix.Add(i, j, cellLight);
                        }
                        else
                        {
                            MessageBox.Show("*** ERROR: expected variable name");
                            return 0;
                        }
                    }
                }
            }
            
            TableLight xx = matrix;
            string dateformat = null;
            string datetype = null;
            Program.ReadInfo readInfo = new Program.ReadInfo();
            Databank databank = new Databank("temp");
            ReadOpenMulbkHelper oRead = new ReadOpenMulbkHelper();
            oRead.Type = EDataFormat.Xlsx;            
            CellOffset cellOffset = new CellOffset();
            Program.options.freq = EFreq.Q;  //TODO TODO TODO
            //oRead.t1 = new GekkoTime(EFreq.Q, 2000, 1);
            //oRead.t2 = new GekkoTime(EFreq.Q, 2000, 1);
            try
            {
                Program.GetTimeseriesFromWorkbookMatrix(cellOffset, oRead, databank, matrix, readInfo, dateformat, datetype);
            }
            catch (Exception e)
            {

            }

            return 12345d;

        }
    }

    public static class InternalHelperMethods
    {
        public static int counter = 0;
        public static string gekcelError1 = "Gekcel terminated because of Gekko error(s)";
        public static string gekcelError2 = "Gekko error. See the error message in the 'Immediate' window in VBA. First open VBA (Alt+F11), then open 'Immediate' (Ctrl+G). You may move the 'Immediate' window to the top of the VBA window, or even make it free-floating.";

        public static void Restart()
        {
            counter++;
            Run("restart;");            
        }

        public static string Run(string commands)
        {
            if (counter == 0)
            {
                Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc.          
                SetWorkingFolderIfNullOrEmpty();
                Program.databanks.storage.Clear();
                Program.databanks.storage.Add(new Databank("Work"));
                Program.databanks.storage.Add(new Databank("Base"));

                Program.databanks.local.Clear();
                Program.databanks.global.Clear();
                Program.databanks.localGlobal = new LocalGlobal();
                Globals.commandMemory = new CommandMemory();
                Globals.gekkoInbuiltFunctions = Program.FindGekkoInbuiltFunctions();
                Program.InitUfunctionsAndArithmeticsAndMore();
                Program.model = new Gekko.Model();

                Program.GetStartingPeriod();
                Globals.lastPrtOrMulprtTable = null;
            }
            counter++;
            Globals.lastPrtOrMulprtTable = null;

            //TODO: To make lastPrtOrMulprtTable show up in cells, we need to make sure that 
            //      there is only one command issued. Or else it will be the last "table".

            string rv = null;

            try
            {
                Program.obeyCommandCalledFromGUI(commands, new P());
            }
            catch (Exception e)
            {
                //We have to catch this exception here, otherwise it ripples through to
                //Excel itself with a strange error message there.
                if (Globals.excelDnaStorage != null)
                {
                    Globals.excelDnaStorage.AppendLine();
                    Globals.excelDnaStorage.AppendLine(gekcelError1);
                }
            }
            finally
            {
                if (Globals.excelDnaStorage != null)
                {
                    rv = Globals.excelDnaStorage.ToString();
                }
            }            
            
            return rv;
        }

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

'Public Function Gekko_GetSeries2(gbkFile As String, names As String, freq As String, t1 as String, t2 as String) As Variant()
'  dim gekko as Object
'  set gekko = createobject(""Gekcel.COMLibrary"")
'  Gekko_GetSeries2 = gekko.Gekko_GetSeries2(gbkFile, names, freq, t1, t2)
'End Function

'Public Function Gekko_SetSeries2(gbkFile As String, names As String, freq As String, t1 as String, t2 as String, cells as Variant()) As Variant()
'  dim gekko as Object
'  set gekko = createobject(""Gekcel.COMLibrary"")
'  Gekko_SetSeries2 = gekko.Gekko_GetSeries2(gbkFile, names, freq, t1, t2, cells)
'End Function

'Sub Gekko_GetGroup2()
'  cells = Gekko_GetSeries2(""demo"", ""x1"", ""a"", ""2020"", ""2021"")
'  nrows = UBound(cells, 1) - LBound(cells, 1) + 1
'  ncols = UBound(cells, 2) - LBound(cells, 2) + 1  
'  Set rValues = Application.Range(""A1:A1"").Resize(nrows, ncols)
'  rValues.ClearContents
'  rValues.Value = cells
'End Sub

'Sub Gekko_SetGroup2()
'  nrows = Range(""A1"").SpecialCells(xlCellTypeLastCell).Row
'  ncols = Range(""A1"").SpecialCells(xlCellTypeLastCell).Column
'  Dim x1() as Variant  
'  Set x = Application.Range(""A1:A1"").Resize(nrows, ncols)  
'  x1 = x.Value
'  temp = Gekko_Test2(x1)
'End Sub

'Public Function Gekko_Test2(cells As Variant) As Double  
'  Dim gekko As Object
'  Set gekko = CreateObject(""Gekcel.COMLibrary"")
'  Gekko_Test2 = gekko.Gekko_Test2(cells)
'End Function

Public Function Gekko_Run2(commands As String) As String  
  Dim gekko As Object
  Set gekko = CreateObject(""Gekcel.COMLibrary"")  
  Gekko_Run2 = gekko.Gekko_Run2(commands)
  Debug.Print Gekko_Run2;
  If InStr(1, Gekko_Run2, """ + InternalHelperMethods.gekcelError1 + @""") <> 0 Then
    Err.Raise Number:=vbObjectError + 513, Description:=""" + gekcelError2 + @"""    '513 to not collide with Excel's own error numbers
  End If
End Function

Public Function Gekko_Fetch2() As Variant()
  dim gekko as Object
  set gekko = createobject(""Gekcel.COMLibrary"")
  Gekko_Fetch2 = gekko.Gekko_Fetch2()
End Function

'Sub Gekko_Insert2()
'  cells = Gekko_Fetch2()
'  nrows = UBound(cells, 1) - LBound(cells, 1) + 1
'  ncols = UBound(cells, 2) - LBound(cells, 2) + 1  
'  Set rValues = Application.Range(""A1:A1"").Resize(nrows, ncols)
'  rValues.ClearContents
'  rValues.Value = cells
'End Sub

Public Sub Gekko_Demo()
  Gekko_Run2 ""tell 'Hello from Gekko';""
  Gekko_Run2 ""time 2015 2020;""
  Gekko_Run2 ""x = 1, 2, 3, 4, 5, 6;""
  Gekko_Run2 ""sheet x;""
  Dim cells As Variant
  cells = Gekko_Fetch2()
  nrows = UBound(cells, 1) - LBound(cells, 1) + 1
  ncols = UBound(cells, 2) - LBound(cells, 2) + 1  
  Set rValues = Application.Range(""A1:A1"").Resize(nrows, ncols)
  rValues.ClearContents
  rValues.Value = cells
End Sub

";

            codeText += "\r\n";
            // -----------------------------------------------------

            codeModule.InsertLines(lineNum, codeText);
            targetExcelFile.Save();  //saves file

            if (true)
            {
                app.Run("h", @"Gekko environment is set up and ready. You may try these:\n=Gekko_Run(""prt <2020 2020> x1!a;"")\n=Gekko_Run(""x1!a[2020] = 888;"")");
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
