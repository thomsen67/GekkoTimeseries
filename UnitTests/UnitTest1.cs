/*
 *
 *

 * cached models get tested a couple of placed (adam workshop and modelrandom). when starting unit testing,
 * all temp files are deleted (also cache mdl-files).

Test_Assign()               Test of VAL, DATE, STRING
Test_Collapse()             COLLAPSE
Test_Databanks()            Test of tsd, tsdx (old and new)
Test_Expressions()          SERIES y = @x(2010), stuff like that
Test_ForwardLooking()       SIM of small models with leads
Test_List()                 LIST commands
Test_Open()                 OPEN command
Test_Period()
Test_PipeAndTell()
Test_Print()
Test_Res()
Test_Time()
Test_Timefilter()
Test_TimeSeries()
Test_UpdAndGenr()
Test_Updx()

Test_ADAMworkshop2011()     Test of a lot of exercises
Test_EnsJJUST()             Test of ENDO/EXO
Test_ModelJul05()           Test SIM of that model
Test_ModelApr07()           Test SIM of that model
Test_ModelApr08()           Test SIM of that model
Test_ModelLille1()          Small model that had some problems at one point
Test_ModelLille2()          Small model that had some problems at one point
Test_ModelLille3()          Small model that had some problems at one point
Test_ModelLille4()          Small model that had some problems at one point

Integration_FM2010()
Integration_FM2012()
Integration_ModelRandom()
Integration_OEM2012()

 *
 *
 * Unit tests for Gekko.
 *
 * Major issues:
 * --------------------------------------------------------------------------------------------------------------
 * 1. Test_ModelLille4(): solves only with backtrack=no
 * 2. Test_ModelApr08(): why do we have to set reorder=no to get the model running?? (and newton does not work)
 * 3. Some of Integration_ModelRandom() fail (newton stall), and 2 have strange model structure problem
 * 4. There are some of the variables in TestKP2010Model
 * 5. In Test_ADAMworkshop2011(), why is it that it is
 *    necessary with reorder=no to be able to simulate the first and last one of those?
 *    Also: check and integrate gekkotest.cmd!
 * 6. Get all UPD operators tested, also $ operator.
 * 7. Should test "terminal growth", but is probably ok.
 * --------------------------------------------------------------------------------------------------------------
 *
 *
 * */

//Assert.AreEqual(1d, double.NaN);                       NOPASS
//Assert.AreEqual(1d, double.NaN, sharedDelta);          PASS <----- !!!!problem
//Assert.AreEqual(double.NaN, double.NaN);               PASS
//Assert.AreEqual(double.NaN, double.NaN, sharedDelta);  PASS
// !!====> so remember no threshold for Nan, or use the one below:
//Assert.IsTrue(double.IsNaN(xxx));
//From time to time, maybe check for occurences of "double.NaN,"

using System;
using System.Text;
using System.Collections.Generic;
//using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gekko;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Antlr.Runtime.Debug;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace UnitTests
{

    public class StringOrList
    {
        public string s = null;
        public StringOrList[] ss = null;

        public StringOrList()
        {
        }

        public StringOrList(params string[] strings)
        {
            StringOrList[] temp = new StringOrList[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                StringOrList temp2 = new StringOrList();
                temp2.s = strings[i];
                temp[i] = temp2;
            }
            this.ss = temp;
        }

        public void Add(string s)
        {
            this.s = s;
        }
        public void Add(StringOrList[] ss)
        {
            this.ss = ss;
        }
    }

    

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // May add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void UnitTest1Initialize(TestContext testContext)
        {
            Globals.unitTestIntegration = false;
            if (false)
            {
                DialogResult result = MessageBox.Show("Include slow INTEGRATION tests?", "Gekko unit tests", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No) Globals.unitTestIntegration = false;
                else Globals.unitTestIntegration = true;
            }

            Program.databanks.storage = new List<Databank>();
            Program.databanks.storage.Add(new Databank(Globals.Work));
            Program.databanks.storage.Add(new Databank(Globals.Ref));

            Program.CreateTempFilesFolder();
            
            string s = Globals.localTempFilesLocation;
            if (!(s.Contains("AppData") && s.Contains("tempfiles")))
            {
                MessageBox.Show("Tried to delete this folder: " + s + "\nBUT WE DO NOT ALLOW THAT (does not contain 'AppData')");
            }
            else
            {
                if (true)  //sometimes good to try setting temporary to false, to force running models on cached files.
                {
                    Program.DeleteFolder(Globals.localTempFilesLocation);
                }
                else
                {
                    MessageBox.Show("HOVHOV - skal denne ikke slås fra........??");
                }
            }

            //We set a default regarding this folder -- not good if the string is empty
            //and we try to write a file etc.
            Program.options.folder_working = Globals.ttPath2 + @"\regres\working";

            Globals.globalPeriodStart = new GekkoTime(EFreq.A, 2000, 1);
            Globals.globalPeriodEnd = new GekkoTime(EFreq.A, 2010, 1);

            Globals.gekkoInbuiltFunctions = Program.FindGekkoInbuiltFunctions();  //uses reflection to do this

            Program.InitUfunctionsAndArithmeticsAndMore();

            //Globals.unitTestCounter++;
            //if (Globals.unitTestWindow == null)
            //{
            //    Thread thread = new Thread(() =>
            //    {
            //        Globals.unitTestWindow = new SimpleUnitTestWindow();
            //        Globals.unitTestWindow.Show();
            //        //Globals.unitTestWindow.Closed += (sender2, e2) =>
            //        //Globals.unitTestWindow.Dispatcher.InvokeShutdown();
            //        System.Windows.Threading.Dispatcher.Run();
            //    });
            //    thread.SetApartmentState(ApartmentState.STA);
            //    thread.Start();

            //    System.Threading.Thread.Sleep(1000);  //wait 1 sec. to make sure the window is finished coming up.
            //    //better would be to check for this, but that would demand a delegate...:
            //    //for (int i = 0; i < 100; i++)
            //    //{
            //    //    if (Globals.unitTestWindow != null && Globals.unitTestWindow.IsLoaded) break;
            //    //    System.Threading.Thread.Sleep(1000);  //wait 1 sec.
            //    //    System.Windows.Forms.MessageBox.Show("Waited " + (i + 1) + " seconds");
            //    //}


            //    //Globals.unitTestWindow = new SimpleUnitTestWindow();
            //    //Globals.unitTestWindow.Show();
            //}
        }

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Globals.threadIsInProcessOfAborting = false;
            Globals.applicationIsInProcessOfAborting = false; 
        }

        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        public static double sharedDelta = 0.00000000001d;  //precision for accepting
        double sharedTableDelta = 0.0001d;  //printing

        private static void _AssertListString(IBank db, string s, StringOrList ss)
        {
            List iv_list = db.GetIVariable(s) as List;
            AssertListStringHelper(iv_list, ss.ss);
        }

        private static void AssertListStringHelper(List iv_list, StringOrList[] ss)
        {
            if (iv_list.list.Count != ss.Length) Assert.IsFalse(true);
            for (int i = 0; i < iv_list.list.Count; i++)
            {
                IVariable item = iv_list.list[i];
                if (item.Type() == EVariableType.String)
                {
                    if (ss[i].s == null) Assert.IsFalse(true);
                    if (!G.Equal(ss[i].s, item.ConvertToString())) Assert.IsFalse(true);
                }
                else if (item.Type() == EVariableType.List)
                {
                    if (ss[i].ss == null) Assert.IsFalse(true);
                    AssertListStringHelper(item as List, ss[i].ss);
                }
                else Assert.IsFalse(true);                
            }            
        }

        [TestMethod]
        public void _Test_ListAndStringFunctions()
        {
            //-------------------------------------------------------------------------
            //-----------  assignment no sideeffects
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("#m2 = #m1;");
            I("#m1[1] = 'x';");
            _AssertListString(First(), "#m1", new StringOrList("x", "b", "c"));
            _AssertListString(First(), "#m2", new StringOrList("a", "b", "c"));

            //-------------------------------------------------------------------------
            //-----------  IN logic 
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("%v = 0;");
            I("if('b' in #m1); %v = 1; end;");
            _AssertScalarVal(First(), "%v", 1d);
            I("%v = 0;");
            I("if('d' in #m1); %v = 1; end;");
            _AssertScalarVal(First(), "%v", 0d);
            I("%v = 0;");
            I("%s = 'a';");
            I("if(%s in #m1); %v = 1; end;");
            _AssertScalarVal(First(), "%v", 1d);
            I("%v = 0;");
            I("if(#m1.contains(%s) == 1); %v = 1; end;");
            _AssertScalarVal(First(), "%v", 1d);

            //-------------------------------------------------------------------------
            //-----------  extend(), includes inserting, and '+', also chaining
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("#m2 = b, c, d;");
            I("#m = #m1.extend(#m2);");
            _AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "b", "c", "d" ));
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c"));  //no sideeffects
            _AssertListString(First(), "#m2", new StringOrList("b", "c", "d"));  //no sideeffects
            FAIL("#m = #m1.extend('f');");  //this is not allowed and #m1 + 'f' would append 'f' to list elements
            //_AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "f" ));
            I("#m = extend(#m1, #m2);");
            _AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "b", "c", "d" ));
            //I("#m = extend(#m1, 'f');");  //not allowed
            //_AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "f" ));
            I("#m = #m1 + #m2;");
            _AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "b", "c", "d" ));
            I("#m = #m1 + 'f';");  //we have that {#m + 'f'} is same as {#m}f
            //_AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "f" ));  //NO!
            _AssertListString(First(), "#m", new StringOrList("af", "bf", "cf"));
            I("#m = #m1.extend(#m2).extend(#m2);");
            _AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "b", "c", "d", "b", "c", "d" ));
            I("#m = #m1 + #m2 + #m2;");
            _AssertListString(First(), "#m", new StringOrList( "a", "b", "c", "b", "c", "d", "b", "c", "d" ));
            I("#m = #m1.extend(3, #m2);");
            _AssertListString(First(), "#m", new StringOrList("a", "b", "b", "c", "d", "c"));
            
            //-------------------------------------------------------------------------
            //-----------  append(), includes inserting
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("#m2 = b, c, d;");
            I("#m = #m1.append(#m2);");
            StringOrList temp = new StringOrList("a", "b", "c", null);
            temp.ss[4 - 1] = new StringOrList("b", "c", "d");
            _AssertListString(First(), "#m", temp);
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c"));  //no sideeffects
            _AssertListString(First(), "#m2", new StringOrList("b", "c", "d"));  //no sideeffects
            I("#m = #m1.append('f').append('g');");
            _AssertListString(First(), "#m", new StringOrList("a", "b", "c", "f", "g"));
            I("#m = #m1.append(3, #m2);");
            temp = new StringOrList("a", "b", null, "c");
            temp.ss[3 - 1] = new StringOrList("b", "c", "d");
            _AssertListString(First(), "#m", temp);

            //-------------------------------------------------------------------------
            //-----------  union()
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("#m2 = b, c, d, b, d;");  //d will not become dublet
            I("#m = #m1.union(#m2);");
            _AssertListString(First(), "#m", new StringOrList("a", "b", "c", "d"));
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c"));  //no sideeffects
            _AssertListString(First(), "#m2", new StringOrList("b", "c", "d", "b", "d"));  //no sideeffects
            I("#m = #m1 || #m2;");
            _AssertListString(First(), "#m", new StringOrList("a", "b", "c", "d"));

            //-------------------------------------------------------------------------
            //-----------  except()
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("#m2 = b, c, d;");  //d will not become dublet
            I("#m = #m1.except(#m2);");
            _AssertListString(First(), "#m", new StringOrList("a"));
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c"));  //no sideeffects
            _AssertListString(First(), "#m2", new StringOrList("b", "c", "d"));  //no sideeffects
            I("#m = #m1 - #m2;");
            _AssertListString(First(), "#m", new StringOrList("a"));

            //-------------------------------------------------------------------------
            //-----------  count()
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c, b;");            
            I("%v = #m1.count('b');");
            _AssertScalarVal(First(), "%v", 2d);
            I("%v = #m1.count('e');");
            _AssertScalarVal(First(), "%v", 0d);

            //-------------------------------------------------------------------------
            //-----------  remove()
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");            
            I("#m = #m1.remove('b');");
            _AssertListString(First(), "#m", new StringOrList("a", "c"));
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c"));  //no sideeffects

            //-------------------------------------------------------------------------
            //-----------  pop()
            //-------------------------------------------------------------------------
            I("reset;");
            I("#m1 = a, b, c;");
            I("#m = #m1.pop(2);");
            _AssertListString(First(), "#m", new StringOrList("a", "c"));
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c"));  //no sideeffects
            I("#m = #m1.pop();");
            _AssertListString(First(), "#m", new StringOrList("a", "b"));
            I("#m = #m1.pop(3);");
            _AssertListString(First(), "#m", new StringOrList("a", "b"));            
            FAIL("#m = #m1.pop(4);");
            FAIL("#m = #m1.pop(0);");

            //-------------------------------------------------------------------------
            //-----------  strip(), lstrip(), rstrip()
            //-------------------------------------------------------------------------
            I("reset;");
            I("%s1 = '  abc   ';");
            I("%s2 = %s1.strip();");
            _AssertScalarString(First(), "%s2", "abc");
            I("%s2 = %s1.stripstart();");
            _AssertScalarString(First(), "%s2", "abc   ");
            I("%s2 = %s1.stripend();");
            _AssertScalarString(First(), "%s2", "  abc");

            //-------------------------------------------------------------------------
            //-----------  replace()
            //-------------------------------------------------------------------------
            I("reset;");
            I("%s1 = 'abcABC';");
            I("%s2 = %s1.replace('ab', 'X');");
            _AssertScalarString(First(), "%s2", "XcXC");
            I("%s2 = %s1.replace('a', 'X');");
            _AssertScalarString(First(), "%s2", "XbcXBC");
            I("%s2 = %s1.replace('a', 'X', 1);");
            _AssertScalarString(First(), "%s2", "XbcABC");

            I("reset;");
            I("#m1 = a, b, c, b;");
            I("#m = #m1.replace('b', 'x');");
            _AssertListString(First(), "#m", new StringOrList("a", "x", "c", "x"));
            FAIL("#m = #m1.replace('b', 'x', 1);");

            //-------------------------------------------------------------------------
            //-----------  length()
            //-------------------------------------------------------------------------
            I("reset;");
            I("%s1 = 'abcABC';");
            I("%v = %s1.length();");
            _AssertScalarVal(First(), "%v", 6d);
            I("reset;");
            I("#m1 = a, b, c, b;");
            I("%v = #m1.length();");
            _AssertScalarVal(First(), "%v", 4d);

            //-------------------------------------------------------------------------
            //-----------  lower()
            //-------------------------------------------------------------------------
            I("reset;");
            I("%s1 = 'abcABC';");
            I("%s2 = %s1.lower();");
            _AssertScalarString(First(), "%s2", "abcabc");

            I("reset;");
            I("#m1 = AB, b, C, b;");
            I("#m = #m1.lower();");
            _AssertListString(First(), "#m", new StringOrList("ab", "b", "c", "b"));

            //-------------------------------------------------------------------------
            //-----------  upper()
            //-------------------------------------------------------------------------
            I("reset;");
            I("%s1 = 'abcABC';");
            I("%s2 = %s1.upper();");
            _AssertScalarString(First(), "%s2", "ABCABC");

            I("reset;");
            I("#m1 = AB, b, C, b;");
            I("#m = #m1.upper();");
            _AssertListString(First(), "#m", new StringOrList("AB", "B", "C", "B"));

            //-------------------------------------------------------------------------
            //-----------  sort()
            //-------------------------------------------------------------------------
            
            I("reset;");
            I("#m1 = e, AB, c, B;");
            I("#m = #m1.sort();");
            _AssertListString(First(), "#m", new StringOrList("AB", "B", "c", "e"));
            _AssertListString(First(), "#m1", new StringOrList("e", "AB", "c", "b"));  //no sideeffects

            //-------------------------------------------------------------------------
            //-----------  unique()
            //-------------------------------------------------------------------------

            I("reset;");
            I("#m1 = a, b, c, b, b, e;");
            I("#m = #m1.unique();");
            _AssertListString(First(), "#m", new StringOrList("a", "b", "c", "e"));
            _AssertListString(First(), "#m1", new StringOrList("a", "b", "c", "b", "b", "e"));  //no sideeffects

            I("reset;");
            I("#m1 = a, B, c, b, B, e;");
            I("#m = #m1.unique();");
            _AssertListString(First(), "#m", new StringOrList("a", "B", "c", "e"));
            _AssertListString(First(), "#m1", new StringOrList("a", "B", "c", "b", "B", "e"));  //no sideeffects

            //-------------------------------------------------------------------------
            //-----------  startswith()
            //-------------------------------------------------------------------------

            I("reset;");
            I("%s1 = 'abcABC';");
            I("%s2 = %s1.startswith('abca');");
            _AssertScalarVal(First(), "%s2", 1d);

            //-------------------------------------------------------------------------
            //-----------  startswith()
            //-------------------------------------------------------------------------

            I("reset;");
            I("%s1 = 'abcABC';");
            I("%s2 = %s1.startswith('abca');");
            _AssertScalarVal(First(), "%s2", 1d);

            //-------------------------------------------------------------------------
            //-----------  flatten()
            //-------------------------------------------------------------------------

            I("reset;");
            I("#m1 = ('a', 'b', ('c', 'd', 'e'), 'f');");
            I("#m = #m1.flatten();");
            _AssertListString(First(), "#m", new StringOrList("a", "b", "c", "d", "e", "f"));
            I("#m = #m1.flatten();");
            I("%s1 = #m1[3][2];");
            I("%s2 = #m[4];");
            I("%s3 = #m1.flatten()[4];");
            _AssertScalarString(First(), "%s1", "d");
            _AssertScalarString(First(), "%s2", "d");
            _AssertScalarString(First(), "%s3", "d");
            I("#m1[1] = 'z';");
            _AssertListString(First(), "#m1", 1, "z");
            _AssertListString(First(), "#m", 1, "a");  //just to be sure that there are no dependency
        }

        [TestMethod]
        public void _Test_Create()
        {
            I("reset;");
            I("option databank create auto = yes;");  //starts with this default
            I("%v = 1;");
            I("#m = a, b;");
            I("a = 5;");
            FAIL("b[i] = 5;");
            I("b = series(1);");
            I("b[i] = 5;");

            I("reset;");
            I("option databank create auto = no;");  //starts with this default
            I("%v = 1;");
            I("#m = a, b;");
            //--------------
            FAIL("a = 5;");
            FAIL("b[i] = 5;");
            FAIL("b = series(1);");
            FAIL("b[i] = 5;");
            //--------------
            I("xx1 = 5;");
            FAIL("xx2[i] = 5;");
            I("xx2 = series(1);");
            I("xx2[i] = 5;");  //xx-name will accept anything inside also with auto=no
            //--------------
            I("option databank create auto = yes;");
            I("c = series(1);");
            I("c[i] = 7;");
            I("option databank create auto = no;");
            FAIL("c[j] = 7;");  //will not allow new elements
            I("c[i] = 8;"); //updating existing is ok
            I("c = series(2);");  //even this is ok

        }

        [TestMethod]
        public void _Test_Compare()
        {
            I("reset;");
            I("time 2001 2002;");
            I("yy = series(1);");
            I("yy[i] = (1000, 1000);");
            I("xx = series(2);");
            I("xx[a, x] = (100, 100);");
            I("xx[b, x] = (200, 200);");
            I("xx[a, y] = (300, 300);");
            I("xx[b, y] = (400, 400);");
            I("#m1 = a, b;");
            I("#m2 = ('a',);");  //"#m2 = a;" does not work as intended
            I("clone;");
            I("xx[b, y] = (400.4, 402);");
            I("yy[i] = (1000.2, 1004);");
            I("yy[j] = 2000;");

            I("option print width = 1000000;");

            I("compare;");  //should work without params            
            I("%s = getfile('compare_databanks.txt');");
            string ss = (First().GetIVariable("%s") as ScalarString).ConvertToString();
            Assert.IsTrue(ss.Contains("Out of the 5 common series, there are differences regarding 2 of them"));
            //_AssertScalarString

            I("compare<dump>;");
            I("p #dif;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");
            
            I("compare < dump sort = rel >;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare < dump sort = abs >;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "yy[i]");
            _AssertListString(First(), "#dif", 2, "xx[b, y]");
            
            I("compare < dump abs = 1.01 >;");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare < dump abs = 2.01 >;");  //xx[b, y] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);            
            _AssertListString(First(), "#dif", 1, "yy[i]");

            I("compare < dump abs = 4.01 >;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);            

            I("compare < dump rel = 0.0039 >;");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare < dump rel = 0.0041 >;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            if (true)
            {
                //same as above but with pch instead of rel

                I("compare < dump pch = 100*0.0039 >;");  //no effect
                I("p <n m q> {#dif};");
                _AssertListSize(First(), "#dif", 2);
                _AssertListString(First(), "#dif", 1, "xx[b, y]");
                _AssertListString(First(), "#dif", 2, "yy[i]");

                I("compare < dump pch = 100*0.0041 >;");  //yy[i] is filtered out
                I("p <n m q> {#dif};");
                _AssertListSize(First(), "#dif", 1);
                _AssertListString(First(), "#dif", 1, "xx[b, y]");
            }

            I("compare < dump rel = 0.0051 >;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);            

            I("compare < dump abs = 2.01 rel = 0.0041 >;"); //all are filtered out, abs catches xx[b, y] and rel catches yy[i]
            I("p #dif;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            // ---------------------------------------
            // ---------------------------------------
            // -------- truncated time ---------------
            // ---------------------------------------
            // ---------------------------------------

            I("compare<2001 2001>;");  //should work without params            
            I("%s = getfile('compare_databanks.txt');");
            ss = (First().GetIVariable("%s") as ScalarString).ConvertToString();
            Assert.IsTrue(ss.Contains("Out of the 5 common series, there are differences regarding 2 of them"));
            //_AssertScalarString

            I("compare<2001 2001 dump>;");
            I("p #dif;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare <2001 2001 dump sort = rel >;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare <2001 2001 dump sort = abs >;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");
            
            I("compare <2001 2001 dump abs = 0.199 >;");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare <2001 2001 dump abs = 0.201 >;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump abs = 0.401 >;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            I("compare <2001 2001 dump rel = 0.0001 >;");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 2);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            _AssertListString(First(), "#dif", 2, "yy[i]");

            I("compare <2001 2001 dump rel = 0.0003 >;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            if (true)
            {
                //same as above but with pch instead of rel

                I("compare <2001 2001 dump pch = 100*0.0001 >;");  //no effect
                I("p <n m q> {#dif};");
                _AssertListSize(First(), "#dif", 2);
                _AssertListString(First(), "#dif", 1, "xx[b, y]");
                _AssertListString(First(), "#dif", 2, "yy[i]");

                I("compare <2001 2001 dump pch = 100*0.0003 >;");  //yy[i] is filtered out
                I("p <n m q> {#dif};");
                _AssertListSize(First(), "#dif", 1);
                _AssertListString(First(), "#dif", 1, "xx[b, y]");
            }

            I("compare <2001 2001 dump rel = 0.0011 >;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);


            // ---------------------------------------
            // ---------------------------------------
            // -------- restricted on xx -------------
            // ---------------------------------------
            // ---------------------------------------
            
            I("compare<2001 2001>xx;");  //should work without params            
            I("%s = getfile('compare_databanks.txt');");
            ss = (First().GetIVariable("%s") as ScalarString).ConvertToString();
            Assert.IsTrue(ss.Contains("Out of the 4 common series, there are differences regarding 1 of them"));
            //_AssertScalarString

            I("compare<2001 2001 dump>xx;");
            I("p #dif;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");            

            I("compare <2001 2001 dump sort = rel >xx;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");            

            I("compare <2001 2001 dump sort = abs >xx;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");            

            I("compare <2001 2001 dump abs = 0.199 >xx;");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            
            I("compare <2001 2001 dump abs = 0.201 >xx;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump abs = 0.401 >xx;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            I("compare <2001 2001 dump rel = 0.0001 >xx;");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");            

            I("compare <2001 2001 dump rel = 0.0003 >xx;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");
            
            I("compare <2001 2001 dump rel = 0.0011 >xx;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            // ---------------------------------------
            // ---------------------------------------
            // -------- restricted on #r -------------
            // ---------------------------------------
            // ---------------------------------------

            I("#r = ('xx',);");  //should work without params            

            I("compare<2001 2001>{#r};");  //should work without params            
            I("%s = getfile('compare_databanks.txt');");
            ss = (First().GetIVariable("%s") as ScalarString).ConvertToString();
            Assert.IsTrue(ss.Contains("Out of the 4 common series, there are differences regarding 1 of them"));
            //_AssertScalarString

            I("compare<2001 2001 dump>{#r};");
            I("p #dif;");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump sort = rel >{#r};");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump sort = abs >{#r};");
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump abs = 0.199 >{#r};");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump abs = 0.201 >{#r};");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump abs = 0.401 >{#r};");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            I("compare <2001 2001 dump rel = 0.0001 >{#r};");  //no effect
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump rel = 0.0003 >{#r};");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001 2001 dump rel = 0.0011 >{#r};");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            // ---------------------------------------
            // ---------------------------------------
            // -------- quarters ---------------------
            // ---------------------------------------
            // ---------------------------------------

            I("reset;");
            I("option freq q;");
            I("time 2001q1 2001q2;");
            I("yy = series(1);");
            I("yy!q[i] = (1000, 1000);");
            I("xx = series(2);");
            I("xx!q[a, x] = (100, 100);");
            I("xx!q[b, x] = (200, 200);");
            I("xx!q[a, y] = (300, 300);");
            I("xx!q[b, y] = (400, 400);");
            I("#m1 = a, b;");
            I("#m2 = ('a',);");  //"#m2 = a;" does not work as intended
            I("clone;");
            I("xx!q[b, y] = (400.4, 402);");
            I("yy!q[i] = (1000.2, 1004);");
            I("yy!q[j] = 2000;");

            //just some simple tests

            I("compare <2001q1 2001q1 dump abs = 0.201 >;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xx[b, y]");

            I("compare <2001q1 2001q1 dump abs = 0.401 >;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);

            // ---------------------------------------
            // ---------------------------------------
            // -------- non-array quarters -----------
            // ---------------------------------------
            // ---------------------------------------

            I("reset;");
            I("option freq q;");
            I("time 2001q1 2001q2;");            
            I("yyi!q = (1000, 1000);");            
            I("xxax!q = (100, 100);");
            I("xxbx!q = (200, 200);");
            I("xxay!q = (300, 300);");
            I("xxby!q = (400, 400);");
            I("#m1 = a, b;");
            I("#m2 = ('a',);");  //"#m2 = a;" does not work as intended
            I("clone;");
            I("xxby!q = (400.4, 402);");
            I("yyi!q = (1000.2, 1004);");
            I("yyj!q = 2000;");

            //just some simple tests

            I("compare <2001q1 2001q1 dump abs = 0.201 >;");  //yy[i] is filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 1);
            _AssertListString(First(), "#dif", 1, "xxby");

            I("compare <2001q1 2001q1 dump abs = 0.401 >;");  //all are filtered out
            I("p <n m q> {#dif};");
            _AssertListSize(First(), "#dif", 0);


        }

        [TestMethod]
        public void _Test_OpenClose()
        {
            Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");

            I("RESET; TIME 2010 2012;");
            I("OPEN <edit> b1;");
            I("xx = 5;");
            I("CLOSE b1;");
            I("OPEN <edit> b2;");
            I("xx = 6;");
            I("CLOSE b2;");
            //------
            //------
            //------
            FAIL("OPEN a:b1;");
            FAIL("OPEN a:b1!q;");
            FAIL("OPEN b1!q;");
            //------
            I("OPEN b1; OPEN b2;");            
            I("CLOSE b1; CLOSE b2;");
            //------
            I("OPEN b1, b2;");
            I("CLOSE b1, b2;");
            //------
            I("OPEN b1, b2 as c1, c2;");
            I("CLOSE c1, c2;");
            //------
            FAIL("OPEN b1, b2 as c1, c2, c3;");            
            //------
            I("OPEN " + Globals.ttPath2 + @"\regres\Databanks\temp\b1, " + Globals.ttPath2 + @"\regres\Databanks\temp\b2 as c1, c2;");
            I("CLOSE c1, c2;");
            //------
            I("OPEN '" + Globals.ttPath2 + @"\regres\Databanks\temp\b1', '" + Globals.ttPath2 + @"\regres\Databanks\temp\b2' as c1, c2;");
            I("CLOSE c1, c2;");
            //------
            I("%s1 = 'b1'; %s2 = 'b2';");
            I("OPEN %s1, %s2 as c1, c2;");
            I("CLOSE c1, c2;");
            //------            
            I("OPEN {%s1}, {%s2} as c1, c2;");  //is ok, since {...} is caught as filename
            I("CLOSE c1, c2;");
            //------
            I("%s3 = 'Databanks';");
            I("%s4 = 'c2';");
            I("OPEN '" + Globals.ttPath2 + @"\regres\{%s3}\temp\b2' as c2;");
            I("CLOSE {%s4};");
            //------
            I("#m1 = b1, b2;");
            I("#m2 = c1, c2;");
            I("OPEN {#m1} as {#m2};");
            I("CLOSE {#m2};");
            //------
            I("#m1 = ('b1',);");
            I("#m1a = (#m1, 'b2');");            
            I("OPEN {#m1a};");
            I("CLOSE {#m1a};");
            //------
            I("#m1 = ('" + Globals.ttPath2 + @"\regres\{%s3}\temp\b1', '" + Globals.ttPath2 + @"\regres\{%s3}\temp\b2');");
            I("#m2 = c1, c2;");
            I("OPEN {#m1} as {#m2};");
            I("CLOSE {#m2};");

        }

        [TestMethod]
        public void _Test_Lag()
        {
            I("RESET; TIME 2001 2005;");
            I("xx1 = (1, 2, 3, 4, 5);");
            _AssertSeries(First(), "xx1", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx1", 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx1", 2004, 4d, sharedDelta);
            _AssertSeries(First(), "xx1", 2005, 5d, sharedDelta);
            _AssertSeries(First(), "xx1", 2006, double.NaN, sharedDelta);
            I("xx2 = xx1[-1];");
            _AssertSeries(First(), "xx2", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx2", 2003, 2d, sharedDelta);
            _AssertSeries(First(), "xx2", 2004, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", 2005, 4d, sharedDelta);
            _AssertSeries(First(), "xx2", 2006, double.NaN, sharedDelta);
            I("xx3 = (xx1 + xx1)[-1];");
            _AssertSeries(First(), "xx3", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx3", 2003, 4d, sharedDelta);
            _AssertSeries(First(), "xx3", 2004, 6d, sharedDelta);
            _AssertSeries(First(), "xx3", 2005, 8d, sharedDelta);
            _AssertSeries(First(), "xx3", 2006, double.NaN, sharedDelta);
            I("xx4 = pch(xx1 + xx1)[-1];");
            _AssertSeries(First(), "xx4", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2003, (2d / 1d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "xx4", 2004, (3d / 2d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "xx4", 2005, (4d / 3d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "xx4", 2006, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2007, double.NaN, sharedDelta);
            I("xx5 = movsum(movsum(xx1, 2), 3);");   //m 3 5 7 9 -> m, m, m, 15 21
            _AssertSeries(First(), "xx5", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2004, 15d, sharedDelta);
            _AssertSeries(First(), "xx5", 2005, 21d, sharedDelta);
            _AssertSeries(First(), "xx5", 2006, double.NaN, sharedDelta);
            I("xx6 = movsum(movsum(xx1, 3), 2);");  //same as before
            _AssertSeries(First(), "xx6", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx6", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx6", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx6", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx6", 2004, 15d, sharedDelta);
            _AssertSeries(First(), "xx6", 2005, 21d, sharedDelta);
            _AssertSeries(First(), "xx6", 2006, double.NaN, sharedDelta);
            I("xx7 = movsum(movsum(xx1, 4), 2);");   //m m m 10 15 -> m, m, m, m, 25
            _AssertSeries(First(), "xx7", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx7", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx7", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx7", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx7", 2004, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx7", 2005, 24d, sharedDelta);
            _AssertSeries(First(), "xx7", 2006, double.NaN, sharedDelta);

            // ------------------------------------------------------------------

            I("RESET; TIME 2000 2005;");
            I("xx1 = (-1, 1, 2, 3, 4, 5);");
            _AssertSeries(First(), "xx1", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2000, -1d, sharedDelta);
            _AssertSeries(First(), "xx1", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx1", 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx1", 2004, 4d, sharedDelta);
            _AssertSeries(First(), "xx1", 2005, 5d, sharedDelta);
            _AssertSeries(First(), "xx1", 2006, double.NaN, sharedDelta);
            I("TIME 2001 2005;");
            I("xx2 = xx1[-1];");
            _AssertSeries(First(), "xx2", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2001, -1d, sharedDelta);
            _AssertSeries(First(), "xx2", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx2", 2003, 2d, sharedDelta);
            _AssertSeries(First(), "xx2", 2004, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", 2005, 4d, sharedDelta);
            _AssertSeries(First(), "xx2", 2006, double.NaN, sharedDelta);
            I("xx3 = (xx1 + xx1)[-1];");
            _AssertSeries(First(), "xx3", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2001, -2d, sharedDelta);
            _AssertSeries(First(), "xx3", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx3", 2003, 4d, sharedDelta);
            _AssertSeries(First(), "xx3", 2004, 6d, sharedDelta);
            _AssertSeries(First(), "xx3", 2005, 8d, sharedDelta);
            _AssertSeries(First(), "xx3", 2006, double.NaN, sharedDelta);
            I("xx4 = (1 + xx1 + xx1 + 1)[-1];");
            _AssertSeries(First(), "xx4", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2001, 0d, sharedDelta);
            _AssertSeries(First(), "xx4", 2002, 4d, sharedDelta);
            _AssertSeries(First(), "xx4", 2003, 6d, sharedDelta);
            _AssertSeries(First(), "xx4", 2004, 8d, sharedDelta);
            _AssertSeries(First(), "xx4", 2005, 10d, sharedDelta);
            _AssertSeries(First(), "xx4", 2006, double.NaN, sharedDelta);

        }

        [TestMethod]
        public void _Test_ArraySeries1()
        {                      
            
            // ------------------------------------

            I("RESET; TIME 2001 2003;");
            I("xx2 = series(2);");
            I("xx2['a', 'b'] = (1, 2, 3);");
            I("xx2['a', 'c'] = (11, 12, 13);");
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2004, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "c" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "c" }, 2001, 11d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "c" }, 2002, 12d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "c" }, 2003, 13d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "c" }, 2004, double.NaN, sharedDelta);

            I("xx3 = xx2;");
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2004, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2001, 11d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2002, 12d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2003, 13d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2004, double.NaN, sharedDelta);

            I("xx2['a', 'b'][2002] = 100;");
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2002, 100d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2002, 2d, sharedDelta);

            I("xx3['a', 'c'] = xx2['a', 'b'];");
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "b" }, 2004, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2002, 100d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2004, double.NaN, sharedDelta);

            I("xx2['a', 'b'][2002] = 200;");
            _AssertSeries(First(), "xx2", new string[] { "a", "b" }, 2002, 200d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "c" }, 2002, 100d, sharedDelta);

            I("RESET; TIME 2001 2001;");
            I("v = series(1);");
            I("#s = ('a', 'b');");
            I("v[#s] = 1;");
            _AssertSeries(First(), "v", new string[] { "a" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "v", new string[] { "a" }, 2001, 1d, sharedDelta);
            _AssertSeries(First(), "v", new string[] { "a" }, 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "v", new string[] { "b" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "v", new string[] { "b" }, 2001, 1d, sharedDelta);
            _AssertSeries(First(), "v", new string[] { "b" }, 2002, double.NaN, sharedDelta);

            I("RESET; TIME 2001 2001;");
            I("series xx3 = series(3);");
            I("series xx2 = series(2);");
            I("#i = a, b;");
            I("#i0 = ('b',);");
            I("#j = x, y;");
            I("#k = m, n;");
            I("xx3[#i, #j, #k] = 5 + 0;");
            I("xx3[#i, #j, #k] $ (#i0[#i]) = 6 + 0;");
            I("xx2[#i, #j] = sum(#k, xx3[#i, #j, #k]) + 0;");
            _AssertSeries(First(), "xx3", new string[] { "a", "x", "m" }, 2001, 5d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "x", "n" }, 2001, 5d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "y", "m" }, 2001, 5d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "a", "y", "n" }, 2001, 5d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "b", "x", "m" }, 2001, 6d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "b", "x", "n" }, 2001, 6d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "b", "y", "m" }, 2001, 6d, sharedDelta);
            _AssertSeries(First(), "xx3", new string[] { "b", "y", "n" }, 2001, 6d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "x" }, 2001, 10d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "a", "y" }, 2001, 10d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "b", "x" }, 2001, 12d, sharedDelta);
            _AssertSeries(First(), "xx2", new string[] { "b", "y" }, 2001, 12d, sharedDelta);



        }
        

        [TestMethod]
        public void _Test_SeriesLags()
        {

            I("reset; time 2000 2007;");
            I("x1 = 1;");            
            I("y1 = x1[-2];");
            I("z1 = (x1+x1)[-2];");
            I("w1 = pch(x1+x1)[-2];");
            I("k1 = dif(pch(x1+x1))[-2];");

            I("<2001 2006>x2 = 2;");
            I("<2002 2005>y2 = x2[-2];");
            I("<2002 2005>z2 = (x2+x2)[-2];");
            I("<2002 2005>w2 = pch(x2+x2)[-2];");
            I("<2002 2005>k2 = dif(pch(x2+x2))[-2];");

            _AssertSeries(First(), "x1", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1", 2000, 2007, 1d, sharedDelta);            
            _AssertSeries(First(), "x1", 2008, double.NaN, sharedDelta);

            _AssertSeries(First(), "y1", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "y1", 2002, 2007, 1d, sharedDelta);
            _AssertSeries(First(), "y1", 2008, double.NaN, sharedDelta);

            _AssertSeries(First(), "z1", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "z1", 2002, 2007, 2d, sharedDelta);
            _AssertSeries(First(), "z1", 2008, double.NaN, sharedDelta);

            _AssertSeries(First(), "w1", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "w1", 2003, 2007, 0d, sharedDelta);
            _AssertSeries(First(), "w1", 2008, double.NaN, sharedDelta);

            _AssertSeries(First(), "k1", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "k1", 2004, 2007, 0d, sharedDelta);
            _AssertSeries(First(), "k1", 2008, double.NaN, sharedDelta);

            //---

            _AssertSeries(First(), "x2", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2", 2001, 2006, 2d, sharedDelta);
            _AssertSeries(First(), "x2", 2007, double.NaN, sharedDelta);

            _AssertSeries(First(), "y2", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "y2", 2003, 2005, 2d, sharedDelta);
            _AssertSeries(First(), "y2", 2006, double.NaN, sharedDelta);

            _AssertSeries(First(), "z2", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "z2", 2003, 2005, 4d, sharedDelta);
            _AssertSeries(First(), "z2", 2006, double.NaN, sharedDelta);

            _AssertSeries(First(), "w2", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "w2", 2004, 2005, 0d, sharedDelta);
            _AssertSeries(First(), "w2", 2006, double.NaN, sharedDelta);

            _AssertSeries(First(), "k2", 2004, double.NaN, sharedDelta);
            _AssertSeries(First(), "k2", 2005, 2005, 0d, sharedDelta);
            _AssertSeries(First(), "k2", 2006, double.NaN, sharedDelta);

        }

        [TestMethod]
        public void _Test_SeriesIndexing()
        {

            I("reset; time 2000 2002;");
            I("xx = 100;");
            I("xx[2001] = 1;");
            I("xx[2002a1] = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 100d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);

            I("reset; time 2001 2003;");
            I("xx = 100;");
            I("xx[2000+1] = 1;");
            I("xx[2001a1+1] = 2;");
            I("p xx;");
            _AssertSeries(First(), "xx", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, 100d, sharedDelta);
            _AssertSeries(First(), "xx", 2004, double.NaN, sharedDelta);
        }
        

        [TestMethod]
        public void _Test_Timeless()
        {
            //tests timeless toghether with gdx

            I("RESET; TIME 2001 2003;");
            I("x1 = timeless(100);");
            I("x2 = 200;");
            I("x3 = series(1);");
            I("x4 = series(1);");
            I("x3['a'] = timeless(300);");
            I("x4['b'] = 400;");
            I("x5 = 0;");
            I("x5 = timeless(500);");  //note: LHS if found as a normal series, so no creation

            HelperTimeless();  //asserts
            
            I("WRITE <gdx> temp;");
            I("RESET;");
            I("READ <gdx> temp;");

            HelperTimeless();  //asserts

        }

        private static void HelperTimeless()
        {
            _AssertSeries(First(), "x1!a", 2000, 100d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2001, 100d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2002, 100d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2003, 100d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2004, 100d, sharedDelta);
            Assert.IsTrue((First().GetIVariable("x1!a") as Series).type == ESeriesType.Timeless);

            _AssertSeries(First(), "x2!a", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!a", 2001, 200d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2002, 200d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2003, 200d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2004, double.NaN, sharedDelta);
            Assert.IsTrue((First().GetIVariable("x2!a") as Series).type == ESeriesType.Normal);

            _AssertSeries(First(), "x3!a", new string[] { "a" }, 2000, 300d, sharedDelta);
            _AssertSeries(First(), "x3!a", new string[] { "a" }, 2001, 300d, sharedDelta);
            _AssertSeries(First(), "x3!a", new string[] { "a" }, 2002, 300d, sharedDelta);
            _AssertSeries(First(), "x3!a", new string[] { "a" }, 2003, 300d, sharedDelta);
            _AssertSeries(First(), "x3!a", new string[] { "a" }, 2004, 300d, sharedDelta);            
            IVariable temp3 = null; (First().GetIVariable("x3!a") as Series).dimensionsStorage.TryGetValue(new MapMultidimItem(new string[] { "a" }), out temp3);
            Assert.IsTrue((temp3 as Series).type == ESeriesType.Timeless);

            _AssertSeries(First(), "x4!a", new string[] { "b" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4!a", new string[] { "b" }, 2001, 400d, sharedDelta);
            _AssertSeries(First(), "x4!a", new string[] { "b" }, 2002, 400d, sharedDelta);
            _AssertSeries(First(), "x4!a", new string[] { "b" }, 2003, 400d, sharedDelta);
            _AssertSeries(First(), "x4!a", new string[] { "b" }, 2004, double.NaN, sharedDelta);            
            IVariable temp4 = null; (First().GetIVariable("x4!a") as Series).dimensionsStorage.TryGetValue(new MapMultidimItem(new string[] { "b" }), out temp4);
            Assert.IsTrue((temp4 as Series).type == ESeriesType.Normal);

            _AssertSeries(First(), "x5!a", 2000, double.NaN, sharedDelta);  //Because it was found a a normal series to begin with
            _AssertSeries(First(), "x5!a", 2001, 500d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2002, 500d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2003, 500d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2004, double.NaN, sharedDelta);  //Because it was found a a normal series to begin with
            Assert.IsTrue((First().GetIVariable("x5!a") as Series).type == ESeriesType.Normal);
        }

        [TestMethod]
        public void _Test_DatabanksInOut()
        {
            //full write/read

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            DatabanksTestHelper8();
            I("WRITE sletmig;");
            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("READ sletmig;");
            Assert.AreEqual(First().storage.Count, 4); DatabanksTestHelper5(); DatabanksTestHelper6(); DatabanksTestHelper7();

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            DatabanksTestHelper8();
            I("WRITE <gdx> sletmig;");
            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("READ <gdx> sletmig;");
            Assert.AreEqual(First().storage.Count, 2); DatabanksTestHelper5(); DatabanksTestHelper6();

            //write/read of 1 var

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            DatabanksTestHelper8();
            I("WRITE xx1 file=sletmig;");
            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("READ sletmig;");
            Assert.AreEqual(First().storage.Count, 1); DatabanksTestHelper6();

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            DatabanksTestHelper8();
            I("WRITE <gdx> xx1 file=sletmig;");
            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("READ <gdx> sletmig;");
            Assert.AreEqual(First().storage.Count, 1); DatabanksTestHelper6();

            //write/read of 2 vars

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            DatabanksTestHelper8();
            I("WRITE xx1, xx2 file=sletmig;");
            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("READ sletmig;");
            Assert.AreEqual(First().storage.Count, 2); DatabanksTestHelper5(); DatabanksTestHelper6();

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            DatabanksTestHelper8();
            I("WRITE <gdx> xx1, xx2 file=sletmig;");
            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("READ <gdx> sletmig;");
            Assert.AreEqual(First().storage.Count, 2); DatabanksTestHelper5(); DatabanksTestHelper6();

        }

        private static void DatabanksTestHelper8()
        {
            I("xx1 = series(1);");
            I("xx1[a] = (101, 102, 103);");
            I("xx1[b] = (201, 202, 203);");
            I("xx2 = (100, 101, 102);");
            I("%v = 100;");
            I("#m = [100, 101];");
        }

        private static void DatabanksTestHelper5()
        {
            _AssertSeries(First(), "xx2", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2000, 100d, sharedDelta);
            _AssertSeries(First(), "xx2", 2001, 101d, sharedDelta);
            _AssertSeries(First(), "xx2", 2002, 102d, sharedDelta);
            _AssertSeries(First(), "xx2", 2003, double.NaN, sharedDelta);

        }

        private static void DatabanksTestHelper7()
        {
            _AssertScalarVal(First(), "%v", 100d);
            _AssertMatrix(First(), "#m", 1, 1, 100d, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 2, 101d, sharedDelta);
        }

        private static void DatabanksTestHelper6()
        {
            _AssertSeries(First(), "xx1", new string[] { "a" }, 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "a" }, 2000, 101d, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "a" }, 2001, 102d, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "a" }, 2002, 103d, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "a" }, 2003, double.NaN, sharedDelta);

            _AssertSeries(First(), "xx1", new string[] { "b" }, 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "b" }, 2000, 201d, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "b" }, 2001, 202d, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "b" }, 2002, 203d, sharedDelta);
            _AssertSeries(First(), "xx1", new string[] { "b" }, 2003, double.NaN, sharedDelta);
        }

        [TestMethod]
        public void _Test_DollarAndSomeIf()
        {
            I("reset; time 2000 2002;");

            I("b = (102, 103, 102);");
            I("#i = a, b;");

            //$ on lhs ------------------------------------------------------------

            I("xx = 1; xx $ (5 == 5) = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 1; xx $ (5 == 6) = 3;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            //I("xx = 1; xx $ (#i['a']) = 2;");
            I("xx = 1; xx $ ('a' in #i) = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 1; xx $ (#i['c']) = 3;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 1; xx[2001] $ (5 == 5) = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 1; xx[2010] $ (5 == 6) = 3;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);
            
            //series condition ---

            I("xx = 1; xx $ (b == 102) = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 1; xx $ (b == 103) = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("b3 = (0, 3, 1);");
            I("xx = 1; xx $ (b3) = 2;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);  
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);
            
            //in map ---

            I("map #m = (xx = 1); #m.xx $ (b == 102) = 2;");
            Map m = Program.databanks.GetFirst().GetIVariable("#m") as Map;            
            _AssertSeries(m, "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(m, "xx", 2000, 2d, sharedDelta);
            _AssertSeries(m, "xx", 2001, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(m, "xx", 2002, 2d, sharedDelta);
            _AssertSeries(m, "xx", 2003, double.NaN, sharedDelta);

            I("map #m = (xx = 1); #m.xx $ (b == 103) = 2;");
            m = Program.databanks.GetFirst().GetIVariable("#m") as Map;
            _AssertSeries(m, "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(m, "xx", 2000, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(m, "xx", 2001, 2d, sharedDelta);
            _AssertSeries(m, "xx", 2002, 0d, sharedDelta);  //maybe ought to be skipped --> = 1
            _AssertSeries(m, "xx", 2003, double.NaN, sharedDelta);
            
            //if ------------------------------------------------------------

            I("if (1 == 1) xx = 0; xx = 2; else xx = 0; xx = 1; end;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("if (1 == 0); xx = 0; xx = 2; else xx = 0; xx = 1; end;");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("if (b == 102); xx = 0; xx = 2; else xx = 0; xx = 1; end;");  //will be false, since 2001 is false
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("b2 = (102, 102, 102);"); 
            I("if (b2 == 102); xx = 0; xx = 2; else xx = 0; xx = 1; end;");  //will be true
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            //$ on rhs ------------------------------------------------------------
            //$ on rhs ------------------------------------------------------------
            //$ on rhs ------------------------------------------------------------

            I("%v = 1 $ (0 == 1);");
            _AssertScalarVal(First(), "%v", 0d);

            I("xx1 = (1, 2, 3);");
            I("xx = 100 $ (xx1 == 2);");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 100d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);
            
            I("xx1 = (1, 2, 3);");
            I("xx2 = (101, 102, 103);");
            I("xx = xx2 $ (xx1 == 2);");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 102d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 100 $ (#i['a']);");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 100d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 100d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 100d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

            I("xx = 100 $ (#i['c']);");
            _AssertSeries(First(), "xx", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx", 2000, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2001, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2002, 0d, sharedDelta);
            _AssertSeries(First(), "xx", 2003, double.NaN, sharedDelta);

        }

        [TestMethod]
        public void _Test_MissingValuesLogic()
        {
            //Testing OPTION series array print missing = ..., and 
            //also p <missing = ...>

            Table table = null;

            // =============== missing in First
            // for each of these, 4 options are tried combined with <n>, <rn>, <m>

            I("RESET; TIME 2001 2001;");
            I("x = series(1);");
            I("x[a1] = 11;");
            //I("x[a2] = 12;");
            I("x[a3] = 13;");
            I("@x = series(1);");
            I("@x[a1] = 1.1;");
            I("@x[a2] = 2.2;");
            I("@x[a3] = 3.3;");
            I("#i = a1, a2, a3;");

            I("OPTION series array print missing = error;");
            FAIL("p<m> x[#i];");

            I("OPTION series array print missing = m;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p<n> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 2.2d, sharedDelta);            
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);

            I("OPTION series array print missing = zero;");                        
            I("p<n> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 0d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 0d - 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);

            I("OPTION series array print missing = skip;");                       
            I("p<n> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 13d, sharedDelta);
            I("p<rn> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 13d - 3.3d, sharedDelta);

            // <missing = ...>
            // <missing = ...>
            // <missing = ...>
            // <missing = ...>
            // <missing = ...>

            I("OPTION series array print missing = error;");
                                  
            I("p<n missing = m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn missing = m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m missing = m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);
                        
            I("p<n missing = zero> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 0d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn missing = zero> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m missing = zero> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 0d - 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);
                        
            I("p<n missing = skip> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 13d, sharedDelta);
            I("p<rn missing = skip> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 2.2d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m missing = skip> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 13d - 3.3d, sharedDelta);

            // =============== missing in Ref
            // for each of these, 4 options are tried combined with <n>, <rn>, <m>

            I("RESET; TIME 2001 2001;");
            I("x = series(1);");
            I("x[a1] = 11;");
            I("x[a2] = 12;");
            I("x[a3] = 13;");
            I("@x = series(1);");
            I("@x[a1] = 1.1;");
            //I("@x[a2] = 2.2;");
            I("@x[a3] = 3.3;");
            I("#i = a1, a2, a3;");

            I("OPTION series array print missing = error;");
            FAIL("p<m> x[#i];");

            I("OPTION series array print missing = m;");                                 
            I("p<n> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);

            I("OPTION series array print missing = zero;");
            I("p<n> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 0d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d - 0d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);

            I("OPTION series array print missing = skip;");
            I("p<n> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 3.3d, sharedDelta);
            I("p<m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 13d - 3.3d, sharedDelta);

            // <missing = ...>
            // <missing = ...>
            // <missing = ...>
            // <missing = ...>
            // <missing = ...>

            //I("OPTION series array print missing = m;");
            I("p<n missing = m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn missing = m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m missing = m> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.IsTrue(table.Get(2, 3).numberShouldShowAsN);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);

            //I("OPTION series array print missing = zero;");
            I("p<n missing = zero> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn missing = zero> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 0d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 3.3d, sharedDelta);
            I("p<m missing = zero> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d - 0d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d - 3.3d, sharedDelta);

            //I("OPTION series array print missing = skip;");
            I("p<n missing = skip> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 12d, sharedDelta);
            Assert.AreEqual(table.Get(2, 4).number, 13d, sharedDelta);
            I("p<rn missing = skip> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 3.3d, sharedDelta);
            I("p<m missing = skip> x[#i];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[a3]");
            Assert.AreEqual(table.Get(2, 2).number, 11d - 1.1d, sharedDelta);
            Assert.AreEqual(table.Get(2, 3).number, 13d - 3.3d, sharedDelta);
            

        }



        [TestMethod]
        public void _Test_SumUnfoldDollarPrint()
        {

            Table table = null;


            
            // setting up the tests
            // setting up the tests ----------------------------------------------
            // setting up the tests

            I("reset;");
            I("time 2001 2003;");
            I("xx = series(2);");
            I("xx[a, x] = (1,2,3);");
            I("xx[b, x] = (4,5,6);");
            I("xx[a, y] = (7,8,9);");
            I("xx[b, y] = (14,15,16);");
            I("yy = series(2);");
            I("yy[a, x] = (11,12,13);");
            I("yy[b, x] = (14,15,16);");
            I("yy[a, y] = (17,18,19);");
            I("yy[b, y] = (24,25,26);");
            I("zz = (3, 2, 1);");
            I("%v = 12;");

            I("#m1 = ('a', 'b');");
            I("#m2 = ('x', 'y');");
            I("#m3 = ('a', );");
            I("#m4 = ('b', );");

            I("xxax = (1,2,3);");
            I("xxbx = (4,5,6);");
            I("xxay = (7,8,9);");
            I("xxby = (14,15,16);");

            I("CLONE;");


            /*                     
                                  xx['a', 'x'] 
                          2001          1.0000 
                          2002          2.0000 
                          2003          3.0000
             */
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[a, x];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);

            /*
                                 xx3['a', 'x']   xx3['b', 'x'] 
                          2010          1.0000          4.0000 
                          2011          2.0000          5.0000 
                          2012          3.0000          6.0000 

            */

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[#m1, x];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 3).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 6.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx{#m1}x;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xxax");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xxbx");
            Assert.AreEqual(table.Get(2, 3).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 6.0000d, 0.0001);

            /*             
                                  xx['a', 'x']    xx['a', 'y'] 
                          2010          1.0000          7.0000 
                          2011          2.0000          8.0000 
                          2012          3.0000          9.0000              
             */

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[a, #m2];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 9.0000d, 0.0001);


            /*
                                 xx['a', 'x']    xx['a', 'y']    xx['b', 'x']    xx['b', 'y'] 
                        2010          1.0000          7.0000          4.0000         14.0000 
                        2011          2.0000          8.0000          5.0000         15.0000 
                        2012          3.0000          9.0000          6.0000         16.0000 
            */
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[#m1, #m2];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 9.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "xx[b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 14.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 16.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx;");  //printing without indices
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 9.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "xx[b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 14.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 16.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx, yy, zz, %v;");  //printing without indices
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 9.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "xx[b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 14.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 16.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "yy[a, x]");
            Assert.AreEqual(table.Get(2, 6).number, 11.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 6).number, 12.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 6).number, 13.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "yy[a, y]");
            Assert.AreEqual(table.Get(2, 7).number, 17.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 7).number, 18.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 7).number, 19.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 8).CellText.TextData[0], "yy[b, x]");
            Assert.AreEqual(table.Get(2, 8).number, 14.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 8).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 8).number, 16.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 9).CellText.TextData[0], "yy[b, y]");
            Assert.AreEqual(table.Get(2, 9).number, 24.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 9).number, 25.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 9).number, 26.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 10).CellText.TextData[0], "zz");
            Assert.AreEqual(table.Get(2, 10).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 10).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 10).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 11).CellText.TextData[0], "%v");
            Assert.AreEqual(table.Get(2, 11).number, 12.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 11).number, 12.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 11).number, 12.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <m> xx, yy, zz, %v;");  //printing without indices, multiplier
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "xx[b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "yy[a, x]");
            Assert.AreEqual(table.Get(2, 6).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 6).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 6).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "yy[a, y]");
            Assert.AreEqual(table.Get(2, 7).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 7).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 7).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 8).CellText.TextData[0], "yy[b, x]");
            Assert.AreEqual(table.Get(2, 8).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 8).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 8).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 9).CellText.TextData[0], "yy[b, y]");
            Assert.AreEqual(table.Get(2, 9).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 9).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 9).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 10).CellText.TextData[0], "zz");
            Assert.AreEqual(table.Get(2, 10).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 10).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 10).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 11).CellText.TextData[0], "%v");
            Assert.AreEqual(table.Get(2, 11).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 11).number, 0.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 11).number, 0.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> yy-xx;");  //printing without indices
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "yy-xx [a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "yy-xx [a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "yy-xx [b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "yy-xx [b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 10.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx;");  //will fold out its elements
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 9.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "xx[b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 14.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 16.0000d, 0.0001);

            // -------------> from here we use single quotes

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> sum(#m1, xx[#m1, 'x']);");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2003"); //why is it not a date?
            //string tmp0 = table.Get(1, 2).CellText.TextData[0];
            //string tmp1 = table.Get(2, 2).CellText.TextData[0];
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "sum(#m1");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], ", xx[#m1, x])");
            Assert.AreEqual(table.Get(3, 2).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(5, 2).number, 9.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> sum(#m2, xx['a', #m2]);");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "sum(#m2");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], ", xx[a, #m2])");
            Assert.AreEqual(table.Get(3, 2).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(5, 2).number, 12.0000d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> sum((#m1, #m2), xx[#m1, #m2]);");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "sum");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "((#m1, #m2), ");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], "xx[#m1, #m2])");
            Assert.AreEqual(table.Get(4, 2).number, 26.0000d, 0.0001);
            Assert.AreEqual(table.Get(5, 2).number, 30.0000d, 0.0001);
            Assert.AreEqual(table.Get(6, 2).number, 34.0000d, 0.0001);

            //No parenthesis in $-condition
            //Globals.lastPrtOrMulprtTable = null;
            //I("p <n> sum((#m1, #m2), xx[#m1, #m2] $ (#m3[#m1]));");
            I("p <n> sum((#m1, #m2), xx[#m1, #m2] $ (#m1 in #m3));");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(7, 1).CellText.TextData[0], "2003"); //why is it not a date?                     
            //Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "s");
            //Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "um((#m1, #m2)");
            //Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], ", xx[#m1, #m2");
            //Assert.AreEqual(table.Get(4, 2).CellText.TextData[0], "] $ #m3[#m1])");

            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "sum((");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "#m1, #m2), xx");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], "[#m1, #m2] $ ");
            Assert.AreEqual(table.Get(4, 2).CellText.TextData[0], "(#m1 in #m3))");

            Assert.AreEqual(table.Get(5, 2).number, 8.0000d, 0.0001);
            Assert.AreEqual(table.Get(6, 2).number, 10.0000d, 0.0001);
            Assert.AreEqual(table.Get(7, 2).number, 12.0000d, 0.0001);
            
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> sum((#m1, #m2), xx[#m1, #m2] $ (#m1 in #m4));");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(7, 1).CellText.TextData[0], "2003"); //why is it not a date?            
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "sum((");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "#m1, #m2), xx");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], "[#m1, #m2] $ ");
            Assert.AreEqual(table.Get(4, 2).CellText.TextData[0], "(#m1 in #m4))");
            Assert.AreEqual(table.Get(5, 2).number, 18.0000d, 0.0001);
            Assert.AreEqual(table.Get(6, 2).number, 20.0000d, 0.0001);
            Assert.AreEqual(table.Get(7, 2).number, 22.0000d, 0.0001);

            // -----------------------------------------------------
            //   test of ignoremissing, 0 for sum() and skip for unfold()
            // -----------------------------------------------------

            I("reset;");
            I("time 2001 2003;");
            I("xx = series(2);");
            I("xx['a', 'x'] = (1,2,3);");
            I("xx['b', 'x'] = (4,5,6);");
            //I("xx['a', 'y'] = (7,8,9);");
            I("xx['b', 'y'] = (14,15,16);");
            I("#m1 = ('a', 'b');");
            I("#m2 = ('x', 'y');");
            FAIL("p <n> sum((#m1, #m2), xx[#m1, #m2]);");
            
            //I("option series array ignoremissing yes;");                        
            I("option series array calc missing = zero;");

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> sum((#m1, #m2), xx[#m1, #m2]);");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "sum");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "((#m1, #m2), ");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], "xx[#m1, #m2])");
            Assert.AreEqual(table.Get(4, 2).number, 19.0000d, 0.0001);
            Assert.AreEqual(table.Get(5, 2).number, 22.0000d, 0.0001);
            Assert.AreEqual(table.Get(6, 2).number, 25.0000d, 0.0001);

            //I("option series array ignoremissing no;");
            I("option series array print missing = error;");
            FAIL("p <n> xx[#m1, #m2];");

            //I("option series array ignoremissing yes;");
            I("option series array print missing = skip;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[#m1, #m2];");
            table = Globals.lastPrtOrMulprtTable;

            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[b, x]");
            Assert.AreEqual(table.Get(2, 3).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, y]");
            Assert.AreEqual(table.Get(2, 4).number, 14.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 16.0000d, 0.0001);

            //difference here is that 1 is added --> so the element xx[a, y]+1 has value 1

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[#m1, #m2]+1;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a, x]+1");
            Assert.AreEqual(table.Get(2, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a, y]+1");
            Assert.AreEqual(table.Get(2, 3).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[b, x]+1");
            Assert.AreEqual(table.Get(2, 4).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "xx[b, y]+1");
            Assert.AreEqual(table.Get(2, 5).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 16.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 17.0000d, 0.0001);

            //testing 1+xx[a, y] just for safety

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> 1+xx[#m1, #m2];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "1+xx[a, x]");
            Assert.AreEqual(table.Get(2, 2).number, 2.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 2).number, 3.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 2).number, 4.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "1+xx[a, y]");
            Assert.AreEqual(table.Get(2, 3).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 3).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 3).number, 1.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "1+xx[b, x]");
            Assert.AreEqual(table.Get(2, 4).number, 5.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 4).number, 6.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 4).number, 7.0000d, 0.0001);
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "1+xx[b, y]");
            Assert.AreEqual(table.Get(2, 5).number, 15.0000d, 0.0001);
            Assert.AreEqual(table.Get(3, 5).number, 16.0000d, 0.0001);
            Assert.AreEqual(table.Get(4, 5).number, 17.0000d, 0.0001);

            // ------------------------ test of label substitution for arrays ----------------

            I("reset;");
            I("time 2001 2003;");
            I("x1 = series(1);");
            I("x2 = series(1);");
            I("x3 = series(3);");
            I("#a = a1, a2;");
            I("#b = b1, b2;");
            I("#c = c1, c2;");
            I("x1[a1] = 1;");
            I("x1[a2] = 2;");
            I("x2[b1] = 3;");
            I("x2[b2] = 4;");
            I("x3[a1, b1, c1] = 5;");
            I("x3[a1, b1, c2] = 6;");
            I("x3[a1, b2, c1] = 7;");
            I("x3[a1, b2, c2] = 8;");
            I("x3[a2, b1, c1] = 9;");
            I("x3[a2, b1, c2] = 10;");
            I("x3[a2, b2, c1] = 11;");
            I("x3[a2, b2, c2] = 12;");

            //Here, #a and #b are free/unfolded, so it is unfolded into 2x2 = 4 columns.
            //The last sum() puts bounds on #a and #b, so the values of these are not in-substituted there
            //first one is:
            //x1[a1] + x2[b1] + sum((#a, #b), x3[#a, #b, c1]);, 1+3+32 = 36.
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> x1[#a] + x2[#b] + sum((#a, #b), x3[#a, #b, #c]);");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(7, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x1[a1] +");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], " x2[b1] + sum");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 2).CellText.TextData[0], "[#a, #b, c1])");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x1[a1] +");
            Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], " x2[b1] + sum");
            Assert.AreEqual(table.Get(3, 3).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 3).CellText.TextData[0], "[#a, #b, c2])");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x1[a1] +");
            Assert.AreEqual(table.Get(2, 4).CellText.TextData[0], " x2[b2] + sum");
            Assert.AreEqual(table.Get(3, 4).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 4).CellText.TextData[0], "[#a, #b, c1])");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "x1[a1] +");
            Assert.AreEqual(table.Get(2, 5).CellText.TextData[0], " x2[b2] + sum");
            Assert.AreEqual(table.Get(3, 5).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 5).CellText.TextData[0], "[#a, #b, c2])");
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "x1[a2] +");
            Assert.AreEqual(table.Get(2, 6).CellText.TextData[0], " x2[b1] + sum");
            Assert.AreEqual(table.Get(3, 6).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 6).CellText.TextData[0], "[#a, #b, c1])");
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "x1[a2] +");
            Assert.AreEqual(table.Get(2, 7).CellText.TextData[0], " x2[b1] + sum");
            Assert.AreEqual(table.Get(3, 7).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 7).CellText.TextData[0], "[#a, #b, c2])");
            Assert.AreEqual(table.Get(1, 8).CellText.TextData[0], "x1[a2] +");
            Assert.AreEqual(table.Get(2, 8).CellText.TextData[0], " x2[b2] + sum");
            Assert.AreEqual(table.Get(3, 8).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 8).CellText.TextData[0], "[#a, #b, c1])");
            Assert.AreEqual(table.Get(1, 9).CellText.TextData[0], "x1[a2] +");
            Assert.AreEqual(table.Get(2, 9).CellText.TextData[0], " x2[b2] + sum");
            Assert.AreEqual(table.Get(3, 9).CellText.TextData[0], "((#a, #b), x3");
            Assert.AreEqual(table.Get(4, 9).CellText.TextData[0], "[#a, #b, c2])");
            Assert.AreEqual(table.Get(5, 2).number, 36d, 0.0001);
            Assert.AreEqual(table.Get(5, 3).number, 40d, 0.0001);
            Assert.AreEqual(table.Get(5, 4).number, 37d, 0.0001);
            Assert.AreEqual(table.Get(5, 5).number, 41d, 0.0001);
            Assert.AreEqual(table.Get(5, 6).number, 37d, 0.0001);
            Assert.AreEqual(table.Get(5, 7).number, 41d, 0.0001);
            Assert.AreEqual(table.Get(5, 8).number, 38d, 0.0001);
            Assert.AreEqual(table.Get(5, 9).number, 42d, 0.0001);

            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> x1[#a] + x2[#b] + sum(#a, sum(#b, x3[#a, #b, #c]));");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(7, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x1[a1] + x2");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "[b1] + sum(#a");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 2).CellText.TextData[0], "#a, #b, c1]))");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x1[a1] + x2");
            Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], "[b1] + sum(#a");
            Assert.AreEqual(table.Get(3, 3).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 3).CellText.TextData[0], "#a, #b, c2]))");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x1[a1] + x2");
            Assert.AreEqual(table.Get(2, 4).CellText.TextData[0], "[b2] + sum(#a");
            Assert.AreEqual(table.Get(3, 4).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 4).CellText.TextData[0], "#a, #b, c1]))");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "x1[a1] + x2");
            Assert.AreEqual(table.Get(2, 5).CellText.TextData[0], "[b2] + sum(#a");
            Assert.AreEqual(table.Get(3, 5).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 5).CellText.TextData[0], "#a, #b, c2]))");
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "x1[a2] + x2");
            Assert.AreEqual(table.Get(2, 6).CellText.TextData[0], "[b1] + sum(#a");
            Assert.AreEqual(table.Get(3, 6).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 6).CellText.TextData[0], "#a, #b, c1]))");
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "x1[a2] + x2");
            Assert.AreEqual(table.Get(2, 7).CellText.TextData[0], "[b1] + sum(#a");
            Assert.AreEqual(table.Get(3, 7).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 7).CellText.TextData[0], "#a, #b, c2]))");
            Assert.AreEqual(table.Get(1, 8).CellText.TextData[0], "x1[a2] + x2");
            Assert.AreEqual(table.Get(2, 8).CellText.TextData[0], "[b2] + sum(#a");
            Assert.AreEqual(table.Get(3, 8).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 8).CellText.TextData[0], "#a, #b, c1]))");
            Assert.AreEqual(table.Get(1, 9).CellText.TextData[0], "x1[a2] + x2");
            Assert.AreEqual(table.Get(2, 9).CellText.TextData[0], "[b2] + sum(#a");
            Assert.AreEqual(table.Get(3, 9).CellText.TextData[0], ", sum(#b, x3[");
            Assert.AreEqual(table.Get(4, 9).CellText.TextData[0], "#a, #b, c2]))");
            Assert.AreEqual(table.Get(5, 2).number, 36d, 0.0001);
            Assert.AreEqual(table.Get(5, 3).number, 40d, 0.0001);
            Assert.AreEqual(table.Get(5, 4).number, 37d, 0.0001);
            Assert.AreEqual(table.Get(5, 5).number, 41d, 0.0001);
            Assert.AreEqual(table.Get(5, 6).number, 37d, 0.0001);
            Assert.AreEqual(table.Get(5, 7).number, 41d, 0.0001);
            Assert.AreEqual(table.Get(5, 8).number, 38d, 0.0001);
            Assert.AreEqual(table.Get(5, 9).number, 42d, 0.0001);

            // ------------------------ test of label substitution for name-composition ----------------

            //see above, same idea as for arrays
            I("reset;");
            I("time 2001 2003;");
            I("#a = a1, a2;");
            I("#b = b1, b2;");
            I("#c = c1, c2;");
            I("x1a1 = 1;");
            I("x1a2 = 2;");
            I("x2b1 = 3;");
            I("x2b2 = 4;");
            I("x3a1b1c1 = 5;");
            I("x3a1b1c2 = 6;");
            I("x3a1b2c1 = 7;");
            I("x3a1b2c2 = 8;");
            I("x3a2b1c1 = 9;");
            I("x3a2b1c2 = 10;");
            I("x3a2b2c1 = 11;");
            I("x3a2b2c2 = 12;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> x1{#a} + x2{#b} + sum(#a, sum(#b, x3{#a}{#b}{#c}));");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "2001"); //why is it not a date?
            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "2002"); //why is it not a date?
            Assert.AreEqual(table.Get(7, 1).CellText.TextData[0], "2003"); //why is it not a date?
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x1a1 ");
            Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "+ x2b1 + sum(");
            Assert.AreEqual(table.Get(3, 2).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 2).CellText.TextData[0], "3{#a}{#b}c1))");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x1a1 ");
            Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], "+ x2b1 + sum(");
            Assert.AreEqual(table.Get(3, 3).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 3).CellText.TextData[0], "3{#a}{#b}c2))");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x1a1 ");
            Assert.AreEqual(table.Get(2, 4).CellText.TextData[0], "+ x2b2 + sum(");
            Assert.AreEqual(table.Get(3, 4).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 4).CellText.TextData[0], "3{#a}{#b}c1))");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "x1a1 ");
            Assert.AreEqual(table.Get(2, 5).CellText.TextData[0], "+ x2b2 + sum(");
            Assert.AreEqual(table.Get(3, 5).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 5).CellText.TextData[0], "3{#a}{#b}c2))");
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "x1a2 ");
            Assert.AreEqual(table.Get(2, 6).CellText.TextData[0], "+ x2b1 + sum(");
            Assert.AreEqual(table.Get(3, 6).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 6).CellText.TextData[0], "3{#a}{#b}c1))");
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "x1a2 ");
            Assert.AreEqual(table.Get(2, 7).CellText.TextData[0], "+ x2b1 + sum(");
            Assert.AreEqual(table.Get(3, 7).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 7).CellText.TextData[0], "3{#a}{#b}c2))");
            Assert.AreEqual(table.Get(1, 8).CellText.TextData[0], "x1a2 ");
            Assert.AreEqual(table.Get(2, 8).CellText.TextData[0], "+ x2b2 + sum(");
            Assert.AreEqual(table.Get(3, 8).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 8).CellText.TextData[0], "3{#a}{#b}c1))");
            Assert.AreEqual(table.Get(1, 9).CellText.TextData[0], "x1a2 ");
            Assert.AreEqual(table.Get(2, 9).CellText.TextData[0], "+ x2b2 + sum(");
            Assert.AreEqual(table.Get(3, 9).CellText.TextData[0], "#a, sum(#b, x");
            Assert.AreEqual(table.Get(4, 9).CellText.TextData[0], "3{#a}{#b}c2))");
            Assert.AreEqual(table.Get(5, 2).number, 36d, 0.0001);
            Assert.AreEqual(table.Get(5, 3).number, 40d, 0.0001);
            Assert.AreEqual(table.Get(5, 4).number, 37d, 0.0001);
            Assert.AreEqual(table.Get(5, 5).number, 41d, 0.0001);
            Assert.AreEqual(table.Get(5, 6).number, 37d, 0.0001);
            Assert.AreEqual(table.Get(5, 7).number, 41d, 0.0001);
            Assert.AreEqual(table.Get(5, 8).number, 38d, 0.0001);
            Assert.AreEqual(table.Get(5, 9).number, 42d, 0.0001);

            //------------- more tests of labels ------------

            I("reset;");
            I("time 2001 2003;");
            I("#a = a1, a2;");
            I("%s = 'a3';");
            I("a1 = 1;");
            I("a2 = 2;");            
            I("a3 = 3;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> {#a}, {%s};");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "a1");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "a2");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "a3");

            I("reset;");
            I("time 2001 2003;");
            I("#a = a1, a2;");
            I("%s = 'a';");
            I("%i2 = 3;");
            I("%s2 = 'i';");
            I("%s3 = 's2';");
            I("a1z = 1;");
            I("a2z = 2;");
            I("a3z = 3;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> {#a+'z'}, {%s+%{%{''+%s3}+''}2+'z'};");  //in {#a...} a plus or minus is allowed. Nothing else.
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "a1z");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "a2z");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "a3z");

            I("reset;");
            I("time 2001 2003;");
            I("#a = a1, a2;");
            I("%s = 'a3';");
            I("xx = series(1);");
            I("xx[a1] = 1;");
            I("xx[a2] = 2;");
            I("xx[a3] = 3;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> xx[#a], xx[%s];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a1]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a2]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[a3]");

            I("reset;");
            I("time 2001 2003;");
            I("function series f(series x, string %s); return x[%s]; end;");
            I("#a = a1, a2;");
            I("%s = 'a';");
            I("%i2 = 3;");
            I("%s2 = 'i';");
            I("%s3 = 's2';");
            I("xx = series(1);");
            I("xx[a1z] = 1;");
            I("xx[a2z] = 2;");
            I("xx[a3z] = 3;");
            I("%s9 = 'x';");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> {%s9+%s9}[#a+'z'], {%s9+%s9}[%s+%{%{''+%s3}+''}2+'z'], f({%s9+%s9}, 'a3z');");  //in {#a...} a plus or minus is allowed. Nothing else.
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx[a1z]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "xx[a2z]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx[a3z]");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "f(xx, 'a3z')");

            //test that indexers and labels work also when the variable (#set) is a function argument.
            I("reset;");
            I("FUNCTION string print_with_set(list #set); PRINT <n> qC[#set]; RETURN ''; END;");
            I("series qc = series(1);");
            I("qc[a] = 1;");
            I("qc[b] = 2;");
            I("list #c = a, b;");
            //Globals.lastPrtOrMulprtTable = null;
            I("TELL print_with_set(#c); ");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "qC[a]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "qC[b]");

            //This is fundamentally a test of PRT ... file=..., where file= accepts both a name and a string.
            //So you can use file=sub\xx.txt or file='sub\xx.txt' or file=%s, where %s is a string.
            //The test also tests PLOT.
            I("reset;");
            Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
            Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
            Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp\analysis");
            Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp\analysis\graphs");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
            I("%output_path = 'Analysis\\Graphs\\';");  //remember \\ is really \
            I("FUNCTION string plotx(string %x); %fp1 = %output_path + %x + '1' + '.svg'; %fp2 = %output_path + %x + '2' + '.svg'; PLOT <title=%x> {%x} file = %fp1; PLOT <title=%x> {%x} file = {%fp2}; RETURN ''; END;");
            I("qc = series(1);");
            I("qc[a] = 1;");
            I("qc[b] = 2;");
            //Globals.lastPrtOrMulprtTable = null;
            I("TELL plotx('qC');");
            string[] ss = Directory.GetFiles(Globals.ttPath2 + @"\regres\Databanks\temp\analysis\graphs");
            Assert.AreEqual(ss.Length, 2);
            Assert.IsTrue(ss[0].EndsWith("qC1.svg"));
            Assert.IsTrue(ss[1].EndsWith("qC2.svg"));

            // --------------------------------------------------------------

            //test print of indexers [-1], [2011], [2011a1]
            I("reset; time 2010 2012;");
            I("x = (1, 2, 3);");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> x, x[-1], x[2011], x[2011a1];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[-1]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[2011]");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "x[2011a1]");
            Assert.AreEqual(table.Get(2, 2).number, 1);
            Assert.AreEqual(table.Get(2, 3).number, double.NaN);
            Assert.AreEqual(table.Get(2, 4).number, 2);
            Assert.AreEqual(table.Get(2, 5).number, 2);
            Assert.AreEqual(table.Get(3, 2).number, 2);
            Assert.AreEqual(table.Get(3, 3).number, 1);
            Assert.AreEqual(table.Get(3, 4).number, 2);
            Assert.AreEqual(table.Get(3, 5).number, 2);
            Assert.AreEqual(table.Get(4, 2).number, 3);
            Assert.AreEqual(table.Get(4, 3).number, 2);
            Assert.AreEqual(table.Get(4, 4).number, 2);
            Assert.AreEqual(table.Get(4, 5).number, 2);

            //test print of list of array-series
            I("reset; time 2010 2012;");
            I("#n = ('n',); #a = ('a',); %n = 'n'; %a = 'a';");
            I("a = series(1);");
            I("a[x] = 200;");
            I("a[y] = 300;");
            I("n = 100;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p<n> a, a, n, 1;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "a[x]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "a[y]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "a[x]");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "a[y]");
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "n");
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "1");
            //Globals.lastPrtOrMulprtTable = null;
            I("p<n> {#a}, {#a}, {#n}, 1;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "a[x]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "a[y]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "a[x]");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "a[y]");
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "n");
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "1");
            if (Globals.UNITTESTFOLLOWUP)
            {
                //Globals.lastPrtOrMulprtTable = null;
                I("p<n> {#{%a + ''}}, {#{%a + ''}}, {#{%n + ''}}, 1;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "a[x]");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "a[y]");
                Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "a[x]");
                Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "a[y]");
                Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "n");
                Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "1");
            }

            //testing {}-name indexed with #m: {...}[#m]
            I("reset; time 2001 2002;");
            I("a = series(1);");
            I("b = series(1);");
            I("#s = ('s1', 's2');");
            I("a[#s] = 1;");
            I("b[#s] = 2;");
            //Globals.lastPrtOrMulprtTable = null;
            I("FOR string %var_name = ('a', 'b'); P <n> {%var_name}[#s]; END;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "b[s1]");  //table only records the last of %var_name
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "b[s2]");
            Assert.AreEqual(table.Get(2, 2).number, 2);
            Assert.AreEqual(table.Get(2, 3).number, 2);
            Assert.AreEqual(table.Get(3, 2).number, 2);
            Assert.AreEqual(table.Get(3, 3).number, 2);

            //Test lag and lead of age, x[#a-1] and x[#a+1]
            I("reset; time 2001 2002;");
            I("x = series(1);");
            I("x['18'] = 100;");
            I("x['19'] = 200;");
            I("x['20'] = 300;");
            I("#a = ('19', '20');");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> x[#a], x[#a-1];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[19]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[20]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[18]");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "x[19]");
            Assert.AreEqual(table.Get(2, 2).number, 200d);
            Assert.AreEqual(table.Get(2, 3).number, 300d);
            Assert.AreEqual(table.Get(2, 4).number, 100d);
            Assert.AreEqual(table.Get(2, 5).number, 200d);
            I("reset; time 2001 2002;");
            I("x = series(1);");
            I("x['18'] = 100;");
            I("x['19'] = 200;");
            I("x['20'] = 300;");
            I("#a = ('18', '19');");
            //Globals.lastPrtOrMulprtTable = null;
            I("p <n> x[#a], x[#a+1];");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "x[18]");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x[19]");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x[19]");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "x[20]");
            Assert.AreEqual(table.Get(2, 2).number, 100d);
            Assert.AreEqual(table.Get(2, 3).number, 200d);
            Assert.AreEqual(table.Get(2, 4).number, 200d);
            Assert.AreEqual(table.Get(2, 5).number, 300d);

        }

        [TestMethod]
        public void _Test_EndoExo()
        {
            //
            //exo x1[a, b] <2002 2004>, y1 <2001 2002>;
            //endo <2003 2005> x2[a, b] <2001 2001>, y2;
            //time 2000 2010;
            //p<n> exo_x1[a, b], exo_y1;
            //p<n> endo_x2[a, b], endo_y2;

            for (int i = 0; i < 2; i++)
            {

                I("reset;");
                I("%s1 = 'a';");
                I("string %s2 = 'x';");
                I("#a = ('a', 'b');");
                I("#k1 = ('k1',);");

                if (i == 0)
                {
                    FAIL("exo {%s2}[%s1+'', 'k1'] <2002 2004>, y<2004 2006>;");
                    FAIL("endo <2003 2005> x['a', 'k1']<2001 2002>, y;");

                    I("{%s2} = series(2);");
                    I("{%s2}[%s1+'', 'k1'] = 1000;");                    
                    I("y = 2000;");

                    I("exo {%s2}[%s1+'', 'k1'] <2002 2004>, y<2004 2006>;");
                    I("endo <2003 2005> x['a', 'k1']<2001 2002>, y;");
                }
                else
                {
                    FAIL("exo {%s2}[#a, #k1] <2002 2004>, y<2004 2006>;");
                    FAIL("endo <2003 2005> x[#a, #k1]<2001 2002>, y;");

                    I("{%s2} = series(2);");
                    I("{%s2}[#a, #k1] = 1000;");
                    I("y = 2000;");

                    I("exo {%s2}[#a, #k1] <2002 2004>, y<2004 2006>;");
                    I("endo <2003 2005> x[#a, #k1]<2001 2002>, y;");
                }

                _AssertSeries(First(), "exo_x", new string[] { "a", "k1" }, 2001, double.NaN, sharedDelta);
                _AssertSeries(First(), "exo_x", new string[] { "a", "k1" }, 2002, 1d, sharedDelta);
                _AssertSeries(First(), "exo_x", new string[] { "a", "k1" }, 2003, 1d, sharedDelta);
                _AssertSeries(First(), "exo_x", new string[] { "a", "k1" }, 2004, 1d, sharedDelta);
                _AssertSeries(First(), "exo_x", new string[] { "a", "k1" }, 2005, double.NaN, sharedDelta);

                if (i == 1)
                {
                    _AssertSeries(First(), "exo_x", new string[] { "b", "k1" }, 2001, double.NaN, sharedDelta);
                    _AssertSeries(First(), "exo_x", new string[] { "b", "k1" }, 2002, 1d, sharedDelta);
                    _AssertSeries(First(), "exo_x", new string[] { "b", "k1" }, 2003, 1d, sharedDelta);
                    _AssertSeries(First(), "exo_x", new string[] { "b", "k1" }, 2004, 1d, sharedDelta);
                    _AssertSeries(First(), "exo_x", new string[] { "b", "k1" }, 2005, double.NaN, sharedDelta);
                }

                _AssertSeries(First(), "exo_y", 2003, double.NaN, sharedDelta);
                _AssertSeries(First(), "exo_y", 2004, 1d, sharedDelta);
                _AssertSeries(First(), "exo_y", 2005, 1d, sharedDelta);
                _AssertSeries(First(), "exo_y", 2006, 1d, sharedDelta);
                _AssertSeries(First(), "exo_y", 2007, double.NaN, sharedDelta);

                
                _AssertSeries(First(), "endo_x", new string[] { "a", "k1" }, 2000, double.NaN, sharedDelta);
                _AssertSeries(First(), "endo_x", new string[] { "a", "k1" }, 2001, 1d, sharedDelta);
                _AssertSeries(First(), "endo_x", new string[] { "a", "k1" }, 2002, 1d, sharedDelta);
                _AssertSeries(First(), "endo_x", new string[] { "a", "k1" }, 2003, double.NaN, sharedDelta);

                if (i == 1)
                {
                    _AssertSeries(First(), "endo_x", new string[] { "b", "k1" }, 2000, double.NaN, sharedDelta);
                    _AssertSeries(First(), "endo_x", new string[] { "b", "k1" }, 2001, 1d, sharedDelta);
                    _AssertSeries(First(), "endo_x", new string[] { "b", "k1" }, 2002, 1d, sharedDelta);
                    _AssertSeries(First(), "endo_x", new string[] { "b", "k1" }, 2003, double.NaN, sharedDelta);

                }

                _AssertSeries(First(), "endo_y", 2002, double.NaN, sharedDelta);
                _AssertSeries(First(), "endo_y", 2003, 1d, sharedDelta);
                _AssertSeries(First(), "endo_y", 2004, 1d, sharedDelta);
                _AssertSeries(First(), "endo_y", 2005, 1d, sharedDelta);
                _AssertSeries(First(), "endo_y", 2006, double.NaN, sharedDelta);
            }

        }

        [TestMethod]
        public void _Test_Print()
        {
            //bank/ref
            I("reset;");
            I("time 2001 2001;");
            I("open <edit> b1;");
            I("x = 2.2;");
            I("close b1;");
            I("open <edit> b2;");
            I("x = 1.8;");
            I("close b2;");
            I("reset; time 2001 2001;");
            I("open b1, b2;");
            I("x = 200;");
            I("ref:x = 100;");
            //Globals.lastPrtOrMulprtTable = null;
            I("prt <m> x;");
            Table table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 2).number, 100d, sharedDelta);
            //Globals.lastPrtOrMulprtTable = null;
            I("prt <bank=b1 ref=b2 m> x;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.Get(2, 2).number, 0.4d, sharedDelta);
        }

        [TestMethod]
        public void _Test_PrintMixedFrequencies()
        {
            //made by running the code and copy-pasting to Excel.
            I("reset;");
            I("time 2001 2003;");
            I("xx1 = (1,2,3);");
            I("xx2 = (4,5,6);");
            I("option freq q;");
            I("xx3 = (1,2,3,4,5,6,7,8,9,10,11,12);");
            I("option freq m;");
            I("xx4 = (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,18,29,30,31,32,33,34,35,36);");
            I("#m = ('xx3!q', 'xx4!m');");
            I("write slet;");
            I("read slet;");
            I("option freq a;");
            I("xx1 = xx1 + 1000;");
            I("xx2 = xx2 + 1000;");
            I("option freq q;");
            I("xx3 = xx3 + 1000;");
            I("option freq m;");
            I("xx4 = xx4 + 1000;");
            I("option freq a;");
            //Globals.lastPrtOrMulprtTable = null;
            I("p xx1, xx2, {#m};");            
            Table table = Globals.lastPrtOrMulprtTable;
            double deltaHere = 0.0001d;
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "xx1");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "%");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "xx2");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "%");
            Assert.AreEqual(table.Get(1, 6).CellText.TextData[0], "xx3!q");
            Assert.AreEqual(table.Get(1, 7).CellText.TextData[0], "%");
            Assert.AreEqual(table.Get(1, 8).CellText.TextData[0], "xx4!m");
            Assert.AreEqual(table.Get(1, 9).CellText.TextData[0], "%");

            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2001");

            Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "m1");
            Assert.AreEqual(table.Get(3, 8).number, 1001d);
            Assert.AreEqual(table.Get(3, 9).number, double.NaN);

            Assert.AreEqual(table.Get(4, 1).CellText.TextData[0], "m2");
            Assert.AreEqual(table.Get(4, 8).number, 1002d);
            Assert.AreEqual(table.Get(4, 9).number, 0.0999, deltaHere);

            Assert.AreEqual(table.Get(5, 1).CellText.TextData[0], "m3");
            Assert.AreEqual(table.Get(5, 8).number, 1003d);
            Assert.AreEqual(table.Get(5, 9).number, 0.0998, deltaHere);

            Assert.AreEqual(table.Get(6, 1).CellText.TextData[0], "mSUM");
            Assert.AreEqual(table.Get(6, 8).number, 1003d);
            Assert.AreEqual(table.Get(6, 9).number, 0.0998, deltaHere);

            Assert.AreEqual(table.Get(7, 1).CellText.TextData[0], "q1");
            Assert.AreEqual(table.Get(7, 6).number, 1001d);
            Assert.AreEqual(table.Get(7, 7).number, double.NaN);

            Assert.AreEqual(table.Get(8, 1).CellText.TextData[0], "m4");
            Assert.AreEqual(table.Get(8, 8).number, 1004d);
            Assert.AreEqual(table.Get(8, 9).number, 0.0997, deltaHere);

            Assert.AreEqual(table.Get(9, 1).CellText.TextData[0], "m5");
            Assert.AreEqual(table.Get(9, 8).number, 1005d);
            Assert.AreEqual(table.Get(9, 9).number, 0.0996, deltaHere);

            Assert.AreEqual(table.Get(10, 1).CellText.TextData[0], "m6");
            Assert.AreEqual(table.Get(10, 8).number, 1006d);
            Assert.AreEqual(table.Get(10, 9).number, 0.0995, deltaHere);

            Assert.AreEqual(table.Get(11, 1).CellText.TextData[0], "mSUM");
            Assert.AreEqual(table.Get(11, 8).number, 1006d);
            Assert.AreEqual(table.Get(11, 9).number, 0.0995, deltaHere);

            Assert.AreEqual(table.Get(12, 1).CellText.TextData[0], "q2");
            Assert.AreEqual(table.Get(12, 6).number, 1002d);
            Assert.AreEqual(table.Get(12, 7).number, 0.0999, deltaHere);

            //skip until row 24

            Assert.AreEqual(table.Get(24, 1).CellText.TextData[0], "qSUM");
            Assert.AreEqual(table.Get(24, 6).number, 1004d);
            Assert.AreEqual(table.Get(24, 7).number, 0.0997, deltaHere);

            Assert.AreEqual(table.Get(25, 1).CellText.TextData[0], "a");
            Assert.AreEqual(table.Get(25, 2).number, 1001d);
            Assert.AreEqual(table.Get(25, 3).number, double.NaN);
            Assert.AreEqual(table.Get(25, 4).number, 1004d);
            Assert.AreEqual(table.Get(25, 5).number, double.NaN);

            Assert.AreEqual(table.Get(27, 1).CellText.TextData[0], "2002");

            //skip until row 70

            Assert.AreEqual(table.Get(70, 1).CellText.TextData[0], "m12");
            Assert.AreEqual(table.Get(70, 8).number, 1036d);
            Assert.AreEqual(table.Get(70, 9).number, 0.0966, deltaHere);

            Assert.AreEqual(table.Get(71, 1).CellText.TextData[0], "mSUM");
            Assert.AreEqual(table.Get(71, 8).number, 1036d);
            Assert.AreEqual(table.Get(71, 9).number, 0.0966, deltaHere);

            Assert.AreEqual(table.Get(72, 1).CellText.TextData[0], "mSUM12");
            Assert.AreEqual(table.Get(72, 8).number, 1036d);
            Assert.AreEqual(table.Get(72, 9).number, 0.0966, deltaHere);

            Assert.AreEqual(table.Get(73, 1).CellText.TextData[0], "q4");
            Assert.AreEqual(table.Get(73, 6).number, 1012d);
            Assert.AreEqual(table.Get(73, 7).number, 0.0989, deltaHere);

            Assert.AreEqual(table.Get(74, 1).CellText.TextData[0], "qSUM");
            Assert.AreEqual(table.Get(74, 6).number, 1012d);
            Assert.AreEqual(table.Get(74, 7).number, 0.0989, deltaHere);

            Assert.AreEqual(table.Get(75, 1).CellText.TextData[0], "a");
            Assert.AreEqual(table.Get(75, 2).number, 1003d);
            Assert.AreEqual(table.Get(75, 3).number, 0.0998, deltaHere);
            Assert.AreEqual(table.Get(75, 4).number, 1006d);
            Assert.AreEqual(table.Get(75, 5).number, 0.09950, deltaHere);

        }



        

        [TestMethod]
        public void _Test_Assignment()
        {
            //There were some problems with glue and percent/hash, checked here
            I("RESET;");
            I("@%a = 100;");
            _AssertScalarVal(Ref(), "%a", 100d);
            I("@#a = (100, 200);");
            _AssertListVal(Ref(), "#a", 1, 100d, sharedDelta);
            _AssertListVal(Ref(), "#a", 2, 200d, sharedDelta);
            I("x=100;");
            I("x%=2;"); //just testing syntax 
            I("x#=2;"); //just testing syntax
            I("@x=100;");
            I("@x%=2;"); //just testing syntax 
            I("@x#=2;"); //just testing syntax
            I("@%a%=2;"); //just testing syntax, should fail afterwards 
            I("@%a#=2;"); //just testing syntax, should fail afterwards



        }

        [TestMethod]
        public void _Test_AssignmentOperators()
        {
            // test of <keep=p>
            I("RESET; TIME 2001 2006;");
            I("x = (0.5, 1.5, 1, 1.01, 1.01*1.03, 1.01*1.03*1.05);"); //1%, 3%, 5%            
            I("<2001 2003 m keep=p> x = 2;");  //adds 2 in 2001-2003, then follow old growth
            _AssertSeries(First(), "x", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x", 2001, 2.5d, sharedDelta);
            _AssertSeries(First(), "x", 2002, 3.5d, sharedDelta);
            _AssertSeries(First(), "x", 2003, 3d, sharedDelta);
            _AssertSeries(First(), "x", 2004, 3d * 1.01d, sharedDelta);
            _AssertSeries(First(), "x", 2005, 3d * 1.01d * 1.03d, sharedDelta);
            _AssertSeries(First(), "x", 2006, 3d * 1.01d * 1.03d * 1.05d, sharedDelta);            
            _AssertSeries(First(), "x", 2007, double.NaN, sharedDelta);


            //First a simple check of <p> x[a] = 3, and <p> x[#i] = 3....
            //After this, we can be pretty sure that operators work for array-series as for normal series.
            I("RESET; TIME 2001 2004;");
            I("x = series(1);");
            I("x[a] = 5;");
            I("<2002 2004 d> x[a] = 3;");
            _AssertSeries(First(), "x", new string[] { "a" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2001, 5d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2002, 8d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2003, 11d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2004, 14d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2005, double.NaN, sharedDelta);
            I("RESET; TIME 2001 2004;");
            I("x = series(1);");
            I("x[a] = 5;");
            I("x[b] = 10;");
            I("#i = a, b;");
            I("<2002 2004 d> x[#i] = 3;");
            _AssertSeries(First(), "x", new string[] { "a" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2001, 5d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2002, 8d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2003, 11d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2004, 14d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a" }, 2005, double.NaN, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b" }, 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b" }, 2001, 10d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b" }, 2002, 13d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b" }, 2003, 16d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b" }, 2004, 19d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b" }, 2005, double.NaN, sharedDelta);


            // ----------------------------------------------------
            // OPERATOR d and ^=
            // 16 combinations:
            //  - 2: raw series or from map
            //  - 4: rhs is scalar, series, list or matrix
            //  - 2: new or old style operator
            // ----------------------------------------------------

            int count = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        I("RESET; TIME 2001 2004;");
                        if (k == 0)
                        {
                            I("y = 5;");
                        }
                        else
                        {
                            I("#m = (y = 5, y2 = 6, %v = 2);"); //only y used, <2001 2004> could be removed when smpl bug is fixed
                        }
                        if (j == 0)
                        {
                            I("x1 = (m(), 1, 2, 3);");
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 d> y = x1;");
                                }
                                else
                                {
                                    I("<2002 2004 d> #m.y = x1;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y ^= x1;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y ^= x1;");
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 d> y = (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004 d> #m.y = (1, 2, 3);");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y ^= (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y ^= (1, 2, 3);");
                                }
                            }
                        }
                        else if (j == 2)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 d> y = [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004 d> #m.y = [1; 2; 3];");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y ^= [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y ^= [1; 2; 3];");
                                }
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 d> y = 3;");
                                }
                                else
                                {
                                    I("<2002 2004 d> #m.y = 3;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y ^= 3;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y ^= 3;");
                                }
                            }
                        }

                        if (j == 3)
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 8d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 11d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 14d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 8d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 11d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 14d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 6d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 8d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 11d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 6d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 8d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 11d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                    }
                }
            }
            Assert.AreEqual(count, 16);  //safety






            // ----------------------------------------------------
            // OPERATOR p and %=
            // 16 combinations:
            //  - 2: raw series or from map
            //  - 4: rhs is scalar, series, list or matrix
            //  - 2: new or old style operator
            // ----------------------------------------------------

            count = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        I("RESET; TIME 2001 2004;");
                        if (k == 0)
                        {
                            I("y = 5;");
                        }
                        else
                        {
                            I("#m = (y = 5, y2 = 6, %v = 2);"); //only y used, <2001 2004> could be removed when smpl bug is fixed
                        }
                        if (j == 0)
                        {
                            I("x1 = (m(), 1, 2, 3);");
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 p> y = x1;");
                                }
                                else
                                {
                                    I("<2002 2004 p> #m.y = x1;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y %= x1;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y %= x1;");
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 p> y = (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004 p> #m.y = (1, 2, 3);");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y %= (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y %= (1, 2, 3);");
                                }
                            }
                        }
                        else if (j == 2)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 p> y = [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004 p> #m.y = [1; 2; 3];");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y %= [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y %= [1; 2; 3];");
                                }
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 p> y = 3;");
                                }
                                else
                                {
                                    I("<2002 2004 p> #m.y = 3;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y %= 3;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y %= 3;");
                                }
                            }
                        }

                        if (j == 3)
                        {





                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 5d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 5d * 1.03d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 5d * 1.03d * 1.03d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 5d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 5d * 1.03d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 5d * 1.03d * 1.03d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 5d * 1.01d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 5d * 1.01d * 1.02d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 5d * 1.01d * 1.02d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 5d * 1.01d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 5d * 1.01d * 1.02d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 5d * 1.01d * 1.02d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }


                        }
                    }
                }
            }
            Assert.AreEqual(count, 16);  //safety

            
            // ----------------------------------------------------
            // OPERATOR m and += (then -= is probably ok too)
            // 16 combinations:
            //  - 2: raw series or from map
            //  - 4: rhs is scalar, series, list or matrix
            //  - 2: new or old style operator
            // ----------------------------------------------------

            count = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        I("RESET; TIME 2001 2004;");
                        if (k == 0)
                        {
                            I("y = 5;");
                        }
                        else
                        {
                            I("#m = (y = 5, y2 = 6, %v = 2);"); //only y used, <2001 2004> could be removed when smpl bug is fixed
                        }
                        if (j == 0)
                        {
                            I("x1 = (m(), 1, 2, 3);");
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 m> y = x1;");
                                }
                                else
                                {
                                    I("<2002 2004 m> #m.y = x1;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y += x1;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y += x1;");
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 m> y = (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004 m> #m.y = (1, 2, 3);");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y += (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y += (1, 2, 3);");
                                }
                            }
                        }
                        else if (j == 2)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 m> y = [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004 m> #m.y = [1; 2; 3];");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y += [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y += [1; 2; 3];");
                                }
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 m> y = 3;");
                                }
                                else
                                {
                                    I("<2002 2004 m> #m.y = 3;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y += 3;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y += 3;");
                                }
                            }
                        }

                        if (j == 3)
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 8d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 8d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 8d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 8d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 8d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 8d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 6d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 7d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 8d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 6d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 7d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 8d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                    }
                }
            }
            Assert.AreEqual(count, 16);  //safety




            // ----------------------------------------------------
            // OPERATOR q and *= (then /= is probably ok too)
            // 16 combinations:
            //  - 2: raw series or from map
            //  - 4: rhs is scalar, series, list or matrix
            //  - 2: new or old style operator
            // ----------------------------------------------------

            count = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        I("RESET; TIME 2001 2004;");
                        if (k == 0)
                        {
                            I("y = 5;");
                        }
                        else
                        {
                            I("#m = (y = 5, y2 = 6, %v = 2);"); //only y used, <2001 2004> could be removed when smpl bug is fixed
                        }
                        if (j == 0)
                        {
                            I("x1 = (m(), 1, 2, 3);");
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 q> y = x1;");
                                }
                                else
                                {
                                    I("<2002 2004 q> #m.y = x1;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y *= 1+x1/100;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y *= 1+x1/100;");
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 q> y = (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004 q> #m.y = (1, 2, 3);");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y *= (1.01, 1.02, 1.03);");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y *= (1.01, 1.02, 1.03);");
                                }
                            }
                        }
                        else if (j == 2)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 q> y = [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004 q> #m.y = [1; 2; 3];");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y *= [1.01; 1.02; 1.03];");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y *= [1.01; 1.02; 1.03];");
                                }
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 q> y = 3;");
                                }
                                else
                                {
                                    I("<2002 2004 q> #m.y = 3;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y *= 1.03;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y *= 1.03;");
                                }
                            }
                        }

                        if (j == 3)
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 5d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 5d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 5d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 5d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 5d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 5d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 5d * 1.01d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 5d * 1.02d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 5d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 5d * 1.01d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 5d * 1.02d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 5d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                    }
                }
            }
            Assert.AreEqual(count, 16);  //safety





            // ----------------------------------------------------
            // OPERATOR mp and #=
            // 16 combinations:
            //  - 2: raw series or from map
            //  - 4: rhs is scalar, series, list or matrix
            //  - 2: new or old style operator
            // ----------------------------------------------------

            count = 0;
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        I("RESET; TIME 2001 2004;");
                        if (k == 0)
                        {
                            //growth: 1%
                            I("y = (5, 5*1.01, 5*1.01*1.01, 5*1.01*1.01*1.01);");
                        }
                        else
                        {
                            //growth: 1%
                            I("#m = (y = (5, 5*1.01, 5*1.01*1.01, 5*1.01*1.01*1.01), y2 = 6, %v = 2);"); //only y used, <2001 2004> could be removed when smpl bug is fixed
                        }
                        if (j == 0)
                        {
                            I("x1 = (m(), 1, 2, 3);");
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 mp> y = x1;");
                                }
                                else
                                {
                                    I("<2002 2004 mp> #m.y = x1;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y #= x1;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y #= x1;");
                                }
                            }
                        }
                        else if (j == 1)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 mp> y = (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004 mp> #m.y = (1, 2, 3);");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y #= (1, 2, 3);");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y #= (1, 2, 3);");
                                }
                            }
                        }
                        else if (j == 2)
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 mp> y = [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004 mp> #m.y = [1; 2; 3];");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y #= [1; 2; 3];");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y #= [1; 2; 3];");
                                }
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004 mp> y = 3;");
                                }
                                else
                                {
                                    I("<2002 2004 mp> #m.y = 3;");
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    I("<2002 2004> y #= 3;");
                                }
                                else
                                {
                                    I("<2002 2004> #m.y #= 3;");
                                }
                            }
                        }

                        if (j == 3)
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 5d * 1.04d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 5d * 1.04d * 1.04d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 5d * 1.04d * 1.04d * 1.04d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 5d * 1.04d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 5d * 1.04d * 1.04d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 5d * 1.04d * 1.04d * 1.04d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                        else
                        {
                            if (k == 0)
                            {
                                _AssertSeries(First(), "y!a", 2000, double.NaN, sharedDelta);
                                _AssertSeries(First(), "y!a", 2001, 5d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2002, 5d * 1.02d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2003, 5d * 1.02d * 1.03d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2004, 5d * 1.02d * 1.03d * 1.04d, sharedDelta);
                                _AssertSeries(First(), "y!a", 2005, double.NaN, sharedDelta);
                                count++;
                            }
                            else
                            {
                                Map m = O.GetIVariableFromString("#m", O.ECreatePossibilities.NoneReturnNull) as Map;
                                Series y = m.GetIVariable("y!a") as Series;
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), double.NaN, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 5d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2002, 1)), 5d * 1.02d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2003, 1)), 5d * 1.02d * 1.03d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2004, 1)), 5d * 1.02d * 1.03d * 1.04d, sharedDelta);
                                _AssertHelperTwoDoubles(y.GetDataSimple(new GekkoTime(EFreq.A, 2005, 1)), double.NaN, sharedDelta);
                                count++;
                            }
                        }
                    }
                }
            }
            Assert.AreEqual(count, 16);  //safety


            



        }

        [TestMethod]
        public void _Test_UserDefinedFunctions()
        {
            I("RESET;");
            I("PROCEDURE f; %x = 111; END;");
            I("f;");
            _AssertScalarVal(First(), "%x", 111d);
            FAIL("f 1;");

            I("RESET;");
            I("PROCEDURE f val %z; %x = %z; END;");
            I("f 7;");
            _AssertScalarVal(First(), "%x", 7d);
            FAIL("f;");

            //It is not allowed to use a command name for a procedure name
            I("RESET;");
            FAIL("PROCEDURE time; tell 'hello'; END;");
            FAIL("PROCEDURE time date %d1, date %d2; tell 'hello'; END;");

            //But other name bits (tokens) that are used in commands are ok.
            I("RESET;");
            I("PROCEDURE gdx; tell 'hello'; END;");
            I("PROCEDURE gdx date %d1, date %d2; tell 'hello'; END;");

            I("RESET;");
            I("FUNCTION void f(); %x = 111; END;");
            I("f();");
            _AssertScalarVal(First(), "%x", 111d);
            FAIL("f(1);");
            FAIL("%y = f();");

            I("RESET;");
            I("FUNCTION val f(); %x = 111; RETURN 2; END;");
            I("f();");  //discarding the return 2 is ok
            _AssertScalarVal(First(), "%x", 111d);
            FAIL("f(1);");
            I("%y = f();");
            _AssertScalarVal(First(), "%y", 2d);

            //Type check 1
            I("RESET;");
            I("FUNCTION val f(val %x); %x = %x +1; RETURN %x; END;");
            I("%v = f(3);");
            _AssertScalarVal(First(), "%v", 4d);

            //Type check 1
            I("RESET;");
            I("FUNCTION val f(val %x); RETURN %x+%x; END;");
            I("%v = f(3);");
            _AssertScalarVal(First(), "%v", 6d);

            //Type check 2, this should fail with type error
            I("RESET;");
            I("FUNCTION val f(string %x); RETURN %x+%x; END;");
            FAIL("%v = f(3);");

            //Type check 3, this should fail with type error
            I("RESET;");
            I("FUNCTION string f(val %x); RETURN %x+%x; END;");
            FAIL("%v = f(3);");

            //Type check 4, this should fail with type error
            I("RESET;");
            I("FUNCTION val f(val %x); RETURN %x+%x; END;");
            FAIL("%v = f('3');");

            //Type check 5
            I("RESET;");
            I("FUNCTION string f(string %x); RETURN %x+%x; END;");
            I("%v = f('3');");
            _AssertScalarString(First(), "%v", "33");

            //Type check 6, this should fail with type error
            I("RESET;");
            I("FUNCTION string f(val %x); RETURN %x+%x; END;");
            FAIL("%v = f('3');");

            //Type check 7, this should fail with type error
            I("RESET;");
            I("FUNCTION val f(string %x); RETURN %x+%x; END;");
            FAIL("%v = f('3');");

            I("RESET;");
            I("function list add2(val %x, val %y); return (%x, %x +%y); end;");
            I("%v = add2(10, 20)[2];");
            _AssertScalarVal(First(), "%v", 30d);

            I("RESET;");
            I("function val f(val %x); return 2*%x; end;");
            I("%v1 = 100;");
            I("%v2 = f(f(%v1));");
            _AssertScalarVal(First(), "%v2", 400d);
        }

        [TestMethod]
        public void _Test_Rename()
        {
            //See also Test_CopyLogic(), where banks, wildcards, <tobank> and <frombank> are tested
            //Here, objects are actually renamed

            //NOTE: rename between banks works kind of like a MOVE-command.

            //  Work     Ref      b1       b10
            //-------------------------------------
            //  x1       x1       x3       <empty>
            //  %x1      %x1      %x3
            //  #x1      #x1      #x3
            //  x2

            // Work:x1 = 11 from 2001-2003

            TestCopyHelper(0);

            I("RENAME x1 to b10:x2;");
            Assert.IsFalse(Program.databanks.GetFirst().ContainsIVariable("x1!a"));
            Assert.IsTrue(Program.databanks.GetDatabank("b10").ContainsIVariable("x2!a"));
                        
            I("RENAME ref:#x1 as *:#x20;");
            Assert.IsFalse(Program.databanks.GetRef().ContainsIVariable("#x1"));
            Assert.IsTrue(Program.databanks.GetRef().ContainsIVariable("#x20"));

            I("RENAME x2, %x1 as x2000, %x1000;");
            Assert.IsFalse(Program.databanks.GetFirst().ContainsIVariable("x2"));
            Assert.IsFalse(Program.databanks.GetFirst().ContainsIVariable("%x1"));
            Assert.IsTrue(Program.databanks.GetFirst().ContainsIVariable("x2000!a"));
            Assert.IsTrue(Program.databanks.GetFirst().ContainsIVariable("%x1000"));

        }

        [TestMethod]
        public void _Test_Copy1()
        {
            //See also Test_CopyLogic(), where banks, wildcards, <tobank> and <frombank> are tested
            //Here, objects are actually copied

            //  Work     Ref      b1       b10
            //-------------------------------------
            //  x1       x1       x3       <empty>
            //  %x1      %x1      %x3
            //  #x1      #x1      #x3
            //  x2

            // Work:x1 = 11 from 2001-2003

            TestCopyHelper(0);

            I("COPY x1 to b10:x2;");
            _AssertSeries(First(), "x1", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1", 2001, 11d, sharedDelta);
            _AssertSeries(First(), "x1", 2002, 11d, sharedDelta);
            _AssertSeries(First(), "x1", 2003, 11d, sharedDelta);
            _AssertSeries(First(), "x1", 2004, double.NaN, sharedDelta);            
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2000, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2001, 11d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2002, 11d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2003, 11d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2004, double.NaN, sharedDelta);

            I("SERIES<2000 2005> x4 = 12;");
            I("COPY <2002 2003 respect> x4 to b10:x2;");
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2000, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2001, 11d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2002, 12d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2003, 12d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2004, double.NaN, sharedDelta);

            I("SERIES<2000 2005> x5 = 13;");            
            I("TIME 2002 2002;");
            I("COPY <respect> x5 to b10:x2;");
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2000, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2001, 11d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2002, 13d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2003, 12d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("b10"), "x2", 2004, double.NaN, sharedDelta);

            I("LIST #m = ('x1',);");
            I("COPY <2002 2003 respect> #m to #m2;");
            
            //TODO: test that the copied series is truncated
            //TODO: also test array-series.

            FAIL("COPY x100 to x101;");
            I("COPY <error = no> x100 to x101;");


        }

        [TestMethod]
        public void _Test_CopyLogic()
        {
            //This also implicitly tests a lot of RENAME functionality,
            //since COPY and RENAME share a lot of code.

            TestCopyHelper(0);

            //  Work     Ref      b1       b10
            //-------------------------------------
            //  x1       x1       x3       <empty>
            //  %x1      %x1      %x3
            //  #x1      #x1      #x3
            //  x2
            //
            

            // copy <from:b1 to:b10> x3, work:x2 to ref:y1, y2;   b1:x3 -> ref:y1, work:x2 -> b10:y2
            // copy <from:b1 to:b10> * to *;


            //Testing copy with these cases before TO
            //
            // a: no bank
            // b: given bank
            // c: wildcard bank
            //
            // names:
            //            
            // 01: list of given names
            // 02: wildcard name (can be complicated)
            //
            // After TO:
            //
            // A: no bank
            // B: given bank
            // C: wildcard bank (only '*')
            //            
            // 11: list of given names
            // 12: wildcard name (must single-star)        

            //---------------------------------------

            // =====================================================================
            // =========== No bank on LHS ==========================================
            // =====================================================================

            //a-01-A-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                //trying {...}-names, only here
                I("copy {'x1'} to {'y1'};");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x1 to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //a-01-A-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x1 to a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax1b!a");

            //---------------------------------------
            //a-01-B-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x1 to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            //---------------------------------------
            //a-01-B-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x1 to b:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:ax1b!a");

            //---------------------------------------
            //a-01-C-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x1 to *:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //a-01-C-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x1 to *:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax1b!a");


            // ========================================================
            // ========================================================

            //a-02-A-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x2* to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //a-02-A-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x2* to a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax2b!a");

            //---------------------------------------
            //a-02-B-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x2* to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            try
            {
                Globals.unitTestCopyHelper2 = true;
                //Test that tobank can be overwritten
                I("copy <tobank = x> x2* to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            //---------------------------------------
            //a-01-B-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x2* to b:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:ax2b!a");

            //---------------------------------------
            //a-01-C-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x2* to *:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //a-01-C-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy x2* to *:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax2b!a");

            // =====================================================================
            // =========== Given bank on LHS =======================================
            // =====================================================================

            //b-01-A-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x3 to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            try
            {
                Globals.unitTestCopyHelper2 = true;
                //Test that frombank can be overwritten
                I("copy <frombank = x> b1:x3 to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //b-01-A-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x3 to a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax3b!a");

            //---------------------------------------
            //b-01-B-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x3 to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            try
            {
                Globals.unitTestCopyHelper2 = true;
                //Test frombank/tobank
                I("copy <frombank = b1 tobank = b> x3 to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            try
            {
                Globals.unitTestCopyHelper2 = true;
                //more test frombank/tobank
                I("copy <frombank = x1 tobank = x2> b1:x3 to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");


            //---------------------------------------
            //b-01-B-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x3 to b:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:ax3b!a");

            //---------------------------------------
            //b-01-C-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x3 to *:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:y1!a");

            //---------------------------------------
            //b-01-C-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x3 to *:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:ax3b!a");

            // ========================================================
            // ========================================================

            //b-02-A-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x? to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //b-02-A-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x? to a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax3b!a");

            //---------------------------------------
            //b-02-B-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x? to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            //---------------------------------------
            //b-01-B-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x? to b:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:ax3b!a");

            //---------------------------------------
            //b-01-C-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x? to *:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:y1!a");

            //---------------------------------------
            //b-01-C-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b1:x? to *:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:ax3b!a");



            // =====================================================================
            // =========== Wildcard bank on LHS ====================================
            // =====================================================================

            //c-01-A-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x3 to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //c-01-A-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x3 to a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax3b!a");

            //---------------------------------------
            //c-01-B-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x3 to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            //---------------------------------------
            //c-01-B-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x3 to b:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:ax3b!a");

            //---------------------------------------
            //c-01-C-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x3 to *:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:y1!a");

            //---------------------------------------
            //c-01-C-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x3 to *:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:ax3b!a");

            // ========================================================
            // ========================================================

            //c-02-A-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b*:x? to y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:y1!a");

            //---------------------------------------
            //c-02-A-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b*:x? to a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax3b!a");

            //---------------------------------------
            //c-02-B-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b*:x? to b:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:y1!a");

            //---------------------------------------
            //c-01-B-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b*:x? to b:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b:ax3b!a");

            //---------------------------------------
            //c-01-C-11
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy b*:x? to *:y1;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 1);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "b1:y1!a");

            //---------------------------------------
            //c-01-C-12
            try
            {
                Globals.unitTestCopyHelper2 = true;
                I("copy *:x? to *:a*b;");
            }
            finally
            {
                Globals.unitTestCopyHelper2 = false;
            }
            Assert.AreEqual(Globals.unitTestCopyHelper.Count, 4);
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s1, "Work:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[0].s2, "Work:ax1b!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[1].s1, "Work:x2!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[1].s2, "Work:ax2b!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[2].s1, "Ref:x1!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[2].s2, "Ref:ax1b!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[3].s1, "b1:x3!a");
            Assert.AreEqual(Globals.unitTestCopyHelper[3].s2, "b1:ax3b!a");


        }

        private static void TestCopyHelper(int i)
        {
            I("reset; time 2001 2003;");
            I("x1 = 11;");
            I("%x1 = 12;");
            I("#x1 = (13,);");
            if (i == 1) I("<2001q1 2003q4> x1!q = 111;");
            I("clone;");  //copies to Ref
            I("x2 = 21;");  //special for Work
            I("open<edit>b1; clear b1;");
            I("x3 = 31;");
            I("%x3 = 32;");
            I("#x3 = (33,);");
            if (i == 1) I("<2001q1 2003q4> x3!q = 131;");
            I("close b1; open b1; unlock b1;");
            //hmmm seems the only way to create a fresh bank not in first position
            I("open<edit>b10; x = 1; close b10; open b10; unlock b10; clear b10;"); //fresh destination bank
            
            
        }


        [TestMethod]
        public void _Test_For()
        {
            I("reset; for string %i = ('a', 'b'); tell '' + %i; end;");
            if (Globals.UNITTESTFOLLOWUP_important)
            {
                I("reset; for val %i = (1, 2); tell '' + %i; end;");
                I("reset; for date %i = (2001q1, 2001q3); tell '' + %i; end;");
                I("reset; series a = 100, series b = 200; for series %i = (a, b); prt %i; end;");
                I("reset; series a = 100, series b = 200; for string %i = ('a', 'b'); prt {%i}; end;");
            }

            //parallel for loop
            I("#m1 = ('a', 'b');");
            I("#m2 = ('x', 'y');");
            I("#m3 = list();");
            I("for string %i = #m1, string %j = #m2; %s = %i + %j; #m3 = #m3 + (%s,); end;");
            _AssertListString(First(), "#m3", 1, "ax");
            _AssertListString(First(), "#m3", 2, "by");

            I("#m3 = list();");
            I("for string %i = #m1, string %j = #m2; %s = %i + %j; #m3 = #m3 + %s; end;");
            _AssertListSize(First(), "#m3", 0);  //a list + string appends string to each element in list

            I("#m3 = list();");
            I("for string %i = #m1, string %j = #m2; %s = %i + %j; #m3 += %s; end;");
            _AssertListSize(First(), "#m3", 0);  //a list + string appends string to each element in list

            I("for string %i = #m1, string %j = #m2; %s = %i + %j; #m3 = #m3.append(%s); end;");
            _AssertListString(First(), "#m3", 1, "ax");
            _AssertListString(First(), "#m3", 2, "by");

            I("time 2001 2001; xx = 0;");
            I("for val %i = 1 to 1e2; xx = xx + %i; end;");
            _AssertSeries(First(), "xx", 2001, 5050d, sharedDelta);

            I("%sum = 0;");
            I("for val %i = 1 to 1e2; %sum += %i; end;");
            _AssertScalarVal(First(), "%sum", 5050d);

            I("function val add1(val %x); return %x + 1; end;");
            I("%sum = 0;");
            I("for val %i = 1 to 1e2; %sum = add1(%sum); end;");
            _AssertScalarVal(First(), "%sum", 100d);

        }

        [TestMethod]
        public void _Test_Map()
        {
            I("reset;");
            I("time 2001 2003;");
            I("xx = 100;");
            I("#m = (%i1 = 'a', #mm = (%i1 = 'b', %i2 = 'c', ts = xx, var<2010 2011> ts = (1, 2)), %v1 = 100);");
            I("#m.%v1 += 1;");
            I("#m.%v1 = #m.%v1 + 1;");
            I("p #m.%i1;");
            I("p #m.#mm.%i1;");
            I("p #m.#mm.%i2;");
            I("p #m.#mm.ts;");

            Map m1 = Program.databanks.GetFirst().GetIVariable("#m") as Map;
            Map m2 = m1.GetIVariable("#mm") as Map;

            _AssertScalarString(m1, "%i1", "a");
            _AssertScalarVal(m1, "%v1", 102d);
            _AssertScalarString(m2, "%i1", "b");
            _AssertScalarString(m2, "%i2", "c");
            _AssertSeries(m2, "ts", 2000, double.NaN, sharedDelta);
            _AssertSeries(m2, "ts", 2001, 100d, sharedDelta);
            _AssertSeries(m2, "ts", 2002, 100d, sharedDelta);
            _AssertSeries(m2, "ts", 2003, 100d, sharedDelta);
            _AssertSeries(m2, "ts", 2004, double.NaN, sharedDelta);
            _AssertSeries(m2, "ts", 2009, double.NaN, sharedDelta);
            _AssertSeries(m2, "ts", 2010, 1d, sharedDelta);
            _AssertSeries(m2, "ts", 2011, 2d, sharedDelta);
            _AssertSeries(m2, "ts", 2012, double.NaN, sharedDelta);

            I("write slet;");
            I("reset;");
            I("read slet;");
            I("p #m.%i1;");
            I("p #m.#mm.%i1;");
            I("p #m.#mm.%i2;");
            I("p #m.#mm.ts;");
            I("p #m['%i1'];");
            I("p #m['#mm']['%i1'];");
            I("p #m['#mm']['%i2'];");
            I("p #m['#mm']['ts'];");

            m1 = Program.databanks.GetFirst().GetIVariable("#m") as Map;
            m2 = m1.GetIVariable("#mm") as Map;

            _AssertScalarString(m1, "%i1", "a");
            _AssertScalarString(m2, "%i1", "b");
            _AssertScalarString(m2, "%i2", "c");
            _AssertSeries(m2, "ts", 2000, double.NaN, sharedDelta);
            _AssertSeries(m2, "ts", 2001, 100d, sharedDelta);
            _AssertSeries(m2, "ts", 2002, 100d, sharedDelta);
            _AssertSeries(m2, "ts", 2003, 100d, sharedDelta);
            _AssertSeries(m2, "ts", 2004, double.NaN, sharedDelta);
            _AssertSeries(m2, "ts", 2009, double.NaN, sharedDelta);
            _AssertSeries(m2, "ts", 2010, 1d, sharedDelta);
            _AssertSeries(m2, "ts", 2011, 2d, sharedDelta);
            _AssertSeries(m2, "ts", 2012, double.NaN, sharedDelta);

        }
    
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------
        // --------------- Test Gekko 3.0 -------------------------------------------------
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------
        
        [TestMethod]
        public void _Test_Gekko20()
        {
            I("FOR string %i = a, b, c; END;");
            I("FOR val %v = 0 to 10 by 2; END;");
            I("FOR date %d = 2000 to 2010 by 2; END;");

        }

        [TestMethod]
        public void _Test_Conversions()
        {
            //Checks val(), date() and string() conversion functions (see Functions.val() etc).
            //TODO Shold do a check with other frequencies than annual

            I("RESET;");
            I("TIME 2000 2010;");
            I("CREATE ser;");
            I("SERIES ser = 2010;");
            I("VAL %v = 2010;");
            I("DATE %d = 2010a1;");
            I("STRING %s = '2010';");
            I("LIST #l = a, b, c;");

            I("STRING %s1 = string(%v);");
            _AssertScalarString(First(), "%s1", "2010");
            I("STRING %s2 = string(%d);");
            _AssertScalarString(First(), "%s2", "2010");            
            I("STRING %s3 = string(%s);");
            _AssertScalarString(First(), "%s3", "2010");
            I("STRING %s4 = string(#l);");
            _AssertScalarString(First(), "%s4", "a, b, c");
            FAIL("STRING %s5 = string(ser);");

            I("VAL %v1 = val(%v);");
            _AssertScalarVal(First(), "%v1", 2010);
            I("VAL %v2 = val(%d);");
            _AssertScalarVal(First(), "%v2", 2010);
            I("VAL %v3 = val(%s);");
            _AssertScalarVal(First(), "%v3", 2010);            
            FAIL("VAL %v4 = val(#l);");
            FAIL("VAL %v5 = val(ser);");

            I("DATE %d1 = date(%v);");
            _AssertScalarDate(First(), "%d1", EFreq.A, 2010, 1);            
            I("DATE %d2 = date(%d);");
            _AssertScalarDate(First(), "%d1", EFreq.A, 2010, 1);
            I("DATE %d3 = date(%s);");
            _AssertScalarDate(First(), "%d1", EFreq.A, 2010, 1);
            FAIL("DATE %d4 = date(#l);");
            FAIL("DATE %d5 = date(ser);");


        }

        [TestMethod]
        public void _Test_Lags()
        {
            //==================== .1 lags ===========================================
            Databank work = First();
            I("RESET;");
            I("time 2001 2005;");
            I("series x = (1, 2, 3, 4, 5);");
            I("prt x.1, x[-1];");
            I("xx1 = x.1; xx2 = x[-1];");
            _AssertSeries(First(), "xx1", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx1", 2005, 4d, sharedDelta);
            _AssertSeries(First(), "xx2", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2002, 1d, sharedDelta);
            _AssertSeries(First(), "xx2", 2005, 4d, sharedDelta);
            I("CREATE xx; SERIES xx = x.1 - x[-1];");
            _AssertSeries(First(), "xx", 2002, 0d, sharedDelta);

            I("CREATE yy; SERIES yy = 1*x.1 -1*x[-1];");            
            _AssertSeries(First(), "yy", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "yy", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "yy", 2002, 0d, sharedDelta);
            _AssertSeries(First(), "yy", 2003, 0d, sharedDelta);
            _AssertSeries(First(), "yy", 2004, 0d, sharedDelta);
            _AssertSeries(First(), "yy", 2005, 0d, sharedDelta);
            _AssertSeries(First(), "yy", 2006, double.NaN, sharedDelta);
        }

        [TestMethod]
        public void _Test_ModelMovavg()
        {
            Program.Flush(); //wipes out existing cached models

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("OPTION freq a;");
            I("MODEL movavg;");
            I("CREATE x; SER <2010 2013> x = (2, 3, 4, 5);");
            I("FOR val %i = 1 to 8; CREATE y{%i}; END;");
            I("SIM <2013 2013>;");
            _AssertSeries(First(), "y1", 2013, (4d + 5d) / 2d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, (3d + 4d + 5d) / 3d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, (3d + 4d) / 2d, sharedDelta);
            _AssertSeries(First(), "y4", 2013, (2d + 3d + 4d) / 3d, sharedDelta);
            _AssertSeries(First(), "y5", 2013, (4d + 5d) / 1d, sharedDelta);
            _AssertSeries(First(), "y6", 2013, (3d + 4d + 5d) / 1d, sharedDelta);
            _AssertSeries(First(), "y7", 2013, (3d + 4d) / 1d, sharedDelta);
            _AssertSeries(First(), "y8", 2013, (2d + 3d + 4d) / 1d, sharedDelta);
        }

        [TestMethod]
        public void _Test_Movavg()
        {            

            I("RESET; MODE data;");            
            I("OPTION freq a;");                        
            I("CREATE x; SER <2006 2013> x = (-.89, .33, -3.8, 4.32, 2, 3, 4, 5);");  //first 3 values are only used for some of the last tests in the following
            I("FOR val %i = 1 to 4; CREATE y{%i}; END;");
            I("TIME 2013 2013;");

            I("SERIES y1 = movavg(0.5 * x + 0.5 * x, 2);");
            I("SERIES y2 = movavg(0.5 * x + 0.5 * x, 3);");
            I("SERIES y3 = movavg(0.5 * x[-1] + 0.5 * x[-1], 2);");
            I("SERIES y4 = movavg(0.5 * x[-1] + 0.5 * x[-1], 3);");
            _AssertSeries(First(), "y1", 2013, (4d + 5d) / 2d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, (3d + 4d + 5d) / 3d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, (3d + 4d) / 2d, sharedDelta);
            _AssertSeries(First(), "y4", 2013, (2d + 3d + 4d) / 3d, sharedDelta);

            I("DELETE y1, y2, y3, y4;");
            I("SERIES y1 = movavg(x, 2);");
            I("SERIES y2 = movavg(x, 3);");
            I("SERIES y3 = movavg(x[-1], 2);");
            I("SERIES y4 = movavg(x[-1], 3);");
            _AssertSeries(First(), "y1", 2013, (4d + 5d) / 2d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, (3d + 4d + 5d) / 3d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, (3d + 4d) / 2d, sharedDelta);
            _AssertSeries(First(), "y4", 2013, (2d + 3d + 4d) / 3d, sharedDelta);

            I("DELETE y1, y2, y3, y4;");
            I("SERIES y1 = movsum(0.5 * x + 0.5 * x, 2);");
            I("SERIES y2 = movsum(0.5 * x + 0.5 * x, 3);");
            I("SERIES y3 = movsum(0.5 * x[-1] + 0.5 * x[-1], 2);");
            I("SERIES y4 = movsum(0.5 * x[-1] + 0.5 * x[-1], 3);");
            _AssertSeries(First(), "y1", 2013, (4d + 5d) / 1d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, (3d + 4d + 5d) / 1d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, (3d + 4d) / 1d, sharedDelta);
            _AssertSeries(First(), "y4", 2013, (2d + 3d + 4d) / 1d, sharedDelta);

            I("DELETE y1, y2, y3, y4;");
            I("SERIES y1 = movsum(x, 2);");
            I("SERIES y2 = movsum(x, 3);");
            I("SERIES y3 = movsum(x[-1], 2);");
            I("SERIES y4 = movsum(x[-1], 3);");
            _AssertSeries(First(), "y1", 2013, (4d + 5d) / 1d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, (3d + 4d + 5d) / 1d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, (3d + 4d) / 1d, sharedDelta);
            _AssertSeries(First(), "y4", 2013, (2d + 3d + 4d) / 1d, sharedDelta);

            I("DELETE y1, y2, y3, y4;");
            I("SERIES y1 = movsum(2*x-x,2);");
            I("SERIES y2 = movsum(movsum(2*x-x,2),2);");
            I("SERIES y3 = movsum(movsum(movsum(2*x-x,2),2),2);");            
            _AssertSeries(First(), "y1", 2013, 9d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, 16d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, 28d, sharedDelta);

            I("DELETE y1, y2, y3, y4;");
            I("SERIES y1 = movavg(2*x-x,2);");
            I("SERIES y2 = movavg(movavg(2*x-x,2),2);");
            I("SERIES y3 = movavg(movavg(movavg(2*x-x,2),2),2);");
            _AssertSeries(First(), "y1", 2013, 4.5d, sharedDelta);
            _AssertSeries(First(), "y2", 2013, 4d, sharedDelta);
            _AssertSeries(First(), "y3", 2013, 3.5d, sharedDelta);

            I("DELETE y1;");            
            I("SERIES y1 = dif(movavg(pch(2*x-x),2));");
            I("SERIES <2012 2013> y2 = movavg(movavg(movavg(2*x-x,4),3),2);");
            _AssertSeries(First(), "y1", 2013, -12.5d, sharedDelta);
            _AssertSeries(First(), "y2", 2012, 1.25083333333333, sharedDelta);
            _AssertSeries(First(), "y2", 2013, 2.2720833333333d, sharedDelta);

        }

        [TestMethod]
        public void Test__Pchy()
        {
            for (int i = 0; i < 3; i++)  //3 times to test with or without caching (third time just for extra safety)
            {
                if (i == 0) Program.Flush(); //wipes out existing cached models

                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
                I("OPTION freq a;");
                I("MODEL pchy;");
                I("CREATE x; SER <2010 2011> x = (200, 202);");
                I("FOR val %i = 1 to 10; CREATE y{%i}; SER <2010 2010> y{%i} = 100; END;");
                I("SIM <2011 2011>;");

                _AssertSeries(First(), "y1", 2011, 100d * ((0.5d * (202d / 200d - 1d) * 100d + 1d) / 100d + 1d), sharedDelta);
                _AssertSeries(First(), "y2", 2011, 0.5d * (202d - 200d) + 1d + 100d, sharedDelta);
                _AssertSeries(First(), "y3", 2011, 0.5d * (202d - 200d) + 1d + 100d, sharedDelta);
                _AssertSeries(First(), "y4", 2011, Math.Exp(0.5d * Math.Log(202d / 200d) + 1d + Math.Log(100d)), sharedDelta);

                _AssertSeries(First(), "y5", 2011, 100d * ((0.5d * (202d / 200d - 1d) * 100d + 1d) / 100d + 1d), sharedDelta);
                _AssertSeries(First(), "y6", 2011, 0.5d * (202d - 200d) + 1d + 100d, sharedDelta);
                _AssertSeries(First(), "y7", 2011, 0.5d * (202d - 200d) + 1d + 100d, sharedDelta);
                _AssertSeries(First(), "y8", 2011, Math.Exp(0.5d * Math.Log(202d / 200d) + 1d + Math.Log(100d)), sharedDelta);
                _AssertSeries(First(), "y9", 2011, 0.5d * 200d + 1d, sharedDelta);
                _AssertSeries(First(), "y10", 2011, 2d, sharedDelta);

                if (i == 0) Program.Flush(); //wipes out existing cached models

                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
                I("OPTION freq q;");
                I("MODEL pchy;");
                I("CREATE x; SER <2010q1 2011q1> x = (200, 1, 2, 3, 202);");
                I("FOR val %i = 1 to 10; CREATE y{%i}; SER <2010q4 2010q4> y{%i} = 7; SER <2010q1 2010q1> y{%i} = 100; END;");
                I("SIM <2011q1 2011q1>;");

                _AssertSeries(First(), "y1", EFreq.Q, 2011, 1, 100d * ((0.5d * (202d / 200d - 1d) * 100d + 1d) / 100d + 1d), sharedDelta);
                _AssertSeries(First(), "y2", EFreq.Q, 2011, 1, 0.5d * (202d - 200d) + 1d + 100d, sharedDelta);
                _AssertSeries(First(), "y3", EFreq.Q, 2011, 1, 0.5d * (202d - 200d) + 1d + 100d, sharedDelta);
                _AssertSeries(First(), "y4", EFreq.Q, 2011, 1, Math.Exp(0.5d * Math.Log(202d / 200d) + 1d + Math.Log(100d)), sharedDelta);

                _AssertSeries(First(), "y5", EFreq.Q, 2011, 1, 7d * ((0.5d * (202d / 3d - 1d) * 100d + 1d) / 100d + 1d), sharedDelta);
                _AssertSeries(First(), "y6", EFreq.Q, 2011, 1, 0.5d * (202d - 3d) + 1d + 7d, sharedDelta);
                _AssertSeries(First(), "y7", EFreq.Q, 2011, 1, 0.5d * (202d - 3d) + 1d + 7d, sharedDelta);
                _AssertSeries(First(), "y8", EFreq.Q, 2011, 1, Math.Exp(0.5d * Math.Log(202d / 3d) + 1d + Math.Log(7d)), sharedDelta);
                _AssertSeries(First(), "y9", EFreq.Q, 2011, 1, 0.5d * 3d + 1d, sharedDelta);
            }

            I("RESET; MODE data;");            
            I("OPTION freq q;");            
            I("CREATE x; SER <2010q1 2011q1> x = 200, 1, 2, 3, 202;");            
            I("SERIES <2011q1 2011q1> y1 = pchy(x);");
            I("SERIES <2011q1 2011q1> y2 = dify(x);");
            I("SERIES <2011q1 2011q1> y3 = diffy(x);");
            I("SERIES <2011q1 2011q1> y4 = dlogy(x);");
            I("SERIES <2011q1 2011q1> y5 = pch(x);");
            I("SERIES <2011q1 2011q1> y6 = dif(x);");
            I("SERIES <2011q1 2011q1> y7 = diff(x);");
            I("SERIES <2011q1 2011q1> y8 = dlog(x);");
            _AssertSeries(First(), "y1", EFreq.Q, 2011, 1,  (202d / 200d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "y2", EFreq.Q, 2011, 1,  (202d - 200d), sharedDelta);
            _AssertSeries(First(), "y3", EFreq.Q, 2011, 1,  (202d - 200d), sharedDelta);
            _AssertSeries(First(), "y4", EFreq.Q, 2011, 1,  Math.Log(202d / 200d), sharedDelta);

            //same with expression inside
            I("RESET; MODE data;");
            I("OPTION freq q;");
            I("CREATE x; SER <2010q1 2011q1> x = 200, 1, 2, 3, 202;");
            I("SERIES <2011q1 2011q1> y1 = pchy(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y2 = dify(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y3 = diffy(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y4 = dlogy(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y5 = pch(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y6 = dif(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y7 = diff(0.5 * x + 0.5 * x);");
            I("SERIES <2011q1 2011q1> y8 = dlog(0.5 * x + 0.5 * x);");
            _AssertSeries(First(), "y1", EFreq.Q, 2011, 1, (202d / 200d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "y2", EFreq.Q, 2011, 1, (202d - 200d), sharedDelta);
            _AssertSeries(First(), "y3", EFreq.Q, 2011, 1, (202d - 200d), sharedDelta);
            _AssertSeries(First(), "y4", EFreq.Q, 2011, 1, Math.Log(202d / 200d), sharedDelta);
            _AssertSeries(First(), "y5", EFreq.Q, 2011, 1, (202d / 3d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "y6", EFreq.Q, 2011, 1, (202d - 3d), sharedDelta);
            _AssertSeries(First(), "y7", EFreq.Q, 2011, 1, (202d - 3d), sharedDelta);
            _AssertSeries(First(), "y8", EFreq.Q, 2011, 1, Math.Log(202d / 3d), sharedDelta);

            //same with expression inside, with lag
            I("RESET; MODE data;");
            I("OPTION freq q;");
            I("CREATE x; SER <2009q4 2011q1> x = 200, 1, 2, 3, 202, 12345;");
            I("SERIES <2011q1 2011q1> y1 = pchy(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y2 = dify(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y3 = diffy(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y4 = dlogy(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y5 = pch(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y6 = dif(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y7 = diff(0.5 * x[-1] + 0.5 * x[-1]);");
            I("SERIES <2011q1 2011q1> y8 = dlog(0.5 * x[-1] + 0.5 * x[-1]);");
            _AssertSeries(First(), "y1", EFreq.Q, 2011, 1, (202d / 200d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "y2", EFreq.Q, 2011, 1, (202d - 200d), sharedDelta);
            _AssertSeries(First(), "y3", EFreq.Q, 2011, 1, (202d - 200d), sharedDelta);
            _AssertSeries(First(), "y4", EFreq.Q, 2011, 1, Math.Log(202d / 200d), sharedDelta);
            _AssertSeries(First(), "y5", EFreq.Q, 2011, 1, (202d / 3d - 1d) * 100d, sharedDelta);
            _AssertSeries(First(), "y6", EFreq.Q, 2011, 1, (202d - 3d), sharedDelta);
            _AssertSeries(First(), "y7", EFreq.Q, 2011, 1, (202d - 3d), sharedDelta);
            _AssertSeries(First(), "y8", EFreq.Q, 2011, 1, Math.Log(202d / 3d), sharedDelta);
        }

        [TestMethod]
        public void Test__ModelStatic()
        {
            //also checking CheckYesNoNull() logic
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("MODEL static;");
            I("CREATE #all;");
            I("TIME 2000 2003;");
            I("SERIES g = 100;");
            I("SERIES y = 500;");
            I("TIME 2001 2003;");
            I("WRITE sletmig;");
            I("READ sletmig;");
            I("SIM<static>;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 1750d, 1d);
            _AssertSeries(First(), "y", 2003, 1750d, 1d);
            //--------------------------
            I("READ sletmig;");
            I("SIM<static=yes>;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 1750d, 1d);
            _AssertSeries(First(), "y", 2003, 1750d, 1d);
            I("READ sletmig;");
            I("SIM<static=no>;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 4875d, 1d);
            _AssertSeries(First(), "y", 2003, 12687.5d, 1d);
            I("READ sletmig;");
            I("SIM;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 4875d, 1d);
            _AssertSeries(First(), "y", 2003, 12687.5d, 1d);
            //--------------------------
            I("OPTION solve static = yes;");
            I("READ sletmig;");
            I("SIM<static=yes>;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 1750d, 1d);
            _AssertSeries(First(), "y", 2003, 1750d, 1d);
            I("READ sletmig;");
            I("SIM<static=no>;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 4875d, 1d);
            _AssertSeries(First(), "y", 2003, 12687.5d, 1d);
            I("READ sletmig;");
            I("SIM;");
            _AssertSeries(First(), "y", 2001, 1750d, 1d);
            _AssertSeries(First(), "y", 2002, 1750d, 1d);
            _AssertSeries(First(), "y", 2003, 1750d, 1d);
        }

        [TestMethod]
        public void _Test_ModelFix()
        {
            //==================== .1 lags ===========================================
            I("RESET; MODE sim;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("MODEL jul05;");
            I("IMPORT<tsd>jul05; CLONE;");
            I("TIME 2006 2008;");
            I("SIM;");
            I("CLONE;");
            _AssertSeries(First(), "fy", 2006, 1407457d, 1d);  //1.11%
            _AssertSeries(First(), "fy", 2007, 1437479d, 1d);  //2.13%
            _AssertSeries(First(), "fy", 2008, 1456464d, 1d);  //1.32%
            I("EXO fy; ENDO tg;");
            I("SERIES fy %= 2;");
            I("SIM;");
            _AssertSeries(First(), "fy", 2006, 1407457d, 1d);  //no effect, same as above, since no SIM<fix> is used
            _AssertSeries(First(), "fy", 2007, 1437479d, 1d);
            _AssertSeries(First(), "fy", 2008, 1456464d, 1d);
            I("SERIES fy %= 2;");
            I("SIM<fix>;");
            _AssertSeries(First(), "fy", 2006, 1419830d, 1d);  //2% growth
            _AssertSeries(First(), "fy", 2007, 1448226d, 1d);
            _AssertSeries(First(), "fy", 2008, 1477191d, 1d);
            _AssertSeries(First(), "tg", 2006, 0.1974d, 0.0001d);
            _AssertSeries(First(), "tg", 2007, 0.2156d, 0.0001d);
            _AssertSeries(First(), "tg", 2008, 0.1831d, 0.0001d);
            I("WRITE sletmig;");
            I("READ sletmig;");
            I("SERIES boil %= 10;");  //was 2%
            I("SIM<fix>;");
            _AssertSeries(First(), "fy", 2006, 1419830d, 1d);  //2% growth
            _AssertSeries(First(), "fy", 2007, 1448226d, 1d);
            _AssertSeries(First(), "fy", 2008, 1477191d, 1d);
            I("PRT fy, tg, boil;");
            _AssertSeries(First(), "tg", 2006, 0.1956d, 0.0001d);  //lower to compensate for boil
            _AssertSeries(First(), "tg", 2007, 0.2109d, 0.0001d);
            _AssertSeries(First(), "tg", 2008, 0.1762d, 0.0001d);
            I("READ sletmig;");
            I("SERIES boil %= 10;");  //was 2%
            I("SIM;"); //goal on fY still active, but not used because of SIM, not SIM<fix>
            _AssertSeries(First(), "fy", 2006, 1419379d, 1d);  //1.97%
            _AssertSeries(First(), "fy", 2007, 1446965d, 1d);  //1.94%
            _AssertSeries(First(), "fy", 2008, 1475103d, 1d);  //1.94%
            _AssertSeries(First(), "tg", 2006, 0.1974d, 0.0001d);  //no change
            _AssertSeries(First(), "tg", 2007, 0.2156d, 0.0001d);
            _AssertSeries(First(), "tg", 2008, 0.1831d, 0.0001d);
            I("READ sletmig;");
            I("SERIES boil %= 10;");  //was 2%
            I("UNFIX;");
            I("SIM<fix>;"); //goal on fY removed, so SIM<fix> is like SIM
            _AssertSeries(First(), "fy", 2006, 1419379d, 1d);  //1.97%, same as above
            _AssertSeries(First(), "fy", 2007, 1446965d, 1d);  //1.94%
            _AssertSeries(First(), "fy", 2008, 1475103d, 1d);  //1.94%
            _AssertSeries(First(), "tg", 2006, 0.1974d, 0.0001d);  //no change
            _AssertSeries(First(), "tg", 2007, 0.2156d, 0.0001d);
            _AssertSeries(First(), "tg", 2008, 0.1831d, 0.0001d);

        }

        [TestMethod]
        public void _Test_Index()
        {
            //==================== INDEX ===========================================

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("index f* to #mylist;                                      //finds all series in Work and puts them in #mylist");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 631);                        
            I("index ref:f* to #mylist;                                 //same, for another bank");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 631);            
            I("index fx..fy to #mylist;                                //range");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 31);
            I("index ref:fx..ref:fy to #mylist;                                //range");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 31);

            if (Globals.UNITTESTFOLLOWUP_important)
            {
                //Now with {}-names.
                I("index {'f'}* to #mylist;                                      //finds all series in Work and puts them in #mylist");
                I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 631);
                I("index {'ref'}:{'f'}* to #mylist;                                 //same, for another bank");
                I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 631);
                I("index {'fx'}..{'fy'} to #mylist;                                //range");
                I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 31);
                I("index {'ref'}:{'fx'}..{'ref'}:{'fy'} to #mylist;                                //range");
                I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 31);
            }

            I("RESET;");
            I("TIME 2001 2003;");
            I("x1 = 1;");
            I("x2 = 2;");
            I("<2001q1 2004q4> x1!q = 1;");
            I("<2001q1 2004q4> x2!q = 2;");
            I("CLONE;");
            I("index {'*'} to #mylist;");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 2);
            I("index {'*:*'} to #mylist;");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 4);
            I("index {'*:#*'} to #mylist;");
            I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 1);  //the list is only in work            
            I("index <showbank=yes> {'*:*'} to #mylist;");  //default
            _AssertHelperList("mylist", new List<string>() { "x1", "x2", "Ref:x1", "Ref:x2" });
            I("index <showbank=no> {'*:*'} to #mylist;");
            _AssertHelperList("mylist", new List<string>() { "x1", "x2", "x1", "x2" });
            I("index <showbank=all> {'*:*'} to #mylist;");
            _AssertHelperList("mylist", new List<string>() { "Work:x1", "Work:x2", "Ref:x1", "Ref:x2" });
            I("index <showfreq=yes> {'*:*!*'} to #mylist;");  //default
            _AssertHelperList("mylist", new List<string>() { "x1", "x1!q", "x2", "x2!q", "Ref:x1", "Ref:x1!q", "Ref:x2", "Ref:x2!q" });
            I("index <showfreq=no> {'*:*!*'} to #mylist;");  //default
            _AssertHelperList("mylist", new List<string>() { "x1", "x1", "x2", "x2", "Ref:x1", "Ref:x1", "Ref:x2", "Ref:x2" });
            I("index <showfreq=all> {'*:*!*'} to #mylist;");  //default
            _AssertHelperList("mylist", new List<string>() { "x1!a", "x1!q", "x2!a", "x2!q", "Ref:x1!a", "Ref:x1!q", "Ref:x2!a", "Ref:x2!q" });
            I("index <showbank=all showfreq=all> {'*:*!*'} to #mylist;");  //default
            _AssertHelperList("mylist", new List<string>() { "Work:x1!a", "Work:x1!q", "Work:x2!a", "Work:x2!q", "Ref:x1!a", "Ref:x1!q", "Ref:x2!a", "Ref:x2!q" });
            
            if (Globals.UNITTESTFOLLOWUP_important)
            {
                I("index {'**'} to #mylist;");
                I("VAL %n = #mylist.len();"); _AssertScalarVal(First(), "%n", 3);  //2 series + the list in work
            }
        }


        [TestMethod]
        public void _Test_Splice()
        {

            Databank work = First();
            //==================== SPLICE ===========================================

            if (false)
            {
                //Maybe also an option to splice where the first series is the anchor

                I("RESET;");
                I("create ts1, ts2, ts0a, ts0b;");
                I("SERIES <2002 2006> ts1 = (2, 3, 4, 5, 6);");
                I("SERIES <2004 2010> ts2 = (41, 42, 43, 44, 45, 46, 46);");
                I("splice ts0a = ts1 ts2;                                    //splicing two series by means of three common observations");                
                double delta = 0.0001d;
                _AssertSeries(First(), "ts0a", 2006, 6d, delta);
                _AssertSeries(First(), "ts0a", 2007, 5.2381d, delta);
                _AssertSeries(First(), "ts0a", 2008, 5.3571d, delta);
                _AssertSeries(First(), "ts0a", 2009, 5.4762d, delta);
                _AssertSeries(First(), "ts0a", 2010, 5.5952d, delta);
                I("splice ts0b = ts1 2006 ts2;                               //splicing on one observation instead, follows ts2 growth from 2007 and on.");                
                _AssertSeries(First(), "ts0b", 2006, 6d, delta);
                _AssertSeries(First(), "ts0b", 2007, 6.1395d, delta);
                _AssertSeries(First(), "ts0b", 2008, 6.2791d, delta);
                _AssertSeries(First(), "ts0b", 2009, 6.4186d, delta);
                _AssertSeries(First(), "ts0b", 2010, 6.5581d, delta);
            }


            // AREMOS RESULTS:

            //ser < 2002 2006 > ts1 = 2,3,4,5,6;
            //ser < 2004 2010 > ts2 = 41,42,43,44,45,46,46;  //fail, last one should be 47... never mind
            //set per 2000 2020;
            //splice ts0a = ts1 ts2;
            //splice ts0b = ts1 2006 ts2;
            //print ts1, ts2, ts0a, ts0b;

            //2002      2.000000               16.800000   14.333333
            //2003      3.000000               25.200000   21.500000
            //2004      4.000000   41.000000   41.000000   28.666667
            //2005      5.000000   42.000000   42.000000   35.833333
            //2006      6.000000   43.000000   43.000000   43.000000
            //2007                 44.000000   44.000000   44.000000
            //2008                 45.000000   45.000000   45.000000
            //2009                 46.000000   46.000000   46.000000
            //2010                 46.000000   46.000000   46.000000

            //          pch(ts1)    pch(ts2)   pch(ts0a)   pch(ts0b)
            //2003     50.000000               50.000000   50.000000
            //2004     33.333333               62.698413   33.333333
            //2005     25.000000    2.439024    2.439024   25.000000
            //2006     20.000000    2.380952    2.380952   20.000000
            //2007                  2.325581    2.325581    2.325581
            //2008                  2.272727    2.272727    2.272727
            //2009                  2.222222    2.222222    2.222222
            //2010                  0.000000    0.000000    0.000000

            if (true)
            {
                I("RESET;");
                I("create ts1, ts2, ts0a, ts0b;");
                I("SERIES <2002 2006> ts1 = (2, 3, 4, 5, 6);");
                I("SERIES <2004 2010> ts2 = (41, 42, 43, 44, 45, 46, 46);");
                I("splice ts0a = ts1 ts2;                                    //splicing two series by means of three common observations");
                double delta = 0.0001d;
                _AssertSeries(First(), "ts0a", 2001, double.NaN, delta);
                _AssertSeries(First(), "ts0a", 2002, 16.8d, delta);
                _AssertSeries(First(), "ts0a", 2003, 25.2d, delta);
                _AssertSeries(First(), "ts0a", 2004, 41d, delta);
                _AssertSeries(First(), "ts0a", 2005, 42d, delta);
                _AssertSeries(First(), "ts0a", 2006, 43d, delta);
                _AssertSeries(First(), "ts0a", 2007, 44d, delta);
                _AssertSeries(First(), "ts0a", 2008, 45d, delta);
                _AssertSeries(First(), "ts0a", 2009, 46d, delta);
                _AssertSeries(First(), "ts0a", 2010, 46d, delta);
                _AssertSeries(First(), "ts0a", 2011, double.NaN, delta);
                I("splice work:ts0b = work:ts1 2006 work:ts2;                               //splicing on one observation instead, follows ts2 growth from 2007 and on.");
                _AssertSeries(First(), "ts0b", 2001, double.NaN, delta);
                _AssertSeries(First(), "ts0b", 2002, 14.3333333d, delta);
                _AssertSeries(First(), "ts0b", 2003, 21.5d, delta);
                _AssertSeries(First(), "ts0b", 2004, 28.6666667d, delta);
                _AssertSeries(First(), "ts0b", 2005, 35.8333333d, delta);
                _AssertSeries(First(), "ts0b", 2006, 43d, delta);
                _AssertSeries(First(), "ts0b", 2007, 44d, delta);
                _AssertSeries(First(), "ts0b", 2008, 45d, delta);
                _AssertSeries(First(), "ts0b", 2009, 46d, delta);
                _AssertSeries(First(), "ts0b", 2010, 46d, delta);
                _AssertSeries(First(), "ts0b", 2011, double.NaN, delta);
                _AssertSeries(First(), "ts0b", 2011, double.NaN, delta);
            }

        }

        [TestMethod]
        public void _Test_AREMOS_ras()
        {
            Databank work = First();
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\AREMOS\Ras';");
            I("RUN regres;");
            CheckFullDatabank(0.0001, 0.01, 2010, 2013);  //must be < 0.0001 abs or < 0.01%. Quite strict.
        }

        [TestMethod]
        public void _Test_Analyze()
        {
            //See details on this example in the analyze code            
            I("reset;");
            I("time 2001 2010;");
            I("create x1,x2,x3;");
            I("ser x1 = (1,2,4,5,6,5,6,7,8,6);");
            I("ser x2 = (5,4,4,5,6,3,6,1,8,6);");
            I("ser x3 = (7,5,4,1,6,5,9,7,8,9);");
            I("analyze (x1, x2, x3);");
            double d = 0.00000001;  //very precise!
            _AssertMatrix(First(), "#corr", 1, 1, 1d, d);
            _AssertMatrix(First(), "#corr", 1, 2, 0.21295885d, d);
            _AssertMatrix(First(), "#corr", 1, 3, 0.31237800d, d);
            _AssertMatrix(First(), "#corr", 2, 1, 0.21295885d, d);
            _AssertMatrix(First(), "#corr", 2, 2, 1d, d);
            _AssertMatrix(First(), "#corr", 2, 3, 0.30733932d, d);
            _AssertMatrix(First(), "#corr", 3, 1, 0.31237800d, d);
            _AssertMatrix(First(), "#corr", 3, 2, 0.30733932d, d);
            _AssertMatrix(First(), "#corr", 3, 3, 1d, d);
                        
            I("reset;");
            I("option freq q;");
            I("time 2001q1 2003q2;");
            I("create x1,x2,x3;");
            I("ser x1 = (1,2,4,5,6,5,6,7,8,6);");
            I("ser x2 = (5,4,4,5,6,3,6,1,8,6);");
            I("ser x3 = (7,5,4,1,6,5,9,7,8,9);");
            I("analyze (x1, x2, x3);");            
            _AssertMatrix(First(), "#corr", 1, 1, 1d, d);
            _AssertMatrix(First(), "#corr", 1, 2, 0.21295885d, d);
            _AssertMatrix(First(), "#corr", 1, 3, 0.31237800d, d);
            _AssertMatrix(First(), "#corr", 2, 1, 0.21295885d, d);
            _AssertMatrix(First(), "#corr", 2, 2, 1d, d);
            _AssertMatrix(First(), "#corr", 2, 3, 0.30733932d, d);
            _AssertMatrix(First(), "#corr", 3, 1, 0.31237800d, d);
            _AssertMatrix(First(), "#corr", 3, 2, 0.30733932d, d);
            _AssertMatrix(First(), "#corr", 3, 3, 1d, d);
        }

           

        [TestMethod]
        public void _Test_Statistikbanken()
        {
            Databank work = First();
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");            
            I("DOWNLOAD 'http://api.statbank.dk/v1/data' statbank.json;");
            _AssertSeries(First(), "pris6_VAREGR_011200_enhed_100!m", EFreq.M, 2000, 1, 98.1d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGR_011100_enhed_100!m", EFreq.M, 2000, 1, 98.3d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGR_011200_enhed_100!m", EFreq.M, 2001, 3, 102.9d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGR_011100_enhed_100!m", EFreq.M, 2001, 3, 103.1d, sharedDelta);

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");            
            I("DOWNLOAD 'http://api.statbank.dk/v1/data' statbank.json;");
            _AssertSeries(First(), "pris6_VAREGR_011200_enhed_100!m", EFreq.M, 2000, 1, 98.1d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGR_011100_enhed_100!m", EFreq.M, 2000, 1, 98.3d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGR_011200_enhed_100!m", EFreq.M, 2001, 3, 102.9d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGR_011100_enhed_100!m", EFreq.M, 2001, 3, 103.1d, sharedDelta);


        }

        [TestMethod]
        public void _Test_Smooth()
        {

            Databank work = First();
            //==================== SMOOTH ===========================================

            //TODO: Test the others
            //TODO: Test the others
            //TODO: Test the others

            I("RESET;");
            I("create ts, ts1, ts2, ts3, ts4;");
            I("SERIES <2002 2004> ts = (2, 3, 4);");
            I("SERIES <2008 2010> ts = (9, 8, 7);"); 
            I("SERIES <2004 2008> tsb = (-1, -2, -3, -4, -5);");
            I("smooth ts1 = ts spline;                                  //fill holes with cubic spline");
            I("smooth work:ts2 = ts linear;                                  //fill holes with linear interpolation");
            I("smooth ts3 = work:ts geometric;                               //fill holes with geometric interpolation");
            I("smooth work:ts4 = work:ts repeat;                                  //fill holes with last known value.");
            I("smooth work:ts5 = work:ts overlay work:tsb;                              //fill holes with other series.");

            _AssertSeries(First(), "ts4", 2004, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", 2005, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", 2006, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", 2007, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", 2008, 9d, sharedDelta);

            _AssertSeries(First(), "ts5", 2004, 4d, sharedDelta);
            _AssertSeries(First(), "ts5", 2005, -2d, sharedDelta);
            _AssertSeries(First(), "ts5", 2006, -3d, sharedDelta);
            _AssertSeries(First(), "ts5", 2007, -4d, sharedDelta);
            _AssertSeries(First(), "ts5", 2008, 9d, sharedDelta);

            I("prt <2002 2010> ts, ts1, ts2, ts3, ts4, ts5;");
        }

        [TestMethod]
        public void _Test_Truncate()
        {
            //==================== TRUNCATE ===========================================            
            I("RESET;");
            I("option freq a;");
            I("create ts4;");
            I("SERIES <2003 2009> ts4 = (1, 1, 2, 3, 4, 5, 5);");
            I("truncate <2005 2007> ts4;");
            _AssertSeries(First(), "ts4", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", 2004, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", 2005, 2d, sharedDelta);
            _AssertSeries(First(), "ts4", 2006, 3d, sharedDelta);
            _AssertSeries(First(), "ts4", 2007, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", 2008, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", 2009, double.NaN, sharedDelta);                       
            
            //quarters
            I("RESET;");
            I("option freq q;");
            I("create ts4;");
            I("SERIES <2000q1 2001q3> ts4 = (1, 1, 2, 3, 4, 5, 5);");
            I("truncate <2000q3 2001q1> ts4;");
            _AssertSeries(First(), "ts4", EFreq.Q, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.Q, 2000, 2, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.Q, 2000, 3, 2d, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.Q, 2000, 4, 3d, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.Q, 2001, 1, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.Q, 2002, 2, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.Q, 2003, 3, double.NaN, sharedDelta);

            //months
            I("RESET;");
            I("option freq m;");
            I("create ts4;");
            I("SERIES <2000m1 2000m7> ts4 = (1, 1, 2, 3, 4, 5, 5);");
            I("truncate <2000m3 2000m5> work:ts4;");  //also testing bank
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 2, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 3, 2d, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 4, 3d, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 5, 4d, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 6, double.NaN, sharedDelta);
            _AssertSeries(First(), "ts4", EFreq.M, 2000, 7, double.NaN, sharedDelta);

            
        }



        [TestMethod]
        public void _Test_ArraySeries2()
        {
            Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
            Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
            I("RESET; MODE data; TIME 2000;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");

            I("SER x = series(3);");
            I("SER x['a', 'b', 'c'] = 3;");
            _AssertSeries(First(), "x", new string[] { "a", "b", "c" }, 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a", "b", "c" }, 2000, 3, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a", "b", "c" }, 2001, double.NaN, sharedDelta);

            I("SER y = series(2);");
            I("y['d', 'e'] = x['a', 'b', 'c'];");
            _AssertSeries(First(), "y", new string[] { "d", "e" }, 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "y", new string[] { "d", "e" }, 2000, 3, sharedDelta);
            _AssertSeries(First(), "y", new string[] { "d", "e" }, 2001, double.NaN, sharedDelta);

            I("SERIES z = 100;"); //normal

            I("WRITE arrayts;");

           
            // ------ test of read and write

            I("RESET; MODE data; TIME 2000;");
            I("READ arrayts;");
            //FIXME, should report 3
            Assert.AreEqual(First().storage.Count, 3);
            _AssertSeries(First(), "x", new string[] { "a", "b", "c" }, 2000, 3, sharedDelta);
            _AssertSeries(First(), "y", new string[] { "d", "e" }, 2000, 3, sharedDelta);
            _AssertSeries(First(), "z", 2000, 100, sharedDelta);
                       

            // --------------------------------
            // looping with lists
            // --------------------------------

            I("RESET; MODE data; TIME 2000;");
            I("LIST #i = a, b;");
            I("LIST #j = x, y;");
            I("y = series(2);");            
            I("y['a', 'x'] = 100;");
            I("y['a', 'y'] = 101;");
            I("y['b', 'x'] = 102;");
            I("y['b', 'y'] = 103;");
            I("z = series(1);");
            I("z['x'] = 1000;");
            I("z['y'] = 1001;");
            I("x = series(2);");
            I("x[#i, #j] = 1 + y[#i, #j] + z[#j];");

            _AssertSeries(First(), "x", new string[] { "a", "x" }, 2000, 1101, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a", "y" }, 2000, 1103, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "x" }, 2000, 1103, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "y" }, 2000, 1105, sharedDelta);

            

            // --------------------------------
            // looping with lists and dollar
            // --------------------------------

            I("RESET; MODE data; TIME 2000;");
            I("option series array calc missing = m;");  //necessary, else it crashes, #784325345435
            I("LIST #i = a, b, c;");
            I("LIST #i0 = a, b;");
            I("LIST #j = x, y;");
            I("y = series(2); z = series(1); x = series(2);");
            I("y['a', 'x'] = 100;");
            I("y['a', 'y'] = 101;");
            I("y['b', 'x'] = 102;");
            I("y['b', 'y'] = 103;");
            I("z['x'] = 1000;");
            I("z['y'] = 1001;");
            I("x[#i, #j] = 1 + y[#i, #j] $ (#i in #i0) + z[#j];");
            //I("x[#i, #j] = 1 + y[#i, #j] $ (#i0[#i]) + z[#j];");
            _AssertSeries(First(), "x", new string[] { "a", "x" }, 2000, 1101d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a", "y" }, 2000, 1103d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "x" }, 2000, 1103d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "y" }, 2000, 1105d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "c", "x" }, 2000, 1001d, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "c", "y" }, 2000, 1002d, sharedDelta);
            I("SER sum = sum(#i0, sum(#j, y[#i0, #j]));");
            _AssertSeries(First(), "sum", EFreq.A, 2000, 1, 406d, sharedDelta);
            //TODO: I("SER sum = sum(#i $ #i0[#i], sum(#j, y[#i0, #j]));");
            //TODO: I("ASER x2[#i $ #i[#i0]] = sum(#j, y[#i, #j]);");
            I("x2 = series(1);");
            I("SER x2[#i0] = sum(#j, y[#i0, #j]);");
            _AssertSeries(First(), "x2", new string[] { "a" }, 2000, 201d, sharedDelta);
            _AssertSeries(First(), "x2", new string[] { "b" }, 2000, 205d, sharedDelta);
            I("SER x2[#i0] = sum(#j, y[#i0, #j]) + sum(#j, y[#i0, #j]) + 10;");
            _AssertSeries(First(), "x2", new string[] { "a" }, 2000, 2 * 201d + 10d, sharedDelta);
            _AssertSeries(First(), "x2", new string[] { "b" }, 2000, 2 * 205d + 10d, sharedDelta);
            
            I("LIST #i0 = list('a');");
            //I("SER x[#i, #j] = 1 + y[#i, #j] $ (#i0[#i]) + z[#j];");
            I("SER x[#i, #j] = 1 + y[#i, #j] $ (#i in #i0) + z[#j];");
            _AssertSeries(First(), "x", new string[] { "a", "x" }, 2000, 1101, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a", "y" }, 2000, 1103, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "x" }, 2000, 1001, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "y" }, 2000, 1002, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "c", "x" }, 2000, 1001, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "c", "y" }, 2000, 1002, sharedDelta);
                   
            I("LIST #i0 = list();");
            //I("SER x[#i, #j] = 1 + y[#i, #j] $ (#i0[#i]) + z[#j];");
            I("SER x[#i, #j] = 1 + y[#i, #j] $ (#i in #i0) + z[#j];");
            _AssertSeries(First(), "x", new string[] { "a", "x" }, 2000, 1001, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "a", "y" }, 2000, 1002, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "x" }, 2000, 1001, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "b", "y" }, 2000, 1002, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "c", "x" }, 2000, 1001, sharedDelta);
            _AssertSeries(First(), "x", new string[] { "c", "y" }, 2000, 1002, sharedDelta);
            
            // ---------- ignore missing cells ------------------------

            I("RESET; MODE data; TIME 2000;");
            I("LIST #i = a, b, c, d;");
            I("y = series(1);");
            I("y['a'] = 100;");
            I("y['b'] = 101;");
            I("y['c'] = 102;");
            FAIL("SER z = sum(#i, y[#i]);");
            FAIL("SER z = y['d']);");            
            I("OPTION series array calc missing = zero; ");
            I("z = series(1);");
            I("SER z['1'] = sum(#i, y[#i]);");
            _AssertSeries(First(), "z", new string[] { "1" }, 2000, 303d, sharedDelta);
            I("SER z['2'] = y['d'];");
            _AssertSeries(First(), "z", new string[] { "2" }, 2000, 0d, sharedDelta);

            if (Globals.UNITTESTFOLLOWUP)
            {
                // --------------------------------
                // fun with loop
                // --------------------------------
                I("RESET; MODE data; TIME 2000;");
                I("LIST #i = a, b, c;");
                I("SER aa = 1; SER bb = 2; SER cc = 3;");
                I("xx = series(1);");
                I("SER xx[#i] = {#i + #i};");  //#0875234598
                _AssertSeries(First(), "xx", new string[] { "a" }, 2000, 1, sharedDelta);
                _AssertSeries(First(), "xx", new string[] { "b" }, 2000, 2, sharedDelta);
                _AssertSeries(First(), "xx", new string[] { "c" }, 2000, 3, sharedDelta);
            }
                       

            // -----------------------------------------
            // DOLLAR CONDITIONAL TESTING
            // -----------------------------------------

            //Switch on timeseries
            I("RESET; MODE data;");
            I("TIME 2001 2005;");
            I("SERIES x = (10, 10, 11, 12, 10);");
            I("SERIES y = (x + 100) $ (x + 10 == 20);");
            _AssertSeries(First(), "y", 2001, 110d, sharedDelta);
            _AssertSeries(First(), "y", 2002, 110d, sharedDelta);
            _AssertSeries(First(), "y", 2003, 0d, sharedDelta);
            _AssertSeries(First(), "y", 2004, 0d, sharedDelta);
            _AssertSeries(First(), "y", 2005, 110d, sharedDelta);
                       

            //Switch on timeseries, testing of different types (VAL vs. SERIES)
            I("RESET; MODE data;");
            I("TIME 2000 2000;");
            I("SERIES x = 10;");
            I("SERIES x2 = 10;");
            I("SERIES y1 = (x + 100) $ (x < 10);");
            _AssertSeries(First(), "y1", 2000, 0d, sharedDelta);
            I("SERIES y2 = (x + 100) $ (x <= 10);");
            _AssertSeries(First(), "y2", 2000, 110d, sharedDelta);
            I("SERIES y3 = (x + 100) $ (x == 10);");
            _AssertSeries(First(), "y3", 2000, 110d, sharedDelta);
            I("SERIES y4 = (x + 100) $ (x >= 10);");
            _AssertSeries(First(), "y4", 2000, 110d, sharedDelta);
            I("SERIES y5 = (x + 100) $ (x > 10);");
            _AssertSeries(First(), "y5", 2000, 0d, sharedDelta);
            I("SERIES y11 = (x + 100) $ (10 > x);");
            _AssertSeries(First(), "y11", 2000, 0d, sharedDelta);
            I("SERIES y12 = (x + 100) $ (10 >= x);");
            _AssertSeries(First(), "y12", 2000, 110d, sharedDelta);
            I("SERIES y13 = (x + 100) $ (10 == x);");
            _AssertSeries(First(), "y13", 2000, 110d, sharedDelta);
            I("SERIES y14 = (x + 100) $ (10 <= x);");
            _AssertSeries(First(), "y14", 2000, 110d, sharedDelta);
            I("SERIES y15 = (x + 100) $ (10 < x);");
            _AssertSeries(First(), "y15", 2000, 0d, sharedDelta);
            I("SERIES y3 = (x + 100) $ (x == x2);");
            _AssertSeries(First(), "y3", 2000, 110d, sharedDelta);
            I("SERIES y3 = (x + 100) $ (x <> x2);");
            _AssertSeries(First(), "y3", 2000, 0d, sharedDelta);

            

            //Switching between two values depending upon same criterion 
            I("RESET; MODE data;");
            I("TIME 2001 2005;");
            I("SERIES x = (10, 10, 11, 12, 10);");
            I("SERIES y1 = 110 $ (x + 10 == 20) + 111 $ (x + 10 <> 20);");
            //This is equivalent, using iif()
            I("SERIES y2 = iif(x + 10, '==', 20, 110, 111);");
            _AssertSeries(First(), "y1", 2001, 110d, sharedDelta);
            _AssertSeries(First(), "y1", 2002, 110d, sharedDelta);
            _AssertSeries(First(), "y1", 2003, 111d, sharedDelta);
            _AssertSeries(First(), "y1", 2004, 111d, sharedDelta);
            _AssertSeries(First(), "y1", 2005, 110d, sharedDelta);
            _AssertSeries(First(), "y2", 2001, 110d, sharedDelta);
            _AssertSeries(First(), "y2", 2002, 110d, sharedDelta);
            _AssertSeries(First(), "y2", 2003, 111d, sharedDelta);
            _AssertSeries(First(), "y2", 2004, 111d, sharedDelta);
            _AssertSeries(First(), "y2", 2005, 110d, sharedDelta);

            

            I("RESET; MODE data;");
            I("TIME 2000 2000;");
            I("VAL %v = 10;");
            I("LIST #m = a, b;");
            I("SERIES y = 2;");
            I("SERIES y = 3 $ (%v == 9);");
            _AssertSeries(First(), "y", 2000, 0d, sharedDelta);
            I("SERIES y = 2;");
            I("SERIES y = 3 $ (%v == 10);");
            _AssertSeries(First(), "y", 2000, 3d, sharedDelta);
            I("SERIES y = 2;");
            //I("SERIES y = 3 $ (#m['c']);");
            I("SERIES y = 3 $ ('c' in #m);");
            _AssertSeries(First(), "y", 2000, 0d, sharedDelta);
            I("SERIES y = 2;");
            //I("SERIES y = 3 $ (#m['b']);");
            I("SERIES y = 3 $ ('b' in #m);");
            _AssertSeries(First(), "y", 2000, 3d, sharedDelta);
            I("SERIES y = 2;");
            I("SERIES y = 3 $ ('b' in #m and %v == 10);");
            _AssertSeries(First(), "y", 2000, 3d, sharedDelta);
            I("SERIES y = 2;");
            I("SERIES y = 3 $ ('b' in #m and %v == 9);");
            _AssertSeries(First(), "y", 2000, 0d, sharedDelta);
            I("SERIES y = 2;");
            I("SERIES y = 3 $ ('c' in #m and %v == 10);");
            _AssertSeries(First(), "y", 2000, 0d, sharedDelta);
            I("SERIES y = 2;");
            I("SERIES y = 3 $ ('c' in #m or %v == 10);");
            _AssertSeries(First(), "y", 2000, 3d, sharedDelta);

          

            //Without parenthesis (...) for lists
            I("SERIES y = 2;");
            //I("SERIES y = 3 $ (#m['a']);");
            I("SERIES y = 3 $ ('a' in #m);");
            _AssertSeries(First(), "y", 2000, 3d, sharedDelta);
            I("SERIES y = 2;");
            I("SERIES y = 3 $ ('c' in #m);");
            _AssertSeries(First(), "y", 2000, 0d, sharedDelta);

            //PRT/PLOT
            I("mode data;");
            I("x = series(2);");
            I("ser x['y', 'a'] = 5;");
            I("ser x['y', 'b'] = 6;");
            I("ser x['z', 'a'] = 7;");
            I("ser x['z', 'b'] = 8;");
            I("list #m1 = a, b;");
            I("list #m2 = y, z;");
            I("prt x[#m2, #m1];");
            I("prt x['y', #m1];");
            I("prt x[#m2, 'a'];");
            I("prt x['y', 'a'];");


            //MULPRT with x[#a, #i]
            I("reset; OPTION databank create auto = yes;");
            I("time 2010 2012;");
            I("x = series(2);");
            I("y = series(2);");
            I("ser x['a', 'i'] = 1;");
            I("ser x['a', 'j'] = 2;");
            I("ser x['b', 'i'] = 3;");
            I("ser x['b', 'j'] = 4;");
            I("ser y['a', 'i'] = 5;");
            I("ser y['a', 'j'] = 6;");
            I("ser y['b', 'i'] = 7;");
            I("ser y['b', 'j'] = 8;");
            I("clone;");
            I("ser x['a', 'i'] = 11;");
            I("ser x['a', 'j'] = 12;");
            I("ser x['b', 'i'] = 13;");
            I("ser x['b', 'j'] = 14;");
            I("ser y['a', 'i'] = 15;");
            I("ser y['a', 'j'] = 16;");
            I("ser y['b', 'i'] = 17;");
            I("ser y['b', 'j'] = 18;");
            I("list #a = a, b;");
            I("list #i = i, j;");
            I("for string %ts = x, y; mulprt {%ts}[#a, #i]; end;");  //should be tested, yields 10 abs difference for each
            
        }

    [TestMethod]
        public void Test__ReadGdx()
        {

            // -----------------------------------------
            // Reading from a gdx file and validating
            // a series statement from Gams model.
            // -----------------------------------------

            I("reset; mode data;");
            //!!!!!!!!!!! FIXME
            //!!!!!!!!!!! FIXME
            //!!!!!!!!!!! FIXME last \
            //!!!!!!!!!!! FIXME
            //!!!!!!!!!!! FIXME
            I("OPTION gams exe folder = 'c:\\GAMS\\win32\\24.8\\';");
            I("OPTION gams time set = 't';");
            I("OPTION gams time prefix = 't';");
            I("OPTION gams time offset = 2006;");
            I("OPTION gams time detect auto = yes;");  //probably not necessary here
            I("read <gdx> c:\\tools\\decomp\\report.gdx;"); //This reads from gdx file, note that 2006 is added to t{i}, for instance t0=2006, t1=2007...            
            //I("list scn = base;");
            //I("list psl = chou, cpub, ccon, cgoo, cser;");
            I("clone;");
            I("delete m;");
            I("aseries M[#psl,#scn] = myFM[#psl,#scn] * F[#psl,#scn] * ((PM[#psl,#scn]/PFF[#psl,#scn])*(PM[#psl,#scn]/PFF[#psl,#scn]))**(-EF[#psl]/2);");
            _AssertSeries(First(), "m", new string[] { "chou", "base" }, 2006, 32.1879605531923d, sharedDelta);
            _AssertSeries(Ref(), "m", new string[] { "chou", "base" }, 2006, 32.1879605531923d, sharedDelta);

            //Cutting off a dimension
            I("reset; mode data;");
            I("OPTION gams exe folder = 'c:\\GAMS\\win32\\24.8\\';");
            I("OPTION gams time set = 't';");
            I("OPTION gams time prefix = 't';");
            I("OPTION gams time offset = 2006;");
            I("OPTION gams time detect auto = yes;");  //probably not necessary here
            I("read <gdx gdxopt='scns.base'> c:\\tools\\decomp\\report.gdx;"); //This reads from gdx file, note that 2006 is added to t{i}, for instance t0=2006, t1=2007...                        
            //I("list psl = chou, cpub, ccon, cgoo, cser;");
            I("clone;");
            I("delete m;");
            I("aseries M[#psl] = myFM[#psl] * F[#psl] * ((PM[#psl]/PFF[#psl])*(PM[#psl]/PFF[#psl]))**(-EF[#psl]/2);");
            _AssertSeries(First(), "m", new string[] { "chou" }, 2006, 32.1879605531923d, sharedDelta);
            _AssertSeries(Ref(), "m", new string[] { "chou" }, 2006, 32.1879605531923d, sharedDelta);

            //comparing scenarios            
            I("reset;");
            I("OPTION gams exe folder = 'c:\\GAMS\\win32\\24.8\\';");
            I("OPTION gams time set = 't';");
            I("OPTION gams time prefix = 't';");
            I("OPTION gams time offset = 2006;");
            I("OPTION gams time detect auto = yes;");  //probably not necessary here
            I("read <gdx gdxopt='scns.base'> c:\\tools\\decomp\\report.gdx;"); //This reads from gdx file, note that 2006 is added to t{i}, for instance t0=2006, t1=2007...                        
            I("write base;");
            I("reset;");
            I("OPTION gams exe folder = 'c:\\GAMS\\win32\\24.8\\';");
            I("OPTION gams time set = 't';");
            I("OPTION gams time prefix = 't';");
            I("OPTION gams time offset = 2006;");
            I("OPTION gams time detect auto = yes;");  //probably not necessary here
            I("read <gdx gdxopt='scns.struc'> c:\\tools\\decomp\\report.gdx;"); //This reads from gdx file, note that 2006 is added to t{i}, for instance t0=2006, t1=2007...                        
            I("write struc;");
            I("reset;");
            I("read <first> struc;");
            I("read <ref> base;");
            I("mulprt m['chou'];");
            I("PRT m['chou'] - @m['chou'];");
            I("PRT work:m['chou'] - ref:m['chou'];");
            _AssertSeries(First(), "m", new string[] { "chou" }, 2006 + 27, 35.0258600515369d, sharedDelta);
            _AssertSeries(Ref(), "m", new string[] { "chou" }, 2006 + 27, 35.0289043466536d, sharedDelta);

            I("reset; mode data;");
            I("OPTION gams exe folder = 'c:\\GAMS\\win32\\24.8\\';");            
            I("read <gdx> c:\\tools\\decomp\\calib.gdx;");            
            _AssertSeries(First(), "adam_ib", new string[] { "iba" }, 1990, 3147.56d, sharedDelta);
            _AssertSeries(First(), "qc_a_y", new string[] { "52", "ccon" }, 2047, 0.000865742646856205d, sharedDelta);

        }
       



        [TestMethod]
        public void _Test_BankSyntaxLogic()
        {
            //COPY: (the following should work for LIST command, too, excluding the stand-alone wildcards a*b and a..b)
            //------------------------------
            //BANK:  "", "@", "ref:", "{%s}:"
            //TS  :  "a", {%s}, #m, a*b, a..b, [a*b], [a..b]          -----> what about (a, b, c) instead of #m??? LATER ON!
            //INDEX: "[a..b]", "[%i..%i+1]"
            //In addition: COPY %s, where %s = 'a' or %s = 'ref:a'

            //PRT
            //------------------------------
            //BANK:  "", "@", "ref:", "{%s}:"
            //TS:    "a", {%s}, #m, [a*b], [a..b]
            //INDEX: "[a..b]", "[%i..%i+1]"

            //LIST: check that a,b,c   or    'a','b','c'         etc are ok.
            //      check that ref:a,ref:b,ref:c   or    'ref:a','ref:b','ref:c'         etc are ok.
            //      check that {%b}:{%a}...

            //It is also possible to use scalar strings, for instance PRT %b:%ts

            I("RESET; mode data;");
            //I("CREATE fa, fb, fc;");
            I("#m = fa, fb, fc;");
            I("{#m} = 1;");
            I("CLONE;");
            I("list #m = fa,fc;");
            I("string %s = 'fb';");
            I("string %b = 'ref';");
            I("string %ba = 'ref:fa';");


            //================================ PRT start ==========================================
            //----------------------------
            I("PRT fa;");
            I("PRT {%s};");
            I("PRT #m;");
            I("PRT ['f*'];");
            I("PRT ['fa'..'fc'];");
            //---
            I("PRT #m[1];");
            I("PRT ['f*'][1];");
            I("PRT ['fa'..'fc'][1];");
            //----------------------------
            I("PRT @fa;");
            I("PRT @{%s};");
            I("PRT @#m;");
            I("PRT ['ref:f*'];");
            I("PRT ['ref:fa'..'ref:fc'];");
            //---
            I("PRT @#m[1];");
            I("PRT ['ref:f*'][1];");
            I("PRT ['ref:fa'..'ref:fc'][1];");
            //----------------------------
            I("PRT ref:fa;");
            I("PRT ref:{%s};");
            I("PRT ref:#m;");
            I("PRT ['ref:f*'];");
            I("PRT ['ref:fa'..'ref:fc'];");
            //---
            I("PRT ref:#m[1];");
            I("PRT ['ref:f*'][1];");
            I("PRT ['ref:fa'..'ref:fc'][1];");
            //----------------------------
            I("PRT {%b}:fa;");
            I("PRT {%b}:{%s};");
            I("PRT {%b}:#m;");
            I("PRT ['{%b}:f*'];");
            I("PRT ['{%b}:fa'..'{%b}:fc'];");
            //---
            I("PRT {%b}:#m[1];");
            I("PRT ['{%b}:f*'][1];");
            I("PRT ['{%b}:fa'..'{%b}:fc'][1];");
            //----------------------------

            FAIL("PRT %b:%s;");
            FAIL("PRT %b:{%s};");
            FAIL("PRT {%b}:%s;");  //%s is not in that bank
            I("PRT {%b}:{%s};");

            //================================ PRT end ==========================================



            //================================ COPY start ==========================================
            //----------------------------
            I("COPY fa TO ref:*;");
            I("COPY {%s} TO ref:*;");
            I("COPY #m TO ref:*;");
            I("COPY f* TO ref:*;");
            I("COPY fa..fc TO ref:*;");
            I("COPY {'f*'} TO ref:*;");
            I("COPY {'fa'..'fc'} TO ref:*;");
            //---
            I("COPY {#m[1]} TO ref:*;");
            //I("COPY f*[1];");                =====> NO GOOD, do not allow
            //I("COPY fa..fc[1];");            =====> NO GOOD, do not allow
            I("COPY {['f*'][1]} TO ref:*;");
            I("COPY {['fa'..'fc'][1]} TO ref:*;");
            //----------------------------
            I("COPY @fa TO work:*;");
            I("COPY @{%s} TO work:*;");
            I("COPY @#m TO work:*;");
            I("COPY @f* TO work:*;");
            I("COPY @fa..@fc TO work:*;");
            I("COPY {'ref:f*'};");
            I("COPY {'ref:fa'..'ref:fc'};");
            I("COPY {'@f*'};");
            I("COPY {'@fa'..'@fc'};");
            //---
            I("COPY @{#m[1]} TO work:*;");
            I("COPY {['@f*'][1]};");     //           =====> NO GOOD, do not allow
            I("COPY {['@fa..@fc'][1]};"); //           =====> NO GOOD, do not allow
            
            //----------------------------
            I("COPY ref:fa TO work:*;");
            I("COPY ref:{%s} TO work:*;");
            I("COPY ref:#m TO work:*;");
            I("COPY ref:f* TO work:*;");
            I("COPY ref:fa..ref:fc TO work:*;");
            I("COPY {'ref:f*'};");
            I("COPY {'ref:fa'..'ref:fc'};");
            //---
            I("COPY ref:{#m[1]} TO work:*;");
            //I("COPY ref:f*[1];");                =====> NO GOOD, do not allow
            //I("COPY ref:fa..fc[1];");            =====> NO GOOD, do not allow
            //I("COPY ref:[f*][1];");
            //I("COPY ref:[fa..fc][1];");
            //----------------------------
            I("COPY {%b}:fa TO work:*;");
            I("COPY {%b}:{%s} TO work:*;");
            I("COPY {%b}:#m TO work:*;");
            I("COPY {%b}:f* TO work:*;");
            I("COPY {%b}:fa..{%b}:fc TO work:*;");
            I("COPY {'{%b}:f*'};");
            I("COPY {'{%b}:fa'..'{%b}:fc'};");
            //---
            I("COPY {%b}:{#m[1]} TO work:*;");
            //I("COPY {%b}:f*[1];");                =====> NO GOOD, do not allow
            //I("COPY {%b}:fa..fc[1];");            =====> NO GOOD, do not allow
            //I("COPY {%b}:[f*][1];");
            //I("COPY {%b}:[fa..fc][1];");
            //----------------------------
            //
            //
            //
            I("COPY %ba TO ref:*;"); //string with bank
            I("COPY %s TO ref:*;"); //string without bank

            I("COPY {%ba} TO work:*;"); //string with bank
            I("COPY {%s} TO ref:*;"); //string without bank
            //================================ COPY end ==========================================



        }

        [TestMethod]
        public void _Test_Indexers()
        {
            Databank work = First();
            Databank base2 = Program.databanks.GetRef();
            I("RESET;");
            I("CREATE fy;");
            I("TIME 2000 2002;");
            I("SERIES fy = (100, 101, 102);");
            I("%s = 'fy';");
            I("%u = 'y';");
            I("%v = 'f';");

            I("SERIES xx1 = {%s}[2000];");
            I("%v1 = {%s}[2000];");
            FAIL("SERIES xx1 = {s}[2000];");
            FAIL("%v1 = {s}[2000];");
            I("SERIES xx2 = fy[2000];");
            I("VAL %v1 = fy[2000];");

            I("SERIES xx2 = f%u[2000];");
            I("%v1 = f%u[2000];");
            FAIL("SERIES xx2 = %v|y[2000];");
            FAIL("VAL %v1 = %v|y[2000];");
            FAIL("SERIES xx2 = %v%u[2000];");
            FAIL("VAL v1 = %v%u[2000];");

            I("SERIES xx2 = f{%u}[2000];");
            I("VAL %v1 = f{%u}[2000];");
            I("SERIES xx2 = {%v}y[2000];");
            I("VAL %v1 = {%v}y[2000];");
            I("SERIES xx2 = {%v}{%u}[2000];");
            I("VAL %v1 = {%v}{%u}[2000];");

            FAIL("SERIES xx2 = f{u}[2000];");
            FAIL("VAL v1 = f{u}[2000];");
            FAIL("SERIES xx2 = {v}y[2000];");
            FAIL("VAL v1 = {v}y[2000];");
            FAIL("SERIES xx2 = {v}{u}[2000];");
            FAIL("VAL v1 = {v}{u}[2000];");

            I("SERIES xx2 = {'f'+%u}[2000];");  //artificial... but must work anyway
            I("%v1 = {'f'+%u}[2000];");

            // ------------------ lists ----------------------------------------
            I("#m = fe, fy;");
            I("SERIES xx3 = {#m[2]}[2000];");
            _AssertSeries(First(), "xx3", 2000, 100d, sharedDelta);
            _AssertSeries(First(), "xx3", 2001, 100d, sharedDelta);
            _AssertSeries(First(), "xx3", 2002, 100d, sharedDelta);
            I("%v1 = {#m[2]}[2000];");
            //TODO: assert
            I("SERIES xx4 = {#m[2]}[2000];");
            _AssertSeries(First(), "xx4", 2000, 100d, sharedDelta);
            _AssertSeries(First(), "xx4", 2001, 100d, sharedDelta);
            _AssertSeries(First(), "xx4", 2002, 100d, sharedDelta);
            I("%v = {#m[2]}[2000];");
            _AssertScalarVal(First(), "%v", 100d);
            I("xx5 = {#m[2]}[-1];");  //lagged values
            _AssertSeries(First(), "xx5", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2001, 100d, sharedDelta);
            _AssertSeries(First(), "xx5", 2002, 101d, sharedDelta);
            _AssertSeries(First(), "xx5", 2003, double.NaN, sharedDelta);

            // ------------- Left-side indexer ---------------------------------
            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp;");
            I("SERIES gdp = (100, 101, 102);");
            I("CLONE;");
            I("SERIES gdp[2011] = 1000;");
            _AssertSeries(First(), "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "gdp", 2010, 100d, sharedDelta);
            _AssertSeries(First(), "gdp", 2011, 1000d, sharedDelta);
            _AssertSeries(First(), "gdp", 2012, 102d, sharedDelta);
            _AssertSeries(First(), "gdp", 2013, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2010, 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2011, 101d, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2012, 102d, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2013, double.NaN, sharedDelta);

            //With SERIES bank:series[...] = ...
            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp;");
            I("SERIES gdp = (100, 101, 102);");
            I("CLONE;");
            I("SERIES ref:gdp[2011] = 1000;");
            _AssertSeries(First(), "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "gdp", 2010, 100d, sharedDelta);
            _AssertSeries(First(), "gdp", 2011, 101d, sharedDelta);
            _AssertSeries(First(), "gdp", 2012, 102d, sharedDelta);
            _AssertSeries(First(), "gdp", 2013, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2010, 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2011, 1000d, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2012, 102d, sharedDelta);
            _AssertSeries(Program.databanks.GetRef(),  "gdp", 2013, double.NaN, sharedDelta);

            //With SERIES #m[1] = ...
            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp;");
            I("SERIES gdp = (100, 101, 102);");
            I("#m = gdp, gdp2;");
            if (Globals.UNITTESTFOLLOWUP)
            {
                //#9874345348
                I("{#m[1]} = 1000;");
                _AssertSeries(First(), "gdp", 2009, double.NaN, sharedDelta);
                _AssertSeries(First(), "gdp", 2010, 1000d, sharedDelta);
                _AssertSeries(First(), "gdp", 2011, 1000d, sharedDelta);
                _AssertSeries(First(), "gdp", 2012, 1000d, sharedDelta);
                _AssertSeries(First(), "gdp", 2013, double.NaN, sharedDelta);
            }

            //With SERIES #m[1][2011] = ...
            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp;");
            I("SERIES gdp = (100, 101, 102);");
            I("#m = gdp, gdp2;");
            if (Globals.UNITTESTFOLLOWUP)
            {
                //#9874345348
                I("{#m[1]}[2011] = 1000;");
                _AssertSeries(First(), "gdp", 2009, double.NaN, sharedDelta);
                _AssertSeries(First(), "gdp", 2010, 100d, sharedDelta);
                _AssertSeries(First(), "gdp", 2011, 1000d, sharedDelta);
                _AssertSeries(First(), "gdp", 2012, 102d, sharedDelta);
                _AssertSeries(First(), "gdp", 2013, double.NaN, sharedDelta);
            }
            
            //---------------- loop with lags ---------------
            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp1, gdp2;");
            I("#m = gdp1, gdp2;");
            I("{#m} = (100, 110, 120);");
            I("CLONE;");

            I("SERIES<2011 2012> gdp1 = gdp1[-1] * 1.01;");  //1% growth
            I("FOR date %t = 2011 to 2012; SERIES gdp2[%t] = gdp2[%t-1] * 1.01; END;");  //1% growth

            _AssertSeries(First(), "gdp1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "gdp1", 2010, 100d, sharedDelta);
            _AssertSeries(First(), "gdp1", 2011, 100d * 1.01d, sharedDelta);
            _AssertSeries(First(), "gdp1", 2012, 110d * 1.01d, sharedDelta);  //Beware: lagged endo does not accumulate anymore, use %= etc.
            _AssertSeries(First(), "gdp1", 2013, double.NaN, sharedDelta);

            _AssertSeries(First(), "gdp2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "gdp2", 2010, 100d, sharedDelta);
            _AssertSeries(First(), "gdp2", 2011, 100d * 1.01d, sharedDelta);
            _AssertSeries(First(), "gdp2", 2012, 100d * 1.01d * 1.01d, sharedDelta);
            _AssertSeries(First(), "gdp2", 2013, double.NaN, sharedDelta);

        }                

        [TestMethod]
        public void _Test_FunctionCondit()
        {
            I("RESET;");
            I("TIME 2001 2003;");
            I("SER xx1 = (1, 2, 3);");
            I("SER xx2 = (3, 2, 1);");
            I("SER xx0 = (7, 8, 9);");
            I("VAL %v = 10;");
            I("SER xx3 = iif(2*xx1, '<', xx2/0.5, 1*xx0, %v/1);");
            _AssertSeries(First(), "xx3", 2000, double.NaN, 0d);
            _AssertSeries(First(), "xx3", 2001, 7d, 0d);
            _AssertSeries(First(), "xx3", 2002, 10d, 0d);
            _AssertSeries(First(), "xx3", 2003, 10d, 0d);
            _AssertSeries(First(), "xx3", 2004, double.NaN, 0d);
            I("DELETE xx3;");
            I("SER xx3 = iif(2*xx1, '<=', xx2/0.5, 1*xx0, %v/1);");
            _AssertSeries(First(), "xx3", 2000, double.NaN, 0d);
            _AssertSeries(First(), "xx3", 2001, 7d, 0d);
            _AssertSeries(First(), "xx3", 2002, 8d, 0d);
            _AssertSeries(First(), "xx3", 2003, 10d, 0d);
            _AssertSeries(First(), "xx3", 2004, double.NaN, 0d);
            I("DELETE xx3;");
            I("SER xx3 = iif(2*xx1, '>', xx2/0.5, 1*xx0, %v/1);");
            _AssertSeries(First(), "xx3", 2000, double.NaN, 0d);
            _AssertSeries(First(), "xx3", 2001, 10d, 0d);
            _AssertSeries(First(), "xx3", 2002, 10d, 0d);
            _AssertSeries(First(), "xx3", 2003, 9d, 0d);
            _AssertSeries(First(), "xx3", 2004, double.NaN, 0d);
            I("DELETE xx3;");
            I("SER xx3 = iif(2*xx1, '>=', xx2/0.5, 1*xx0, %v/1);");
            _AssertSeries(First(), "xx3", 2000, double.NaN, 0d);
            _AssertSeries(First(), "xx3", 2001, 10d, 0d);
            _AssertSeries(First(), "xx3", 2002, 8d, 0d);
            _AssertSeries(First(), "xx3", 2003, 9d, 0d);
            _AssertSeries(First(), "xx3", 2004, double.NaN, 0d);
            I("DELETE xx3;");
            I("SER xx3 = iif(2*xx1, '==', xx2/0.5, 1*xx0, %v/1);");
            _AssertSeries(First(), "xx3", 2000, double.NaN, 0d);
            _AssertSeries(First(), "xx3", 2001, 10d, 0d);
            _AssertSeries(First(), "xx3", 2002, 8d, 0d);
            _AssertSeries(First(), "xx3", 2003, 10d, 0d);
            _AssertSeries(First(), "xx3", 2004, double.NaN, 0d);
            I("DELETE xx3;");
            I("SER xx3 = iif(2*xx1, '<>', xx2/0.5, 1*xx0, %v/1);");
            _AssertSeries(First(), "xx3", 2000, double.NaN, 0d);
            _AssertSeries(First(), "xx3", 2001, 7d, 0d);
            _AssertSeries(First(), "xx3", 2002, 10d, 0d);
            _AssertSeries(First(), "xx3", 2003, 9d, 0d);
            _AssertSeries(First(), "xx3", 2004, double.NaN, 0d);
        }

        [TestMethod]
        public void Test__GenrLeftSideFunction()
        {
            I("RESET;");
            I("CREATE gdp;");
            I("TIME 2010 2012;");
            I("SERIES gdp = 100, 101, 102;");
            I("TIME 2011 2012;");
            I("SERIES log(gdp) = 10;");
            _AssertSeries(First(), "gdp", 2010, 100d, 0d);
            _AssertSeries(First(), "gdp", 2011, Math.Exp(10d), 0d);
            _AssertSeries(First(), "gdp", 2012, Math.Exp(10d), 0d);
            I("TIME 2010 2012;");
            I("SERIES gdp = 100, 101, 102;");
            I("TIME 2011 2012;");
            I("SERIES dlog(gdp) = 0.1;");
            _AssertSeries(First(), "gdp", 2010, 100d, 0d);
            _AssertSeries(First(), "gdp", 2011, 100d * Math.Exp(0.1d), 0d);
            _AssertSeries(First(), "gdp", 2012, 100d * Math.Exp(0.1d) * Math.Exp(0.1d), 0d);
            I("TIME 2010 2012;");
            I("SERIES gdp = 100, 101, 102;");
            I("TIME 2011 2012;");
            I("SERIES pch(gdp) = 10;");
            _AssertSeries(First(), "gdp", 2010, 100d, 0d);
            _AssertSeries(First(), "gdp", 2011, 100d * 1.1d, 0d);
            _AssertSeries(First(), "gdp", 2012, 100d * 1.1d * 1.1d, 0d);
            I("TIME 2010 2012;");
            I("SERIES gdp = 100, 101, 102;");
            I("TIME 2011 2012;");
            I("SERIES dif(gdp) = 0.1;");
            _AssertSeries(First(), "gdp", 2010, 100d, 0d);
            _AssertSeries(First(), "gdp", 2011, 100.1d, 0.000001d);
            _AssertSeries(First(), "gdp", 2012, 100.2d, 0.000001d);
            I("TIME 2010 2012;");
            I("SERIES gdp = 100, 101, 102;");
            I("TIME 2011 2012;");
            I("SERIES diff(gdp) = 0.1;");
            _AssertSeries(First(), "gdp", 2010, 100d, 0d);
            _AssertSeries(First(), "gdp", 2011, 100.1d, 0.000001d);
            _AssertSeries(First(), "gdp", 2012, 100.2d, 0.000001d);




        }

        [TestMethod]
        public void _Test_Series()
        {
            //See also Test__Indexer

            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp;");
            I("SERIES gdp = (100, 101, 102);");
            I("CLONE;");
            I("SERIES gdp = 1000;");

            _AssertSeries(First(), "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "gdp", 2010, 1000d, sharedDelta);
            _AssertSeries(First(), "gdp", 2011, 1000d, sharedDelta);
            _AssertSeries(First(), "gdp", 2012, 1000d, sharedDelta);
            _AssertSeries(First(), "gdp", 2013, double.NaN, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2010, 100d, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2011, 101d, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2012, 102d, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2013, double.NaN, sharedDelta);

            //Testing SERIES bank:series = ...
            I("RESET;");
            I("TIME 2010 2012;");
            I("CREATE gdp;");
            I("SERIES gdp = (100, 101, 102);");
            I("CLONE;");
            I("SERIES ref:gdp = 1000;");
            _AssertSeries(First(), "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "gdp", 2010, 100d, sharedDelta);
            _AssertSeries(First(), "gdp", 2011, 101d, sharedDelta);
            _AssertSeries(First(), "gdp", 2012, 102d, sharedDelta);
            _AssertSeries(First(), "gdp", 2013, double.NaN, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2009, double.NaN, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2010, 1000d, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2011, 1000d, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2012, 1000d, sharedDelta);
            _AssertSeries(Ref(),  "gdp", 2013, double.NaN, sharedDelta);
        }

        [TestMethod]
        public void _Test_Cache()
        {
            //But cache not active anymore...?
            
            //==================== TRUNCATE ===========================================
            Databank work = First();

            //One-by-one poses no problems ever
            I("RESET;");
            I("TIME 2000 2000;");
            I("CREATE ts1, ts2;");
            I("SERIES ts1 = 100;");
            I("SERIES ts2 = 100;");
            I("DELETE ts1;");
            I("CREATE ts1;");
            I("SERIES ts1 = 300;");
            _AssertSeries(First(), "ts1", 2000, 300d, sharedDelta);
            _AssertSeries(First(), "ts2", 2000, 100d, sharedDelta);

            //One chunk is problematic
            I(@"
                RESET;
                TIME 2000 2000;
                CREATE ts1, ts2;
                SERIES ts1 = 100;
                SERIES ts2 = 100;
                DELETE ts1;
                CREATE ts1;
                SERIES ts1 = 300;
            ");
            _AssertSeries(First(), "ts1", 2000, 300d, sharedDelta);
            _AssertSeries(First(), "ts2", 2000, 100d, sharedDelta);








        }

        [TestMethod]
        public void _Test_Copy2()
        {           

            //TODO: quarters and months

            //We assume all the respect etc. stuff (tested above) still works...!
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\temp';");
            //Create bank1 and bank2

            Databank bank1 = null;
            Databank bank2 = null;
            Databank work = null;
            Databank base2 = null;
            

            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            if (true)
            {
                //we test this once
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            // ======================================================
            // ================== TESTING <from> and <to> etc.
            // ======================================================

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Testing <frombank=...> and <tobank=...>
            // -------------------------------------------------
            I("UNLOCK bank2;");
            I("COPY <frombank=bank1 tobank=bank2> a1 TO a2;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);  //from here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 101, sharedDelta); //to here
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Testing <frombank=...> and <tobank=...> where these are overridden with explicit banks
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY <frombank=bank1 tobank=bank2> bank2:a1 TO bank1:a2;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Testing <frombank=...> and <tobank=...> where these are overridden with explicit banks
            //    Here we use placeholder after TO
            // -------------------------------------------------
            I("UNLOCK bank2;");
            I("UNLOCK bank1;");
            I("COPY <frombank=bank1 tobank=bank2> bank2:a1 TO bank1:*;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Testing <frombank=...> and <tobank=...> where these are overridden with explicit banks
            //    Here we use wildcard before TO and placeholder after TO
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("UNLOCK bank2;");
            I("COPY <frombank=bank1 tobank=bank2> bank2:a* TO bank1:*;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 202, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta); //from here
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Testing <frombank=...> and <tobank=...> where these are overridden with explicit banks
            //    Here we use range before TO and placeholder after TO
            // -------------------------------------------------
            I("UNLOCK bank2;");
            I("UNLOCK bank1;");
            I("COPY <frombank=bank1 tobank=bank2> bank2:a2..bank2:a3 TO bank1:*;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 202, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta); //from here
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Testing <frombank=...> and <tobank=...> where these are overridden with explicit banks
            //    Here we use lists
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("UNLOCK bank2;");
            I("COPY <frombank=bank1 tobank=bank2> bank2:{#m} TO bank1:{#m};");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            //Same as above, just using .addbank() instead
            I("UNLOCK bank1;");
            I("UNLOCK bank2;");
            I("COPY <frombank=bank1 tobank=bank2> {#m.addbank('bank2')} TO {#m.addbank('bank1')};");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(), "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(), "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(), "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }


            // ======================================================
            // ================== TESTING without <from> and <to> etc.
            // ======================================================

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Simple
            // -------------------------------------------------
            I("COPY a1 TO a2;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);  //from here
                _AssertSeries(First(), "a2", 2010, -101, sharedDelta);  //to here
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Explicit banks
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY bank2:a1 TO bank1:a2;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Here we use placeholder after TO
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY bank2:a1 TO bank1:*;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Here we use wildcard before TO and placeholder after TO
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY bank2:a* TO bank1:*;");

            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 202, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta); //from here
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Here we use range before TO and placeholder after TO
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY bank2:a2..bank2:a3 TO bank1:*;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 202, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta); //from here
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Here we use lists
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY bank2:{#m} TO bank1:{#m};");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta); //from here
            }


            // ======================================================
            // ================== TESTING @
            // ======================================================

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Simple
            // -------------------------------------------------
            I("COPY @a1 TO @a2;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta); //from here
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -201, sharedDelta); //to here
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Explicit banks
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY @a1 TO bank1:a2;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);  //from here
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, -201, sharedDelta); //to here
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Here we use placeholder after TO
            // -------------------------------------------------
            I("COPY bank2:a1 TO @*;");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, 201, sharedDelta); //to here
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta); //from here
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Another variant
            // -------------------------------------------------
            I("UNLOCK bank1;");
            I("COPY @{#m} TO bank1:{#m};");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -101, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta); //from here
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta); //from here
                _AssertSeries(bank1, "a1", 2010, -201, sharedDelta); //to here
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, -203, sharedDelta); //to here
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }


            // ======================================================
            // ================== TESTING complicated stuff
            // ======================================================

            //Includes RESET
            Helper_copy_init(ref work, ref bank1, ref bank2, ref base2);
            // -------------------------------------------------
            //    Expressions
            //    it even seems you can put a bank: before the stuff after TO here
            // -------------------------------------------------
            I("STRING %s1 = 'r';");
            I("STRING %s2 = 'ef:a1';");
            I("COPY {%s1+%s2} TO {#m[1]+''};");
            if (true)
            {
                _AssertSeries(First(), "a1", 2010, -201, sharedDelta);
                _AssertSeries(First(), "a2", 2010, -102, sharedDelta);
                _AssertSeries(First(), "a3", 2010, -103, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a1", 2010, -201, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a2", 2010, -202, sharedDelta);
                _AssertSeries(Program.databanks.GetRef(),  "a3", 2010, -203, sharedDelta);
                _AssertSeries(bank1, "a1", 2010, 101, sharedDelta);
                _AssertSeries(bank1, "a2", 2010, 102, sharedDelta);
                _AssertSeries(bank1, "a3", 2010, 103, sharedDelta);
                _AssertSeries(bank2, "a1", 2010, 201, sharedDelta);
                _AssertSeries(bank2, "a2", 2010, 202, sharedDelta);
                _AssertSeries(bank2, "a3", 2010, 203, sharedDelta);
            }

            //======================== COPY ============================================

            //TODO: test q and m freqs
            //      copy x; or copy x as x; should give error, but is not harmful.

            //=====no respect

            //-----does not exist beforehand

            //one item
            I("RESET;");
            I("TIME 2010 2011;");
            I("create a1;");
            I("SERIES a1 = (111, 112);");
            I("COPY a1 TO b1;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);

            //two items
            I("RESET;");
            I("TIME 2010 2011;");
            I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("COPY a1, a2 TO b1, b2;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2010, 222d, sharedDelta);
            _AssertSeries(First(), "b2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "b2", 2012, double.NaN, sharedDelta);

            //-----does exist beforehand

            //one item
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, b1;");
            I("SERIES a1 = (111, 112);");
            I("TIME 2009 2012;");
            I("SERIES b1 = 777;");
            I("COPY a1 TO b1;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);

            //two items
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, a2, b1, b2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("TIME 2009 2012;");
            I("SERIES b1 = 777;");
            I("SERIES b2 = 888;");
            I("COPY a1, a2 TO b1, b2;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2010, 222d, sharedDelta);
            _AssertSeries(First(), "b2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "b2", 2012, double.NaN, sharedDelta);

            //=====with respect <>

            //-----does not exist beforehand

            //one item
            I("RESET;");
            I("TIME 2010 2011;");
            I("create a1;");
            I("SERIES a1 = (111, 112);");
            I("COPY<2011 2011 respect> a1 TO b1;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);

            //two items
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("COPY<2011 2011 respect>  a1, a2 TO b1, b2;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "b2", 2012, double.NaN, sharedDelta);

            //-----does exist beforehand

            //one item
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, b1;");
            I("SERIES a1 = (111, 112);");
            I("TIME 2009 2012;");
            I("SERIES b1 = 777;");
            I("COPY<2011 2011 respect>  a1 TO b1;");
            _AssertSeries(First(), "b1", 2009, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, 777d, sharedDelta);

            //two items
            I("RESET;");
            I("TIME 2010 2011;");
            I("create a1, a2, b1, b2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("TIME 2009 2012;");
            I("SERIES b1 = 777;");
            I("SERIES b2 = 888;");
            I("COPY<2011 2011 respect>  a1, a2 TO b1, b2;");
            _AssertSeries(First(), "b1", 2009, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, 777d, sharedDelta);
            _AssertSeries(First(), "b2", 2009, 888d, sharedDelta);
            _AssertSeries(First(), "b2", 2010, 888d, sharedDelta);
            _AssertSeries(First(), "b2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "b2", 2012, 888d, sharedDelta);

            //=====with respect and TIME command

            //-----does not exist beforehand

            //one item
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1;");
            I("SERIES a1 = (111, 112);");
            I("TIME 2011 2011;");
            I("COPY<respect> a1 TO b1;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);

            //two items
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("TIME 2011 2011;");
            I("COPY<respect>  a1, a2 TO b1, b2;");
            _AssertSeries(First(), "b1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "b2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "b2", 2012, double.NaN, sharedDelta);

            //-----does exist beforehand

            //one item
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, b1;");
            I("SERIES a1 = (111, 112);");
            I("TIME 2009 2012;");
            I("SERIES b1 = 777;");
            I("TIME 2011 2011;");
            I("COPY<respect>  a1 TO b1;");
            _AssertSeries(First(), "b1", 2009, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, 777d, sharedDelta);

            //two items
            I("RESET;");
            I("TIME 2010 2011;");
            I("create a1, a2, b1, b2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("TIME 2009 2012;");
            I("SERIES b1 = 777;");
            I("SERIES b2 = 888;");
            I("TIME 2011 2011;");
            I("COPY<respect>  a1, a2 TO b1, b2;");
            _AssertSeries(First(), "b1", 2009, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2010, 777d, sharedDelta);
            _AssertSeries(First(), "b1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "b1", 2012, 777d, sharedDelta);
            _AssertSeries(First(), "b2", 2009, 888d, sharedDelta);
            _AssertSeries(First(), "b2", 2010, 888d, sharedDelta);
            _AssertSeries(First(), "b2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "b2", 2012, 888d, sharedDelta);

            //====================== copy from other bank

            //We assume all the respect etc. stuff (tested above) still works...!
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("CLONE;");  //Clones into Base
            I("DELETE a1, a2;"); //Now only left in Base
            I("COPY ref:a1, ref:a2;");
            _AssertSeries(First(), "a1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "a1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "a1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "a1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "a2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "a2", 2010, 222d, sharedDelta);
            _AssertSeries(First(), "a2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "a2", 2012, double.NaN, sharedDelta);

            //====================== wildcards

            //We assume all the respect etc. stuff (tested above) still works...!
            I("RESET;");
            I("TIME 2010 2011;");
            I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("CLONE;");  //Clones into Base
            I("DELETE a1, a2;"); //Now only left in Base
            I("COPY ref:a*;");
            _AssertSeries(First(), "a1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "a1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "a1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "a1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "a2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "a2", 2010, 222d, sharedDelta);
            _AssertSeries(First(), "a2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "a2", 2012, double.NaN, sharedDelta);

            //We assume all the respect etc. stuff (tested above) still works...!
            I("RESET;");
            I("TIME 2010 2011;");
            //I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("CLONE;");  //Clones into Base
            I("DELETE a1, a2;"); //Now only left in Base
            I("COPY ref:a1..ref:a2;");
            _AssertSeries(First(), "a1", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "a1", 2010, 111d, sharedDelta);
            _AssertSeries(First(), "a1", 2011, 112d, sharedDelta);
            _AssertSeries(First(), "a1", 2012, double.NaN, sharedDelta);
            _AssertSeries(First(), "a2", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "a2", 2010, 222d, sharedDelta);
            _AssertSeries(First(), "a2", 2011, 223d, sharedDelta);
            _AssertSeries(First(), "a2", 2012, double.NaN, sharedDelta);

            //====================== copy between two banks

            //Wildcards and ranges to the right of TO: we only allow 1 item with 1 star!

            FAIL("COPY * TO work:a1..a2;");
            FAIL("COPY * TO work:a?;");
            FAIL("COPY * TO work:a*b*c;");
            FAIL("COPY * TO work:a*c, work:d;");
            FAIL("COPY * TO work:d, work:a*c;");

            FAIL("COPY * TO a1..a2;");
            FAIL("COPY * TO a?;");
            FAIL("COPY * TO a*b*c;");
            FAIL("COPY * TO a*c, d;");
            FAIL("COPY * TO d, a*c;");

            FAIL("COPY a,b,c TO d,e;");  //3 versus 2

            

            //Test of COPY <error=no>

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\temp';");
            I("SERIES xx1 = 1;");
            I("OPEN<edit>tmp; CLEAR tmp;");
            I("COPY work:xx1 to tmp:xx1;");
            FAIL("COPY work:xx2 to tmp:xx2;");
            FAIL("COPY work:xx2* to tmp:*;");
            Assert.AreEqual(Program.databanks.GetDatabank("tmp").storage.Count, 1);
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\temp';");
            I("SERIES xx1 = 1;");
            I("OPEN<edit>tmp; CLEAR tmp;");
            I("COPY <error=no> work:xx1 to tmp:xx1;");
            I("COPY <error=no> work:xx2 to tmp:xx2;");
            I("COPY <error=no> work:xx2* to tmp:*;");
            Assert.AreEqual(Program.databanks.GetDatabank("tmp").storage.Count, 1);

            if (true)
            {
                //some random syntax checks
                I("reset;");
                I("fy = 100;");
                I("fy = 100;");
                I("fe = 100;");
                I("pcp = 100;");
                I("a = 1; b = 2; c = 3;");
                I("#m = fy, fe;");
                I("clone;");

                I("COPY fy TO fe;");
                I("COPY fy TO @fe;");
                I("COPY @fy TO fe;");
                I("COPY @fy TO @fe;");

                I("COPY fy TO ref:fe;");
                I("COPY ref:fy TO fe;");
                I("COPY work:fy TO ref:fe;");

                //hmmm, copying to itself, and TO statement in the middle how does that parse?
                //I("COPY work:fy, fe, @fe, work:{#m}, {#m}, @{#m} TO ref:fe, @pcp, fy, work:{#m}, {#m}, @{#m};");

                I("COPY a, b, c TO x*y;");
                I("COPY {#m} TO x*y;");

                I("COPY ref:a, work:b, ref:c TO ref:x*y;");
                I("COPY ref:{#m} TO ref:x*y;");

                I("COPY @a, b, @c TO @x*y;");
                I("COPY @{#m} TO @x*y;");

                FAIL("COPY @a, b, @c;");  //b copied to itself
                I("COPY @{#m};");

            }
        }

        private static void Helper_copy_init(ref Databank work, ref Databank bank1, ref Databank bank2, ref Databank base2)
        {
            I("RESET; mode data;");

            I("TIME 2010 2010;");
            I("OPEN<edit>bank1;");
            I("CLEAR bank1;");
            //I("CREATE a1, a2, a3;");
            I("SERIES a1 = 101;");
            I("SERIES a2 = 102;");
            I("SERIES a3 = 103;");
            I("CLOSE bank1;");
            I("OPEN<edit>bank2;");
            I("CLEAR bank2;");
            //I("CREATE a1, a2, a3;");
            I("SERIES a1 = 201;");
            I("SERIES a2 = 202;");
            I("SERIES a3 = 203;");
            I("CLOSE bank2;");

            I("TIME 2010 2010;");
            //I("CREATE a1, a2, a3;");
            I("SERIES a1 = -201;");
            I("SERIES a2 = -202;");
            I("SERIES a3 = -203;");
            I("CLONE;");
            I("SERIES a1 = -101;");
            I("SERIES a2 = -102;");
            I("SERIES a3 = -103;");
            I("OPEN bank1, bank2;");
            I("LIST #m = a1, a3;");
            I("STRING %s = 'a2';");

            bank1 = Program.databanks.GetDatabank("bank1");
            bank2 = Program.databanks.GetDatabank("bank2");
            work = Program.databanks.GetDatabank(Globals.Work);
            base2 = Program.databanks.GetDatabank(Globals.Ref);
        }


        [TestMethod]
        public void _Test_ReadImportTo()
        {
            //also tests EXPORT a little bit
            I("RESET; TIME 2010 2015;");
            I("xx1 = 10; xx2 = 20;");
            I("WRITE temp;");
            I("EXPORT<tsd>temptsd;");
            I("READ temp TO *;");
            I("READ temp TO slet1;");
            I("IMPORT<tsd>temptsd TO *;");
            I("IMPORT<tsd>temptsd TO slet2;");
            Assert.AreEqual(Program.databanks.storage.Count, 6);
        }

        [TestMethod]
        public void _Test_Rename2()
        {
            Databank work = First();

            //======================== rename

            //two items
            I("RESET;");
            I("time 2010 2011;");
            I("create a1, a2;");
            I("SERIES a1 = (111, 112);");
            I("SERIES a2 = (222, 223);");
            I("RENAME a1, a2 as b1, b2;");
            Assert.IsNull(work.GetIVariable("a1!a"));
            Assert.IsNull(work.GetIVariable("a2!a"));
            Assert.IsNotNull(work.GetIVariable("b1!a"));
            Assert.IsNotNull(work.GetIVariable("b2!a"));
            Assert.AreEqual(work.storage.Count, 2);
            I("PRT b1, b2;");
        }        

        [TestMethod]
        public void _Test_Seasonal()
        {
            // ===> In AREMOS, you can run the following:

            //            clear;
            //set freq q;
            //series<2000q1 2009q2> y =
            // 85.2  87.2  87.1  87.2  87.3  90
            // 90.1  90.4  90.5  92.4  92.5  94.7
            // 96.3  98.5  98.6  98.5  99.2  99.8
            // 100.4  100.5  101.2  102.3  101.9  101.7
            // 102.9  103.5  103.5  103.5  104.6  104.5
            // 104.9  104.9  105.9  106  106  106
            // 106.5  106.7 ;
            //x12a y saa "mode=mult sigmalim=(1.50,2.50) seasonalma=msr force=totals print=alltables";
            //print y.saa;

            //                                         y.saa

            //                             2000
            //                               Q1           85.80
            //                               Q2           86.44
            //                               Q3           87.07
            //                               Q4           87.38
            //                             2001
            //                               Q1           87.86
            //                               Q2           89.23
            //                               Q3           90.09
            //                               Q4           90.61
            //                             2002
            //                               Q1           90.96
            //                               Q2           91.67
            //                               Q3           92.51
            //                               Q4           94.96
            //                             2003
            //                               Q1           96.62
            //                               Q2           97.82
            //                               Q3           98.60
            //                               Q4           98.86
            //                             2004
            //                               Q1           99.34
            //                               Q2           99.22
            //                               Q3          100.41
            //                               Q4          100.93
            //                             2005
            //                               Q1          101.20
            //                               Q2          101.81
            //                               Q3          101.92
            //                               Q4          102.18
            //                             2006
            //                               Q1          102.75
            //                               Q2          103.13
            //                               Q3          103.56
            //                               Q4          103.96
            //                             2007
            //                               Q1          104.37
            //                               Q2          104.23
            //                               Q3          104.97
            //                               Q4          105.32
            //                             2008
            //                               Q1          105.64
            //                               Q2          105.80
            //                               Q3          106.08
            //                               Q4          106.38
            //                             2009
            //                               Q1          106.25
            //                               Q2          106.51

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\seasonal';");
            I("OPTION freq q;");
            I("CREATE y; SERIES <2000q1 2009q2> y = '85.2  87.2  87.1  87.2  87.3  90   90.1  90.4  90.5  92.4  92.5  94.7   96.3  98.5  98.6  98.5  99.2  99.8  100.4  100.5  101.2  102.3  101.9  101.7  102.9  103.5  103.5  103.5  104.6  104.5  104.9  104.9  105.9  106  106  106   106.5  106.7' ;");
            I("CREATE ytrue; SERIES <2000q1 2009q2> ytrue = '85.80 86.44 87.07 87.38 87.86 89.23 90.09 90.61 90.96 91.67 92.51 94.96 96.62 97.82 98.60 98.86 99.34 99.22 100.41 100.93 101.20 101.81 101.92 102.18 102.75 103.13 103.56 103.96 104.37 104.23 104.97 105.32 105.64 105.80 106.08 106.38 106.25 106.51';");
            I("STRING %param = 'save=(d10, d11, saa) mode=mult sigmalim=(1.50,2.50) seasonalma=msr force=totals print=alltables';");
            I("LIST #m = list('y');");
            I("X12A <2000q1 2009q2 param = %param> work:{#m};");
            Series ts1 = First().GetIVariable("ytrue!q") as Series;
            Series ts2 = First().GetIVariable("y_saa!q") as Series;
            foreach (GekkoTime gt in new GekkoTimeIterator(new GekkoTime(EFreq.Q, 2000, 1), new GekkoTime(EFreq.Q, 2009, 2)))
            {
                Assert.AreEqual(ts1.GetDataSimple(gt), ts2.GetDataSimple(gt), 0.005d);
            }

            //mode er mult eller add
            //save er d11 eller saa
            //force



        }

        [TestMethod]
        public void _Test__Ols()
        {
            Databank work = First();

            //======================== OLS


            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            //Same data as for Test__R()
            I("CREATE lna1, pcp, bul1;");
            I("SERIES <1998 2010> lna1 = data(' 166.223000  173.221000  179.571000  187.343000  194.888000  202.959000  209.426000  215.134000  222.716000  230.520000  238.518000  246.654000  254.991000 ');");
            I("SERIES <1998 2010> pcp  = data(' 0.9502030   0.9699920   1.0000000   1.0235000   1.0401100   1.0605400   1.0754700   1.0977800   1.1121200   1.1314800   1.1513000   1.1717600   1.1871600  ');");
            I("SERIES <1998 2010> bul1 = data(' 0.0684791   0.0591698   0.0560344   0.0535439   0.0535003   0.0631703   0.0649875   0.0578112   0.0473207   0.0404508   0.0467488   0.0472923   0.0475191  ');");
            I("OLS <2000 2010> dlog(lna1) = dlog(pcp), dlog(pcp.1), bul1, bul1.1;");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("0.144517"));  //stupid test, must be done better...
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("0.613875"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("0.186740"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("-0.350908"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("0.0298039"));
            
            I("time 2000 2010;");
            I("create s0, s1, s2, s3, s4, s5;");
            I("series s0 = dlog(lna1);");
            I("series s1 = dlog(pcp);");
            I("series s2 = dlog(pcp.1);");
            I("series s3 = bul1;");
            I("series s4 = bul1.1;");
            I("series s5 = 1;");
            I("LIST #m = s4, s5;");
            I("matrix #x = pack(2000, 2010, s1, work:s2, s3, work:{#m});");  //just testing the syntax and that lists work
            I("matrix #y = pack(2000, 2010, s0);");
            I("matrix #b = inv(t(#x)*#x)*t(#x)*#y;");
            I("prt #b;");
            _AssertMatrix(First(), "#b", "rows", 5);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 0.144517, 0.000001d);
            _AssertMatrix(First(), "#b", 2, 1, 0.613875, 0.000001d);
            _AssertMatrix(First(), "#b", 3, 1, 0.186740, 0.000001d);
            _AssertMatrix(First(), "#b", 4, 1, -0.350908, 0.000001d);
            _AssertMatrix(First(), "#b", 5, 1, 0.0298039, 0.000001d);

            //======================== See Test__R for the same results

            // AREMOS on jul05.tsd (IMPORT<tsd mute>jul05.tsd.
            // => set per 2000 2010
            // => equ xx dlog(lna1)=dlog(pcp), dlog(pcp.1), bul1, bul1.1;
            // XX
            // Ordinary Least Squares
            // ANNUAL data for   11 periods from 2000 to 2010
            // Date: 19 AUG 2015

            // dlog(lna1)

            //   =    0.14452 * dlog(pcp) + 0.61388 * dlog(pcp)[-1] + 0.18674 * bul1
            //       (0.63661)             (2.59596)                 (0.92202)

            //      - 0.35091 * bul1[-1] + 0.02980
            //       (1.72706)            (3.33310)

            // Sum Sq     0.0001   Std Err    0.0035   LHS Mean   0.0352
            // R Sq       0.6250   R Bar Sq   0.3751   F  4,  6   2.5004
            // D.W.( 1)   1.8651   D.W.( 2)   2.9208

            //                 *
            //OLS estimation 2000-2010 (n = 11)
            // lhs : dlog(lna1)
            // c1  : dlog(pcp)
            // c2  : dlog(pcp.1)
            // c3  : bul1
            // c4  : bul1.1
            // c5  : 1
            // -------------------------------------------------------------------------------------------
            //  Coeff                  c1              c2              c3              c4              c5
            // -------------------------------------------------------------------------------------------
            //  Estimate         0.144517        0.613875        0.186740       -0.350908       0.0298039
            //  Std error        0.227011        0.236473        0.202534        0.203182       0.0089418
            //  T-stat               0.64            2.60            0.92            1.73            3.33
            // ------------------------------------------------------------------------------------------
            //                 * R2: 0.625034    SEE: 0.00346154    DW: 1.8651

            //                 *
            //                 *
            //                 * MATRIX:

            //                      1
            //     1           0.1445
            //     2           0.6139
            //     3           0.1867
            //     4          -0.3509
            //     5           0.0298

        }

        [TestMethod]
        public void _Test_R()
        {
            //TODO: Test what happens if <target> is used on file without gekkoimport, and vice versa

            Databank work = First();

            //======================== See Test__Ols for the same results
            //            Coefficients:
            //(Intercept)           x1           x2           x3           x4
            //     0.0298       0.1445       0.6139       0.1867      -0.3509

            //> summary(fit)

            //Call:
            //lm(formula = y ~ x)

            //Residuals:
            //       Min         1Q     Median         3Q        Max
            //-0.0033362 -0.0021311 -0.0005585  0.0018475  0.0050588

            //Coefficients:
            //             Estimate Std. Error t value Pr(>|t|)
            //(Intercept)  0.029804   0.008942   3.333   0.0157 *
            //x1           0.144517   0.227011   0.637   0.5479
            //x2           0.613875   0.236473   2.596   0.0409 *
            //x3           0.186740   0.202534   0.922   0.3921
            //x4          -0.350908   0.203182  -1.727   0.1349
            //---
            //Signif. codes:  0 '***' 0.001 '**' 0.01 '*' 0.05 '.' 0.1 ' ' 1

            //Residual standard error: 0.003462 on 6 degrees of freedom
            //Multiple R-squared:  0.625,	Adjusted R-squared:  0.3751
            //F-statistic:   2.5 on 4 and 6 DF,  p-value: 0.1516

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            //Same data as for Test__Ols()
            I("CREATE lna1, pcp, bul1;");
            I("SERIES <1998 2010> lna1 = data(' 166.223000  173.221000  179.571000  187.343000  194.888000  202.959000  209.426000  215.134000  222.716000  230.520000  238.518000  246.654000  254.991000') ;");
            I("SERIES <1998 2010> pcp  = data(' 0.9502030   0.9699920   1.0000000   1.0235000   1.0401100   1.0605400   1.0754700   1.0977800   1.1121200   1.1314800   1.1513000   1.1717600   1.1871600')  ;");
            I("SERIES <1998 2010> bul1 = data(' 0.0684791   0.0591698   0.0560344   0.0535439   0.0535003   0.0631703   0.0649875   0.0578112   0.0473207   0.0404508   0.0467488   0.0472923   0.0475191')  ;");
            I("time 2000 2010;");
            I("create s0, s1, s2, s3, s4, s5;");
            I("series s0 = dlog(lna1);");
            I("series s1 = dlog(pcp);");
            I("series s2 = dlog(pcp.1);");
            I("series s3 = bul1;");
            I("series s4 = bul1.1;");
            //I("series s5 = 1;");
            I("matrix #x = pack(2000, 2010, s1, s2, s3, s4);");
            I("matrix #y = pack(2000, 2010, s0);");
            I("r_file ols.r;");
            I("r_export <target = 'data1'> #x, #y;");
            I("r_run;");
            I("PRT #beta;");
            _AssertMatrix(First(), "#beta", "rows", 5);
            _AssertMatrix(First(), "#beta", "cols", 1);
            _AssertMatrix(First(), "#beta", 1, 1, 0.0298039, 0.000001d);
            _AssertMatrix(First(), "#beta", 2, 1, 0.144517, 0.000001d);
            _AssertMatrix(First(), "#beta", 3, 1, 0.613875, 0.000001d);
            _AssertMatrix(First(), "#beta", 4, 1, 0.186740, 0.000001d);
            _AssertMatrix(First(), "#beta", 5, 1, -0.350908, 0.000001d);
            I("s0fit = unpack(2000, 2010, #yfit[.., 1]);");
            //TODO: check s0fit above

            //testing without target
            I("r_file ols2.r;");
            I("r_export #x, #y;");
            I("r_run;");
            I("PRT #beta2;");
            _AssertMatrix(First(), "#beta2", "rows", 5);
            _AssertMatrix(First(), "#beta2", "cols", 1);
            _AssertMatrix(First(), "#beta2", 1, 1, 0.0298039, 0.000001d);
            _AssertMatrix(First(), "#beta2", 2, 1, 0.144517, 0.000001d);
            _AssertMatrix(First(), "#beta2", 3, 1, 0.613875, 0.000001d);
            _AssertMatrix(First(), "#beta2", 4, 1, 0.186740, 0.000001d);
            _AssertMatrix(First(), "#beta2", 5, 1, -0.350908, 0.000001d);



        }

        [TestMethod]
        public void _Test_Blueprint()
        {
            //TEST THIS:

            I("#axc = a, x, c;");
            I("%b = 'x';");
            I("%s = #axc[2];");
            _AssertScalarString(First(), "%s", "x");
            I("%s = #a{'x'}c[2];");
            _AssertScalarString(First(), "%s", "x");
            I("%s = #a{%s}c[2];");
            _AssertScalarString(First(), "%s", "x");
            I("%s = #{'a' + %s + 'c'}[2];");
            _AssertScalarString(First(), "%s", "x");
            I("%s = #{'a{%s}c'}[2];");
            _AssertScalarString(First(), "%s", "x");
            I("%s = #(a%s|c)[2];");
            _AssertScalarString(First(), "%s", "x");
            FAIL("%s = #(a{%s}c)[2];"); //use: #a{%s}c without parenthesis

            // #(a%b|c)[2]
            // #(a{%b}c)[2]
            // #{'a' + %b + 'c'}[2] 
            // #{'a{%b}c'}[2] 


            //----- new stuff with $ etc.

            Databank work = First();
            I("RESET;");
            I("TIME 2000 2001;");
            I("CREATE fy;");
            I("SERIES fy = 2;");
            I("%s = 'fy';");

            //Print a string
            I("PRT %s;");  //will print as string
            I("PRT {%s};"); //print as series

            // ---------------------------- test of #(...)
            //Giver disse to lister:
            // a pV010000
            // a pV020000
            // a pV030000
            // ---
            // b pV040000
            // b pV050000
            //I("string sum=''; list erha = V010000,V020000,V030000; list erhb = V040000,V050000; for i = a, b; string xx = 'erh%i'; list xxx = #(%xx) prefix=p; string sum = %sum + string(#xxx[2]); end;");
            I("string %sum=''; list #erha = V010000,V020000,V030000; list #erhb = V040000,V050000; for string %i = a, b; string %xx = 'erh{%i}'; list #xxx = #{%xx}.prefix('p'); string %sum = %sum + #xxx[2]; end;");
            _AssertScalarString(First(), "%sum", "pV020000pV050000");

            // ----------------- First we use % ----------------------------------------------

            //Define a string from a string
            I("STRING %s1 = %s;");
            _AssertScalarString(First(), "%s1", "fy");
            I("STRING %s2 = '%s';");
            _AssertScalarString(First(), "%s2", "%s");
            I("STRING %s2 = '{%s}';");
            _AssertScalarString(First(), "%s2", "fy");
            I("STRING %s2b = 'x%s|y';");
            _AssertScalarString(First(), "%s2b", "x%s|y");
            I("STRING %s2b = 'x{%s}y';");
            _AssertScalarString(First(), "%s2b", "xfyy");

            I("RESET;");
            I("CREATE fx, fy;");
            I("TIME 2000 2000;");
            I("LIST #n = fx, fy;"); //is list of names
            I("STRING %s = 'def';");
            I("STRING %s2 = %s + #n[2];");
            _AssertScalarString(First(), "%s2", "deffy");
            I("STRING %s4 = %s + '{#n[2]}';");
            _AssertScalarString(First(), "%s4", "deffy");


            //----- new stuff end.

            //-----------------------------
            //Testing stuff in the blueprint paper
            //-----------------------------
            work = First();
            I("RESET;");
            I("TIME 2000 2001;");

            I("VAL %v1 = 1;");
            I("VAL %v2 = 2;");
            I("VAL %z = 3;");
            I("VAL %x = %v1 + %v2;");
            I("VAL %y = %x/%z;");
            _AssertScalarVal(First(), "%x", 3d);
            _AssertScalarVal(First(), "%y", 1d);
            I("VAL %y = (%v1+%v2)/%z;");
            _AssertScalarVal(First(), "%y", 1d);
            I("VAL %y = %v1+%v2/%z;");
            _AssertScalarVal(First(), "%y", 1d + 2d / 3d);

            I("CREATE gdp, g, dp, dpg;");
            I("SERIES gdp = (1, 2);");
            I("SERIES g = (3, 4);");
            I("SERIES dp = (5, 6);");
            I("SERIES dpg = (7, 8);");
            I("STRING %s = 'gdp';");
            I("PRT %s;");  //just prints plain string
            I("STRING %i = 'g';");
            I("STRING %j = 'dp';");
            //Below testing out a lot of combinations (including blanks)
            I("PRT %i + %j;"); //plain
            I("PRT {%i} + {%j};");  //prints g and dp series
            FAIL("PRT {i} + {j};");  //same
            I("PRT {%i + %j};");  //gdp
            I("PRT {'g' + %j};");  //gdp
            I("PRT {%i}{%j};");  //gdp
            FAIL("PRT {%i} {%j};");
            FAIL("PRT {i}{j};");  //same
            FAIL("PRT {i} {j};");
            I("PRT g{%j};");  //gdp
            FAIL("PRT g {%j};");
            FAIL("PRT g{j};");  //same
            FAIL("PRT g {j};");
            I("PRT g%j;");  //gdp
            I("PRT g %j;");  //gdp --------------> before FAIL, but %j is now in 3.0 interpreted as a label
            FAIL("PRT %j|g;");  //dpg
            I("PRT {%j}g;");  //dpg
            FAIL("PRT %j |g;");
            FAIL("PRT %j| g;");
            FAIL("PRT %j g;");
            FAIL("PRT %i%j;");  //gdp
            I("PRT %i %j;");  //gdp  --------------> before FAIL, but %j is now in 3.0 interpreted as a label
            I("PRT %j;");  //'dp'
            I("PRT {%j};");  //dp
            FAIL("PRT {j};");  //dp

            //--------------------------------------------

            I("RESET;");
            I("TIME 2010 2010;");
            I("CREATE a1, a2;");
            I("LIST #a = a1, a2;");
            I("SERIES {#a} = 100;");
            I("SERIES a2 = 200;");
            _AssertSeries(First(), "a1", 2010, 100, sharedDelta);
            _AssertSeries(First(), "a2", 2010, 200, sharedDelta);
            I("PRT #a;");
            I("PRT #a[2];");  //PRT a2
            I("SERIES a2 = {#a[1]} + 1;");  //SERIES a2 = a1;
            _AssertSeries(First(), "a1", 2010, 100, sharedDelta);
            _AssertSeries(First(), "a2", 2010, 101, sharedDelta);
            // Important
            // Important --> we can use {#a}[1] or {#a[1]} as we choose
            // Important
            I("SERIES a2 = {#a}[1] + 2;");
            _AssertSeries(First(), "a2", 2010, 102, sharedDelta);
            FAIL("#a = 1a, 1b;");
            FAIL("#a = 1;");
            FAIL("#a = 007;");
            FAIL("#a = '007';");
            FAIL("#a = 2015q3;");
            I("#a = ('1', '007', '2015q3');");

            if (Globals.UNITTESTFOLLOWUP)
            {
                //fix freq issue
            }
            I("OPTION freq q;");
            I("VAL %a1 = 1;");
            I("DATE %a3 = 2015q3;");
            I("LIST #a = (string(%a1), '007', string(%a3));");
            //AssertHelperList("a", new List<string>() { "1", "007", "2015q3" });
            _AssertListString(First(), "#a", 1, "1");
            _AssertListString(First(), "#a", 2, "007");
            _AssertListString(First(), "#a", 3, "2015q3");

            //--------------------------------------------

            I("RESET;");
            I("TIME 2000 2001;");
            I("CREATE fxa0, fxb0;");
            I("SERIES fxa0 = (1, 2);");
            I("SERIES fxb0 = (3, 4);");
            I("LIST #a = a, b;");
            I("FOR string %i = #a; PRT fX{%i}0; END;");
            I("FOR VAL %i = 1 to #a.len(); PRT fX{#a[%i]}0; END;");

            if (Globals.UNITTESTFOLLOWUP)
            {
                //Maybe allow a list def on LHS for such cases:
                //(pxa, pxb, fxa, fXb) = 100;

                I("CREATE pxa, pxb, fxa, fXb;");
                I("SERIES pxa, pxb, fxa, fXb = 100;");
                I("LIST a1 = a, B;");
                I("FOR string %i = #a1; PRT px{i}*fX{i}; END;");
                I("FOR string %i = #a1; PRT px%i*fX%i; END;");  //same
            }

            //----------------
            //LIST operators, ranges etc.
            //----------------


            I("TIME 2010;");
            I("LIST #a = a1, a2, a3, a4, a5;");
            I("VAL %i = 2;");
            I("CREATE a2, y;");
            I("SERIES a2 = 100;");
            I("LIST #b1 = #a[%i..%i+2];"); //b1 = a2, a3, a4. Sublist of names at position %i, %i+1, %i+2
            _AssertListString(First(), "#b1", new StringOrList(new string[] { "a2", "a3", "a4" }));
            FAIL("VAL %b2 = #a[0];");         //b2 = 5, length of the list as a value
            I("VAL %b2 = #a.len();");
            _AssertScalarVal(First(), "%b2", 5d);
            FAIL("VAL %z = #a[%i][2010];");
            I("VAL %z = {#a[%i]}[2010] + 1;");
            _AssertScalarVal(First(), "%z", 101d);
            FAIL("VAL %z = {#a}[%i][2010] + 2;");  //fails because only a2 exists

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("CREATE gdp3;");
            I("SERIES gdp3 = 100;");
            I("STRING %s = 'dp';");
            I("VAL %x = 2;");
            I("PRT g{%s+%x*(%x+1)/2};");  //one kind of val+string
            I("VAL %v = 3;");
            I("PRT g%s%v;");                //another kind of val+string
            I("PRT g{%s}{%v};");                //another kind of val+string
            FAIL("PRT g{s}{v};");                //another kind of val+string


            I("TIME 2004 2006;");
            I("VAL %v1 = 0.4;");
            I("VAL %v2 = 0.3;");
            I("VAL %v3 = 0.2;");
            I("VAL %v4 = 0.1;");
            I("CREATE gdp2; SERIES gdp2 = 0;");
            I("FOR VAL %i = 1 to 4; SERIES gdp2 = gdp2 + %(v%i) * fy[-%i]; END;");
            I("CREATE gdp_true;");
            I("SERIES gdp_true = 0.4*fy[-1]+0.3*fy[-2]+0.2*fy[-3]+0.1*fy[-4];");
            _AssertSeries(First(), "gdp2", 2005, 1325794d, sharedDelta);
            _AssertSeries(First(), "gdp_true", 2005, 1325794d, sharedDelta);

        }

        //private static List<string> GetListOfStrings(string s)
        //{
        //    return Program.GetListOfStringsFromList(Program.scalars[Globals.symbolCollection + s]);
        //}

        private static List<string> _GetListOfStrings(string s)
        {
            IVariable iv = Program.databanks.GetFirst().GetIVariable("#" + s);
            List list = iv as List;                      
            return Program.GetListOfStringsFromList(list);
        }

        //private static void AssertHelperList(string s, List<string> ss)
        //{
        //    List<string> x = GetListOfStrings(s);
        //    Assert.AreEqual(ss.Count, x.Count);
        //    for (int i = 0; i < ss.Count; i++)
        //    {
        //        Assert.IsTrue(string.Compare(ss[i], x[i]) == 0);
        //    }
        //}

        private static void _AssertHelperList(string s, List<string> ss)
        {
            List<string> x = _GetListOfStrings(s);
            Assert.AreEqual(ss.Count, x.Count);
            for (int i = 0; i < ss.Count; i++)
            {
                Assert.IsTrue(string.Compare(ss[i], x[i]) == 0);
            }
        }

        [TestMethod]
        public void _Test_Ini()
        {
            I("RESET;");
            I("INI;");
        }

        [TestMethod]
        public void _Test_LagFunctions()
        {
            //Inbuilt functions involving lags

            Databank work = First();
            I("RESET;");
            I("CREATE gdp, x, a;");
            I("TIME 2010 2012;");
            I("SERIES<2010 2010> gdp = 100;");
            I("SERIES<2011 2012> gdp ^= 1;");  //100, 101, 102
            I("SERIES<2010 2012> x = 1;");            
            I("SERIES<2009 2012> a = (10, 13, 17, 25);");
                        
            I("SERIES xx1 = dlog(gdp);");
            _AssertSeries(First(), "xx1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2011, Math.Log(101d / 100d), sharedDelta);
            _AssertSeries(First(), "xx1", 2012, Math.Log(102d / 101d), sharedDelta);
            I("SERIES xx2 = pch(gdp);");
            _AssertSeries(First(), "xx2", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2011, 100d * (101d / 100d - 1d), sharedDelta);
            _AssertSeries(First(), "xx2", 2012, 100d * (102d / 101d - 1d), sharedDelta);
            I("SERIES xx3 = dif(gdp);");
            _AssertSeries(First(), "xx3", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2011, (101d - 100d), sharedDelta);
            _AssertSeries(First(), "xx3", 2012, (102d - 101d), sharedDelta);
            I("SERIES xx4 = diff(gdp);");
            _AssertSeries(First(), "xx4", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2011, (101d - 100d), sharedDelta);
            _AssertSeries(First(), "xx4", 2012, (102d - 101d), sharedDelta);
            I("SERIES xx5 = lag(gdp, 2);");
            _AssertSeries(First(), "xx5", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2012, 100d, sharedDelta);            

            I("SERIES xx1 = dlog(gdp/x+0);");
            _AssertSeries(First(), "xx1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2011, Math.Log(101d / 100d), sharedDelta);
            _AssertSeries(First(), "xx1", 2012, Math.Log(102d / 101d), sharedDelta);
            I("SERIES xx2 = pch(gdp/x+0);");
            _AssertSeries(First(), "xx2", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2011, 100d * (101d / 100d - 1d), sharedDelta);
            _AssertSeries(First(), "xx2", 2012, 100d * (102d / 101d - 1d), sharedDelta);
            I("SERIES xx3 = dif(gdp/x+0);");
            _AssertSeries(First(), "xx3", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2011, (101d - 100d), sharedDelta);
            _AssertSeries(First(), "xx3", 2012, (102d - 101d), sharedDelta);
            I("SERIES xx4 = diff(gdp/x+0);");
            _AssertSeries(First(), "xx4", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2011, (101d - 100d), sharedDelta);
            _AssertSeries(First(), "xx4", 2012, (102d - 101d), sharedDelta);
            I("SERIES xx5 = lag(gdp/x+0, 2);");
            _AssertSeries(First(), "xx5", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2012, 100d, sharedDelta);

            I("SERIES xx1 = dlog(gdp[-1]);");
            _AssertSeries(First(), "xx1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2012, Math.Log(101d / 100d), sharedDelta);
            I("SERIES xx2 = pch(gdp[-1]);");
            _AssertSeries(First(), "xx2", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2012, 100d * (101d / 100d - 1d), sharedDelta);
            I("SERIES xx3 = dif(gdp[-1]);");
            _AssertSeries(First(), "xx3", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2012, (101d - 100d), sharedDelta);
            I("SERIES xx4 = diff(gdp[-1]);");
            _AssertSeries(First(), "xx4", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2012, (101d - 100d), sharedDelta);
            I("SERIES xx5 = lag(gdp[-1], 1);");
            _AssertSeries(First(), "xx5", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2012, 100d, sharedDelta);

            I("SERIES xx1 = dlog(gdp[-1]/x+0);");
            _AssertSeries(First(), "xx1", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2012, Math.Log(101d / 100d), sharedDelta);
            I("SERIES xx2 = pch(gdp[-1]/x+0);");
            _AssertSeries(First(), "xx2", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2012, 100d * (101d / 100d - 1d), sharedDelta);
            I("SERIES xx3 = dif(gdp[-1]/x+0);");
            _AssertSeries(First(), "xx3", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", 2012, (101d - 100d), sharedDelta);
            I("SERIES xx4 = diff(gdp[-1]/x+0);");
            _AssertSeries(First(), "xx4", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx4", 2012, (101d - 100d), sharedDelta);
            I("SERIES xx5 = lag(gdp[-1]/x+0, 1);");
            _AssertSeries(First(), "xx5", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx5", 2012, 100d, sharedDelta);
            I("SERIES xx6 = dif(gdp) + dif(gdp);");
            _AssertSeries(First(), "xx6", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx6", 2011, 2d, sharedDelta);
            _AssertSeries(First(), "xx6", 2012, 2d, sharedDelta);
            I("SERIES xx7 = dif(a) + dif(a[-1]);");  //10 13 17 25 from 2009-12, dif(a) -> m, 3, 4, 8, dif(a.1) -> m, m, 3, 4
            _AssertSeries(First(), "xx7", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx7", 2011, 4d + 3d, sharedDelta);
            _AssertSeries(First(), "xx7", 2012, 8d + 4d, sharedDelta);
            I("SERIES xx8 = dif(a) + dif(dif(a[-1]));");  //10 13 17 25 from 2009-12, dif(a) -> m, 3, 4, 8, dif(dif(a.1)) -> m, m, m, 1
            _AssertSeries(First(), "xx8", 2010, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx8", 2011, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx8", 2012, 8d + 1d, sharedDelta);
            I("SERIES<2009 2012> xx9 = 123, 124, 125, 126;");
            I("SERIES<2012 2012> dif(xx9) = dif(a) + dif(dif(a[-1]));");  //see above
            _AssertSeries(First(), "xx9", 2010, 124d, sharedDelta);
            _AssertSeries(First(), "xx9", 2011, 125d, sharedDelta);
            _AssertSeries(First(), "xx9", 2012, 125d + 8d + 1d, sharedDelta);

            //I("SERIES ");
        }

        [TestMethod]
        public void _Test_Filenames()
        {
            //Gekko 3.0 is more restrictive, use {...} to in-substitute

            I("RESET;");
            I("STRING %s1 = 'Thomas';");
            I("STRING %s2 = 'Tho';");
            I("STRING %s3 = 'mas';");
            I("STRING %s4 = 'sletmig';");

            //raw
            I("PIPE c:\\Thomas\\Desktop\\gekko\\testing\\sletmig;"); I("PIPE con;");
                       
            //raw with {}
            I("PIPE c:\\{%s1}\\Desktop\\gekko\\testing\\{%s4};"); I("PIPE con;");
            //raw with {}
            I("PIPE c:\\{%s2}mas\\Desktop\\gekko\\testing\\{%s4};"); I("PIPE con;");
            //raw with {}
            I("PIPE c:\\Tho{%s3}\\Desktop\\gekko\\testing\\{%s4};"); I("PIPE con;");
            //raw with {}
            I("PIPE c:\\{%s2}{%s3}\\Desktop\\gekko\\testing\\{%s4};"); I("PIPE con;");

            //quoted
            I("PIPE 'c:\\Thomas\\Desktop\\gekko\\testing\\sletmig';"); I("PIPE con;");
            //quoted with %            
            I("PIPE 'c:\\' + %s2 + 'mas\\Desktop\\gekko\\testing\\%s4';"); I("PIPE con;");
            //quoted with %
            I("PIPE 'c:\\Tho{%s3}\\Desktop\\gekko\\testing\\{%s4}';"); I("PIPE con;");
            
            //expression
            I("PIPE 'c:\\Thomas\\Desktop\\gek' + 'ko\\testing\\sletmig';"); I("PIPE con;");
        }

        [TestMethod]
        public void _Test_FunctionAndProcedureVariants()
        {
            // ----------- user defined functions

            //0 arg, 0 return
            I("RESET; CLS;");
            I("FUNCTION void f(); TELL 'a1y'; END; f();");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a1y"));

            //1 arg, 0 return
            I("RESET; CLS;");
            I("FUNCTION void f(val %v); TELL 'a2y' + %v; END; f(1);");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a2y1"));

            //0 arg, 1 return
            I("RESET; CLS;");
            I("FUNCTION string f(); TELL 'a3y'; RETURN 'a3y'; END; %s = f();");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a3y"));
            _AssertScalarString(First(), "%s", "a3y");
            I("CLS; f();"); //discards return value
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a3y"));

            //1 arg, 1 return
            I("RESET; CLS;");
            I("FUNCTION string f(val %v); TELL 'a4y' + %v; RETURN 'a4y' + %v; END; %s = f(1);");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a4y1"));
            _AssertScalarString(First(), "%s", "a4y1");
            I("CLS; f(1);"); //discards return value
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a4y1"));

            // ----------- user defined procedures

            //0 arg, 0 return
            I("RESET; CLS;");
            I("PROCEDURE f; TELL 'a5y'; END; f;");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a5y"));

            //1 arg, 0 return
            I("RESET; CLS;");
            I("PROCEDURE f val %v; TELL 'a6y' + %v; END; f 1;");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a6y1"));

            // ----------- user defined, as object functions


            //1 arg, 0 return
            I("RESET; CLS;");
            I("FUNCTION void f(val %v); TELL 'a7y' + %v; END; %x = 1; %x.f();");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a7y1"));


            //1 arg, 1 return
            I("RESET; CLS;");
            I("FUNCTION string f(val %v); TELL 'a8y' + %v; RETURN 'a8y' + %v; END; %x = 1; %s = %x.f();");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a8y1"));
            _AssertScalarString(First(), "%s", "a8y1");

            I("CLS; %zz = 1; %zz.f();"); //discards return value

            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("a8y1"));


            // ----------- inbuilt functions

            //0 arg, 0 return
            //hmmm, no current like that, but it does work!

            //0 arg, 1 return
            I("reset;");
            I("%v = runif();");
            I("runif();");

            //2 arg, 0 return
            I("reset;");
            I("a = series(1);");
            I("setdomains(a, ('#s',));");

            //1 arg, 1 return
            I("reset;");
            I("%v = log(1);");
            I("log(1);");

            // ----------- inbuilt functions, as object functions

            //2 arg, 0 return
            I("reset;");
            I("a = series(1);");
            I("a.setdomains(('#s',));");

            //1 arg, 1 return
            I("reset;");
            I("%v = 1.log();");
            I("%zz = 1; %zz.log();");

        }

        [TestMethod]
        public void _Test_StackTrace()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\StackTrace';");
            I("RUN p1;");

            Globals.unitTestScreenOutput = new StringBuilder();
            Globals.errorHelper = "1a";
            FAIL("RUN p1;");
            string s = Globals.unitTestScreenOutput.ToString();
            string ss = G.NL +
                @"*** ERROR: ErrorHelper #1a" + G.NL
              + @"*** ERROR: Running file 'c:\Thomas\Gekko\regres\StackTrace\p1.gcm', line 10" + G.NL
              + @"    [  10]:   %v = errorhelper('1a'); //line 10" + G.NL
              + @"" + G.NL
              + @"    Call stack: Command line calling -->" + G.NL
              + @"    c:\Thomas\Gekko\regres\StackTrace\p1.gcm (run-time error in line 10)" + G.NL + G.NL;
            Assert.IsTrue(s == ss);

            // -----

            Globals.unitTestScreenOutput = new StringBuilder();
            Globals.errorHelper = "1b";
            FAIL("RUN p1;");
            s = Globals.unitTestScreenOutput.ToString();
            ss = G.NL +
                @"*** ERROR: ErrorHelper #1b" + G.NL
              + @"*** ERROR: Running file 'c:\Thomas\Gekko\regres\StackTrace\p1.gcm', line 20" + G.NL
              + @"    [  20]:   %v = errorhelper('1b'); //line 20" + G.NL
              + @"" + G.NL
              + @"    Call stack: Command line calling -->" + G.NL
              + @"    c:\Thomas\Gekko\regres\StackTrace\p1.gcm (run-time error in line 20)" + G.NL + G.NL;
            Assert.IsTrue(s == ss);

            // -----

            Globals.unitTestScreenOutput = new StringBuilder();
            Globals.errorHelper = "1c";
            FAIL("RUN p1;");
            s = Globals.unitTestScreenOutput.ToString();
            ss = G.NL +
                @"*** ERROR: ErrorHelper #1c" + G.NL
              + @"*** ERROR: Running file 'c:\Thomas\Gekko\regres\StackTrace\p1.gcm', line 30" + G.NL
              + @"    [  30]:   %v = errorhelper('1c'); //line 30" + G.NL
              + @"" + G.NL
              + @"    Call stack: Command line calling -->" + G.NL
              + @"    c:\Thomas\Gekko\regres\StackTrace\p1.gcm (run-time error in line 30)" + G.NL + G.NL;
            Assert.IsTrue(s == ss);


        }

        [TestMethod]
        public void _Test_DefaultDomains()
        {
            Databank work = First();
            //simplest possible
            I("RESET;");
            I("time 2015 2020;");
            I("x = series(1); p = series(1);");
            I("x[a] = 100; p[a] = 2;");
            I("x[b] = 110; p[b] = 3;");
            I("#i = a, b;");  //not actually used
            I("setdomains(x, ('#i',));");
            I("setdomains(p, ('#i',));");
            I("p<n> x;");
            Table table = Globals.lastPrtOrMulprtTable; Assert.AreEqual(table.GetColMaxNumber(), 3);
            I("p<n> p*x;");
            table = Globals.lastPrtOrMulprtTable; Assert.AreEqual(table.GetColMaxNumber(), 3);
            I("#default = map();");
            I("#default.#i = list('b');");
            I("p<n> x;");
            table = Globals.lastPrtOrMulprtTable; Assert.AreEqual(table.GetColMaxNumber(), 2);
            I("p<n> p*x;");
            table = Globals.lastPrtOrMulprtTable; Assert.AreEqual(table.GetColMaxNumber(), 2);
            I("p<n> x*p;");
            table = Globals.lastPrtOrMulprtTable; Assert.AreEqual(table.GetColMaxNumber(), 2);

            // -----------------------------

            I("reset;");
            I("#s = ('e1', 'e2');");
            I("a = series(1);");
            I("@a = series(1);");
            I("a[#s] = 1;");
            I("@a[e1] = 2;");
            FAIL("p <q> a;  //fejler");
            I("setdomains(a, ('#s',));");            
            I("setdomains(@a, ('#s',));");
            FAIL("p <q> a;  //fejler");
            I("global:#default = map();");
            I("global:#default.#s = ('e1',); ");
            I("p <q> a;");
            table = Globals.lastPrtOrMulprtTable; Assert.AreEqual(table.GetColMaxNumber(), 2);            

        }


        [TestMethod]
        public void _Test_FunctionsInBuilt()
        {
            Databank work = First();
            //simplest possible
            I("RESET;");
            I("VAL %a1 = 1;");
            I("VAL %a2 = 2;");
            I("VAL %a3 = 3;");
            I("VAL %a4 = 4;");
            I("CREATE ts1, ts2, ts3, ts4, ts5, v1;");
            I("TIME 2010 2012;");
            I("SERIES ts1 = 1;");
            I("SERIES ts2 = 2;");
            I("SERIES ts3 = 3;");
            I("SERIES ts4 = 4;");
            I("LIST #m1 = ts1, ts2, ts3, ts4;");
            I("LIST #m2 = ts1, ts2;");
            I("SERIES ts5 = (10, 12, 16);");

            //avg()
            I("VAL %v1 = avg(%a1, %a2);");
            _AssertScalarVal(First(), "%v1", 1.5, sharedDelta);
            I("SERIES xx1 = avg(ts1, ts2);");
            _AssertSeries(First(), "xx1", 2010, 2012, 1.5, sharedDelta);
            I("SERIES xx1 = avg(ts1);");
            _AssertSeries(First(), "xx1", 2010, 2012, 1, sharedDelta);
            I("SERIES xx1 = avg({#m1});");
            _AssertSeries(First(), "xx1", 2010, 2012, 10d / 4d, sharedDelta);

            if (Globals.UNITTESTFOLLOWUP)
            {

                //avgt()
                I("VAL v1 = avgt(ts5);");
                _AssertScalarVal(First(), "%v1", 38d / 3d, sharedDelta);
                I("VAL v1 = avgt(2011, 2012, ts5);");
                AssertHelperScalarVal("v1", 28d / 2d, sharedDelta);

                //sumt()
                I("VAL v1 = sumt(ts5);");
                _AssertScalarVal(First(), "%v1", 38d, sharedDelta);
                I("VAL v1 = sumt(2011, 2012, ts5);");
                _AssertScalarVal(First(), "%v1", 28d, sharedDelta);

            }

            //time()
            I("SER xxtrend = time();");
            _AssertSeries(First(), "xxtrend", 2010, 2010d, sharedDelta);
            _AssertSeries(First(), "xxtrend", 2011, 2011d, sharedDelta);
            _AssertSeries(First(), "xxtrend", 2012, 2012d, sharedDelta);

            //avgt()
            I("SER v1 = avgt(ts5);");
            _AssertSeries(First(), "v1", 2011, 38d / 3d, sharedDelta);
            I("SER v1 = avgt(ts5, 2011, 2012);");
            _AssertSeries(First(), "v1", 2011, 28d / 2d, sharedDelta);
            
            //sumt()
            I("SER v1 = sumt(ts5);");
            _AssertSeries(First(), "v1", 2011, 38d, sharedDelta);            
            I("SER v1 = sumt(ts5, 2011, 2012);");
            _AssertSeries(First(), "v1", 2011, 28d, sharedDelta);            

            //sqrt()
            I("VAL %v1 = sqrt(%a4);");
            _AssertScalarVal(First(), "%v1", 2, sharedDelta);
            I("SERIES xx1 = sqrt(ts4);");
            _AssertSeries(First(), "xx1", 2010, 2012, 2, sharedDelta);

            //sum()
            I("VAL %v1 = sum(%a1, %a2);");
            _AssertScalarVal(First(), "%v1", 3, sharedDelta);
            I("SERIES xx1 = sum(ts1, ts2);");
            _AssertSeries(First(), "xx1", 2010, 2012, 3, sharedDelta);
            I("SERIES xx1 = avg(ts1);");
            _AssertSeries(First(), "xx1", 2010, 2012, 1, sharedDelta);
            I("SERIES xx1 = sum({#m1});");
            _AssertSeries(First(), "xx1", 2010, 2012, 10d, sharedDelta);
            //To make sure that it is not confused with sum(#i, x[#i]) stuff
            I("SERIES xx1 = sum({#m1}, {#m2});");            
            _AssertSeries(First(), "xx1", 2010, 2012, 13d, sharedDelta);
            //To make sure that it is not confused with sum(#i, x[#i]) stuff
            I("SERIES xx1 = sum({#m1}, ts1);");
            _AssertSeries(First(), "xx1", 2010, 2012, 11d, sharedDelta);
            I("RESET; TIME 2000 2000; MODE data;");
            I("open<edit> xx;");
            I("ser xx1 = 1;");
            I("ser xx2 = 2;");            
            I("close xx;");
            I("open xx;");
            I("list #xx = xx1, xx2;");
            I("ser yy = sum({#xx});");  //test that timeseries are searched for in data mode
            _AssertSeries(First(), "yy", 2000, 3d, sharedDelta);

            I("RESET;");
            I("OPEN <edit> b1;");
            I("OPEN <edit> b2;");
            I("STRING %b1 = bankname('first');");
            I("STRING %b0 = bankname('ref');");
            I("STRING %b1a = bankname(1);");
            I("STRING %b2 = bankname(2);");
            I("STRING %b3 = bankname(3);");
            I("VAL %v = bankname(0);");
            _AssertScalarString(First(), "%b1", "b2");
            _AssertScalarString(First(), "%b1a", "b2");
            _AssertScalarString(First(), "%b0", "Ref");            
            _AssertScalarString(First(), "%b2", "b1");
            _AssertScalarString(First(), "%b3", "Work");
            _AssertScalarVal(First(), "%v", 3d);

            //percentile()
            I("RESET;");
            I("CREATE a;");
            I("TIME 2000 2003;");
            I("SERIES a =  (1, 4, 7, 2);");
            I("VAL %v = percentile(a, 0.0);");
            _AssertScalarVal(First(), "%v", 1.0d, sharedDelta);  //same as min
            I("VAL %v = percentile(a, 0.1);");
            _AssertScalarVal(First(), "%v", 1.3d, sharedDelta);
            I("VAL %v = percentile(a, 0.2);");
            _AssertScalarVal(First(), "%v", 1.6d, sharedDelta);
            I("VAL %v = percentile(a, 0.3);");
            _AssertScalarVal(First(), "%v", 1.9d, sharedDelta);
            I("VAL %v = percentile(a, 0.4);");
            _AssertScalarVal(First(), "%v", 2.4d, sharedDelta);
            I("VAL %v = percentile(a, 0.5);");
            _AssertScalarVal(First(), "%v", 3.0d, sharedDelta);  //same as median
            I("VAL %v = percentile(a, 0.6);");
            _AssertScalarVal(First(), "%v", 3.6d, sharedDelta);
            I("VAL %v = percentile(a, 0.7);");
            _AssertScalarVal(First(), "%v", 4.3d, sharedDelta);
            I("VAL %v = percentile(a, 0.8);");
            _AssertScalarVal(First(), "%v", 5.2d, sharedDelta);
            I("VAL %v = percentile(a, 0.9);");
            _AssertScalarVal(First(), "%v", 6.1d, sharedDelta);
            I("VAL %v = percentile(a, 1.0);");
            _AssertScalarVal(First(), "%v", 7.0d, sharedDelta);  //same as max

            //percentile()
            I("RESET; TIME 2000 2002;");
            I("VAL %v = m();");
            I("SER xx = (m(), 1, m());");
            I("VAL %v1 = 0;");
            I("VAL %v2 = 0;");
            I("IF(ismiss(%v)==1); VAL %v1 = 1; END;");
            I("IF(ismiss(xx[2002])==1); VAL %v2 = 1; END;");
            _AssertScalarVal(First(), "%v1", 1.0d, sharedDelta);
            _AssertScalarVal(First(), "%v2", 1.0d, sharedDelta);


        }
        
        [TestMethod]
        public void Test__FunctionsUserDefined()
        {
            Databank work = First();
            //simplest possible
            I("RESET;");
            I("function val f(val x); return %x*%x; end; val y = f(3+1);");
            //val y = f(4);
            _AssertScalarVal(First(), "%y", 16);

            //simplest possible, with same-name x
            I("RESET;");
            I("function val f(val x); return %x*%x; end; val x = 5; val y = f(3+1);");
            //val y = f(4);
            _AssertScalarVal(First(), "%x", 5);
            _AssertScalarVal(First(), "%y", 16);

            //normal and nested calls with two params
            I("RESET;");
            I("function val f(val x, val y); return %x*%y; end; val q1 = 3; val q2 = 4; val q3 = f(%q1, %q2); val q4 = f(f(%q1, %q2)+0, f(%q1, %q2));");
            //val q3 = f(%q1, %q2); val q4 = f(f(%q1, %q2), f(%q1, %q2));
            _AssertScalarVal(First(), "%q3", 12);
            _AssertScalarVal(First(), "%q4", 144);

            I("RESET;");
            //2 return values, 1 param
            I("function (val, val) f(val x); return (2*%x,3*%x); end; (val x1, val x2) = f(7);");
            //(val x1, val x2) = f(7);
            _AssertScalarVal(First(), "%x1", 14);
            _AssertScalarVal(First(), "%x2", 21);

            I("RESET;");
            //5 return values of different type, 0 param
            I("function (val, string, date, list, series) f(); list _temp = q1, q2; create _newvar; SERIES<2010 2011>_newvar = 777; return (123.45, 'abc', 2010a1, #_temp, _newvar); end; time 2009 2012; create x4; (val x0, string x1, date x2, list x3, series <2009 2012> x4) = f();");
            //(val x1, val x2) = f(7);
            _AssertScalarVal(First(), "%x0", 123.45);
            _AssertScalarString(First(), "%x1", "abc");
            _AssertScalarDate(First(), "%x2", EFreq.A, 2010, 1);
            List<string> temp = _GetListOfStrings("x3");
            Assert.AreEqual(temp.Count, 2);
            Assert.AreEqual(temp[0], "q1");
            Assert.AreEqual(temp[1], "q2");
            _AssertSeries(First(), "x4", 2009, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4", 2010, 777, sharedDelta);
            _AssertSeries(First(), "x4", 2011, 777, sharedDelta);
            _AssertSeries(First(), "x4", 2012, double.NaN, sharedDelta);
            //FIXME: _newvar should not exist afterwards

            I("RESET;");
            //3 return values, 0 params
            I("function (val, val, val) f(); val x = 5; return (2*%x, 3*%x, 4*%x); end; (val x1, val x2, val x3) = f();");
            //(val x1, val x2, val x3) = f();
            _AssertScalarVal(First(), "%x1", 10);
            _AssertScalarVal(First(), "%x2", 15);
            _AssertScalarVal(First(), "%x3", 20);
            //-----------------------------------------
            //---- FIXME: this is wrong, but right ----
            //-----------------------------------------
            _AssertScalarVal(First(), "%x", 5);

            //3 return values, 1 2-tuple params + 2 return values, 0 params.
            //Shows how tuples can feed into each other via f(g()), where g() returns a 2-tuple.
            I("RESET;");
            I("function (val, val, val) f((val x, val y)); return (200*%x, 300*%x, 400*%y); end; function (val, val) g(); return (2, 3); end; (val x1, val x2, val x3) = f(g());");
            //(val x1, val x2, val x3) = f(g());
            _AssertScalarVal(First(), "%x1", 400);
            _AssertScalarVal(First(), "%x2", 600);
            _AssertScalarVal(First(), "%x3", 1200);

            //Using a list() function that functions as a container
            //And calling f() recursively on itself (linear process) like this:
            //f(f(list(0.5, 0.8)))
            //Also tested that a free-floating tuple works:
            //f(f((0.5, 0.8)))
            //Same as before, just without the 'list'
            //Note that (val x, val y) = (1, 2) is not allowed: tuples are not thought as vectors! only used as function arguments and return values.
            I("RESET;");
            I("function (val, val) list(val x1, val x2); return (%x1, %x2); end; function (val, val) f((val x1, val x2)); return (0.4*%x1+0.2*%x2+3, 0.7*%x2+0.1*%x1+1); end; (val x1, val x2) = f(list(0.5, 0.8)); (val y1, val y2) = f(f(list(0.5, 0.8))); (val xx1, val xx2) = f((0.5, 0.8)); (val yy1, val yy2) = f(f((0.5, 0.8)));");
            //(val x1, val x2) = f(list(0.5, 0.8)); (val y1, val y2) = f(f(list(0.5, 0.8))); (val xx1, val xx2) = f((0.5, 0.8)); (val yy1, val yy2) = f(f((0.5, 0.8)));
            _AssertScalarVal(First(), "%x1", 3.36d, 1e-8d);
            _AssertScalarVal(First(), "%x2", 1.61d, 1e-8d);
            _AssertScalarVal(First(), "%y1", 4.666d, 1e-8d);
            _AssertScalarVal(First(), "%y2", 2.463d, 1e-8d);
            _AssertScalarVal(First(), "%xx1", 3.36d, 1e-8d);
            _AssertScalarVal(First(), "%xx2", 1.61d, 1e-8d);
            _AssertScalarVal(First(), "%yy1", 4.666d, 1e-8d);
            _AssertScalarVal(First(), "%yy2", 2.463d, 1e-8d);

            // -----------------------------------------------------------
            // ----------- hiding/shadowing, and local/global variables
            // -----------------------------------------------------------

            //testing access of global variable (%z)
            I("RESET;");
            I("function val f(val x, val y); return %x*%y + %z; end; val z = 100; val q = f(3, 4+0);");
            //val q = f(3, 4);
            _AssertScalarVal(First(), "%q", 112);
            _AssertScalarVal(First(), "%z", 100);

            //local param shadows/hides global variable (%z)
            I("RESET;");
            I("function val f(val x, val y, val z); return %x*%y + %z; end; val z = 100; val q = f(3, 4, 1000+0);");
            //val q = f(3, 4, 1000);
            _AssertScalarVal(First(), "%q", 1012);
            _AssertScalarVal(First(), "%z", 100);  //is not changed, since %z is a parameter

            //local param shadows/hides global variable (%z)
            I("RESET;");
            I("function val f(val x, val y); val z = 1000; return %x*%y + %z; end; val z = 100; val q = f(3, 4+0);");
            //val q = f(3, 4);
            _AssertScalarVal(First(), "%q", 1012);
            //-----------------------------------------
            //---- FIXME: this is wrong, but right ----
            //-----------------------------------------
            _AssertScalarVal(First(), "%z", 1000);

            // -----------------------------------------------------------
            // ----------- dates -----------------------------------------
            // -----------------------------------------------------------

            I("RESET;");
            I("function date f(date x); return %x+3; end; date y = f(2000a1+0);");
            //date y = f(2000a1);
            _AssertScalarDate(First(), "%y", EFreq.A, 2003, 1);

            // -----------------------------------------------------------
            // ----------- strings ---------------------------------------
            // -----------------------------------------------------------

            I("RESET;");
            I("function string f(string x); return %x+'shine'; end; string y = f('sun');");
            //string y = f('sun');
            _AssertScalarString(First(), "%y", "sunshine");

            // -----------------------------------------------------------
            // ----------- lists -----------------------------------------
            // -----------------------------------------------------------

            I("RESET;");
            I("function list f(list x, list y); return union(#x, #y); end; list xx=x1,x2; list yy=y1, y2;list z = f(#xx, #yy);");
            //list z = f(#xx, #yy);
            List<string> z = _GetListOfStrings("z");

            Assert.AreEqual(CountListOfStrings(), 3);  //so we are sure there are no #x and #y lists
            Assert.AreEqual(z.Count, 4);
            Assert.AreEqual(z[0], "x1");
            Assert.AreEqual(z[1], "x2");
            Assert.AreEqual(z[2], "y1");
            Assert.AreEqual(z[3], "y2");

            // -----------------------------------------------------------
            // ----------- matrix ----------------------------------------
            // -----------------------------------------------------------

            I("RESET;");
            I("function matrix multiply(matrix x, matrix y); return #x * #y; end; matrix a = [1, 2 || 3, 4]; matrix b = [9, 8 || 7, 6]; matrix z = [0, 0 || 0, 0]; matrix c = multiply(#a, #b+#z);");
            _AssertMatrix(First(), "c", "rows", 2);
            _AssertMatrix(First(), "c", "cols", 2);
            _AssertMatrix(First(), "c", 1, 1, 23d, 0d);
            _AssertMatrix(First(), "c", 1, 2, 20d, 0d);
            _AssertMatrix(First(), "c", 2, 1, 55d, 0d);
            AssertHelperMatrix("c", 2, 2, 48d, 0d);
            
            Assert.AreEqual(z[0], "x1");
            Assert.AreEqual(z[1], "x2");
            Assert.AreEqual(z[2], "y1");
            Assert.AreEqual(z[3], "y2");

            // -----------------------------------------------------------
            // ----------- series ----------------------------------------
            // -----------------------------------------------------------

            I("RESET;");
            I("TIME 2000 2001;");
            I("function series f(series x, series y); return x*y; end; VAL v = 0; create xx, yy, zz; SERIES xx= 2; SERIES yy = 3; SERIES zz = f(xx, yy+%v);");
            //SERIES zz = f(xx, yy);
            UData u;
            u = Data("xx", 2000, "a"); Assert.AreEqual(u.w, 2);
            u = Data("xx", 2001, "a"); Assert.AreEqual(u.w, 2);
            u = Data("yy", 2000, "a"); Assert.AreEqual(u.w, 3);
            u = Data("yy", 2001, "a"); Assert.AreEqual(u.w, 3);
            u = Data("zz", 2000, "a"); Assert.AreEqual(u.w, 6);
            u = Data("zz", 2001, "a"); Assert.AreEqual(u.w, 6);

            I("RESET;");
            I("TIME 2000 2001;");
            I("function series f(series x, series y); return x*y; end; VAL v = 0; create xx, yy, zz; SERIES xx= 2; SERIES yy = 3; SERIES zz = f(f(xx+%v, yy+%v), yy+%v);");
            //nested use of series function
            //SERIES zz = f(f(xx, yy) yy);
            u = Data("xx", 2000, "a"); Assert.AreEqual(u.w, 2);
            u = Data("xx", 2001, "a"); Assert.AreEqual(u.w, 2);
            u = Data("yy", 2000, "a"); Assert.AreEqual(u.w, 3);
            u = Data("yy", 2001, "a"); Assert.AreEqual(u.w, 3);
            u = Data("zz", 2000, "a"); Assert.AreEqual(u.w, 18);
            u = Data("zz", 2001, "a"); Assert.AreEqual(u.w, 18);

            I("RESET;");
            I("TIME 2000 2001;");
            I("function (series, series) f(series x, series y); return (x*y, x*x); end; create xx, yy, zz1, zz2; SERIES xx= 2; SERIES yy = 3; (series zz1, series zz2) = f(xx, yy);");
            _AssertSeries(First(), "zz1", 2000, 6, sharedDelta);
            _AssertSeries(First(), "zz1", 2001, 6, sharedDelta);
            _AssertSeries(First(), "zz2", 2000, 4, sharedDelta);
            _AssertSeries(First(), "zz2", 2001, 4, sharedDelta);

        }

        private static int CountListOfStrings()
        {
            int count = 0;
            foreach (KeyValuePair<string, IVariable> kvp in Program.databanks.GetFirst().storage)
            {
                if (kvp.Value.Type() == EVariableType.List)
                {
                    count++;
                }
            }
            return count;
        }

        private static void AssertHelperScalarVal(string s, double d)
        {
            AssertHelperScalarVal(s, d, 0d);
        }

        private static void AssertHelperMatrix(string s, int i, int j, double d, double delta)
        {
            Matrix m = (Matrix)Program.databanks.GetFirst().GetIVariable(Globals.symbolCollection + s);
            Assert.AreEqual(m.data[i - 1, j - 1], d, delta);
        }

        private static void AssertHelperMatrix(string s, string dim, int i)
        {
            int dim2 = 0;
            if (dim == "cols") dim2 = 1;
            else if (dim == "rows") ;
            else throw new GekkoException();
            Matrix m = (Matrix)Program.databanks.GetFirst().GetIVariable(Globals.symbolCollection + s);
            Assert.AreEqual(m.data.GetLength(dim2), i);
        }

        private static void AssertHelperScalarVal(string s, double d, double delta)
        {
            double d2 = ((ScalarVal)Program.databanks.GetFirst().GetIVariable(s)).val;
            _AssertHelperTwoDoubles(d2, d, delta);
        }

        private static void AssertHelperScalarDate(string s, EFreq freq, int super, int sub)
        {
            Assert.AreEqual(((ScalarDate)Program.databanks.GetFirst().GetIVariable(s)).date.freq, freq);
            Assert.AreEqual(((ScalarDate)Program.databanks.GetFirst().GetIVariable(s)).date.super, super);
            Assert.AreEqual(((ScalarDate)Program.databanks.GetFirst().GetIVariable(s)).date.sub, sub);            
        }

        private static void AssertHelperScalarString(string s, string s2)
        {
            Assert.AreEqual(((ScalarString)Program.databanks.GetFirst().GetIVariable(s)).string2, s2);
        }

        [TestMethod]
        public void _Test_Disp()
        {
            I("RESET;");
            I("CREATE ts;");
            I("TIME 2010 2012;");
            I("SERIES ts = 1.23456;");
            I("SERIES<2011 2012>ts %= 5.6789;");
            I("TIME 2009 2013;");
            I("DISP ts;");
            //string s = "";
            //s += "                      ts  %" + G.NL;
            //s += "2009              M  ******" + G.NL;
            //s += "2010         1.2346  ******" + G.NL;
            //s += "2011         1.3047    5.68" + G.NL;
            //s += "2012         1.3788    5.68" + G.NL;
            //s += "2013              M  ******" + G.NL;
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.2346"));  //stupid test, must be done better...
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3047"));
            //Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3788"));  //this is hidden for view
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("5.68"));
        }

        [TestMethod]
        public void Test__Info()
        {
            //Does not test the result, just that it parses
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("MODEL jul05;");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("DISP<2010 2010 info>fy;");
            FAIL("DISP<2010 2011 info>fy;");
        }

        [TestMethod]
        public void _Test_Itershow()
        {
            //Does not test the result, just that it parses
            I("RESET;");
            I("OPTION solve gauss dump = yes;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("MODEL jul05;");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("SIM<2006 2006>;");
            I("ITERSHOW <2010 2010>fy;");
        }

        [TestMethod]
        public void _Test_Mulbk()
        {
            //TODO:
            //We really should have a READ<work>..., READ<ref>..., the last one is MULBK...
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("READ jul05tsdx;");
            UData u = Data("fy", 2006, "a"); Assert.AreEqual(u.w, u.b); double fy0 = u.w;
            I("SERIES<2006 2006>fy += 100;");
            u = Data("fy", 2006, "a"); Assert.AreEqual(u.w, u.b + 100); Assert.AreEqual(u.w, fy0 + 100); //100 higher in work
            I("READ <ref> jul05tsdx;");
            u = Data("fy", 2006, "a"); Assert.AreEqual(u.w, u.b + 100); Assert.AreEqual(u.w, fy0 + 100);  //still 100 higher in work
            I("CLONE;");
            u = Data("fy", 2006, "a"); Assert.AreEqual(u.w, u.b); Assert.AreEqual(u.w, fy0 + 100); //both 100 higher now
        }

        [TestMethod]
        public void _Test_Sys()
        {
            I("RESET;");
            I("SYS 'di'+'r';");  //SYS 'dir' is ok            
        }

        [TestMethod]
        public void _Test_Tell()
        {
            I("RESET;");
            I("TELL<nocr>'hel'+'lo1';");
            I("TELL 'hel'+'lo2';");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("hello1hello2"));
        }

        

        [TestMethod]
        public void _Test_NameComposition()
        {
            //-----------------------------
            //Check of SERIES x = y
            //-----------------------------
            Databank work = First();
            I("RESET;");
            I("TIME 2000 2005;");
            I("CREATE x;");
            I("CREATE y;");
            I("SERIES x = 2;");
            I("SERIES y = 3;");
            I("SERIES x = y;");
            _AssertSeries(First(), "x", 2000, 2005, 3, sharedDelta);
            _AssertSeries(First(), "y", 2000, 2005, 3, sharedDelta);

            //-----------------------------
            //Check of SERIES x = %y, were %y is a scalar
            //-----------------------------
            I("RESET;");
            I("TIME 2000 2005;");
            I("CREATE x;");
            I("VAL %y = 3;");
            I("SERIES x = %y;");
            _AssertSeries(First(), "x", 2000, 2005, 3, sharedDelta);

            //-----------------------------
            //Check of SERIES x = %y, where %y is a string, val, date
            //-----------------------------
            I("RESET;");
            I("TIME 2000 2005;");
            I("CREATE x;");
            I("CREATE a;");
            I("SERIES a = 3;");
            I("STRING %y = 'a';");
            I("VAL %z = 4;");
            I("DATE %d = 2000;");
            I("SERIES x = {%y};");
            _AssertSeries(First(), "x", 2000, 2005, 3, sharedDelta);
            I("SERIES x = %z;");
            _AssertSeries(First(), "x", 2000, 2005, 4, sharedDelta);

            FAIL("SERIES x = %d;"); //Must not accept date here

            //-----------------------------
            //Check of all combinations of a, %b, {%c}, including {c} as alias for {%c}
            //-----------------------------
            I("RESET;");
            I("TIME 2000 2005;");
            I("STRING %b = 'x';");
            I("STRING %c = 'y';");
            I("VAL %d = 123;");
            I("CREATE zz;");
            I("CREATE a;"); I("SERIES a = 3;");    //a=3
            I("CREATE x;"); I("SERIES x = 4;");    //x=4
            I("CREATE y;"); I("SERIES y = 5;");    //y=5
            I("CREATE ax;"); I("SERIES ax = 6;");  //ax=6
            I("CREATE ay;"); I("SERIES ay = 7;");  //ay=7
            I("CREATE xa;"); I("SERIES xa = 8;");  //xa=8
            I("CREATE xx;"); I("SERIES xx = 9;");  //xx=9
            I("CREATE xy;"); I("SERIES xy = 10;"); //xy=10
            I("CREATE ya;"); I("SERIES ya = 11;"); //ya=11
            I("CREATE yx;"); I("SERIES yx = 12;"); //yx=12
            I("CREATE yy;"); I("SERIES yy = 13;"); //yx=13
            I("SERIES zz = a;"); _AssertSeries(First(), "zz", 2000, 2005, 3, sharedDelta);
            FAIL("SERIES zz = %b;");
            I("SERIES zz = {%c};"); _AssertSeries(First(), "zz", 2000, 2005, 5, sharedDelta);
            FAIL("SERIES zz = {c};"); //_AssertSeries(First(), "zz", 2000, 2005, 5, sharedDelta);
            I("SERIES zz = %d;"); _AssertSeries(First(), "zz", 2000, 2005, 123, sharedDelta);
            FAIL("SERIES zz = {%d};");

            I("SERIES zz = a%b;"); _AssertSeries(First(), "zz", 2000, 2005, 6, sharedDelta);
            I("SERIES zz = a{%c};"); _AssertSeries(First(), "zz", 2000, 2005, 7, sharedDelta);
            FAIL("SERIES zz = a{c};"); //_AssertSeries(First(), "zz", 2000, 2005, 7, sharedDelta);

            //To do this, setting a scalar first, you must use {...}
            //Using %(...) is something completely different
            FAIL("SERIES zz = %b|a;"); //_AssertSeries(First(), "zz", 2000, 2005, 8, sharedDelta);
            FAIL("SERIES zz = %b%b;"); //_AssertSeries(First(), "zz", 2000, 2005, 9, sharedDelta);
            FAIL("SERIES zz = %b{%c};"); //_AssertSeries(First(), "zz", 2000, 2005, 10, sharedDelta);
            FAIL("SERIES zz = %b{c};"); //_AssertSeries(First(), "zz", 2000, 2005, 10, sharedDelta);

            I("SERIES zz = {%c}a;"); _AssertSeries(First(), "zz", 2000, 2005, 11, sharedDelta);
            FAIL("SERIES zz = {c}a;"); //_AssertSeries(First(), "zz", 2000, 2005, 11, sharedDelta);
            I("SERIES zz = {%c}%b;"); _AssertSeries(First(), "zz", 2000, 2005, 12, sharedDelta);
            FAIL("SERIES zz = {c}%b;"); //_AssertSeries(First(), "zz", 2000, 2005, 12, sharedDelta);
            I("SERIES zz = {%c}{%c};"); _AssertSeries(First(), "zz", 2000, 2005, 13, sharedDelta);
            FAIL("SERIES zz = {c}{c};"); //_AssertSeries(First(), "zz", 2000, 2005, 13, sharedDelta);

            //-----------------------------
            //Recursive
            //Check of SERIES x = %(%s), a%(%s)b, a{%(%s)}b, a{%({'a'+'b'})}b;
            //-----------------------------
            I("RESET;");
            I("TIME 2000 2005;");
            I("CREATE x1, x2, x3, x4;");
            I("CREATE y, ayb;");
            I("SERIES y = 3;");
            I("SERIES ayb = 3;");
            I("STRING %s = 's1';");
            I("STRING %s1 = 'y';");
            I("STRING %ab = 'y';");
            FAIL("SERIES x1 = {%(%s)};");
            I("SERIES x1 = {%{%s}};");
            _AssertSeries(First(), "x1", 2000, 2005, 3, sharedDelta);
            FAIL("SERIES x2 = a%(%s)b;");
            FAIL("SERIES x2 = a%{%s}b;");  //looks for %s1b, fair enough, needs '|'
            I("SERIES x2 = a%{%s}|b;");
            _AssertSeries(First(), "x2", 2000, 2005, 3, sharedDelta);
            FAIL("SERIES x3 = a{%(%s)}b;");
            I("SERIES x3 = a{%{%s}}b;");
            _AssertSeries(First(), "x3", 2000, 2005, 3, sharedDelta);
            FAIL("SERIES x4 = a{%({'a'+'b'})}b;");  //also ayb
            I("SERIES x4 = a{%{'a'+'b'}}b;");  //also ayb
            _AssertSeries(First(), "x4", 2000, 2005, 3, sharedDelta);

            if (Globals.UNITTESTFOLLOWUP)
            {
                //List def is more stringent now
                //LIST
                List<string> x = null;
                I("RESET;");
                I("STRING %s = 'u';");
                I("LIST #x = a, b, c:d, 'e', 'f:g', %s, %s+%s, {%s}, {s}, {s}:{s}, %s+':'+%s, a%s:%s|a;");
                x = _GetListOfStrings("x"); Assert.AreEqual(x.Count, 12); Assert.AreEqual(x[0], "a"); Assert.AreEqual(x[1], "b"); Assert.AreEqual(x[2], "c:d"); Assert.AreEqual(x[3], "e"); Assert.AreEqual(x[4], "f:g"); Assert.AreEqual(x[5], "u"); Assert.AreEqual(x[6], "uu"); Assert.AreEqual(x[7], "u"); Assert.AreEqual(x[8], "u"); Assert.AreEqual(x[9], "u:u"); Assert.AreEqual(x[10], "u:u"); Assert.AreEqual(x[11], "au:ua");
                I("CREATE a, abc, abbc, adc, addc;");
                I("LIST x = [*];");
                x = _GetListOfStrings("x"); Assert.AreEqual(x.Count, 5); Assert.AreEqual(x[0], "a"); Assert.AreEqual(x[1], "abbc"); Assert.AreEqual(x[2], "abc"); Assert.AreEqual(x[3], "adc"); Assert.AreEqual(x[4], "addc");
                I("LIST x = [a*];");
                x = _GetListOfStrings("x"); Assert.AreEqual(x.Count, 5); Assert.AreEqual(x[0], "a"); Assert.AreEqual(x[1], "abbc"); Assert.AreEqual(x[2], "abc"); Assert.AreEqual(x[3], "adc"); Assert.AreEqual(x[4], "addc");
                I("LIST x = [*c];");
                x = _GetListOfStrings("x"); Assert.AreEqual(x.Count, 4); Assert.AreEqual(x[0], "abbc"); Assert.AreEqual(x[1], "abc"); Assert.AreEqual(x[2], "adc"); Assert.AreEqual(x[3], "addc");
                I("LIST x = [?];");
                x = _GetListOfStrings("x"); Assert.AreEqual(x.Count, 1); Assert.AreEqual(x[0], "a");
                I("LIST x = [a?c];");
                x = _GetListOfStrings("x"); Assert.AreEqual(x.Count, 2); Assert.AreEqual(x[0], "abc"); Assert.AreEqual(x[1], "adc");
            }

        }

        private static void FAIL(string s)
        {
            // NOTE this logic:
            //-------------------------------------
            // I("TELL 'adsf';");            Passed
            // I("TELL 'adsf;");             Failed
            // FAIL("TELL 'adsf';");         Failed
            // FAIL("TELL 'adsf;");          Passed
            //-------------------------------------

            Globals.lastPrtOrMulprtTable = null;

            bool b = false;
            try
            {
                I(s);  //fail
                b = true;
                //Assert.Fail(); // Must not accept this ---> hmmm this does not work: Assert.Fail() will fail itself.....
            }
            catch (Exception) {
                //By calling this, we emulate the cleanup done in the 'real' Gekko when exceptions are thrown
                Program.GekkoExceptionCleanup(new P());  //new P() is a bit of a hack to make it work here (I() method does the same thing with new P())
            };
            if (b) throw new GekkoException();
        }

        private static void _AssertListVal(IBank db, string s, int i, double x, double delta)
        {
            List iv_list = db.GetIVariable(s) as List;
            ScalarVal sv = iv_list.list[i - 1] as ScalarVal;
            _AssertHelperTwoDoubles(x, sv.val, sharedDelta);
        }

        private static void _AssertListString(IBank db, string s, int i, string x)
        {
            List iv_list = db.GetIVariable(s) as List;
            ScalarString ss = iv_list.list[i - 1] as ScalarString;
            Assert.AreEqual(x, ss.string2);
        }            

        private static void _AssertListSize(IBank db, string s, int n)
        {
            List iv_list = db.GetIVariable(s) as List;            
            Assert.AreEqual(n, iv_list.list.Count);
        }

        private static void _AssertMatrix(IBank db, string s, int i, int j, double d, double delta)
        {
            Matrix m = db.GetIVariable(s) as Matrix;
            _AssertHelperTwoDoubles(d, m.data[i - 1, j - 1], delta);
        }

        private static void _AssertMatrix(IBank db, string s, string rowscols, double d)
        {
            Matrix m = db.GetIVariable(s) as Matrix;
            if (G.Equal(rowscols, "rows"))
            {
                Assert.AreEqual(d, m.data.GetLength(0));
            }
            else if (G.Equal(rowscols, "cols"))
            {
                Assert.AreEqual(d, m.data.GetLength(1));
            }
            else throw new GekkoException();
        }

        private static void _AssertScalarString(IBank db, string s, string s2)
        {
            ScalarString ss = db.GetIVariable(s) as ScalarString;
            Assert.AreEqual(ss.string2, s2);
        }

        private static void _AssertScalarVal(IBank db, string s, double d)
        {
            ScalarVal sv = db.GetIVariable(s) as ScalarVal;
            Assert.AreEqual(sv.val, d);            
        }

        private static void _AssertScalarVal(IBank db, string s, double d, double delta)
        {
            ScalarVal sv = db.GetIVariable(s) as ScalarVal;            
            _AssertHelperTwoDoubles(sv.val, d, delta);
        }

        private static void _AssertScalarDate(IBank db, string s, EFreq freq, int super, int sub)
        {
            ScalarDate sd = db.GetIVariable(s) as ScalarDate;
            Assert.AreEqual(sd.date.freq, freq);
            Assert.AreEqual(sd.date.super, super);
            Assert.AreEqual(sd.date.sub, sub);
        }


        private static void _AssertSeries(IBank db, string s, int year, double x, double delta)
        {
            _AssertSeries(db, s, null, EFreq.A, year, 1, year, 1, x, delta);
        }

        private static void _AssertSeries(IBank db, string s, int year1, int year2, double x, double delta)
        {
            _AssertSeries(db, s, null, EFreq.A, year1, 1, year2, 1, x, delta);
        }

        private static void _AssertSeries(IBank db, string s, EFreq freq, int year, int subper, double x, double delta)
        {
            _AssertSeries(db, s, null, freq, year, subper, year, subper, x, delta);
        }

        private static void _AssertSeries(IBank db, string s, string[] indexes, int year, double x, double delta)
        {
            _AssertSeries(db, s, indexes, EFreq.A, year, 1, year, 1, x, delta);
        }

        private static void _AssertSeries(IBank db, string s, string[] indexes, int year1, int year2, double x, double delta)
        {
            _AssertSeries(db, s, indexes, EFreq.A, year1, 1, year2, 1, x, delta);
        }

        private static void _AssertSeries(IBank db, string s, string[] indexes, EFreq freq, int year, int subper, double x, double delta)
        {
            _AssertSeries(db, s, indexes, freq, year, subper, year, subper, x, delta);
        }

        private static void _AssertSeries(IBank db, string s2, string[] indexes, EFreq freq, int year1, int sub1, int year2, int sub2, double x, double delta)
        {
            string s = G.Chop_FreqAdd(s2, Program.options.freq);

            GekkoTime t1 = new GekkoTime(freq, year1, sub1);
            GekkoTime t2 = new GekkoTime(freq, year2, sub2);

            if (t1.StrictlyLargerThan(t2)) throw new GekkoException();
            
            Series ts = null;
            
            if (indexes != null)
            {
                Series tsGhost = db.GetIVariable(s) as Series;
                if (tsGhost.type != ESeriesType.ArraySuper) throw new GekkoException();
                if (tsGhost.dimensions == 0) throw new GekkoException();
                IVariable iv = null; tsGhost.dimensionsStorage.TryGetValue(new MapMultidimItem(indexes), out iv);
                ts = iv as Series;
            }
            else
            {
                ts = db.GetIVariable(s) as Series;
                if (ts.type == ESeriesType.ArraySuper)
                    throw new GekkoException();
                if (ts.dimensions != 0)
                    throw new GekkoException(); //should be caught above              
            }            

            foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
            {
                double y = ts.GetDataSimple(t);
                _AssertHelperTwoDoubles(x, y, delta);
            }
        }


        //private static void AssertHelper(Databank db, string s, int year, double x, double delta)
        //{
        //    AssertHelper(db, s, null, EFreq.A, year, 1, year, 1, x, delta);
        //}
        
        //private static void AssertHelper(Databank db, string s, int year1, int year2, double x, double delta)
        //{
        //    AssertHelper(db, s, null, EFreq.A, year1, 1, year2, 1, x, delta);
        //}

        //private static void AssertHelper(Databank db, string s, EFreq freq, int year, int subper, double x, double delta)
        //{
        //    AssertHelper(db, s, null, freq, year, subper, year, subper, x, delta);
        //}
        
        //private static void AssertHelper(Databank db, string s, string[] indexes, int year, double x, double delta)
        //{
        //    AssertHelper(db, s, indexes, EFreq.A, year, 1, year, 1, x, delta);
        //}

        //private static void AssertHelper(Databank db, string s, string[] indexes, int year1, int year2, double x, double delta)
        //{
        //    AssertHelper(db, s, indexes, EFreq.A, year1, 1, year2, 1, x, delta);
        //}

        //private static void AssertHelper(Databank db, string s, string[] indexes, EFreq freq, int year, int subper, double x, double delta)
        //{
        //    AssertHelper(db, s, indexes, freq, year, subper, year, subper, x, delta);
        //}

        //private static void AssertHelper(Databank db, string s, string[] indexes, EFreq freq, int year1, int sub1, int year2, int sub2, double x, double delta)
        //{
        //    GekkoTime t1 = new GekkoTime(freq, year1, sub1);
        //    GekkoTime t2 = new GekkoTime(freq, year2, sub2);

        //    if (t1.StrictlyLargerThan(t2)) throw new GekkoException();

        //    IVariable[] indexes2 = null;

        //    string name = null;

        //    if (indexes != null)
        //    {
        //        name = s + Globals.symbolTurtle + Series.GetHashCodeFromIvariables(indexes);
        //    }
        //    else
        //    {
        //        name = s;
        //    }

        //    Series ts = db.GetVariable(freq, name);

        //    foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
        //    {
        //        double y = ts.GetDataNonLight(t);
        //        _AssertHelperTwoDoubles(x, y, delta);
        //    }
        //}

        private static void _AssertHelperTwoDoubles(double x, double y, double delta)
        {
            if (NaNCheck(x, y))
            {
                Assert.Fail("Missing: should be " + x.ToString() + ", but is " + y.ToString());
            }
            else if (G.isNumericalError(x) && G.isNumericalError(y))
            {
                //both are NaN which is ok
            }
            else
            {
                try
                {
                    Assert.AreEqual(y, x, delta);
                }
                catch
                {
                    //breakpoint can be inserted below
                    throw;
                }
            }
        }

        private static bool NaNCheck(double x, double y)
        {
            bool nanCheck = (G.isNumericalError(x) && !G.isNumericalError(y)) || (!G.isNumericalError(x) && G.isNumericalError(y));
            return nanCheck;
        }        

        [TestMethod]
        public void _Test_Table()
        {
            //table tablesmall

            //just a test of syntax
            I("RESET;");
            I("table xx = new table();");
            I("table xx.currow.setdates(1,2000,2010);");
            I("table xx.currow.next();");
            I("table xx.currow.setvalues(1,2000,2010,1,'n',0.001,'f10.3');");
            I("table xx.currow.next();");
            I("table xx.currow.setvalues(1,2000,2010,1,'n',0.001,'f10.3');");
            I("table xx.currow.mergecols(3, 4);");
            I("table xx.currow.aligncenter(3);");
            I("table xx.currow.settopborder(2, 3);");
            I("table xx.currow.setleftborder(1);");
            I("table xx.currow.hideleftborder(1);");
            I("table xx.currow.next();");
            I("table xx.currow.settext(1,'hejsa');");
            I("table xx.currow.settext(2,'med dig');");
            I("table xx.print();");

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("TABLE <2010 2014> tablesmall;");
        }

        [TestMethod]
        public void _Test_Delete()
        {
            //Also checked somewhat in Test_Create()
            foreach (string f in new string[] { "a", "q" })  //testing both A and Q
            {

                //just testing that it parses
                I("RESET;");
                I("OPTION freq " + f + ";");
                I("CREATE a, b, c, d;");
                Assert.AreEqual(First().storage.Count, 4);
                I("DELETE b, c;");
                Assert.AreEqual(First().storage.Count, 2);
                I("CREATE x1, x2;");
                Assert.AreEqual(First().storage.Count, 4);
                I("DELETE *;");
                Assert.AreEqual(First().storage.Count, 0);
                I("CREATE x1, x2, x3;");
                Assert.AreEqual(First().storage.Count, 3);
                
                I("RESET;");
                I("OPTION freq " + f + ";");
                I("CREATE a, b, c, d;");
                Assert.AreEqual(First().storage.Count, 4);
                I("DELETE b, c;");
                Assert.AreEqual(First().storage.Count, 2);
                I("CREATE x1, x2;");
                Assert.AreEqual(First().storage.Count, 4);
                I("DELETE *;");
                Assert.AreEqual(First().storage.Count, 0);
                I("CREATE x1, x2, x3;");
                Assert.AreEqual(First().storage.Count, 3);
            }                
        }

        [TestMethod]
        public void _Test_Hdg()
        {
            //Does not test the result of a simulation, only the commands
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("HDG 'abe' + 'kat';");
            Assert.AreEqual(First().info1, "abekat");
        }
        
        [TestMethod]
        public void _Test_GotoTarget()
        {
            I("RESET;");
            I("VAL %x = 1; GOTO lbl1; TARGET lbl1; VAL %x = 2; VAL %x = %x + 10;");
            _AssertScalarVal(First(), "%x", 12);

            I("RESET;");
            I("VAL %x = 1; GOTO lbl1; VAL %x = 2; TARGET lbl1; VAL %x = %x + 10;");
            _AssertScalarVal(First(), "%x", 11);
            //_AssertScalarVal(First(), "%x", 11);

            I("RESET;");
            I("VAL %x = 1; GOTO lbl1; VAL %x = 2; VAL %x = %x + 10; TARGET lbl1;");
            //_AssertScalarVal(First(), "%x", 1);
            _AssertScalarVal(First(), "%x", 1);

            //more refs to same TARGET
            I("RESET;");
            I("VAL %x = 1; GOTO lbl1; GOTO lbl1; VAL %x = 2; VAL %y = 10; TARGET lbl1;");
            _AssertScalarVal(First(), "%x", 1);
            //_AssertScalarVal(First(), "%x", 1);

            I("RESET;");
            I("VAL %sum = 0; FOR val %i = 1 to 5; VAL %sum = %sum + %i; END;");
            //_AssertScalarVal(First(), "%sum", 15);
            _AssertScalarVal(First(), "%sum", 15);

            I("RESET;");
            I("VAL %sum = 0; FOR val %i = 1 to 5; IF(%i == 4) GOTO lbl1; END; VAL %sum = %sum + %i; END; TARGET lbl1;");
            //_AssertScalarVal(First(), "%sum", 6);
            _AssertScalarVal(First(), "%sum", 6);
            //Assert.IsFalse(Program.scalars.ContainsKey("i"));  //should be removed by the finally clause in the C# loop
            Assert.IsFalse(Program.databanks.GetFirst().ContainsIVariable("%i"));  //should be removed by the finally clause in the C# loop

            //TARGET without GOTO is ok.
            I("RESET;");
            I("TARGET lbl1;");

            //orphaned labels are ok.
            I("RESET;");
            I("GOTO lbl2; TARGET lbl1; TARGET lbl2;");

            //TARGET dublet
            I("RESET;");
            FAIL("VAL %x = 1; GOTO lbl1; VAL %x = 2; VAL %y = 10; TARGET lbl1; TARGET lbl1;");

            //non-existing TARGET
            I("RESET;");
            FAIL("VAL %x = 1; GOTO lbl1; VAL %x = 2; VAL %y = 10; TARGET lbl2;");

            //wrong scope
            I("RESET;");
            FAIL("val %sum = 0; goto l1; for val %i=1 to 5; TARGET l1; val %sum = %sum + %i; end;");

        }

        [TestMethod]
        public void _Test_ExportSeries()
        {
            //Only syntax check: do this seriously...!
            I("RESET;");
            I("TIME 2005 2010;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("SYS'del exportseries." + Globals.extensionCommand + "';");
            I("EXPORT<2005 2010 gcm='='> fy, fe file=exportseries;");
        }

        [TestMethod]
        public void _Test_EndoExo2()
        {
            //Checks ENDO, EXO and UNFIX
            //Does not test the result of a simulation, only the commands
            I("RESET; MODE sim;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("MODEL jul05;");  //otherwise ENDO/EXO will refuse to work
            I("ENDO a, b, c, d;");
            I("EXO x, y, z;");
            Assert.AreEqual(Program.model.endogenized.Count, 4);
            Assert.AreEqual(Program.model.exogenized.Count, 3);
            //I("ENDO;");
            //Assert.AreEqual(Program.model.endogenized.Count, 0);
            //Assert.AreEqual(Program.model.exogenized.Count, 3);
            //I("EXO;");
            //Assert.AreEqual(Program.model.endogenized.Count, 0);
            //Assert.AreEqual(Program.model.exogenized.Count, 0);
            //I("EXO?;");
            //I("EXO ?;");
            //I("ENDO?;");
            //I("ENDO ?;");
            I("ENDO a, b, c, d;");
            I("EXO x, y, z;");
            Assert.AreEqual(Program.model.endogenized.Count, 4);
            Assert.AreEqual(Program.model.exogenized.Count, 3);
            I("UNFIX;");
            Assert.AreEqual(Program.model.endogenized.Count, 0);
            Assert.AreEqual(Program.model.exogenized.Count, 0);
        }

        [TestMethod]
        public void _Test_Findmisingdata()
        {
            //Does not test the result, only the command
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("MODEL jul05;");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("FINDMISSINGDATA<2012 2015>fy,enl,pcp,iwbz;");
            I("FINDMISSINGDATA;");

            //Tests replacement of M with a value (here 0)
            I("RESET;");
            Databank w = First();
            I("CREATE a, b;");
            I("TIME 2000 2003;");
            I("SERIES a = (1, m(), 2, m());");
            I("SERIES b = (m(), 1, m(), 2);");
            I("FINDMISSINGDATA <replace = 0> a, b;");
            _AssertSeries(w, "a", 2000, 1, sharedDelta);
            _AssertSeries(w, "a", 2001, 0, sharedDelta);
            _AssertSeries(w, "a", 2002, 2, sharedDelta);
            _AssertSeries(w, "a", 2003, 0, sharedDelta);
            _AssertSeries(w, "b", 2000, 0, sharedDelta);
            _AssertSeries(w, "b", 2001, 1, sharedDelta);
            _AssertSeries(w, "b", 2002, 0, sharedDelta);
            _AssertSeries(w, "b", 2003, 2, sharedDelta);
            I("RESET;");
            I("CREATE a, b;");
            I("TIME 2000 2003;");
            I("SERIES a = (1, m(), 2, m());");
            I("SERIES b = (m(), 1, m(), 2);");
            I("FINDMISSINGDATA <2001 2002 replace = 0> a;");
            _AssertSeries(w, "a", 2000, 1, sharedDelta);
            _AssertSeries(w, "a", 2001, 0, sharedDelta);
            _AssertSeries(w, "a", 2002, 2, sharedDelta);
            _AssertSeries(w, "a", 2003, double.NaN, sharedDelta);
            _AssertSeries(w, "b", 2000, double.NaN, sharedDelta);
            _AssertSeries(w, "b", 2001, 1, sharedDelta);
            _AssertSeries(w, "b", 2002, double.NaN, sharedDelta);
            _AssertSeries(w, "b", 2003, 2, sharedDelta);

            I("RESET;");
            I("CREATE a, b;");
            I("TIME 2000 2003;");
            I("SERIES a = (1, m(), 2, m());");
            I("SERIES b = (m(), 1, m(), 2);");
            I("FINDMISSINGDATA <replace = 0>;");
            _AssertSeries(w, "a", 2000, 1, sharedDelta);
            _AssertSeries(w, "a", 2001, 0, sharedDelta);
            _AssertSeries(w, "a", 2002, 2, sharedDelta);
            _AssertSeries(w, "a", 2003, 0, sharedDelta);
            _AssertSeries(w, "b", 2000, 0, sharedDelta);
            _AssertSeries(w, "b", 2001, 1, sharedDelta);
            _AssertSeries(w, "b", 2002, 0, sharedDelta);
            _AssertSeries(w, "b", 2003, 2, sharedDelta);
            I("RESET;");
            I("CREATE a, b;");
            I("TIME 2000 2003;");
            I("SERIES a = (1, m(), 2, m());");
            I("SERIES b = (m(), 1, m(), 2);");
            I("FINDMISSINGDATA <2001 2002 replace = 0>;");
            _AssertSeries(w, "a", 2000, 1, sharedDelta);
            _AssertSeries(w, "a", 2001, 0, sharedDelta);
            _AssertSeries(w, "a", 2002, 2, sharedDelta);
            _AssertSeries(w, "a", 2003, double.NaN, sharedDelta);
            _AssertSeries(w, "b", 2000, double.NaN, sharedDelta);
            _AssertSeries(w, "b", 2001, 1, sharedDelta);
            _AssertSeries(w, "b", 2002, 0, sharedDelta);
            _AssertSeries(w, "b", 2003, 2, sharedDelta);
        }

        [TestMethod]
        public void _Test_OpenCascading()
        {
            I("RESET;");
            I("TIME 2000 2000;");
            I("OPEN<edit>sletmig1;");
            I("CREATE a, b, c;");
            I("a = 100; b = 100; c = 100;");
            I("CLOSE *;");
            I("OPEN<edit>sletmig2;");
            I("CREATE a, b;");
            I("a = 777; b = 777;");
            I("CLOSE *;");

            I("RESET;");
            I("OPTION databank search = yes;");
            I("TIME 2000 2000;;");
            I("CREATE a; SERIES a = 888;");
            I("OPEN sletmig2;");
            I("OPEN sletmig1;");
            I("CREATE xa, xb, xc;");
            I("SER xa = a;");
            I("SER xb = b;");
            I("SER xc = c;");
            
            //1. work          a = 888
            //2. ref
            //3. sletmig2      a = b = 777
            //4. sletmig1      a = b = c = 100

            _AssertSeries(First(), "xa", 2000, 888d, 0d);
            _AssertSeries(First(), "xb", 2000, 777d, 0d);
            _AssertSeries(First(), "xc", 2000, 100d, 0d);

            I("OPTION databank search = no;");
            I("CREATE ya, yb, yc;");
            I("SER ya = a;");
            FAIL("SER yb = b;");
            FAIL("SER yc = c;");
            _AssertSeries(First(), "ya", 2000, 888d, 0d);

            // ---------------------------------------------
            //       REPEAT OF ABOVE, now with something in the ref databank
            //       The ref databank must never be searched!
            // ---------------------------------------------
                        
            I("RESET;");
            I("OPTION databank search = yes;");
            I("TIME 2000 2000;");
            I("CREATE a; SERIES a = 888;");
            I("OPEN sletmig2;");
            I("OPEN sletmig1;");
            I("CREATE xa, xb, xc;");
            I("SER xa = a;");
            I("SER xb = b;");
            I("SER xc = c;");
            I("READ<ref> sletmig1;");       //   <------ NEW

            //1. work          a = 888
            //2. ref           a = b = c = 100   <------ NEW
            //3. sletmig2      a = b = 777
            //4. sletmig1      a = b = c = 100

            Assert.AreEqual(Program.databanks.GetRef().storage.Count, 3);     //   <------ NEW
            _AssertSeries(Program.databanks.GetRef(), "a", 2000, 100d, 100d);  //   <------ NEW
            _AssertSeries(Program.databanks.GetRef(), "b", 2000, 100d, 100d);  //   <------ NEW
            _AssertSeries(Program.databanks.GetRef(), "c", 2000, 100d, 100d);  //   <------ NEW
            //below there is no impact from the values in the reference bank
            _AssertSeries(First(), "xa", 2000, 888d, 0d);
            _AssertSeries(First(), "xb", 2000, 777d, 0d);
            _AssertSeries(First(), "xc", 2000, 100d, 0d); 
            
        }

        [TestMethod]
        public void _Test_ScalarSubstitionInStrings()
        {
            I("RESET;");
            I("VAL %v = -1.2345;");
            I("STRING %s = 'a%v|b';");
            _AssertScalarString(First(), "%s", "a%v|b");
            I("STRING %s = 'a{%v}b';");
            _AssertScalarString(First(), "%s", "a-1.2345b");
            I("STRING %s = 'a~{%v}b';");
            _AssertScalarString(First(), "%s", "a{%v}b");
            FAIL("STRING %s = 'a'b';");
            I("STRING %s = 'a~'b';");
            _AssertScalarString(First(), "%s", "a'b");

            I("RESET;");
            I("DATE %d = 2001;");
            I("STRING %s = 'a{%d}b';");
            _AssertScalarString(First(), "%s", "a2001b");

            I("RESET;");
            I("DATE %d = 2001q2;");
            I("STRING %s = 'a{%d}b';");
            _AssertScalarString(First(), "%s", "a2001q2b");

            I("RESET;");
            I("STRING %s2= 'Hej';");
            FAIL("STRING %s = 'a%{s2}b';");
            I("STRING %s = 'a%{%s2}b';");
            _AssertScalarString(First(), "%s", "a%Hejb");

            I("RESET;");
            I("STRING %n = 'Hej';");
            I("STRING %s7 = 'a{%n}b';");
            _AssertScalarString(First(), "%s7", "aHejb");
            I("STRING %s7a = 'a{%n}{%n}b';");  //two substitutions
            _AssertScalarString(First(), "%s7a", "aHejHejb");
            
            I("RESET;");
            I("STRING %n = 'Hej';");
            FAIL("STRING %ss1 = 'a{n}b';");
            I("STRING %ss2 = 'a{%n}b';");
            //_AssertScalarString(First(), "%ss1", "aHejb");
            _AssertScalarString(First(), "%ss2", "aHejb");

            I("RESET;");
            I("STRING %s = 'Hej';");
            FAIL("STRING %ss1 = 'a{s}b';");
            I("STRING %ss2 = 'a{%s}b';");
            FAIL("STRING %ss3 = 'a{%s}{%s}{s}b';");
            FAIL("STRING %ss4 = 'a{%s} {%s} {s}b';");
            //_AssertScalarString(First(), "%ss1", "aHejb");
            _AssertScalarString(First(), "%ss2", "aHejb");
            //_AssertScalarString(First(), "%ss3", "aHejHejHejb");
            //_AssertScalarString(First(), "%ss4", "aHej Hej Hejb");

            // --------- tilde ------------

            I("RESET;");
            I("VAL %v = -1.2345;");
            I("STRING %s = 'a~%v|b';");
            _AssertScalarString(First(), "%s", "a~%v|b");  //no effect
            I("STRING %s = 'a%v|b';");
            _AssertScalarString(First(), "%s", "a%v|b");  //no effect
            I("STRING %s = 'a|b';");
            _AssertScalarString(First(), "%s", "a|b");  //no effect

            I("RESET;");
            I("STRING %n = 'Hej';");            
            I("STRING %ss2 = 'a~{%n}b';");            
            _AssertScalarString(First(), "%ss2", "a{%n}b");
            FAIL("STRING %ss2 = 'ab'c';");
            I("STRING %ss2 = 'ab~'c';");
            _AssertScalarString(First(), "%ss2", "ab'c");
            I("STRING %ss2 = 'a{'b'}c';");
            _AssertScalarString(First(), "%ss2", "abc");
            I("STRING %ss2 = 'a{'x~'b'}c';");
            _AssertScalarString(First(), "%ss2", "ax'bc");

        }

        [TestMethod]
        public void _Test_OpenClose2()
        {
            // ----------------------------------------------------------
            //          - OPEN command, also OPEN<edit>, <first>, <last>, <ref>, etc., also non-existing files.
            //          - CLOSE command
            //          uses a \temp folder that is deleted first
            // ----------------------------------------------------------

            if (false)
            {

                //Time stamps

                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
                I("SYS'del testbank.gbk';");
                I("SYS'del testbank1.gbk';");
                I("SYS'del testbank2.gbk';");
                I("OPEN<edit>testbank;"); //new file                                 
                I("SERIES <1999 2004> xx1 = 100;");
                I("OPEN<edit>testbank as testbank2;"); //new file with same name
                I("SERIES <1999 2004> xx1 = 100;");
                //now we have two new RAM-banks with data
                I("CLOSE testbank;");  //testbank.gbk is written to file
                I("CLOSE testbank2;");  //FAIL this should fail because the file suddenly appears from nowhere


                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
                I("SYS'del testbank.gbk';");
                I("SYS'del testbank1.gbk';");
                I("SYS'del testbank2.gbk';");
                I("OPEN<edit>testbank;"); //new file 
                I("SERIES <1999 2004> xx1 = 100;");
                I("CLOSE testbank;"); //writing testbank.gbk with hash1            
                I("OPEN<edit>testbank as testbank1;"); //existing file with hash1
                I("SERIES <1999 2004> xx2 = 100;");
                I("OPEN<edit>testbank as testbank2;"); //existing file with hash1
                I("CLOSE testbank1;"); //existing file testbank.gbk is altered, now has hash2            
                I("CLOSE testbank2;");  //FAIL testbank.gbk is written, but it was read with hash1, and when checking now it has hash2            

            }

            if (true)
            {               
                
                //Basic test first

                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
                I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                I("OPEN <tsd> small;");
                I("SERIES <1999 2004> fy1 = 100;");
                I("SERIES <1999 2004> xx1 = small:fy1-fy1;");
                I("SERIES <1999 2004> xx2 = @fy1-fy1;");
                I("SERIES <1999 2004> xx3 = work:fy1-fy1;");
                I("SERIES <1999 2004> xx4 = ref:fy1-fy1;");
                UData u;
                u = Data("xx1", 2000, "a"); Assert.AreEqual(u.w, 1.23454321E+02d - 100d);
                u = Data("xx2", 2000, "a"); Assert.AreEqual(u.w, 1.23454321E+02d - 100d);
                u = Data("xx3", 2000, "a"); Assert.AreEqual(u.w, 0d);
                u = Data("xx4", 2000, "a"); Assert.AreEqual(u.w, 1.23454321E+02d - 100d);

                //test CLOSE
                I("OPEN <tsd> small as test1;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                I("CLOSE test1;");
                Assert.AreEqual(Program.databanks.storage.Count, 3);
                I("OPEN <tsd> small as test1;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);

                //============ more advanced tests =============================
                // test OPEN<edit>
                // test OPEN<ref>
                // test LOCK
                // test that data is written back
                //---------------------------------

                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");

                // ------------ open normal bank

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("TIME 2010 2011;");
                I("CREATE a;");
                I("SERIES a = (10, 11);");
                I("WRITE temp\\bank1;"); //<----
                I("SERIES a = (20, 21);");
                I("WRITE temp\\bank2;"); //<----
                I("SERIES a = (50, 51);");
                I("WRITE temp\\bank3;"); //<----

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 3);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                I("CLOSE bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                //bulk close
                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank1;");
                I("OPEN temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);                
                I("CLOSE bank1, bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank1;");
                I("OPEN temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                
                    Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                    Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                

                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                FAIL("OPEN temp\\bank1, temp\\bank2 as bank3;");
                I("OPEN temp\\bank1, temp\\bank2 as *, bank3;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank3");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                // ---- EDIT and REF, also with test of PRT<m>, PRT @a, etc.

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank2;");
                I("OPEN<first>temp\\bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                Assert.AreEqual(Program.databanks.storage[0].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "Work");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank2;");
                I("OPEN<first>temp\\bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                Assert.AreEqual(Program.databanks.storage[0].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "Work");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                I("CLOSE bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 3);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank2");

                if (false)  //swapping
                {

                    I("RESET;");
                    //if (i == 0) I("OPTION databank logic = aremos;");
                    I("OPEN temp\\bank2;");
                    I("OPEN<ref>temp\\bank1;");
                    Assert.AreEqual(Program.databanks.storage.Count, 4);
                    Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                    Assert.AreEqual(Program.databanks.storage[1].name, "bank1");
                    Assert.AreEqual(Program.databanks.storage[2].name, Globals.Ref);
                    Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                    I("CLOSE *;");
                    Assert.AreEqual(Program.databanks.storage.Count, 2);
                    Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                    Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                    

                    I("RESET;");
                    //if (i == 0) I("OPTION databank logic = aremos;");
                    I("OPEN temp\\bank2;");
                    I("OPEN<ref>temp\\bank1;");
                    Assert.AreEqual(Program.databanks.storage.Count, 4);
                    Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                    Assert.AreEqual(Program.databanks.storage[1].name, "bank1");
                    Assert.AreEqual(Program.databanks.storage[2].name, Globals.Ref);
                    Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                    I("CLOSE bank1;");
                    Assert.AreEqual(Program.databanks.storage.Count, 3);
                    Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                    Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                    Assert.AreEqual(Program.databanks.storage[2].name, "bank2");

                    //An unswap test
                    I("RESET;");
                    //if (i == 0) I("OPTION databank logic = aremos;");
                    I("READ temp\\bank1;");
                    I("OPEN<first>temp\\bank2;");
                    I("OPEN<ref>temp\\bank3;");
                    Assert.AreEqual(Program.databanks.storage.Count, 4);
                    Assert.AreEqual(Program.databanks.storage[0].name, "bank2");
                    Assert.AreEqual(Program.databanks.storage[1].name, "bank3");
                    Assert.AreEqual(Program.databanks.storage[2].name, Globals.Ref); //bank1
                    Assert.AreEqual(Program.databanks.storage[3].name, "Work"); //bank1
                    I("UNSWAP;");
                    Assert.AreEqual(Program.databanks.storage.Count, 4);
                    Assert.AreEqual(Program.databanks.storage[0].name, "Work"); //bank1
                    Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref); //bank1
                    Assert.AreEqual(Program.databanks.storage[2].name, "bank2");
                    Assert.AreEqual(Program.databanks.storage[3].name, "bank3");

                    //also with tests of PRT etc
                    I("RESET;");
                    //if (i == 0) I("OPTION databank logic = aremos;");
                    I("READ temp\\bank1;");
                    I("OPEN<first>temp\\bank2;");
                    I("OPEN<ref>temp\\bank3;");
                    Assert.AreEqual(Program.databanks.storage.Count, 4);
                    Assert.AreEqual(Program.databanks.storage[0].name, "bank2");
                    Assert.AreEqual(Program.databanks.storage[1].name, "bank3");
                    Assert.AreEqual(Program.databanks.storage[2].name, Globals.Ref); //bank1
                    Assert.AreEqual(Program.databanks.storage[3].name, "Work"); //bank1
                    Globals.lastPrtOrMulprtTable = null;
                    I("PRT<2010 2010> a;");
                    Table table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, 20d, sharedDelta);
                    table = null;
                    I("PRT<2010 2010> @a;");
                    table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, 50d, sharedDelta);
                    Globals.lastPrtOrMulprtTable = null;
                    I("PRT<2010 2010 m> a;");
                    table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, -30d, sharedDelta);
                    // -----------------
                    table = null;
                    I("PRT<2010 2010> bank2:a;");
                    table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, 20d, sharedDelta);
                    Globals.lastPrtOrMulprtTable = null;
                    I("PRT<2010 2010> bank3:a;");
                    table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, 50d, sharedDelta);
                    table = null;
                    I("PRT<2010 2010> work:a;");
                    table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, 10d, sharedDelta);
                    Globals.lastPrtOrMulprtTable = null;
                    I("PRT<2010 2010> ref:a;");
                    table = Globals.lastPrtOrMulprtTable;
                    Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2010"); //why is it not a date?
                    _AssertHelperTwoDoubles(table.Get(2, 2).number, 10d, sharedDelta);
                }

                // ---- succession

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN \\temp\\bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 3);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                I("OPEN \\temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                
                    Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                    Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                
                I("OPEN<first> \\temp\\bank1;");
                Assert.AreEqual(Program.databanks.storage.Count, 4);
                Assert.AreEqual(Program.databanks.storage[0].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "Work");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank2");

                // ------- <first>, <>, <last>

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN<first>temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 5);
                Assert.AreEqual(Program.databanks.storage[0].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank2");
                Assert.AreEqual(Program.databanks.storage[3].name, Globals.Work);
                Assert.AreEqual(Program.databanks.storage[4].name, "bank3");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 5);
                Assert.AreEqual(Program.databanks.storage[0].name, Globals.Work);
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                
                    Assert.AreEqual(Program.databanks.storage[2].name, "bank3");
                    Assert.AreEqual(Program.databanks.storage[3].name, "bank1");
                    Assert.AreEqual(Program.databanks.storage[4].name, "bank2");
                
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN<last> temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 5);
                Assert.AreEqual(Program.databanks.storage[0].name, Globals.Work);
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank3");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[4].name, "bank2");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                // ------- <sec>

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN<pos=2>temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage[0].name, Globals.Work);                
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                Assert.AreEqual(Program.databanks.storage[4].name, "bank3");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                // ------- <pos=n>

                I("RESET;");
                //if (i == 0) I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN<pos=1>temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 5);
                Assert.AreEqual(Program.databanks.storage[0].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank2");
                Assert.AreEqual(Program.databanks.storage[3].name, Globals.Work);
                Assert.AreEqual(Program.databanks.storage[4].name, "bank3");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN <pos=2> temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 5);
                Assert.AreEqual(Program.databanks.storage[0].name, Globals.Work);
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank2");
                Assert.AreEqual(Program.databanks.storage[4].name, "bank3");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                I("OPEN<pos=3> temp\\bank1, temp\\bank2;");
                Assert.AreEqual(Program.databanks.storage.Count, 5);
                Assert.AreEqual(Program.databanks.storage[0].name, Globals.Work);
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "bank3");
                Assert.AreEqual(Program.databanks.storage[3].name, "bank1");
                Assert.AreEqual(Program.databanks.storage[4].name, "bank2");
                I("CLOSE *;");
                Assert.AreEqual(Program.databanks.storage.Count, 2);
                Assert.AreEqual(Program.databanks.storage[0].name, "Work");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bank3;");
                FAIL("OPEN<pos=4> temp\\bank1, temp\\bank2;");

                // --------- changing stuff ----------------

                //changing first stuff
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <edit> temp\\bankTemp;");
                I("TIME 2010 2010;");
                I("SERIES a = 100;");
                I("CLOSE bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 100, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);

                //changing REF stuff
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                if (false)
                {
                    I("RESET;");
                    // if (i == 0)I("OPTION databank logic = aremos;");
                    I("OPEN <ref> temp\\bankTemp;");
                    I("TIME 2010 2010;");
                    FAIL("SERIES @a = 100;");
                    I("UNLOCK bankTemp;");
                    I("SERIES @a = 100;");
                    I("CLOSE bankTemp;");
                }

                
                    I("RESET;");
                    // if (i == 0)I("OPTION databank logic = aremos;");
                    I("OPEN <edit> temp\\bankTemp;");
                    I("TIME 2010 2010;");                    
                    I("SERIES a = 100;");
                    I("CLOSE bankTemp;");
                

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 100, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);

                //changing normal open bank
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                I("TIME 2010 2010;");
                FAIL("SERIES bankTemp:a = 100;");
                I("UNLOCK bankTemp;");
                I("SERIES bankTemp:a = 100;");
                I("CLOSE bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 100, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);

                // ------------ <save=no> ----------------

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <edit save=no> temp\\bankTemp;");
                I("TIME 2010 2010;");
                I("SERIES a = 100;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 100, sharedDelta);  //changed
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);
                I("CLOSE bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 10, sharedDelta);  //original
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);


                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <edit> temp\\bankTemp;");
                I("TIME 2010 2010;");
                I("SERIES a = 100;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 100, sharedDelta);  //changed
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);
                I("CLOSE <save=no> bankTemp;");
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2010, 10, sharedDelta);  //original
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2011, 11, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankTemp"), "a", 2012, double.NaN, sharedDelta);


                // --------- LOCK/UNLOCK ----------------

                //changing first stuff
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <first> temp\\bankTemp;");
                I("TIME 2010 2010;");
                FAIL("SERIES a = 100;");
                I("UNLOCK bankTemp;");
                I("SERIES a = 100;");

                //changing ref stuff
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                FAIL("OPEN <ref> temp\\bankTemp;");
                //I("TIME 2010 2010;");
                //FAIL("SERIES @a = 100;");
                //I("UNLOCK bankTemp;");
                //I("SERIES @a = 100;");

                //changing normal open bank
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTemp;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankTemp;");
                I("TIME 2010 2010;");
                FAIL("SERIES bankTemp:a = 100;");
                I("UNLOCK bankTemp;");
                I("SERIES bankTemp:a = 100;");

                // --------- non .gbk formats ----------------

                //changing FIRST stuff
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("EXPORT <tsd> temp\\bankTsd;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <edit tsd> temp\\bankTsd;");
                I("TIME 2010 2010;");
                FAIL("SERIES a = 100;");

                //changing REF stuff
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("EXPORT <tsd> temp\\bankTsd;");

                //I("RESET;");
                //// if (i == 0)I("OPTION databank logic = aremos;");
                //I("OPEN <ref tsd> temp\\bankTsd;");
                //I("TIME 2010 2010;");
                //FAIL("SERIES @a = 100;");

                //changing normal open bank
                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("READ \\temp\\bank1;");
                I("WRITE temp\\bankTsd;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <tsd> temp\\bankTsd;");
                I("TIME 2010 2010;");
                FAIL("SERIES bankTsd:a = 100;");

                // --------- constructing a bank ----------------

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN <edit> temp\\bankNew;");
                Assert.AreEqual(Program.databanks.storage.Count, 3);
                Assert.AreEqual(Program.databanks.storage[0].name, "bankNew");
                Assert.AreEqual(Program.databanks.storage[1].name, Globals.Ref);
                Assert.AreEqual(Program.databanks.storage[2].name, "Work");
                I("CREATE tsNew;");
                I("SERIES <2010 2010> tsNew = 12345;");
                I("CLOSE bankNew;");

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                I("OPEN temp\\bankNew;");
                _AssertSeries(Program.databanks.GetDatabank("bankNew"), "tsNew", 2009, double.NaN, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankNew"), "tsNew", 2010, 12345, sharedDelta);
                _AssertSeries(Program.databanks.GetDatabank("bankNew"), "tsNew", 2011, double.NaN, sharedDelta);

                // --------- illegals ----------------

                I("RESET;");
                // if (i == 0)I("OPTION databank logic = aremos;");
                FAIL("CLOSE work;");
                FAIL("CLOSE ref;");
                FAIL("OPEN<edit>a, b;");
                FAIL("OPEN<ref>a, b;");
                //FAIL("OPEN *, b;");
                //FAIL("OPEN a, *;");
                //FAIL("OPEN *, *;");
            }
        }

        [TestMethod]
        public void _Test_Cls()
        {
            I("CLS;");  //hard to check result of this here... never mind.
        }

        [TestMethod]
        public void _Test_DatabankLocalGlobal()
        {

            

            // ===========================================================
            // ================= test get/set of local and global databank
            // ================= tests shadowing, no use of LOCAL/GLOBAL keyword
            // ===========================================================

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");

            I("RESET;");
            I("MODE data;");

            I("OPEN <edit> tmp; CLEAR tmp;");
            I("%s = 'tmp';");
            I("CLOSE tmp;");
            I("OPEN tmp;");     
            
            //--------- test of assignments       

            I("%x1 = 1;");
            I("ref:%y1 = %x1;");
            _AssertScalarVal(Ref(), "%y1", 1d);

            I("local:%x2 = 2;");
            I("ref:%y2 = %x2;");
            _AssertScalarVal(Ref(), "%y2", 2d);
            
            FAIL("tmp:%x3 = 3;");
            I("UNLOCK tmp;");
            I("tmp:%x3 = 3;");
            I("ref:%y3 = %x3;");
            _AssertScalarVal(Ref(), "%y3", 3d);

            I("global:%x4 = 4;");
            I("ref:%y4 = %x4;");
            _AssertScalarVal(Ref(), "%y4", 4d);

            // ----------- test of shadowing -------------

            I("reset; time 2000 2002; OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");

            I("RESET;");
            I("MODE data;");
            I("OPEN tmp;");  //has %s = 'tmp'

            I("first:%s = 'first';");            
            I("local:%s = 'local';");
            I("global:%s = 'global';");
            I("ref:%s = %s;");
            _AssertScalarString(Ref(), "%s", "local");

            I("CLEAR local;");
            I("ref:%s = %s;");
            _AssertScalarString(Ref(), "%s", "first");

            I("CLEAR <first>;");
            I("ref:%s = %s;");
            _AssertScalarString(Ref(), "%s", "tmp");

            FAIL("CLEAR tmp;");
            I("UNLOCK tmp;");
            I("CLEAR tmp;");
            I("ref:%s = %s;");
            _AssertScalarString(Ref(), "%s", "global");

            I("CLEAR global;");
            FAIL("ref:%s = %s;");

            // ----------- test of LOCAL/GLOBAL commands -------------

            I("RESET;");
            I("LOCAL x;");
            Assert.AreEqual(Local().storage.Count, 0);
            I("x = 5;");
            Assert.AreEqual(Local().storage.Count, 1);
            I("DELETE x;");
            Assert.AreEqual(Local().storage.Count, 0);
            Assert.AreEqual(First().storage.Count, 0);

            I("RESET;");
            I("LOCAL x;");
            Assert.AreEqual(Local().storage.Count, 0);
            I("x = 5;");
            Assert.AreEqual(Local().storage.Count, 1);
            I("RENAME x TO y;"); //should stay in local bank
            Assert.AreEqual(Local().storage.Count, 0);
            Assert.AreEqual(First().storage.Count, 1);

            I("RESET;");
            I("GLOBAL x;");
            Assert.AreEqual(Global().storage.Count, 0);
            I("x = 5;");
            Assert.AreEqual(Global().storage.Count, 1);
            I("DELETE x;");
            Assert.AreEqual(Global().storage.Count, 0);
            Assert.AreEqual(First().storage.Count, 0);

            I("RESET;");
            I("GLOBAL x;");
            Assert.AreEqual(Global().storage.Count, 0);
            I("x = 5;");
            Assert.AreEqual(Global().storage.Count, 1);
            I("RENAME x TO y;"); //should stay in global bank
            Assert.AreEqual(Global().storage.Count, 0);
            Assert.AreEqual(First().storage.Count, 1);

            // ----------------

            I("RESET;");
            I("LOCAL %s1;");
            I("%s1 = 'local';");
            I("%s2 = %s1;");  //also works in sim-mode without searching!
            I("first:%s1 = 'first';");
            I("%s3 = first:%s1;");
            Assert.AreEqual(Local().storage.Count, 1);
            _AssertScalarString(Local(), "%s1", "local");
            Assert.AreEqual(First().storage.Count, 3);
            _AssertScalarString(First(), "%s1", "first");
            _AssertScalarString(First(), "%s2", "local");
            _AssertScalarString(First(), "%s3", "first");

            I("RESET;");
            I("GLOBAL %s1;");
            I("%s1 = 'global';");
            I("%s2 = %s1;");  //also works in sim-mode without searching!
            I("first:%s1 = 'first';");
            I("%s3 = first:%s1;");
            Assert.AreEqual(Global().storage.Count, 1);
            _AssertScalarString(Global(), "%s1", "global");
            Assert.AreEqual(First().storage.Count, 3);
            _AssertScalarString(First(), "%s1", "first");
            _AssertScalarString(First(), "%s2", "global");
            _AssertScalarString(First(), "%s3", "first");


            I("RESET;");
            I("LOCAL<all>;");
            I("%s1 = 'local';");
            I("%s2 = %s1;");  //also works in sim-mode without searching!
            I("first:%s1 = 'first';");
            I("first:%s3 = 'first';");
            I("%s4 = first:%s1;");
            Assert.AreEqual(Local().storage.Count, 3);
            _AssertScalarString(Local(), "%s1", "local");
            _AssertScalarString(Local(), "%s2", "local");
            _AssertScalarString(Local(), "%s4", "first");
            Assert.AreEqual(First().storage.Count, 2);
            _AssertScalarString(First(), "%s1", "first");
            _AssertScalarString(First(), "%s3", "first");

            I("RESET;");
            I("GLOBAL<all>;");
            I("%s1 = 'global';");
            I("%s2 = %s1;");  //also works in sim-mode without searching!            
            I("first:%s1 = 'first';");
            I("first:%s3 = 'first';");
            I("%s4 = first:%s1;");
            Assert.AreEqual(Global().storage.Count, 3);
            _AssertScalarString(Global(), "%s1", "global");
            _AssertScalarString(Global(), "%s2", "global");
            _AssertScalarString(Global(), "%s4", "first");
            Assert.AreEqual(First().storage.Count, 2);
            _AssertScalarString(First(), "%s1", "first");
            _AssertScalarString(First(), "%s3", "first");


            // -------------- testing nested gcm, functions, procedures combined with local/global

            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Local_Global';");
            Globals.unitTestScreenOutput.Clear();
            I("RUN t1;"); //nested gcm
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("t1--> t1 t2--> t2 t3--> t3 t2--> t2 t1--> t1 "));
            Globals.unitTestScreenOutput.Clear();
            I("RUN t4;"); //function
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("777 102 777 "));
            Globals.unitTestScreenOutput.Clear();
            I("RUN t5;"); //procedure
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("777 102 777 "));
            Globals.unitTestScreenOutput.Clear();
            I("RUN s1;"); //nested gcm
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("t1--> t1 t2--> t2 t3--> t3 t2--> t2 t1--> t1 "));
            Globals.unitTestScreenOutput.Clear();
            I("RUN s4;"); //function
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("777 102 666 777 "));
            Globals.unitTestScreenOutput.Clear();
            I("RUN s5;"); //procedure
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("777 102 666 777 "));
            
        }

        [TestMethod]
        public void _Test_Wildcard()
        {
            //  Work     Ref      b1       b10
            //-------------------------------------
            //  x1       x1       x3       <empty>
            //  %x1      %x1      %x3
            //  #x1      #x1      #x3
            //  x2       x1!q     x3!q
            //  x1!q

            TestCopyHelper(1);

            // ------------------------------------------------
            // no bank
            // ------------------------------------------------

            I("#m = ['*'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x2"));
            I("#m = ['*!*'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x1!q", "x2"));

            I("#m = ['a'..'z'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x2"));
            FAIL("#m = ['a!*'..'z!*'];");  //freq must be the same for ranges, could maybe implement !* at some point...
            I("#m = ['a!a'..'z!a'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x2"));

            I("#m = ['*1'];");
            _AssertListString(First(), "#m", new StringOrList("x1"));
            I("#m = ['*1!*'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x1!q"));
            I("#m = ['a'..'x1'];");
            _AssertListString(First(), "#m", new StringOrList("x1"));

            I("#m = ['%*'];");
            _AssertListString(First(), "#m", new StringOrList("%x1"));
            I("#m = ['#*'];");
            _AssertListString(First(), "#m", new StringOrList("#m", "#x1"));
            I("#m = ['#a'..'#z'];");
            _AssertListString(First(), "#m", new StringOrList("#m", "#x1"));

            // ------------------------------------------------
            // with bank
            // ------------------------------------------------

            I("#m = ['b1:*'];");
            _AssertListString(First(), "#m", new StringOrList("b1:x3"));
            FAIL("#m = ['b1:a'..'z'];");
            FAIL("#m = ['b1:a'..'work:z'];");
            I("#m = ['b1:a'..'b1:z'];");  //must be same bank both places
            _AssertListString(First(), "#m", new StringOrList("b1:x3"));
            I("#m = ['b1:*!*'];");
            _AssertListString(First(), "#m", new StringOrList("b1:x3", "b1:x3!q"));
            FAIL("#m = ['b1:a!*'..'b1:z!*'];");

            I("#m = ['b1:*3'];");
            _AssertListString(First(), "#m", new StringOrList("b1:x3"));
            I("#m = ['b1:*3!*'];");
            _AssertListString(First(), "#m", new StringOrList("b1:x3", "b1:x3!q"));

            I("#m = ['b1:%*'];");
            _AssertListString(First(), "#m", new StringOrList("b1:%x3"));
            I("#m = ['b1:#*'];");
            _AssertListString(First(), "#m", new StringOrList("b1:#x3"));
            I("#m = ['b1:#a'..'b1:#z'];");
            _AssertListString(First(), "#m", new StringOrList("b1:#x3"));

            // ------------------------------------------------
            // all banks
            // ------------------------------------------------

            I("#m = ['*:*'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x2", "ref:x1", "b1:x3"));
            I("#m = ['*:*!*'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x1!q", "x2", "ref:x1", "ref:x1!q", "b1:x3", "b1:x3!q"));
            I("#m = ['*:a'..'*:z'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x2", "ref:x1", "b1:x3"));

            I("#m = ['*:*3'];");
            _AssertListString(First(), "#m", new StringOrList("b1:x3"));
            I("#m = ['*:*3!*'];");
            _AssertListString(First(), "#m", new StringOrList("b1:x3", "b1:x3!q"));


            I("#m = ['*:%*'];");
            _AssertListString(First(), "#m", new StringOrList("%x1", "ref:%x1", "b1:%x3"));
            I("#m = ['*:#*'];");
            _AssertListString(First(), "#m", new StringOrList("#m", "#x1", "ref:#x1", "b1:#x3"));
            I("#m = ['*:#a'..'*:#z'];");
            _AssertListString(First(), "#m", new StringOrList("#m", "#x1", "ref:#x1", "b1:#x3"));

            // ------------------------------------------------
            // everything
            // ------------------------------------------------

            I("#m = ['*:*!*'] + ['*:%*'] + ['*:#*'];");
            _AssertListString(First(), "#m", new StringOrList("x1", "x1!q", "x2", "ref:x1", "ref:x1!q", "b1:x3", "b1:x3!q", "%x1", "ref:%x1", "b1:%x3", "#m", "#x1", "ref:#x1", "b1:#x3"));
            I("#m = ['*:**'];");
            _AssertListString(First(), "#m", new StringOrList("#m", "#x1", "%x1", "x1", "x1!q", "x2", "ref:#x1", "ref:%x1", "ref:x1", "ref:x1!q", "b1:#x3", "b1:%x3", "b1:x3", "b1:x3!q"));

            // ------------------------------------------------
            // testing curly, returns list<var> not list<string>
            // ------------------------------------------------

            I("#m = {'*:*3'};"); //b1:x3            
            I("<2001 2003> zz = #m[1];");
            _AssertSeries(First(), "zz", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "zz", 2001, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2002, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2003, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2004, double.NaN, sharedDelta);

            I("#m = {['*:*3']};"); //b1:x3            
            I("<2001 2003> zz = #m[1];");
            _AssertSeries(First(), "zz", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "zz", 2001, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2002, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2003, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2004, double.NaN, sharedDelta);

            I("#m = {'*:x3'..'*:x4'};"); //b1:x3            
            I("<2001 2003> zz = #m[1];");
            _AssertSeries(First(), "zz", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "zz", 2001, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2002, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2003, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2004, double.NaN, sharedDelta);

            I("#m = {['*:x3'..'*:x3']};"); //b1:x3            
            I("<2001 2003> zz = #m[1];");
            _AssertSeries(First(), "zz", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "zz", 2001, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2002, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2003, 31d, sharedDelta);
            _AssertSeries(First(), "zz", 2004, double.NaN, sharedDelta);

            // ----------

            I("#m = {['*:*!*'] + ['*:%*'] + ['*:#*']};");
            _AssertListSize(First(), "#m", 14 + 1);  //includes zz variable

            // -------------------------------------------------
            // --------------- testing on a databank ---------------
            // -------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
                        
            I("LIST #a = fxa, fxb, pcp, tg, pxqz;");
            I("LIST #a1 = #a['fX*'];              //pattern in #a list");
            I("LIST #a2 = #a['f?nz'];             //pattern in #a list");
            I("LIST #a3 = #a['pxa'..'pxqz'];      //range in #a list");
            I("LIST #a4 = ['fX*'];                //pattern in Work databank");
            I("LIST #a5 = ['f?n'];                //pattern in Work databank");
            I("LIST #a6 = ['pxa'..'pxqz'];        //range in Work databank");
            I("LIST #a7 = ['*'];                  //all annual series in Work databank");

            _AssertListSize(First(), "#a1", 2);
            _AssertListSize(First(), "#a2", 0);
            _AssertListSize(First(), "#a3", 1);
            _AssertListSize(First(), "#a4", 30);
            _AssertListSize(First(), "#a5", 4);
            _AssertListSize(First(), "#a6", 42);
            _AssertListSize(First(), "#a7", 8358);
            
        }

        [TestMethod]
        public void _Test_Clear()
        {
            //do a better test where banks are OPENed

            I("RESET;");
            I("CREATE x1, x2;");
            I("CLONE;");
            I("CREATE x3;");
            I("CLEAR<first>;");
            Assert.AreEqual(First().storage.Count, 0);
            Assert.AreEqual(Program.databanks.GetRef().storage.Count, 2);

            I("RESET;");
            I("CREATE x1, x2;");
            I("CLONE;");
            I("CREATE x3;");
            I("CLEAR<ref>;");
            Assert.AreEqual(First().storage.Count, 3);
            Assert.AreEqual(Program.databanks.GetRef().storage.Count, 0);

            I("RESET;");
            I("CREATE x1, x2;");
            I("CLONE;");
            I("CREATE x3;");
            I("CLEAR;");
            Assert.AreEqual(First().storage.Count, 0);
            Assert.AreEqual(Program.databanks.GetRef().storage.Count, 0);



        }

        [TestMethod]
        public void _Test_Restart()
        {
            I("RESTART;");
        }

        [TestMethod]
        public void _Test_Reset()
        {
            I("RESET;");
        }

        [TestMethod]
        public void _Test_Checkoff()
        {
            I("RESET;");
            I("#xx = x, y;");
            I("%ww = 'w';");
            I("CHECKOFF a, b, {#xx}, {%ww}, c;");
            Assert.AreEqual(Globals.checkoff.Count, 6);
            Assert.AreEqual(Globals.checkoff[0], "a");
            Assert.AreEqual(Globals.checkoff[1], "b");
            Assert.AreEqual(Globals.checkoff[2], "x");
            Assert.AreEqual(Globals.checkoff[3], "y");
            Assert.AreEqual(Globals.checkoff[4], "w");
            Assert.AreEqual(Globals.checkoff[5], "c");
            I("CHECKOFF ?;");
            Assert.AreEqual(Globals.checkoff.Count, 6);
            I("CHECKOFF;");
            Assert.AreEqual(Globals.checkoff.Count, 0);
        }
        

        [TestMethod]
        public void _Test_Interpolate()
        {
            // ================================
            //        A to Q
            // ================================

            //also testing bank on rhs, implicitely also tests this for COLLAPSE
            I("RESET; MODE data;");
            I("OPTION freq a;");
            I("TIME 2001 2005;");
            I("SERIES @x = (1, 5, 3, m(), 10);");
            I("INTERPOLATE x1!q = ref:x!a repeat;");
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            for (int i = 1; i <= 4; i++)
            {
                _AssertSeries(First(), "x1!q", EFreq.Q, 2001, i, 1d, sharedDelta);
                _AssertSeries(First(), "x1!q", EFreq.Q, 2002, i, 5d, sharedDelta);
                _AssertSeries(First(), "x1!q", EFreq.Q, 2003, i, 3d, sharedDelta);
                _AssertSeries(First(), "x1!q", EFreq.Q, 2004, i, double.NaN, sharedDelta);
                _AssertSeries(First(), "x1!q", EFreq.Q, 2005, i, 10d, sharedDelta);
            }
            _AssertSeries(First(), "x1!q", EFreq.Q, 2006, 1, double.NaN, sharedDelta);

            //

            //also testing bank on lhs, implicitely also tests this for COLLAPSE
            I("RESET; MODE data;");
            I("OPTION freq a;");
            I("TIME 2001 2005;");
            I("SERIES x = (1, 5, 3, m(), 10);");
            I("INTERPOLATE ref:x1!q = x!a prorate;");
            _AssertSeries(Ref(), "x1!q", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            for (int i = 1; i <= 4; i++)
            {
                _AssertSeries(Ref(), "x1!q", EFreq.Q, 2001, i, 1d / 4d, sharedDelta);
                _AssertSeries(Ref(), "x1!q", EFreq.Q, 2002, i, 5d / 4d, sharedDelta);
                _AssertSeries(Ref(), "x1!q", EFreq.Q, 2003, i, 3d / 4d, sharedDelta);
                _AssertSeries(Ref(), "x1!q", EFreq.Q, 2004, i, double.NaN, sharedDelta);
                _AssertSeries(Ref(), "x1!q", EFreq.Q, 2005, i, 10d / 4d, sharedDelta);
            }
            _AssertSeries(Ref(), "x1!q", EFreq.Q, 2006, 1, double.NaN, sharedDelta);

            // ================================
            //        A to M
            // ================================

            I("RESET; MODE data;");
            I("TIME 2001 2005;");
            I("SERIES x = (1, 5, 3, m(), 10);");
            I("OPTION freq m;");
            I("INTERPOLATE x1 = x!a repeat;");  //the .m is removed on lhs
            _AssertSeries(First(), "x1!m", EFreq.M, 2000, 4, double.NaN, sharedDelta);
            for (int i = 1; i <= 12; i++)
            {
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, i, 1d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2002, i, 5d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2003, i, 3d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2004, i, double.NaN, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2005, i, 10d, sharedDelta);
            }
            _AssertSeries(First(), "x1!m", EFreq.M, 2006, 1, double.NaN, sharedDelta);

            //

            I("RESET; MODE data;");
            I("OPTION freq a;");
            I("TIME 2001 2005;");
            I("SERIES x = (1, 5, 3, m(), 10);");
            I("INTERPOLATE x1!m = x prorate;");  //the .a is removed on rhs
            _AssertSeries(First(), "x1!m", EFreq.M, 2000, 4, double.NaN, sharedDelta);
            for (int i = 1; i <= 12; i++)
            {
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, i, 1d / 12d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2002, i, 5d / 12d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2003, i, 3d / 12d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2004, i, double.NaN, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2005, i, 10d / 12d, sharedDelta);
            }
            _AssertSeries(First(), "x1!m", EFreq.M, 2006, 1, double.NaN, sharedDelta);


            // ================================
            //        Q to M
            // ================================

            I("RESET; MODE data;");
            I("TIME 2001 2005;");
            I("OPTION freq q;");
            I("SERIES <2001q1 2002q1> x = (1, 5, 3, m(), 10);");
            I("INTERPOLATE x1!m = x!q repeat;");
            _AssertSeries(First(), "x1!m", EFreq.M, 2000, 12, double.NaN, sharedDelta);
            for (int i = 1; i <= 3; i++)
            {
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 0 + i, 1d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 3 + i, 5d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 6 + i, 3d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 9 + i, double.NaN, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2002, 0 + i, 10d, sharedDelta);
            }
            _AssertSeries(First(), "x1!m", EFreq.M, 2002, 4, double.NaN, sharedDelta);

            I("RESET; MODE data;");
            I("TIME 2001 2005;");
            I("OPTION freq q;");
            I("SERIES <2001q1 2002q1> x = (1, 5, 3, m(), 10);");
            I("INTERPOLATE x1!m = x!q prorate;");
            _AssertSeries(First(), "x1!m", EFreq.M, 2000, 12, double.NaN, sharedDelta);
            for (int i = 1; i <= 3; i++)
            {
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 0 + i, 1d / 3d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 3 + i, 5d / 3d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 6 + i, 3d / 3d, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2001, 9 + i, double.NaN, sharedDelta);
                _AssertSeries(First(), "x1!m", EFreq.M, 2002, 0 + i, 10d / 3d, sharedDelta);
            }
            _AssertSeries(First(), "x1!m", EFreq.M, 2002, 4, double.NaN, sharedDelta);

        }


        [TestMethod]
        public void _Test_Collapse()
        {            
            I("RESET; MODE data;");
            I("TIME 2000 2003;");
            I("OPTION freq q;");
            I("CREATE x;");
            I("SERIES x =   (1, 2, 3, 4,       5, 6, 7, 8,       9, 10, -1, -2,     -3, 9, -3, 7);");
            I("COLLAPSE x1!a = x!q total; ");
            I("OPTION freq a;");
            I("COLLAPSE x2 = x!q; ");
            I("OPTION freq q;");
            I("COLLAPSE x3!a = x!q avg; ");
            I("COLLAPSE x4!a = work:x!q first; ");  //testing bank also
            I("COLLAPSE work:x5!a = x!q last; ");   //testing bank also
            I("OPTION freq a;");  //otherwise GetVariable() goes wrong: badly needs a fix for all this frequency stuff

            _AssertSeries(First(), "x1!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!a", 2000, 10d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2001, 26d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2002, 16d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2003, 10d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x2!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!a", 2000, 10d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2001, 26d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2002, 16d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2003, 10d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x3!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!a", 2000, 10d / 4d, sharedDelta);
            _AssertSeries(First(), "x3!a", 2001, 26d / 4d, sharedDelta);
            _AssertSeries(First(), "x3!a", 2002, 16d / 4d, sharedDelta);
            _AssertSeries(First(), "x3!a", 2003, 10d / 4d, sharedDelta);
            _AssertSeries(First(), "x3!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x4!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4!a", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "x4!a", 2001, 5d, sharedDelta);
            _AssertSeries(First(), "x4!a", 2002, 9d, sharedDelta);
            _AssertSeries(First(), "x4!a", 2003, -3d, sharedDelta);
            _AssertSeries(First(), "x4!a", 2004, double.NaN, sharedDelta);


            _AssertSeries(First(), "x5!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x5!a", 2000, 4d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2001, 8d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2002, -2d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2003, 7d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2004, double.NaN, sharedDelta);

            
            I("RESET;");
            I("TIME 2000 2001;");
            I("OPTION freq m;");
            I("CREATE x;");
            I("SERIES x =   (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24);");
            I("COLLAPSE x1!a = x!m total; ");
            I("OPTION freq a;");
            I("COLLAPSE x2 = x!m; ");
            I("OPTION freq m;");
            I("COLLAPSE x3!a = x!m avg; ");
            I("COLLAPSE x4!a = x!m first; ");
            I("COLLAPSE x5!a = x!m last; ");
            I("OPTION freq a;");

            _AssertSeries(First(), "x1!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!a", 2000, 78d, sharedDelta);
            _AssertSeries(First(), "x1!a", 2001, 222d, sharedDelta);            
            _AssertSeries(First(), "x1!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x2!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!a", 2000, 78d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2001, 222d, sharedDelta);
            _AssertSeries(First(), "x2!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x3!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!a", 2000, 78d / 12d, sharedDelta);
            _AssertSeries(First(), "x3!a", 2001, 222d / 12d, sharedDelta);
            _AssertSeries(First(), "x3!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x4!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4!a", 2000, 1d, sharedDelta);
            _AssertSeries(First(), "x4!a", 2001, 13d, sharedDelta);
            _AssertSeries(First(), "x4!a", 2004, double.NaN, sharedDelta);

            _AssertSeries(First(), "x5!a", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "x5!a", 2000, 12d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2001, 24d, sharedDelta);
            _AssertSeries(First(), "x5!a", 2004, double.NaN, sharedDelta);
//
            I("RESET;");
            I("TIME 2000 2000;");
            I("OPTION freq m;");
            I("CREATE x;");
            I("SERIES x =   (1, 2, 3,   4, 5, 6,   7, 8, 9,   10, 11, 12);");
            I("COLLAPSE x1!q = x!m total; ");
            I("OPTION freq q;");
            I("COLLAPSE x2 = x!m; ");
            I("OPTION freq m;");
            I("COLLAPSE x3!q = x!m avg; ");
            I("COLLAPSE x4!q = x!m first; ");
            I("COLLAPSE x5!q = x!m last; ");
            I("OPTION freq q;");

            _AssertSeries(First(), "x1!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 1, 6d, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 2, 15d, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 3, 24d, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 4, 33d, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x2!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 1, 6d, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 2, 15d, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 3, 24d, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 4, 33d, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x3!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 1, 6d / 3d, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 2, 15d / 3d, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 3, 24d / 3d, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 4, 33d / 3d, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x4!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 1, 1d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 2, 4d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 3, 7d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 4, 10d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x5!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 1, 3d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 2, 6d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 3, 9d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 4, 12d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);


            
            //Testing series with holes (M/NaN) inside
            I("RESET;");
            I("TIME 2000 2000;");
            I("OPTION freq m;");
            I("CREATE x;");
            I("SERIES x =   (M(), 2, 3,   4, 5, 6,   7, M(), 9,   10, 11, M());");
            I("COLLAPSE x1!q = x!m total; ");
            I("OPTION freq q;");
            I("COLLAPSE x2 = x!m; ");
            I("OPTION freq m;");
            I("COLLAPSE x3!q = x!m avg; ");
            I("COLLAPSE x4!q = x!m first; ");
            I("COLLAPSE x5!q = x!m last; ");
            I("OPTION freq q;");

            _AssertSeries(First(), "x1!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 2, 15d, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 3, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x1!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x2!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 2, 15d, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 3, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x2!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x3!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 2, 15d / 3d, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 3, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x3!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x4!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 2, 4d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 3, 7d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2000, 4, 10d, sharedDelta);
            _AssertSeries(First(), "x4!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);

            _AssertSeries(First(), "x5!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 1, 3d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 2, 6d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 3, 9d, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "x5!q", EFreq.Q, 2001, 1, double.NaN, sharedDelta);
            
        }

        [TestMethod]
        public void _Test_For2()
        {
            //----------------------------------------------------------------------
            //Test of FOR
            //  - string loops
            //  - val loops
            //  - date loops
            //----------------------------------------------------------------------
            //------------------------------------
            // string loops
            //------------------------------------
            I("RESET;");
            I("LIST #l = list();");
            I("FOR string %i = a, b1; FOR string %j = ('1', '2a'); LIST #l = #l.append('{%i}{%j}'); END; END;");
            List<string> l = _GetListOfStrings("l");
            Assert.AreEqual(l.Count, 4);
            Assert.AreEqual(l[0], "a1");
            Assert.AreEqual(l[1], "a2a");
            Assert.AreEqual(l[2], "b11");
            Assert.AreEqual(l[3], "b12a");
            //------------------------------------
            I("LIST #l1 = list();");
            I("LIST #l2 = a, b1;");
            I("LIST #l3 = ('1', '2a');");
            I("FOR string %i = #l2; FOR string %j = #l3; #l1 = #l1.append('{%i}{%j}'); END; END;");
            List<string> l1 = _GetListOfStrings("l1");
            Assert.AreEqual(l1.Count, 4);
            Assert.AreEqual(l1[0], "a1");
            Assert.AreEqual(l1[1], "a2a");
            Assert.AreEqual(l1[2], "b11");
            Assert.AreEqual(l1[3], "b12a");
            //------------------------------------
            I("RESET;");
            I("LIST #l1 = list();");
            I("STRING %s1 = 'u';");
            I("LIST #l2 = a, b1;");
            I("FOR string %i = (%s1,) + #l2; #l1 = #l1.append(%i); END;");
            l1 = _GetListOfStrings("l1");
            Assert.AreEqual(l1.Count, 3);
            Assert.AreEqual(l1[0], "u");
            Assert.AreEqual(l1[1], "a");
            Assert.AreEqual(l1[2], "b1");
            //------------------------------------
            I("RESET;");
            I("VAL %s1 = 1.1;");
            FAIL("FOR string %i = %s1; TELL ''; END;");
            //------------------------------------
            I("RESET;");
            I("DATE %d1 = 2000;");
            FAIL("FOR string %i = %d1; TELL ''; END;");

            //------------------------------------
            // Parallel STRING loops
            //------------------------------------

            I("RESET;");
            I("LIST #m1 = a1, b1;");
            I("LIST #m2 = a2, b2;");
            I("LIST #m3 = a3, b3;");
            I("STRING %s = '';");
            I("FOR string %i=#m1 string %j=#m2; STRING %s = %s + '[{%i},{%j}]'; END;");
            _AssertScalarString(First(), "%s", "[a1,a2][b1,b2]");
            I("STRING %s = '';");
            I("FOR string %i=#m1 string %j=#m2 string %k=#m3; STRING %s = %s + '[{%i},{%j},{%k}]'; END;");
            _AssertScalarString(First(), "%s", "[a1,a2,a3][b1,b2,b3]");

            I("STRING %s = '';");
            I("FOR string %i=#m1 string %j=#m2; FOR string %ii=#m1 string %jj=#m2; STRING %s = %s + '[{%i},{%j},{%ii},{%jj}]'; END; END;");
            _AssertScalarString(First(), "%s", "[a1,a2,a1,a2][a1,a2,b1,b2][b1,b2,a1,a2][b1,b2,b1,b2]");

            FAIL("FOR string %i=#m1 string %i=#m2; END;");
            FAIL("FOR string %i=#m1 string %j=#m2 string %i=#m3; END;");
            I("LIST #m3 = a3, b3, c3;");
            FAIL("FOR string %i=#m1 string %j=#m2 string %k=#m3; END;");

            //------------------------------------
            // VAL loops
            //------------------------------------
            I("RESET;");
            I("TIME 2000 2000;");
            I("SERIES xx1 = 0;");
            I("FOR val %i = 1 to 9; SERIES xx1 = xx1 + %i; END;");            
            _AssertSeries(First(), "xx1", 2000, 45d, sharedDelta);
            //------------- just testing the doubledot (..) here:
            if (Globals.UNITTESTFOLLOWUP)
            {
                I("RESET;");
                I("TIME 2000 2000;");
                I("SERIES xx1 = 0;");
                I("FOR val %i = 1..9; SERIES xx1 = xx1 + %i; END;");
                _AssertSeries(First(), "xx1", 2000, 45d, sharedDelta);
            }
            //------------------------------------
            I("RESET;");
            I("TIME 2000 2000;");
            I("SERIES xx1 = 0;");
            I("FOR val %i = 1 to 9 by 2; SERIES xx1 = xx1 + %i; END;");
            _AssertSeries(First(), "xx1", 2000, 25d, sharedDelta);
            //------------------------------------
            I("RESET;");
            I("TIME 2000 2000;");
            I("SERIES xx1 = 0;");
            I("FOR val %i = 9 to 1 by -2; SERIES xx1 = xx1 + %i; END;");
            _AssertSeries(First(), "xx1", 2000, 25d, sharedDelta);
            //------------------------------------
            I("RESET;");
            I("TIME 2000 2000;");
            I("SERIES xx1 = 0;");
            I("VAL %x1 = 1;");
            I("VAL %x2 = 9;");
            I("VAL %x3 = 2;");
            I("FOR val %i = %x1+%x1-%x1 to %x2+%x1-%x1 by %x3+%x1-%x1; SERIES xx1 = xx1 + %i; END;");
            _AssertSeries(First(), "xx1", 2000, 25d, sharedDelta);
            //------------------------------------
            // val is converted into date here (only works for annual)
            I("RESET;");
            I("TIME 2000 2000;");
            I("SERIES xx1 = 0;");
            // one must explicitly convert #i to #d. Maybe at a later point "SERIES <date(#i) date(#i)>" could be made legal.
            I("FOR val %i = 2000 to 2003; DATE %d = %i; SERIES <%d %d> xx1 = %i; END;");
            _AssertSeries(First(), "xx1", 2000, 2000d, sharedDelta);
            _AssertSeries(First(), "xx1", 2001, 2001d, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, 2002d, sharedDelta);

            //------------------------------------
            // DATE loops
            //------------------------------------
            // val is converted into date here (only works for annual), #873502938
            // maybe a good idea with val(%d), else error-prone. The other way (fY[%v]) not so error-prone.
            I("RESET;");
            I("TIME 2000 2000;");
            I("DATE %d1 = 2000;");
            I("DATE %d2 = 2005;");
            I("VAL %v = 2;");
            I("SERIES xx1 = 0;");
            I("FOR date %d = %d1 to %d2 by %v; SERIES <%d %d> xx1 = val(%d); END;");
            _AssertSeries(First(), "xx1", 1998, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2000, 2000d, sharedDelta);
            _AssertSeries(First(), "xx1", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, 2002d, sharedDelta);
            _AssertSeries(First(), "xx1", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2004, 2004d, sharedDelta);
            _AssertSeries(First(), "xx1", 2005, double.NaN, sharedDelta);            
            _AssertSeries(First(), "xx1", 2006, double.NaN, sharedDelta);
            
            //--------------------------------------------------------------------------
            I("RESET;");
            I("TIME 2000 2000;");
            I("DATE %d1 = 2004;");
            I("DATE %d2 = 2000;");
            I("VAL %v = 2;");
            I("SERIES xx1 = 0;");
            I("FOR date %d = %d1 to %d2 by -%v; SERIES <%d %d> xx1 = val(%d); END;");
                        
            _AssertSeries(First(), "xx1", 1998, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2000, 2000d, sharedDelta);
            _AssertSeries(First(), "xx1", 2001, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, 2002d, sharedDelta);
            _AssertSeries(First(), "xx1", 2003, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2004, 2004d, sharedDelta);
            _AssertSeries(First(), "xx1", 2005, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2006, double.NaN, sharedDelta);
        }

        [TestMethod]
        public void _Test_If()
        {                        
            // Testing IF and logical operators etc.
            I("RESET;");

            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            I("OPTION freq q;");  //otherwise it will not work...!

            I("STRING %s1 = 'aBc';");
            I("STRING %s2 = 'aBcD';");
            I("VAL %v0 = 99.0;");
            I("VAL %v1 = 100.0;");
            I("VAL %v2 = 101.0;");
            I("VAL %v3 = miss();");
            I("DATE %d0 = 2000q3;");
            I("DATE %d1 = 2000q4;");
            I("DATE %d2 = 2001q1;");

            I("IF(%s1 == 'abc'); END;");  //empty test
            I("IF(%s1 == 'abc'); ELSE tell'hej'; END;");  //empty test
            I("IF(%s1 == 'abc'); tell 'hej'; ELSE ; END;");  //empty test
            I("IF(%s1 == 'abc'); ELSE ; END;");  //empty test

            //basic
            I("IF(%s1 == 'abc') VAL %q = 0; VAL %xx = 1; ELSE VAL %q = 0; VAL %xx = 0; END;");
            _AssertScalarVal(First(), "%xx", 1d);
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);

            I("IF(%s1 == 'abc ') VAL %q = 0; VAL %xx = 1; ELSE VAL %q = 0; VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%s1 == 'abc0') VAL %q = 0; VAL %xx = 1; ELSE VAL %q = 0; VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            //parentheses + not
            I("IF((%s1 == 'abc')) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(not %s1 == 'abc') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(not(%s1 == 'abc')) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            //or table
            I("IF(%s1 == 'abc' or %s2 == 'abcd') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%s1 == 'abc' or %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%s1 == 'abc7' or %s2 == 'abcd') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%s1 == 'abc7' or %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            //and table
            I("IF(%s1 == 'abc' and %s2 == 'abcd') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%s1 == 'abc' and %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%s1 == 'abc7' and %s2 == 'abcd') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%s1 == 'abc7' and %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            //precedence and over or
            I("IF(%s1 == 'abc' or %s2 == 'abcd7' and %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%s1 == 'abc' or (%s2 == 'abcd7' and %s2 == 'abcd7')) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF((%s1 == 'abc' or %s2 == 'abcd7') and %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            //precedence not
            I("IF(not %s1 == 'abc7' and %s2 == 'abcd7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(not( %s1 == 'abc7' and %s2 == 'abcd7')) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            //expressions
            I("IF(%s1 == 'abc' and %s1+%s2 == 'abcabcd' and %s1+%s2+%s1 == 'abcabcd'+%s1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            //decorated with a lot of parentheses
            I("IF(((%s1 == 'abc')) and (((%s1)+%s2) == ('abcabcd')) and (%s1+(%s2+(%s1))) == ('abcabcd'+%s1)) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            // == and <> operators
            I("IF(%s1 == 'abc') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%s1 <> 'abc') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%s1 == 'abc7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%s1 <> 'abc7') VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            I("IF(%v3 == miss()) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v3 <> miss()) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v2 == miss()) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v2 <> miss()) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            //values, relations

            I("IF(%v0+1-1 < 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v0+1-1 <= 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v0+1-1 == 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v0+1-1 >= 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v0+1-1 > 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v0+1-1 <> 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            I("IF(%v1+1-1 < 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v1+1-1 <= 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v1+1-1 == 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v1+1-1 >= 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v1+1-1 > 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v1+1-1 <> 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            I("IF(%v2+1-1 < 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v2+1-1 <= 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v2+1-1 == 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%v2+1-1 >= 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v2+1-1 > 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%v2+1-1 <> 100+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            //dates, relations

            I("IF(%d0-1 < 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d0-1 <= 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d0-1 == 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d0-1 >= 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d0-1 > 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d0-1 <> 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            I("IF(%d1-1 < 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d1-1 <= 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d1-1 == 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d1-1 >= 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d1-1 > 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d1-1 <> 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);

            I("IF(%d2-1 < 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d2-1 <= 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d2-1 == 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 0.0d);
            _AssertScalarVal(First(), "%xx", 0d);
            I("IF(%d2-1 >= 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d2-1 > 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(%d2-1 <> 2000q4-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            // FIXME FIXME FIXME
            I("OPTION freq a;");  //otherwise it will not work...!

            //indexer
            I("RESET;");
            I("CREATE y;");
            I("DATE %d = 2000;");
            I("VAL %v = 2000;");  //must be integer, else fail
            I("TIME 2000 2001;");
            I("SERIES y = 123;");
            I("IF(y[%d+1-1] == 123.0+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);
            I("IF(y[%v+1-1] == 123.0+1-1) VAL %xx = 1; ELSE VAL %xx = 0; END;");
            //Assert.AreEqual(Program.scalars["xx"].GetValOLD(null), 1.0d);
            _AssertScalarVal(First(), "%xx", 1d);

            //casting

            if (Globals.UNITTESTFOLLOWUP)
            {

                /*

                //casting VAL STRING
                I("IF(string('123')==string(123)) VAL xx = 1; ELSE VAL xx = 0; END;");
                Assert.AreEqual(Program.scalars["xx"].GetVal(), 1.0d);
                I("IF(val('123')==val(123)) VAL xx = 1; ELSE VAL xx = 0; END;");
                Assert.AreEqual(Program.scalars["xx"].GetVal(), 1.0d);

                //casting VAL DATE
                I("IF(date(2000a1)==date(2000.0)) VAL xx = 1; ELSE VAL xx = 0; END;");
                Assert.AreEqual(Program.scalars["xx"].GetVal(), 1.0d);
                I("IF(val(2000.0)==val(2000a1)) VAL xx = 1; ELSE VAL xx = 0; END;");
                Assert.AreEqual(Program.scalars["xx"].GetVal(), 1.0d);

                //casting STRING DATE
                I("IF(date(2000a1)==date('2000')) VAL xx = 1; ELSE VAL xx = 0; END;");
                Assert.AreEqual(Program.scalars["xx"].GetVal(), 1.0d);
                I("IF(string('2000')==string(2000a1)) VAL xx = 1; ELSE VAL xx = 0; END;");
                Assert.AreEqual(Program.scalars["xx"].GetVal(), 1.0d);

                */
            }


        }

        [TestMethod]
        public void _Test_List()
        {
            I("#m = ('a', 'b');");

            I("p #m['a'];");
            I("%v1 = #m['a'];");
            _AssertScalarVal(First(), "%v1", 1d);

            I("p #m['c'];");
            I("%v2 = #m['c'];");
            _AssertScalarVal(First(), "%v2", 0d);

            I("p #m['a*'];");
            I("#v3 = #m['*'];");
            _AssertListSize(First(), "#v3", 2);
            _AssertListString(First(), "#v3", 1, "a");
            _AssertListString(First(), "#v3", 2, "b");

            I("#m = ('a', [1,2;3,4]);");
            I("p #m[2];");
            I("p #m[2][1,1];");
            Assert.AreEqual(((First().GetIVariable("#m") as List).list[1] as Matrix).data[1, 1], 4d);

            I("#m = (('a', 'b'), 'c');");
            Assert.AreEqual((((First().GetIVariable("#m") as List).list[0] as List).list[1] as ScalarString).string2, "b");
            I("#m[1][2]='x';");
            I("p #m[1][2];");

            //test listfiles
            I("#(listfile m) = a, b, c;");
            I("#(listfile m) = #(listfile m) + #(listfile m);");
            I("%v = #(listfile m).length();");
            _AssertScalarVal(First(), "%v", 6);

            I("#(listfile {'n'}) = a, b, c;");
            I("#(listfile {'n'}) = #(listfile {'n'}) + #(listfile {'n'});");
            I("%v = #(listfile {'n'}).length();");
            _AssertScalarVal(First(), "%v", 6);

            string p = Globals.ttPath2 + @"\regres\mm";
            I("#(listfile {'" + p + "'}) = a, b, c;");
            I("#(listfile {'" + p + "'}) = #(listfile {'" + p + "'}) + #(listfile {'" + p + "'});");
            I("%v = #(listfile {'" + p + "'}).length();");
            _AssertScalarVal(First(), "%v", 6);

            I("time 2000 2000;");
            I("#(listfile i) = a, b;");
            I("x = series(1); x[a] = 1; x[b] = 2;");
            if (Globals.UNITTESTFOLLOWUP_important)
            {
                I("y = sum(#(listfile i), x[#(listfile i)]);");  //using listfile in sum(), only simple i accepted.            
            _AssertSeries(First(), "y", 2000, 3, sharedDelta);
            }
            FAIL("y = sum(#(listfile {'i'}), x[#(listfile {'i'})]);");  //using listfile in sum(), only simple i accepted.



            //----------------------------------------------------------------------
            //Test of LIST
            //----------------------------------------------------------------------
            I("RESET;");
            I("LIST #x1 = a, b, c, d, e;");
            Assert.AreEqual(_GetListOfStrings("x1").Count, 5);
            I("LIST #x2 = ('0', '00', '000');");
            Assert.AreEqual(_GetListOfStrings("x2").Count, 3);
            if (Globals.UNITTESTFOLLOWUP)
            {
                I("LIST <direct> #x2d = 0, 00, 000;");
                Assert.AreEqual(_GetListOfStrings("x2d").Count, 3);
                I("LIST <direct> #x3d = 0e, 00e, 000e;");
                Assert.AreEqual(_GetListOfStrings("x3d").Count, 3);
            }
            I("LIST #x3 = ('0e', '00e', '000e');");
            Assert.AreEqual(_GetListOfStrings("x3").Count, 3);            
            FAIL("LIST #x4 = #x1, #x2, #x3;");
            I("LIST #x4 = #x1 + #x2 + #x3;");
            Assert.AreEqual(_GetListOfStrings("x4").Count, 11);
            I("STRING %s1 = 'tt';");
            FAIL("LIST #x5 = %s1, %s1;");
            I("LIST #x5 = (%s1, %s1);");
            Assert.AreEqual(_GetListOfStrings("x5").Count, 2);
            I("LIST #x6 = x%s1, x{%s1};");
            Assert.AreEqual(_GetListOfStrings("x6").Count, 2);
            Assert.AreEqual(_GetListOfStrings("x6")[0], "xtt");
            Assert.AreEqual(_GetListOfStrings("x6")[1], "xtt");
            I("LIST #x6a = ('x' + %s1, 'x{%s1}');");
            Assert.AreEqual(_GetListOfStrings("x6a").Count, 2);
            Assert.AreEqual(_GetListOfStrings("x6a")[0], "xtt");
            Assert.AreEqual(_GetListOfStrings("x6a")[1], "xtt");
            I("LIST #x7 = a, b;");
            I("LIST #x7 = #x7.append('c').append('d');");
            Assert.AreEqual(_GetListOfStrings("x7").Count, 4);
            I("LIST #x7 = a, b;");
            I("LIST #x7 = #x7.extend(('c', 'd'));");  //or use '+'
            Assert.AreEqual(_GetListOfStrings("x7").Count, 4);
            I("LIST #x8 = list();");
            I("LIST #x8 = #x8 + ('c', 'd');"); //instead of extend()
            Assert.AreEqual(_GetListOfStrings("x8").Count, 2);  //such nesting is ok, equivalent to adding
            I("LIST #x9 = list();");
            Assert.AreEqual(_GetListOfStrings("x9").Count, 0);
            I("LIST #x10 = list(); LIST #x10 = list();");
            Assert.AreEqual(_GetListOfStrings("x9").Count, 0);

            //  ---------------------
            //  WILDCARDS start
            //  ---------------------
            I("LIST #y = abc, cde, abcd, abxcd, abxxcd, abxxxcd, af, ae;");
            I("LIST #y1 = #y['a*'];");
            List<string> y1 = _GetListOfStrings("y1");
            Assert.AreEqual(y1.Count, 7);
            Assert.AreEqual(y1[0], "abc");
            Assert.AreEqual(y1[1], "abcd");
            Assert.AreEqual(y1[2], "abxcd");
            Assert.AreEqual(y1[3], "abxxcd");
            Assert.AreEqual(y1[4], "abxxxcd");
            Assert.AreEqual(y1[5], "af");
            Assert.AreEqual(y1[6], "ae");  //not sorted

            I("LIST #y2 = #y['ab*'];");
            List<string> y2 = _GetListOfStrings("y2");
            Assert.AreEqual(y2.Count, 5);
            Assert.AreEqual(y2[0], "abc");
            Assert.AreEqual(y2[1], "abcd");
            Assert.AreEqual(y2[2], "abxcd");
            Assert.AreEqual(y2[3], "abxxcd");
            Assert.AreEqual(y2[4], "abxxxcd");

            I("LIST #y3 = #y['a?'];");
            List<string> y3 = _GetListOfStrings("y3");
            Assert.AreEqual(y3.Count, 2);
            Assert.AreEqual(y3[0], "af");
            Assert.AreEqual(y3[1], "ae");  //not sorted         

            I("LIST #y4 = #y['ab?'];");
            List<string> y4 = _GetListOfStrings("y4");
            Assert.AreEqual(y4.Count, 1);
            Assert.AreEqual(y4[0], "abc");

            I("LIST #y5 = #y['ab*cd'];");
            List<string> y5 = _GetListOfStrings("y5");
            Assert.AreEqual(y5.Count, 4);
            Assert.AreEqual(y5[0], "abcd");
            Assert.AreEqual(y5[1], "abxcd");
            Assert.AreEqual(y5[2], "abxxcd");
            Assert.AreEqual(y5[3], "abxxxcd");

            I("LIST #y6 = #y['ab?cd'];");
            List<string> y6 = _GetListOfStrings("y6");
            Assert.AreEqual(y6.Count, 1);
            Assert.AreEqual(y6[0], "abxcd");

            I("LIST #y7 = #y['*e'];");
            List<string> y7 = _GetListOfStrings("y7");
            Assert.AreEqual(y7.Count, 2);
            Assert.AreEqual(y7[0], "cde");  //not sorted
            Assert.AreEqual(y7[1], "ae");            

            I("LIST #y8 = #y['?e'];");
            List<string> y8 = _GetListOfStrings("y8");
            Assert.AreEqual(y8.Count, 1);
            Assert.AreEqual(y8[0], "ae");

            I("LIST #y9 = #y['*'];");
            List<string> y9 = _GetListOfStrings("y9");
            Assert.AreEqual(y9.Count, 8);

            I("LIST #y10 = #y['?'];");
            List<string> y10 = _GetListOfStrings("y10");
            Assert.AreEqual(y10.Count, 0);

            //  ---------------------
            //  WILDCARDS end
            //  ---------------------

            //  ---------------------
            //  NAME RANGES start
            //  ---------------------
            //  Parser-wise these are much easier than wildcards

            I("LIST #z1 = #y['ab'..'ae'];");
            List<string> z1 = _GetListOfStrings("z1");
            Assert.AreEqual(z1.Count, 6);

            //  ---------------------
            //  NAME RANGES end
            //  ---------------------

            //==================== LIST operators etc. =======================================

            Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
            Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");

            I("list #a = a4, a2, a3, a1;");
            I("val %i = 2;");
            I("val %n = #a.length();                                      //number of items");
            _AssertScalarVal(First(), "%n", 4d);
            I("list #a2 = #a[%i..%i+1];                                   //sublist");
            _AssertHelperList("a2", new List<string>() { "a2", "a3" });
            I("string %a3 = #a[%i+1];                                    //single element");
            _AssertScalarString(First(), "%a3", "a3");
            I("list #a3 = (#a[%i+1],);                                   //single element");
            _AssertHelperList("a3", new List<string>() { "a3" });
            I("list #a4 = #a.sort();                                       //sort");
            _AssertHelperList("a4", new List<string>() { "a1", "a2", "a3", "a4" });
            I("list #a5 = #a.prefix('pf').suffix('sf');                    //pre/suffix");
            _AssertHelperList("a5", new List<string>() { "pfa4sf", "pfa2sf", "pfa3sf", "pfa1sf" });            
            I("list #a6 = #a5.replaceinside('pf', '');"); //without 'inside' it will look for a whole string
            _AssertHelperList("a6", new List<string>() { "a4sf", "a2sf", "a3sf", "a1sf" });
            I("list #a7 = #a['a2'..'a3'];                                 //sublist with alphabetical range");
            _AssertHelperList("a7", new List<string>() { "a2", "a3" });
            I("list #a8 = #a['a*'];                                      //sublist with wildcard");
            _AssertHelperList("a8", new List<string>() { "a4", "a2", "a3", "a1" });
            I("list #a9 = #a['a?'];                                      //sublist with wildcard");
            _AssertHelperList("a9", new List<string>() { "a4", "a2", "a3", "a1" });
            I("list #x1 = a1, a2, a3, a4;");
            I("list #x2 = a2, a3, a5, a6;");
            I("list #x3 = #x1 + #x2;                                      //concatenation");
            _AssertHelperList("x3", new List<string>() { "a1", "a2", "a3", "a4", "a2", "a3", "a5", "a6" });
            I("list #x4 = union(#x1, #x2);                                     //union");
            _AssertHelperList("x4", new List<string>() { "a1", "a2", "a3", "a4", "a5", "a6" });
            I("list #x5 = except(#x1, #x2);                                     //difference");
            _AssertHelperList("x5", new List<string>() { "a1", "a4" });
            I("list #x6 = intersect(#x1, #x2);                                     //intersection");
            _AssertHelperList("x6", new List<string>() { "a2", "a3" });
            I("list #(listfile b) = b1, b2, b3, b4;                        //creates b.lst, accepts path and file too");
            I("list #(listfile c) = c1, c2, c3, c4;");
            I("list #(listfile bc) = union(#(listfile b), #(listfile c));        //listfiles work like normal lists");
            I("list #bc2 = #(listfile bc);");
            _AssertHelperList("bc2", new List<string>() { "b1", "b2", "b3", "b4", "c1", "c2", "c3", "c4" });

            //dublets
            I("list #x1 = a1, a2, a3, a4, a1;");
            I("list #x2 = a2, a3, a5, a6, a2;");
            I("list #x3 = #x1 + #x2;                                      //concatenation");
            _AssertHelperList("x3", new List<string>() { "a1", "a2", "a3", "a4", "a1", "a2", "a3", "a5", "a6", "a2" });
            I("list #x4 = union(#x1, #x2);                                     //union");
            _AssertHelperList("x4", new List<string>() { "a1", "a2", "a3", "a4", "a1", "a5", "a6" });
            I("list #x5 = except(#x1, #x2);                                     //difference");
            _AssertHelperList("x5", new List<string>() { "a1", "a4", "a1" });
            I("list #x6 = intersect(#x1, #x2);                                     //intersection");
            _AssertHelperList("x6", new List<string>() { "a2", "a3" });
            I("list #x7 = #x1.unique();");
            _AssertHelperList("x7", new List<string>() { "a1", "a2", "a3", "a4" });
            I("list #x8 = #x1.sort().unique();");
            _AssertHelperList("x8", new List<string>() { "a1", "a2", "a3", "a4" });



        }

        [TestMethod]
        public void _Test_TimeLogic()
        {
            //----------------------------------------------------------------------
            //Test of CloneAndAdd() method for quarters (months will probably be ok as well then)
            //----------------------------------------------------------------------

            GekkoTime[] gts = new GekkoTime[20];
            int counter = 0;
            for (int y = 2000; y <= 2004; y++)
            {
                for (int q = 1; q <= 4; q++)
                {
                    GekkoTime gt = new GekkoTime(EFreq.Q, y, q);
                    gts[counter] = gt;
                    counter++;
                }
            }
            counter = 0;
            for (int y = 2000; y <= 2004; y++)
            {
                for (int q = 1; q <= 4; q++)
                {
                    GekkoTime gt = gts[counter];
                    GekkoTime gtsLag = gt.Add(-1);
                    GekkoTime gtsLead = gt.Add(+1);
                    GekkoTime gtsLag5 = gt.Add(-5);
                    GekkoTime gtsLead5 = gt.Add(+5);
                    if (counter > 0) Assert.AreEqual(gts[counter - 1].super, gtsLag.super);
                    if (counter > 0) Assert.AreEqual(gts[counter - 1].sub, gtsLag.sub);
                    if (counter > 4) Assert.AreEqual(gts[counter - 5].super, gtsLag5.super);
                    if (counter > 4) Assert.AreEqual(gts[counter - 5].sub, gtsLag5.sub);
                    if (counter < 19) Assert.AreEqual(gts[counter + 1].super, gtsLead.super);
                    if (counter < 19) Assert.AreEqual(gts[counter + 1].sub, gtsLead.sub);
                    if (counter < 15) Assert.AreEqual(gts[counter + 5].super, gtsLead5.super);
                    if (counter < 15) Assert.AreEqual(gts[counter + 5].sub, gtsLead5.sub);
                    counter++;
                }
            }

            //----------------------------------------------------------------------
            //Test of GetPeriod() from Series (converts an index back to GekkoTime)
            //----------------------------------------------------------------------

            //Testing annual
            for (int ii = 1; ii <= 1; ii++)
            {
                Series ts = new Series(EFreq.A, "testing!a");
                ts.SetData(new GekkoTime(EFreq.A, 2000, ii), 12345d);  //2000q1 will usually have index 100 then.
                int index = 100;
                GekkoTime gt0 = ts.GetPeriod(index);
                for (int i = -20; i <= 20; i++)
                {
                    int j = index + i;
                    GekkoTime temp = ts.GetPeriod(j);
                    GekkoTime gt = gt0.Add(i);
                    Assert.AreEqual(gt.super, temp.super);
                    Assert.AreEqual(gt.sub, temp.sub);
                    Assert.AreEqual(temp.sub, 1);
                }
            }

            //Testing quarterly (more fragile)
            for (int ii = 1; ii <= 4; ii++)
            {
                Series ts = new Series(EFreq.Q, "testing!q");
                ts.SetData(new GekkoTime(EFreq.Q, 2000, ii), 12345d);  //2000q1 will usually have index 100 then.
                int index = 100;
                GekkoTime gt0 = ts.GetPeriod(index);
                for (int i = -20; i <= 20; i++)
                {
                    int j = index + i;
                    GekkoTime temp = ts.GetPeriod(j);
                    GekkoTime gt = gt0.Add(i);
                    Assert.AreEqual(gt.super, temp.super);
                    Assert.AreEqual(gt.sub, temp.sub);
                }
            }

            //Testing monthly (more fragile)
            for (int ii = 1; ii <= 12; ii++)
            {
                Series ts = new Series(EFreq.M, "testing!m");
                ts.SetData( new GekkoTime(EFreq.M, 2000, ii), 12345d);  //2000q1 will usually have index 100 then.
                int index = 100;
                GekkoTime gt0 = ts.GetPeriod(index);
                for (int i = -20; i <= 20; i++)
                {
                    int j = index + i;
                    GekkoTime temp = ts.GetPeriod(j);
                    GekkoTime gt = gt0.Add(i);
                    Assert.AreEqual(gt.super, temp.super);
                    Assert.AreEqual(gt.sub, temp.sub);
                }
            }

            //----------------------------------------------------------------------
            //Testing GekkoTimeIterator
            //Testing Observations()
            //----------------------------------------------------------------------

            int c = 0;
            int c1 = 0;
            foreach (GekkoTime gt1 in new GekkoTimeIterator(new GekkoTime(EFreq.A, 2000, 1), new GekkoTime(EFreq.A, 2004, 1)))
            {
                int c2 = 0;
                foreach (GekkoTime gt2 in new GekkoTimeIterator(new GekkoTime(EFreq.A, 2000, 1), new GekkoTime(EFreq.A, 2004, 1)))
                {
                    int obs = GekkoTime.Observations(gt1, gt2);
                    int obsTrue = c2 - c1 + 1;
                    Assert.AreEqual(obs, obsTrue);
                    c2++;
                    c++;
                }
                c1++;
            }
            Assert.AreEqual(c, 25);

            c = 0;
            c1 = 0;

            foreach (GekkoTime gt1 in new GekkoTimeIterator(new GekkoTime(EFreq.Q, 2000, 1), new GekkoTime(EFreq.Q, 2003, 1)))
            {
                int c2 = 0;
                foreach (GekkoTime gt2 in new GekkoTimeIterator(new GekkoTime(EFreq.Q, 2000, 1), new GekkoTime(EFreq.Q, 2003, 1)))
                {
                    int obs = GekkoTime.Observations(gt1, gt2);
                    int obsTrue = c2 - c1 + 1;
                    Assert.AreEqual(obs, obsTrue);
                    c2++;
                    c++;
                }
                c1++;
            }
            Assert.AreEqual(c, 13 * 13);

            c = 0;
            c1 = 0;
            foreach (GekkoTime gt1 in new GekkoTimeIterator(new GekkoTime(EFreq.M, 2000, 1), new GekkoTime(EFreq.M, 2002, 1)))
            {
                int c2 = 0;
                foreach (GekkoTime gt2 in new GekkoTimeIterator(new GekkoTime(EFreq.M, 2000, 1), new GekkoTime(EFreq.M, 2002, 1)))
                {
                    int obs = GekkoTime.Observations(gt1, gt2);
                    int obsTrue = c2 - c1 + 1;
                    Assert.AreEqual(obs, obsTrue);
                    c2++;
                    c++;
                }
                c1++;
            }
            Assert.AreEqual(c, 25 * 25);

            // ----------------------------------------------------------
            // ------------- testing global time and time iterator ------
            // ----------------------------------------------------------

            I("RESET;");
            //Setting tqQ to 0.12345 in 2000q2, and augmenting 1 per quarter up to 2002q3.
            I("Option freq q;");
            I("CREATE tsQ;");
            I("TIME 2000q2 2000q2;");
            I("SERIES tsQ = 0.12345;");
            I("TIME 2000q3 2002q3;");
            I("SERIES tsQ ^= 1;");
            GekkoTime ggt1 = new GekkoTime(EFreq.Q, 2000, 2);
            GekkoTime ggt2 = new GekkoTime(EFreq.Q, 2002, 3);
            Assert.IsTrue(double.IsNaN((First().GetIVariable("tsQ!q") as Series).GetDataSimple(ggt1.Add(-1))));
            Assert.IsTrue(double.IsNaN((First().GetIVariable("tsQ!q") as Series).GetDataSimple(ggt2.Add(1))));
            counter = 0;
            foreach (GekkoTime gt in new GekkoTimeIterator(ggt1, ggt2))
            {
                Assert.AreEqual((First().GetIVariable("tsQ!q") as Series).GetDataSimple(gt), 0.12345d + (double)counter);
                counter++;
            }

            // ----------------------------------------------------------
            // Testing integers used with periods where freq is Q or M
            // ----------------------------------------------------------

            I("RESET;");
            I("Option freq q;");
            I("TIME 2000 2002;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.Q);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2000);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 1);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2002);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 4);

            I("RESET;");
            I("Option freq q;");
            I("TIME 2000q3 2002;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.Q);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2000);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 3);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2002);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 4);

            I("RESET;");
            I("Option freq m;");
            I("TIME 2000 2002;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2000);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 1);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2002);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 12);

            I("RESET;");
            I("Option freq q;");
            I("SERIES<2000 2000>xx = 1;");
            UData u;
            u = Data("xx", 1999, 4, "q"); Assert.AreEqual(u.w, double.NaN);
            u = Data("xx", 2000, 1, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 2, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 3, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 4, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2001, 1, "q"); Assert.AreEqual(u.w, double.NaN);

            I("RESET;");
            I("Option freq q;");
            I("SERIES<2000 2000q3>xx = 1;");
            u = Data("xx", 1999, 4, "q"); Assert.AreEqual(u.w, double.NaN);
            u = Data("xx", 2000, 1, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 2, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 3, "q"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 4, "q"); Assert.AreEqual(u.w, double.NaN);

            I("RESET;");
            I("Option freq m;");
            I("SERIES<2000 2000>xx = 1;");
            u = Data("xx", 1999, 12, "m"); Assert.AreEqual(u.w, double.NaN);
            u = Data("xx", 2000, 1, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 2, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 3, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 4, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 5, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 6, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 7, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 8, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 9, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 10, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 11, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2000, 12, "m"); Assert.AreEqual(u.w, 1);
            u = Data("xx", 2001, 1, "m"); Assert.AreEqual(u.w, double.NaN);

        }

        [TestMethod]
        public void _Test_TimeSeries()
        {
            //Chreates long timeseries running from 1500 to 3000 yers (a, q or m, so many periods for q and m).
            //For putting data in: testing both SetData() and GetDataSequence() and setting the array value.
            //Testing that the data reads out correctly, both with GetData() and GetDataSequence()
            //Puts in 1500 / 1501 etc. for 'a', 1500.01 / 1500.02 / 1500.03 / 1500.04 / 1501.01 etc. for 'q'
            Series tsA1 = new Series(EFreq.A, "test!a");
            Series tsQ1 = new Series(EFreq.Q, "test!q");
            Series tsM1 = new Series(EFreq.M, "test!m");
            Series tsA2 = new Series(EFreq.A, "test!a");
            Series tsQ2 = new Series(EFreq.Q, "test!q");
            Series tsM2 = new Series(EFreq.M, "test!m");
            int start = 1500;
            int end = 3000;
            //writing data to timeseries
            for (int i = start; i <= end; i++)
            {
                tsA1.SetData(new GekkoTime(EFreq.A, i, 1), (double)i);
                {
                    int index1 = 0;
                    int index2 = 0;
                    double[] data = tsA2.GetDataSequence(out index1, out index2, new GekkoTime(EFreq.A, i, 1), new GekkoTime(EFreq.A, i, 1));
                    Assert.AreEqual(index1, index2);
                    data[index1] = (double)i;
                }

                for (int j = 1; j <= 4; j++)
                {
                    tsQ1.SetData(new GekkoTime(EFreq.Q, i, j), (double)i + (double)j / 100d);
                    {
                        int index1 = 0;
                        int index2 = 0;
                        double[] data = tsQ2.GetDataSequence(out index1, out index2, new GekkoTime(EFreq.Q, i, j), new GekkoTime(EFreq.Q, i, j));
                        Assert.AreEqual(index1, index2);
                        data[index1] = (double)i + (double)j / 100d;
                    }
                }
                for (int j = 1; j <= 12; j++)
                {
                    tsM1.SetData(new GekkoTime(EFreq.M, i, j), (double)i + (double)j / 100d);
                    int index1 = 0;
                    int index2 = 0;
                    double[] data = tsM2.GetDataSequence(out index1, out index2, new GekkoTime(EFreq.M, i, j), new GekkoTime(EFreq.M, i, j));
                    Assert.AreEqual(index1, index2);
                    data[index1] = (double)i + (double)j / 100d;
                }
            }

            for (int i = start; i <= end; i++)
            {
                tsA1.SetData(new GekkoTime(EFreq.A, i, 1), (double)i);
                {
                    int index1 = 0;
                    int index2 = 0;
                    double[] data = tsA2.GetDataSequenceUnsafePointerReadOnly(out index1, out index2, new GekkoTime(EFreq.A, i, 1), new GekkoTime(EFreq.A, i, 1));
                    Assert.AreEqual(index1, index2);
                    data[index1] = (double)i;
                }

                for (int j = 1; j <= 4; j++)
                {
                    tsQ1.SetData(new GekkoTime(EFreq.Q, i, j), (double)i + (double)j / 100d);
                    {
                        int index1 = 0;
                        int index2 = 0;
                        double[] data = tsQ2.GetDataSequenceUnsafePointerReadOnly(out index1, out index2, new GekkoTime(EFreq.Q, i, j), new GekkoTime(EFreq.Q, i, j));
                        Assert.AreEqual(index1, index2);
                        data[index1] = (double)i + (double)j / 100d;
                    }
                }
                for (int j = 1; j <= 12; j++)
                {
                    tsM1.SetData(new GekkoTime(EFreq.M, i, j), (double)i + (double)j / 100d);
                    int index1 = 0;
                    int index2 = 0;
                    double[] data = tsM2.GetDataSequenceUnsafePointerReadOnly(out index1, out index2, new GekkoTime(EFreq.M, i, j), new GekkoTime(EFreq.M, i, j));
                    Assert.AreEqual(index1, index2);
                    data[index1] = (double)i + (double)j / 100d;
                }
            }

            //testing that data is correct when reading

            for (int i = start; i <= end; i++)
            {
                Assert.AreEqual(tsA1.GetDataSimple(new GekkoTime(EFreq.A, i, 1)), (double)i);
                Assert.AreEqual(tsA2.GetDataSimple(new GekkoTime(EFreq.A, i, 1)), (double)i);
                for (int j = 1; j <= 4; j++)
                {
                    Assert.AreEqual(tsQ1.GetDataSimple(new GekkoTime(EFreq.Q, i, j)), (double)i + (double)j / 100d);
                    Assert.AreEqual(tsQ2.GetDataSimple(new GekkoTime(EFreq.Q, i, j)), (double)i + (double)j / 100d);
                }
                for (int j = 1; j <= 12; j++)
                {
                    Assert.AreEqual(tsM1.GetDataSimple(new GekkoTime(EFreq.M, i, j)), (double)i + (double)j / 100d);
                    Assert.AreEqual(tsM2.GetDataSimple(new GekkoTime(EFreq.M, i, j)), (double)i + (double)j / 100d);
                }
            }

            Assert.IsTrue(double.IsNaN(tsA1.GetDataSimple(new GekkoTime(EFreq.A, start - 1, 1))));
            Assert.IsTrue(double.IsNaN(tsA1.GetDataSimple(new GekkoTime(EFreq.A, end + 1, 1))));
            Assert.IsTrue(double.IsNaN(tsA2.GetDataSimple(new GekkoTime(EFreq.A, start - 1, 1))));
            Assert.IsTrue(double.IsNaN(tsA2.GetDataSimple(new GekkoTime(EFreq.A, end + 1, 1))));

            Assert.IsTrue(double.IsNaN(tsA1.GetDataSimple((new GekkoTime(EFreq.A, start, 1)).Add(-1))));
            Assert.IsTrue(double.IsNaN(tsA1.GetDataSimple((new GekkoTime(EFreq.A, end, 1)).Add(1))));
            Assert.IsTrue(double.IsNaN(tsA2.GetDataSimple((new GekkoTime(EFreq.A, start, 1)).Add(-1))));
            Assert.IsTrue(double.IsNaN(tsA2.GetDataSimple((new GekkoTime(EFreq.A, end, 1)).Add(1))));

            Assert.IsTrue(double.IsNaN(tsQ1.GetDataSimple((new GekkoTime(EFreq.Q, start, 1)).Add(-1))));
            Assert.IsTrue(double.IsNaN(tsQ1.GetDataSimple((new GekkoTime(EFreq.Q, end, 4)).Add(1))));
            Assert.IsTrue(double.IsNaN(tsQ2.GetDataSimple((new GekkoTime(EFreq.Q, start, 1)).Add(-1))));
            Assert.IsTrue(double.IsNaN(tsQ2.GetDataSimple((new GekkoTime(EFreq.Q, end, 4)).Add(1))));

            Assert.IsTrue(double.IsNaN(tsM1.GetDataSimple((new GekkoTime(EFreq.M, start, 1)).Add(-1))));
            Assert.IsTrue(double.IsNaN(tsM1.GetDataSimple((new GekkoTime(EFreq.M, end, 12)).Add(1))));
            Assert.IsTrue(double.IsNaN(tsM2.GetDataSimple((new GekkoTime(EFreq.M, start, 1)).Add(-1))));
            Assert.IsTrue(double.IsNaN(tsM2.GetDataSimple((new GekkoTime(EFreq.M, end, 12)).Add(1))));

            //testing getting data with GetDataSequence()
            List<Series> listA = new List<Series>();
            listA.Add(tsA1);
            listA.Add(tsA2);
            foreach (Series tsA in listA)
            {
                int index1 = 0;
                int index2 = 0;
                double[] data = tsA.GetDataSequence(out index1, out index2, new GekkoTime(EFreq.A, start - 1, 1), new GekkoTime(EFreq.A, end + 1, 1));
                int counter = 0;
                for (int i = index1; i <= index2; i++)
                {
                    double v = data[i];
                    if (i == index1 || i == index2)
                    {
                        Assert.IsTrue(double.IsNaN(v));
                    }
                    else Assert.AreEqual(v, (double)(start + counter - 1));
                    counter++;
                }
            }

            List<Series> listQ = new List<Series>();
            listQ.Add(tsQ1);
            listQ.Add(tsQ2);
            foreach (Series tsQ in listQ)
            {
                int index1 = 0;
                int index2 = 0;
                double[] data = tsQ.GetDataSequence(out index1, out index2, new GekkoTime(EFreq.Q, start, 1).Add(-1), new GekkoTime(EFreq.Q, end, 4).Add(1));
                int counter = 0;
                for (int i = index1; i <= index2; i++)
                {
                    double v = data[i];
                    if (i == index1 || i == index2)
                    {
                        Assert.IsTrue(double.IsNaN(v));
                    }
                    else Assert.AreEqual(v, (double)(start + (counter - 1) / 4) + (double)((counter - 1) % 4 + 1) / 100d);
                    counter++;
                }
            }

            List<Series> listM = new List<Series>();
            listM.Add(tsM1);
            listM.Add(tsM2);
            foreach (Series tsM in listM)
            {
                int index1 = 0;
                int index2 = 0;
                double[] data = tsM.GetDataSequence(out index1, out index2, new GekkoTime(EFreq.M, start, 1).Add(-1), new GekkoTime(EFreq.M, end, 12).Add(1));
                int counter = 0;
                for (int i = index1; i <= index2; i++)
                {
                    double v = data[i];
                    if (i == index1 || i == index2)
                    {
                        Assert.IsTrue(double.IsNaN(v));
                    }
                    else Assert.AreEqual(v, (double)(start + (counter - 1) / 12) + (double)((counter - 1) % 12 + 1) / 100d);
                    counter++;
                }
            }
        }


        [TestMethod]
        public void Test__Rebase()
        {            
            I("RESET;");
            I("TIME 2010 2012;");
            I("MODE data;");
            I("SER y1 = (-7, 3, 4);");
            I("OPEN<edit>temp;");
            I("SER y2 = (7, -3, -4);");
            I("CLOSE temp; OPEN temp;");
            FAIL("REBASE <bank=work prefix=re index = 100> y1, temp:y2 2011;");
            I("UNLOCK temp;");
            I("REBASE <bank=work prefix=re index = 100> y1, temp:y2 2011;");
            _AssertSeries(First(), "rey1", 2010, -7d / 3d * 100d, sharedDelta);
            _AssertSeries(First(), "rey1", 2011, 3d / 3d * 100d, sharedDelta);
            _AssertSeries(First(), "rey1", 2012, 4d / 3d * 100d, sharedDelta);            
            _AssertSeries(Program.databanks.GetDatabank("temp"), "rey2", 2010, 7d / (-3d) * 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "rey2", 2011, -3d / (-3d) * 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "rey2", 2012, -4d / (-3d) * 100d, sharedDelta);
            I("REBASE <bank=work prefix=re index = 100> y1, temp:y2 2011 2012;");
            _AssertSeries(First(), "rey1", 2010, -7d / ((3d + 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(First(), "rey1", 2011, 3d / ((3d + 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(First(), "rey1", 2012, 4d / ((3d + 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "rey2", 2010, 7d / ((-3d - 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "rey2", 2011, -3d / ((-3d - 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "rey2", 2012, -4d / ((-3d - 4d) / 2d) * 100d, sharedDelta);
            FAIL("REBASE <bank=work prefix=re index = 100> y1, temp:y2 2011 2013;");
            FAIL("REBASE <bank=work prefix=re index = 100> y1 2010 2012;");
            FAIL("REBASE <bank=work prefix=re index = 100> temp:y2 2010 2012;");
            FAIL("REBASE y1 2012 2011;");
            FAIL("REBASE y1 2010q1 2012;");
            FAIL("REBASE y1 2010m1 2012q4;");
            I("LIST m = work:y1, y2;");
            I("REBASE <bank=temp index = 1> #m 2010;");
            _AssertSeries(First(), "y1", 2010, -7d / (-7d) * 1d, sharedDelta);
            _AssertSeries(First(), "y1", 2011, 3d / (-7d) * 1d, sharedDelta);
            _AssertSeries(First(), "y1", 2012, 4d / (-7d) * 1d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "y2", 2010, 7d / (7d) * 1d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "y2", 2011, -3d / (7d) * 1d, sharedDelta);
            _AssertSeries(Program.databanks.GetDatabank("temp"), "y2", 2012, -4d / (7d) * 1d, sharedDelta);

            //quarterly
            I("OPTION freq q;");
            I("TIME 2010q1 2010q3;");
            I("SER z1 = -7, 3, 4;");
            I("REBASE z1 2010q2 2010q3;");
            _AssertSeries(First(), "z1", EFreq.Q, 2009, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "z1", EFreq.Q, 2010, 1, -7d / ((3d + 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(First(), "z1", EFreq.Q, 2010, 2, 3d / ((3d + 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(First(), "z1", EFreq.Q, 2010, 3, 4d / ((3d + 4d) / 2d) * 100d, sharedDelta);
            _AssertSeries(First(), "z1", EFreq.Q, 2010, 4, double.NaN, sharedDelta);

            //using a whole year indicator for quarterly
            I("TIME 2010q4 2012q1;");
            I("SER z2 = 1, 2, 3, 4, 5, 6;");
            I("REBASE z2 2011;");
            _AssertSeries(First(), "z2", EFreq.Q, 2010, 3, double.NaN, sharedDelta);
            _AssertSeries(First(), "z2", EFreq.Q, 2010, 4, 1d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z2", EFreq.Q, 2011, 1, 2d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z2", EFreq.Q, 2011, 2, 3d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z2", EFreq.Q, 2011, 3, 4d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z2", EFreq.Q, 2011, 4, 5d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z2", EFreq.Q, 2012, 1, 6d / (14d / 4d) * 100d, sharedDelta);            
            _AssertSeries(First(), "z2", EFreq.Q, 2012, 2, double.NaN, sharedDelta);

            //Same as above
            I("TIME 2010q4 2012q1;");
            I("SER z3 = 1, 2, 3, 4, 5, 6;");
            I("REBASE z3 2011q1 2011q4;");
            _AssertSeries(First(), "z3", EFreq.Q, 2010, 3, double.NaN, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2010, 4, 1d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2011, 1, 2d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2011, 2, 3d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2011, 3, 4d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2011, 4, 5d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2012, 1, 6d / (14d / 4d) * 100d, sharedDelta);
            _AssertSeries(First(), "z3", EFreq.Q, 2012, 2, double.NaN, sharedDelta);

        }

        [TestMethod]
        public void _Test_Random()
        {            
            I("RESET;");
            I("MATRIX #mean = [10; 11];");
            I("MATRIX #covar = [10, 3; 3, 2];");
            I("MATRIX #norm = rnorm(#mean, #covar);");
            I("VAL %norm = rnorm(1, 2);");
            //hard to test random numbers.......               
        }

        
        [TestMethod]
        public void _Test_Time()
        {
            //
            // Testing TIME            
            I("RESET;");
            I("TIME 2000 2016;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.A);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2000);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 1);
            Assert.AreEqual(Globals.globalPeriodEnd.freq, EFreq.A);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2016);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 1);
            I("TIME 2010;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.A);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2010);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 1);
            Assert.AreEqual(Globals.globalPeriodEnd.freq, EFreq.A);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2010);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 1);
            I("OPTION freq m;");
            I("TIME 2000 2016;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2000);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 1);
            Assert.AreEqual(Globals.globalPeriodEnd.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2016);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 12);
            I("TIME 2010;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2010);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 1);
            Assert.AreEqual(Globals.globalPeriodEnd.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2010);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 12);
            I("TIME 2010m5;");
            Assert.AreEqual(Globals.globalPeriodStart.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodStart.super, 2010);
            Assert.AreEqual(Globals.globalPeriodStart.sub, 5);
            Assert.AreEqual(Globals.globalPeriodEnd.freq, EFreq.M);
            Assert.AreEqual(Globals.globalPeriodEnd.super, 2010);
            Assert.AreEqual(Globals.globalPeriodEnd.sub, 5);
        }

        [TestMethod]
        public void Test__Timefilter()
        {
            //
            // Testing TIMEFILTER, implicit a little bit of PRT testing too
            // tested for annual and quarters
            //
            I("RESET;");
            I("TIME 2000 2016;");
            I("SERIES xx = 7777777;");
            I("TIMEFILTER 2002, 2003, 2005..2008, 2010..2015 by 2;");
            I("PRT xx;");
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("1999   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2000   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2001   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2002   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2003   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2004   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2005   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2006   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2007   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2008   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2009   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2010   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2011   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2012   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2013   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2014   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2015   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2016   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2017   7777777"));

            I("RESET;");
            I("option freq q;");
            I("time 2006q1 2009q4;");
            I("SERIES xx=7777777;");
            I("timefilter 2006q1,2006q3,2007q2,2007q4..2009q4 by 4;");
            I("prt xx;");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2006q1   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2006q2   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2006q3   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2006q4   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2007q1   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2007q2   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2007q3   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2007q4   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2008q1   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2008q2   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2008q3   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2008q4   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2009q1   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2009q2   7777777"));
            Assert.IsFalse(Globals.unitTestScreenOutput.ToString().Contains("2009q3   7777777"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2009q4   7777777"));

            I("RESET;");
            I("TIME 2000 2004;");
            I("SERIES<1999 1999> xx = 100;");
            I("SERIES<2000 2004> xx % 10;");
            I("TIMEFILTER 2000,2004;");
            I("PRT xx;");
            Table tab = Globals.unitTestTablePointer;
            int counter = 0;
            TestCell(ref counter, tab, 1, 2, CellType.Text, "xx");
            TestCell(ref counter, tab, 1, 3, CellType.Text, "%");
            TestCell(ref counter, tab, 2, 1, CellType.Text, "2000");
            TestCell(ref counter, tab, 2, 2, CellType.Number, 110d, sharedDelta);
            TestCell(ref counter, tab, 2, 3, CellType.Number, 10d, sharedDelta);
            TestCell(ref counter, tab, 3, 1, CellType.Text, "2004");
            TestCell(ref counter, tab, 3, 2, CellType.Number, 161.0510d, sharedDelta);
            TestCell(ref counter, tab, 3, 3, CellType.Number, 10d, sharedDelta);
            Assert.AreEqual(counter, tab.Count());

            I("RESET;");
            I("TIME 2000 2004;");
            I("SERIES<1999 1999> xx = 100;");
            I("SERIES<2000 2004> xx % 10;");
            I("TIMEFILTER 2000,2004;");
            I("PRT<filter=avg> xx;");
            tab = Globals.unitTestTablePointer;
            counter = 0;
            TestCell(ref counter, tab, 1, 2, CellType.Text, "xx");
            TestCell(ref counter, tab, 1, 3, CellType.Text, "%");
            TestCell(ref counter, tab, 2, 1, CellType.Text, "2000");
            TestCell(ref counter, tab, 2, 2, CellType.Number, 110d, sharedDelta);
            TestCell(ref counter, tab, 2, 3, CellType.Number, 10d, sharedDelta);
            TestCell(ref counter, tab, 3, 1, CellType.Text, "2001-2004");
            TestCell(ref counter, tab, 3, 2, CellType.Number, 140.39025d, sharedDelta);
            TestCell(ref counter, tab, 3, 3, CellType.Number, 10d, sharedDelta);
            Assert.AreEqual(counter, tab.Count());

            I("RESET;");
            I("TIME 2000 2004;");
            I("SERIES<1999 1999> xx = 100;");
            I("SERIES<2000 2003> xx % 10;");
            I("SERIES<2004 2004> xx % 20;");
            I("TIMEFILTER 2000,2004;");
            I("PRT<filter=avg> xx;");
            tab = Globals.unitTestTablePointer;
            counter = 0;
            TestCell(ref counter, tab, 1, 2, CellType.Text, "xx");
            TestCell(ref counter, tab, 1, 3, CellType.Text, "%");
            TestCell(ref counter, tab, 2, 1, CellType.Text, "2000");
            TestCell(ref counter, tab, 2, 2, CellType.Number, 110d, sharedDelta);
            TestCell(ref counter, tab, 2, 3, CellType.Number, 10d, sharedDelta);
            TestCell(ref counter, tab, 3, 1, CellType.Text, "2001-2004");
            TestCell(ref counter, tab, 3, 2, CellType.Number, 144.0505d, sharedDelta);
            TestCell(ref counter, tab, 3, 3, CellType.Number, 12.4190d, 0.0001d);
            Assert.AreEqual(counter, tab.Count());
        }

        private static void TestCell(ref int counter, Table tab, int row, int col, CellType type, string s)
        {
            Cell c = tab.Get(row, col);
            Assert.AreEqual(c.cellType, type);
            Assert.AreEqual(type, CellType.Text);
            Assert.AreEqual(c.CellText.TextData[0], s);
            counter++;
        }

        private static void TestCell(ref int counter, Table tab, int row, int col, CellType type, double d, double crit)
        {
            Cell c = tab.Get(row, col);
            Assert.AreEqual(c.cellType, type);
            Assert.AreEqual(type, CellType.Number);
            Assert.AreEqual(c.number, d, crit);
            counter++;
        }

        [TestMethod]
        public void _Test_ObjectOrientation()
        {
            // ---------------------------------------------------------------------------
            // object method with 0 arguments, called as object method, or as function
            // ---------------------------------------------------------------------------

            I("reset;");
            I("function val volume(map #m);"
            + "  %volume = 0;"
            + "  if (#m.%type == 'square');"
            + "    %volume = #m.%size * #m.%size;"
            + "  else;"
            + "  if (#m.%type == 'box');"
            + "    %volume = #m.%size * #m.%size * #m.%size;"
            + "  end; end;"
            + "  return %volume;"
            + "end;");                        
            I("#sq = (%type = 'square', %size = 2, %color = 'red');");
            I("#bx = (%type = 'box', %size = 3, %color = 'green');");
            I("tell'';");
            I("tell'Type ' + #sq.%type + ': size = ' + #sq.%size + ', volume = ' + #sq.volume() + ', color = ' + #sq.%color;");
            I("tell'Type ' + #bx.%type + ': size = ' + #bx.%size + ', volume = ' + #bx.volume() + ', color = ' + #bx.%color;");

            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("Type square: size = 2, volume = 4, color = red"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("Type box: size = 3, volume = 27, color = green"));

            // ---------------------------------------------------------------------------
            // object method with 1 argument, called as object method, or as function
            // ---------------------------------------------------------------------------

            I("reset;");
            I("function list plus(list #m, val %v);"
            + "  for val %i = 1 to #m.len();"
            + "    #m[%i] = #m[%i] + %v;"
            + "  end;"
            + "  return #m;"
            + "end;");

            I("function list mul(list #m, val %v);"
            + "  for val %i = 1 to #m.len();"
            + "    #m[%i] = #m[%i] * %v;"
            + "  end;"
            + "  return #m;"
            + "end;");

            I("#m1 = (1, 2, 3);");
            I("#m1 = plus(#m1, 100);");

            I("#m2 = (1, 2, 3);");
            I("#m2 = #m2.plus(200);");

            I("#m3 = (1, 2, 3);");
            I("#m3 = mul(plus(#m3, 200), 1000);");   //(1 + 200) * 1000 = 201000, not 1200 if the other order

            I("#m4 = (1, 2, 3);");
            I("#m4 = #m4.plus(200).mul(10000);");  //(1 + 200) * 10000 = 2010000, not 10200 if the other order

            I("print #m1;");
            I("print #m2;");
            I("print #m3;");
            I("print #m4;");

            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("101, 102, 103"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("201, 202, 203"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("201000, 202000, 203000"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("2010000, 2020000, 2030000"));


        }

        [TestMethod]
        public void _Test_PipeAndTell()
        {
            //
            // Testing of PIPE, PIPE<append>, PIPE<html>
            // Implicitly also testing TELL
            //
            string working = Globals.ttPath2 + @"\regres\temp";
            Program.DeleteFolder(working);
            Directory.CreateDirectory(working);
            I("OPTION folder working = '" + working + "';");
            // ------------------------
            // Testing txt piping
            // ------------------------
            I("RESET;");
            I("PIPE tempfile.txt;");
            I("TELL 'abcdefg';");
            I("PIPE con;");
            using (StreamReader sr = new StreamReader(working + "\\tempfile.txt"))
            {
                string s = sr.ReadToEnd();
                Assert.IsTrue(s == "abcdefg" + G.NL);
            }
            // ----------------------------------------------
            // Testing txt piping not appending as default
            // ----------------------------------------------
            I("PIPE tempfile.txt;");
            I("TELL 'hijklmn';");
            I("PIPE con;");
            using (StreamReader sr = new StreamReader(working + "\\tempfile.txt"))
            {
                string s = sr.ReadToEnd();
                Assert.IsTrue(s == "hijklmn" + G.NL);
            }
            // ------------------------
            // Testing txt pipe append
            // ------------------------
            I("PIPE <append> tempfile.txt;");
            I("TELL 'opqrstu';");
            I("PIPE con;");
            using (StreamReader sr = new StreamReader(working + "\\tempfile.txt"))
            {
                string s = sr.ReadToEnd();
                Assert.IsTrue(s == "hijklmn" + G.NL + "opqrstu" + G.NL);
            }
            // ------------------------
            // Testing html piping
            // ------------------------
            I("RESET;");
            I("PIPE <html> tempfile.html;");
            I("TELL '<p style=\"font-size:20px\">abcdefg</p>';");  //to test ""
            I("PIPE con;");
            using (StreamReader sr = new StreamReader(working + "\\tempfile.html"))
            {
                string s = sr.ReadToEnd();
                string[] s2 = s.Split(new string[] { "<body>", "</body>" }, StringSplitOptions.None);
                Assert.IsTrue(s2[1] == G.NL + "<p style=\"font-size:20px\">abcdefg</p>" + G.NL);
            }
            // ------------------------
            // Testing html pipe append
            // ------------------------
            I("PIPE <html append> tempfile.html;");
            I("TELL '<p style=\"font-size:40px\">hijklmn</p>';");  //to test ""
            I("PIPE con;");
            using (StreamReader sr = new StreamReader(working + "\\tempfile.html"))
            {
                string s = sr.ReadToEnd();
                string[] s2 = s.Split(new string[] { "<body>", "</body>" }, StringSplitOptions.None);
                Assert.IsTrue(s2[1] == G.NL + "<p style=\"font-size:20px\">abcdefg</p>" + G.NL + "<p style=\"font-size:40px\">hijklmn</p>" + G.NL);
            }
        }

        [TestMethod]
        public void _Test_Databanks()
        {
            // ----------------------------------------------------------
            //          - tsd and tsdx (latter with .tsd or .bin inside)
            //          - testing READ and WRITE of these files,
            //          - and also READ<merge> and WRITE x y z file=xx (subset)
            //          - and also WRITE<2001 2005> x y z file=xx (subset and part of period)
            //          - Also tests Trim() of databanks
            //          - Plus some testing that timeseries start and end periods are ok
            // ----------------------------------------------------------

            /* These are data in small.tsd:

            ------------------------------------------------

                             fY1           nine            one
              1999              M         9.9900         1.1100
              2000       123.4543         9.9990         2.2200
              2001              M         9.9999         3.3300
              2002      -234.5432
              2003         0.3454
              2004              M

            ------------------------------------------------

                              fY1             qq
              1999q1                        3.3333
              1999q2                        4.4444
              1999q3                        5.5556
              1999q4
              2000q1              M
              2000q2       123.4543
              2000q3              M
              2000q4      -234.5432
              2001q1         0.3454
              2001q2              M

            ------------------------------------------------

                                fY1             mm
              1999m1                         6.6667
              1999m2                         7.7778
              1999m3                         8.8889
              1999m4
              1999m5
              1999m6
              1999m7
              1999m8
              1999m9
              1999m10
              1999m11
              1999m12
              2000m1               M
              2000m2        123.4543
              2000m3               M
              2000m4       -234.5432
              2000m5          0.3454
              2000m6               M

            ------------------------------------------------

             */
                   

            for (int i = 0; i < 2; i++)
            {
                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks';");
                Globals.databanksAsProtobuffers = false;
                I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                if (i == 1) First().Trim();
                I("WRITE temp\\small_old;");
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>small2; CLONE;");
                if (i == 1) First().Trim();
                I("WRITE temp\\small2_old;");

                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
                Globals.databanksAsProtobuffers = true;
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                if (i == 1) First().Trim();
                I("WRITE temp\\small_new;");
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>small2; CLONE;");
                if (i == 1) First().Trim();
                I("WRITE temp\\small2_new;");

                Globals.databanksAsProtobuffers = true;  //setting it back to default

                //-------------------------------------------------------------
                //testing READ and READ<merge> for three types of databanks
                //-------------------------------------------------------------

                //TSD
                I("RESET;");
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                if (i == 1) First().Trim();
                I("IMPORT<tsd>small2; CLONE;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();

                //TSDX
                I("RESET;");
                if (i == 1) First().Trim();
                I("READ temp\\small_old;");
                if (i == 1) First().Trim();
                I("READ <merge> temp\\small2_old;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();

                //TSDX
                I("RESET;");
                if (i == 1) First().Trim();
                I("READ temp\\small_new;");
                if (i == 1) First().Trim();
                I("READ <merge> temp\\small2_new;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();

                //Testing WRITE and WRITE of subset for three databanks (we don't test write of tsdx 1.0 format from Gekko 1.4, not relevant anymore)

                //TSD
                I("RESET;");
                I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                if (i == 1) First().Trim();
                I("IMPORT<tsd>small2; CLONE;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("EXPORT <tsd> temp\\all;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("RESET;");
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>temp\\all; CLONE;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();

                //TSD
                //only subset written
                I("RESET;");
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                if (i == 1) First().Trim();
                I("IMPORT<tsd>small2; CLONE;");
                if (i == 1) First().Trim();
                I("EXPORT <tsd> fy1, one file=temp\\subset;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("RESET;");
                if (i == 1) First().Trim();
                I("CLEAR<first>; IMPORT<tsd>temp\\subset; CLONE;");
                Assert.AreEqual(First().storage.Count, 2);
                Assert.AreEqual(Program.databanks.GetRef().storage.Count, 2);

                //TSDX
                I("RESET;");
                if (i == 1) First().Trim();
                I("READ temp\\small_new;");
                if (i == 1) First().Trim();
                I("READ <merge> temp\\small2_new;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("WRITE temp\\all_new;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("RESET;");
                if (i == 1) First().Trim();
                I("READ temp\\all_new;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();

                //TSDX
                //tsdx 1.1: only subset written
                I("RESET;");
                if (i == 1) First().Trim();
                I("READ temp\\small_new;");
                if (i == 1) First().Trim();
                I("READ <merge> temp\\small2_new;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("WRITE fy1, one file=temp\\subset_new;");
                if (i == 1) First().Trim();
                DatabanksTestHelper();
                if (i == 1) First().Trim();
                I("RESET;");
                if (i == 1) First().Trim();
                I("READ temp\\subset_new;");
                if (i == 1) First().Trim();
                Assert.AreEqual(First().storage.Count, 2);
                Assert.AreEqual(Program.databanks.GetRef().storage.Count, 2);

                //TSD
                //tsd: only part of period written
                I("RESET;");
                I("TIME 2001 2003;");
                I("CREATE a, b, c;");
                I("SERIES a = 1;");
                I("SERIES b = 2;");
                I("SERIES c = 3;");
                if (i == 1) First().Trim();
                DatabanksTestHelper2();
                I("EXPORT<2002 2002 tsd> temp\\timetrunc;");
                if (i == 1) First().Trim();
                DatabanksTestHelper2();
                I("CLEAR<first>; IMPORT<tsd>temp\\timetrunc; CLONE;");
                if (i == 1) First().Trim();
                DatabanksTestHelper3();

                //TSDX
                //tsdx 1.1: only part of period written
                I("RESET;");
                I("TIME 2001 2003;");
                I("CREATE a, b, c;");
                I("SERIES a = 1;");
                I("SERIES b = 2;");
                I("SERIES c = 3;");
                if (i == 1) First().Trim();
                DatabanksTestHelper2();
                I("WRITE<2002 2002> temp\\timetrunc;");
                if (i == 1) First().Trim();
                DatabanksTestHelper2();
                I("READ temp\\timetrunc;");
                if (i == 1) First().Trim();
                DatabanksTestHelper3();

                //TSDX
                //tsdx 1.1: only part of vars and part of period written
                I("RESET;");
                I("TIME 2001 2003;");
                I("CREATE a, b, c;");
                I("SERIES a = 1;");
                I("SERIES b = 2;");
                I("SERIES c = 3;");
                if (i == 1) First().Trim();
                DatabanksTestHelper2();
                I("WRITE<2002 2002> a, c file=temp\\timetrunc2;");
                if (i == 1) First().Trim();
                DatabanksTestHelper2();
                I("READ temp\\timetrunc2;");
                if (i == 1) First().Trim();
                DatabanksTestHelper4();

                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");

                //CSV
                //testing CSV read/write -------------------------------------------

                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");

                List<string> freqs = new List<string> { "a", "q", "m" };

                foreach (string freq in freqs)
                {
                    I("OPTION freq " + freq + ";");
                    I("CLEAR<first>; IMPORT<tsd>small; CLONE;;");
                    I("EXPORT <csv> temp\\small_" + freq + ";");
                    I("CLEAR<first>; IMPORT<tsd>small2; CLONE;");
                    I("EXPORT <csv> temp\\small2_" + freq + ";");
                }

                I("RESET;");

                I("CLEAR<first>; IMPORT<csv>temp\\small_a; CLONE;");
                I("OPTION freq q;");
                I("IMPORT<csv>temp\\small_q; CLONE;");
                I("OPTION freq m;");
                I("IMPORT<csv>temp\\small_m; CLONE;");

                I("OPTION freq a;");
                I("IMPORT<csv>temp\\small2_a; CLONE;");
                I("OPTION freq q;");
                I("IMPORT<csv>temp\\small2_q; CLONE;");
                I("OPTION freq m;");
                I("IMPORT<csv>temp\\small2_m; CLONE;");

                I("OPTION freq a;");
                DatabanksTestHelper();
                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");

                //PRN
                //testing PRN read/write -------------------------------------------

                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");

                freqs = new List<string> { "a", "q", "m" };

                foreach (string freq in freqs)
                {
                    I("OPTION freq " + freq + ";");
                    I("CLEAR<first>; IMPORT<tsd>small; CLONE;");
                    I("EXPORT <prn> temp\\small_" + freq + ";");
                    I("CLEAR<first>; IMPORT<tsd>small2; CLONE;");
                    I("EXPORT <prn> temp\\small2_" + freq + ";");
                }

                I("RESET;");

                I("CLEAR<first>; IMPORT<prn>temp\\small_a; CLONE;");
                I("OPTION freq q;");
                I("IMPORT<prn>temp\\small_q; CLONE;");
                I("OPTION freq m;");
                I("IMPORT<prn>temp\\small_m; CLONE;");

                I("OPTION freq a;");
                I("IMPORT<prn>temp\\small2_a; CLONE;");
                I("OPTION freq q;");
                I("IMPORT<prn>temp\\small2_q; CLONE;");
                I("OPTION freq m;");
                I("IMPORT<prn>temp\\small2_m; CLONE;");

                I("OPTION freq a;");
                DatabanksTestHelper();
                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");

                if (Globals.UNITTESTFOLLOWUP_important)
                {
                    //Test that completely empty series are not written to bank

                    List<string> freqs2 = new List<string>() { "a", "q", "m" };
                    List<string> types = new List<string>() { "gbk", "tsd", "prn", "csv", "xlsx" };
                    foreach (string freq in freqs2)
                    {
                        foreach (string type in types)
                        {
                            Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                            Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
                            I("RESET;");
                            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
                            I("OPTION freq " + freq + ";");
                            if (freq == "a") I("TIME 2010 2012;");
                            else if (freq == "q") I("TIME 2010q1 2010q4;");
                            else if (freq == "m") I("TIME 2010m1 2010m4;");
                            I("SERIES xx1 = 1;");
                            I("SERIES xx2 = 2;");
                            if (freq == "a") I("TRUNCATE <2014 2016> xx2;");
                            else if (freq == "q") I("TRUNCATE <2011q1 2011q1> xx2;");
                            else if (freq == "m") I("TRUNCATE <2009m1 2009m12> xx2;");
                            I("WRITE <" + type + "> truncate;");
                            I("RESET;");
                            I("OPTION freq " + freq + ";");
                            I("READ <" + type + "> truncate;");
                            int number = Program.databanks.GetFirst().storage.Count;
                            Assert.AreEqual(1, number);
                            //just testing the first
                            if (freq == "a") _AssertSeries(Program.databanks.GetFirst(), "xx1", EFreq.A, 2010, 1, 1d, sharedDelta);
                            if (freq == "q") _AssertSeries(Program.databanks.GetFirst(), "xx1!q", EFreq.Q, 2010, 1, 1d, sharedDelta);
                            if (freq == "m") _AssertSeries(Program.databanks.GetFirst(), "xx1!m", EFreq.M, 2010, 1, 1d, sharedDelta);
                        }
                    }
                }

                if (true)
                {

                    // Testing READ and IMPORT with periods

                    Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                    Directory.CreateDirectory(Globals.ttPath2 + @"\regres\Databanks\temp");
                    I("RESET;");
                    I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");                    
                    I("OPTION freq a;");
                    I("SER <2010 2013> xx1 = 1;");
                    I("OPTION freq q;");
                    I("SER <2010q1 2013q2> xx2 = 2;");
                    I("OPTION freq m;");
                    I("SER <2010m1 2013m2> xx3 = 3;");
                    I("WRITE mixed;");
                    I("RESET; IMPORT<2011 2011>mixed;");
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2010, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2011, 1, 1d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2012, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", EFreq.Q, 2010, 4, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", null, EFreq.Q, 2011, 1, 2011, 4, 2d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", EFreq.Q, 2012, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", EFreq.M, 2010, 12, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", null, EFreq.M, 2011, 1, 2011, 12, 3d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", EFreq.M, 2012, 1, double.NaN, sharedDelta);
                    I("RESET; IMPORT<2011q2 2011q3>mixed;");
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2010, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2011, 1, 1d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2012, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", EFreq.Q, 2011, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", null, EFreq.Q, 2011, 2, 2011, 3, 2d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", EFreq.Q, 2011, 4, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", EFreq.M, 2011, 3, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", null, EFreq.M, 2011, 4, 2011, 9, 3d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", EFreq.M, 2011, 10, double.NaN, sharedDelta);
                    I("RESET; IMPORT<2011m2 2011m4>mixed;");
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2010, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2011, 1, 1d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx1!a", EFreq.A, 2012, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", EFreq.Q, 2010, 4, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", null, EFreq.Q, 2011, 1, 2011, 2, 2d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx2!q", EFreq.Q, 2011, 3, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", EFreq.M, 2011, 1, double.NaN, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", null, EFreq.M, 2011, 2, 2011, 4, 3d, sharedDelta);
                    _AssertSeries(Program.databanks.GetFirst(), "xx3!m", EFreq.M, 2011, 5, double.NaN, sharedDelta);
                    FAIL("IMPORT<2011u1 2011u1>mixed;");
                    FAIL("IMPORT<2011 2011q1>mixed;");
                    FAIL("IMPORT<2011q1 2011m12>mixed;");

                    // ----------------------------------------------------------------------------
                    //Testing a scrolling 'window' of READ/IMPORT time limit, on quarters
                    //pcim and tsp formats not tested
                    for (int jj = 0; jj < 2; jj++)
                    {
                        List<string> types = new List<string>() { "gbk", "tsd", "prn", "csv", "xlsx" };
                        foreach (string s in types)
                        {
                            I("RESET;");
                            I("OPTION freq q;");
                            I("SER<2000q1 2000q3> xx1 = (1,2,3);");
                            I("WRITE <" + s + "> temp;");
                            int count1 = 0;
                            GekkoTime t0 = new GekkoTime(EFreq.Q, 2000, 1);
                            foreach (GekkoTime t1 in new GekkoTimeIterator(new GekkoTime(EFreq.Q, 2000, 1), new GekkoTime(EFreq.Q, 2000, 3)))
                            {
                                int count2 = 0;
                                foreach (GekkoTime t2 in new GekkoTimeIterator(new GekkoTime(EFreq.Q, 2000, 1), new GekkoTime(EFreq.Q, 2000, 3)))
                                {
                                    if (count1 > count2) continue;
                                    I("SER<2000q1 2000q3> xx1 = (100,200,300);");
                                    if (jj == 0)
                                    {
                                        I("IMPORT<" + t1.ToString() + " " + t2.ToString() + " " + s + "> temp;");
                                    }
                                    else
                                    {
                                        I("READ<" + t1.ToString() + " " + t2.ToString() + " " + s + "> temp;");
                                    }
                                    for (int ii = 0; ii < count1; ii++)
                                    {
                                        //original                                
                                        GekkoTime ttemp = t0.Add(ii);
                                        if (jj == 0)
                                        {
                                            _AssertSeries(Program.databanks.GetFirst(), "xx1!q", EFreq.Q, ttemp.super, ttemp.sub, (ii + 1) * 100, sharedDelta);
                                        }
                                        else
                                        {
                                            _AssertSeries(Program.databanks.GetFirst(), "xx1!q", EFreq.Q, ttemp.super, ttemp.sub, double.NaN, sharedDelta);
                                        }
                                    }
                                    for (int ii = count1; ii <= count2; ii++)
                                    {
                                        GekkoTime ttemp = t0.Add(ii);
                                        _AssertSeries(Program.databanks.GetFirst(), "xx1!q", EFreq.Q, ttemp.super, ttemp.sub, (ii + 1) * 1, sharedDelta);
                                    }
                                    for (int ii = count2 + 1; ii < 3; ii++)
                                    {
                                        //original                                
                                        GekkoTime ttemp = t0.Add(ii);
                                        if (jj == 0)
                                        {
                                            _AssertSeries(Program.databanks.GetFirst(), "xx1!q", EFreq.Q, ttemp.super, ttemp.sub, (ii + 1) * 100, sharedDelta);
                                        }
                                        else
                                        {
                                            _AssertSeries(Program.databanks.GetFirst(), "xx1!q", EFreq.Q, ttemp.super, ttemp.sub, double.NaN, sharedDelta);
                                        }
                                    }
                                    count2++;
                                }
                                count1++;
                            }
                        }  //for each type
                    }// for IMPORT, READ
                }

                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");

            }
        }

        private static void DatabanksTestHelper4()
        {
            int nWork = First().storage.Count;
            Assert.AreEqual(nWork, 2);
            UData u;
            //just testing start/end for one of the timeseris (must be enough)
            Series ts = First().GetIVariable("a" + "!a") as Series;
            GekkoTime gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 2002);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2002);
            Assert.AreEqual(gt.sub, 1);
            u = Data("a", 2001, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("a", 2002, "a"); Assert.AreEqual(u.w, 1d);
            u = Data("a", 2003, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("c", 2001, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("c", 2002, "a"); Assert.AreEqual(u.w, 3d);
            u = Data("c", 2003, "a"); Assert.AreEqual(u.w, double.NaN);
        }

        private static void DatabanksTestHelper3()
        {
            int nWork = First().storage.Count;
            Assert.AreEqual(nWork, 3);
            UData u;
            //just testing start/end for one of the timeseris (must be enough)
            Series ts = First().GetIVariable("a" + "!a") as Series;
            GekkoTime gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 2002);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2002);
            Assert.AreEqual(gt.sub, 1);
            u = Data("a", 2001, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("a", 2002, "a"); Assert.AreEqual(u.w, 1d);
            u = Data("a", 2003, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("b", 2001, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("b", 2002, "a"); Assert.AreEqual(u.w, 2d);
            u = Data("b", 2003, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("c", 2001, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("c", 2002, "a"); Assert.AreEqual(u.w, 3d);
            u = Data("c", 2003, "a"); Assert.AreEqual(u.w, double.NaN);
        }

        private static void DatabanksTestHelper2()
        {
            int nWork = First().storage.Count;
            Assert.AreEqual(nWork, 3);
            UData u;
            //just testing start/end for one of the timeseris (must be enough)
            Series ts = First().GetIVariable("a" + "!a") as Series;
            GekkoTime gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 2001);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2003);
            Assert.AreEqual(gt.sub, 1);
            u = Data("a", 2000, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("a", 2001, "a"); Assert.AreEqual(u.w, 1d);
            u = Data("a", 2002, "a"); Assert.AreEqual(u.w, 1d);
            u = Data("a", 2003, "a"); Assert.AreEqual(u.w, 1d);
            u = Data("a", 2004, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("b", 2000, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("b", 2001, "a"); Assert.AreEqual(u.w, 2d);
            u = Data("b", 2002, "a"); Assert.AreEqual(u.w, 2d);
            u = Data("b", 2003, "a"); Assert.AreEqual(u.w, 2d);
            u = Data("b", 2004, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("c", 2000, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("c", 2001, "a"); Assert.AreEqual(u.w, 3d);
            u = Data("c", 2002, "a"); Assert.AreEqual(u.w, 3d);
            u = Data("c", 2003, "a"); Assert.AreEqual(u.w, 3d);
            u = Data("c", 2004, "a"); Assert.AreEqual(u.w, double.NaN);
        }

        private static void DatabanksTestHelper()
        {
            int nWork = First().storage.Count;
            Assert.AreEqual(nWork, 7);

            UData u;
            u = Data("one", 1999, "a"); Assert.AreEqual(u.w, 1.11d);
            u = Data("one", 2000, "a"); Assert.AreEqual(u.w, 2.22d);
            u = Data("one", 2001, "a"); Assert.AreEqual(u.w, 3.33d);
            Series ts = First().GetIVariable("one!a") as Series;
            GekkoTime gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2001);
            Assert.AreEqual(gt.sub, 1);

            u = Data("nine", 1999, "a"); Assert.AreEqual(u.w, 9.99d);
            u = Data("nine", 2000, "a"); Assert.AreEqual(u.w, 9.999d);
            u = Data("nine", 2001, "a"); Assert.AreEqual(u.w, 9.9999d);
            ts = First().GetIVariable("nine!a") as Series;
            gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2001);
            Assert.AreEqual(gt.sub, 1);

            u = Data("qq", 1999, 1, "q"); Assert.AreEqual(u.w, 3.33333333d);
            u = Data("qq", 1999, 2, "q"); Assert.AreEqual(u.w, 4.44444444d);
            u = Data("qq", 1999, 3, "q"); Assert.AreEqual(u.w, 5.55555555d);
            ts = First().GetIVariable("qq!q") as Series;
            gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 3);

            u = Data("mm", 1999, 1, "m"); Assert.AreEqual(u.w, 6.66666666d);
            u = Data("mm", 1999, 2, "m"); Assert.AreEqual(u.w, 7.77777777d);
            u = Data("mm", 1999, 3, "m"); Assert.AreEqual(u.w, 8.88888888d);
            ts = First().GetIVariable("mm!m") as Series;
            gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 3);

            u = Data("fy1", 1997, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 1998, "a"); Assert.AreEqual(u.w, 6.54320000E+00d);
            u = Data("fy1", 1999, "a"); Assert.AreEqual(u.w, 5.43210000E+00);
            u = Data("fy1", 2000, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 2001, "a"); Assert.AreEqual(u.w, 3.00000000E+00);
            u = Data("fy1", 2002, "a"); Assert.AreEqual(u.w, 4.00000000E+00);
            u = Data("fy1", 2003, "a"); Assert.AreEqual(u.w, 3.45432123E-01d);
            u = Data("fy1", 2004, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 2005, "a"); Assert.AreEqual(u.w, double.NaN);
            ts = First().GetIVariable("fy1!a") as Series;
            gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1998);
            Assert.AreEqual(gt.sub, 1);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2004);
            Assert.AreEqual(gt.sub, 1);

            u = Data("fy1", 1999, 3, "q"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 1999, 4, "q"); Assert.AreEqual(u.w, 6.54320000E+00d);
            u = Data("fy1", 2000, 1, "q"); Assert.AreEqual(u.w, 5.43210000E+00);
            u = Data("fy1", 2000, 2, "q"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 2000, 3, "q"); Assert.AreEqual(u.w, 3.00000000E+00);
            u = Data("fy1", 2000, 4, "q"); Assert.AreEqual(u.w, 4.00000000E+00);
            u = Data("fy1", 2001, 1, "q"); Assert.AreEqual(u.w, 3.45432123E-01d);
            u = Data("fy1", 2001, 2, "q"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 2001, 3, "q"); Assert.AreEqual(u.w, double.NaN);
            ts = First().GetIVariable("fy1!q") as Series;
            gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 4);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2001);
            Assert.AreEqual(gt.sub, 2);

            u = Data("fy1", 1999, 11, "m"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 1999, 12, "m"); Assert.AreEqual(u.w, 6.54320000E+00d);
            u = Data("fy1", 2000, 1, "m"); Assert.AreEqual(u.w, 5.43210000E+00);
            u = Data("fy1", 2000, 2, "m"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 2000, 3, "m"); Assert.AreEqual(u.w, 3.00000000E+00);
            u = Data("fy1", 2000, 4, "m"); Assert.AreEqual(u.w, 4.00000000E+00);
            u = Data("fy1", 2000, 5, "m"); Assert.AreEqual(u.w, 3.45432123E-01d);
            u = Data("fy1", 2000, 6, "m"); Assert.AreEqual(u.w, double.NaN);
            u = Data("fy1", 2000, 7, "m"); Assert.AreEqual(u.w, double.NaN);
            ts = First().GetIVariable("fy1!m") as Series;
            gt = ts.GetPeriodFirst();
            Assert.AreEqual(gt.super, 1999);
            Assert.AreEqual(gt.sub, 12);
            gt = ts.GetPeriodLast();
            Assert.AreEqual(gt.super, 2000);
            Assert.AreEqual(gt.sub, 6);
            return;
        }

        [TestMethod]
        public void _Test_Matrix()
        {

            //I("time 2001 2003; xx = [1; 2; 3];");
            //_AssertSeries(First(), "xx", 2000, double.NaN, sharedDelta);
            //_AssertSeries(First(), "xx", 2001, 1d, sharedDelta);
            //_AssertSeries(First(), "xx", 2002, 2d, sharedDelta);
            //_AssertSeries(First(), "xx", 2003, 3d, sharedDelta);
            //_AssertSeries(First(), "xx", 2004, double.NaN, sharedDelta);

            I("RESET; TIME 2001 2005;");
            I("xx1 = (1, 2, 3, 4, 5);");
            I("#m = xx1;");  //autoconverted into matrix    WHY???
            _AssertMatrix(First(), "#m", 1, 1, 1, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 2, 2, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 3, 3, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 4, 4, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 5, 5, sharedDelta);

            I("#m = [1, 2; 3, 4];");
            _AssertMatrix(First(), "#m", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 2, 4d, sharedDelta);

            I("#m[1..2] = [100; 200];");
            _AssertMatrix(First(), "#m", 1, 1, 100d, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 1, 200d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 2, 4d, sharedDelta);

            I("#m = [1, 2; 3, 4];");
            I("#m[1..2, 1] = #m[1..2, 2];");
            _AssertMatrix(First(), "#m", 1, 1, 2d, sharedDelta);
            _AssertMatrix(First(), "#m", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 2, 4d, sharedDelta);

            I("#m = [1; 2; 3];");
            I("#m[2..3] = [20; 30];");
            _AssertMatrix(First(), "#m", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#m", 2, 1, 20d, sharedDelta);
            _AssertMatrix(First(), "#m", 3, 1, 30d, sharedDelta);


            I("RESET;");
            I("MATRIX #a = m(2,3);");
            I("MATRIX #a[1, 1] = 1;");
            I("MATRIX #a[1, 2] = 2;");
            I("MATRIX #a[1, 3] = 3;");
            I("MATRIX #a[2, 1] = 4;");
            I("MATRIX #a[2, 2] = 5;");
            I("MATRIX #a[2, 3] = 6;");
            I("MATRIX #b = zeros(3, 4);");
            I("#b[1, 1] = 7;");
            I("#b[1, 2] = 8;");
            I("#b[1, 3] = 9;");
            I("#b[1, 4] = 10;");
            I("#b[2, 1] = 11;");
            I("#b[2, 2] = 12;");
            I("#b[2, 3] = 13;");
            I("#b[2, 4] = 14;");
            I("#b[3, 1] = 15;");
            I("#b[3, 2] = 16;");
            I("#b[3, 3] = 17;");
            I("#b[3, 4] = 18;");
            I("#onebyone = zeros(1, 1);");
            I("#onebyone[1, 1] = 2;");

            // ----------- multiply and divide
            I("#c = #a * #b;");            
            _AssertMatrix(First(), "#c", 1, 1, 74, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 80, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 86, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 4, 92, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 173, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 188, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 203, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 4, 218, sharedDelta);
            I("VAL %v = 2;");
            I("#c2 = #c * %v;");
            _AssertMatrix(First(), "#c2", 1, 1, 2 * 74, sharedDelta);
            _AssertMatrix(First(), "#c2", 1, 2, 2 * 80, sharedDelta);
            _AssertMatrix(First(), "#c2", 1, 3, 2 * 86, sharedDelta);
            _AssertMatrix(First(), "#c2", 1, 4, 2 * 92, sharedDelta);
            _AssertMatrix(First(), "#c2", 2, 1, 2 * 173, sharedDelta);
            _AssertMatrix(First(), "#c2", 2, 2, 2 * 188, sharedDelta);
            _AssertMatrix(First(), "#c2", 2, 3, 2 * 203, sharedDelta);
            _AssertMatrix(First(), "#c2", 2, 4, 2 * 218, sharedDelta);
            I("#c3 = %v * #c;");
            _AssertMatrix(First(), "#c3", 1, 1, 2 * 74, sharedDelta);
            _AssertMatrix(First(), "#c3", 1, 2, 2 * 80, sharedDelta);
            _AssertMatrix(First(), "#c3", 1, 3, 2 * 86, sharedDelta);
            _AssertMatrix(First(), "#c3", 1, 4, 2 * 92, sharedDelta);
            _AssertMatrix(First(), "#c3", 2, 1, 2 * 173, sharedDelta);
            _AssertMatrix(First(), "#c3", 2, 2, 2 * 188, sharedDelta);
            _AssertMatrix(First(), "#c3", 2, 3, 2 * 203, sharedDelta);
            _AssertMatrix(First(), "#c3", 2, 4, 2 * 218, sharedDelta);
            I("#c4 = #c / %v;");
            _AssertMatrix(First(), "#c4", 1, 1, 0.5d * 74, sharedDelta);
            _AssertMatrix(First(), "#c4", 1, 2, 0.5d * 80, sharedDelta);
            _AssertMatrix(First(), "#c4", 1, 3, 0.5d * 86, sharedDelta);
            _AssertMatrix(First(), "#c4", 1, 4, 0.5d * 92, sharedDelta);
            _AssertMatrix(First(), "#c4", 2, 1, 0.5d * 173, sharedDelta);
            _AssertMatrix(First(), "#c4", 2, 2, 0.5d * 188, sharedDelta);
            _AssertMatrix(First(), "#c4", 2, 3, 0.5d * 203, sharedDelta);
            _AssertMatrix(First(), "#c4", 2, 4, 0.5d * 218, sharedDelta);
            FAIL("#c4 = %v / #c;");
            I("#c5 = #c * #onebyone;");
            _AssertMatrix(First(), "#c5", 1, 1, 2 * 74, sharedDelta);
            _AssertMatrix(First(), "#c5", 1, 2, 2 * 80, sharedDelta);
            _AssertMatrix(First(), "#c5", 1, 3, 2 * 86, sharedDelta);
            _AssertMatrix(First(), "#c5", 1, 4, 2 * 92, sharedDelta);
            _AssertMatrix(First(), "#c5", 2, 1, 2 * 173, sharedDelta);
            _AssertMatrix(First(), "#c5", 2, 2, 2 * 188, sharedDelta);
            _AssertMatrix(First(), "#c5", 2, 3, 2 * 203, sharedDelta);
            _AssertMatrix(First(), "#c5", 2, 4, 2 * 218, sharedDelta);
            I("#c6 = #onebyone * #c;");
            _AssertMatrix(First(), "#c6", 1, 1, 2 * 74, sharedDelta);
            _AssertMatrix(First(), "#c6", 1, 2, 2 * 80, sharedDelta);
            _AssertMatrix(First(), "#c6", 1, 3, 2 * 86, sharedDelta);
            _AssertMatrix(First(), "#c6", 1, 4, 2 * 92, sharedDelta);
            _AssertMatrix(First(), "#c6", 2, 1, 2 * 173, sharedDelta);
            _AssertMatrix(First(), "#c6", 2, 2, 2 * 188, sharedDelta);
            _AssertMatrix(First(), "#c6", 2, 3, 2 * 203, sharedDelta);
            _AssertMatrix(First(), "#c6", 2, 4, 2 * 218, sharedDelta);
            I("#c7 = #c / #onebyone;");
            _AssertMatrix(First(), "#c7", 1, 1, 0.5d * 74, sharedDelta);
            _AssertMatrix(First(), "#c7", 1, 2, 0.5d * 80, sharedDelta);
            _AssertMatrix(First(), "#c7", 1, 3, 0.5d * 86, sharedDelta);
            _AssertMatrix(First(), "#c7", 1, 4, 0.5d * 92, sharedDelta);
            _AssertMatrix(First(), "#c7", 2, 1, 0.5d * 173, sharedDelta);
            _AssertMatrix(First(), "#c7", 2, 2, 0.5d * 188, sharedDelta);
            _AssertMatrix(First(), "#c7", 2, 3, 0.5d * 203, sharedDelta);
            _AssertMatrix(First(), "#c7", 2, 4, 0.5d * 218, sharedDelta);

            // --------- indexing
            I("#r1 = zeros(1, 3);");
            I("#r1[1,2] = 77;");
            I("#c1 = zeros(3, 1);");
            I("#c1[2,1] = 77;");
            FAIL("%x = #r1[2];");
            I("%x = #c1[2];");
            _AssertScalarVal(First(), "%x", 77d);

            // --------------- add and subtract
            I("RESET;");
            I("#a = zeros(2, 3);");
            I("#a[1, 1] = 1;");
            I("#a[1, 2] = 2;");
            I("#a[1, 3] = 3;");
            I("#a[2, 1] = 4;");
            I("#a[2, 2] = 5;");
            I("#a[2, 3] = 6;");
            I("#b = zeros(2, 3);");
            I("#b[1, 1] = 11;");
            I("#b[1, 2] = 12;");
            I("#b[1, 3] = 13;");
            I("#b[2, 1] = 14;");
            I("#b[2, 2] = 15;");
            I("#b[2, 3] = 16;");
            I("#c = #a + #b;");
            _AssertMatrix(First(), "#c", 1, 1, 12d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 14d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 16d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 18d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 20d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 22d, sharedDelta);
            I("%v = 2;");
            FAIL("#c2 = %v + #a;");
            FAIL("#c2 = #a + %v;");
            FAIL("#c2 = %v - #a;");
            FAIL("#c2 = #a - %v;");

            // ----------- transpose
            I("RESET;");
            I("#a = zeros(2, 3);");
            I("#a[1, 1] = 1;");
            I("#a[1, 2] = 2;");
            I("#a[1, 3] = 3;");
            I("#a[2, 1] = 4;");
            I("#a[2, 2] = 5;");
            I("#a[2, 3] = 6;");
            I("#b = t(#a);");

            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 4d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 2d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 2, 6d, sharedDelta);

            // #construction/concatenation
            // -----------
            I("RESET;");
            I("#a = [1,2;3,4];");
            I("%v11 = #a[1,1];");
            I("%v12 = #a[1,2];");
            I("%v21 = #a[2,1];");
            I("%v22 = #a[2,2];");
            
            _AssertScalarVal(First(), "%v11", 1d);
            _AssertScalarVal(First(), "%v12", 2d);
            _AssertScalarVal(First(), "%v21", 3d);
            _AssertScalarVal(First(), "%v22", 4d);
            I("#b = [5,6,7;8,9,10];");
            I("#c = [11,12;13,14;15,16];");
            I("#d = [17,18,19;20,21,22;23,24,25];");
            I("#x = [#a,#b;#c,#d];");
            _AssertMatrix(First(), "#x", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#x", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#x", 1, 3, 5d, sharedDelta);
            _AssertMatrix(First(), "#x", 1, 4, 6d, sharedDelta);
            _AssertMatrix(First(), "#x", 1, 5, 7d, sharedDelta);

            _AssertMatrix(First(), "#x", 2, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#x", 2, 2, 4d, sharedDelta);
            _AssertMatrix(First(), "#x", 2, 3, 8d, sharedDelta);
            _AssertMatrix(First(), "#x", 2, 4, 9d, sharedDelta);
            _AssertMatrix(First(), "#x", 2, 5, 10d, sharedDelta);

            _AssertMatrix(First(), "#x", 3, 1, 11d, sharedDelta);
            _AssertMatrix(First(), "#x", 3, 2, 12d, sharedDelta);
            _AssertMatrix(First(), "#x", 3, 3, 17d, sharedDelta);
            _AssertMatrix(First(), "#x", 3, 4, 18d, sharedDelta);
            _AssertMatrix(First(), "#x", 3, 5, 19d, sharedDelta);

            _AssertMatrix(First(), "#x", 4, 1, 13d, sharedDelta);
            _AssertMatrix(First(), "#x", 4, 2, 14d, sharedDelta);
            _AssertMatrix(First(), "#x", 4, 3, 20d, sharedDelta);
            _AssertMatrix(First(), "#x", 4, 4, 21d, sharedDelta);
            _AssertMatrix(First(), "#x", 4, 5, 22d, sharedDelta);

            _AssertMatrix(First(), "#x", 5, 1, 15d, sharedDelta);
            _AssertMatrix(First(), "#x", 5, 2, 16d, sharedDelta);
            _AssertMatrix(First(), "#x", 5, 3, 23d, sharedDelta);
            _AssertMatrix(First(), "#x", 5, 4, 24d, sharedDelta);
            _AssertMatrix(First(), "#x", 5, 5, 25d, sharedDelta);

            //trying to fail some of them
            FAIL("#x = [#b,#a;#c,#d];");
            FAIL("#x = [#a,#c;#b,#d];");
            FAIL("#x = [#a,#b;#d,#c];");
            FAIL("#x = [#c,#b;#a,#d];");
            FAIL("#x = [#a,#d;#c,#b];");

            //recursive
            I("#x = [[1,2;3,4],#b;#c,#d];");

            I("#x = [1,2 ;3,4];");
            I("#x = [1,2; 3,4];");
            I("#x = [1,2 ; 3,4];");
            FAIL("#x = [1,2| |3,4];");
            FAIL("#x = [1,2 | | 3,4];");

            I("#x = [1;3];");
            _AssertMatrix(First(), "#x", "rows", 2);
            _AssertMatrix(First(), "#x", "cols", 1);
            _AssertMatrix(First(), "#x", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#x", 2, 1, 3d, sharedDelta);

            I("#x = [1,3];");
            _AssertMatrix(First(), "#x", "rows", 1);
            _AssertMatrix(First(), "#x", "cols", 2);
            _AssertMatrix(First(), "#x", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#x", 1, 2, 3d, sharedDelta);

            I("#x = [1];");
            _AssertMatrix(First(), "#x", "rows", 1);
            _AssertMatrix(First(), "#x", "cols", 1);
            _AssertMatrix(First(), "#x", 1, 1, 1d, sharedDelta);

            // -------- subsections ---------

            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#b = #a[1..2, 2..3];");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 2d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 6d, sharedDelta);

            I("#b = #a[..2, 2..];");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 2d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 6d, sharedDelta);

            I("#b = #a[.., ..];");
            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 3);

            I("#b = #a[2, 2..3];");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 6d, sharedDelta);

            I("#b = #a[2..3, 2];");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 8d, sharedDelta);

            I("#b = #a[3..3, 3..3];");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 9d, sharedDelta);

            I("#a = [1 ; 3 ; 7];");
            I("#b = #a[2..3];");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 7d, sharedDelta);

            // -------- pack and unpack ---------

            I("RESET;");
            I("TIME 2010 2020;");
            I("SERIES <2000 2002> xx1 = (5, 6, 7);");
            I("SERIES <2000 2002> xx2 = (15, 16, 17);");
            I("#b = pack(2000, 2002, xx1, xx2);");
            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 6d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 15d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 16d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 2, 17d, sharedDelta);
            I("TIME 2000 2002;");
            I("#c = pack(xx1, xx2);");
            _AssertMatrix(First(), "#c", "rows", 3);
            _AssertMatrix(First(), "#c", "cols", 2);
            _AssertMatrix(First(), "#c", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 6d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 15d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 16d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 2, 17d, sharedDelta);
            I("yy1 = unpack(#b[.., 1]);");
            _AssertSeries(First(), "yy1", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "yy1", 2000, 5d, sharedDelta);
            _AssertSeries(First(), "yy1", 2001, 6d, sharedDelta);
            _AssertSeries(First(), "yy1", 2002, 7d, sharedDelta);
            _AssertSeries(First(), "yy1", 2003, double.NaN, sharedDelta);
            I("yy2 = unpack(2000, 2002, #b[.., 2]);");
            _AssertSeries(First(), "yy2", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "yy2", 2000, 15d, sharedDelta);
            _AssertSeries(First(), "yy2", 2001, 16d, sharedDelta);
            _AssertSeries(First(), "yy2", 2002, 17d, sharedDelta);
            _AssertSeries(First(), "yy2", 2003, double.NaN, sharedDelta);

            I("RESET;");
            I("OPTION freq m;;");
            I("TIME 2010 2020;");
            I("SERIES <2000m1 2000m3> xx1 = (5, 6, 7);");
            I("SERIES <2000m1 2000m3> xx2 = (15, 16, 17);");
            I("#b = pack(2000m1, 2000m3, xx1, xx2);");
            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 6d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 15d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 16d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 2, 17d, sharedDelta);
            I("TIME 2000m1 2000m3;");
            I("#c = pack(xx1, xx2);");
            _AssertMatrix(First(), "#c", "rows", 3);
            _AssertMatrix(First(), "#c", "cols", 2);
            _AssertMatrix(First(), "#c", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 6d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 15d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 16d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 2, 17d, sharedDelta);
            I("yy1 = unpack(#b[.., 1]);");
            _AssertSeries(First(), "yy1", EFreq.M, 1999, 12, double.NaN, sharedDelta);
            _AssertSeries(First(), "yy1", EFreq.M, 2000, 1, 5d, sharedDelta);
            _AssertSeries(First(), "yy1", EFreq.M, 2000, 2, 6d, sharedDelta);
            _AssertSeries(First(), "yy1", EFreq.M, 2000, 3, 7d, sharedDelta);
            _AssertSeries(First(), "yy1", EFreq.M, 2000, 4, double.NaN, sharedDelta);
            I("yy2 = unpack(2000m1, 2000m3, #b[.., 2]);");
            _AssertSeries(First(), "yy2", EFreq.M, 1999, 12, double.NaN, sharedDelta);
            _AssertSeries(First(), "yy2", EFreq.M, 2000, 1, 15d, sharedDelta);
            _AssertSeries(First(), "yy2", EFreq.M, 2000, 2, 16d, sharedDelta);
            _AssertSeries(First(), "yy2", EFreq.M, 2000, 3, 17d, sharedDelta);
            _AssertSeries(First(), "yy2", EFreq.M, 2000, 4, double.NaN, sharedDelta);

            // -------------- divide(), multiply()
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6];");
            I("#b = [2, 3, 4 ; 5, 6, 7];");
            I("#b1 = [2, 3 ; 5, 6 ; 1, 1];");
            I("#b2 = [2, 3, 4 ; 5, 6, 7 ; 1, 1, 1];");
            I("#c = multiply(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d * 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d * 4d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d * 5d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d * 6d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d * 7d, sharedDelta);
            I("#c = divide(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d / 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d / 4d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d / 5d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d / 6d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d / 7d, sharedDelta);
            FAIL("#c = multiply(#a, #b1);");
            FAIL("#c = multiply(#a, #b2);");
            //Special 1
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6];");
            I("#b = [2, 3, 4];");            
            I("#c = multiply(#a, #b);"); 
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d * 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d * 4d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d * 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d * 4d, sharedDelta);
            FAIL("#c = multiply(#b, #a);");  //not the other way around
            I("#c = divide(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d / 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d / 4d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d / 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d / 4d, sharedDelta);
            FAIL("#c = divide(#b, #a);");  //not the other way around
            //Special 2
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6];");
            I("#b = [2 ; 3];");
            I("#c = multiply(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d * 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d * 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d * 3d, sharedDelta);
            FAIL("#c = multiply(#b, #a);");  //not the other way around            
            I("#c = divide(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d / 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d / 3d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d / 3d, sharedDelta);
            FAIL("#c = divide(#b, #a);");  //not the other way around
            //Special 3
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6];");
            I("#b = [2];");
            I("#c = multiply(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d * 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d * 2d, sharedDelta);
            FAIL("#c = multiply(#b, #a);");  //not the other way around            
            I("#c = divide(#a, #b);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 2d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 3d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 4d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 5d / 2d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 6d / 2d, sharedDelta);
            FAIL("#c = divide(#b, #a);");  //not the other way around

            // -------------- zeros(), ones()

            I("RESET;");
            I("#c = ones(2, 3);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 1d, sharedDelta);
            I("#c = zeros(2, 3);");
            _AssertMatrix(First(), "#c", "rows", 2);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 0d, sharedDelta);

            // -------------i()
            I("#c = i(3);");
            _AssertMatrix(First(), "#c", "rows", 3);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 3, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 2, 0d, sharedDelta);


            // -------------- inv()
            I("RESET;");
            I("#a = [1, 2, 3 ; -4, 5, -6 ; 7, 8, 9];");
            I("#b = inv(#a);");
            I("#c = #b*#a;");
            _AssertMatrix(First(), "#c", "rows", 3);
            _AssertMatrix(First(), "#c", "cols", 3);
            _AssertMatrix(First(), "#c", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 3, 1d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 1, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 2, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#c", 3, 2, 0d, sharedDelta);
            FAIL("#d = inv([1, 2, 3 ; -4, 5, -6]);");
            FAIL("#d = inv([1, 2 ; 3, 4 ; 5, 6]);");
            FAIL("#d = inv([1, 2 ; 10, 20);");

            I("RESET;");
            I("#a = [1, 2 ; 3, 4];");
            I("%v = det(#a);");
            _AssertScalarVal(First(), "%v", -2d);

            // ------------ diagonal, diag()
            I("RESET;");
            I("#a = [1, 2 ; 3, 4];");
            I("#b = diag(#a);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 4d, sharedDelta);
            I("RESET;");
            I("#a = [1 ; 2];");
            I("#b = diag(#a);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 2d, sharedDelta);
            FAIL("#b = diag([1, 2]);");
            FAIL("#b = diag([1, 2 ; 3, 4 ; 5, 6]);");
            I("#b = diag([123]);");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 123d, sharedDelta);

            // ------------ sumr(), sumc(), minr(), minc(), maxr(), maxc()
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6];");
            I("#b = sumr(#a);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 6d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 15d, sharedDelta);
            I("#b = avgr(#a);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 6d / 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 15d / 3d, sharedDelta);
            I("#b = minr(#a);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 4d, sharedDelta);
            I("#b = maxr(#a);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 1);
            _AssertMatrix(First(), "#b", 1, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 6d, sharedDelta);
            I("#b = sumc(#a);");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 7d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 9d, sharedDelta);
            I("#b = avgc(#a);");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 5d / 2d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 7d / 2d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 9d / 2d, sharedDelta);
            I("#b = minc(#a);");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 3d, sharedDelta);
            I("#b = maxc(#a);");
            _AssertMatrix(First(), "#b", "rows", 1);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 6d, sharedDelta);

            // ------------ trace()
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("%v = trace(#a);");
            _AssertScalarVal(First(), "%v", 15d);

            // ------------ round()
            I("RESET;");
            I("#a = [1.235, -2.55 ; 1.55, 7.77];");
            I("#b = round(#a, 1);");
            _AssertMatrix(First(), "#b", "rows", 2);
            _AssertMatrix(First(), "#b", "cols", 2);
            _AssertMatrix(First(), "#b", 1, 1, 1.2d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, -2.6d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 1.6d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 7.8d, sharedDelta);

            // ---------------------------------------------
            // --------- left-side indexer -----------------
            // ---------------------------------------------
                        
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1..2, 1..2] = [10, 20 ; 30, 40];");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 40d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[..2, ..2] = [10, 20 ; 30, 40];");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 40d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[2..3, 1..2] = [10, 20 ; 30, 40];");
            _AssertMatrix(First(), "#a", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 40d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1..2, 2..3] = [10, 20 ; 30, 40];");
            _AssertMatrix(First(), "#a", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 40d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[2..3, 2..3] = [10, 20 ; 30, 40];");
            _AssertMatrix(First(), "#a", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 40d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[2.., 2..] = [10, 20 ; 30, 40];");
            _AssertMatrix(First(), "#a", 1, 1, 1d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 40d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1..2, 1..2] = 100;");
            _AssertMatrix(First(), "#a", 1, 1, 100d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 100d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 100d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 100d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1..2, 1] = [10 ; 30];");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 30d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 5d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1, 1..2] = [10, 20];");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 20d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 5d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1, 1] = [10];");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 5d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[1, 1] = 10;");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 2d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 3d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 4d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 5d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 6d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 7d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 8d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 9d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            I("#a[.., ..] = 10;");
            _AssertMatrix(First(), "#a", "rows", 3);
            _AssertMatrix(First(), "#a", "cols", 3);
            _AssertMatrix(First(), "#a", 1, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 2, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 1, 3, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 2, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 2, 3, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 1, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 2, 10d, sharedDelta);
            _AssertMatrix(First(), "#a", 3, 3, 10d, sharedDelta);
            I("RESET;");
            I("#a = [1, 2, 3 ; 4, 5, 6 ; 7, 8, 9];");
            FAIL("#a[1..2, 2..2] = [10, 20 ; 30, 40];");
            FAIL("#a[2..2, 1..2] = [10, 20 ; 30, 40];");
            FAIL("#a[2..1, 1..2] = [10, 20 ; 30, 40];");
            FAIL("#a[1..2, 2..1] = [10, 20 ; 30, 40];");
            FAIL("#a[0..1, 1..2] = [10, 20 ; 30, 40];");
            FAIL("#a[1..2, 0..1] = [10, 20 ; 30, 40];");
            FAIL("#a[3..4, 1..2] = [10, 20 ; 30, 40];");
            FAIL("#a[1..2, 3..4] = [10, 20 ; 30, 40];");
            FAIL("#a[1, 1..2] = [10, 20 ; 30, 40];");
            FAIL("#a[1..2, 1] = [10, 20 ; 30, 40];");
            FAIL("#a[1, 1] = [10, 20 ; 30, 40];");

            I("RESET;");
            I("#a = [25, 15, -5 ; 15, 18, 0 ; -5, 0, 11];");
            I("#b = chol(#a, 'lower');");  //upper er default
            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 1, -1d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 3, 3d, sharedDelta);

            I("RESET;");
            I("#a = [25, 15, -5 ; 15, 18, 0 ; -5, 0, 11];");
            I("#b = t(chol(#a, 'upper'));");
            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 1, -1d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 3, 3d, sharedDelta);

            I("RESET;");
            I("#a = [25, 15, -5 ; 15, 18, 0 ; -5, 0, 11];");
            I("#b = t(chol(#a));");  //upper er default
            _AssertMatrix(First(), "#b", "rows", 3);
            _AssertMatrix(First(), "#b", "cols", 3);
            _AssertMatrix(First(), "#b", 1, 1, 5d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 2, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 1, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 1, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 2, 3d, sharedDelta);
            _AssertMatrix(First(), "#b", 2, 3, 0d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 1, -1d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 2, 1d, sharedDelta);
            _AssertMatrix(First(), "#b", 3, 3, 3d, sharedDelta);

            //testing labels -----------------------------

            I("RESET;");
            I("LIST #rn = a, b, c;");
            I("LIST #cn = x, y, z, w;");
            I("matrix <rownames = #rn colnames = #cn> #a = ones(3, 4);");
            Matrix m = (Matrix)Program.databanks.GetFirst().GetIVariable("#a");
            Assert.AreEqual(m.rownames.Count, 3);
            Assert.AreEqual(m.colnames.Count, 4);
            I("PRT #a;");

            
        }

        [TestMethod]
        public void _Test_Scalars()
        {
            // Testing of VAL, DATE and STRING. Also test of indexer (fY[2000]).

            // ----------------------------------------------------------
            // ------------- testing VAL --------------------------------
            // ----------------------------------------------------------

            I("RESET;");

            //1.2345 set at start
            I("%v1 = 1.2345;");
            I("%v2 = %v1 * 10;");
            I("%v3 = %v1 + 10;");
            I("%v4 = %v1 / 10;");
            I("%v5 = log(%v1);");
            I("%v6 = exp(%v1);");
            I("%v7 = abs(%v1);");

            _AssertScalarVal(First(), "%v1", 1.2345d);
            _AssertScalarVal(First(), "%v2", 12.345d, sharedDelta);
            _AssertScalarVal(First(), "%v3", 11.2345d);
            _AssertScalarVal(First(), "%v4", 0.12345d, sharedDelta);
            _AssertScalarVal(First(), "%v5", Math.Log(1.2345d));
            _AssertScalarVal(First(), "%v6", Math.Exp(1.2345d));
            _AssertScalarVal(First(), "%v7", Math.Abs(1.2345d));

            FAIL("%v777 = v1 * 10;");  //this is not legal

            // ----------------------------------------------------------
            // ------------- testing VAL indexers (fy[2000]) ------------
            // ----------------------------------------------------------

            I("RESET;");

            I("TIME 2000 2003;");
            I("SERIES xx = 100;");
            I("SERIES<2001 2004> xx ^= 1;");
            I("%v2000 = xx[2000];");
            I("%v2001 = xx[2001];");
            I("%v2002 = xx[2002] + xx[2003];");

            _AssertScalarVal(First(), "%v2000", 100d);
            _AssertScalarVal(First(), "%v2001", 101d);
            _AssertScalarVal(First(), "%v2002", 205d);

            I("DATE %d = 2003;");
            I("%v = 2005;");
            I("%v2004 = xx[%d+1] + xx[%v-1];");
            _AssertScalarVal(First(), "%v2004", 208d);
            I("%v2004 = xx[2003a1+1] + xx[2005-1];");
            _AssertScalarVal(First(), "%v2004", 208d);  //same
            I("%v2004 = xx[2004a1] + xx[2004];");
            _AssertScalarVal(First(), "%v2004", 208d);  //same

            //Brief test on quarters
            I("OPTION freq q;");
            I("TIME 2000q1 2000q1;");
            I("SERIES xx = 111;");
            I("DATE %d = 1999q4;");
            I("VAL %v2001q = xx[%d+1];");
            _AssertScalarVal(First(), "%v2001q", 111d);
            I("VAL %v2001q = xx[1999q4+1];"); //same
            _AssertScalarVal(First(), "%v2001q", 111d);
            I("VAL %v2001q = xx[2000q1];"); //same
            _AssertScalarVal(First(), "%v2001q", 111d);

            // ----------------------------------------------------------
            // ------------- testing DATE -------------------------------
            // ----------------------------------------------------------

            // 1. Testing addition of periods
            // 2. Testing TIME %d1-3 %d2+4
            // 3. Testing DATE d3 = %v6 + 2 (%v6 is a VAL)

            I("RESET;");

            I("DATE %d1 = 2000;");
            I("DATE %d2 = %d1 + 2;");

            _AssertScalarDate(First(), "%d1", EFreq.A, 2000, 1);
            _AssertScalarDate(First(), "%d2", EFreq.A, 2002, 1);

            I("TIME %d1-3 %d2+4;");

            Assert.IsTrue(Globals.globalPeriodStart.super == 1997);
            Assert.IsTrue(Globals.globalPeriodStart.sub == 1);
            Assert.IsTrue(Globals.globalPeriodEnd.super == 2006);
            Assert.IsTrue(Globals.globalPeriodEnd.sub == 1);

            I("VAL %v6 = 2010;");
            I("DATE %d3 = %v6 + 2;");

            _AssertScalarVal(First(), "%v6", 2010);
            _AssertScalarDate(First(), "%d3", EFreq.A, 2012, 1);

            // ----------------------------------------------------------
            // ------------- testing DATE for freq Q --------------------
            // ----------------------------------------------------------

            // 1. Testing addition of periods
            // 2. Testing TIME %d1Q-2 %d2Q+5

            I("RESET;");

            I("OPTION freq q;");

            //TODO: should work independently of freq setting: have a freq in DATE object
            //      related: have a freq in time object

            I("DATE %d1Q = 2000q4;");
            I("DATE %d2Q = %d1Q + 2;");

            _AssertScalarDate(First(), "%d1Q", EFreq.Q, 2000, 4);
            _AssertScalarDate(First(), "%d2Q", EFreq.Q, 2001, 2);

            I("TIME %d1Q-2 %d2Q+5;");

            Assert.IsTrue(Globals.globalPeriodStart.super == 2000);
            Assert.IsTrue(Globals.globalPeriodStart.sub == 2);
            Assert.IsTrue(Globals.globalPeriodEnd.super == 2002);
            Assert.IsTrue(Globals.globalPeriodEnd.sub == 3);

            // ----------------------------------------------------------
            // ------------- testing STRING -----------------------------
            // ----------------------------------------------------------

            // 1. Testing STRING s3 = #s1 + #s2 (concatenation)

            I("RESET;");

            I("STRING %s1 = 'hel';");
            I("STRING %s2 = 'lo';");
            I("STRING %s3 = %s1 + %s2;");
            I("STRING %s4 = '{%s1}';");
            I("STRING %s5 = '~{%s1}';");

            _AssertScalarString(First(), "%s1", "hel");
            _AssertScalarString(First(), "%s2", "lo");
            _AssertScalarString(First(), "%s3", "hello");
            _AssertScalarString(First(), "%s4", "hel");  //in-substitute
            _AssertScalarString(First(), "%s5", "{%s1}");  //must not in-substitute here

            //==================== STRING functions etc. =======================================

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CLEAR<first>; IMPORT<tsd>jul05; CLONE;");
            I("time 2010 2015;");
            I("string %a = 'b';");
            I("string %b = 'c';");
            I("string %d = %{%a};                                        //recursive: %(%a) -> %('b') -> %b -> 'c', so %d = 'c'.");
            _AssertScalarString(First(), "%d", "c");
            
            I("string %n = 'a';");                                         //testing recursive on left-side
            I("val %v{%n} = 7;");
            I("val %(vv%n) = 77;");
            I("string %s = 'b';");
            I("val %vvv{%s} = 777;");
            I("val %(vvvv%s) = 7777;"); 
            _AssertScalarVal(First(), "%va", 7);
            _AssertScalarVal(First(), "%vva", 77);
            _AssertScalarVal(First(), "%vvvb", 777);
            _AssertScalarVal(First(), "%vvvvb", 7777);

            I("string %n = 'a';");
            I("CREATE {%n};");  //This should be possible, had a bug previously, so we test it here
            
            I("string %s1 = 'aa bb cc';");
            I("string %s2 = 'bb';");
            I("string %s3 = 'Ab';");

            I("string %s = concat(%s1, %s2);                             //same as '+' operator");
            _AssertScalarString(First(), "%s", "aa bb ccbb");
            I("val %n = length(%s1);");
            _AssertScalarVal(First(), "%n", 8);
            I("string %s = lower(%s3);                                    //lower case");
            _AssertScalarString(First(), "%s", "ab");
            I("string %s = piece(%s1, 4, 2);                             //substring");
            _AssertScalarString(First(), "%s", "bb");
            I("string %s = replace(%s1, %s2, 'xx');                      //replace %s2 with 'xx' in %s1");
            _AssertScalarString(First(), "%s", "aa xx cc");
            I("val %n = search(%s1, %s2);                                //find pos of %s2 in %s1");
            _AssertScalarVal(First(), "%n", 4);
            I("string %s = strip(%s1, %s2);                              //remove %s2 from %s1");
            _AssertScalarString(First(), "%s", "aa  cc");
            I("string %s = upper(%s1);                                   //upper case");
            _AssertScalarString(First(), "%s", "AA BB CC");
            I("string %s = strip('  abc  ');");
            _AssertScalarString(First(), "%s", "abc");
            I("val %v = startswith('abcde', 'abc');");
            _AssertScalarVal(First(), "%v", 1d);
            I("val %v = startswith('abcde', 'abd');");
            _AssertScalarVal(First(), "%v", 0d);
            I("val %v = endswith('abcde', 'cde');");
            _AssertScalarVal(First(), "%v", 1d);
            I("val %v = endswith('abcde', 'cdf');");
            _AssertScalarVal(First(), "%v", 0d);

            I("string %s = currentDateTime();                            //corresponds to STAMP command");
            I("string %s = currentFreq();                                //for instance 'a' or 'q' etc.");
            _AssertScalarString(First(), "%s", "a");
            I("date %d = currentPerStart();                              //for instance 2001q1");
            _AssertScalarDate(First(), "%d", EFreq.A, 2010, 1);
            I("date %d = currentPerEnd();                                //for instance 2005q4");
            _AssertScalarDate(First(), "%d", EFreq.A, 2015, 1);
            I("string %s = fromSeries(fY, 'label');                    //same as ASSIGN s FROM SERIES fY NAME. There will be fromMatrix etc.");
            _AssertScalarString(First(), "%s", "");
            I("string %s = fromSeries(Work:fY, 'label');               //can use bankname with colon");
            _AssertScalarString(First(), "%s", "");

            I("string %s = fromSeries(fY, 'source');");
            _AssertScalarString(First(), "%s", "");
            I("string %s = fromSeries(fY, 'stamp');");
            _AssertScalarString(First(), "%s", "04/28/ 8");

            I("date %d = fromSeries(fY, 'perStart');                   //first obs");
            _AssertScalarDate(First(), "%d", EFreq.A, 1998, 1);
            I("date %d = fromSeries(fY, 'perEnd');                     //last obs");
            _AssertScalarDate(First(), "%d", EFreq.A, 2079, 1);
            I("string %s = fromSeries(fY, 'freq');                     //freq of timeseries");
            _AssertScalarString(First(), "%s", "a");
            I("string %s = gekkoVersion();                               //for instance '1.8.1'");



        }

        [TestMethod]
        public void _Test_Expressions()
        {
            // ---------------------------------------------------------------------------
            // ------------- testing expressions, @, (<year>), (-1), (+1), functions etc.
            // ---------------------------------------------------------------------------
            I("RESET;");
            I("CREATE xA, xB;");
            I("SERIES<2000 2000> xA = 10;");
            I("SERIES<2001 2002> xA ^= 1;");
            I("CLONE;");
            I("SERIES<2001 2002> xA += 1;");  //10 11 12 in ref, 10 12 13 in work
            I("SERIES<2000 2002> xB = @xA;");

            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, 10d, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 11d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);
                        
            I("SERIES<2000 2002> xB = @xA[-1];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 10d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, 11d, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);
            
            I("SERIES<2000 2002> xB = xA[-1];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 10d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);
            
            I("SERIES<2000 2002> xB = @xA[+1];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, 11d, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);

            I("SERIES<2000 2002> xB = xA[+1];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 13d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);

            //just testing use of scalar for lead
            I("VAL %lead = 1;");
            I("SERIES<2000 2002> xB = xA[+%lead];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 13d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);

            I("SERIES<2000 2002> xB = xA[2002];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, 13d, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 13d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, 13d, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);
            I("SERIES<2000 2002> xB = @xA[2002];");
            _AssertSeries(First(), "xb", 1999, double.NaN, sharedDelta);
            _AssertSeries(First(), "xb", 2000, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2001, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2002, 12d, sharedDelta);
            _AssertSeries(First(), "xb", 2003, double.NaN, sharedDelta);
        }       

        [TestMethod]
        public void _Test_DataFormatsInOut()
        {

            //tsdx
            //tsd
            //csv
            //prn
            //xls(x), also via SHEET (includes importing matrix), both internal and excel engine            
            //flat
            //gnplot (only writing)
            //tsp... hmmm not done...
            //px (only reading)

            //The testing includes tests where data is put in a separate bank named 'other'
            //There is also a test of IMPORT<per1 per2>

            // testing on annual
            // testing on annual
            // testing on annual
            // testing on annual
            // testing on annual
            // testing on annual
            // testing on annual

            List<string> other = new List<string>();
            other.Add(null);
            if (Globals.UNITTESTFOLLOWUP_important) other.Add("other:");

            foreach (string bank in other)
            {

                I("RESET;");
                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
                // ------ tsdx
                if (bank == null)
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<" + Globals.extensionDatabank + ">temp;");
                    I("RESET;");
                    I("READ<" + Globals.extensionDatabank + ">temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ tsdx, selection
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<" + Globals.extensionDatabank + ">xx1, " + bank + "xx3 file=temp;");
                    I("RESET;");
                    I("READ<" + Globals.extensionDatabank + ">temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ tsd
                if (bank == null)
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<tsd>temp;");
                    I("RESET;");
                    I("READ<tsd>temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ tsd, selection
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<tsd>xx1, " + bank + "xx3 file=temp;");
                    I("RESET;");
                    I("READ<tsd>temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ csv
                if (bank == null)
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001 2002 csv>temp;");
                    I("RESET;");
                    I("READ<csv>temp;");
                    ReadFormatsHelper("a", bank);

                    //cols
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001 2002 csv cols>temp;");
                    I("RESET;");
                    I("READ<csv cols>temp;");
                    ReadFormatsHelper("a", bank);

                }
                // ------ csv, selection
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<2001 2002 csv>xx1, " + bank + "xx3 file=temp;");
                    I("RESET;");
                    I("READ<csv>temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ prn
                if (bank == null)
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001 2002 prn>temp;");
                    I("RESET;");
                    I("READ<prn>temp;");
                    ReadFormatsHelper("a", bank);

                    //cols
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001 2002 prn cols>temp;");
                    I("RESET;");
                    I("READ<prn cols>temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ prn, selection
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<2001 2002 prn>xx1, " + bank + "xx3 file=temp;");
                    I("RESET;");
                    I("READ<prn>temp;");
                    ReadFormatsHelper("a", bank);
                }
                // ------ xlsx
                for (int i = 0; i < 2; i++)
                {
                    if (bank == null)
                    {
                        I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        I("WRITE<xlsx>temp;");
                        I("RESET;");
                        I("READ<xlsx>temp;");
                        ReadFormatsHelper("a", bank);

                        //cols
                        I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        I("WRITE<xlsx cols>temp;");
                        I("RESET;");
                        I("READ<xlsx cols>temp;");
                        ReadFormatsHelper("a", bank);
                    }
                    // ------ xlsx, selection
                    {
                        I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                        I("WRITE<xlsx>xx1, " + bank + "xx3 file=temp;");
                        I("RESET;");
                        I("READ<xlsx>temp;");
                        ReadFormatsHelper("a", bank);
                    }

                    // ------ xlsx cells with SHEET and SHEET<import>               
                    {
                        I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                        I("SHEET <2001 2002 SHEET='test' CELL='C5' DATES=no NAMES=no COLORS=no> xx1, " + bank + "xx3 file=temp;");  //export
                        I("RESET;");
                        I("SHEET <2001 2002 IMPORT SHEET='test' CELL='C5'> xx1, xx3 file=temp;");  //import            
                        ReadFormatsHelper("a", bank);

                        //cols
                        I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                        I("SHEET <2001 2002 SHEET='test' CELL='C5' DATES=no NAMES=no COLORS=no cols> xx1, " + bank + "xx3 file=temp;");  //export
                        I("RESET;");
                        I("SHEET <2001 2002 IMPORT SHEET='test' CELL='C5' cols> xx1, xx3 file=temp;");  //import            
                        ReadFormatsHelper("a", bank);

                        //
                        // test matrix import
                        //
                        I("SHEET <2001 2002 IMPORT MATRIX SHEET='test' CELL='C5'> work:#m file=temp;");  //imports 2x2 matrix #m
                        _AssertMatrix(First(), "#m", 1, 1, 1001, sharedDelta);
                        _AssertMatrix(First(), "#m", 1, 2, 3001, sharedDelta);
                        _AssertMatrix(First(), "#m", 2, 1, 1002, sharedDelta);                        
                        _AssertMatrix(First(), "#m", 2, 2, 3002, sharedDelta);                        
                    }
                }
                // ------ gnuplot (not actually testing the file)     
                if (bank == null)
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001 2002 gnuplot>temp;");
                }
                // ------ gnuplot, selection  (not actually testing the file)                
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<2001 2002 gnuplot>xx1, " + bank + "xx3 file=temp;");
                }
                // ------ flat
                if (bank == null)
                {
                    I("RESET; TIME 2001 2002; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001 2002 flat>temp;");
                    I("RESET;");
                    I("READ<flat>temp;");
                    ReadFormatsHelper("a", bank);
                }

                // ---------- Testing on quarters
                // ---------- Testing on quarters
                // ---------- Testing on quarters
                // ---------- Testing on quarters
                // ---------- Testing on quarters

                I("RESET;");
                Program.DeleteFolder(Globals.ttPath2 + @"\regres\Databanks\temp");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\temp';");
                I("OPTION freq q;");
                // ------ tsdx
                if (bank == null)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<" + Globals.extensionDatabank + ">temp;");
                    I("RESET;");
                    I("READ<" + Globals.extensionDatabank + ">temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ tsdx, selection
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<" + Globals.extensionDatabank + ">xx1, " + bank + "xx3 file=temp;");
                    I("RESET;");
                    I("READ<" + Globals.extensionDatabank + ">temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ tsd
                if (bank == null)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<tsd>temp;");
                    I("RESET;");
                    I("READ<tsd>temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ tsd, selection
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<tsd>xx1, " + bank + "xx3 file=temp;");
                    I("RESET;");
                    I("READ<tsd>temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ csv
                if (bank == null)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001q1 2001q2 csv>temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<csv>temp;");
                    ReadFormatsHelper("q", bank);

                    //cols
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001q1 2001q2 csv cols>temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<csv cols>temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ csv, selection
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<2001q1 2001q2 csv>xx1, " + bank + "xx3 file=temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<csv>temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ prn
                if (bank == null)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001q1 2001q2 prn>temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<prn>temp;");
                    ReadFormatsHelper("q", bank);

                    //cols
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001q1 2001q2 prn cols>temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<prn cols>temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ prn, selection
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<2001q1 2001q2 prn>xx1, " + bank + "xx3 file=temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<prn>temp;");
                    ReadFormatsHelper("q", bank);
                }
                // ------ xlsx
                for (int i = 0; i < 2; i++)
                {
                    if (bank == null)
                    {
                        I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        I("WRITE<xlsx>temp;");
                        I("RESET; OPTION freq q;");  //must tell Gekko what freq
                        I("READ<xlsx>temp;");
                        ReadFormatsHelper("q", bank);

                        //cols
                        I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        I("WRITE<xlsx cols>temp;");
                        I("RESET; OPTION freq q;");  //must tell Gekko what freq
                        I("READ<xlsx cols>temp;");
                        ReadFormatsHelper("q", bank);
                    }
                    // ------ xlsx, selection
                    {
                        I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                        if (i == 0) I("OPTION sheet engine = internal;");
                        else I("OPTION sheet engine = excel;");
                        if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                        I("WRITE<xlsx>xx1, " + bank + "xx3 file=temp;");
                        I("RESET; OPTION freq q;");  //must tell Gekko what freq
                        I("READ<xlsx>temp;");
                        ReadFormatsHelper("q", bank);
                    }

                    // ------ xlsx cells with SHEET and SHEET<import>
                    {
                        if (Globals.UNITTESTFOLLOWUP_important)
                        {
                            I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                            if (i == 0) I("OPTION sheet engine = internal;");
                            else I("OPTION sheet engine = excel;");
                            if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                            I("SHEET <2001q1 2001q2 SHEET='test' CELL='C5' DATES=no NAMES=no COLORS=no> xx1, " + bank + "xx3 file=temp;");  //export
                            I("RESET; OPTION freq q;");  //must tell Gekko what freq
                            I("SHEET <2001q1 2001q2 IMPORT SHEET='test' CELL='C5'> xx1, xx3 file=temp;");  //import            
                            ReadFormatsHelper("q", bank);

                            //TODO: <cols>
                        }
                    }
                }
                // ------ gnuplot (not actually testing the file)
                if (bank == null)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001q1 2001q2 gnuplot>temp;");
                }
                // ------ gnuplot, selection  (not actually testing the file)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    if (bank != null) I("OPEN <edit> other; CLEAR other;  SER xx3 = (4001, 4002); CLOSE other; OPEN other;");
                    I("WRITE<2001q1 2001q2 gnuplot>xx1, " + bank + "xx3 file=temp;");
                }

                // ------ flat
                if (bank == null)
                {
                    I("RESET; OPTION freq q; TIME 2001q1 2001q2; SER xx1 = (1001, 1002); SER xx3 = (3001, 3002);");
                    I("WRITE<2001q1 2001q2 flat>temp;");
                    I("RESET; OPTION freq q;");  //must tell Gekko what freq
                    I("READ<flat>temp;");
                    ReadFormatsHelper("q", bank);                    
                }
            }


            // ----------------------------- testing px reading ---------------------
            // this downloads the px:             //I("DOWNLOAD http://api.statbank.dk/v1/data statbank.json dump = data;");
            // Note varegruppe instead of VAREGR

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
            I("IMPORT <px> data;");
            _AssertSeries(First(), "pris6_VAREGRuppe_011200_enhed_100!m", EFreq.M, 2000, 1, 98.1d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011100_enhed_100!m", EFreq.M, 2000, 1, 98.3d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011200_enhed_100!m", EFreq.M, 2001, 3, 102.9d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011100_enhed_100!m", EFreq.M, 2001, 3, 103.1d, sharedDelta);
                        
            //Truncated time period
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");            
            I("IMPORT <2000m2 2001m3 px> data;");
            _AssertSeries(First(), "pris6_VAREGRuppe_011200_enhed_100!m", EFreq.M, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011100_enhed_100!m", EFreq.M, 2000, 1, double.NaN, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011200_enhed_100!m", EFreq.M, 2000, 2, 97.8, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011100_enhed_100!m", EFreq.M, 2000, 2, 98.7, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011200_enhed_100!m", EFreq.M, 2001, 3, 102.9d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011100_enhed_100!m", EFreq.M, 2001, 3, 103.1d, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011200_enhed_100!m", EFreq.M, 2001, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "pris6_VAREGRuppe_011100_enhed_100!m", EFreq.M, 2001, 4, double.NaN, sharedDelta);

            //Another px-file, with blanks and parentheses in stub
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
            I("IMPORT <px> prod01;");            
            _AssertSeries(First(), "PROD01_saesonkorrigering_EJSAESON_brancheDB07_BC!m", EFreq.M, 2017, 3, 119.9d, sharedDelta);
            _AssertSeries(First(), "PROD01_saesonkorrigering_EJSAESON_brancheDB07_10001!m", EFreq.M, 2017, 3, 106.5d, sharedDelta);
            

        }

        private static void ReadFormatsHelper(string freq, string bank)
        {
            if (bank == null)
            {
                if (freq == "a")
                {
                    _AssertSeries(First(), "xx1", 2000, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx1", 2001, 1001, sharedDelta);
                    _AssertSeries(First(), "xx1", 2002, 1002, sharedDelta);
                    _AssertSeries(First(), "xx1", 2003, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3", 2000, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3", 2001, 3001, sharedDelta);
                    _AssertSeries(First(), "xx3", 2002, 3002, sharedDelta);
                    _AssertSeries(First(), "xx3", 2003, double.NaN, sharedDelta);
                }
                else if (freq == "q")
                {
                    _AssertSeries(First(), "xx1!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx1!q", EFreq.Q, 2001, 1, 1001, sharedDelta);
                    _AssertSeries(First(), "xx1!q", EFreq.Q, 2001, 2, 1002, sharedDelta);
                    _AssertSeries(First(), "xx1!q", EFreq.Q, 2001, 3, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3!q", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3!q", EFreq.Q, 2001, 1, 3001, sharedDelta);
                    _AssertSeries(First(), "xx3!q", EFreq.Q, 2001, 2, 3002, sharedDelta);
                    _AssertSeries(First(), "xx3!q", EFreq.Q, 2001, 3, double.NaN, sharedDelta);
                }
                else throw new GekkoException();
            }
            else
            {
                if (freq == "a")
                {
                    _AssertSeries(First(), "xx1", 2000, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx1", 2001, 1001, sharedDelta);
                    _AssertSeries(First(), "xx1", 2002, 1002, sharedDelta);
                    _AssertSeries(First(), "xx1", 2003, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3", 2000, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3", 2001, 4001, sharedDelta);
                    _AssertSeries(First(), "xx3", 2002, 4002, sharedDelta);
                    _AssertSeries(First(), "xx3", 2003, double.NaN, sharedDelta);
                }
                else if (freq == "q")
                {
                    _AssertSeries(First(), "xx1", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx1", EFreq.Q, 2001, 1, 1001, sharedDelta);
                    _AssertSeries(First(), "xx1", EFreq.Q, 2001, 2, 1002, sharedDelta);
                    _AssertSeries(First(), "xx1", EFreq.Q, 2001, 3, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3", EFreq.Q, 1999, 4, double.NaN, sharedDelta);
                    _AssertSeries(First(), "xx3", EFreq.Q, 2001, 1, 4001, sharedDelta);
                    _AssertSeries(First(), "xx3", EFreq.Q, 2001, 2, 4002, sharedDelta);
                    _AssertSeries(First(), "xx3", EFreq.Q, 2001, 3, double.NaN, sharedDelta);
                }
                else throw new GekkoException();
            }

        }

        [TestMethod]
        public void _Test_ExportR()
        {
            //TODO: look at the R file...
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\temp';");
            I("MATRIX #m1 = [1, 2 ; 3, 4];");
            I("MATRIX #m2 = [11, 12 ; 13, 14];");
            //I("EXPORT<r> ('#m1', '#m2') file=matrix.r;");
            I("EXPORT<r> #m1, #m2 file=matrix.r;");
        }

        [TestMethod]
        public void Test__ExportSeries()
        {

            // ------------------------ '=' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series = '='> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("RUN deleteme." + Globals.extensionCommand + ";");
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0d);
            _AssertSeries(First(), "y", 2002, 102d, 0d);
            _AssertSeries(First(), "y", 2003, 104d, 0d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ '<nothing>' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("RUN deleteme." + Globals.extensionCommand + ";");
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0d);
            _AssertSeries(First(), "y", 2002, 102d, 0d);
            _AssertSeries(First(), "y", 2003, 104d, 0d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ 'n' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series = n> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("RUN deleteme." + Globals.extensionCommand + ";");
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0d);
            _AssertSeries(First(), "y", 2002, 102d, 0d);
            _AssertSeries(First(), "y", 2003, 104d, 0d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ '^' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101 rep *;");
            I("SERIES <2002 2003> y ^ 1, 2;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2002 2003 series = '^'> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("SERIES <2001 2001> y = 101;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0d);
            _AssertSeries(First(), "y", 2002, 102d, 0d);
            _AssertSeries(First(), "y", 2003, 104d, 0d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ 'd' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101 rep *;");
            I("SERIES <2002 2003 d> y = 1, 2;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2002 2003 series = d> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("SERIES <2001 2001> y = 101;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0d);
            _AssertSeries(First(), "y", 2002, 102d, 0d);
            _AssertSeries(First(), "y", 2003, 104d, 0d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ '%' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101 rep *;");
            I("SERIES <2002 2003> y % 2 4;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2002 2003 series = '%'> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("SERIES <2001 2001> y = 101;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0.000001d);
            _AssertSeries(First(), "y", 2002, 1.02 * 101d, 0.000001d);
            _AssertSeries(First(), "y", 2003, 1.04 * 1.02 * 101d, 0.000001d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ 'p' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101 rep *;");
            I("SERIES <2002 2003 p> y = 2, 4;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2002 2003 series = p> deleteme;");
            I("RESET;");
            I("CREATE y;");
            I("SERIES <2001 2001> y = 101;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 101d, 0.000001d);
            _AssertSeries(First(), "y", 2002, 1.02 * 101d, 0.000001d);
            _AssertSeries(First(), "y", 2003, 1.04 * 1.02 * 101d, 0.000001d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ '+' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("CLONE;");
            I("SERIES y + 1, 2, 4;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series = '+'> deleteme;");
            I("RESET; TIME 2001 2003;");
            I("CREATE y;");
            I("SERIES y = 101, 102, 104;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 102d, 0.000001d);
            _AssertSeries(First(), "y", 2002, 104d, 0.000001d);
            _AssertSeries(First(), "y", 2003, 108d, 0.000001d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ 'm' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("CLONE;");
            I("SERIES <m> y = 1, 2, 4;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series = m> deleteme;");
            I("RESET; TIME 2001 2003;");
            I("CREATE y;");
            I("SERIES y = 101, 102, 104;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 102d, 0.000001d);
            _AssertSeries(First(), "y", 2002, 104d, 0.000001d);
            _AssertSeries(First(), "y", 2003, 108d, 0.000001d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ '*' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("CLONE;");
            I("SERIES <2001 2003> y * 1.01, 1.02, 1.04;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series = '*'> deleteme;");
            I("RESET; TIME 2001 2003;");
            I("CREATE y;");
            I("SERIES y = 101, 102, 104;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 1.01d * 101d, 0.000001d);
            _AssertSeries(First(), "y", 2002, 1.02d * 102d, 0.000001d);
            _AssertSeries(First(), "y", 2003, 1.04d * 104d, 0.000001d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);

            // ------------------------ 'q' --------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("CREATE y;");
            I("TIME 2001 2003;");
            I("SERIES y = 101, 102, 104;");
            I("CLONE;");
            I("SERIES <2001 2003 q> y = 1, 2, 4;");
            I("SYS'del deleteme." + Globals.extensionCommand + "';");
            I("EXPORT<2001 2003 series = q> deleteme;");
            I("RESET; TIME 2001 2003;");
            I("CREATE y;");
            I("SERIES y = 101, 102, 104;");
            I("RUN deleteme." + Globals.extensionCommand + ";");            
            _AssertSeries(First(), "y", 2000, double.NaN, 0d);
            _AssertSeries(First(), "y", 2001, 1.01d * 101d, 0.000001d);
            _AssertSeries(First(), "y", 2002, 1.02d * 102d, 0.000001d);
            _AssertSeries(First(), "y", 2003, 1.04d * 104d, 0.000001d);
            _AssertSeries(First(), "y", 2004, double.NaN, 0d);


        }

        

        [TestMethod]
        public void Test__Print()
        {

            //TODO: make more print tests (expressions, transformations...)

            I("RESET;");
            I("CREATE ts;");
            I("TIME 2010 2012;");
            I("SERIES ts = 1.23456;");
            I("SERIES<2011 2012>ts % 5.6789;");
            I("TIME 2009 2013;");
            I("PRT ts;");
            //string s = "";
            //s += "                      ts  %" + G.NL;
            //s += "2009              M  ******" + G.NL;
            //s += "2010         1.2346  ******" + G.NL;
            //s += "2011         1.3047    5.68" + G.NL;
            //s += "2012         1.3788    5.68" + G.NL;
            //s += "2013              M  ******" + G.NL;
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.2346"));  //stupid test, must be done better...
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3047"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3788"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("5.68"));

            //quarterly
            I("RESET;");
            I("CLS;"); //to get unitTestScreenOutput flushed
            I("OPTION freq q;");
            I("CREATE ts;");
            I("TIME 2010q3 2011q1;");
            I("SERIES ts = 1.23456;");
            I("SERIES<2010q4 2011q1>ts % 5.6789;");
            I("TIME 2010q2 2011q2;");
            I("PRT ts;");
            //s = "                         ts  %" + G.NL;
            //s += "2010q2               M  ******" + G.NL;
            //s += "2010q3          1.2346  ******" + G.NL;
            //s += "2010q4          1.3047    5.68" + G.NL;
            //s += "2011q1          1.3788    5.68" + G.NL;
            //s += "2011q2               M  ******" + G.NL;
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.2346"));  //stupid test, must be done better...
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3047"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3788"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("5.68"));


            //quarterly
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Databanks\';");
            I("CLS;"); //to get unitTestScreenOutput flushed
            I("OPTION freq q;");
            I("CREATE ts;");
            I("TIME 2010q3 2011q1;");
            I("SERIES ts = 1.23456;");
            I("SERIES<2010q4 2011q1>ts % 5.6789;");
            I("TIME 2010q2 2011q2;");
            I("WRITE temp;");
            I("CLONE;");
            I("PRT @ts;");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.2346"));  //stupid test, must be done better...
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3047"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3788"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("5.68"));
            I("RESET;");
            I("OPTION freq q;");
            I("CLS;"); //to get unitTestScreenOutput flushed
            I("OPEN <" + Globals.extensionDatabank + "> temp;");
            I("PRT temp:ts;");
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.2346"));  //stupid test, must be done better...
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3047"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("1.3788"));
            Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains("5.68"));

            // ---------------------------------------------------------------------------------
            // ------------------- more precise tests, using clipboard table -------------------
            // ---------------------------------------------------------------------------------

            //We test use of lists, of names, of colons, of @
            //also <_lev> etc.

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("MODEL jul05;");            
            I("TIME 2011 2015;");
            I("CREATE px, fx, x;");
            I("LIST xx = px;");
            I("SERIES px = 1, 1.1, 1.2, 1.4, 1.3;");
            I("SERIES fx = 100, 120, 90, 110, 150;");
            I("SERIES x = px * fx;");
            I("CLONE;");
            I("SERIES px * 1.01, 1.05, 1.03, 1.02, 1.04;");
            I("SERIES fx * 1.11, 1.15, 1.13, 1.12, 1.14;");
            I("SERIES x = px * fx;");
            I("OPEN<edit>mybank; CLEAR mybank;");
            I("CREATE fx; SERIES fx = 200;");
            I("CLOSE mybank; OPEN mybank;");            
            
            //=======================
            //=======================
            //=======================
            // TODO: do this properly, the below results are from 1.8
            //       has been visually checked for 2.0.2
            //=======================
            //=======================
            
            //Should show both level and pch
            I("PRT<2014 2014 r> #xx;");

            //Should show only level
            I("PRT<2014 2014 rn> #xx;");
            
            //TODO also test %n where %n is name 'fx'
            //Should show (E)
            I("PRT<2014 2014> #xx;");

            //[lev]             px           x/px 
            //2014          1.4280       123.2000 
            I("PRT<2014 2014 abs> #xx, x/px;");
            
            // [%]        px      x/px 
            //2014     15.53     21.14
            I("PRT<2014 2014 pch> #xx, x/px;");
            
            //        px [%]     x/px [dif] 
            //2014     15.53        21.5000             
            I("PRT<2014 2014 pch> #xx, x/px<dif>;");
            
            //                 px           x/px 
            //2014         1.4280       123.2000 
            I("PRT<2014 2014 nopch> #xx, x/px;");
            
            //[dif]             px           x/px 
            //2014          0.1920        21.5000
            I("PRT<2014 2014 dif> #xx, x/px;");
            
            //[dif%]        px      x/px 
            //2014        8.52     47.44
            I("PRT<2014 2014 gdif> #xx, x/px;");
            
            //                 px          [dif]       [%]           x/px          [dif]       [%] 
            //2014         1.4280         0.1920     15.53       123.2000        21.5000     21.14 
            I("PRT<2014 2014 _dif> #xx, x/px;");
            
            //                 px  x/px [%] 
            //2014              1      21.1 
            I("PRT<2014 2014> #xx <n dec=0>, x/px<p dec=1>;");
            
            //                 px       [%]         [@lev]      [@%]         [mdif]      [m%] 
            //2014         1.4280     15.53         1.4000     16.67         0.0280      2.00 
                                                                                  
            //               x/px       [%]         [@lev]      [@%]         [mdif]      [m%] 
            //2014       123.2000     21.14       110.0000     22.22        13.2000     12.00 
            I("PRT<2014 2014 n p r rp m q> #xx, x/px;");            
            
            //                 px      (E)%           x/px      (E)% 
            //2014         1.4280     15.53       123.2000     21.14 
            I("PRT<2014 2014 nofilter> #xx, x/px;");
            
            //[mdif]      mybank:fx 
            //2014          90.0000 
            I("PRT<2014 2014 m> mybank:fx;");
            
            //[@lev]      mybank:fx 
            //2014         110.0000             
            I("PRT<2014 2014 r> mybank:fx;");
            
            //                 px          [dif]       [%]           x/px          [dif]       [%] 
            //2014         1.4280         0.1920     15.53       123.2000        21.5000     21.14 
            I("OPTION print prt dif = yes;");
            I("PRT<2014 2014> #xx, x/px;");
            I("OPTION print prt dif = no;");

            //[lev]             px           x/px 
            //2014          1.4280       123.2000 
            I("MULPRT<2014 2014 lev> #xx, x/px;");
            
            //                 px         [mdif]      [m%]           x/px         [mdif]      [m%] 
            //2014         1.4280         0.0280      2.00       123.2000        13.2000     12.00 
            I("MULPRT<2014 2014 _lev> #xx, x/px;");
            
            //[mdif]             px           x/px 
            //2014           0.0280        13.2000 
            I("MULPRT<2014 2014 abs> #xx, x/px;");

            //[m%]        px      x/px 
            //2014      2.00     12.00            
            I("MULPRT<2014 2014 pch> #xx, x/px;");
            
            //[mdif%]        px      x/px 
            //2014        -1.13     -1.08 
            I("MULPRT<2014 2014 gdif> #xx, x/px;");
            
            //                 px         %       Baseline         %     Difference         % 
            //2014         1.4280     15.53         1.4000     16.67         0.0280      2.00 
                                                                                  
            //               x/px         %       Baseline         %     Difference         % 
            //2014       123.2000     21.14       110.0000     22.22        13.2000     12.00 
            I("MULPRT<2014 2014 v> #xx, x/px;");
            
            //                 px         [mdif]      [m%]           x/px         [mdif]      [m%] 
            //2014         1.4280         0.0280      2.00       123.2000        13.2000     12.00 
            I("OPTION print mulprt lev = yes;");
            I("MULPRT<2014 2014> #xx, x/px;");
            I("OPTION print mulprt lev = no;");
            
            FAIL("MULPRT<n>#xx, x/px;");
            FAIL("MULPRT<d>#xx, x/px;");
            FAIL("MULPRT<p>#xx, x/px;");
            FAIL("MULPRT<dp>#xx, x/px;");
            FAIL("MULPRT<r>#xx, x/px;");
            FAIL("MULPRT<rn>#xx, x/px;");
            FAIL("MULPRT<rd>#xx, x/px;");
            FAIL("MULPRT<rp>#xx, x/px;");
            FAIL("MULPRT<rdp>#xx, x/px;");
            FAIL("MULPRT<m>#xx, x/px;");
            FAIL("MULPRT<q>#xx, x/px;");
            FAIL("MULPRT<mp>#xx, x/px;");

            //=======================
            //=======================
            //=======================
            //=======================
            //=======================




            Globals.lastPrtOrMulprtTable = null;
            I("PRT<2014 2014> #xx, x/px;");
            Table table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.GetRowMaxNumber(), 2);
            Assert.AreEqual(table.GetColMaxNumber(), 5);            
            Assert.IsNull(table.Get(1, 1));
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "(E)%");
            Assert.AreEqual(table.Get(1, 4).CellText.TextData[0], "x/px");
            Assert.AreEqual(table.Get(1, 5).CellText.TextData[0], "%");
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
            _AssertHelperTwoDoubles(table.Get(2, 2).number, 1.428, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 3).number, 15.53398058, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 4).number, 123.2, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 5).number, 21.14060964, sharedTableDelta);

            Globals.lastPrtOrMulprtTable = null;
            I("PRT<2014 2014 n> #xx, x/px;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.GetRowMaxNumber(), 2);
            Assert.AreEqual(table.GetColMaxNumber(), 3);
            Assert.IsNull(table.Get(1, 1));
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");            
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");            
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?            
            _AssertHelperTwoDoubles(table.Get(2, 2).number, 1.428, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 3).number, 123.2, sharedTableDelta);

            Globals.lastPrtOrMulprtTable = null;
            I("PRT<2014 2014 d> #xx, x/px;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.GetRowMaxNumber(), 2);
            Assert.AreEqual(table.GetColMaxNumber(), 3);            
            Assert.AreEqual(table.Get(1, 1).CellText.TextData[0], "[dif]");
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");          
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
            _AssertHelperTwoDoubles(table.Get(2, 2).number, 0.1920, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 3).number, 21.50, sharedTableDelta);

            Globals.lastPrtOrMulprtTable = null;
            I("PRT<2014 2014 p> #xx, x/px;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.GetRowMaxNumber(), 2);
            Assert.AreEqual(table.GetColMaxNumber(), 3);            
            Assert.AreEqual(table.Get(1, 1).CellText.TextData[0], "[%]");
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");          
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
            _AssertHelperTwoDoubles(table.Get(2, 2).number, 15.5340, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 3).number, 21.1406, sharedTableDelta);

            Globals.lastPrtOrMulprtTable = null;
            I("PRT<2014 2014 m> #xx, x/px;");
            table = Globals.lastPrtOrMulprtTable;
            Assert.AreEqual(table.GetRowMaxNumber(), 2);
            Assert.AreEqual(table.GetColMaxNumber(), 3);            
            Assert.AreEqual(table.Get(1, 1).CellText.TextData[0], "[mdif]");
            Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
            Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");          
            Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
            _AssertHelperTwoDoubles(table.Get(2, 2).number, 0.0280, sharedTableDelta);
            _AssertHelperTwoDoubles(table.Get(2, 3).number, 13.2, sharedTableDelta);

            //TODO TODO TODO TODO
            //TODO TODO TODO TODO
            //TODO TODO TODO TODO
            //TODO TODO TODO TODO
            //TODO TODO TODO TODO
            //TODO TODO TODO TODO do these
            if (false)
            {
                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 q> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 3);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.AreEqual(table.Get(1, 1).CellText.TextData[0], "[m%]");
                //Assert.IsNull(table.Get(1, 2));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x");
                Assert.IsNull(table.Get(2, 1));
                Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "px [m%]");
                Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], "/px [m%]");
                Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(3, 2).number, 2d, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(3, 3).number, 12d, sharedTableDelta);

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 dp> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 3);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "p");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/p");
                Assert.IsNull(table.Get(2, 1));
                Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "x [dif%]");
                Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], "x [dif%]");
                Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(3, 2).number, 8.5210, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(3, 3).number, 47.4450, sharedTableDelta);

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 mp> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 3);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");
                Assert.IsNull(table.Get(2, 1));
                Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], " [mdif%]");
                Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], " [mdif%]");
                Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(3, 2).number, -1.1327, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(3, 3).number, -1.0816, sharedTableDelta);

                // ----------- BASE bank ------------

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 r> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 2);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");
                Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(2, 2).number, 1.4, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(2, 3).number, 110, sharedTableDelta);

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 n> #xx, ref:x/@px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 2);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "ref:x/@px");
                Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(2, 2).number, 1.428, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(2, 3).number, 110, sharedTableDelta);

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 rd> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 2);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px [@dif]");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px [@dif]");
                Assert.AreEqual(table.Get(2, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(2, 2).number, 0.2, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(2, 3).number, 20d, sharedTableDelta);

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 rp> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 3);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x");
                Assert.IsNull(table.Get(2, 1));
                Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], "px [@%]");
                Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], "/px [@%]");
                Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(3, 2).number, 16.6667, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(3, 3).number, 22.2222, sharedTableDelta);

                Globals.lastPrtOrMulprtTable = null;
                I("PRT<2014 2014 rdp> #xx, x/px;");
                table = Globals.lastPrtOrMulprtTable;
                Assert.AreEqual(table.GetRowMaxNumber(), 3);
                Assert.AreEqual(table.GetColMaxNumber(), 3);
                Assert.IsNull(table.Get(1, 1));
                Assert.AreEqual(table.Get(1, 2).CellText.TextData[0], "px");
                Assert.AreEqual(table.Get(1, 3).CellText.TextData[0], "x/px");
                Assert.IsNull(table.Get(2, 1));
                Assert.AreEqual(table.Get(2, 2).CellText.TextData[0], " [@dif%]");
                Assert.AreEqual(table.Get(2, 3).CellText.TextData[0], " [@dif%]");
                Assert.AreEqual(table.Get(3, 1).CellText.TextData[0], "2014"); //why is it not a date?
                _AssertHelperTwoDoubles(table.Get(3, 2).number, 7.5758, sharedTableDelta);
                _AssertHelperTwoDoubles(table.Get(3, 3).number, 47.2222, sharedTableDelta);

            }

            //TODO TODO
            //These two should give the same (in levels):
            //mulprt lang11:fy;
            //prt lang11:fy-ref:fy;
            //So MULPRT with bank:var tries to find var in ref bank.
            //This is logical, because of the special case MULPRT work:var!
            //Seems this is ok now, but do the test

        }

        [TestMethod]
        public void _Test_Res()
        {
            //-----------------------------------------------------------
            //----------------- testing RES -----------------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\RES\';");
            I("RUN test_res.cmd;");
            CheckFullDatabank(0.0001, 0.0001, 1990, 2009);  //must be < 0.0001% or < 0.0001 absolute. Quite strict.
        }

        [TestMethod]
        public void _Test_Efter()
        {
            //-----------------------------------------------------------
            //----------------- testing EFTER ---------------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\EFTER\';");
            I("RUN test_efter.cmd;");
            //EFTER period is 2005-6, extra years checked for safety
            //The test is a bit artificial, just doing an EFTER on a model and databank. But the result is
            //taken from Gekko 1.4 which we consider the 'truth' regarding EFTER :-).
            //Get a real check from TMK at some point.
            //The EFTER command here produces 149 differences, and there are both real vars and add-factors among these.
            CheckFullDatabank(0.0001, 0.0001, 2004, 2007);  //must be < 0.0001% or < 0.0001 absolute. Quite strict.
        }

        [TestMethod]
        public void Test__ModelLille1()
        {
            //-----------------------------------------------------------
            //----------------- testing lille1.cmd ----------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Models\';");
            I("RUN lille1.cmd;");
            double delta = 0.0002d;
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 42960.0455d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 42960.0455d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 85920.0909d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 85920.0909d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 284964.7035d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 284964.7035d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 945121.0002d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 945121.0002d, delta);
        }

        [TestMethod]
        public void Test__ModelLille2()
        {
            //-----------------------------------------------------------
            //----------------- testing lille2.cmd ----------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Models\';");
            I("RUN lille2.cmd;");
            double delta = 0.01d;  //the numbers are quite large, so 0.01 is strict.
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 67695.0934d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 67695.0934d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 77545.9412d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 77545.9412d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 222821.3945d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 222821.3945d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 989331.9881d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 989331.9881d, delta);
        }

        [TestMethod]
        public void Test__ModelLille3()
        {
            //-----------------------------------------------------------
            //----------------- testing lille3.cmd ----------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Models\';");
            I("RUN lille3.cmd;");
            double delta = 0.0001d;  //the numbers are quite large, so 0.01 is strict.
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 146098.8121d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 146098.8121d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 48482.9387d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 48482.9387d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 533625.2015d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 533625.2015d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 935807.1374d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 935807.1374d, delta);
        }

        [TestMethod]
        public void Test__ModelLille4()
        {
            //-----------------------------------------------------------
            //----------------- testing lille4.cmd ----------------------
            //-----------------------------------------------------------
            I("RESET;");
            //TODO: WHY does it only solve with backtrack = no????????????????????????
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Models\';");
            I("RUN lille4.cmd;"); //option solve newton backtrack = no;
            double delta = 0.0001d;  //the numbers are quite large, so 0.01 is strict.
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 161071.7813d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x1" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 161071.7813d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 44425.9141d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x2" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 44425.9141d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 540864.2500d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x3" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 540864.2500d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2000, 1)), 921567.5625d, delta);
            _AssertHelperTwoDoubles((First().GetIVariable("x4" + "!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, 2001, 1)), 921567.5625d, delta);
        }

        [TestMethod]
        public void _Test_ModelQuarterly()
        {
            //-----------------------------------------------------------
            //----------------- testing quarterly model -----------------
            //----------------- also tests data for DJZ-vars ------------
            //-----------------------------------------------------------
            I("RESET;");
            I("OPTION freq q;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\Models\';");            
            I("model lilleq;");
            I("read lilleq;");
            I("time 2000q1 2001q1;");
            I("SIM;");

            double delta = 0.001d;

            _AssertSeries(First(), "y", EFreq.Q, 1999, 4, 100d, delta);
            _AssertSeries(First(), "y", EFreq.Q, 2000, 1, 400d, delta);
            _AssertSeries(First(), "y", EFreq.Q, 2000, 2, 600d, delta);
            _AssertSeries(First(), "y", EFreq.Q, 2000, 3, 733.3333d, delta);
            _AssertSeries(First(), "y", EFreq.Q, 2000, 4, 822.2222d, delta);
            _AssertSeries(First(), "y", EFreq.Q, 2001, 1, 881.4815d, delta);
            _AssertSeries(First(), "y", EFreq.Q, 2001, 2, double.NaN, delta);

            _AssertSeries(First(), "c", EFreq.Q, 1999, 4, 100d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 1, 200d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 2, 400d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 3, 533.3333d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 4, 622.2222d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2001, 1, 681.4815d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2001, 2, double.NaN, delta);

            _AssertSeries(First(), "i", EFreq.Q, 1999, 3, double.NaN, delta);
            _AssertSeries(First(), "i", EFreq.Q, 1999, 4, 100d, delta);
            _AssertSeries(First(), "i", EFreq.Q, 2000, 1, 100d, delta);
            _AssertSeries(First(), "i", EFreq.Q, 2000, 2, 100d, delta);
            _AssertSeries(First(), "i", EFreq.Q, 2000, 3, 100d, delta);
            _AssertSeries(First(), "i", EFreq.Q, 2000, 4, 100d, delta);
            _AssertSeries(First(), "i", EFreq.Q, 2001, 1, 100d, delta);
            _AssertSeries(First(), "i", EFreq.Q, 2001, 2, double.NaN, delta);

            _AssertSeries(First(), "g", EFreq.Q, 1999, 3, double.NaN, delta);
            _AssertSeries(First(), "g", EFreq.Q, 1999, 4, 100d, delta);
            _AssertSeries(First(), "g", EFreq.Q, 2000, 1, 100d, delta);
            _AssertSeries(First(), "g", EFreq.Q, 2000, 2, 100d, delta);
            _AssertSeries(First(), "g", EFreq.Q, 2000, 3, 100d, delta);
            _AssertSeries(First(), "g", EFreq.Q, 2000, 4, 100d, delta);
            _AssertSeries(First(), "g", EFreq.Q, 2001, 1, 100d, delta);
            _AssertSeries(First(), "g", EFreq.Q, 2001, 2, double.NaN, delta);

            _AssertSeries(First(), "dc", EFreq.Q, 1999, 4, double.NaN, delta);
            _AssertSeries(First(), "dc", EFreq.Q, 2000, 1, 0d, delta);
            _AssertSeries(First(), "dc", EFreq.Q, 2000, 2, 0d, delta);
            _AssertSeries(First(), "dc", EFreq.Q, 2000, 3, 0d, delta);
            _AssertSeries(First(), "dc", EFreq.Q, 2000, 4, 0d, delta);
            _AssertSeries(First(), "dc", EFreq.Q, 2001, 1, 0d, delta);
            _AssertSeries(First(), "dc", EFreq.Q, 2001, 2, double.NaN, delta);

            _AssertSeries(First(), "jrc", EFreq.Q, 1999, 4, double.NaN, delta);
            _AssertSeries(First(), "jrc", EFreq.Q, 2000, 1, 0d, delta);
            _AssertSeries(First(), "jrc", EFreq.Q, 2000, 2, 0d, delta);
            _AssertSeries(First(), "jrc", EFreq.Q, 2000, 3, 0d, delta);
            _AssertSeries(First(), "jrc", EFreq.Q, 2000, 4, 0d, delta);
            _AssertSeries(First(), "jrc", EFreq.Q, 2001, 1, 0d, delta);
            _AssertSeries(First(), "jrc", EFreq.Q, 2001, 2, double.NaN, delta);

            _AssertSeries(First(), "zc", EFreq.Q, 1999, 4, double.NaN, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 1, 200d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 2, 400d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 3, 533.3333d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2000, 4, 622.2222d, delta);
            _AssertSeries(First(), "c", EFreq.Q, 2001, 1, 681.4815d, delta);
            _AssertSeries(First(), "zc", EFreq.Q, 2001, 2, double.NaN, delta);

        }

        [TestMethod]
        public void Test__EnsJJUST()
        {
            Assert.Inconclusive(Globals.unitTestIntegrationMessage);
            return;

            //-----------------------------------------------------------
            //----------------- testing JJUST juli ----------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\ENS-fremskrivn\JJUST2012\';");
            I("RUN master;");
            I("SERIES xxxx = qjzdkq+qjzdkn;");

            //string s = "";
            //s += "                   qJsum %E       qjzdkq+qjzdkn %E" + G.NL;
            //s += "2008    431061.2932   -1.86    620495.6000   -2.35" + G.NL;
            //s += "2009    412372.7746   -4.34    591314.5600   -4.70" + G.NL;
            //s += "2010    441003.0777    6.94    624244.2500    5.57" + G.NL;
            //s += "2011    441003.0775    0.00    624244.2243    0.00" + G.NL;
            //s += "2012    441003.0775    0.00    624244.2243    0.00" + G.NL;
            //s += "2013    441003.0775    0.00    624244.2243    0.00" + G.NL;
            //s += "2014    441003.0775    0.00    624244.2243    0.00" + G.NL;
            //s += "2015    441003.0775    0.00    624244.2243    0.00" + G.NL;
            //Assert.IsTrue(Globals.unitTestScreenOutput.ToString().Contains(s));

            UData u;
            u = Data("qjsum", 2008, "a"); Assert.AreEqual(u.w, 431061.2932d, 0.0001d);
            u = Data("qjsum", 2009, "a"); Assert.AreEqual(u.w, 412372.7746d, 0.0001d);
            u = Data("qjsum", 2010, "a"); Assert.AreEqual(u.w, 441003.0777d, 0.0001d);
            u = Data("qjsum", 2011, "a"); Assert.AreEqual(u.w, 441003.0775d, 0.0001d);
            u = Data("qjsum", 2012, "a"); Assert.AreEqual(u.w, 441003.0775d, 0.0001d);
            u = Data("qjsum", 2015, "a"); Assert.AreEqual(u.w, 441003.0775d, 0.0001d);

            u = Data("xxxx", 2008, "a"); Assert.AreEqual(u.w, 620495.6000d, 0.0001d);
            u = Data("xxxx", 2009, "a"); Assert.AreEqual(u.w, 591314.5600d, 0.0001d);
            u = Data("xxxx", 2010, "a"); Assert.AreEqual(u.w, 624244.2500d, 0.0001d);
            u = Data("xxxx", 2011, "a"); Assert.AreEqual(u.w, 624244.2243d, 0.0001d);
            u = Data("xxxx", 2012, "a"); Assert.AreEqual(u.w, 624244.2243d, 0.0001d);
            u = Data("xxxx", 2015, "a"); Assert.AreEqual(u.w, 624244.2243d, 0.0001d);
        }

        [TestMethod]
        public void Test__TranslateExjan15()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\translate\gekko18\exjan15';");
            I("RUN REGRES;");
            for (int i = 2013; i <= 2022; i++)
            {
                UData u = Data("fy", i, "a"); Assert.AreEqual(u.m, 0d, 11d);  //small differences for 2014-2022, not sure why, could be simulation options
            }
        }

        [TestMethod]
        public void _Test_Alias()
        {            
            I("option interface alias = yes;");
            I("#(listfile m) = (('fy', 'c[a]'), ('fe', 'c[b]'));");            
            I("global:#alias = #(listfile m);");
            I("c = series(1); c[a] = 100; c[b] = 200;");
            I("prt fy, fe;");            
        }

        [TestMethod]
        public void Test__Meta()
        {
            I("reset;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\meta';");
            I("model meta;");
            I("time 2000 2001;");
            I("create #all;");
            I("create extra1, extra2;");
            I("series tg = 0.25;");
            I("series extra1 = 2, 3;");
            I("series extra2 = 1/extra1 + 0.1*extra1[-1];");
            I("sim;");
            string stamp2 = Program.GetDateStamp();

            _AssertSeries(First(), "q", 2000, 1d, 0.0001d);
            _AssertSeries(First(), "q", 2001, 1d, 0.0001d);
            _AssertSeries(First(), "pxnk", 2000, 2d, 0.0001d);
            _AssertSeries(First(), "pxnk", 2001, 2d, 0.0001d);

            MetaHelperLabel();
            MetaHelperSourceStamp(stamp2);

            I("HDG 'A databank for unit testing purposes!';");

            I("WRITE meta;");
            I("RESET;");
            I("READ meta;");

            MetaHelperLabel();
            MetaHelperSourceStamp(stamp2);

            Databank w = Program.databanks.GetFirst();
            Assert.AreEqual(w.info1, "A databank for unit testing purposes!");
            //TODO: Assert.AreEqual(w.date, "---todo---");
            Assert.AreEqual(w.readInfo.modelName, "meta.frm");
            Assert.AreEqual(w.readInfo.modelInfo, "Adam Oktober 2012");
            Assert.AreEqual(w.readInfo.modelDate, "05-03-2013 22:54:00");
            Assert.AreEqual(w.readInfo.modelSignature, "ATuOTa263peolAocqEvrPA");
            Assert.AreEqual(w.readInfo.modelHash, "UZp0jFd11z6zvfS3MHPsaA");
            Assert.AreEqual(w.readInfo.modelLastSimPeriod, "2000-2001");
            //Assert.AreEqual(w.readInfo.modelLastSimStamp, "---todo---");
            Assert.AreEqual(w.readInfo.modelLargestLag, "0");
            Assert.AreEqual(w.readInfo.modelLargestLead, "0");

        }
        [TestMethod]
        public void Test__DatabankSerFormat()
        {
            I("RESET;");
            I("READ <ser> " + Globals.ttPath2 + @"\regres\Databanks\test;");
            _AssertSeries(First(), "xx1", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx1", 2001, 1d, sharedDelta);
            _AssertSeries(First(), "xx1", 2002, -2d, sharedDelta);
            _AssertSeries(First(), "xx1", 2003, 1e5d, sharedDelta);
            _AssertSeries(First(), "xx1", 2004, -1.23d, sharedDelta);
            _AssertSeries(First(), "xx1", 2005, double.NaN, sharedDelta);

            _AssertSeries(First(), "xx2", 2000, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx2", 2001, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", 2002, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", 2003, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", 2004, 3d, sharedDelta);
            _AssertSeries(First(), "xx2", 2005, double.NaN, sharedDelta);

            _AssertSeries(First(), "xx3", EFreq.Q, 2000, 4, double.NaN, sharedDelta);
            _AssertSeries(First(), "xx3", EFreq.Q, 2001, 1, 1d, sharedDelta);
            _AssertSeries(First(), "xx3", EFreq.Q, 2001, 2, 2, sharedDelta);
            _AssertSeries(First(), "xx3", EFreq.Q, 2001, 3, 3d, sharedDelta);
            _AssertSeries(First(), "xx3", EFreq.Q, 2001, 4, 4, sharedDelta);
            _AssertSeries(First(), "xx3", EFreq.Q, 2002, 1, double.NaN, sharedDelta);
        }


        [TestMethod]
        public void _Test_Doc()
        {
            I("reset;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\meta';");
            I("import<tsd>meta;");
            I("DOC y label='' source='' stamp='';");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.label, "");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.source, "");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.stamp, "");
            I("DOC y label='a' source='b' stamp='c';");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.label, "a");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.source, "b");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.stamp, "c");

            //Testing quarters
            I("RESET;");
            I("OPTION freq q;");
            I("CREATE x, y;");
            I("LIST #m = x, y;");
            I("DOC {#m} label='a' source='b' stamp='c';");
            Assert.AreEqual((First().GetIVariable("x!q") as Series).meta.label, "a");
            Assert.AreEqual((First().GetIVariable("x!q") as Series).meta.source, "b");
            Assert.AreEqual((First().GetIVariable("x!q") as Series).meta.stamp, "c");
            Assert.AreEqual((First().GetIVariable("y!q") as Series).meta.label, "a");
            Assert.AreEqual((First().GetIVariable("y!q") as Series).meta.source, "b");
            Assert.AreEqual((First().GetIVariable("y!q") as Series).meta.stamp, "c");

            //Test inline label in SERIES
            I("reset;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\meta';");
            I("import<tsd>meta;");
            FAIL("SERIES y 'a' = 5;");
            I("SERIES <label='a' source='b' units='c' stamp='d'> y = 5;");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.label, "a");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.source, "b");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.units, "c");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.stamp, "d");
        }

        [TestMethod]
        public void _Test_MetaTsd()
        {
            I("reset;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\meta';");
            I("import<tsd>meta;");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.label, "label");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.source, "2/yyyy");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.stamp, "12/25/15");
            _AssertSeries(First(), "y", 1999, double.NaN, 0d);
            _AssertSeries(First(), "y", 2000, 1d / 3d, 0.0000001d);
            _AssertSeries(First(), "y", 2001, 1d / 3d, 0.0000001d);
            _AssertSeries(First(), "y", 2002, 1d / 3d, 0.0000001d);
            _AssertSeries(First(), "y", 2003, double.NaN, 0d);
            I("export<tsd>metaTemp;");
            I("reset;");
            I("import<tsd>metaTemp;");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.label, "label");
            Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.source, "2/yyyy");
            if (Globals.UNITTESTFOLLOWUP)
            {
                Assert.AreEqual((First().GetIVariable("y!a") as Series).meta.stamp, "25-12-2015");
            }

            _AssertSeries(First(), "y", 1999, double.NaN, 0d);
            _AssertSeries(First(), "y", 2000, 1d / 3d, 0.0000001d);
            _AssertSeries(First(), "y", 2001, 1d / 3d, 0.0000001d);
            _AssertSeries(First(), "y", 2002, 1d / 3d, 0.0000001d);
            _AssertSeries(First(), "y", 2003, double.NaN, 0d);
        }

        private static void MetaHelperLabel()
        {
            Assert.AreEqual((First().GetIVariable("q" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("pxnk" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("pxnk2" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("tg" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("Dpxnk" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("JRpxnk" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("Zpxnk" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("extra1" + "!a") as Series).meta.label, null);
            Assert.AreEqual((First().GetIVariable("extra2" + "!a") as Series).meta.label, null);
        }

        private static void MetaHelperSourceStamp(string stamp2)
        {
            // endo simul

            Assert.AreEqual((First().GetIVariable("q" + "!a") as Series).meta.source, "2000-2001: SIM meta.frm (hash UZp0jFd11z6zvfS3MHPsaA)");
            Assert.AreEqual((First().GetIVariable("q" + "!a") as Series).meta.stamp, stamp2);  //might fail around midnight!!

            Assert.AreEqual((First().GetIVariable("pxnk" + "!a") as Series).meta.source, "2000-2001: SIM meta.frm (hash UZp0jFd11z6zvfS3MHPsaA)");
            Assert.AreEqual((First().GetIVariable("pxnk" + "!a") as Series).meta.stamp, stamp2);  //might fail around midnight!!

            // endo tablevars

            Assert.AreEqual((First().GetIVariable("pxnk2" + "!a") as Series).meta.source, "2000-2001: SIM meta.frm (hash UZp0jFd11z6zvfS3MHPsaA)");
            Assert.AreEqual((First().GetIVariable("pxnk2" + "!a") as Series).meta.stamp, stamp2);  //might fail around midnight!!

            // true exo

            Assert.AreEqual((First().GetIVariable("tg" + "!a") as Series).meta.source, "2000-2001: series tg = 0.25");  //is detected as GENR type, nothing to do about that
            Assert.AreEqual((First().GetIVariable("tg" + "!a") as Series).meta.stamp, stamp2);  //might fail around midnight!!

            // exo DJZ

            Assert.AreEqual((First().GetIVariable("Dpxnk" + "!a") as Series).meta.source, null);
            Assert.AreEqual((First().GetIVariable("Dpxnk" + "!a") as Series).meta.stamp, null);

            Assert.AreEqual((First().GetIVariable("JRpxnk" + "!a") as Series).meta.source, null);
            Assert.AreEqual((First().GetIVariable("JRpxnk" + "!a") as Series).meta.stamp, null);

            Assert.AreEqual((First().GetIVariable("Zpxnk" + "!a") as Series).meta.source, null);
            Assert.AreEqual((First().GetIVariable("Zpxnk" + "!a") as Series).meta.stamp, null);

            // Created vars

            Assert.AreEqual((First().GetIVariable("extra1" + "!a") as Series).meta.source, "2000-2001: series extra1 = 2, 3");
            Assert.AreEqual((First().GetIVariable("extra1" + "!a") as Series).meta.stamp, stamp2);  //might fail around midnight!!

            Assert.AreEqual((First().GetIVariable("extra2" + "!a") as Series).meta.source, "2000-2001: series extra2 = 1/extra1 + 0.1*extra1[-1]");
            Assert.AreEqual((First().GetIVariable("extra2" + "!a") as Series).meta.stamp, stamp2);  //might fail around midnight!!
        }

        [TestMethod]
        public void Test__TranslateExaug15()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\translate\gekko18\exaug15';");
            I("RUN REGRES;");
            for (int i = 2014; i <= 2023; i++)
            {
                UData u = Data("fy", i, "a"); Assert.AreEqual(u.m, 0d, 1e-8d);  //almost exact reproduction!
            }
        }

        [TestMethod]
        public void Test__TranslateAREMOS1()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\translate\aremos\aremos1';");
            I("sys'del t1." + Globals.extensionCommand + "';");
            I("translate <aremos> t1;");
            I("run t1;");
            _AssertScalarString(First(), "%s", "abc");
            _AssertScalarString(First(), "%n", "bce");
            _AssertScalarDate(First(), "%d1", EFreq.A, 2002, 1);
            _AssertScalarDate(First(), "%d2", EFreq.A, 1995, 1);
            _AssertScalarDate(First(), "%d3", EFreq.Q, 1990, 3);
            _AssertScalarVal(First(), "%v1", 1.2345);
            _AssertScalarVal(First(), "%v2", -1.23e-5);
            _AssertSeries(First(), "bce", 2000, double.NaN, 0d);
            _AssertSeries(First(), "bce", 2001, 5d, 0d);
            _AssertSeries(First(), "bce", 2002, 6d, 0d);
            _AssertSeries(First(), "bce", 2003, 7d, 0d);
            _AssertSeries(First(), "bce", 2004, double.NaN, 0d);
            _AssertScalarVal(First(), "%v3", 1.2345 * 6d);
            _AssertScalarString(First(), "%s2", "value:abc");
            _AssertSeries(First(), "bce2", EFreq.Q, 1990, 2, double.NaN, 0d);
            _AssertSeries(First(), "bce2", EFreq.Q, 1990, 3, 5d, 0d);
            _AssertSeries(First(), "bce2", EFreq.Q, 1990, 4, 6d, 0d);
            _AssertSeries(First(), "bce2", EFreq.Q, 1991, 1, 7d, 0d);
            _AssertSeries(First(), "bce2", EFreq.Q, 1991, 2, double.NaN, 0d);
            _AssertScalarVal(First(), "%v4", 1.2345 * 6d);
            
        }

        [TestMethod]
        public void _Test_HPFilter()
        {
            UData u = null;

            //testing chain index
            //test taken from MATLAB: http://www.mathworks.se/help/symbolic/mupad_ref/stats-hodrickprescottfilter.html
            I("RESET;");
            I("TIME 1996 2004;");
            I("CREATE input;");
            I("SERIES input = data('1242 1353 1142 1255 1417 1312 1440 1422 1470');");
            I("xx = hpfilter(input, 1996, 2004, 10, 0);");
            I("xx2 = input.hpfilter(1996, 2004, 10);");
            I("xx3 = hpfilter(input, 10, 0);");
            I("xx4 = input.hpfilter(10);");
            EqualTimeseries("xx", "xx2", 1996, 2004);
            EqualTimeseries("xx", "xx3", 1996, 2004);
            EqualTimeseries("xx", "xx4", 1996, 2004);
            u = Data("xx", 1995, "a"); Assert.AreEqual(u.w, double.NaN);
            u = Data("xx", 1996, "a"); _AssertHelperTwoDoubles(u.w, 1239.848378d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 1997, "a"); _AssertHelperTwoDoubles(u.w, 1255.015604d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 1998, "a"); _AssertHelperTwoDoubles(u.w, 1270.397993d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 1999, "a"); _AssertHelperTwoDoubles(u.w, 1296.009146d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, "a"); _AssertHelperTwoDoubles(u.w, 1329.022865d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2001, "a"); _AssertHelperTwoDoubles(u.w, 1362.512038d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2002, "a"); _AssertHelperTwoDoubles(u.w, 1398.347268d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2003, "a"); _AssertHelperTwoDoubles(u.w, 1433.347951d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2004, "a"); _AssertHelperTwoDoubles(u.w, 1468.498758d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2005, "a"); Assert.AreEqual(u.w, double.NaN);

            I("RESET;");
            I("OPTION freq m;");
            I("TIME 2000m1 2000m9;");
            I("CREATE input;");
            I("SERIES input = data('1242 1353 1142 1255 1417 1312 1440 1422 1470');");
            I("SERIES xx = hpfilter(input, 2000m1, 2000m9, 10, 0);");
            u = Data("xx", 1999, 12, "m"); Assert.AreEqual(u.w, double.NaN);
            u = Data("xx", 2000, 1, "m"); _AssertHelperTwoDoubles(u.w, 1239.848378d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 2, "m"); _AssertHelperTwoDoubles(u.w, 1255.015604d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 3, "m"); _AssertHelperTwoDoubles(u.w, 1270.397993d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 4, "m"); _AssertHelperTwoDoubles(u.w, 1296.009146d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 5, "m"); _AssertHelperTwoDoubles(u.w, 1329.022865d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 6, "m"); _AssertHelperTwoDoubles(u.w, 1362.512038d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 7, "m"); _AssertHelperTwoDoubles(u.w, 1398.347268d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 8, "m"); _AssertHelperTwoDoubles(u.w, 1433.347951d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 9, "m"); _AssertHelperTwoDoubles(u.w, 1468.498758d, 0.0001d);  //0.01% difference accepted
            u = Data("xx", 2000, 10, "m"); Assert.AreEqual(u.w, double.NaN);
        }

        private static void EqualTimeseries(string s1, string s2, int tStart, int tEnd)
        {
            for (int t = tStart; t <= tEnd; t++)
            {
                UData u = Data(s1, t, "a");
                UData u2 = Data(s2, t, "a");
                Assert.AreEqual(u.w, u2.w);
            }
        }


        [TestMethod]
        public void _Test_PriceIndexFunctions()
        {
            UData u = null;

            //testing chain index
            I("RESET; MODE data;");  //data mode, because we test searching in banks
            I("TIME 90 2002;");
            I("CREATE pibp, pibo, pibh, fibp, fibo, fibh, pib, fib;");
            //from dec09 databank
            I("SERIES <1998 2002> pibp = data(' 0.948133  0.971452  1.000000  1.041416  1.064057');");
            I("SERIES <1998 2002> pibo = data(' 0.933059  0.969186  1.000000  1.040763  1.065663');");
            I("SERIES <1998 2002> pibh = data(' 0.950284  0.988645  1.000000  1.041834  1.065160');");
            I("SERIES <1998 2002> fIbp = data(' 57576.828125  52771.550781  54661.460938  55727.980469  51411.250000');");
            I("SERIES <1998 2002> fIbo = data(' 11918.860352  11847.049805  12981.000000  14474.000000  13855.219727');");
            I("SERIES <1998 2002> fIbh = data(' 53102.210938  55394.871094  61085.609375  55432.808594  55892.488281');");
            I("SERIES <1998 2002> pib = data(' 0.947841  0.979390  1.000000  1.041533  1.064828');");
            I("SERIES <1998 2002> fIb = data(' 122568.000000  119988.000000  128727.000000  125635.000000  121152.000000');");
            I("list #p= pibp, pibo, pibh;");
            I("list #x=fIbp, fIbo, work:fIbh;");
            //I("SERIES <98 2002> (xx_p, xx_x) = laspchain(#p, #x, 2000);");
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO  think about how to indicate time in tuples
            //TODO TODO TODO
            //TODO TODO TODO
            I("OPEN<edit>temp;");
            I("TIME 98 2002;");
            //I("(SERIES xx_p, SERIES xx_x) = laspchain(#p, #x, 2000);");  //these have to be searched for in Work databank, but it is ok since we use data mode.
            I("MAP #m = laspchain(#p, #x, 2000);");  //these have to be searched for in Work databank, but it is ok since we use data mode.
            I("xx_p = #m.p;");
            I("xx_x = #m.q;");
            I("COPY temp:xx_p TO work:xx_p;");
            I("COPY temp:xx_x TO work:xx_x;");
            I("CLOSE temp;");
            I("CREATE dif_p, dif_x;");
            I("SERIES dif_p = pib/xx_p;");
            I("SERIES dif_x = fib/xx_x;");
            for (int i = 1998; i <= 2002; i++)
            {
                u = Data("dif_p", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.0001d);  //0.01% difference accepted (some loss of precision when reading ADAM data)
                u = Data("dif_x", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.0001d);
            }

            // ------------------------------------------------------
            // ------------------------------------------------------
            // ------------------------------------------------------

            I("RESET;");
            I("TIME 98 2002;");
            I("list #pris = pcp, pco, pim, pib, pit, pil, pm, pE ;");
            I("list #mgd = fCp, fCo, fIm, fIb, fIt, fIl, fM, fE ;");            
            I("list #mgd = fCp, fCo, fIm, fIb, fIt, fIl, -fM, fE ;");  //fM is subtracted in fY definition
            I("create fy, py;");
            //from dec09 databank
            I("SERIES <1998 2002> pcp = data(' 0.955412  0.973540  1.000000  1.023497  1.040734');");
            I("SERIES <1998 2002> pco = data(' 0.960245  0.982991  1.000000  1.033200  1.062184')  ;");
            I("SERIES <1998 2002> pim =  data('1.010228  0.999511  1.000000  1.009896  1.023762')  ;");
            I("SERIES <1998 2002> pib =  data('0.947841  0.979390  1.000000  1.041533  1.064828')  ;");
            I("SERIES <1998 2002> pit =  data('0.779661  0.690265  1.000000  1.166667  2.833333')  ;");
            I("SERIES <1998 2002> pil =  data('0.486864  0.754511  1.000000  0.841816  0.802019')  ;");
            I("SERIES <1998 2002> pm =  data('0.937313  0.932782  1.000000  1.015045  0.989444')  ;");
            I("SERIES <1998 2002> pe =  data('0.928665  0.924367  1.000000  1.015529  1.002082')  ;");
            I("SERIES <1998 2002> fCp =  data('617837.187500  615416.000000  616682.187500  617185.312500  626727.312500 ') ;");
            I("SERIES <1998 2002> fCo =  data('310454.000000  317775.000000  325099.000000  332233.000000  339124.000000 ') ;");
            I("SERIES <1998 2002> fIm =  data('122378.296875  124763.000000  134033.000000  133691.000000  138563.500000 ') ;");
            I("SERIES <1998 2002> fIb =  data('122568.000000  119988.000000  128727.000000  125635.000000  121152.000000 ') ;");
            I("SERIES <1998 2002> fIt =  data('118.000000  113.000000  120.000000  -18.000000  -6.000000 ') ;");
            I("SERIES <1998 2002> fIl =  data('22000.000000  -2106.000000  11204.000000  7289.000000  11592.000000 ') ;");
            I("SERIES <1998 2002> fM =  data('448199.312500  463879.406250  524253.187500  534168.875000  574250.625000 ') ;");
            I("SERIES <1998 2002> fE =  data('478868.906250  534380.875000  602351.125000  621238.312500  646970.000000 ') ;");
            I("SERIES <1998 2002> py =  data('0.954835  0.970887  1.000000  1.024961  1.048567 ') ;");
            I("SERIES <1998 2002> fY =  data('1218658.000000  1249862.000000  1293965.000000  1303086.000000  1309156.000000 ') ;");
            //I("SERIES <98 2002> (xx_p, xx_x) = laspchain(#pris, #mgd, 2000);");

            I("TIME 98 2002;");

            I("MAP #m = laspchain(#pris, #mgd, 2000);");  //these have to be searched for in Work databank, but it is ok since we use data mode.
            I("xx_p = #m.p;");
            I("xx_x = #m.q;");                       

            I("CREATE dif_p, dif_x;");
            I("SERIES dif_p = py/xx_p;");
            I("SERIES dif_x = fy/xx_x;");
            for (int i = 1998; i <= 2002; i++)
            {
                u = Data("dif_p", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.00001d);  //0.01% difference accepted (some loss of precision when reading ADAM data)
                u = Data("dif_x", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.00001d);
            }

            // ------------------------------------------------------
            // ------------------------------------------------------
            // ------------------------------------------------------

            //Testing fixed base index
            I("RESET;");
            I("TIME 98 2002;");
            I("list #pris = pcp, pco, pim, pib, pit, pil, pm, pE ;");
            I("list #mgd = fCp, fCo, fIm, fIb, fIt, fIl, fM, fE ;");            
            I("list #mgd = fCp, fCo, fIm, fIb, fIt, fIl, -fM, fE ;");  //fM is subtracted in fY definition
            I("create fy, py;");
            //from jul05 databank
            I("SERIES <1998 2002> pcp = data(' 0.950203  0.969992  1.000000  1.023500  1.040110 ') ;");
            I("SERIES <1998 2002> pco = data(' 0.950436  0.975481  1.000000  1.033200  1.060210 ') ;");
            I("SERIES <1998 2002> pim = data(' 1.001860  1.002600  1.000010  1.009900  1.022520 ') ;");
            I("SERIES <1998 2002> pib = data(' 0.944726  0.980369  1.000010  1.041530  1.063590 ') ;");
            I("SERIES <1998 2002> pit = data(' 0.828829  1.040000  1.000000  1.199870  1.795900 ') ;");
            I("SERIES <1998 2002> pil = data(' 0.706035  -1.297190  1.000000  0.841789  0.849201 ') ;");
            I("SERIES <1998 2002> pm = data(' 0.925393  0.929556  1.000000  1.015050  0.984646 ') ;");
            I("SERIES <1998 2002> pe =  data('0.933015  0.923524  1.000000  1.015530  0.997510 ') ;");
            I("SERIES <1998 2002> fCp = data(' 621224.000000  617668.000000  616682.000000  617185.000000  627102.000000 ') ;");
            I("SERIES <1998 2002> fCo = data(' 313658.000000  320221.000000  325099.000000  332233.000000  339755.000000 ') ;");
            I("SERIES <1998 2002> fIm = data(' 123407.000000  124370.000000  134033.000000  133691.000000  138730.000000 ') ;");
            I("SERIES <1998 2002> fIb = data(' 122971.000000  119872.000000  128727.000000  125635.000000  121293.000000 ') ;");
            I("SERIES <1998 2002> fIt = data(' 111.000000  75.000000  120.000000  -17.611000  -9.324000 ') ;");
            I("SERIES <1998 2002> fIl = data(' 15171.300000  1224.990000  11204.500000  7288.880000  10947.500000 ') ;");
            I("SERIES <1998 2002> fM = data(' 453972.000000  465489.000000  524253.000000  534168.000000  577049.000000 ') ;");
            I("SERIES <1998 2002> fE = data(' 476637.000000  534869.000000  602351.000000  621238.000000  649935.000000 ') ;");
            I("SERIES <1998 2002> fY = data(' 1219210.000000  1252810.000000  1293960.000000  1303080.000000  1310700.000000 ') ;");
            I("SERIES <1998 2002> py = data(' 0.954403  0.968600  1.000000  1.024960  1.047330');");

            //I("SERIES <98 2002> (xx_p, xx_x) = laspfixed(#pris, #mgd, 2000);");

            I("MAP #m = laspfixed(#pris, #mgd, 2000);");  //these have to be searched for in Work databank, but it is ok since we use data mode.
            I("xx_p = #m.p;");
            I("xx_x = #m.q;");

            I("TIME 98 2002;");
            //I("(series xx_p, series xx_x) = laspfixed(#pris, #mgd, 2000);");
            I("CREATE dif_p, dif_x;");
            I("SERIES dif_p = py/xx_p;");
            I("SERIES dif_x = fy/xx_x;");
            for (int i = 1998; i <= 2002; i++)
            {
                u = Data("dif_p", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.00001d);  //0.01% difference accepted (some loss of precision when reading ADAM data)
                u = Data("dif_x", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.00001d);
            }

            // ----------- not using explicit map
                        
            I("zz_x = laspfixed(#pris, #mgd, 2000).q;");  //direct pick out of .q field.
            I("SERIES dif_zz = fy/zz_x;");
            for (int i = 1998; i <= 2002; i++)
            {                
                u = Data("dif_zz", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.00001d);
            }

            // ----------- direct use of strings

            I("ww_x = laspfixed(('pcp', 'pco', 'pim', 'pib', 'pit', 'pil', 'pm', 'pE'), ('fCp', 'fCo', 'fIm', 'fIb', 'fIt', 'fIl', '-fM', 'fE'), 2000).q;");  //direct pick out of .q field.
            I("SERIES dif_ww = fy/ww_x;");
            for (int i = 1998; i <= 2002; i++)
            {
                u = Data("dif_ww", i, "a"); _AssertHelperTwoDoubles(u.w, 1.0d, 0.00001d);
            }           

        }

        [TestMethod]
        public void Test__ForwardLookingStackedTimeMiniExcel()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward';");
            I("RUN st1;");  //See st.xlsx
            double epsilon = 0.00035d;
            UData u = null;
            double[] xx = new double[] { 12.0000, 10.4402, 9.1965, 12.8125, 12.6469, 11.7909, 15.4698, 16.2052, 12.5011, 13.6092, 11.7736, 15.7687, 13.2121, 14.5027, 17.0762, 17.3749, 19.6337, 20.6965, 19.4992, 19.1998, 16.0000, 15.0000 };
            for (int i = 0; i < 22; i++)
            {
                u = Data("y", 2000 + i, "a"); _AssertHelperTwoDoubles(u.w, xx[i], epsilon);
            }

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward';");
            I("RUN st2a;"); //See st.xlsx
            epsilon = 0.0001d;
            u = Data("y", 2000, "a"); _AssertHelperTwoDoubles(u.w, 12d, epsilon);
            u = Data("y", 2001, "a"); _AssertHelperTwoDoubles(u.w, 8.1809d, epsilon);
            u = Data("y", 2002, "a"); _AssertHelperTwoDoubles(u.w, 4.9362d, epsilon);
            u = Data("y", 2003, "a"); _AssertHelperTwoDoubles(u.w, 3d, epsilon);

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward';");
            I("RUN st2b;"); //See st.xlsx
            epsilon = 0.0001d;
            u = Data("y", 2000, "a"); _AssertHelperTwoDoubles(u.w, 12d, epsilon);
            u = Data("y", 2001, "a"); _AssertHelperTwoDoubles(u.w, 8.1809d, epsilon);
            u = Data("y", 2002, "a"); _AssertHelperTwoDoubles(u.w, 6.7512d, epsilon);
            u = Data("y", 2003, "a"); _AssertHelperTwoDoubles(u.w, 9.0502d, epsilon);
            u = Data("y", 2004, "a"); _AssertHelperTwoDoubles(u.w, 4d, epsilon);

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward';");
            I("RUN st2c;"); //See st.xlsx
            epsilon = 0.0001d;
            u = Data("y", 2000, "a"); _AssertHelperTwoDoubles(u.w, 12d, epsilon);
            u = Data("y", 2001, "a"); _AssertHelperTwoDoubles(u.w, 8.1809d, epsilon);
            u = Data("y", 2002, "a"); _AssertHelperTwoDoubles(u.w, 6.7512d, epsilon);
            u = Data("y", 2003, "a"); _AssertHelperTwoDoubles(u.w, 10.6173d, epsilon);
            u = Data("y", 2004, "a"); _AssertHelperTwoDoubles(u.w, 9.2235d, epsilon);
            u = Data("y", 2005, "a"); _AssertHelperTwoDoubles(u.w, 5d, epsilon);

        }

        [TestMethod]
        public void Test__ForwardLookingStackedTimeADAM()
        {
            Assert.Inconclusive(Globals.unitTestIntegrationMessage);
            return;
            //Can probably be deleted, the model is found elsewhere
            UData u = null;
            double e = 0.0001d;
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward';");
            I("RUN re.cmd;");
            u = Data("fy", 2014, "a"); Assert.AreEqual(u.q, 0.0016d, e);
            u = Data("fy", 2015, "a"); Assert.AreEqual(u.q, 0.0064d, e);
            u = Data("fy", 2016, "a"); Assert.AreEqual(u.q, 0.0086d, e);
            u = Data("fy", 2017, "a"); Assert.AreEqual(u.q, 0.0134d, e);
            u = Data("fy", 2018, "a"); Assert.AreEqual(u.q, 0.0158d, e);
            u = Data("fy", 2019, "a"); Assert.AreEqual(u.q, 0.0170d, e);
            u = Data("fy", 2020, "a"); Assert.AreEqual(u.q, 0.0734d, e);
            u = Data("fy", 2021, "a"); Assert.AreEqual(u.q, 0.0798d, e);
            u = Data("fy", 2022, "a"); Assert.AreEqual(u.q, 0.0717d, e);
            u = Data("fy", 2023, "a"); Assert.AreEqual(u.q, 0.0651d, e);
            u = Data("fy", 2024, "a"); Assert.AreEqual(u.q, 0.0613d, e);
            u = Data("fy", 2025, "a"); Assert.AreEqual(u.q, 0.0575d, e);
            u = Data("fy", 2026, "a"); Assert.AreEqual(u.q, 0.0542d, e);
            u = Data("fy", 2027, "a"); Assert.AreEqual(u.q, 0.0499d, e);
            u = Data("fy", 2028, "a"); Assert.AreEqual(u.q, 0.0456d, e);
            u = Data("fy", 2029, "a"); Assert.AreEqual(u.q, 0.0418d, e);
            u = Data("fy", 2030, "a"); Assert.AreEqual(u.q, 0.0376d, e);
            u = Data("fy", 2031, "a"); Assert.AreEqual(u.q, 0.0362d, e);
            u = Data("fy", 2032, "a"); Assert.AreEqual(u.q, 0.0344d, e);
            u = Data("fy", 2033, "a"); Assert.AreEqual(u.q, 0.0318d, e);
            u = Data("fy", 2034, "a"); Assert.AreEqual(u.q, 0.0289d, e);
            u = Data("fy", 2035, "a"); Assert.AreEqual(u.q, 0.0284d, e);
            u = Data("fy", 2036, "a"); Assert.AreEqual(u.q, 0.0266d, e);
            u = Data("fy", 2037, "a"); Assert.AreEqual(u.q, 0.0256d, e);
            u = Data("fy", 2038, "a"); Assert.AreEqual(u.q, 0.0243d, e);
            u = Data("fy", 2039, "a"); Assert.AreEqual(u.q, 0.0240d, e);
            u = Data("fy", 2040, "a"); Assert.AreEqual(u.q, 0.0226d, e);
            u = Data("fy", 2041, "a"); Assert.AreEqual(u.q, 0.0223d, e);
            u = Data("fy", 2042, "a"); Assert.AreEqual(u.q, 0.0220d, e);
            u = Data("fy", 2043, "a"); Assert.AreEqual(u.q, 0.0217d, e);
            u = Data("fy", 2044, "a"); Assert.AreEqual(u.q, 0.0209d, e);
            u = Data("fy", 2045, "a"); Assert.AreEqual(u.q, 0.0220d, e);
            u = Data("fy", 2046, "a"); Assert.AreEqual(u.q, 0.0219d, e);
            u = Data("fy", 2047, "a"); Assert.AreEqual(u.q, 0.0211d, e);
            u = Data("fy", 2048, "a"); Assert.AreEqual(u.q, 0.0209d, e);
            u = Data("fy", 2049, "a"); Assert.AreEqual(u.q, 0.0203d, e);
            u = Data("fy", 2050, "a"); Assert.AreEqual(u.q, 0.0204d, e);
            u = Data("fy", 2051, "a"); Assert.AreEqual(u.q, 0.0214d, e);
            u = Data("fy", 2052, "a"); Assert.AreEqual(u.q, 0.0209d, e);
            u = Data("fy", 2053, "a"); Assert.AreEqual(u.q, 0.0203d, e);
            u = Data("fy", 2054, "a"); Assert.AreEqual(u.q, 0.0194d, e);
            u = Data("fy", 2055, "a"); Assert.AreEqual(u.q, 0.0186d, e);
            u = Data("fy", 2056, "a"); Assert.AreEqual(u.q, 0.0174d, e);
            u = Data("fy", 2057, "a"); Assert.AreEqual(u.q, 0.0167d, e);
            u = Data("fy", 2058, "a"); Assert.AreEqual(u.q, 0.0157d, e);
            u = Data("fy", 2059, "a"); Assert.AreEqual(u.q, 0.0162d, e);
            u = Data("fy", 2060, "a"); Assert.AreEqual(u.q, 0.0150d, e);
        }

        [TestMethod]
        public void Test__ForwardLookingStackedTimeSmall()
        {
            UData u = null;
            double e = 0.0002d;

            //!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!! activate again!
            //!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!
            if (Globals.UNITTESTFOLLOWUP)
            {
                //This tests a manual rolling out of the model, with y__2000, y__2001, ..., y__2031 etc. solve
                //for an artificial period (2006).
                //The result is the same as when using Fair-Taylor.
                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
                I("RUN re1x." + Globals.extensionCommand + ";");
                ForwardLookingHelperAssert(u, e);

                //This is a similar model, but with both y(-1), y and y(+1)
                //The result is the same as when using Fair-Taylor.
                e = 0.0001d;
                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
                I("RUN re2x." + Globals.extensionCommand + ";");
                ForwardLookingHelperAssert2(u, e);

                //This is real stacked time
                e = 0.0001d;
                I("RESET;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
                I("RUN re2y." + Globals.extensionCommand + ";");
                ForwardLookingHelperAssert2(u, e);
            }

            //no fair-taylor loop
            e = 0.0001d;
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("RUN re3y;");
            ForwardLookingHelperAssert3(u, e);

            //has to do fair-taylor loop, 2 periods, starting with 2000 and 2001
            //note that terminal = exo
            e = 0.0005d;
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("RUN re3y_horizon2;");
            ForwardLookingHelperAssert3(u, e);

            //has to do fair-taylor loop, 3 periods, 2000, 2001 and 2002
            //note that terminal = exo
            e = 0.0006d;
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("RUN re3y_horizon1;");
            ForwardLookingHelperAssert3(u, e);

        }

        [TestMethod]
        public void _Test_RAMLargeAware()
        {
            //Tests the version deployed
            //
            // rem c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\bin\editbin.exe  /LARGEADDRESSAWARE Gekko.exe > zzz
            // call editbin.exe  /LARGEADDRESSAWARE Gekko.exe > zzz, se xx.bat i c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\
            //
            Assert.IsTrue(Program.IsLargeAware(@"c:\Program Files (x86)\Gekko\gekko.exe"));
        }

        [TestMethod]
        public void Test__ForwardLookingExhaustive()
        {
            //TODO: It could be interesting to analyze number of simulations used (or time), and the
            //      precision of results.

            // ------------------------------------------------
            //   y = 0.1 y[-1] + 0.2 y + 0.3 y[+1] + 100
            //
            //   terminal = EXO
            //
            //   y[2000] = y[2004] = 100, else missings (j=0)
            //   or full of 100's in all places (j=1)
            //
            //   SIM<2001 2003>;
            //
            //   y[2001] = 221.551472722393 (true values)
            //   y[2002] = 224.137670064322
            //   y[2003] = 190.517067464932
            //
            // ------------------------------------------------

            double e = 0.01;
            for (int j = 0; j < 2; j++)  //databank full of M or with some values
            {
                for (int k = 0; k < 2; k++)  //exo, const (growth not working at the moment)
                {
                    for (int i = 0; i < 4; i++)  //nfair-newton, nfair-gauss, fair-newton, fair-gauss
                    {
                        I("RESET;");
                        I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward\exhaustive';");
                        I("model m1;");
                        I("create #all;");

                        if (k == 0)
                        {
                            I("option solve forward terminal exo;");
                        }
                        else if (k == 1)
                        {
                            I("option solve forward terminal const;");
                        }
                        //else if (k == 2)
                        //{
                        //    I("option solve forward terminal growth;");
                        //}

                        if (i == 0)
                        {
                            I("option solve forward method nfair;");
                            I("option solve method newton;");
                        }
                        else if (i == 1)
                        {
                            I("option solve forward method nfair;");
                            I("option solve method gauss;");
                        }
                        else if (i == 2)
                        {
                            I("option solve forward method fair;");
                            I("option solve method newton;");
                        }
                        else if (i == 3)
                        {
                            I("option solve forward method fair;");
                            I("option solve method gauss;");
                        }
                        I("SERIES<2000 2000> y = 100;");
                        I("SERIES<2004 2004> y = 100;");
                        if (j == 1)
                        {
                            I("SERIES<2001 2003> y = 100;");  //initilizing, should not alter anything
                        }
                        I("sim<2001 2003>;");
                        if (k == 0 && i <= 1) Assert.AreEqual(Globals.simCounter, 6);  //exo nfair
                        if (k == 0 && i >= 2) Assert.AreEqual(Globals.simCounter, 8);  //exo fair
                        if (k == 1 && i <= 1) Assert.AreEqual(Globals.simCounter, 6);  //const nfair
                        if (k == 1 && i >= 2) Assert.AreEqual(Globals.simCounter, 8);  //const fair
                        UData u = null;
                        if (k == 0)
                        {
                            u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 221.551472722393d, e);  //true values
                            u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 224.137670064322d, e);
                            u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 190.517067464932d, e);  //2004 value is 100
                        }
                        else if (k == 1)
                        {
                            u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 230.2491808d, e);  //true values
                            u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 247.3307572d, e);
                            u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 249.465867d, e);  //2004 value is 249.465867
                        }
                        //else if (k == 2)
                        //{
                        //    u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 230.5964965d, e);  //true values
                        //    u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 248.2570415d, e);
                        //    u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 251.8205101d, e);  //2004 value is 255.4351287
                        //}

                    }
                }
            }

            // ------------------------------------------------
            //   Here we do stacked time
            //   Stacked time is quite limited, must be EXO, data must be initialized, and we must use INIT no.
            //
            //   y = 0.1 y[-1] + 0.2 y + 0.3 y[+1] + 100
            //
            //   terminal = EXO
            //
            //   full of 100's in all places (j=1)
            //
            //   SIM<2001 2003>;
            //
            //   y[2001] = 221.551472722393 (true values)
            //   y[2002] = 224.137670064322
            //   y[2003] = 190.517067464932
            //
            // ------------------------------------------------

            if (Globals.UNITTESTFOLLOWUP)
            {
                if (true)
                {
                    //TODO #098437523985 this will issue a warning
                    I("RESET;");
                    I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\forward\exhaustive';");
                    I("model m1;");
                    I("create #all;");
                    I("option solve forward terminal exo;");
                    I("option solve data init no;");
                    I("option solve forward method stacked;");
                    I("option solve forward stacked horizon 3;");
                    I("option solve method newton;");
                    I("SERIES<2001 2003> y = 100;");
                    I("SERIES<2000 2000> y = 100;");
                    I("SERIES<2004 2004> y = 100;");
                    I("sim<2001 2003>;");
                    e = 0.0003;
                    UData u = null;
                    u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 221.551472722393d, e);  //true values
                    u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 224.137670064322d, e);
                    u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 190.517067464932d, e);
                }
            }

            // ------------------------------------------------
            //   Full ADAM
            //
            //   terminal = EXO. Med testsim tjek er hw-afvigelse mindre end 1 absolut.
            //
            //                 hw         %
            //2014   7767280.3680      0.03
            //2015   7857380.6000      1.16
            //2016   7925375.7853      0.87
            //2017   7990811.9406      0.83
            //2018   8072091.5696      1.02
            //2019   8167623.6743      1.18
            //2020   8275127.1701      1.32
            //2021   8391581.8653      1.41
            //2022   8515610.1329      1.48
            //2023   8645500.1097      1.53
            //2024   8779598.6256      1.55
            //2025   8916835.1413      1.56
            //2026   9056693.6021      1.57
            // ------------------------------------------------

            e = 100d;
            for (int k = 0; k < 1; k++)   //TODO: for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    I("RESET;");
                    I("model jul13re;");
                    I("CLEAR; IMPORT <pcim> lang100; CLONE;");
                    I("time 2025 2025;");
                    I("SERIES hw = (Ydl_hc/pcpuxh)/(1-1.015/((1+0.015)*(1+0.1)));");
                    I("FOR date %t = 2024 to 2010 by -1; time %t %t; SERIES hw = Ydl_hc/pcpuxh+hw[+1]/((1+0.015)*(1+0.1)); END;");
                    I("time 2010 2025;");
                    I("SERIES pihw = iwbz*hw;");
                    I("SERIES <2014 2025> dcpuxhw  = 1;");
                    I("SERIES <2014 2025> zcpuxhw  % 3.53;");
                    I("time 2014 2025;");
                    if (k == 0)
                    {
                        I("option solve forward terminal exo;");
                        I("SERIES<2026 2026> hw = hw[-1];");
                    }
                    else if (k == 1)
                    {
                        I("option solve forward terminal const;");
                    }
                    else if (k == 2)
                    {
                        I("option solve forward terminal growth;");
                    }
                    if (i == 0)
                    {
                        I("option solve forward method nfair;");
                        I("option solve method newton;");
                    }
                    else if (i == 1)
                    {
                        I("option solve forward method nfair;");
                        I("option solve method gauss;");
                    }
                    else if (i == 2)
                    {
                        I("option solve forward method fair;");
                        I("option solve method newton;");
                    }
                    else if (i == 3)
                    {
                        I("option solve forward method fair;");
                        I("option solve method gauss;");
                    }
                    I("sim;");
                    I("OPTION solve data init = no;");
                    I("SERIES dcpuxhw = 0;");
                    I("sim;");
                    I("CLONE;");
                    I("SERIES<2020 2025>dfvmo = 1;");
                    I("SERIES<2020 2025>zfvmo * 1.01;");
                    I("time 2014 2025;");
                    I("sim;");

                    UData u = null;
                    if (k == 0)
                    {
                        u = Data("hw", 2014, "a"); Assert.AreEqual(u.w, 7767280.3680d, e);
                        u = Data("hw", 2015, "a"); Assert.AreEqual(u.w, 7857380.6000d, e);
                        u = Data("hw", 2016, "a"); Assert.AreEqual(u.w, 7925375.7853d, e);
                        u = Data("hw", 2017, "a"); Assert.AreEqual(u.w, 7990811.9406d, e);
                        u = Data("hw", 2018, "a"); Assert.AreEqual(u.w, 8072091.5696d, e);
                        u = Data("hw", 2019, "a"); Assert.AreEqual(u.w, 8167623.6743d, e);
                        u = Data("hw", 2020, "a"); Assert.AreEqual(u.w, 8275127.1701d, e);
                        u = Data("hw", 2021, "a"); Assert.AreEqual(u.w, 8391581.8653d, e);
                        u = Data("hw", 2022, "a"); Assert.AreEqual(u.w, 8515610.1329d, e);
                        u = Data("hw", 2023, "a"); Assert.AreEqual(u.w, 8645500.1097d, e);
                        u = Data("hw", 2024, "a"); Assert.AreEqual(u.w, 8779598.6256d, e);
                        u = Data("hw", 2025, "a"); Assert.AreEqual(u.w, 8916835.1413d, e);  //2026 value is 9056693.6021 (1.57%)
                    }
                    else if (k == 1)
                    {
                        //TODO
                    }
                    else if (k == 2)
                    {
                        //TODO
                    }
                }
            }
        }

        [TestMethod]
        public void Test__ForwardLookingSmall()
        {
            UData u = null;
            double e = 0.01d;

            List<double> dd = new List<double>() { 1.0d, 0.5d };  //other damping awaits conv.crits etc. (0.7 yields slightly different results)
            foreach(double d in dd)
            {
                //-----------------------------
                ForwardLookingHelper();  //sets up model and data from 1999-2030, sim period is 2000-2030
                I("OPTION solve forward fair damp = " + (1 - d) + ";");
                I("OPTION solve forward method = none;");
                I("SERIES <2031 2031> y = 800;");
                I("SIM<2000 2030>;");
                //these results seem ok, tested afterwards with testsim() and a model with a cheat variable ylead instead of y(+1)
                //ylead for testing is then set to 800 in 2030 (else 1000)
                //when repeated, this should converge to real result by the way (with terminal exo).
                for (int t = 2000; t <= 2019; t++)
                {
                    u = Data("y", t, "a"); Assert.AreEqual(u.w, 1000.0000d, e);
                }
                u = Data("y", 2020, "a"); Assert.AreEqual(u.w, 1166.6666d, e);
                u = Data("y", 2021, "a"); Assert.AreEqual(u.w, 1166.6666d, e);
                for (int t = 2022; t <= 2029; t++)
                {
                    u = Data("y", t, "a"); Assert.AreEqual(u.w, 1000.0000d, e);
                }
                u = Data("y", 2030, "a"); Assert.AreEqual(u.w, 866.6667d, e);
                //-----------------------------
                ForwardLookingHelper();  //sets up model and data from 1999-2030, sim period is 2000-2030
                I("OPTION solve forward fair damp = " + (1 - d) + ";");
                //no setting of y in 2031
                I("SIM<2000 2030>;");
                //these results seem ok, tested afterwards with testsim(). This is ok for 2000-2029, and for 2030 there is a
                //difference. This is because "OPTION solve forward terminal const" is implicit, and since y is solved to
                //1000 for 2030, this is used as y(+1) in 2030, even though the databank says y=800 in 2030.
                u = ForwardLookingHelperAssert(u, e);
                //-----------------------------
                ForwardLookingHelper();  //sets up model and data from 1999-2030, sim period is 2000-2030
                I("OPTION solve forward fair damp = " + (1 - d) + ";");
                I("OPTION solve forward terminal exo;");
                //no setting of y in 2031
                FAIL("SIM<2000 2030>;");
                //-----------------------------
                ForwardLookingHelper();  //sets up model and data from 1999-2030, sim period is 2000-2030
                I("OPTION solve forward fair damp = " + (1 - d) + ";");
                I("OPTION solve forward terminal exo;");
                I("SERIES <2031 2031> y = 800;");
                I("SIM<2000 2030>;");
                //these results seem ok, tested afterwards with testsim() where there are no differences.
                //different from above because of y[2031] difference.
                ForwardLookingHelperAssertExo(u, e);  //asserts
            }
            //-----------------------------
            ForwardLookingHelper();  //sets up model and data from 1999-2030, sim period is 2000-2030
            I("OPTION solve forward method = none;");
            I("SERIES <2031 2031> y = 800;");
            for (int i = 0; i < 35; i++)  //will not pass with 30 SIMs
            {
                I("SIM<2000 2030>;");
            }
            //Should give same result as "terminal exo" with repeated sims
            ForwardLookingHelperAssertExo(u, e);  //asserts
            //-----------------------------
            ForwardLookingHelper();  //sets up model and data from 1999-2030, sim period is 2000-2030
            I("OPTION solve forward method = none;");
            FAIL("SIM<2000 2030>;");

            // --------------------------------------------------

            I("RESET;");
            I("RUN re2;");
            ForwardLookingHelperAssert2(u, 0.001d);  //this solution is a bit less precise than the 'true' newton solution.

        }

        private static UData ForwardLookingHelperAssert3(UData u, double e)
        {
            u = Data("y", 2000, "a"); Assert.AreEqual(u.w, 200.0455d, e);
            u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 200.2273d, e);
            u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 201.0682d, e);
            return u;
        }

        private static UData ForwardLookingHelperAssert2(UData u, double e)
        {
            u = Data("y", 2000, "a"); Assert.AreEqual(u.w, 1753.4028d, e);
            u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 1805.1042d, e);
            u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 1694.3056d, e);
            u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 1565.1823d, e);
            u = Data("y", 2004, "a"); Assert.AreEqual(u.w, 1424.1970d, e);
            return u;
        }

        private static UData ForwardLookingHelperAssert(UData u, double e)
        {
            u = Data("y", 2000, "a"); Assert.AreEqual(u.w, 1000.0835d, e);
            u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 1000.1253d, e);
            u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 1000.1879d, e);
            u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 1000.2819d, e);
            u = Data("y", 2004, "a"); Assert.AreEqual(u.w, 1000.4228d, e);
            u = Data("y", 2005, "a"); Assert.AreEqual(u.w, 1000.6343d, e);
            u = Data("y", 2006, "a"); Assert.AreEqual(u.w, 1000.9514d, e);
            u = Data("y", 2007, "a"); Assert.AreEqual(u.w, 1001.4271d, e);
            u = Data("y", 2008, "a"); Assert.AreEqual(u.w, 1002.1408d, e);
            u = Data("y", 2009, "a"); Assert.AreEqual(u.w, 1003.2113d, e);
            u = Data("y", 2010, "a"); Assert.AreEqual(u.w, 1004.8170d, e);
            u = Data("y", 2011, "a"); Assert.AreEqual(u.w, 1007.2255d, e);
            u = Data("y", 2012, "a"); Assert.AreEqual(u.w, 1010.8383d, e);
            u = Data("y", 2013, "a"); Assert.AreEqual(u.w, 1016.2576d, e);
            u = Data("y", 2014, "a"); Assert.AreEqual(u.w, 1024.3864d, e);
            u = Data("y", 2015, "a"); Assert.AreEqual(u.w, 1036.5797d, e);
            u = Data("y", 2016, "a"); Assert.AreEqual(u.w, 1054.8696d, e);
            u = Data("y", 2017, "a"); Assert.AreEqual(u.w, 1082.3045d, e);
            u = Data("y", 2018, "a"); Assert.AreEqual(u.w, 1123.4568d, e);
            u = Data("y", 2019, "a"); Assert.AreEqual(u.w, 1185.1852d, e);
            u = Data("y", 2020, "a"); Assert.AreEqual(u.w, 1277.7779d, e);
            u = Data("y", 2021, "a"); Assert.AreEqual(u.w, 1166.6667d, e);
            for (int t = 2022; t <= 2030; t++)
            {
                u = Data("y", t, "a"); Assert.AreEqual(u.w, 1000.0000d, e);
            }
            return u;
        }

        private static void ForwardLookingHelperAssertExo(UData u, double e)
        {
            u = Data("y", 2000, "a"); Assert.AreEqual(u.w, 1000.0828d, e);
            u = Data("y", 2001, "a"); Assert.AreEqual(u.w, 1000.1242d, e);
            u = Data("y", 2002, "a"); Assert.AreEqual(u.w, 1000.1863d, e);
            u = Data("y", 2003, "a"); Assert.AreEqual(u.w, 1000.2795d, e);
            u = Data("y", 2004, "a"); Assert.AreEqual(u.w, 1000.4193d, e);
            u = Data("y", 2005, "a"); Assert.AreEqual(u.w, 1000.6290d, e);
            u = Data("y", 2006, "a"); Assert.AreEqual(u.w, 1000.9435d, e);
            u = Data("y", 2007, "a"); Assert.AreEqual(u.w, 1001.4153d, e);
            u = Data("y", 2008, "a"); Assert.AreEqual(u.w, 1002.1230d, e);
            u = Data("y", 2009, "a"); Assert.AreEqual(u.w, 1003.1845d, e);
            u = Data("y", 2010, "a"); Assert.AreEqual(u.w, 1004.7769d, e);
            u = Data("y", 2011, "a"); Assert.AreEqual(u.w, 1007.1654d, e);
            u = Data("y", 2012, "a"); Assert.AreEqual(u.w, 1010.7481d, e);
            u = Data("y", 2013, "a"); Assert.AreEqual(u.w, 1016.1222d, e);
            u = Data("y", 2014, "a"); Assert.AreEqual(u.w, 1024.1834d, e);
            u = Data("y", 2015, "a"); Assert.AreEqual(u.w, 1036.2752d, e);
            u = Data("y", 2016, "a"); Assert.AreEqual(u.w, 1054.4129d, e);
            u = Data("y", 2017, "a"); Assert.AreEqual(u.w, 1081.6194d, e);
            u = Data("y", 2018, "a"); Assert.AreEqual(u.w, 1122.4291d, e);
            u = Data("y", 2019, "a"); Assert.AreEqual(u.w, 1183.6438d, e);
            u = Data("y", 2020, "a"); Assert.AreEqual(u.w, 1275.4657d, e);
            u = Data("y", 2021, "a"); Assert.AreEqual(u.w, 1163.1985d, e);
            u = Data("y", 2022, "a"); Assert.AreEqual(u.w, 994.7977d, e);
            u = Data("y", 2023, "a"); Assert.AreEqual(u.w, 992.1964d, e);
            u = Data("y", 2024, "a"); Assert.AreEqual(u.w, 988.2946d, e);
            u = Data("y", 2025, "a"); Assert.AreEqual(u.w, 982.4418d, e);
            u = Data("y", 2026, "a"); Assert.AreEqual(u.w, 973.6627d, e);
            u = Data("y", 2027, "a"); Assert.AreEqual(u.w, 960.4939d, e);
            u = Data("y", 2028, "a"); Assert.AreEqual(u.w, 940.7408d, e);
            u = Data("y", 2029, "a"); Assert.AreEqual(u.w, 911.1112d, e);
            u = Data("y", 2030, "a"); Assert.AreEqual(u.w, 866.6667d, e);
        }

        private static void ForwardLookingHelper()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");
            I("MODEL re1;");
            I("TIME 1999 2030;");
            I("CREATE y; SERIES y=1000;");
            I("CREATE c; SERIES c=800;");
            I("CREATE i; SERIES i=100;");
            I("CREATE g; SERIES g=100;");
            I("SERIES <2020 2021> g = 200;");
        }

        [TestMethod]
        public void _Test_ModelJul05()
        {
            //-----------------------------------------------------------
            //----------------- testing jul05 ---------------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("RUN jul05.gcm;");
            double[] limits = new double[3];
            limits[0] = 0.001d; //%
            limits[1] = 0.001d; //%
            limits[2] = 30d; //abs
            CheckDatabankSample(limits, 2000, 2079, true);
            I("OPTION solve method newton;");
            I("SIM<2006 2079>;");
            CheckDatabankSample(limits, 2000, 2079, true);
        }

        [TestMethod]
        public void _Test_ModelApr07()
        {
            //-----------------------------------------------------------
            //----------------- testing apr07 ---------------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("RUN apr07.gcm;");
            double[] limits = new double[3];
            limits[0] = 0.001d; //%
            limits[1] = 0.001d; //%
            limits[2] = 68d; //abs    (augmented from 61 to 65 after default reorder=no, augmented to 68 after NFT must have changed something a little bit)
            CheckDatabankSample(limits, 2000, 2079, true);
            I("OPTION solve method newton;");
            I("SIM <2009 2079>;");
            CheckDatabankSample(limits, 2000, 2079, true);
        }

        [TestMethod]
        public void _Test_ModelApr08()
        {
            //-----------------------------------------------------------
            //----------------- testing apr08 ---------------------------
            //-----------------------------------------------------------

            //TODO: why do we have to set reorder=no to get the model running??
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models';");  //needs "'" since it contains a "-"
            I("RUN apr08.gcm;");
            double[] limits = new double[3];
            limits[0] = 0.001d; //%
            limits[1] = 0.002d; //%
            limits[2] = 35d; //abs
            CheckDatabankSample(limits, 2000, 2047, true);
            if (false)
            {
                //seems newton does not converge
                I("OPTION solve method newton;");
                I("SIM<2008 2047>;");
                CheckDatabankSample(limits, 2000, 2079, true);
            }
        }

        [TestMethod]
        public void _Test_Return()
        {
            //-----------------------------------------------------------
            //----------------- testing RETURN (without arguments)
            //-----------------------------------------------------------
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\HelperFiles\Return\3_0';");
            //----- test recursive call of cmd files -----
            I("RESET;");
            I("RUN return1;");
            //_AssertScalarString(First(), "%x", "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            _AssertScalarString(First(), "%x", "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            //----- test RETURN -----
            I("RESET;");
            I("RUN return1a;");
            //_AssertScalarString(First(), "%x", "abcdABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            _AssertScalarString(First(), "%x", "abcdABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        [TestMethod]
        public void _Test_Stop()
        {
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\HelperFiles\Return\3_0';");
            //-----------------------------------------------------------
            //----------------- testing STOP --------------
            //-----------------------------------------------------------
            //----- test STOP -----
            I("RESET;");
            FAIL("RUN return1b;");            
            _AssertScalarString(First(), "%x", "abcd");
            //Assert.AreEqual(Assigns.GetAssign("x").string2, "abcd");
        }

        [TestMethod]
        public void _Test_Exit()
        {
            //-----------------------------------------------------------
            //----------------- testing EXIT --------------
            //-----------------------------------------------------------
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\HelperFiles\Return\';");
            //----- test EXIT -----
            I("RESET;");
            FAIL("RUN return1c.cmd;");
            _AssertScalarString(First(), "%x", "abcd");
        }

        [TestMethod]
        public void _Test_MAKRO_test1()
        {
            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\MAKRO\test1\';");
            I("RUN makro;");
        }

        [TestMethod]
        public void _Test_ADAMworkshop2011()
        {
            for (int i = 0; i < 2; i++)
            {
                //first time, it will be parsed and compiled (since cache is always flushed before unit testing)
                //second time, it will be loaded from cache
                //so the loop tests that the cache works.
                UData u = null;
                I("RESET; MODE sim;");
                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\ADAM-kursus\';");  //needs "'" since it contains a "-"
                I("option solve gauss reorder no;"); //Necessary for historical simulation, maybe also for last test... would be nice to be able to avoid it
                I("MODEL dec09_tony;");

                // ----- First some tests that have been problematic before: ------------------------------
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");  //TT changed 20-01-2013
                I("SERIES<2008 2008> tg += 0.03;");
                I("sim <2008 2010>;");
                u = Data("fcp", 2008, "a"); Assert.AreEqual(u.q, -0.87, 0.01d); //%
                u = Data("fcp", 2009, "a"); Assert.AreEqual(u.q, -1.04, 0.01d); //%
                u = Data("fcp", 2010, "a"); Assert.AreEqual(u.q, -1.47, 0.01d); //%
                u = Data("q", 2008, "a"); Assert.AreEqual(u.q, -0.20, 0.01d); //%
                u = Data("q", 2009, "a"); Assert.AreEqual(u.q, -0.20, 0.01d); //%
                u = Data("q", 2010, "a"); Assert.AreEqual(u.q, -0.31, 0.01d); //%
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2025> boil *= 1.10;");
                I("sim <2010 2025>;");
                u = Data("boil", 2010, "a"); Assert.AreEqual(u.q, 10d, 0.01d); //%
                u = Data("boil", 2025, "a"); Assert.AreEqual(u.q, 10d, 0.01d); //%
                u = Data("fy", 2010, "a"); Assert.AreEqual(u.q, -0.06, 0.01d); //%
                u = Data("fy", 2011, "a"); Assert.AreEqual(u.q, -0.10, 0.01d); //%
                u = Data("fy", 2012, "a"); Assert.AreEqual(u.q, -0.12, 0.01d); //%
                u = Data("fy", 2013, "a"); Assert.AreEqual(u.q, -0.14, 0.01d); //%
                u = Data("fy", 2014, "a"); Assert.AreEqual(u.q, -0.16, 0.01d); //%
                u = Data("fy", 2025, "a"); Assert.AreEqual(u.q, -0.14, 0.01d); //%
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("PRT pws_cf, pws_cr, pws_lse;");
                I("SERIES <2010 2010> pws_lse *= 1.1;");
                I("SERIES <2010 2010> jrpws_cf += 0.1;");
                I("SERIES <2010 2010> jrpws_cr += 0.1;");
                I("SIM <2010 2010>;");
                u = Data("wn_h", 2010, "a"); Assert.AreEqual(u.q, 20.69d, 0.01d); //% PCIM says 20.59%
                u = Data("wn_cf", 2010, "a"); Assert.AreEqual(u.q, -28.42d, 0.01d); //% PCIM says -28.43%
                u = Data("wn_cr", 2010, "a"); Assert.AreEqual(u.q, 6.58d, 0.01d); //%
                // ----- Problematic tests end ----------------------------------------------------------------

                //--- b3 ----
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> jcpuxh  += 1000;");
                I("SIM <2010 2010>;");
                u = Data("cpuxh", 2010, "a"); Assert.AreEqual(u.q, 0.14, 0.01d); //%
                u = Data("cg", 2010, "a"); Assert.AreEqual(u.q, 0.0255, 0.001d);
                u = Data("cb", 2010, "a"); Assert.AreEqual(u.q, 0.56, 0.01d); //%
                u = Data("cbu", 2010, "a"); Assert.AreEqual(u.q, 0.1059, 0.001d); //%

                //--- b4 ----
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> jrcpuxh  += 0.01000;");
                I("SIM <2010 2040>;");
                u = Data("cpuxh", 2010, "a"); Assert.AreEqual(u.q, 1.03, 0.01d); //%
                u = Data("cpuxh", 2040, "a"); Assert.AreEqual(u.q, -0.11, 0.01d); //%

                //--- b5 ----
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> JRcpuxh  += 0.01;");
                I("SERIES <2010 2040> JRcpuxhw  += 0.01;");
                I("SIM <2010 2040>;");
                u = Data("cpuxh", 2010, "a"); Assert.AreEqual(u.q, 1.03, 0.01d); //%
                u = Data("cpuxh", 2011, "a"); Assert.AreEqual(u.q, 1.23, 0.01d); //%
                u = Data("cpuxh", 2012, "a"); Assert.AreEqual(u.q, 1.36, 0.01d); //%
                u = Data("cpuxh", 2013, "a"); Assert.AreEqual(u.q, 1.45, 0.01d); //%
                u = Data("cpuxh", 2014, "a"); Assert.AreEqual(u.q, 1.51, 0.01d); //%
                u = Data("cpuxh", 2015, "a"); Assert.AreEqual(u.q, 1.55, 0.01d); //%
                u = Data("cpuxh", 2040, "a"); Assert.AreEqual(u.q, 0.14, 0.01d); //%

                //--- c1 ----
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2040> iwdm  += 0.01;");
                I("SIM <2010 2040>;");
                u = Data("iwb30", 2010, "a"); Assert.AreEqual(u.m, 0.01, 0.0001d); //%
                u = Data("iwbflx", 2010, "a"); Assert.AreEqual(u.m, 0.01, 0.0001d); //%
                u = Data("buibhx", 2010, "a"); Assert.AreEqual(u.m, 0.0075, 0.0001d); //%
                u = Data("phk", 2010, "a"); Assert.AreEqual(u.q, -4.6, 0.1); //%

                //--- c2 ----
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("prt rpibhe, drpibhe;");
                I("SERIES <2010 2015> Zrpibhe  = 0.07;");
                I("SIM <2010 2040>;");
                u = Data("buibhx", 2010, "a"); Assert.AreEqual(u.q, -47.543, 0.01); //%
                u = Data("rpibhe", 2010, "a"); Assert.AreEqual(u.m, 0.05, 0.001); //abs
                u = Data("phk", 2010, "a"); Assert.AreEqual(u.q, 15.96, 0.01); //%
                u = Data("wcp", 2010, "a"); Assert.AreEqual(u.q, 24.08, 0.01); //%
                u = Data("fy", 2011, "a"); Assert.AreEqual(u.q, 0.80, 0.01); //%

                //--- c3 ----
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> nhlo = 30;");
                I("SIM <2010 2010>;");
                u = Data("buibhx", 2010, "a"); Assert.AreEqual(u.m, 0.0080, 0.0001); //%
                u = Data("phk", 2010, "a"); Assert.AreEqual(u.q, -4.49, 0.01); //%

                // d1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("PRT <2010 2010> pe59, fe59;");
                I("PRT 100*500/E59;");
                I("SERIES <2010 2010> jrfe59  += 0.0013255;");
                I("SIM <2010 2010>;");
                u = Data("e59", 2010, "a"); Assert.AreEqual(u.m, 500.87, 0.01);
                u = Data("fe59", 2010, "a"); Assert.AreEqual(u.m, 460.74, 0.01);

                // d2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> fImo  += 500;");
                I("SIM <2010 2010>;");
                u = Data("fm", 2010, "a"); Assert.AreEqual(u.m, 326.47, 0.01);
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> fIbo  += 500;");
                I("SIM <2010 2010>;");
                u = Data("fm", 2010, "a"); Assert.AreEqual(u.m, 255.28, 0.01);

                // d3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2011 2011> fee2  *= 0.919;");
                I("SERIES <2011 2011> fee59 *= 0.878;");
                I("SERIES <2011 2011> feesq *= 0.843;");
                I("SIM <2011 2040>;");
                I("TIME 2011 2011;");
                u = Data("fe", 2011, "a"); Assert.AreEqual(u.q, -6.15, 0.01);  //%
                u = Data("fy", 2011, "a"); Assert.AreEqual(u.q, -2.38, 0.01);  //%
                u = Data("fcp", 2011, "a"); Assert.AreEqual(u.q, -0.30, 0.01);  //%
                u = Data("fcp", 2012, "a"); Assert.AreEqual(u.q, -1.12, 0.01);  //%
                u = Data("q", 2011, "a"); Assert.AreEqual(u.m, -47.31, 0.01);
                u = Data("ul", 2011, "a"); Assert.AreEqual(u.m, 35.74, 0.01);
                u = Data("lna", 2011, "a"); Assert.AreEqual(u.q, -0.38, 0.01);  //%

                // e1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("RUN e1.cmd;");
                I("TIME 2010 2010;");
                I("PRT -200/fCe*1000;");
                I("SERIES <2010 2010> JRbfce += -0.005313;");
                I("SIM <2010 2010>;");
                u = Data("fce", 2010, "a"); Assert.AreEqual(u.m, -186.06, 0.01);
                u = Data("fxe", 2010, "a"); Assert.AreEqual(u.m, 0);
                u = Data("fxne", 2010, "a"); Assert.AreEqual(u.m, -74.57, 0.01);
                u = Data("fxng", 2010, "a"); Assert.AreEqual(u.m, -4.13, 0.01);
                u = Data("fm3", 2010, "a"); Assert.AreEqual(u.m, -9.66, 0.01);
                u = Data("ce", 2010, "a"); Assert.AreEqual(u.m, -254.56, 0.01);
                u = Data("spg_ce", 2010, "a"); Assert.AreEqual(u.m, -51.01, 0.01);
                u = Data("spp_ce", 2010, "a"); Assert.AreEqual(u.m, -74.34, 0.01);
                u = Data("xne_ce", 2010, "a"); Assert.AreEqual(u.m, -102.64, 0.01);

                // e2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("SERIES dfcp = 1;");
                I("SERIES dfcs = 1;");
                I("PRT 1000/pcs, 1000/pcp;");
                I("SERIES <2010 2010> zfcs += 848;");
                I("SERIES <2010 2010> zfcp += 838;");
                I("SIM <2010 2010>;");
                u = Data("y", 2010, "a"); Assert.AreEqual(u.m, 906, 1);
                u = Data("cp", 2010, "a"); Assert.AreEqual(u.m, 1010, 1);

                // e3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("SERIES dfcp = 1;");
                I("SERIES dfcv = 1;");
                I("PRT 1000/pcv, 1000/pcp;");
                I("SERIES zfcv += 945;");
                I("SERIES zfcp += 838;");
                I("SIM <2010 2010>;");
                u = Data("y", 2010, "a"); Assert.AreEqual(u.m, 734, 1);
                u = Data("q", 2010, "a"); Assert.AreEqual(u.m, 0.595, 0.002);

                // e4
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("PRT pe59;");
                I("SERIES <2010 2010> jdpe59 += -0.010796;");
                I("SIM <2010 2020>;");
                u = Data("fe59", 2010, "a"); Assert.AreEqual(u.q, 0.7525, 0.001);
                u = Data("fe59", 2020, "a"); Assert.AreEqual(u.q, 1.842, 0.001);

                // f1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES dtp1 = 1;");
                I("SERIES dtp2 = 1;");
                I("LIST #tve = ztvea, ztveb, ztvee, ztveh, ztvene, ztvenf, ztveng, ztvenz, ztveqf, ztveqz, ztveqs;");
                I("SERIES <2010 2040> {#tve}  *= 1.10;");
                I("SIM<2010 2040>;");
                u = Data("fvep", 2010, "a"); Assert.AreEqual(u.q, -0.165, 0.001);
                u = Data("spp_vep", 2010, "a"); Assert.AreEqual(u.q, 9.686, 0.001);

                // f2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> jrfxnf  += 0.01;");
                I("sim< 2010 2040>;");
                u = Data("xnf", 2010, "a"); Assert.AreEqual(u.m, 1321, 1);
                u = Data("e01", 2010, "a"); Assert.AreEqual(u.m, 1121, 1);

                // f3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("RUN f3.cmd;");
                I("SERIES<2010 2040> {#dtl} *= 1.01;");
                I("SIM <2010 2040>;");
                I("prt<m> hqp;");
                I("prt<m> fxp, pxp, fe;");
                I("prt<m> lna/pcp, lna, pcp;");
                I("prt 100*((lna/pcp)/(@lna/@pcp)-1), pcp, lna;");
                u = Data("hqp", 2010, "a"); Assert.AreEqual(u.q, -0.323, 0.001);
                u = Data("fe", 2010, "a"); Assert.AreEqual(u.q, 0.182, 0.001);

                //HERTIL


                // f4
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2040> jiwlo  += 0.01;");
                I("SIM <2010 2040>;");
                u = Data("uimp", 2010, "a"); Assert.AreEqual(u.q, 4.892, 0.001);

                // g1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2040> Dlna  = 1;");
                I("SERIES <2010 2040> Zlna  *= 1.01;");
                I("SIM <2010 2040>;");
                I("MULPRT lna, pwnzw, pxnzw, pxnz;");
                I("PRT ((pxnz/pwnzw)/(@pxnz/@pwnzw)-1)*100;");
                u = Data("pwnzw", 2010, "a"); Assert.AreEqual(u.q, 0.411, 0.001);

                // g2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2040>dlna  = 1;");
                I("SERIES <2010 2040>zlna  *= 1.01;");
                I("SIM <2010 2040>;");
                u = Data("pwqzw", 2010, "a"); Assert.AreEqual(u.q, 0.533, 0.001);
                u = Data("pwqfw", 2010, "a"); Assert.AreEqual(u.q, 0.685, 0.001);


                // g3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES<2010 2040 > JRuimnz += .01;");
                I("SIM;");
                I("PRT ((pxnz/pwnzw)/(@pxnz/@pwnzw)-1)*100;");
                u = Data("pwnzw", 2010, "a"); Assert.AreEqual(u.q, 0.0477, 0.001);

                // h1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010 >jrlna += .05;");
                I("SIM <2010 2040>;");
                u = Data("lna", 2010, "a"); Assert.AreEqual(u.q, 5.401, 0.001);
                u = Data("lna", 2040, "a"); Assert.AreEqual(u.q, -0.774, 0.001);

                // h2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2040> ur2  += 10;");
                I("SIM <2010 2040>;");
                u = Data("ua", 2010, "a"); Assert.AreEqual(u.m, 7.605, 0.001);
                u = Data("q", 2040, "a"); Assert.AreEqual(u.m, 10.150, 0.001);

                // h3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2020> dlisa  = 1;");
                I("SIM <2010 2020>;");
                u = Data("lna", 2020, "a"); Assert.AreEqual(u.q, -4.316, 0.001);
                u = Data("btyd", 2020, "a"); Assert.AreEqual(u.q, -12.763, 0.001);

                // i2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("time 2010 2010;");
                I("prt fce, 500/fce;");
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("SERIES dtp2 = 1;");
                I("SERIES ztpce += 0.0133;");
                I("SIM <2010 2010>;");
                u = Data("fce", 2010, "a"); Assert.AreEqual(u.q, -0.410, 0.001);

                // i3a
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("SERIES dtp2 = 1;");
                I("ENDO ztpce;");
                I("EXO spp_ce;");
                I("SERIES <2010 2010> spp_ce  += 500;");
                I("SIM<fix> ;");
                //I("ENDO; EXO; OPTION solve method gauss;");
                I("UNFIX;");
                u = Data("tpce", 2010, "a"); Assert.AreEqual(u.m, 0.0152, 0.0001);

                // i3b
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("SERIES dtp2 = 1;");
                I("ENDO ztpce;");
                I("EXO fCe;");
                I("SERIES <2010 2010>fCe  *= 0.98;");
                I("SIM<fix> ;");
                //I("ENDO; EXO; OPTION solve method gauss;");
                I("UNFIX;");
                u = Data("tpce", 2010, "a"); Assert.AreEqual(u.m, 0.066999, 0.0001);


                // Opg J
                // j1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010> fibo  += 1000;");
                I("SIM <2010 2020>;");
                u = Data("tf_o_z", 2010, "a"); Assert.AreEqual(u.m, 1364, 1);
                u = Data("tfn_o", 2010, "a"); Assert.AreEqual(u.m, -821, 1);

                // j2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("PRT 0.001/iwbdm;");
                I("SERIES <2010 2020>kiwbdm  += 0.0283;");
                I("SIM <2010 2020>;");
                u = Data("iwbz", 2010, "a"); Assert.AreEqual(u.m, 0.00099899, 0.00001);

                // j3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("PRT tfs_x_os;");
                I("SERIES tfs_x_os += -1000;");
                I("SIM< 2010 2010>;");
                u = Data("fy", 2010, "a"); Assert.AreEqual(u.m, 1.66, 1);
                u = Data("wn_osslog", 2010, "a"); Assert.AreEqual(u.m, 1046, 1);

                // k1
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("SERIES <2010 2010>jdfvmo  += 1000;");
                I("SIM <2010 2040>;");
                I("TIME 2010 2010;");
                u = Data("Vmo", 2010, "a"); Assert.AreEqual(u.m, 1235, 1);
                u = Data("Y", 2010, "a"); Assert.AreEqual(u.m, 1086, 1);

                // k2
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("PRT spg;");
                I("PRT 0.25*(40000/spg);");
                I("SERIES <2010 2010> tg  += -0.0542;");
                I("SIM <2010 2015>;");
                u = Data("fCp", 2010, "a"); Assert.AreEqual(u.q, 1.3446, 0.001);

                // k3
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("PRT pibo;");
                I("PRT 40000/pibo;");
                I("SERIES <2010 2010> fibo  += 28996;");
                I("SIM< 2010 2015>;");
                u = Data("fY", 2010, "a"); Assert.AreEqual(u.q, 1.9447, 0.001);
                u = Data("Q", 2010, "a"); Assert.AreEqual(u.m, 35.858, 0.001);

                // k4
                I("CLEAR<first>; IMPORT<tsd>lang10; CLONE;");
                I("TIME 2010 2010;");
                I("PRT 47/Ha*100;");
                I("SERIES <2010 2040>JHa  += 47;");
                I("SERIES <2010 2010>JRlna  += -0.02868;");
                I("SIM <2010 2040>;");
                u = Data("HQ", 2010, "a"); Assert.AreEqual(u.q, 1.2606, 0.001);
                u = Data("Q", 2010, "a"); Assert.AreEqual(u.m, -43.73, 0.01);

                if (Globals.UNITTESTFOLLOWUP)
                {
                    // l2
                    I("CLEAR<first>; IMPORT<tsd>hist1110; CLONE;");
                    I("RUN EXJAN11.cmd;");
                    I("SIM 2010 2012;");
                    I("PRT lna, Q, Enl;");
                    I("WRITE grund1;");  //clone??
                    I("READ <ref> grund1;");  //clone??
                    I("SERIES dlna = 1;");
                    I("SERIES zlna % 1;");
                    I("SIM <2010 2012>;");
                    I("MULPRT JRlna   ; () se hvordan der er genereret nye jled");
                    I("WRITE grund2;");
                    I("SERIES dlna = 0;");
                    I("SERIES <2010 2012>qwo  + 20;");
                    I("SIM <2010 2012>;");
                    I("READ <ref> grund2;");
                    I("MULPRT lna, Q, Enl;");
                    u = Data("Q", 2010, "a"); Assert.AreEqual(u.m, 23.102, 0.001);
                    u = Data("Enl", 2010, "a"); Assert.AreEqual(u.m, -2256.98, 0.01);
                }
            }
        }


        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // ==================== INTEGRATION TESTS (slow) ===============================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================
        // =============================================================================

        [TestMethod, Timeout(7200000)]  //two hours
        public void Integration_ModelRandom()
        {
            if (!Globals.unitTestIntegration)
            {
                Assert.Inconclusive(Globals.unitTestIntegrationMessage);
                return;
            }

            for (int i = 0; i < 2; i++)  //to get cached .mdl files tested too (second time, for i=1)
            {

                //Takes about an hour in debug mode
                //TODO: look at ii>200 for endoexo (n*.cmd), done only 20% of models now
                //      166, 181: why are they bad for endoexo: draw graph of them
                //      why the stalls??
                List<int> bad = new List<int>() { 3, 11, 20, 23, 34, 40, 42, 62, 64, 65, 79, 95, 99, 107, 109, 114, 120, 137, 140, 145, 147, 149, 157, 166, 175, 179, 181, 182, 185, 188, 191, 196, 198 };
                List<int> stall = new List<int>() { 5, 6, 33, 36, 41, 43, 45, 47, 68, 75, 77, 90, 91, 102, 122, 133, 165, 169, 178 };

                //-----------------------------------------------------------
                //----------------- testing random models -------------------
                //-----------------------------------------------------------

                I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\models\random\';");

                double delta = 0.00001d;  //newton

                for (int ii = 0; ii < 1000; ii++)
                {
                    for (int jj = 0; jj <= 2; jj++)  //0:gauss, 1:newton 2:newton_with_two_goals
                    {
                        if (jj == 2)
                        {
                            if (ii > 200) continue;
                            if (bad.Contains(ii)) continue;
                            if (stall.Contains(ii)) continue;
                        }

                        if (jj == 0) delta = 0.0005;  //lower for gauss

                        I("RESET;");

                        if (jj == 0) I("OPTION solve method gauss;");
                        else if (jj == 1) I("OPTION solve method newton;");
                        else if (jj == 2) I("OPTION solve method newton;");
                        else throw new Exception();

                        I("OPTION databank file format = tsd;");

                        I("READ m.tsd;");
                        if (jj == 2) I("RUN n" + ii + ".cmd;");
                        else I("RUN m" + ii + ".cmd;");
                        for (int t = 2002; t <= 2100; t++)
                        {
                            _AssertHelperTwoDoubles((First().GetIVariable("sum!a") as Series).GetDataSimple(new GekkoTime(EFreq.A, t, 1)), 0d, delta);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void Integration_OEM2012()
        {
            if (!Globals.unitTestIntegration)
            {
                Assert.Inconclusive(Globals.unitTestIntegrationMessage);
                return;
            }

            //-----------------------------------------------------------
            //----------------- testing OEM konj.kørsel -----------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\OEM-fremskrivn\Konj_2011\'");
            I("ADD master.cmd;");
            I("READ<ref tsdx> NY_BANK_FM.orig.tsdx;");
            CheckDatabankSample(new double[] { 0.001, 0.001, 0.02 }, 2010, 2013, true);  //.001 % is very small! And also 0.02 (abs)
        }

        [TestMethod]
        public void Integration_FM2013vaekst()
        {
            if (!Globals.unitTestIntegration)
            {
                Assert.Inconclusive(Globals.unitTestIntegrationMessage);
                return;
            }

            //-----------------------------------------------------------
            //----------------- testing FM vækstanalyse -----------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\FM-fremskrivn\Vaekst\'");
            I("ADD b17.add;");
            I(@"READ<ref> \banker\b17_orig;");
            List<string> ignore = new List<string>() { "ZCOIPYWO", "ZCOIDEM", "ZCOCDEM", "ZCOCPYWO", "ZCOII", "ZInvbo", "ZInvmo", "ZCOIM", "ZPNPG", "ZPNPE", "ZPFPG", "ZPFPE", "ZPNPH", "ZPFPH", "ZPFPS", "ZPNPS", "ZPNPF", "ZPFPF", "ZPFPB", "ZPNPB", "ZPFPV", "ZPNPV",
"xx_afil", "xx_fytr", "xx_fywo11", "xx_opspkv1", "xx_pytr", "xx_opspkv2", "xx_si", "xx_YTR", "YSXpuc",
"YSXpia",
"YSXpib",
"YSXpic",
"YSXtip1",
"YSXsafm",
"YSXysx",
"YSXjysx",
"XXqord",
"XXTUKXA",
"XXTULU",
"XXTUAAA",
"XXTUASBI",
"XXTUAUDR",
"XXTULL",
"XXsamlomv",
"XXur",
"XXREST",
"XXSARPO",
"XXRESTI",
"XXRESTU",
"xxlqn",
"xxtya",
"xxtyb",
"xxtyfp",
"xxpu",
"xxtphi",
"xxneti",
"xxsafm",
"xxkor",
"bYWBY",
"YWBY",
"YrBY",
"xxywby",
"xxyfby",
"xxbywby",
"xxbyw1",
"xxbywa1",
"xxbywb1",
"xxbywh1",
"xxbywe1",
"xxbywne1",
"xxbywnf1",
"xxbywng1",
"xxbywnz1",
"xxbywo2",
"xxbywp1",
"xxbywqf1",
"xxbywqz1",
"xxbywqs1",
"xxbywby1",
"XXBYW",
"XXBYWA",
"XXBYWE",
"XXBYWNG",
"XXBYWNF",
"XXBYWNZ",
"XXBYWB",
"XXBYWQS",
"XXBYWQF",
"XXBYWQZ",
"XXBYWH",
"XXBYWO",
"xxxxbywp",
"xxxxbywby",
"xxbywp",
"xxpminuse",
"xxHB",
"xxTB",
"xxSB",
"xxLF",
"xxOEU",
"xxOQ",
"xxFormue",
"xxTFN_E",
"xxYwn_e",
"xx_co",
"xx_co1",
"xx_cor",
"xx_cor1",
"xx_eu",
"xx_ty",
"xx_tod",
"xx_tod1",
"xx_co_x",
"xx_co1_x",
"xx_cor_x",
"xx_cor1_x",
"xx_tod_x",
"xx_tod1_x",
"xxsi",
"XXSD",
"XXTFOI",
"XXOVRU",
"XXSA",
"XXSISUO",
"XXIO",
"XXTY",
"XXCO",
"XXTFOU",
"XXOIL",
"XXOVRI",
"PENSUD",
"PENSIND",
"PUDNET",
"SKATUD",
"SKATIND",
"SKATUNET",
"SKATFAKTOR",
"YSXy",
"YSXyw",
"YSXtya",
"YSXtyb",
"YSXtyc",
"YSXpua",
"YSXpub"
            };
            CheckFullDatabank(3d, double.NaN, 2010, 2020, ignore); //only checks absolute, must be < 3 absolute. Omits some Z-variables.

        }


        [TestMethod]
        public void Integration_FM2012()
        {
            if (!Globals.unitTestIntegration)
            {
                Assert.Inconclusive(Globals.unitTestIntegrationMessage);
                return;
            }

            //-----------------------------------------------------------
            //----------------- testing KP2012 --------------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\FM-fremskrivn\KP2012'");  //needs "'" since it contains a "-"
            I("ADD master.cmd;");
            I("ADD C56_udd54_nybef_hurtig.ADD");
            //Corresponds to: C56_udd54_nybef_hurtig_oldsim.tsdx
            I("SERIES <2012 2100> xx_fy =  1570025.837440  1593612.502167  1620234.893015  1657934.068628  1708457.896067  1743939.229578  1778146.942871  1802353.261887  1828812.460211  1852879.024827  1878866.563437  1902855.328806  1926229.890014  1951386.698845  1974221.001675  2004844.681430  2024255.989422  2046650.634639  2079390.504085  2098037.182960  2125103.828120  2143606.784571  2166740.982185  2202725.741074  2226106.855584  2256373.894839  2277536.899391  2299522.172193  2335310.917182  2360082.114953  2393599.143981  2419008.343025  2451610.926395  2487340.508335  2520758.349198  2559485.849414  2595930.551220  2635265.586487  2692910.919507  2734128.461303  2784909.701019  2829857.386988  2878149.264040  2931903.250865  2979047.816019  3029696.267504  3076248.441924  3123317.984324  3173110.722007  3219200.408314  3268654.346953  3313379.410826  3359121.581619  3419413.715441  3463816.510535  3514712.592418  3560897.413096  3609411.780036  3662660.447587  3713030.406492  3769364.695757  3821889.775852  3877196.187195  3949451.051151  4005462.202596  4068966.941546  4127712.277814  4188784.794300  4254357.117046  4316807.727987  4385669.788460  4450671.019094  4518954.999449  4602015.877817  4670811.811989  4746768.532078  4818060.791017  4892042.437954  4968245.312622  5042009.754278  5121674.720197  5196062.618117  5270465.405548  5353767.116486  5429792.640934  5512403.014823  5588521.826348  5666689.839807  5761793.266212;");
            I("SERIES <2012 2100> xx_cp =  897667.668010  931109.565365  970088.821440  1015536.708132  1058744.085965  1104899.969861  1152587.303670  1198638.118240  1245477.236181  1291417.514657  1335912.417807  1390930.237440  1444507.464833  1496501.055577  1546830.990817  1603657.346719  1653455.065134  1710932.065801  1773819.820761  1822383.549472  1890529.326485  1946059.616416  2015826.834407  2087537.905492  2144521.190261  2221682.007971  2285454.505220  2359471.048283  2444622.213933  2514805.542369  2610125.792705  2691722.406946  2791055.491041  2890811.047067  2989335.956787  3100243.075923  3207569.682872  3325260.759575  3460609.637197  3570688.203542  3719908.660649  3852186.855157  4000353.254782  4154712.072698  4303084.722116  4469603.237375  4631112.770516  4804158.913224  4985798.176195  5165917.449354  5357431.483737  5544872.449305  5746947.368788  5969535.304377  6161212.559551  6398521.508632  6619309.283426  6858913.679646  7111132.168522  7359629.381002  7630281.336747  7897238.670423  8187339.216077  8506586.346255  8786816.769133  9129493.729443  9451808.848651  9799582.211326  10164138.955451  10524780.094558  10914417.783300  11299304.969367  11715014.468440  12162176.320100  12570777.407010  13050464.687402  13508765.443509  14000919.830788  14507659.238095  15017752.972976  15560221.842004  16097617.145254  16670930.917723  17280989.853429  17864397.019050  18505311.723347  19130083.517998  19801402.280846  20520202.610137;");
            I("SERIES <2012 2100> xx_enl =  96888.972963  95531.813241  75847.848113  72446.566453  89371.829572  85888.645224  80996.105407  71976.146991  64579.346171  63047.934568  64296.238916  65678.929201  63290.782398  60310.245445  56539.534849  56609.885376  51966.367757  45073.333910  41427.818251  38245.705643  35798.036628  28168.562967  21272.740023  20120.068073  17184.352139  15396.440910  7433.964902  -3498.552147  -9105.890465  -15909.530511  -20231.027639  -30201.914625  -38839.981269  -47111.430455  -54171.020374  -58614.984338  -64799.042901  -70007.447302  -61742.826900  -57578.095574  -50657.229581  -47133.290321  -42607.144130  -34369.062084  -27649.208890  -19420.582601  -15262.451413  -12505.795427  -11305.483340  -11680.823403  -7977.300325  -10334.450577  -15180.479269  -7339.090332  -8244.699453  -10129.791543  -19308.609325  -30635.363047  -43547.912170  -55292.523397  -62832.734163  -77492.129138  -94130.467041  -92625.013198  -100911.829100  -109884.779187  -127764.257543  -147798.128415  -170357.652535  -190862.951732  -204082.065726  -223445.343523  -242450.846600  -239778.955937  -245556.780608  -248212.699019  -261267.330583  -274516.123060  -289880.271011  -304702.579285  -311527.093209  -330545.896712  -359992.644379  -387020.802765  -416818.973557  -442215.140532  -485340.430917  -539026.964611  -563703.890792;");
            double[] limits = new double[3];
            limits[0] = 0.02;
            limits[1] = 0.01;
            limits[2] = 232;  //still pretty tight: this is a pretty small number, Enl can be counted in 100.000's.
            CheckDatabankSample(limits, 2012, 2100, false);
        }

        [TestMethod]
        public void Integration_FM2010()
        {
            if (!Globals.unitTestIntegration)
            {
                Assert.Inconclusive(Globals.unitTestIntegrationMessage);
                return;
            }

            //-----------------------------------------------------------
            //----------------- testing KP2010 --------------------------
            //-----------------------------------------------------------

            I("RESET;");
            I("OPTION folder working = '" + Globals.ttPath2 + @"\regres\FM-fremskrivn\KP2010\Gekko-korsel\testing\'");  //needs "'" since it contains a "-"
            I("ADD master.cmd;");
            //corresponds to: dec09_v33_TRUE.tsd;
            I("SERIES <2008 2100> xx_fY =  1435490.000000  1373289.000000  1391442.000000  1413182.000000  1441097.000000  1473164.000000  1511066.000000  1550120.000000  1570930.000000  1592025.000000  1613365.000000  1641889.000000  1664905.000000  1692876.000000  1723610.000000  1753669.000000  1781790.000000  1821529.000000  1852389.000000  1894102.000000  1919927.000000  1944952.000000  1992546.000000  2016812.000000  2052909.000000  2082109.000000  2111499.000000  2166416.000000  2194695.000000  2236172.000000  2270515.000000  2305317.000000  2364779.000000  2399529.000000  2443535.000000  2484569.000000  2529925.000000  2581943.000000  2630733.000000  2685808.000000  2739016.000000  2793206.000000  2866431.000000  2921260.000000  2983298.000000  3042466.000000  3102445.000000  3167548.000000  3228322.000000  3294139.000000  3356155.000000  3418409.000000  3504852.000000  3566584.000000  3636668.000000  3702209.000000  3768058.000000  3839949.000000  3908698.000000  3984130.000000  4055833.000000  4128946.000000  4233012.000000  4307336.000000  4392973.000000  4474521.000000  4557207.000000  4651282.000000  4737357.000000  4832055.000000  4922336.000000  5013683.000000  5135070.000000  5227128.000000  5330751.000000  5429336.000000  5528907.000000  5646362.000000  5749005.000000  5861732.000000  5969315.000000  6077777.000000  6198789.000000  6308830.000000  6422325.000000  6537013.000000  6653395.000000  6785864.000000  6905918.000000  7037552.000000  7163614.000000  7291016.000000  7447027.000000  ;");
            I("SERIES <2008 2100> xx_Cp =  845511.000000  817199.700000  853401.200000  887541.100000  928749.000000  970620.500000  1017797.000000  1066729.000000  1109606.000000  1151184.000000  1194930.000000  1239318.000000  1289165.000000  1339732.000000  1399507.000000  1457954.000000  1512814.000000  1591914.000000  1645666.000000  1728652.000000  1778927.000000  1846813.000000  1954690.000000  1996028.000000  2093825.000000  2160379.000000  2246235.000000  2381732.000000  2421140.000000  2538549.000000  2623620.000000  2725411.000000  2878921.000000  2944622.000000  3080362.000000  3196381.000000  3328385.000000  3477575.000000  3606644.000000  3765698.000000  3913383.000000  4074229.000000  4285677.000000  4422397.000000  4622174.000000  4805293.000000  5004980.000000  5227344.000000  5428476.000000  5665945.000000  5886964.000000  6127873.000000  6448593.000000  6646093.000000  6944603.000000  7214090.000000  7507209.000000  7833265.000000  8124865.000000  8469056.000000  8788896.000000  9140901.000000  9615628.000000  9908582.000000  10340500.000000  10745450.000000  11177850.000000  11680250.000000  12107430.000000  12635240.000000  13120830.000000  13651710.000000  14332180.000000  14786650.000000  15435320.000000  16032860.000000  16678870.000000  17443820.000000  18056910.000000  18838540.000000  19557210.000000  20338250.000000  21222020.000000  21997110.000000  22887510.000000  23797070.000000  24739370.000000  25795130.000000  26736000.000000  27857670.000000  28917750.000000  30065890.000000  31445450.000000  ;");
            I("SERIES <2008 2100> xx_Enl =  37968.400000  48770.730000  42148.610000  43361.240000  48607.430000  57331.440000  64910.950000  74294.150000  58097.810000  43765.380000  27595.330000  23305.570000  23426.910000  24213.100000  21164.140000  19296.390000  18416.570000  10975.780000  12962.000000  6963.698000  11766.030000  1748.699000  -12226.270000  -3850.437000  -15207.480000  -16938.340000  -25957.450000  -44985.570000  -26209.310000  -38862.190000  -39503.830000  -49051.160000  -62634.250000  -46197.330000  -56880.430000  -58993.430000  -64882.790000  -69213.050000  -66707.720000  -68209.110000  -62670.090000  -60206.550000  -60436.790000  -31412.130000  -26783.010000  -12705.220000  -3460.915000  5288.071000  19700.840000  29224.920000  45768.210000  55235.480000  59665.820000  107744.500000  116109.600000  137832.600000  149589.400000  158956.800000  172010.000000  180743.500000  198345.700000  204343.600000  201691.900000  267163.900000  269685.700000  290838.600000  299133.700000  302536.800000  324733.000000  330189.400000  351862.100000  358308.300000  355769.100000  436077.800000  442734.800000  477731.200000  495803.100000  507094.300000  564946.500000  587967.300000  637108.700000  667087.700000  686997.000000  751203.300000  774781.600000  804092.000000  828932.300000  857806.600000  930911.600000  957647.300000  1016751.000000  1046732.000000  1064218.000000 ;");
            double[] limits = new double[3];
            limits[0] = 0.01;
            limits[1] = 0.01;
            limits[2] = 150;
            CheckDatabankSample(limits, 2008, 2100, false);
            I("READ<ref> true_from_pcim.tsd;");
            bool isOk = Program.TestKP2010Model();  //very thorough check of databank 2005-2020. All vars except J- and Z-vars.
            Assert.IsTrue(isOk);
        }

        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------ helper methods --------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // ------------------------------------------------------------

        private static UData Data(string s, int year, string freq)
        {
            return Data(s, year, 1, freq);
        }

        private static UData Data(string s, int year, int sub, string freq)
        {
            s = G.Chop_AddFreq(s, freq);
            if (!G.Chop_HasFreq(s)) G.Chop_AddFreq(s, "a");
            string f = G.Chop_GetFreq(s);

            //hack: false used 4 places in GetVariable() and Contains()
            double valWork = (First().GetIVariable(s) as Series).GetDataSimple(new GekkoTime(G.GetFreq(f), year, sub));
            double valBase = double.NaN;
            if (Ref().ContainsIVariable(s)) valBase = (Ref().GetIVariable(s) as Series).GetDataSimple(new GekkoTime(G.GetFreq(f), year, sub));
            double valWorkLag = (First().GetIVariable(s) as Series).GetDataSimple(new GekkoTime(G.GetFreq(f), year, sub).Add(-1));
            double valBaseLag = double.NaN;
            if (Ref().ContainsIVariable(s)) valBaseLag = (Ref().GetIVariable(s) as Series).GetDataSimple(new GekkoTime(G.GetFreq(f), year, sub).Add(-1));
            //end hack

            UData u = new UData();
            u.w = valWork;
            u.b = valBase;
            u.m = valWork - valBase;
            u.q = (valWork / valBase - 1d) * 100d;
            u.d = valWork - valWorkLag;
            u.p = (valWork / valWorkLag - 1d) * 100d;
            return u;
        }

        private static void CheckDatabankSample(double[] limits, int t1, int t2, bool useBank)
        {
            Series fy = First().GetIVariable("fy!a") as Series;
            Series cp = First().GetIVariable("cp!a") as Series;
            Series enl = First().GetIVariable("enl!a") as Series;
            Series xx_fy = null;
            Series xx_cp = null;
            Series xx_enl = null;
            if (useBank)
            {
                xx_fy = Program.databanks.GetRef().GetIVariable("fy!a") as Series;
                xx_cp = Program.databanks.GetRef().GetIVariable("cp!a") as Series;
                xx_enl = Program.databanks.GetRef().GetIVariable("enl!a") as Series;
            }
            else
            {
                xx_fy = First().GetIVariable("xx_fy!a") as Series;
                xx_cp = First().GetIVariable("xx_cp!a") as Series;
                xx_enl = First().GetIVariable("xx_enl!a") as Series;
            }
            for (int t = t1; t <= t2; t++)
            {
                double p1 = (fy.GetDataSimple(new GekkoTime(EFreq.A, t, 1)) / xx_fy.GetDataSimple(new GekkoTime(EFreq.A, t, 1)) - 1) * 100d;
                double p2 = (cp.GetDataSimple(new GekkoTime(EFreq.A, t, 1)) / xx_cp.GetDataSimple(new GekkoTime(EFreq.A, t, 1)) - 1) * 100d;
                double a3 = enl.GetDataSimple(new GekkoTime(EFreq.A, t, 1)) - xx_enl.GetDataSimple(new GekkoTime(EFreq.A, t, 1));
                Assert.IsTrue(Math.Abs(p1) < limits[0]); //%
                Assert.IsTrue(Math.Abs(p2) < limits[1]); //%
                Assert.IsTrue(Math.Abs(a3) < limits[2]); //abs
            }
        }

        private static void I(string s)
        {
            Globals.lastPrtOrMulprtTable = null;
            Program.obeyCommandCalledFromGUI(s, new P());
        }

        private static void CheckFullDatabank(double deltaAbs, double deltaRel, int t1, int t2)
        {
            List<string> ignore = new List<string>();
            CheckFullDatabank(deltaAbs, deltaRel, t1, t2, ignore);
        }

        private static Databank First()
        {
            return Program.databanks.GetFirst();
        }

        private static Databank Ref()
        {
            return Program.databanks.GetRef();
        }

        private static Databank Local()
        {
            return Program.databanks.GetLocal();
        }

        private static Databank Global()
        {
            return Program.databanks.GetGlobal();
        }

        private static void CheckFullDatabank(double deltaAbs, double deltaRel, int t1, int t2, List<string> ignore)
        {
            //Remember that the check is < deltaAbs OR < deltaRel.
            foreach (string s in First().storage.Keys)
            {
                if (G.Chop_HasSigil(s)) continue;
                foreach (string ss in ignore)
                {
                    if (G.Equal(s, ss)) goto Flag;
                }
                Series tsW = First().GetIVariable(s) as Series;
                Series tsB = Program.databanks.GetRef().GetIVariable(s) as Series;

                if (tsW == null || tsB == null)
                {
                    Assert.Fail("Missing time series");
                }

                for (int t = t1; t <= t2; t++)
                {
                    double dW = tsW.GetDataSimple(new GekkoTime(EFreq.A, t, 1));
                    double dB = tsB.GetDataSimple(new GekkoTime(EFreq.A, t, 1));
                    if (double.IsNaN(dW) && double.IsNaN(dB))
                    {
                        //do nothing
                    }
                    else if (double.IsNaN(dW) || double.IsNaN(dB))
                    {
                        Assert.Fail("One and only one of 2 doubles are NaN");
                    }
                    else
                    {
                        double abs = Math.Abs(dW - dB);
                        double rel = Math.Abs((dW / dB - 1) * 100d);
                        if (double.IsNaN(deltaRel))
                        {
                            Assert.IsTrue(abs < deltaAbs);
                        }
                        else
                        {
                            Assert.IsTrue(abs < deltaAbs || rel < deltaRel);
                        }
                    }
                }
            Flag: ;
            }
        }

        public class UData
        {
            public double m;
            public double q;
            public double d;
            public double p;
            public double w;
            public double b;
        }

    }
}
