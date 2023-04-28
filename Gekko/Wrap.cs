using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Gekko
{
    /// <summary>
    /// Essentially a List&lt;string&gt;
    /// </summary>
    public class WrapHelper5
    {
        public int linesAtStart = 1;  //see also the method MainOmitVeryFirstNewLine()
        public List<string> storage = new List<string>();
        public string consolidated = null;

        public WrapHelper5()
        {
        }

        public WrapHelper5(int linesAtStart)
        {
            this.linesAtStart = linesAtStart;
        }
    }

    /// <summary>
    /// A helper class for printing multiple lines. It basically just stores List&lt;string&gt; inside.
    /// Supports indents and other stuff. Will eventually replace all the G.Writeln() variants. Link
    /// to help system is easy: {a{here¤series.htm}a}.
    /// </summary>
    public class Wrap : IDisposable
    {
        //Note: links to documentation are easy, for instance "Read more in help system "

        private List<WrapHelper5> storageMain = new List<WrapHelper5>(); //shown in main error text
        private List<WrapHelper5> storageMore = new List<WrapHelper5>(); //link regarding more information
        private EWrapType type = EWrapType.Writeln;
        private bool throwExceptionForError = true;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type"></param>
        public Wrap(EWrapType type)
        {
            this.type = type;
            this.storageMain.Add(new WrapHelper5());
            this.storageMore.Add(new WrapHelper5(0));  //no blank line at start in output tab
        }

        /// <summary>
        /// After using(...) {...}, Exe() is run so that it gets printed.
        /// </summary>
        public void Dispose()
        {
            this.Exe1();
        }

        /// <summary>
        /// With this, the Error object will not throw any exception
        /// </summary>
        public void ThrowNoException()
        {
            this.throwExceptionForError = false;
        }

        /// <summary>
        /// Add text to "main"
        /// </summary>
        /// <param name="s"></param>
        public void MainAdd(string s)
        {
            this.storageMain[this.storageMain.Count - 1].storage.Add(s);
        }

        /// <summary>
        /// Add a section break to "main". Blank line in between.
        /// </summary>
        public void MainNewLine()
        {
            this.storageMain.Add(new WrapHelper5());
        }

        /// <summary>
        /// Remove starting line before the first section of 
        /// </summary>
        public void MainOmitVeryFirstNewLine()
        {
            WrapHelper5 wh = null;
            if (this.storageMain != null && this.storageMain.Count > 0) wh = this.storageMain[0];
            wh.linesAtStart = 0;
        }

        /// <summary>
        /// Add a section break to "main". No blank line in between.
        /// </summary>
        public void MainNewLineTight()
        {
            this.storageMain.Add(new WrapHelper5(0));
        }

        /// <summary>
        /// Add text to "more".
        /// </summary>
        /// <param name="s"></param>
        public void MoreAdd(string s)
        {
            this.storageMore[this.storageMore.Count - 1].storage.Add(s);
        }

        /// <summary>
        /// Add a section break to "more". Blank line in between.
        /// </summary>
        public void MoreNewLine()
        {
            this.storageMore.Add(new WrapHelper5());
        }

        /// <summary>
        /// Add a section break to "more". No blank line in between.
        /// </summary>
        public void MoreNewLineTight()
        {
            this.storageMore.Add(new WrapHelper5(0));
        }

        /// <summary>
        /// Actually "runs" the object, printing stuff on screen.
        /// If the type is Error, a GekkoException will be thrown after printing.
        /// Will call Exe2(), on the GUI thread.
        /// </summary>
        public void Exe1(Exception e)
        {
            if (type == EWrapType.Error && this.throwExceptionForError && !G.IsDecompOrFindThread())
            {
                //if throwExceptionForError == false, an exception is not thrown, and CrossThreadStuff.Wrap() will print the error below.
                //if decomp or find thread, the error is thrown later on.
                Globals.numberOfErrors++;
                //this "stores" the error, for later pretty printing when the exception is caught (HandleRunErrors.cs)
                //the GekkoException will be stored inside an innerException when caught later on, 
                //not sure why.
                //will never execute CrossThreadStuff.Wrap(this) below here, but it is called where the 
                //exception is caught (HandleRunErrors.cs).
                if (e == null)
                {
                    throw new GekkoException(this);
                }
                else
                {
                    GekkoException ge = e as GekkoException;
                    if (ge == null)
                    {
                        //not a GekkoException, but a normal Exception.
                        //this may sometimes occur when we are doing a try/catch, catching Exception, 
                        //and using that Exception in Error(..., e). If the exception is not a GekkoException,
                        //we end up here.
                        //??? should we piggyback any C# error message here?
                        throw new GekkoException(this);
                    }
                    else
                    {
                        ge.wraps.Add(this);
                        throw ge;  //rethrow it
                    }
                }
            }
            else if (type == EWrapType.Warning)
            {
                Globals.numberOfWarnings++;
            }

            if (G.IsDecompOrFindThread()) this.Exe2();  //keep it on its own thread
            else CrossThreadStuff.Wrap(this);  //calls .Exe2() on the GUI thread.
        }

        /// <summary>
        /// Actually "runs" the object, printing stuff on screen.
        /// If the type is Error, a GekkoException will be thrown after printing.
        /// Will call Exe2(), on the GUI thread.
        /// </summary>
        public void Exe1()
        {
            Exe1(null);
        }

        /// <summary>
        /// The new way of printing text on screen. Will eventually replace all
        /// of the G.Write() and G.Writeln() methods.
        /// </summary>
        /// <param name="w"></param>
        public void Exe2()
        {
            //When we get here, we are typically at a line that has just breaked.

            ETabs tab = ETabs.Main;
            string marginFirst = "";
            int lineWidth = Program.options.print_width;
            bool mustAlsoPrintOnScreen = false;

            Color color = Color.Empty;

            if (this.type == EWrapType.Error)
            {
                marginFirst = Globals.errorString;
                color = Color.Red;
                if (Globals.errorMemory == null) Globals.errorMemory = new StringBuilder();
            }
            else if (this.type == EWrapType.Warning)
            {
                marginFirst = Globals.warningString;
                color = Globals.warningColor;
            }
            else if (this.type == EWrapType.Note)
            {
                marginFirst = Globals.noteString;
            }
            else if (this.type == EWrapType.Writeln)
            {
                Writeln w = this as Writeln;  //used later on            
                color = w.color;
                lineWidth = w.lineWidth;
                mustAlsoPrintOnScreen = w.mustAlsoPrintToScreen;
                marginFirst = w.indent;
                tab = w.tab;
            }

            string margin = G.Blanks(marginFirst.Length);

            //-------------------------------
            //The message in main tab
            //-------------------------------
            this.ConsolidateLines("main");

            if (tab == ETabs.Output)
            {
                Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
                O.Cls("output");
            }

            if (G.IsDecompOrFindThread())
            {
                if (Globals.decompIsCalculatingButtonColors)
                {
                    //ignore the error completely, so not even a popup box
                }
                else
                {
                    MessageBoxShow();
                    if (type == EWrapType.Error && this.throwExceptionForError)
                    {
                        throw new GekkoException();
                    }
                }
            }
            else
            {

                for (int ii = 0; ii < this.storageMain.Count; ii++)
                {
                    string m = marginFirst;
                    if (ii > 0)
                    {
                        m = margin;
                        //color = Color.Empty;
                    }
                    WrapHelper(this.storageMain[ii].linesAtStart, 1, m, margin, this.storageMain[ii].consolidated, lineWidth, color, tab, this.type, mustAlsoPrintOnScreen);
                }

                if (this.storageMore[0].storage.Count > 0)
                {
                    Action<GAO> a = (gao) =>
                    {
                        //-------------------------------
                        //The long explanation in output tab
                        //-------------------------------
                        Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
                        O.Cls("output");
                        this.ConsolidateLines("more");
                        for (int ii = 0; ii < this.storageMore.Count; ii++)
                        {
                            WrapHelper(this.storageMore[ii].linesAtStart, 1, "", "", this.storageMore[ii].consolidated, lineWidth, Color.Empty, ETabs.Output, this.type, mustAlsoPrintOnScreen);
                        }
                    };

                    //---------------------------------------------------------------
                    //The link in the main tab to the explanation in the output tab
                    //---------------------------------------------------------------

                    WrapHelper(0, 2, margin, margin, "--> Info and detailed explanation " + G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ".", lineWidth, color, ETabs.Main, this.type, mustAlsoPrintOnScreen);
                }

                if (!G.IsUnitTesting()) Gui.gui.ScrollToEnd(Gui.gui.textBoxMainTabUpper); //if not, the text is not scrolled if many lines.
            }

        }

        /// <summary>
        /// Show the contents of the wrapper in a MessageBox instead of in the main GUI.
        /// </summary>
        private void MessageBoxShow()
        {
            string s1 = MetaConsolidate(this.storageMain);
            string s2= MetaConsolidate(this.storageMore);
            if (s2 != null) s1 += "\n\n" + s2;
            string s3 = s1 + "\n\nBeware of inconsistent data.";
            MessageBox.Show(s3);
        }

        /// <summary>
        /// Helper for MessageBoxShow(), assembling parts of wrapper as plain text.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private string MetaConsolidate(List<WrapHelper5> x)
        {
            string s1 = null;
            foreach (WrapHelper5 w in x)
            {
                if (w.consolidated != null) s1 += w.consolidated.Trim() + "\n\n";
            }
            if (s1 == null)
            {
            }
            else
            {
                s1 = s1.Trim();
                if (!s1.EndsWith(".")) s1 += ".";
            }
            return s1;
        }

        /// <summary>
        /// Helper method to consolidate List&lt;string&gt; lines in a section into one string. Handles blanks etc. too so that it is pretty.
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        private void ConsolidateLines(string type)
        {
            List<WrapHelper5> ss = null;
            if (type == "main") ss = this.storageMain;
            else if (type == "more") ss = this.storageMore;
            else throw new GekkoException();

            foreach (WrapHelper5 xx in ss)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < xx.storage.Count; i++)
                {
                    string add = "";
                    bool remove = false;
                    if (i > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(xx.storage[i].Trim());
                }
                xx.consolidated = sb.ToString();
            }
        }


        /// <summary>
        /// Helper for Wrap(). Note: linesAtStart == 0 similar to G.Writeln(), and lines == 1 similar to 
        /// G.Writeln2(). Note: linesAtEnd = 0 similar to G.Write(). So if calling with WrapHelper(1, 1, ...),
        /// it works similarly to a G.Writeln2().
        /// </summary>
        /// <param name="linesAtStart">blank lines</param>
        /// /// <param name="linesAtEnd">blank lines</param>
        /// <param name="marginFirst">For instance "*** ERROR: "</param>
        /// <param name="margin">For instance "           ", blanks corresponding to "*** ERROR: "</param>
        /// <param name="s">Text to show</param>
        /// <param name="isPiping">Pipe to file?</param>
        /// <param name="color">Text color (can be Color.Empty)</param>
        /// <param name="tab">Which tab to print it?</param>
        private static void WrapHelper(int linesAtStart, int linesAtEnd, string marginFirst, string margin, string s, int lineWidth, Color color, ETabs tab, EWrapType type, bool mustAlsoPrintOnScreen)
        {
            if (s.Trim() == "") return;

            if (Globals.runningOnTTComputer && s.Trim().StartsWith("TTH: ")) color = Color.Gray;

            int i1 = 0;
            List<TwoInts> links = new List<TwoInts>();
            while (true)
            {
                i1 = s.IndexOf(Globals.linkActionStart, i1);  //linkActionDelimiter
                if (i1 == -1) break;
                int i2 = s.IndexOf(Globals.linkActionEnd, i1 + 1);
                if (i2 == -1) break;  //strange
                links.Add(new TwoInts(i1, i2));
                i1 = i2 + 1;
            }

            int col = 0;

            int textLengthStart = -12345;  //used to set color at the very end    
            RichTextBoxEx textBox = null;
            if (!G.IsUnitTesting())
            {
                if (tab == ETabs.Output) textBox = Gui.gui.textBoxOutputTab;
                else textBox = Gui.gui.textBoxMainTabUpper;
                textLengthStart = textBox.TextLength;
            }

            string nl = "";
            for (int i = 0; i < linesAtStart; i++)
            {
                nl += G.NL;
            }

            G.PrintLowLevelAppendText(textBox, nl + marginFirst, type, mustAlsoPrintOnScreen);

            col = margin.Length;

            for (int i = 0; i < links.Count; i++)
            {
                // ........ {a{ ..link1.... }a} ........... {a{ ...link2.... }a} ..........

                int lastC = 0;
                if (i > 0) lastC = links[i - 1].int2 + Globals.linkActionEnd.Length;
                string normalText = G.Substring(s, lastC, links[i].int1 - 1);
                if (true)
                {
                    col = WrapText(textBox, normalText, margin, col, lineWidth, color, type, mustAlsoPrintOnScreen);
                }
                string[] ss = G.Substring(s, links[i].int1 + Globals.linkActionStart.Length, links[i].int2 - 1).Split(Globals.linkActionDelimiter);  //delimiter must be there
                string linkText = ss[0];
                string linkLink = "action:" + ss[1];
                if (col + linkText.Length > lineWidth)
                {
                    //insert a line break no matter what the character before is. Link cannot be broken/wrapped                        
                    G.PrintLowLevelAppendText(textBox, G.NL + margin, type, mustAlsoPrintOnScreen);
                    col = margin.Length;
                }

                G.PrintLowLevelAppendTextAbstract(textBox, linkText, linkLink, type, mustAlsoPrintOnScreen);
                col += linkText.Length;

                if (i == links.Count - 1)
                {
                    //get the last bit
                    string normalText2 = G.Substring(s, links[i].int2 + Globals.linkActionEnd.Length, s.Length - 1);
                    if (normalText2 == null) normalText2 = "";  //can be null
                    col = WrapText(textBox, normalText2, margin, col, lineWidth, color, type, mustAlsoPrintOnScreen);
                }
            }

            if (links.Count == 0)
            {
                WrapText(textBox, s, margin, col, lineWidth, color, type, mustAlsoPrintOnScreen);
            }

            //Always insert a newline now, we are not doing the equivalent to Write().

            string nl2 = "";
            for (int i = 0; i < linesAtEnd; i++)
            {
                nl2 += G.NL;
            }
            if (nl2 != "") G.PrintLowLevelAppendText(textBox, nl2, type, mustAlsoPrintOnScreen);
            
            if (color != Color.Empty && textBox != null && textLengthStart != -12345 && textBox.TextLength - textLengthStart > 0)
            {
                G.PrintLowLevelSetColor(textBox, textLengthStart, color);
            }
        }
        
        private static int WrapText(RichTextBoxEx textBox, string text, string margin, int colCounter, int lineWidth, Color color, EWrapType type, bool mustAlsoPrintOnScreen)
        {
            while (true)
            {
                if (colCounter + text.Length > lineWidth)
                {
                    //          |mmmmm..............................|
                    //          |mmmmm..................this is a really long line
                    //
                    // becomes
                    //          |mmmmm..............................|
                    //          |mmmmm..................this is a
                    //          |mmmmmreally long line
                    //
                    // where . are some chars, and m is left-margin (blanks).
                    // so when right margin is exceeded, we find a suitable blank to break on (after it)

                    //find best wrap, even if it wraps outside of the margin

                    int bestWrapI = -12345;
                    for (int ii = 0; ii < text.Length; ii++)
                    {
                        if (text[ii] == ' ')
                        {
                            if (colCounter + ii > lineWidth)
                            {
                                if (bestWrapI != -12345) break;  //we found a wrap inside margin, else carry on
                            }
                            bestWrapI = ii;
                        }
                    }

                    if (bestWrapI == -12345)
                    {
                        //no wrap found: long line without any blanks. Just wrap at 0
                        bestWrapI = 0;
                    }

                    string s1 = G.Substring(text, 0, bestWrapI);
                    text = G.Substring(text, bestWrapI + 1, text.Length - 1);
                    G.PrintLowLevelAppendText(textBox, s1 + G.NL + margin, type, mustAlsoPrintOnScreen);
                    colCounter = margin.Length;
                }
                else
                {
                    //easy, there is room for the text   
                    G.PrintLowLevelAppendText(textBox, text, type, mustAlsoPrintOnScreen);
                    colCounter += text.Length;
                    break;  //the end
                }
            }
            return colCounter;
        }
    }


    /// <summary>
    /// Inherits from Wrap class. For easier syntax when constructing.
    /// </summary>
    public class Error : Wrap
    {
        //public bool showImmediatelyAndDoNotThrowException = false;

        /// <summary>
        /// Object of Wrap type
        /// </summary>
        public Error() : base(EWrapType.Error)
        {
        }

        /// <summary>
        /// Usage: new Error("Error in ...");
        /// </summary>
        /// <param name="s"></param>
        public Error(string s) : base(EWrapType.Error)
        {
            this.MainAdd(s);
            this.Exe1();
        }

        /// <summary>
        /// Usage: new Error("Error in ...", e);
        /// </summary>
        /// <param name="s"></param>
        public Error(string s, Exception e) : base(EWrapType.Error)
        {
            this.MainAdd(s);
            this.Exe1(e);
        }

        /// <summary>
        /// Usage: new Error("Error in ...");. If throwExceptionForError == false, an exception will not 
        /// be thrown (which it normally will for the Error object). Setting throwExceptionForError = false is
        /// used a few places when failing, but should not normally be used.
        /// </summary>
        /// <param name="s"></param>
        public Error(string s, bool throwExceptionForError) : base(EWrapType.Error)
        {
            this.MainAdd(s);
            if (throwExceptionForError == false) this.ThrowNoException();
            this.Exe1();
        }
    }

    /// <summary>
    /// Inherits from Wrap class. For easier syntax when constructing.
    /// </summary>
    public class Warning : Wrap
    {
        /// <summary>
        /// Object of Wrap type
        /// </summary>
        public Warning() : base(EWrapType.Warning)
        {
        }

        /// <summary>
        /// Do not assign to anything. Usage: new Warning("Beware...");
        /// </summary>
        /// <param name="s"></param>
        public Warning(string s) : base(EWrapType.Warning)
        {
            this.MainAdd(s);
            this.Exe1();
        }
    }

    /// <summary>
    /// Inherits from Wrap class. For easier syntax when constructing.
    /// </summary>
    public class Note : Wrap
    {
        /// <summary>
        /// Object of Wrap type
        /// </summary>
        public Note() : base(EWrapType.Note)
        {
        }

        /// <summary>
        /// Do not assign to anything. Usage: new Note("Beware...");
        /// </summary>
        /// <param name="s"></param>
        public Note(string s) : base(EWrapType.Note)
        {
            this.MainAdd(s);
            this.Exe1();
        }
    }

    /// <summary>
    /// Inherits from Wrap class. For easier syntax when constructing. Use: new Writeln("..."); instead
    /// of: G.Writeln("...");
    /// </summary>
    public class Writeln : Wrap
    {
        public ETabs tab = ETabs.Main;  //main or ouput
        public string indent = "";
        public Color color = Color.Empty;
        public int lineWidth = Program.options.print_width;  //-12345 for null, int.MaxValue for no limits
        public bool mustAlsoPrintToScreen = false;  //is always activated for Error() or Warning() types.
        
        /// <summary>
        /// Usage only via "using": using(var w = new Writeln()) {... w.MainAdd(...) ...}
        /// </summary>
        public Writeln() : base(EWrapType.Writeln)
        {
        }

        /// <summary>
        /// Usage only via "using": using(var w = new Writeln(130, Color.Gray)) {... w.MainAdd(...) ...}
        /// </summary>
        /// <param name="indent">"  " for indent, "* " for bullet point, etc.</param>
        /// <param name="lineWidth">Use -12345 to indicate null, or int.MaxValue for no limits</param>
        /// <param name="color">use Color.Empty to indicate null</param>
        /// <param name="mustAlsoWriteToScreen">true if it will print to screen even when piping</param>
        public Writeln(string indent, int lineWidth, Color color, bool mustAlsoWriteToScreen, ETabs tab) : base(EWrapType.Writeln)
        {
            Helper(indent, lineWidth, color, mustAlsoWriteToScreen, tab);
        }

        /// <summary>
        /// Usage via "new": new Writeln("Hello hello...");
        /// </summary>
        /// <param name="s"></param>
        public Writeln(string s) : base(EWrapType.Writeln)
        {
            this.MainAdd(s);
            this.Exe1();
        }

        /// <summary>
        /// Usage via "new": new Writeln("Hello hello", 130, Color.Gray). Use -12345 for lineWidth to indicate null or int.MaxValue for no limits,
        /// or Color.Empty for color to indicate null.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lineWidth">Use -12345 to indicate null, , or int.MaxValue for no limits</param>
        /// <param name="color">use Color.Empty to indicate null</param>
        public Writeln(string s, string indent, int lineWidth, Color color, bool mustAlsoWriteToScreen, ETabs tab) : base(EWrapType.Writeln)
        {
            Helper(indent, lineWidth, color, mustAlsoWriteToScreen, tab);
            this.MainAdd(s);
            this.Exe1();
        }        

        private void Helper(string indent, int lineWidth, Color color, bool mustAlsoWriteToScreen, ETabs tab)
        {
            if (lineWidth != -12345) this.lineWidth = lineWidth;
            if (color != Color.Empty) this.color = color;
            if (!string.IsNullOrEmpty(indent)) this.indent = indent;
            this.tab = tab;
            this.mustAlsoPrintToScreen = mustAlsoWriteToScreen;
        }
    }    
}


