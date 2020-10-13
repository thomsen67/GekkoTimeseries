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
    //TT: Basically, we want to be able to have Excel use Gekko, but without opening Gekko and running 
    //    commands in Gekko. There are three ways of using Gekko methods from within Excel:
    //
    //      (1) Just typing "=" in a cell, and calling a Gekko method like "=Gekko_GetData(...)",
    //          returning some value
    //      (2) Calling the same Gekko_GetData(...) method from within a VB script inside Excel.
    //      (3) Using buttons in the Ribbon interface in Excel (point and click)
    //
    //    Bullet (1) comes directly from ExcelDNA, but to do (2), we need to use a so-called COM server.
    //
    //

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
        <group id='group3' label='VBA calls'>   
          <button id='button3a' imageMso='MacroPlay' size='large' label='Run VBA' onAction='OnButtonPressed3' />  
        </group>            
        <group id='group1' label='Gekko reading'>              
          <button id='button1a' imageMso='FileOpen' size='large' label='Read' onAction='OnButtonPressed1' />                   
        </group>
        <group id='group2' label='Gekko writing'>                            
          <button id='button2a' imageMso='FileSave' size='large' label='Write' onAction='OnButtonPressed2' />
        </group>            
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
            //TT: Inserting and calling VB code via the Ribbon ("play" button)
            //To do this, you must have this activated in Excel:
            //-------------------------------------------------------------
            //Go to Excel options
            //Go to Trust Center
            //Go to Trust Center Settings
            //Goto Macro settings
            //Check this: Trust access to the VBA object model
            //-------------------------------------------------------------

            //First, copy the demo.gbk file, so that it is easy to use
            //To recreate or alter this databank, use the following .gcm code.
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
Public Sub Button1_Click()
  MsgBox ""Hello1 from Gekko""
End Sub

Public Sub h(word)
  MsgBox ""Hello2 from "" & word
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
End Function";

            codeText += "\r\n";
            // -----------------------------------------------------

            codeModule.InsertLines(lineNum, codeText);
            targetExcelFile.Save();  //saves file

            //TT: Calling two of the VBA macros for testing            
            string macro = string.Format("{0}!{1}.{2}", targetExcelFile.Name, newStandardModule.Name, "Button1_Click");
            app.Run(macro);                        
            app.Run("h", "Gekko");  //h("Gekko")
            
            //app.Quit();

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

        //TT: Version that can be called by VBA
        public double Gekko_GetData2(string gbkFile, string variableWithFreq, string date)
        {
            return ExcelFunctionCalls.Gekko_GetData1(gbkFile, variableWithFreq, date);
        }

        //TT: Version that can be called by VBA
        public double Gekko_SetData2(string gbkFile, string variableWithFreq, string date, double d)
        {
            return ExcelFunctionCalls.Gekko_SetData1(gbkFile, variableWithFreq, date, d);
        }
    }

    public static class ExcelFunctionCalls
    {
        //TT: Type this in cell: =Gekko_GetData1("demo.gbk"; "x1!a"; "2020")
        [ExcelFunction(Name = "Gekko_GetData1", Description = "Gets a data value from a timeseries in a gbk databank file")]
        public static double Gekko_GetData1(
            [ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile,
            [ExcelArgument(Name = "variableWithFreq", Description = "Name of timeseries including frequency, for instance x!a or y!q")] string variableWithFreq,
            [ExcelArgument(Name = "date", Description = "Date, for instance 2020, 2020q2 or 2020m7")] string date)
        {
            Globals.excelDna = true;  //so that it does not try to print on screen etc.                        
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
            Gekko.Series ts = db.GetIVariable(variableWithFreq) as Gekko.Series;
            GekkoTime gt = GekkoTime.FromStringToGekkoTime(date, true, true);
            double d = ts.GetDataSimple(gt);
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
            Globals.excelDna = true;  //so that it does not try to print on screen etc.                        
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            if (true)
            {
                //TT: hack because of path problem when writing
                if(gbkFile.Contains(":")|| gbkFile.Contains("\\") || gbkFile.Contains("/"))
                {
                    //TT: has path
                }
                else
                {
                    gbkFile = Path.GetDirectoryName(ExcelDnaUtil.XllPath) + "\\" + gbkFile;
                }
            }
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
            Gekko.Series ts = db.GetIVariable(variableWithFreq) as Gekko.Series;
            GekkoTime gt = GekkoTime.FromStringToGekkoTime(date, true, true);
            ts.SetData(gt, value);
            InternalHelperMethods.WriteGbkDatabankToFile(gbkFile, db);
            return 1d; //1 for 'true'
        }                
    }

    public static class InternalHelperMethods
    {
        //These helper methods are not shown in Excel
        public static Databank ReadGbkDatabankFromFile(string gbkFile)
        {
            //Read gbk file
            ReadOpenMulbkHelper oRead = new ReadOpenMulbkHelper();
            Program.ReadInfo info = new Program.ReadInfo();
            string tsdxFile = null;
            string tempTsdxPath = null;
            int NaNCounter = 0;
            Databank db = null;
            try
            {
                db = Program.GetDatabankFromFile(null, oRead, info, gbkFile, gbkFile, oRead.dateformat, oRead.datetype, ref tsdxFile, ref tempTsdxPath, ref NaNCounter);
            }
            catch (Exception e)
            {

            }
            return db;
        }

        public static void WriteGbkDatabankToFile(string gbkFile, Databank db)
        {
            //TT: hmmm, the two following seem necessary...
            Program.databanks.storage = new System.Collections.Generic.List<Databank>();            
            Program.databanks.storage.Add(db);
            int i = Program.WriteGbk(db, GekkoTime.tNull, GekkoTime.tNull, gbkFile, false, null, null, true, false);
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
