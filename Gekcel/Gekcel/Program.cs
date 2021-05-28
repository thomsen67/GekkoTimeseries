using System;
using System.Windows.Forms;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.Office.Interop.Excel;
using Microsoft.Vbe.Interop;
using System.Threading;
using System.IO;
using Gekko;
using ExcelDna.IntelliSense;
using ExcelDna.ComInterop;
using Extensibility;
using System.Diagnostics;

//About 64-bit, maybe see this: https://colinlegg.wordpress.com/2016/09/07/my-first-c-net-udf-using-excel-dna-and-visual-studio/

//TODO: prt should work. Maybe wipe cells before sheet/prt? Optional. Starting cell?
//      sheet does not work with <cols>, transposed, fix
//      cls should wipe, and be careful
//      special gekcel commands?? SHEET without filename, filename is stupid.
//      Other commands like TELL to decorate cells?
//      Option to aviod overwrites.
//      Buttons and clicks etc.
//      Setting up immediate window, activate if not activated, focus, perhaps clear. Putting it on top??
//      How to issue commands one by one?
//      Do plots work??
//      Decorate Excel with Gekko version number.
//      See what EViews does.

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
    // 4. Run the macro "Demo"   
    //
    // Else when deployed:
    //
    // 1. Open up Gekcel.xlsm (activate content)
    // 2. Drag Gekcel.xll on it
    // 3. If there is a security warning, click "activate"
    // 4. Run the macro "Demo"       //
    //
    // https://stackoverflow.com/questions/14896215/how-do-you-set-the-value-of-a-cell-using-excel-dna
    // http://www.eviews.com/download/whitepapers/EViews_COM_Automation.pdf   
    // CLS: See https://stackoverflow.com/questions/10203349/use-vba-to-clear-immediate-window
    
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
            //do Link to guided tour in ribbon
            //This is the Ribbon interface (a 'Gekko' tab) that will show up in Excel
            return @"
<customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
  <ribbon>
    <tabs>
      <tab id='tab1' label='Gekko'>        
        <group id='group3' label='Info'>   
          <button id='button3a' imageMso='Info' size='large' label='Gekko/Gekcel' onAction='OnButtonPressed3' />  
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

        /// <summary>
        /// When info icon is pressed in Gekko Ribbon add-in, the Gekcel guide opens up.
        /// </summary>
        /// <param name="control"></param>
        public void OnButtonPressed3(IRibbonControl control)
        {
            //This way of starting a process will work in .NET Core too.
            var psi = new ProcessStartInfo
            {
                FileName = @"https://www.t-t.dk/gekko/docs/user-manual/index.html?excel_addin_gekcel.htm",
                UseShellExecute = true
            };
            Process.Start(psi);
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

        public object[,] Gekko_Get()
        {
            return ExcelFunctionCalls.Gekko_Get();
        }        

        public double Gekko_Put(object[,] cells, int baseOfArrayZeroOrOne)
        {
            return ExcelFunctionCalls.Gekko_Put(cells, baseOfArrayZeroOrOne);
        }

        public string Gekko(string commands, string path)
        {
            return ExcelFunctionCalls.Gekko(commands, path);
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

        //[ExcelFunction(Name = "Gekko_Deleteme", Description = "Delete this")]
        //public static string Gekko_Deleteme()
        //{
        //    return ExcelFunctionCalls.CallingFileName();
        //}

        [ExcelFunction(Name = "Gekko_Get", Description = "Transfers data from a Gekko databank to sheet cells")]
        public static object[,] Gekko_Get()
        {
            return Globals.excelDnaData.cells;
        }        

        [ExcelFunction(Name = "Gekko_Put", Description = "Transfers data from sheet cells to a Gekko databank (baseOfArray can be index = 0 or 1)")]
        public static double Gekko_Put(object[,] cells, int baseOfArrayZeroOrOne)
        {            
            Program.PrepareExcelDna(Path.GetDirectoryName(ExcelDnaUtil.XllPath)); //necessary for it to run ANTLR etc. MUST use GetDirectoryName()          
            
            TableLight matrix = new TableLight();  //1-based

            int offset = 1 - baseOfArrayZeroOrOne;

            for (int i = baseOfArrayZeroOrOne; i < cells.GetLength(0) + baseOfArrayZeroOrOne; i++)
            {
                for (int j = baseOfArrayZeroOrOne; j < cells.GetLength(1) + baseOfArrayZeroOrOne; j++)
                {
                    object cell = cells[i, j];

                    if (cell == null) continue;

                    int ii = i + offset;
                    int jj = j + offset;

                    if (cell.GetType() == typeof(double))
                    {
                        matrix.Add(ii, jj, new CellLight((double)cell)); //as double
                    }
                    else if (cell.GetType() == typeof(int))
                    {
                        matrix.Add(ii, jj, new CellLight((double)((int)cell))); //as double
                    }
                    else if (cell.GetType() == typeof(string))
                    {
                        matrix.Add(ii, jj, new CellLight((string)cell)); //as string
                    }
                    else if (cell.GetType() == typeof(DateTime))
                    {
                        MessageBox.Show("*** ERROR: Date cells are not supported (yet)");
                        throw new Exception();
                    }
                }
            }

            Globals.excelDnaData = new ExcelDnaData();
            Globals.excelDnaData.tableLight = matrix;
            
            return 12345d;
        }

        [ExcelFunction(Name = "Gekko", Description = "Run Gekko command(s)")]
        public static string Gekko(string commands, string path)
        {
            return InternalHelperMethods.Run(commands, path);
        }

        ///// <summary>
        ///// Taken from here: https://microsoft.public.excel.sdk.narkive.com/NJhRTpmT/obtaining-the-full-path-of-a-workbook-from-an-xll
        ///// </summary>
        ///// <returns></returns>
        //[ExcelFunction(IsMacroType = true)]
        //public static string CallingFileName()
        //{
        //    string s = null;
        //    try
        //    {
        //        ExcelReference reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
        //        string sheetName = (string)XlCall.Excel(XlCall.xlSheetNm, reference);
        //        object o1 = XlCall.Excel(XlCall.xlfGetDocument, 2, sheetName);
        //        object o2 = XlCall.Excel(XlCall.xlfGetDocument, 88, sheetName);
        //        string s1 = (string)o1;
        //        string s2 = (string)o2;
        //        s = System.IO.Path.Combine(s1, s2);
        //    }
        //    catch { }
        //    return s;
        //}
    }

    public static class InternalHelperMethods
    {
        public static int counter = 0;
        public static string gekcelError1 = "Gekcel terminated because of Gekko error(s)";
        public static string gekcelError2 = "Gekko error. See the error message in the 'Immediate' window in VBA. First open VBA (Alt+F11), then open 'Immediate' (Ctrl+G). You may move the 'Immediate' window to the top of the VBA window, or even make it free-floating.";
                
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

Public Function Gekko(commands As String) As String  
  Dim gekcel As Object
  Set gekcel = CreateObject(""Gekcel.COMLibrary"")
  Gekko = gekcel.Gekko(commands, Application.ActiveWorkbook.Path)
  Debug.Print Gekko;
  If InStr(1, Gekko, """ + InternalHelperMethods.gekcelError1 + @""") <> 0 Then
    Err.Raise Number:=vbObjectError + 513, Description:=""" + gekcelError2 + @"""    '513 to not collide with Excel's own error numbers
  End If
End Function

Public Sub Gekko_Get()
  Dim cells As Variant  
  cells = CreateObject(""Gekcel.COMLibrary"").Gekko_Get()
  cells = Handle_Cells(cells)
  nrows = UBound(cells, 1) - LBound(cells, 1) + 1
  ncols = UBound(cells, 2) - LBound(cells, 2) + 1
  Set rValues = Application.Range(""A1:A1"").Resize(nrows, ncols)
  rValues.ClearContents
  rValues.Value = cells
End Sub

'Get data from a Gekko databank into a 2D array of Excel cells (rows = variables, cols = periods)
'Example: mark an area of cells where you want to put the data (at least 2x2, but should probably be larger)
'While the area is selected, type this: =Gekko_GetGroup(""demo""; ""a""; ""2020""; ""2025""; """"; """"; ""x1""; ""x2"")
'Instead of finishing with [Enter], use [Ctrl]+[Shift]+[Enter]. This way, it is used as an array formula, and the
'formula will be shown with {}-curlies in Excel.
'Parameters:
'databank (string): State the .gbk databank, for instance ""demo"" or ""c:\data\demo.gbk""
'freq (string):     State the frequency of the variables, for instance ""q""
'per1 (string):     Starting period, for instance ""2020q1""
'per2 (string):     Ending period, for instance ""2021q3""
'operator (string): Gekko operators, for instance ""p"" for percent growth, or ""n p"" for both levels and percent growth.
'options (string):  Not used yet
'names (string):    Names of timeseries, for instance ""x1, x2, x3"". You may also use ""x1""; ""x2""; ""x3"" instead.
Public Function Gekko_GetGroup(databank As String, freq As String, per1 As String, per2 As String, operator As String, options As String, ParamArray names() As Variant) As Variant
  args = "": comma = ""
  For Each name In names: args = args & comma & name: comma = "", "": Next name
  Gekko ""read "" & databank & "";""
  Gekko ""option freq = "" & freq & "";""
  Gekko ""time "" & per1 & "" "" & per2 & "";""
  Gekko ""sheet <"" & operator &""> "" & args & "";""
  Dim cells As Variant
  cells = CreateObject(""Gekcel.COMLibrary"").Gekko_Get()
  cells(0, 0) = """"
  cells = Handle_Cells(cells)
  Gekko_GetGroup = cells
End Function

Public Sub Gekko_Put()
  nrows = Range(""A1"").SpecialCells(xlCellTypeLastCell).Row
  ncols = Range(""A1"").SpecialCells(xlCellTypeLastCell).Column  
  Set x = Application.Range(""A1:A1"").Resize(nrows, ncols)
  Dim x1() As Variant
  x1 = x.Value  
  Dim temp As Variant  
  temp = CreateObject(""Gekcel.COMLibrary"").Gekko_Put(x1, 1)
End Sub

Public Sub Gekko_Demo()  
  Gekko ""tell gekkoinfo('short4');""
  Gekko ""time 2015 2020;""
  Gekko ""x1 = 1, 2, 3, 4, 5, 6;""
  Gekko ""x2 = 2, 3, 4, 5, 6, 7;""
  Gekko ""clone;""
  Gekko ""sheet x1, x2;""
  Gekko_Get  
  Range(""C2"").Value = 2.1
  Range(""D3"").Value = 3.9
  Gekko_Put
  Gekko ""import <xlsx> gekcel;""
  Gekko ""compare file=compare.txt;""
  Gekko ""edit compare.txt;""
End Sub

'Handles NaN values in Excel
Private Function Handle_Cells(ByRef cells As Variant)
  For i = LBound(cells, 1) To UBound(cells, 1)
    For j = LBound(cells, 2) To UBound(cells, 2)
      'NaN values from Gekko get represented as -1.#IND in Excel, therefore this.
      If InStr(CStr(cells(i, j)), ""#IND"") <> 0 Then cells(i, j) = CVErr(xlErrNA)
    Next j
  Next i
  Handle_Cells = cells
End Function

";

            codeText += "\r\n";
            // -----------------------------------------------------

            codeModule.InsertLines(lineNum, codeText);
            targetExcelFile.Save();  //writes the .xlsm file with the VBA-macro included. The file is used in both Gekcel 32- and 64-bit.

            //app.Quit();
        }                        

        public static string Run(string commands, string xlsmPath)
        {
            if (counter == 0)
            {
                //TODO: set this up in a better way, using Program.Re() or RESTART
                //#7980345743573
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;                
                Program.PrepareExcelDna2(Path.GetDirectoryName(ExcelDnaUtil.XllPath), xlsmPath);
            }

            counter++;
            Globals.lastPrtOrMulprtTable = null;

            //TODO: To make lastPrtOrMulprtTable show up in cells, we need to make sure that 
            //      there is only one command issued. Or else it will be the last "table".

            string rv = null;

            try
            {
                Program.RunGekkoCommands(commands, "", 0, new P());
            }
            catch (Exception e)
            {
                //We have to catch this exception here, otherwise it ripples through to
                //Excel itself with a strange error message there.
                if (Globals.excelDnaOutput != null)
                {
                    Globals.excelDnaOutput.AppendLine();
                    Globals.excelDnaOutput.AppendLine(gekcelError1);
                }
            }
            finally
            {
                if (Globals.excelDnaOutput != null)
                {
                    rv = Globals.excelDnaOutput.ToString();
                }
            }

            return rv;
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
                MessageBox.Show("*** ERROR: " + ex);
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
                MessageBox.Show("*** ERROR: loading COM AddIn: " + ex);
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
