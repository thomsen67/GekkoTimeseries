using System;
using System.Windows.Forms;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Gekko;
using ExcelDna.IntelliSense;
using ExcelDna.ComInterop;
using Extensibility;
//Test DFG
namespace Gekcel
{
    [ComVisible(true)]
    public class RibbonController : ExcelRibbon
    {
        static void Main(string[] args)
        {         
        }

        public override string GetCustomUI(string RibbonID)
        {
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
            Microsoft.Office.Interop.Excel.Application app = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;
            //Workbook wb = app.Workbooks[1];            
            Worksheet ws = (Worksheet)app.ActiveSheet;
            try
            {
                app.Run("h", "Gekko");
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }

            //The sheet must contain this macro:
            //----------------------------------
            //Sub h(word)
            //  d = Range("B2").Value
            //  MsgBox "Hello from " & word & ", value of B2 is: " & d
            //End Sub
            //----------------------------------
            
        }
    }


    //https://github.com/Excel-DNA/Samples/tree/master/DnaComServer
    //https://brooklynanalyticsinc.com/2019/04/09/excel-dna-or-why-are-you-still-using-vba/
    //http://mikejuniperhill.blogspot.com/2014/03/interfacing-c-and-vba-with-exceldna_16.html
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class COMLibrary
    {
        //TT: In order to be able to use this function on Excel cells, create this VBA code
        //    in your sheet. The code must be put under "Modules", same place as macros.
        //    Otherwise it does not show up when typing "=Ge..." in a cell.
        //
        //  Public Function Gekko_ThirtyDaysAgo() As Date
        //    dim gekko as Object
        //    set gekko = createobject("Gekcel.COMLibrary")
        //    Gekko_ThirtyDaysAgo = gekko.thirtydaysago()
        //  End Function        

        public DateTime ThirtyDaysAgo()
        {
            return DateTime.Today - TimeSpan.FromDays(30);
        }

        public double GetAbaseData(string startDate, string endDate, string frequency, string series)
            // DFG: Jeg får en fejl i den her funktion, det virker til at den ikke kan loade Gekko ordentligt. Gekko_-funktionerne virker heller
            // ikke, så det kan være det er noget med min maskine.
        {
            string seriesWithFrequency = series + "!" + frequency;

            Databank db = InternalHelperMethods.ReadGbkDatabankFromFile("c:\\abase.gbk");
            Gekko.Series ts = db.GetIVariable(seriesWithFrequency) as Gekko.Series;
            GekkoTime gt = GekkoTime.FromStringToGekkoTime(startDate, true, true);
            double d = ts.GetDataSimple(gt);
            return d;

        }
    }


    [ComVisible(true)]
    [Guid("a407a925-2ebf-4452-9c42-e431a382276f")]
    [ProgId("GekkoExcelComAddIn.ComAddIn")]
    public class GekkoExcelComAddIn : ExcelComAddIn
    {
        public void OnConnection(object Application,
                                 ext_ConnectMode ConnectMode,
                                 object AddInInst,
                                 ref Array custom)
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

            //NOTE: See c:\Thomas\slet regarding files. 7z.dll needs to be in same folder.

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
            Globals.excelDnaPath = Path.GetDirectoryName(ExcelDnaUtil.XllPath);
            string path = Path.Combine(Globals.excelDnaPath, fileName);
            string path2 = Path.Combine(Globals.excelDnaPath, fileName2);
            double d = double.NaN;

            if (true)
            {
                int i1 = Program.databanks.GetFirst().storage.Count;
                int i2 = Program.databanks.GetRef().storage.Count;
                MessageBox.Show("Before " + i1 + " " + i2);
                Program.obeyCommandCalledFromGUI(@"clone;", new P());
                int i3 = Program.databanks.GetFirst().storage.Count;
                int i4 = Program.databanks.GetRef().storage.Count;
                MessageBox.Show("After " + i3 + " " + i4);
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


}
