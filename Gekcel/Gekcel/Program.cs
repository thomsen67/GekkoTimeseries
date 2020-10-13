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
            <group id='group1' label='Gekko reading'>              
              <button id='button1a' imageMso='FileOpen' size='large' label='Read' onAction='OnButtonPressed1' />                   
            </group>
            <group id='group2' label='Gekko writing'>                            
              <button id='button2a' imageMso='FileSave' size='large' label='Write' onAction='OnButtonPressed2' />
            </group >
            <group id='group3' label='VBA calls'>   
              <button id='button3a' imageMso='MacroPlay' size='large' label='Run VBA' onAction='OnButtonPressed3' />  
            </group >
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
            // -----------------------------------------------------
            codeText += 
            @"
Public Sub Button1_Click()
  MsgBox ""Hi from Gekko""
End Sub

Public Sub h(word)
  MsgBox ""Hello from "" & word
End Sub

Public Function Gekko_GetData2(gbkFile As String, variableWithFreq As String, date2 As String) As Double
  dim gekko as Object
  set gekko = createobject(""Gekcel.COMLibrary"")
  Gekko_GetData2 = gekko.GetData2(gbkFile, variableWithFreq, date2)
End Function

Public Function Gekko_ThirtyDaysAgo() As Date
  dim gekko as Object
  set gekko = createobject(""Gekcel.COMLibrary"")
  Gekko_ThirtyDaysAgo = gekko.ThirtyDaysAgo()
End Function";

            codeText += "\r\n";
            // -----------------------------------------------------

            codeModule.InsertLines(lineNum, codeText);            

            targetExcelFile.Save();  //saves file

            // run the macros
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
        //TT: These are 
        
        //DFG: links regarding COM interface
        //https://github.com/Excel-DNA/Samples/tree/master/DnaComServer
        //https://brooklynanalyticsinc.com/2019/04/09/excel-dna-or-why-are-you-still-using-vba/
        //http://mikejuniperhill.blogspot.com/2014/03/interfacing-c-and-vba-with-exceldna_16.html
        
        //TT: Callable by VBA version, type this in cell: =Gekko_ThirtyDaysAgo()
        public DateTime ThirtyDaysAgo()
        {
            return DateTime.Today - TimeSpan.FromDays(30);
        }

        //TT: Callable by VBA version, type this in cell: =Gekko_GetData2("demo.gbk"; "x1!a"; "2000")
        public double GetData2(string gbkFile, string variableWithFreq, string date)
        {
            //TT: Same as Gekko_GetData(...), just callable from VBA
            Globals.excelDna = true;  //so that it does not try to print on screen etc.     
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
            Gekko.Series ts = db.GetIVariable(variableWithFreq) as Gekko.Series;
            GekkoTime gt = GekkoTime.FromStringToGekkoTime(date, true, true);
            double d = ts.GetDataSimple(gt);
            return d;
        }
    }

    public static class ExcelFunctionCalls
    {
        [ExcelFunction(Description = "Reverse string example from Gekko")]
        public static string Gekko_ReverseString([ExcelArgument(Name = "input_string", Description = "String to reverse")] string input_string)
        {
            string reversestring = "";
            int ilength = input_string.Length - 1;
            while (ilength >= 0)
            {
                reversestring = reversestring + input_string[ilength];
                ilength--;
            }
            return reversestring;            
        }

        [ExcelFunction(Name = "Gekko_Roundtrip", Description = "Roundtrip")]
        public static double Gekko_Roundtrip()
        {
            Globals.excelDna = true;  //so that it does not try to print on screen etc.
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            
            //Data
            Program.databanks.storage.Add(new Databank("Work"));
            Program.databanks.storage.Add(new Databank("Ref"));

            Gekko.Series ts2 = new Gekko.Series(EFreq.A, "testing1234" + "!a");
            ts2.SetData(new GekkoTime(EFreq.A, 2020, 1), 1234d);
            Program.databanks.GetFirst().AddIVariable("testing1234" + "!a", ts2);

            string fileName = "test.gbk";
            string fileName2 = "test2.gbk";
            string name = "y";
            double v = 54321d;
            int year = 2000;            
            string path = Path.Combine(Globals.excelDnaPath, fileName);
            string path2 = Path.Combine(Globals.excelDnaPath, fileName2);
            double d = double.NaN;

            if (true)
            {
                int i1 = Program.databanks.GetFirst().storage.Count;
                int i2 = Program.databanks.GetRef().storage.Count;                
                Program.obeyCommandCalledFromGUI(@"clone;", new P());
                int i3 = Program.databanks.GetFirst().storage.Count;
                int i4 = Program.databanks.GetRef().storage.Count;                
            }

            if (true)
            {
                //Setup Work databanks with series inside (used for writing)            
                Gekko.Series ts5 = new Gekko.Series(EFreq.A, name + "!a");
                ts5.SetData(new GekkoTime(EFreq.A, year, 1), v);
                Program.databanks.GetFirst().AddIVariable(name + "!a", ts5);
                InternalHelperMethods.WriteGbkDatabankToFile(path, Program.databanks.GetFirst());
                MessageBox.Show("Give Gekko a few seconds of pause to release the gbk file for writing");
                Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(path);  
                Gekko.Series ts = db.GetIVariable(name + "!a") as Gekko.Series;
                GekkoTime gt = new GekkoTime(EFreq.A, 2000, 1);
                d = ts.GetDataSimple(gt);
                MessageBox.Show("value of name: " + d);
            }
            return d;
        }

        //TT: Callable by VBA version, type this in cell: =Gekko_GetData2("demo.gbk"; "x1!a"; "2000")
        [ExcelFunction(Name = "Gekko_GetData", Description = "Gets a data value from a timeseries in a gbk databank file")]
        public static double Gekko_GetData([ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile, 
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

        [ExcelFunction(Name = "Gekko_SetData", Description = "Sets a data value in a timeseries in a gbk databank file")]        
        public static void Gekko_SetData([ExcelArgument(Name = "gbkFile", Description = "Absolute path and filename for gbk file")] string gbkFile, [ExcelArgument(Name = "variableWithFreq", Description = "Name of timeseries including frequency, for instance x!a or y!q")] string variableWithFreq, [ExcelArgument(Name = "date", Description = "Date, for instance 2020, 2020q2 or 2020m7")] string date, [ExcelArgument(Name = "value", Description = "Value of observation")] double value)
        {
            Globals.excelDna = true;  //so that it does not try to print on screen etc.                        
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile(gbkFile);
            Gekko.Series ts = db.GetIVariable(variableWithFreq) as Gekko.Series;
            GekkoTime gt = GekkoTime.FromStringToGekkoTime(date, true, true);
            ts.SetData(gt, value);
            InternalHelperMethods.WriteGbkDatabankToFile(gbkFile, db);
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
