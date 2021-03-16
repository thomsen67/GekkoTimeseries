using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Gekko
{
    /// <summary>
    /// Essentially a List&lt;string&gt;
    /// </summary>
    public class WrapHelper5
    {
        public int linesAtStart = 1;
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
        public void MainNewLines()
        {
            this.storageMain.Add(new WrapHelper5());
        }

        /// <summary>
        /// Add a section break to "main". No blank line in between.
        /// </summary>
        public void MainNewLine()
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
        public void MoreNewLines()
        {
            this.storageMore.Add(new WrapHelper5());
        }

        /// <summary>
        /// Add a section break to "more". No blank line in between.
        /// </summary>
        public void MoreNewLine()
        {            
            this.storageMore.Add(new WrapHelper5(0));
        }        

        /// <summary>
        /// Actually "runs" the object, printing stuff on screen.
        /// If the type is Error, a GekkoException will be thrown after printing.
        /// Will call Exe2(), on the GUI thread.
        /// </summary>
        public void Exe1()
        {
            CrossThreadStuff.Wrap(this);  //calls G.Wrap(), see #klsdjsdklgj9
            if (type == EWrapType.Error)
            {
                Globals.numberOfErrors++;
                throw new GekkoException();                
            }
            else if (type == EWrapType.Warning)
            {                
                Globals.numberOfWarnings++;
            }
        }

        /// <summary>
        /// The new way of printing text on screen. Will eventually replace all
        /// of the G.Write() and G.Writeln() methods.
        /// </summary>
        /// <param name="w"></param>
        /// #klsdjsdklgj9
        public void Exe2()
        {
            //When we get here, we are typically at a line that has just breaked.            

            string marginFirst = "";
            Color color = Color.Empty;
            bool mustAlsoPrintOnScreen = false;

            if (this.type == EWrapType.Error)
            {
                marginFirst = Globals.errorString;
                color = Color.Red;
                mustAlsoPrintOnScreen = true;  //so we get an error on screen even if piping or muting
                if (Globals.errorMemory == null) Globals.errorMemory = new StringBuilder();
            }
            else if (this.type == EWrapType.Warning)
            {
                marginFirst = Globals.warningString;
                color = Globals.warningColor;
                mustAlsoPrintOnScreen = true;
            }
            else if (this.type == EWrapType.Note)
            {
                marginFirst = Globals.noteString;
            }
            else if (this.type == EWrapType.Writeln)
            {
                marginFirst = "";  //just stating the obvious
            }
            
            string margin = G.Blanks(marginFirst.Length);

            //-------------------------------
            //The short message in main tab
            //-------------------------------
            this.ConsolidateLines("main");
            for (int ii = 0; ii < this.storageMain.Count; ii++)
            {
                string m = marginFirst;
                if (ii > 0)
                {
                    m = margin;
                    color = Color.Empty;
                }
                WrapHelper(this.storageMain[ii].linesAtStart, 1, m, margin, this.storageMain[ii].consolidated, color, ETabs.Main, this.type);
            }

            if (this.storageMore[0].storage.Count > 0)
            {

                Action a = () =>
                {
                    //-------------------------------
                    //The long explanation in output tab
                    //-------------------------------
                    Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
                    O.Cls("output");
                    this.ConsolidateLines("more");
                    for (int ii = 0; ii < this.storageMain.Count; ii++)
                    {
                        WrapHelper(this.storageMore[ii].linesAtStart, 1, "", "", this.storageMore[ii].consolidated, Color.Empty, ETabs.Output, this.type);
                    }
                };

                //---------------------------------------------------------------
                //The link in the main tab to the explanation in the output tab
                //---------------------------------------------------------------

                WrapHelper(1, 1, margin, margin, "Detailed explanation " + G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ".", Color.Empty, ETabs.Main, this.type);
            }

            Gui.gui.ScrollToEnd(Gui.gui.textBoxMainTabUpper);  //if not, the text is not scrolled if many lines.
            
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
        private static void WrapHelper(int linesAtStart, int linesAtEnd, string marginFirst, string margin, string s, Color color, ETabs tab, EWrapType type)
        {
            if (s.Trim() == "") return;

            int i1 = 0;
            List<TwoInts> links = new List<TwoInts>();
            while (true)
            {
                i1 = s.IndexOf(Globals.linkActionStart, i1);  //linkActionDelimiter
                if (i1 == -1) break;
                int i2 = s.IndexOf(Globals.linkActionEnd, i1 + 1);
                if (i2 == -1) break;  //strange
                links.Add(new TwoInts() { int1 = i1, int2 = i2 });
                i1 = i2 + 1;
            }

            int textLengthStart = Gui.gui.textBoxMainTabUpper.TextLength;
            int col = 0;
            int colMax = Program.options.print_width;            

            RichTextBoxEx textBox = Gui.gui.textBoxMainTabUpper;
            if (tab == ETabs.Output) textBox = Gui.gui.textBoxOutputTab;

            string nl = "";
            for (int i = 0; i < linesAtStart; i++)
            {
                nl += G.NL;
            }

            G.AppendText(textBox, nl + marginFirst, type);

            col = margin.Length;

            for (int i = 0; i < links.Count; i++)
            {
                // ........ {a{ ..link1.... }a} ........... {a{ ...link2.... }a} ..........

                int lastC = 0;
                if (i > 0) lastC = links[i - 1].int2 + Globals.linkActionEnd.Length;
                string normalText = G.Substring(s, lastC, links[i].int1 - 1);
                if (true)
                {
                    col = WrapText(textBox, normalText, margin, col, colMax, color, type);
                }
                string[] ss = G.Substring(s, links[i].int1 + Globals.linkActionStart.Length, links[i].int2 - 1).Split(Globals.linkActionDelimiter);  //delimiter must be there
                string linkText = ss[0];
                string linkLink = "action:" + ss[1];
                if (col + linkText.Length > colMax)
                {
                    //insert a line break no matter what the character before is. Link cannot be broken/wrapped                        
                    G.AppendText(textBox, G.NL + margin, type);
                    col = margin.Length;
                }

                G.AppendLink(textBox, linkText, linkLink, type);
                col += linkText.Length;

                if (i == links.Count - 1)
                {
                    //get the last bit
                    string normalText2 = G.Substring(s, links[i].int2 + Globals.linkActionEnd.Length, s.Length - 1);
                    col = WrapText(textBox, normalText2, margin, col, colMax, color, type);
                }
            }

            if (links.Count == 0)
            {
                WrapText(textBox, s, margin, col, colMax, color, type);
            }

            //Always insert a newline now, we are not doing the equivalent to Write().


            string nl2 = "";
            for (int i = 0; i < linesAtEnd; i++)
            {
                nl2 += G.NL;
            }
            if (nl2 != "") G.AppendText(textBox, nl2, type);

            if (color != Color.Empty)
            {
                textBox.Select(textLengthStart, textBox.TextLength);
                textBox.SelectionColor = color;
            }
        }

        private static int WrapText(RichTextBoxEx textBox, string text, string margin, int colCounter, int colMax, Color color, EWrapType type)
        {
            while (true)
            {
                if (colCounter + text.Length > colMax)
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
                            if (colCounter + ii > colMax)
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
                    G.AppendText(textBox, s1 + G.NL + margin, type);
                    colCounter = margin.Length;
                }
                else
                {
                    //easy, there is room for the text   
                    G.AppendText(textBox, text, type);
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
        /// <summary>
        /// Object of Wrap type
        /// </summary>
        public Error() : base(EWrapType.Error)
        {
        }

        /// <summary>
        /// Do not assign to anything. Usage: new Error("Error in ...");
        /// </summary>
        /// <param name="s"></param>
        public Error(string s) : base(EWrapType.Error)
        {
            this.MainAdd(s);
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
        /// <summary>
        /// Object of Wrap type
        /// </summary>
        public Writeln() : base(EWrapType.Writeln)
        {
        }

        /// <summary>
        /// Do not assign to anything. Usage: new Note("Beware...");
        /// </summary>
        /// <param name="s"></param>
        public Writeln(string s) : base(EWrapType.Writeln)
        {
            this.MainAdd(s);
            this.Exe1();
        }
    }
}


