using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace Gekko
{
    public static class Plot
    {
        public static string CallGnuplot(PlotTable plotTable, O.Prt o, List<O.Prt.Element> containerExplode, EFreq highestFreq, bool showWindow, P p)
        {
            //Måske en SYS gnuplot til at starte et vindue op.
            //See #23475432985 regarding options that default = no, and are activated with empty node like <boxstack/>

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            string extension = "emf";
            if (o.opt_filename != null)
            {
                extension = Path.GetExtension(o.opt_filename);
                if (extension.StartsWith(".")) extension = extension.Substring(1);
                if (extension == "")
                {
                    o.opt_filename = G.AddExtension(o.opt_filename, ".emf");
                    extension = "emf";
                }
                if (!G.Equal(extension, "emf") && !G.Equal(extension, "png") && !G.Equal(extension, "svg") && !G.Equal(extension, "pdf"))
                {
                    new Error("In PLOT, expected file type is emf, png, svg or pdf");
                    //throw new GekkoException();
                }
                extension = extension.ToLower().Trim();  //gnuplot does not like upper-case file types
            }

            //bool isInside = true;
            //bool test2 = false;
            int count = containerExplode.Count;
            bool firstXLabelFix = true;

            bool isInside = false;  //corresponds to at
            if (highestFreq == EFreq.A || highestFreq == EFreq.U)
            {
                //annual
                if (G.Equal(Program.options.plot_xlabels_annual, "between")) isInside = true;
            }
            else
            {
                //nonannual
                if (G.Equal(Program.options.plot_xlabels_nonannual, "between")) isInside = true;
            }

            //https://groups.google.com/forum/#!topic/comp.graphics.apps.gnuplot/csbgSFAbIv4

            double zoom = 1d;
            double fontfactor = 1d;

            //make as wpf window, detect dpi on screen at set size accordingly (http://stackoverflow.com/questions/5977445/how-to-get-windows-display-settings)

            if (count == 0)
            {
                new Error("PLOT called with 0 variables");
            }
            int numberOfObs = GekkoTime.Observations(o.t1, o.t2);
            int rr = Program.RandomInt();
            string file1 = "temp" + rr + ".dat";
            string file2 = "temp" + rr + "." + extension;
            string file3 = "temp" + rr + ".gp";
            string heading = "";
            string pplotType = "emf";

            XmlDocument doc = new XmlDocument();

            if (o.opt_using != null || Program.options.plot_using != "")
            {
                XmlDocument doc1 = null;
                XmlDocument doc2 = null;

                if (Program.options.plot_using != "")
                {
                    string fileName = Program.options.plot_using;
                    fileName = G.AddExtension(fileName, "." + Globals.extensionPlot);
                    FindFileHelper ffh = Program.FindFile(fileName, null, true, true, p);
                    fileName = ffh.realPathAndFileName;
                    if (fileName == null) new Error("The file does not exist: " + ffh.prettyPathAndFileName);

                    doc1 = new XmlDocument();
                    string xmlText = Program.GetTextFromFileWithWait(fileName);

                    try
                    {
                        doc1.LoadXml(xmlText);
                    }
                    catch (Exception e)
                    {
                        new Error("Plot template file: '" + fileName + "'. " + Program.GetXmlError(e, fileName));
                    }
                }

                if (o.opt_using != null)
                {

                    string fileName = o.opt_using;
                    bool cancel = false;
                    if (fileName == "*")
                    {
                        Program.SelectFile(Globals.extensionPlot, ref fileName, ref cancel);
                    }
                    if (cancel) return null;

                    fileName = G.AddExtension(fileName, "." + Globals.extensionPlot);
                    FindFileHelper ffh = Program.FindFile(fileName, null, true, true, p);
                    fileName = ffh.realPathAndFileName;
                    if (fileName == null) new Error("The file does not exist: " + ffh.prettyPathAndFileName);

                    doc2 = new XmlDocument();
                    string xmlText = Program.GetTextFromFileWithWait(fileName);

                    try
                    {
                        doc2.LoadXml(xmlText);
                    }
                    catch (Exception e)
                    {
                        new Error("Plot template file: '" + fileName + "'. " + Program.GetXmlError(e, fileName));
                    }
                }

                if (doc1 != null)
                {
                    //global template

                    if (doc2 != null)
                    {
                        //doc1 = x1, doc2 = x2
                        //XmlNode temp = doc1.ImportNode(doc2, true) as XmlDocument;  --> hmmm does not work, maybe just do it in code, not as a xml merge
                        doc = doc2;
                    }
                    else
                    {
                        //doc1 = x1, doc2 = null
                        doc = doc1;
                    }
                }
                else
                {
                    if (doc2 != null)
                    {
                        //doc1 = null, doc2 = x2
                        doc = doc2;
                    }
                    else
                    {
                        //doc1 = null, doc2 = null
                        //do nothing, doc will be unchanged (empty)
                    }
                }
            }

            string currentDir = Directory.GetCurrentDirectory();  //remembered in order to switch back
            string path = Globals.localTempFilesLocationGnuplot + "\\tempfiles";
            // Determine whether the directory exists.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Directory.SetCurrentDirectory(path); //so that gnuplot can access the files

            List<string> labels1 = new List<string>();
            List<string> labels2 = new List<string>();
            string fileGp = path + "\\" + file3;

            string fileData = path + "\\" + file1;

            //List<string> tabLines = data.Print();

            //structure of plotTable is (0 has 6 points, 1 has 3, 2 has 4).
            //                dates[0]   dates[1]   dates[2]     values[0]   values[1]   values[2]
            // i = 0             .          .         .              .          .            .
            // i = 1             .          .         .              .          .            .
            // i = 2             .          .         .              .          .            .
            // i = 3             .                    .              .                       .
            // i = 4             .                                   .                        

            int max = int.MinValue;
            for (int j = 0; j < plotTable.dates.Count; j++)
            {
                max = Math.Max(max, plotTable.dates[j].Count);
            }

            using (FileStream fs = Program.WaitForFileStream(fileData, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter tw = G.GekkoStreamWriter(fs))
            {
                //TODO:
                //In the middle of mixed freq data, there can be a lot of lines like these. Perhaps detect and remove.
                //Perhaps same problem with smpl set too wide on non-mixed freqs.
                //Also, daily data points could be aggregated with min/max for a range on the x axis, if this range corresponds
                //to < 1 pixel on the screen. Still, gnuplot is pretty fast at plotting.
                //1984.44861 M M M M M M M
                //1984.45139 M M M M M M M
                //1984.45417 M M M M M M M
                //1984.45694 M M M M M M M
                //1984.45972 M M M M M M M

                for (int i = 0; i < max; i++)
                {
                    //foreach (string s in tabLines) tw.WriteLine(s);                         
                    for (int j = 0; j < plotTable.dates.Count; j++)
                    {
                        if (i < plotTable.dates[j].Count)
                        {
                            tw.Write(string.Format("{0:0.#####}", plotTable.dates[j][i]) + " "); //width of 1 day is about 0.003 of a year. So 0.00001 is precise compared to that
                        }
                        else
                        {
                            tw.Write("M ");
                        }
                    }
                    for (int j = 0; j < plotTable.values.Count; j++)
                    {
                        if (i < plotTable.values[j].Count && !G.isNumericalError(plotTable.values[j][i]))
                        {
                            tw.Write(plotTable.values[j][i].ToString() + " ");
                        }
                        else
                        {
                            tw.Write("M ");
                        }
                    }
                    tw.WriteLine();
                }
            }

            XmlNodeList lines3 = doc.SelectNodes("gekkoplot/lines/line");

            // ---------------------------------------------
            // --------- loading main section start
            // ---------------------------------------------

            string plotcode1 = GetText(null, null, null, doc.SelectSingleNode("gekkoplot/plotcode"), null);
            string plotcode2 = o.opt_plotcode;
            string plotcode = null;
            if (!string.IsNullOrEmpty(plotcode1) && !string.IsNullOrEmpty(plotcode2))
            {
                plotcode = plotcode1 + "; " + plotcode2;
            }
            else
            {
                plotcode = plotcode1 + plotcode2;  //result may be empty string
            }

            string size2 = GetText(null, o.opt_size, null, doc.SelectSingleNode("gekkoplot/size"), null);
            string title = GetText(null, o.opt_title, null, doc.SelectSingleNode("gekkoplot/title"), null);
            string subtitle = GetText(null, o.opt_subtitle, null, doc.SelectSingleNode("gekkoplot/subtitle"), null);
            string font = GetText(null, o.opt_font, null, doc.SelectSingleNode("gekkoplot/font"), "Verdana");
            double fontsize = Program.ParseIntoDouble(GetText(null, G.isNumericalError(o.opt_fontsize) ? null : o.opt_fontsize.ToString(), null, doc.SelectSingleNode("gekkoplot/fontsize"), "12"));
            string bold = GetText(null, o.opt_bold, null, doc.SelectSingleNode("gekkoplot/bold"), null);
            string italic = GetText(null, o.opt_italic, null, doc.SelectSingleNode("gekkoplot/italic"), null);
            string ticsInOut = GetText(null, o.opt_tics, null, doc.SelectSingleNode("gekkoplot/tics"), "out");
            string grid = GetText(null, o.opt_grid, null, doc.SelectSingleNode("gekkoplot/grid"), "yes");  //normally null or "" --> grid. Switch off with <grid>no</grid>                        
            string gridstyle = GetText(null, o.opt_gridstyle, null, doc.SelectSingleNode("gekkoplot/gridstyle"), "linecolor rgb \"#d3d3d3\" dashtype 3 linewidth 1.5");
            string key = GetText(null, o.opt_key, null, doc.SelectSingleNode("gekkoplot/key"), "out horiz bot center Left reverse height 1");  //height 1 givers nicer vertical spacing
            string palette = GetText(null, o.opt_palette, null, doc.SelectSingleNode("gekkoplot/palette"), "red,web-green,web-blue,orange,dark-blue,magenta,brown4,dark-violet,grey50,black");
            string stack = GetText(null, o.opt_stack, null, doc.SelectSingleNode("gekkoplot/stack"), "no");  //default: no, #23475432985    
            double boxwidth = Program.ParseIntoDouble(GetText(null, G.isNumericalError(o.opt_boxwidth) ? null : o.opt_boxwidth.ToString(), null, doc.SelectSingleNode("gekkoplot/boxwidth"), "0.75"));
            string boxgap = GetText(null, G.isNumericalError(o.opt_boxgap) ? null : o.opt_boxgap.ToString(), null, doc.SelectSingleNode("gekkoplot/boxgap"), "2");
            string separate = GetText(null, o.opt_separate, null, doc.SelectSingleNode("gekkoplot/separate"), "no"); //default: no, #23475432985                        

            List<string> xlines = GetText(doc.SelectNodes("gekkoplot/xline"));
            if (!o.opt_xline.IsNull()) xlines.Add(o.opt_xline.ToString());
            List<string> xlinebefores = GetText(doc.SelectNodes("gekkoplot/xlinebefore"));
            if (!o.opt_xlinebefore.IsNull()) xlinebefores.Add(o.opt_xlinebefore.ToString());
            List<string> xlineafters = GetText(doc.SelectNodes("gekkoplot/xlineafter"));
            if (!o.opt_xlineafter.IsNull()) xlineafters.Add(o.opt_xlineafter.ToString());

            string ymirror = GetText(null, o.opt_ymirror, null, doc.SelectSingleNode("gekkoplot/ymirror"), "0"); //y2 mirror could be either no (0), tics (1), tics+labels (2), tics+labels+axislabel (3). With grid set, the mirror is not so important.
            string ytitle = GetText(null, o.opt_ytitle, null, doc.SelectSingleNode("gekkoplot/ytitle"), null);
            string y2title = GetText(null, o.opt_y2title, null, doc.SelectSingleNode("gekkoplot/y2title"), null);
            List<string> ylines = GetText(doc.SelectNodes("gekkoplot/yline"));
            if (!G.isNumericalError(o.opt_yline)) ylines.Add(o.opt_yline.ToString());
            List<string> y2lines = GetText(doc.SelectNodes("gekkoplot/y2line"));
            if (!G.isNumericalError(o.opt_y2line)) y2lines.Add(o.opt_y2line.ToString());

            string ymax = GetText(null, o.opt_ymax.ToString(), null, doc.SelectSingleNode("gekkoplot/ymax"), null);
            string ymaxsoft = GetText(null, o.opt_ymaxsoft.ToString(), null, doc.SelectSingleNode("gekkoplot/ymaxsoft"), null);
            string ymaxhard = GetText(null, o.opt_ymaxhard.ToString(), null, doc.SelectSingleNode("gekkoplot/ymaxhard"), null);
            string y2max = GetText(null, o.opt_y2max.ToString(), null, doc.SelectSingleNode("gekkoplot/y2max"), null);
            string y2maxsoft = GetText(null, o.opt_y2maxsoft.ToString(), null, doc.SelectSingleNode("gekkoplot/y2maxsoft"), null);
            string y2maxhard = GetText(null, o.opt_y2maxhard.ToString(), null, doc.SelectSingleNode("gekkoplot/y2maxhard"), null);

            string ymin = GetText(null, o.opt_ymin.ToString(), null, doc.SelectSingleNode("gekkoplot/ymin"), null);
            string yminsoft = GetText(null, o.opt_yminsoft.ToString(), null, doc.SelectSingleNode("gekkoplot/yminsoft"), null);
            string yminhard = GetText(null, o.opt_yminhard.ToString(), null, doc.SelectSingleNode("gekkoplot/yminhard"), null);
            string y2min = GetText(null, o.opt_y2min.ToString(), null, doc.SelectSingleNode("gekkoplot/y2min"), null);
            string y2minsoft = GetText(null, o.opt_y2minsoft.ToString(), null, doc.SelectSingleNode("gekkoplot/y2minsoft"), null);
            string y2minhard = GetText(null, o.opt_y2minhard.ToString(), null, doc.SelectSingleNode("gekkoplot/y2minhard"), null);

            string xzeroaxis = GetText(null, o.opt_xzeroaxis, null, doc.SelectSingleNode("gekkoplot/xzeroaxis"), "yes");
            string x2zeroaxis = GetText(null, o.opt_x2zeroaxis, null, doc.SelectSingleNode("gekkoplot/x2zeroaxis"), "no"); //default: no, #23475432985 

            //the options in <lines> may override this.
            XmlNode linetypeMain = doc.SelectSingleNode("gekkoplot/type");
            XmlNode dashtypeMain = doc.SelectSingleNode("gekkoplot/dashtype");
            XmlNode linewidthMain = doc.SelectSingleNode("gekkoplot/linewidth");
            XmlNode linecolorMain = doc.SelectSingleNode("gekkoplot/linecolor");
            XmlNode pointtypeMain = doc.SelectSingleNode("gekkoplot/pointtype");
            XmlNode pointsizeMain = doc.SelectSingleNode("gekkoplot/pointsize");
            XmlNode fillstyleMain = doc.SelectSingleNode("gekkoplot/fillstyle");

            List<string> labels = GetText(doc.SelectNodes("gekkoplot/label"));
            List<string> arrows = GetText(doc.SelectNodes("gekkoplot/arrows"));

            // ---------------------------------------------
            // --------- loading main section end
            // ---------------------------------------------

            bool stacked = false;
            if (NotNullAndNotNo(stack)) stacked = true; //#23475432985

            List<string> palette2 = null;
            if (palette != null) palette2 = new List<string>(palette.Split(','));
            if (palette2 == null || palette2.Count == 0)
            {
                //this should not be possible, but in any case...
                new Error("PLOT gpt palette is empty");
            }

            bool isSeparated = NotNullAndNotNo(separate);  //#23475432985

            double linewidthCorrection = 1d;
            double pointsizeCorrection = 1d;
            if (G.Equal(extension, "svg") || G.Equal(extension, "png"))
            {
                linewidthCorrection = 2d / 3d;
                pointsizeCorrection = 0.8d / 0.5d;
            }
            else if (G.Equal(extension, "pdf"))
            {
                linewidthCorrection = 2d / 3d;
                pointsizeCorrection = 2d / 3d;
            }

            List<int> boxesY = new List<int>();
            List<int> boxesY2 = new List<int>();
            List<int> areasY = new List<int>();
            List<int> areasY2 = new List<int>();

            int numberOfY2s = 0; //not used in first pass, but gathered
            double histoGap = double.NaN;  //not used in first pass
            double d_width = double.NaN;  //not used in first pass
            double d_width2 = double.NaN;  //not used in first pass
            double d_width3 = double.NaN;  //not used in first pass
            double left = double.NaN;  //not used in first pass
            double[] minMax = new double[6]; minMax[0] = double.MaxValue; minMax[1] = double.MinValue; minMax[2] = double.MaxValue; minMax[3] = double.MinValue; minMax[4] = double.MaxValue; minMax[5] = double.MinValue;

            // ---------------------------------------
            // ---------------------------------------
            //          FIRST PASS
            //       first pass just counts
            //       boxes, filledcurves, and
            //       a few other things.
            // ---------------------------------------
            // ---------------------------------------

            List<string> labelsNonBroken = new List<string>();
            for (int j = 0; j < count; j++)
            {
                string label = "";
                if (containerExplode[j].labelOLD[0] != null) label = containerExplode[j].labelOLD[0];
                labelsNonBroken.Add(label);
            }

            double[] dataMin = new double[containerExplode.Count];
            double[] dataMax = new double[containerExplode.Count];
            //List<string> labelsNonBroken = new List<string>();
            for (int j = 0; j < count; j++)
            {
                double min2 = double.MaxValue;
                double max2 = double.MinValue;
                foreach (double d in plotTable.values[j])
                {
                    if (!G.isNumericalError(d))
                    {
                        min2 = Math.Min(min2, d);
                        max2 = Math.Max(max2, d);
                    }
                }

                dataMin[j] = min2;
                dataMax[j] = max2;
            }

            string discard = PlotHandleLines(true, ref numberOfY2s, minMax, dataMin, dataMax, o, count, labelsNonBroken, file1, lines3, boxesY, boxesY2, areasY, areasY2, linetypeMain, dashtypeMain, linewidthMain, linecolorMain, pointtypeMain, pointsizeMain, fillstyleMain, stacked, palette2, isSeparated, d_width, d_width2, d_width3, left, containerExplode, linewidthCorrection, pointsizeCorrection, isInside, highestFreq);

            StringBuilder txt = new StringBuilder();

            txt.AppendLine("set size " + zoom + "," + zoom + "");
            txt.AppendLine("set encoding iso_8859_1");
            txt.AppendLine("set format y " + Globals.QT + "%g" + Globals.QT);  //uses for instance 1.65e+006, not trying to put uppercase exponent which fails in emf terminal
            txt.AppendLine("set format y2 " + Globals.QT + "%g" + Globals.QT);  //uses for instance 1.65e+006, not trying to put uppercase exponent which fails in emf terminal
            txt.AppendLine("set datafile missing \"NaN\"");

            int ii = 0;
            foreach (string s in key.ToLower().Split(' '))
            {
                if (s.StartsWith("out")) ii++;
                if (s.StartsWith("bot")) ii++;
            }

            string enhanced = null;
            string pdfSize = null;
            if (G.Equal(extension, "emf") || G.Equal(extension, "pdf"))
            {
                enhanced = " enhanced";
                fontsize = 0.95 * fontsize;
                if (G.Equal(extension, "pdf"))
                {
                    pdfSize = " size 4, 3";  //default is 5 x 3 inches, too wide.
                }
            }
            else
            {
                fontsize = 0.75 * fontsize;
            }

            txt.AppendLine("set terminal " + extension + enhanced + " font '" + font + "," + (zoom * fontsize) + "'" + pdfSize); ;

            txt.AppendLine("set output \"" + file2 + "\"");
            txt.AppendLine("set key " + key);

            if (G.Equal(Program.options.plot_decimalseparator, "comma"))
            {
                txt.AppendLine("set decimalsign ','");
            }

            if (G.Equal(extension, "emf"))
            {
                fontfactor = 1.4d / 1.2d;
            }
            else if (G.Equal(extension, "svg"))
            {
                fontfactor = 1.4d / 1.2d;
            }
            else if (G.Equal(extension, "png"))
            {
                fontfactor = 1.0d / 1.2d;
            }
            else if (G.Equal(extension, "pdf"))
            {
                fontfactor = .8d;
            }

            double siz1 = 1.5d * zoom * fontsize * fontfactor;
            double siz2 = zoom * fontsize * fontfactor;

            string bold2 = "";
            if (bold != null) bold2 = bold.Replace(" ", "").ToLower();
            string[] bold3 = bold2.Split(',');
            foreach (string s in bold3)
            {
                if (s.Trim() == "") continue;  //can contain empty entry
                if (s != "title" && s != "ytitle" && s != "xtics" && s != "ytics" && s != "key")
                {
                    new Error("<bold = '...'> must be title, ytitle, xtics, ytics or key");
                }
            }
            string title_bold = null; if (bold3.Contains("title")) title_bold = " Bold";
            string ytitle_bold = null; if (bold3.Contains("ytitle")) ytitle_bold = " Bold";
            string xtics_bold = null; if (bold3.Contains("xtics")) xtics_bold = " Bold";
            string ytics_bold = null; if (bold3.Contains("ytics")) ytics_bold = " Bold";
            string key_bold = null; if (bold3.Contains("key")) key_bold = " Bold";

            string italic2 = "";
            if (italic != null) italic2 = italic.Replace(" ", "").ToLower();
            string[] italic3 = italic2.Split(',');
            string title_italic = null; if (italic3.Contains("title")) title_italic = " Italic";
            string ytitle_italic = null; if (italic3.Contains("ytitle")) ytitle_italic = " Italic";
            string xtics_italic = null; if (italic3.Contains("xtics")) xtics_italic = " Italic";
            string ytics_italic = null; if (italic3.Contains("ytics")) ytics_italic = " Italic";
            string key_italic = null; if (italic3.Contains("key")) key_italic = " Italic";
            foreach (string s in italic3)
            {
                if (s.Trim() == "") continue;  //can contain empty entry
                if (s != "title" && s != "ytitle" && s != "xtics" && s != "ytics" && s != "key")
                {
                    new Error("<italic = '...'> must be title, ytitle, xtics, ytics or key");
                }
            }

            txt.AppendLine("set title font " + "'" + font + title_bold + title_italic + "," + siz1 + "'");
            txt.AppendLine("set ylabel font " + "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'");
            txt.AppendLine("set y2label font " + "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'");
            txt.AppendLine("set xtics font " + "'" + font + xtics_bold + xtics_italic + "," + siz2 + "'");
            txt.AppendLine("set ytics font " + "'" + font + ytics_bold + ytics_italic + "," + siz2 + "'");
            if (numberOfY2s > 0 || ymirror == "2" || ymirror == "3") txt.AppendLine("set y2tics font " + "'" + font + ytics_bold + ytics_italic + "," + siz2 + "'");
            txt.AppendLine("set key font " + "'" + font + key_bold + key_italic + "," + siz2 + "'");

            string set_yrange = null;
            string set_y2range = null;
            if (isSeparated)
            {
                double alpha1 = 0.05d;
                double alpha2 = 0.05;
                double beta = 0.30d;
                //linesMin=4, linesmMax=5
                //boxesMin=0, boxesMax=1
                set_yrange = (minMax[4] - (alpha1 + alpha2 + beta) * (minMax[5] - minMax[4])) + ":" + minMax[5];
                set_y2range = (minMax[0] - alpha2 / beta * (minMax[1] - minMax[0])) + ":" + (minMax[1] + (1 + alpha1) / beta * (minMax[1] - minMax[0]));
            }
            else
            {
                set_yrange = GnuplotYrange(ymin, yminsoft, yminhard, ymax, ymaxsoft, ymaxhard);
                set_y2range = GnuplotYrange(y2min, y2minsoft, y2minhard, y2max, y2maxsoft, y2maxhard);
            }

            if (set_yrange.Trim() != ":") txt.AppendLine("set yrange [" + set_yrange + "]");
            if (set_y2range.Trim() != ":") txt.AppendLine("set y2range [" + set_y2range + "]");


            if ((highestFreq == EFreq.A || highestFreq == EFreq.U))
            {
                //annual or undated
                if (numberOfObs > 140)
                {
                    txt.AppendLine("set xtics 20");
                    txt.AppendLine("set mxtics 20");
                }
                else if (numberOfObs > 70)
                {
                    txt.AppendLine("set xtics 10");
                    txt.AppendLine("set mxtics 10");
                }
                else
                {
                    txt.AppendLine("set xtics 5");
                    txt.AppendLine("set mxtics 5");
                }
            }


            //txt.AppendLine("set xtic scale 1.7, 0.85");
            txt.AppendLine("set xtic scale 2, 0.7");
            txt.AppendLine("set xtics nomirror " + ticsInOut + "");

            if (NotNullAndNotNo(xzeroaxis)) txt.AppendLine("set xzeroaxis lt -1"); //draws x axis. May get ugly if residuals are present.

            bool setTitlePlaceholder = false;
            if (numberOfY2s == 0 && !isSeparated)
            {
                //the y2 axis is just mirrored
                if (ymirror == "0")  //nothing
                {
                    txt.AppendLine("set ytics nomirror " + ticsInOut);
                    txt.AppendLine("set border 3");
                    if (!G.NullOrBlanks(ytitle)) setTitlePlaceholder = SetYAxisText(ytitle, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", true);

                }
                else if (ymirror == "1")  //y2 axis
                {
                    txt.AppendLine("set ytics " + ticsInOut);
                    txt.AppendLine("set border 11");
                    if (!G.NullOrBlanks(ytitle)) setTitlePlaceholder = SetYAxisText(ytitle, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", true);
                }
                else if (ymirror == "2")  //y2 axis and y2 tics
                {
                    txt.AppendLine("set ytics " + ticsInOut);
                    txt.AppendLine("set y2tics " + ticsInOut);
                    txt.AppendLine("set border 11");
                    if (!G.NullOrBlanks(ytitle)) setTitlePlaceholder = SetYAxisText(ytitle, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", true);
                }
                else if (ymirror == "3")
                {
                    txt.AppendLine("set ytics " + ticsInOut);  //y2 axis and y2 tics and y2 label
                    txt.AppendLine("set y2tics " + ticsInOut);
                    txt.AppendLine("set border 11");
                    if (!G.NullOrBlanks(ytitle)) setTitlePlaceholder = SetYAxisText(ytitle, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", true);
                    if (!G.NullOrBlanks(ytitle)) setTitlePlaceholder = SetYAxisText(ytitle, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", false);
                }
            }
            else
            {
                //there is a series being shown at the y2 axis
                txt.AppendLine("set ytics nomirror " + ticsInOut);
                txt.AppendLine("set y2tics " + ticsInOut);
                txt.AppendLine("set border 11");
                if (!G.NullOrBlanks(ytitle)) setTitlePlaceholder = SetYAxisText(ytitle, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", true);
                if (!G.NullOrBlanks(y2title)) setTitlePlaceholder = SetYAxisText(y2title, txt, "'" + font + ytitle_bold + ytitle_italic + "," + siz2 + "'", false);
                if (NotNullAndNotNo(x2zeroaxis) || isSeparated) txt.AppendLine("set x2zeroaxis lt -1");  //draws x axis for y2=0, #23475432985 
            }

            //must be after labels
            string subtitle2 = null;
            if (!G.NullOrBlanks(subtitle)) subtitle2 = subtitle;
            if (!G.NullOrBlanks(o.opt_subtitle)) subtitle2 = o.opt_subtitle;
            if (!G.NullOrBlanks(subtitle2)) subtitle2 = "\\n{/*0.80 " + subtitle2 + "}";
            string title2 = null;
            if (!G.NullOrBlanks(title)) title2 = title;
            if (!G.NullOrBlanks(o.opt_title)) title2 = o.opt_title;
            if (!G.NullOrBlanks(title2))
            {
                txt.AppendLine("set title " + Globals.QT + Program.EncodeDanish(GnuplotText(title2 + subtitle2, true)) + Globals.QT);
            }
            else
            {
                if (setTitlePlaceholder) txt.AppendLine("set title " + Globals.QT + " " + Globals.QT);
            }

            foreach (string s in xlines)
            {
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(s);
                double d = G.FromDateToFloating(gt) + 0.5d + GetXAdjustmentForInsideTics(isInside, highestFreq);
                txt.AppendLine("set arrow from " + d + ", graph 0 to " + d + ", graph 1 nohead");
            }

            foreach (string s in xlinebefores)
            {
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(s);
                double d = (G.FromDateToFloating(gt) + G.FromDateToFloating(gt.Add(-1))) / 2d + 0.5d + GetXAdjustmentForInsideTics(isInside, highestFreq);
                txt.AppendLine("set arrow from " + d + ", graph 0 to " + d + ", graph 1 nohead");
            }

            foreach (string s in xlineafters)
            {
                GekkoTime gt = GekkoTime.FromStringToGekkoTime(s);
                double d = (G.FromDateToFloating(gt) + G.FromDateToFloating(gt.Add(1))) / 2d + 0.5d + GetXAdjustmentForInsideTics(isInside, highestFreq);
                txt.AppendLine("set arrow from " + d + ", graph 0 to " + d + ", graph 1 nohead");
            }

            foreach (string s in ylines)
            {
                double d = Program.ParseIntoDouble(s);
                if (!G.isNumericalError(d)) txt.AppendLine("set arrow from graph 0, first " + d + " to graph 1, first " + d + " nohead");
            }

            foreach (string s in y2lines)
            {
                if (numberOfY2s > 0)  //theses lines are ignored if there is no y2 axis shown
                {
                    double d = Program.ParseIntoDouble(s);
                    if (!G.isNumericalError(d)) txt.AppendLine("set arrow from graph 0, second " + d + " to graph 1, second " + d + " nohead");
                }
            }

            if (G.Equal(grid, "yes"))  //it can be an empty <grid/>
            {
                //txt.AppendLine("set style line 102 lc rgb '#d3d3d3' dt 3 lw 1.5");  //line width looks ok in Gekko window, with lw 1 it looks bad there.
                txt.AppendLine("set style line 102 " + gridstyle);  //lt 0 or dt 3 gives ugly lines when viewed in Gekko
                txt.AppendLine("set grid back ls 102");
            }
            else if (G.Equal(grid, "yline"))
            {
                txt.AppendLine("set style line 102 " + gridstyle);  //lt 0 or dt 3 gives ugly lines when viewed in Gekko
                txt.AppendLine("set grid ytics back ls 102");
            }
            else if (G.Equal(grid, "xline"))
            {
                txt.AppendLine("set style line 102 " + gridstyle);  //lt 0 or dt 3 gives ugly lines when viewed in Gekko                
                txt.AppendLine("set grid xtics back ls 102");
            }

            if (isInside)
            {
                HandleXTicsInside(o, highestFreq, firstXLabelFix, txt);
            }
            else
            {
                int mxtics = -12345;
                string ticsTxt = null;
                mxtics = HandleXTicsAt(labels1, labels2, ref ticsTxt, mxtics, highestFreq);
                if (ticsTxt != null) txt.AppendLine(ticsTxt);
            }

            if (!string.IsNullOrEmpty(plotcode))
            {
                txt.AppendLine("");
                txt.AppendLine(plotcode);  //user code
                txt.AppendLine("");
            }

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            double widthForBoxes = 1d;
            if (highestFreq == EFreq.Q)
            {
                widthForBoxes = 1d / 4d;
            }
            else if (highestFreq == EFreq.M)
            {
                widthForBoxes = 1d / 12d;
            }
            else if (highestFreq == EFreq.W)
            {
                widthForBoxes = 1d / 53d;
            }
            else if (highestFreq == EFreq.D)
            {
                widthForBoxes = 1d / 366d;
            }

            histoGap = (int)Program.ParseIntoDouble(boxgap);
            if (boxesY.Count + boxesY2.Count == 1) histoGap = 0;
            d_width = widthForBoxes / (double)(boxesY.Count + boxesY2.Count + histoGap);
            d_width2 = boxwidth * d_width;
            d_width3 = boxwidth * widthForBoxes;
            left = d_width * (double)(boxesY.Count + boxesY2.Count - 1) / 2d;

            if (boxesY.Count + boxesY2.Count + areasY.Count + areasY2.Count > 0)
            {
                txt.AppendLine("f(x) = (sgn(x+1.2345e-30) + 1)/2");  //1 if x > 0, else 0. 1.2345e-30 added to avoid 0 becoming 0.5
            }

            // ---------------------------------------
            // ---------------------------------------
            //          SECOND PASS
            // ---------------------------------------
            // ---------------------------------------
            string plotline = PlotHandleLines(false, ref numberOfY2s, minMax, dataMin, dataMax, o, count, labelsNonBroken, file1, lines3, boxesY, boxesY2, areasY, areasY2, linetypeMain, dashtypeMain, linewidthMain, linecolorMain, pointtypeMain, pointsizeMain, fillstyleMain, stacked, palette2, isSeparated, d_width, d_width2, d_width3, left, containerExplode, linewidthCorrection, pointsizeCorrection, isInside, highestFreq);
            txt.AppendLine(plotline);

            string emfName = CallGnuplot2(o, rr, file2, file3, currentDir, path, fileGp, fileData, txt);

            CallGnuplotMakeWindow(o, labelsNonBroken, emfName);

            return emfName;
        }

        private static void CallGnuplotMakeWindow(O.Prt o, List<string> labelsNonBroken, string emfName)
        {
            if (o.opt_filename != null && o.opt_filename != "")
            {
                string fileNameWithPath = Program.CreateFullPathAndFileName(o.opt_filename);
                Program.WaitForFileCopy(emfName, fileNameWithPath);
                G.Writeln2("PLOT created file " + fileNameWithPath);
                return;
            }

            o.emfName = emfName;

            if (!o.guiGraphIsRefreshing)
            {
                PrtOptionsHelper po = new PrtOptionsHelper();

                string code = null;
                List<OptString> codes = o.operators;
                if (codes.Count == 1 && G.Equal(codes[0].s2, "yes")) code = codes[0].s1;

                if (code != null)
                {
                    if (G.Equal(code, "m"))
                    {
                        po.isLevel = false;
                        po.isDiff = true;
                        po.isPch = false;
                        po.isMultiplier = true;
                    }
                    else if (G.Equal(code, "q"))
                    {
                        po.isLevel = false;
                        po.isDiff = false;
                        po.isPch = true;
                        po.isMultiplier = true;
                    }
                    else if (G.Equal(code, "d") || G.Equal(code, "rd"))
                    {
                        po.isLevel = false;
                        po.isDiff = true;
                        po.isPch = false;
                        po.isMultiplier = false;
                    }
                    else if (G.Equal(code, "p") || G.Equal(code, "rp"))
                    {
                        po.isLevel = false;
                        po.isDiff = false;
                        po.isPch = true;
                        po.isMultiplier = false;
                    }
                }
                else
                {
                    po.isLevel = true;
                    po.isDiff = false;
                    po.isPch = false;
                    po.isMultiplier = false;
                }
                po.isLog = false;
                po.isDlog = false;

                GraphOptions graphOptions = new GraphOptions();
                graphOptions.counter = o.counter;
                graphOptions.localBanks = null;
                graphOptions.emfName = emfName;
                graphOptions.po = po;
                graphOptions.pph = null;
                graphOptions.precedents = null;
                graphOptions.tEnd = o.t2;
                graphOptions.tStart = o.t1;
                graphOptions.graphVars = null;
                graphOptions.graphVarsNames = labelsNonBroken;
                graphOptions.title = null;
                graphOptions.printStorageAsFuncCounter = o.printStorageAsFuncCounter;

                //G.Writeln("Calling gnuplot2");

                Thread thread = new Thread(new ParameterizedThreadStart(Program.GraphThreadFunction));
                thread.SetApartmentState(ApartmentState.STA);
                thread.CurrentCulture = CultureInfo.InvariantCulture;
                //thread.CurrentCulture = new System.Globalization.CultureInfo("en-US");  //gets . instead of , in doubles
                thread.Start(graphOptions);

                //Also see #9237532567
                //This stuff makes sure we wait for the window to open, before we move on with the code.
                for (int i = 0; i < 6000; i++)  //up to 60 s, then we move on anyway
                {
                    System.Threading.Thread.Sleep(10);  //0.01s
                    if (graphOptions.windowIsShown)
                    {
                        break;
                    }
                }
            }
            else
            {
                o.guiGraphRefreshingFilename = emfName;
            }
        }

        private static string CallGnuplot2(O.Prt o, int rr, string file2, string file3, string currentDir, string path, string fileGp, string fileData, StringBuilder txt)
        {
            using (FileStream fs = Program.WaitForFileStream(fileGp, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter tw = G.GekkoStreamWriter(fs))
            {
                tw.WriteLine(txt);
                tw.Flush(); //probably not necessary
                tw.Close(); //probably not necessary
            }

            if (G.Equal(o.opt_dump, "yes"))
            {
                try
                {
                    File.Copy(fileGp, Program.options.folder_working + "\\" + "gekkoplot.gp", true);
                    File.Copy(fileData, Program.options.folder_working + "\\" + "gekkoplot.dat", true);
                    string text = null;
                    text = File.ReadAllText(Program.options.folder_working + "\\" + "gekkoplot.gp");
                    text = text.Replace("temp" + rr, "gekkoplot");
                    File.WriteAllText(Program.options.folder_working + "\\" + "gekkoplot.gp", text);
                    text = File.ReadAllText(Program.options.folder_working + "\\" + "gekkoplot.dat");
                    text = text.Replace("temp" + rr, "gekkoplot");
                    File.WriteAllText(Program.options.folder_working + "\\" + "gekkoplot.dat", text);
                    G.Writeln2("Dumped gnuplot files gekkoplot.gp (script) and gekkoplot.dat (data) in the working folder");

                }
                catch
                {
                    new Warning("PLOT<dump> failed: are gekkoplot.gp or gekkoplot.dat blocked?");
                }
            }

            string emfName = path + "\\" + file2;
            string exe = "wgnuplot51.exe";

            Process process = new Process();
            if (G.IsUnitTesting())
            {
                process.StartInfo.FileName = Globals.ttPath2 + "\\" + Globals.ttPath3 + @"\Gekko\bin\Debug\gnuplot\" + exe;
            }
            else
            {
                process.StartInfo.FileName = Application.StartupPath + "\\gnuplot\\" + exe;
            }

            //NOTE: quotes added because this path may contain blanks
            process.StartInfo.Arguments = Globals.QT + path + "\\" + file3 + Globals.QT;
            bool msg = false;
            bool exited = false;
            try
            {
                process.Start();
                exited = process.WaitForExit(1 * 60 * 1000);  //1 minute, has been > 5 sec at DORS
                if (!exited)
                {
                    MessageBox.Show("*** ERROR: The gnuplot call did not respond within 60 seconds, so the " + G.NL + "gnuplot call was aborted.");
                    msg = true;
                    throw new GekkoException();
                }
                else if (process.ExitCode != 0)
                {
                    MessageBox.Show("*** ERROR: The generated gnuplot script file had an unexpected error. If you use PLOT<dump>, Gekko will dump \nthe files gekkoplot.gp and gekkoplot.dat in the working folder. \nThese files can be tried out in gnuplot 5.1, to locate the error \n(by means of 'load gekkoplot.gp' in gnuplot).");
                    msg = true;
                    throw new GekkoException();
                }
            }
            catch (Exception e)
            {
                if (exited && !msg)
                {
                    MessageBox.Show("*** ERROR: There was a internal problem calling gnuplot." + G.NL + "ERROR: " + e.Message);
                }
                throw;
            }

            process.Close();

            //resets current dir to previous location
            Directory.SetCurrentDirectory(currentDir);
            return emfName;
        }

        private static string GetText(XmlNode x, string def)
        {
            return GetText(null, null, null, x, def);
        }

        private static string GetText(string y1, string y2, XmlNode y3, XmlNode y4, string y5)
        {
            //it seems the xml reader auto-trims the strings
            string s = y5; //maybe null, "", or a real string

            if (y4 != null)
            {
                if (y4.InnerText.StartsWith("//"))
                {
                    //do nothing, as if the tag does not even exist
                }
                else
                {
                    s = y4.InnerText;  //if <tag></tag> or <tag/>, s will be = "". We say that this overrides prior settings.                
                }
            }
            else
            {
                //do nothing: the tag does not exist
            }

            if (y3 != null)
            {
                if (y3.InnerText.StartsWith("//"))
                {
                    //do nothing, as if the tag does not even exist
                }
                else
                {
                    s = y3.InnerText;  //if <tag></tag> or <tag/>, s will be = "". We say that this overrides prior settings.                
                }
            }
            else
            {
                //do nothing: the tag does not exist
            }

            if (y2 != null)
            {
                //For instance PLOT <color='red> x1, x2;
                s = y2;
            }

            if (y1 != null)
            {
                //For instance PLOT x1<color='red'>, x2;
                s = y1;
            }

            return s;
        }

        private static List<string> GetText(XmlNodeList x)
        {
            List<string> ss = new List<string>();
            foreach (XmlNode y in x)
            {
                ss.Add(GetText(y));
            }
            return ss;
        }

        private static string GetText(XmlNode x)
        {
            return GetText(x, null);
        }

        private static void HandleXTicsInside(O.Prt o, EFreq highestFreq, bool firstXLabelFix, StringBuilder txt)
        {
            //TODO: if there are too many minor tics, turn them off
            //TODO: if years get too cramped, show every second (even)
            //TODO: if years get too cramped, show only 15, 16, 17, not 2015, 2016, 2017
            //TODO: if years are still very cramped, switch to isInside = false!

            //??? what about mixed freqs?? They can either be (a) only annual at-tics, and q and m are without tics
            //                                             or (b) between-tics, showing highest freq if not too many minor, else next-highest.
            //    with mixed freqs we treat is as highest frequency plot, and sneak in the lower freqs.
            //OPTION plot xlabels nonannual = at | between | auto ;  //auto will start with between and then jump to at
            //OPTION plot xlabels annual    = at | between | auto ;  //auto will start with between and then jump to at
            //OPTION plot xlabels between truncate = digits | skip | both | auto ; 

            double extra = 0;

            int t1 = o.t1.super;
            int t2 = o.t2.super;

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            int numberOfMinorTics = 1;
            if (highestFreq == EFreq.Q)
            {
                numberOfMinorTics = 4;
                extra = (double)o.t2.sub / (double)numberOfMinorTics;
            }
            else if (highestFreq == EFreq.M)
            {
                numberOfMinorTics = 12;
                extra = (double)o.t2.sub / (double)numberOfMinorTics;
            }
            else if (highestFreq == EFreq.W)
            {
                numberOfMinorTics = 53;
                extra = (double)o.t2.sub / (double)numberOfMinorTics;
            }
            else if (highestFreq == EFreq.D)
            {
                numberOfMinorTics = 12; //we show month minor tics                    
                extra = (double)o.t2.sub / (double)numberOfMinorTics;  //approximate, which is ok (sub will be months here)
            }
            else
            {
                numberOfMinorTics = 1;
                extra = 1;
            }

            bool deduct = false;
            if (extra <= 0.40) deduct = true;

            if (Program.options.plot_xlabels_digits != 4 && Program.options.plot_xlabels_digits != 2)
            {
                new Error("'OPTION plot xlabels digits' should be either 4 or 2");
                //throw new GekkoException();
            }

            string ss = null;
            for (int t = t1; t <= t2; t++)
            {
                int years = t2 - t1 + 1;
                bool twoDigits = false;
                if (Program.options.plot_xlabels_digits == 2) twoDigits = true;

                int skip = 1;
                if (twoDigits)
                {
                    if (years > 2 * 48) skip = 10;
                    else if (years > 48) skip = 5;
                    else if (years > 24) skip = 2;
                }
                else
                {
                    //4 digits
                    if (years > 2 * 24) skip = 10;
                    else if (years > 24) skip = 5;
                    else if (years > 12) skip = 2;
                }

                string tx = t.ToString();
                if (twoDigits) tx = (t % 100).ToString().PadLeft(2, '0');

                if (skip > 1 && t % skip != 0) tx = null;  //all uneven are zapped

                if (deduct && t == t2) tx = null;

                string tlabel = "\"" + tx + "\"";

                ss += tlabel + " " + t + " " + "0, ";  //0 is major tic

                //========================================================================================================
                //                          FREQUENCY LOCATION, indicates where to implement more frequencies
                //========================================================================================================

                if ((highestFreq == EFreq.Q && years <= 75) || (highestFreq == EFreq.M && years <= 25) || (highestFreq == EFreq.W && years <= 6) || highestFreq == EFreq.D)
                {
                    for (int tt = 1; tt < numberOfMinorTics; tt++)  //is skipped if sub = 1
                    {
                        double d = (double)tt / (double)numberOfMinorTics;
                        ss += (t + d) + " " + "1, ";  //1 is minor tic
                    }
                }
            }

            txt.AppendLine("set xtics (" + ss + ")");
            txt.AppendLine("set xtics offset first 0.5, first 0");  //moves xtic labels a half year to the right, but not the tic itself
            if (firstXLabelFix)
            {
                if ((highestFreq == EFreq.M && o.t1.freq == EFreq.M && o.t1.sub <= 1) || (highestFreq == EFreq.Q && o.t1.freq == EFreq.Q && o.t1.sub <= 1))  //these could perhaps be <=4 and <=2 respectively. Often the plot starts in first subperiod anyway.
                {
                    //only show whole first year if monthly and m1-m4 or quarterly and q1-q2
                    double tStart = (double)t1 - 0.000000001d;                      //deducts a small number to activate the first x-axis label
                    txt.AppendLine("set xrange [" + tStart.ToString() + ":]");      //see above
                }
            }
        }

        private static bool SetYAxisText(string ytitle, StringBuilder txt, string font, bool isLeft)
        {
            bool setTitlePlaceholder;
            setTitlePlaceholder = true; //to make space
            if (isLeft) txt.AppendLine("set label \"" + GnuplotText(ytitle) + "\" at graph 0, graph 1 offset -3,2.2 left font " + font);
            else txt.AppendLine("set label \"" + GnuplotText(ytitle) + "\" at graph 1, graph 1 offset 3,2.2 right font " + font);
            return setTitlePlaceholder;
        }

        private static string PlotHandleLines(bool firstPass, ref int numberOfY2s, double[] minMax, double[] dataMin, double[] dataMax, O.Prt o, int count, List<string> labelsNonBroken, string file1, XmlNodeList lines3, List<int> boxesY, List<int> boxesY2, List<int> areasY, List<int> areasY2, XmlNode linetypeMain, XmlNode dashtypeMain, XmlNode linewidthMain, XmlNode linecolorMain, XmlNode pointtypeMain, XmlNode pointsizeMain, XmlNode fillstyleMain, bool stacked, List<string> palette2, bool isSeparated, double d_width, double d_width2, double d_width3, double left, List<O.Prt.Element> co, double linewidthCorrection, double pointsizeCorrection, bool isInside, EFreq highestFreq)
        {
            int manyXValues = 0;  //0 or 1            

            string plotline = "plot ";

            int boxesYCounter = 0;
            int boxesY2Counter = 0;
            int areasYCounter = 0;
            int areasY2Counter = 0;
            int iii = 0;
            for (int i = 0; i < count; i++)
            {
                iii = i;
                XmlNode line3 = lines3[i];

                //defaults
                string dlinetype = "lines";
                if (Program.options.plot_lines_points) dlinetype = "linespoints";
                string ddashtype = "1";
                string dlinewidth = "3";
                string dlinecolor = palette2[i % palette2.Count].Trim();
                string dpointtype = "7";
                string dpointsize = "0.5";
                string dfillstyle = "solid";
                string dy2_ = "no";

                string linetype = null;
                string dashtype = null;
                string linewidth = null;
                string linecolor = null;
                string pointtype = null;
                string pointsize = null;
                string fillstyle = null;
                string label = null;
                string y2 = null;

                bool isExplicit = false;
                string labelCleaned = labelsNonBroken[i];
                if (labelCleaned.StartsWith(Globals.labelCheatString))
                {
                    isExplicit = true;
                    labelCleaned = labelCleaned.Substring(Globals.labelCheatString.Length);
                }

                // ---------------------------------------------
                // --------- loading lines section start
                // ---------------------------------------------

                linetype = GetText(co[i].linetype, o.opt_linetype, line3 == null ? null : line3.SelectSingleNode("type"), linetypeMain, dlinetype);
                dashtype = GetText(co[i].dashtype, o.opt_dashtype, line3 == null ? null : line3.SelectSingleNode("dashtype"), dashtypeMain, ddashtype);
                linewidth = GetText(G.isNumericalError(co[i].linewidth) ? null : co[i].linewidth.ToString(), G.isNumericalError(o.opt_linewidth) ? null : o.opt_linewidth.ToString(), line3 == null ? null : line3.SelectSingleNode("linewidth"), linewidthMain, dlinewidth);
                linecolor = GetText(co[i].linecolor, o.opt_linecolor, line3 == null ? null : line3.SelectSingleNode("linecolor"), linecolorMain, dlinecolor);
                pointtype = GetText(co[i].pointtype, o.opt_pointtype, line3 == null ? null : line3.SelectSingleNode("pointtype"), pointtypeMain, dpointtype);
                pointsize = GetText(G.isNumericalError(co[i].pointsize) ? null : co[i].pointsize.ToString(), G.isNumericalError(o.opt_pointsize) ? null : o.opt_pointsize.ToString(), line3 == null ? null : line3.SelectSingleNode("pointsize"), pointsizeMain, dpointsize);
                fillstyle = GetText(co[i].fillstyle, o.opt_fillstyle, line3 == null ? null : line3.SelectSingleNode("fillstyle"), fillstyleMain, dfillstyle);
                y2 = GetText(co[i].y2, null, line3 == null ? null : line3.SelectSingleNode("y2"), null, "no"); //default: no, #23475432985
                label = HandleLabel(line3, isExplicit, labelCleaned);

                if (G.Equal(linetype, "boxes"))
                {
                    if (isSeparated) y2 = "yes";  //set y for all lines, and y2 for all boxes --> this overrides other settings
                }
                else
                {
                    fillstyle = null;  //fillstyle will fail if combined with other line types.
                    if (isSeparated) y2 = "no";  //set y for all lines, and y2 for all boxes --> this overrides other settings
                }



                // ---------------------------------------------
                // --------- loading lines section end
                // ---------------------------------------------


                if (G.Equal(linetype, "boxes") && fillstyle.Contains("solid"))
                {
                    linewidth = "1";  //otherwise the borders of these get blurred
                }

                label = GnuplotText(label);

                string s = null;
                if (!G.NullOrBlanks(linetype))
                {
                    if (G.Equal(linetype, "filledcurve") || G.Equal(linetype, "filledcurves"))
                    {
                        s += " with " + linetype + " y1=0";  //so the area is towards the x-axis
                    }
                    else
                    {
                        s += " with " + linetype;
                    }
                }
                if (NotNullAndNotNo(y2))
                {
                    if (firstPass) numberOfY2s++;
                    s += " axes x1y2";  //#23475432985
                }

                try
                {
                    if (linewidthCorrection != 1d)
                    {
                        double temp = G.ParseIntoDouble(linewidth);
                        linewidth = (temp * linewidthCorrection).ToString();
                    }
                    if (pointsizeCorrection != 1d)
                    {
                        double temp = G.ParseIntoDouble(pointsize);
                        pointsize = (temp * pointsizeCorrection).ToString();
                    }
                }
                catch { };

                if (!G.NullOrBlanks(dashtype)) s += " dashtype " + dashtype;
                if (!G.NullOrBlanks(linewidth)) s += " linewidth " + linewidth;
                if (!G.NullOrBlanks(linecolor)) s += " linecolor rgb \"" + linecolor.ToLower() + "\"";  //in gnuplot, the linecolor must be lower-case
                if (!G.NullOrBlanks(pointtype)) s += " pointtype " + pointtype;
                if (!G.NullOrBlanks(pointtype)) s += " pointsize " + pointsize;
                if (!G.NullOrBlanks(fillstyle)) s += " fillstyle " + fillstyle;

                string label2 = label;
                if (label != null && label != "") label2 = label + "   "; //blanks added to separate items in the legend                    
                s += " title " + Globals.QT + label2 + Globals.QT;

                //linestyle is an association of linecolor, linewidth, dashtype, pointtype
                //linetype is the same, just permanent
                //box: fillstyle empty|solid|pattern, border|noborder

                string xAdjustment = null;
                if (G.Equal(linetype, "boxes"))
                {
                    if (firstPass)
                    {
                        minMax[0] = Math.Min(minMax[0], dataMin[i]);
                        minMax[1] = Math.Max(minMax[1], dataMax[i]);
                    }

                    if (line3 == null || line3.SelectSingleNode("y2") == null)
                    {
                        boxesYCounter++;
                        if (firstPass) boxesY.Add(i);
                    }
                    else
                    {
                        boxesY2Counter++;
                        if (firstPass) boxesY2.Add(i);
                    }


                    if (stacked)
                    {
                        //see also #34252435
                        string ss = null;
                        if (line3 == null || line3.SelectSingleNode("y2") == null)
                        {
                            //the boxes could be i = 0, 2, 4, 5. The first of these is $1+$3+$5+$6 (note 1 added), the second is $3+$5+$6, etc.                            
                            //the f(x) funcion return 1 if positive, else 0.
                            //with the f function, we get: f($1*$6)*$1 + f($3*$6)*$3 + f($5*$6)*$5 + f($6*$6)*$6 --> the last f($6*$6) could be omitted since it will alway return 1
                            for (int k = boxesYCounter - 1; k < boxesY.Count; k++)
                            {
                                //see similar code below
                                ss += "f($" + (boxesY[k] + (count + 1)) + "*$" + (boxesY[boxesYCounter - 1] + (count + 1)) + ")*$" + (boxesY[k] + (count + 1)) + "+";
                            }
                        }
                        else
                        {
                            //the boxes could be i = 0, 2, 4, 5. The first of these is $1+$3+$5+$6 (note 1 added), the second is $3+$5+$6, etc.
                            for (int k = boxesY2Counter - 1; k < boxesY2.Count; k++)
                            {
                                //see similar code above
                                ss += "f($" + (boxesY2[k] + (count + 1)) + "*$" + (boxesY2[boxesYCounter - 1] + (count + 1)) + ")*$" + (boxesY2[k] + (count + 1)) + "+";
                            }
                        }
                        if (ss != null && ss.EndsWith("+")) ss = ss.Substring(0, ss.Length - 1); //remove last '+'                       
                        if (isInside)
                        {
                            xAdjustment = "($" + (iii + 1) + "+(" + GetXAdjustmentForInsideTics(isInside, highestFreq) + ")):(" + ss + ")" + ":(" + d_width3 + ")";
                        }
                        else
                        {
                            if (true)
                            {
                                double d = -0.5;
                                xAdjustment = "($" + (iii + 1) + " +(" + d + "))" + ":(" + ss + ")" + ":(" + d_width3 + ")";
                            }

                        }
                    }
                    else
                    {
                        //adjusting horizontal position for clustered boxes
                        double d = (boxesYCounter + boxesY2Counter - 1) * d_width - left;

                        if (true)
                        {
                            if (!isInside) d = d - 0.5;
                        }

                        if (isInside)
                        {
                            xAdjustment = "($" + (iii + 1) + " +(" + d + ")+(" + GetXAdjustmentForInsideTics(isInside, highestFreq) + ")):" + (i + (count + 1)) + ":(" + d_width2 + ")";
                        }
                        else
                        {
                            xAdjustment = "($" + (iii + 1) + " +(" + d + ")):" + (i + (count + 1)) + ":(" + d_width2 + ")";
                        }
                    }
                }
                else if (G.Equal(linetype, "filledcurve") || G.Equal(linetype, "filledcurves"))
                {
                    if (firstPass)
                    {
                        minMax[2] = Math.Min(minMax[2], dataMin[i]);
                        minMax[3] = Math.Max(minMax[3], dataMax[i]);
                    }
                    if (line3 == null || line3.SelectSingleNode("y2") == null)
                    {
                        areasYCounter++;
                        if (firstPass) areasY.Add(i);
                    }
                    else
                    {
                        areasY2Counter++;
                        if (firstPass) areasY2.Add(i);
                    }

                    if (stacked)
                    {
                        //see comments under #34252435
                        string ss = null;
                        if (line3 == null || line3.SelectSingleNode("y2") == null)
                        {
                            for (int k = areasYCounter - 1; k < areasY.Count; k++)
                            {
                                //see similar code below
                                ss += "f($" + (areasY[k] + (count + 1)) + "*$" + (areasY[areasYCounter - 1] + (count + 1)) + ")*$" + (areasY[k] + (count + 1)) + "+";
                            }
                        }
                        else
                        {
                            for (int k = areasY2Counter - 1; k < areasY2.Count; k++)
                            {
                                //see similar code above
                                ss += "f($" + (areasY2[k] + (count + 1)) + "*$" + (areasY2[areasYCounter - 1] + (count + 1)) + ")*$" + (areasY2[k] + (count + 1)) + "+";
                            }
                        }
                        if (ss != null && ss.EndsWith("+")) ss = ss.Substring(0, ss.Length - 1);  //remove last '+'                       
                        if (isInside)
                        {
                            xAdjustment = "($" + (iii + 1) + "+(" + GetXAdjustmentForInsideTics(isInside, highestFreq) + ")):(" + ss + ")";
                        }
                        else
                        {
                            xAdjustment = "" + (iii + 1) + ":(" + ss + ")";
                        }
                    }
                    else
                    {
                        if (isInside)
                        {
                            xAdjustment = "($" + (iii + 1) + "+(" + GetXAdjustmentForInsideTics(isInside, highestFreq) + ")):" + (i + (count + 1));  //just normal positioning
                        }
                        else
                        {
                            xAdjustment = "" + (iii + 1) + ":" + (i + (count + 1));  //just normal positioning
                        }
                    }
                }
                else
                {
                    if (firstPass)
                    {
                        minMax[4] = Math.Min(minMax[4], dataMin[i]);
                        minMax[5] = Math.Max(minMax[5], dataMax[i]);
                    }

                    xAdjustment = "($" + (iii + 1) + "+(" + GetXAdjustmentForInsideTics(isInside, highestFreq) + ")):" + (i + (count + 1));

                }

                //string xlabel = GnuplotText(label);

                plotline += "\"" + file1 + "\" using " + xAdjustment + s;

                if (i < count - 1) plotline += ", ";
            }

            return plotline;
        }

        private static double GetXAdjustmentForInsideTics(bool isInside, EFreq highestFreq)
        {
            if (!isInside && (highestFreq == EFreq.A || highestFreq == EFreq.U)) return -0.5;
            else return 0d;
            //if (!isInside) return 0d;
            //int sub = 1;
            //if (highestFreq == EFreq.Q) sub = 4;
            //else if (highestFreq == EFreq.M) sub = 12;
            //double adj = 1d / sub / 2d;
            //return adj;
        }

        private static bool NotNullAndNotNo(string s)
        {
            //#23475432985
            return s != null && !G.Equal(s, "no");
        }

        private static string HandleLabel(XmlNode line3, bool isExplicit, string labelCleaned)
        {
            string label;
            string labelGpt = line3 == null ? null : GetText(line3.SelectSingleNode("label"));
            if (isExplicit)  //for instance: PLOT x*y 'product';
            {
                label = labelCleaned;  //overrides any xml label                        
            }
            else  //for instance: PLOT x*y;
            {
                label = labelCleaned;
                if (!G.NullOrBlanks(labelGpt)) label = labelGpt;  //xml label overrides variables
            }

            return label;
        }


        private static string GnuplotText(string s)
        {
            return GnuplotText(s, false);
        }


        private static string GnuplotText(string s, bool omitCurly)
        {
            //cf. http://ayapin-film.sakura.ne.jp/Gnuplot/Docs/ps_guide.pdf
            if (s == null) return null;
            string s2 = s;
            s2 = s2.Replace(@"_", @"\\_");
            s2 = s2.Replace(@"@", @"\\@");
            if (!omitCurly)
            {
                s2 = s2.Replace(@"{", @"\\{");
                s2 = s2.Replace(@"}", @"\\}");
            }
            s2 = s2.Replace(@"^", @"\\^");
            s2 = s2.Replace(@"&", @"\\&");
            return s2;
        }

        private static int HandleXTicsAt(List<string> labels1, List<string> labels2, ref string ticsTxt, int mxtics, EFreq highestFreq)
        {
            if (highestFreq == EFreq.A || highestFreq == EFreq.U)
            {
                //do nothing
                ticsTxt = null;
            }
            else
            {
                List<int> subperiods;
                int onlyYears;
                mxtics = GnuplotHandleXAxisLabelsAt(labels1, mxtics, out subperiods, out onlyYears);

                string s3 = null;
                int c = -1;
                for (int i = 0; i < labels1.Count; i++)
                {
                    c++;
                    //int subper=labels2[i]
                    //if (labels1.Count > 20 && c %  != 0) continue;
                    string[] split = labels2[i].Split(new char[] { '/' });
                    if (onlyYears != -12345 && int.Parse(split[0]) % onlyYears != 0) continue;
                    if (subperiods.Contains(int.Parse(split[1])))
                    {
                        string xx = labels2[i];
                        s3 += "\"" + labels1[i] + "\" \"" + xx + "\", ";
                    }
                }
                if (s3 != null)
                {
                    if (s3.EndsWith(", ")) s3 = s3.Substring(0, s3.Length - 2);
                    ticsTxt = "set xtics (" + s3 + ")" + G.NL;
                }
            }

            return mxtics;
        }

        private static string FromGnuplotDateToFloatingValue(string[] split)
        {
            return ((double)int.Parse(split[0]) + ((double)int.Parse(split[1]) - 1d) / 12d).ToString();
        }

        private static int GnuplotHandleXAxisLabelsAt(List<string> labels1, int mxtics, out List<int> subperiods, out int onlyYears)
        {
            subperiods = new List<int>();
            onlyYears = -12345;
            if (Program.options.freq == EFreq.Q)
            {
                if (labels1.Count <= 12)  //for quarterly, 12 corresponds to 3 years with 4 subpers each
                {
                    subperiods.Add(1);  //q1
                    subperiods.Add(4);  //q2
                    subperiods.Add(7);  //q3
                    subperiods.Add(10);  //q4
                }
                else if (labels1.Count <= 24)
                {
                    subperiods.Add(1);  //q1
                    subperiods.Add(7);  //q3
                    mxtics = 2;
                }
                else if (labels1.Count <= 48)
                {
                    subperiods.Add(1);  //q1
                    mxtics = 4;
                }
                else if (labels1.Count <= 5 * 48)
                {
                    onlyYears = 5;
                    subperiods.Add(1);  //q1
                    mxtics = 5;
                }
                else if (labels1.Count <= 10 * 48)
                {
                    onlyYears = 10;
                    subperiods.Add(1);  //q1
                    mxtics = 10;
                }
                else
                {
                    onlyYears = 20;
                    subperiods.Add(1);  //q1
                }
            }
            else  //monthly
            {
                if (labels1.Count <= 12)  //for monthly, 12 corresponds to 1 year with 12 subpers
                {
                    subperiods.Add(1);  //m1
                    subperiods.Add(2);  //m2
                    subperiods.Add(3);  //m3
                    subperiods.Add(4);  //m4
                    subperiods.Add(5);  //m5
                    subperiods.Add(6);  //m6
                    subperiods.Add(7);  //m7
                    subperiods.Add(8);  //m8
                    subperiods.Add(9);  //m9
                    subperiods.Add(10);  //m10
                    subperiods.Add(11);  //m11
                    subperiods.Add(12);  //m12
                }
                else if (labels1.Count <= 24)
                {
                    subperiods.Add(1);  //m1
                    subperiods.Add(3);  //m3
                    subperiods.Add(5);  //m5
                    subperiods.Add(7);  //m7
                    subperiods.Add(9);  //m9
                    subperiods.Add(11);  //m11
                    mxtics = 2;
                }
                else if (labels1.Count <= 36)
                {
                    subperiods.Add(1);  //m1
                    subperiods.Add(4);  //m4
                    subperiods.Add(7);  //m7
                    subperiods.Add(10);  //m10
                    mxtics = 3;
                }
                else if (labels1.Count <= 48)
                {
                    subperiods.Add(1);  //m1
                    subperiods.Add(5);  //m5
                    subperiods.Add(9);  //m9
                    mxtics = 4;
                }
                else if (labels1.Count <= 72)
                {
                    subperiods.Add(1);  //m1
                    subperiods.Add(7);  //m7
                    mxtics = 6;
                }
                else if (labels1.Count <= 144)
                {
                    subperiods.Add(1);  //m1
                }
                else if (labels1.Count <= 15 * 48)
                {
                    onlyYears = 5;
                    subperiods.Add(1);  //m1
                    mxtics = 5;
                }
                else if (labels1.Count <= 30 * 48)
                {
                    onlyYears = 10;
                    subperiods.Add(1);  //m1
                    mxtics = 10;
                }
                else
                {
                    onlyYears = 20;
                    subperiods.Add(1);  //m1
                }
            }

            return mxtics;
        }

        private static string GnuplotYrange(string ymin, string yminsoft, string yminhard, string ymax, string ymaxsoft, string ymaxhard)
        {
            // [  yminhard < * < yminsoft  : ymaxsoft < * < ymaxhard ] 
            // TODO: it would be nice to test the inequalities above, because if they are violated, they have no effect --> free borders, even if ymin/ymax is set.

            if (G.Equal(ymin, "NaN")) ymin = null;
            if (G.Equal(yminsoft, "NaN")) yminsoft = null;
            if (G.Equal(yminhard, "NaN")) yminhard = null;
            if (G.Equal(ymax, "NaN")) ymax = null;
            if (G.Equal(ymaxsoft, "NaN")) ymaxsoft = null;
            if (G.Equal(ymaxhard, "NaN")) ymaxhard = null;

            string left = null;
            string right = null;
            if (!G.NullOrBlanks(ymin))
            {
                left = ymin;
            }
            else
            {
                if (!G.NullOrBlanks(yminhard) && !G.NullOrBlanks(yminsoft))
                {
                    left = yminhard + " < * < " + yminsoft;
                }
                else if (!G.NullOrBlanks(yminhard) && G.NullOrBlanks(yminsoft))
                {
                    left = yminhard + " < * ";
                }
                else if (G.NullOrBlanks(yminhard) && !G.NullOrBlanks(yminsoft))
                {
                    left = " * < " + yminsoft;
                }
                else
                {
                    left = "";
                }
            }

            if (!G.NullOrBlanks(ymax))
            {
                double xx = Program.ParseIntoDouble(ymax);  //just testing
                right = ymax;
            }
            else
            {
                if (!G.NullOrBlanks(ymaxhard) && !G.NullOrBlanks(ymaxsoft))
                {
                    right = ymaxsoft + " < * < " + ymaxhard;
                }
                else if (!G.NullOrBlanks(ymaxhard) && G.NullOrBlanks(ymaxsoft))
                {
                    right = " * < " + ymaxhard;
                }
                else if (G.NullOrBlanks(ymaxhard) && !G.NullOrBlanks(ymaxsoft))
                {
                    right = ymaxsoft + " < * ";
                }
                else
                {
                    right = "";
                }
            }
            return left + ":" + right;
        }






    }
}
