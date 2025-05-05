using System;
using System.Windows;

namespace Gekko
{

    public class RefreshHelper
    {
        public string op = "";  //can also be n1/n2 (left or right n)
        public bool? isLog = false;
        public bool? isYoy = false;
        public string period = "";
        public bool? isIndex = null;
        public bool? isRef = false;
        public bool? isAll = false;
        public double fontScaling = 1d;
        public double sizeScaling = 1d;
        public bool isRefreshing = false;
        public bool isButton = false;

        public RefreshHelper Clone()
        {
            RefreshHelper r = new RefreshHelper();
            r.op = this.op;
            r.isLog = this.isLog;
            r.isYoy = this.isYoy;
            r.period = this.period;
            r.isIndex = this.isIndex;
            r.isRef = this.isRef;
            r.isAll = this.isAll;
            r.fontScaling = this.fontScaling;
            r.isRefreshing = this.isRefreshing;
            return r;
        }
    }
    
    /// <summary>
    /// Interaction logic for WindowPlot.xaml
    /// </summary>
    public partial class WindowPlot : Window
    {
        public GraphOptions _graphOptions = null;
        public bool _shown;
        public RefreshHelper _refresh = new Gekko.RefreshHelper();

        public WindowPlot(GraphOptions graphOptions)
        {
            this._graphOptions = graphOptions;

            Globals.disableRadioButtons = 1;
            try
            {
                InitializeComponent();
                if (graphOptions.scaleGeneral < 0.99d || graphOptions.scaleGeneral > 1.01d)
                {                                       
                    this.svgfile.Width = graphOptions.scaleGeneral * Globals.guiPlotSvgWidth;
                    this.svgfile.Height = graphOptions.scaleGeneral * Globals.guiPlotSvgHeight;
                    this.PlotMain.Width = this.svgfile.Width + Globals.guiPlotWindowWidth - Globals.guiPlotSvgWidth;
                    this.PlotMain.Height = this.svgfile.Height + Globals.guiPlotWindowHeight - Globals.guiPlotSvgHeight;
                }
                radioButton_n1.IsChecked = true;
                radioButton_n2.IsChecked = false;
                radioButton_d.IsChecked = false;
                radioButton_p.IsChecked = false;
                radioButton_dp.IsChecked = false;
                radioButton_m.IsChecked = false;
                radioButton_q.IsChecked = false;
                radioButton_mp.IsChecked = false;
            }
            finally
            {
                Globals.disableRadioButtons = 0;
            }

            this.Left = Globals.guiGraphWindowLeftDistance;
            this.Top = Globals.guiGraphWindowTopDistance;

            Globals.disableRadioButtons = 1;
            try
            {
                SetControls(graphOptions, null);
            }
            finally
            {
                Globals.disableRadioButtons = 0;
            }

            webBrowser.Source = new Uri(graphOptions.emfName);
        }

        private void SetControls(GraphOptions graphOptions, RefreshHelper refresh)
        {
            bool isQOrM = false;
            if (graphOptions.o != null && (graphOptions.o.t1.freq == EFreq.Q || graphOptions.o.t1.freq == EFreq.M)) isQOrM = true;

            CheckBox_ref.IsChecked = false;
            CheckBox_ref.IsEnabled = true;
            CheckBox_ref.Opacity = 1d;

            CheckBox_all.IsChecked = false;
            CheckBox_all.IsEnabled = true;
            CheckBox_all.Opacity = 1d;

            CheckBox_log.IsChecked = false;
            CheckBox_log.IsEnabled = true;
            CheckBox_log.Opacity = 1d;

            CheckBox_yoy.IsChecked = false;
            CheckBox_yoy.IsEnabled = true;
            CheckBox_yoy.Opacity = 1d;

            TextBox_period.Text = StringPeriod(graphOptions);

            CheckBox_index.IsChecked = false;
            CheckBox_index.IsEnabled = true;
            CheckBox_index.Opacity = 1d;

            radioButton_n1.IsChecked = false;
            radioButton_n1.IsEnabled = true;
            radioButton_n1.Opacity = 1d;

            radioButton_n2.IsChecked = false;
            radioButton_n2.IsEnabled = true;
            radioButton_n2.Opacity = 1d;

            radioButton_d.IsChecked = false;
            radioButton_d.IsEnabled = true;
            radioButton_d.Opacity = 1d;

            radioButton_p.IsChecked = false;
            radioButton_p.IsEnabled = true;
            radioButton_p.Opacity = 1d;

            radioButton_dp.IsChecked = false;
            radioButton_dp.IsEnabled = true;
            radioButton_dp.Opacity = 1d;

            radioButton_m.IsChecked = false;
            radioButton_m.IsEnabled = true;
            radioButton_m.Opacity = 1d;

            radioButton_q.IsChecked = false;
            radioButton_q.IsEnabled = true;
            radioButton_q.Opacity = 1d;

            radioButton_mp.IsChecked = false;
            radioButton_mp.IsEnabled = true;
            radioButton_mp.Opacity = 1d;

            string opRawLowerStart = "";
            if (graphOptions.code != null) opRawLowerStart = graphOptions.code.ToLower();
            string opHere = opRawLowerStart;

            bool isR = false;
            bool isA = false;
            bool isL = false;
            bool isI = false;
            bool isYoy = false;

            if (refresh != null)
            {
                isR = refresh.isRef == true;
                isA = refresh.isAll == true;
                isL = refresh.isLog == true;
                isI = refresh.isIndex == true;
                isYoy = refresh.isYoy == true;
                opHere = refresh.op.ToLower();
            }
            else
            {
                if (opRawLowerStart.EndsWith("l")) isL = true;
                if (opRawLowerStart.StartsWith("r")) isR = true;
                if (opRawLowerStart.StartsWith("a")) isA = true;
                if (!graphOptions.index.IsNull()) isI = true;  //also true for <i=...>.
                if (graphOptions.yoy) isYoy = true;  //also true for <i=...>.
            }

            if (opHere.EndsWith("l"))
            {
                opHere = opHere.Substring(0, opHere.Length - 1);
            }
            if (opHere.StartsWith("r") || opHere.StartsWith("a"))
            {
                opHere = opHere.Substring(1);
            }

            if (isR) CheckBox_ref.IsChecked = true;
            if (isA) CheckBox_all.IsChecked = true;
            if (isL) CheckBox_log.IsChecked = true;
            if (isI) CheckBox_index.IsChecked = true;
            if (isYoy) CheckBox_yoy.IsChecked = true;
            if (G.Equal(opHere, "") || G.Equal(opHere, "n")) radioButton_n1.IsChecked = true;
            else if (G.Equal(opHere, "n1")) radioButton_n1.IsChecked = true;
            else if (G.Equal(opHere, "n2")) radioButton_n2.IsChecked = true;
            else if (G.Equal(opHere, "d")) radioButton_d.IsChecked = true;
            else if (G.Equal(opHere, "p")) radioButton_p.IsChecked = true;
            else if (G.Equal(opHere, "dp")) radioButton_dp.IsChecked = true;
            else if (G.Equal(opHere, "m")) radioButton_m.IsChecked = true;
            else if (G.Equal(opHere, "q")) radioButton_q.IsChecked = true;
            else if (G.Equal(opHere, "mp")) radioButton_mp.IsChecked = true;

            if (CheckBox_log.IsChecked == true)
            {
                radioButton_p.IsEnabled = false;
                radioButton_p.Opacity = 0.5;
                radioButton_dp.IsEnabled = false;
                radioButton_dp.Opacity = 0.5;
                radioButton_m.IsEnabled = false;
                radioButton_m.Opacity = 0.5;
                radioButton_q.IsEnabled = false;
                radioButton_q.Opacity = 0.5;
                radioButton_mp.IsEnabled = false;
                radioButton_mp.Opacity = 0.5;
            }

            if (CheckBox_index.IsChecked == true)
            {
                radioButton_p.IsEnabled = false;
                radioButton_p.Opacity = 0.5;
                radioButton_dp.IsEnabled = false;
                radioButton_dp.Opacity = 0.5;
                radioButton_q.IsEnabled = false;
                radioButton_q.Opacity = 0.5;
                radioButton_mp.IsEnabled = false;
                radioButton_mp.Opacity = 0.5;
            }

            if (CheckBox_yoy.IsChecked == true)
            {
                radioButton_n1.IsEnabled = false;
                radioButton_n1.Opacity = 0.5;
                radioButton_n2.IsEnabled = false;
                radioButton_n2.Opacity = 0.5;
                radioButton_m.IsEnabled = false;
                radioButton_m.Opacity = 0.5;
                radioButton_q.IsEnabled = false;
                radioButton_q.Opacity = 0.5;
                radioButton_mp.IsEnabled = false;
                radioButton_mp.Opacity = 0.5;
            }

            if (CheckBox_ref.IsChecked == true)
            {
                radioButton_m.IsEnabled = false;
                radioButton_m.Opacity = 0.5;
                radioButton_q.IsEnabled = false;
                radioButton_q.Opacity = 0.5;
                radioButton_mp.IsEnabled = false;
                radioButton_mp.Opacity = 0.5;
            }

            if (CheckBox_all.IsChecked == true)
            {
                radioButton_m.IsEnabled = false;
                radioButton_m.Opacity = 0.5;
                radioButton_q.IsEnabled = false;
                radioButton_q.Opacity = 0.5;
                radioButton_mp.IsEnabled = false;
                radioButton_mp.Opacity = 0.5;
            }

            if (radioButton_m.IsChecked == true || radioButton_q.IsChecked == true || radioButton_mp.IsChecked == true)
            {
                CheckBox_ref.IsEnabled = false;
                CheckBox_ref.Opacity = 0.5;
                CheckBox_all.IsEnabled = false;
                CheckBox_all.Opacity = 0.5;
            }

            if (radioButton_p.IsChecked == true || radioButton_dp.IsChecked == true || radioButton_q.IsChecked == true || radioButton_mp.IsChecked == true)
            {
                CheckBox_index.IsEnabled = false;
                CheckBox_index.Opacity = 0.5;
            }

            if (radioButton_n1.IsChecked == true || radioButton_n2.IsChecked == true || radioButton_m.IsChecked == true || radioButton_q.IsChecked == true || radioButton_mp.IsChecked == true)
            {
                CheckBox_yoy.IsEnabled = false;
                CheckBox_yoy.Opacity = 0.5;
            }

            if (radioButton_p.IsChecked == true || radioButton_dp.IsChecked == true || radioButton_m.IsChecked == true || radioButton_q.IsChecked == true || radioButton_mp.IsChecked == true)
            {
                CheckBox_log.IsEnabled = false;
                CheckBox_log.Opacity = 0.5;
            }

            if (CheckBox_yoy.IsChecked == false && !isQOrM)
            {
                //Deactivate it if not chosen with <yoy> and only annual freq is shown
                CheckBox_yoy.IsEnabled = false;
                CheckBox_yoy.Opacity = 0.5;
            }
        }

        private string StringPeriod(GraphOptions graphOptions)
        {
            return graphOptions.tStart.ToString() + " " + graphOptions.tEnd.ToString();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (_shown) return;
            _shown = true;
            this._graphOptions.windowIsShown = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Globals.guiGraphWindowTopDistance = (int)Math.Max(1, this.Top);
            Globals.guiGraphWindowLeftDistance = (int)Math.Max(1, this.Left);
            try
            {
                if (Globals.windowsPlot != null && this != null) Globals.windowsPlot.Remove(this);
            }
            catch { }
        }        

        private void Button_copy(object sender, RoutedEventArgs e)
        {
            // Copy the .svg file to the clipboard for use in e.g. Word
            //string[] ss = new string[1];
            //ss[0] = this.graphOptions.emfName;
            //IDataObject iData = new DataObject(DataFormats.FileDrop, ss);
            //Clipboard.SetDataObject(iData, true);
            string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling);
            Clipboard.SetText(plotName);
        }

        private void Button_save(object sender, RoutedEventArgs e)
        {
            //Copy the .svg file to file for later use in e.g. Word
            string input = "gekkoplot";
            string inputLast = "svg";
            string name2 = Program.Add1ToFileName(input, inputLast, Program.options.folder_working);
            string enddir = Program.options.folder_working + "\\" + name2;
            string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling);
            Program.WaitForFileCopy(plotName, enddir);
            this.label1.Text = name2;
            Program.DelayAction(4000, new Action(() => { try { if (this.label1.Text == name2) this.label1.Text = ""; } catch { } }));
            //this.label2.Text = "saved in working folder";
        }

        private void Button_saveas(object sender, RoutedEventArgs e)
        {
            string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling);

            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "svg files (*.svg)|*.svg|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                InitialDirectory = Program.options.folder_working
            };
            if (saveFileDialog1.ShowDialog() == true)
            {
                Program.WaitForFileCopy(plotName, saveFileDialog1.FileName);                
            }
        }

        private string CreatePlotFileInBackground(double fontScaling, double sizeScaling)
        {
            RefreshHelper refresh = new RefreshHelper();
            refresh.op = GetOperator();
            refresh.isLog = CheckBox_log.IsChecked;
            refresh.isRef = CheckBox_ref.IsChecked;
            refresh.isAll = CheckBox_all.IsChecked;
            refresh.isIndex = CheckBox_index.IsChecked;
            refresh.isYoy= CheckBox_yoy.IsChecked;
            refresh.period = TextBox_period.Text;
            refresh.fontScaling = fontScaling;
            refresh.sizeScaling = sizeScaling;
            refresh.isRefreshing = true;  //so we do not get a new plot window
            refresh.isButton = true;
            string plotName = Refresh(new GraphHelper(refresh), false);
            return plotName;
        }

        private void Button_refresh(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Refresh()
        {            
            RefreshHelper refresh = new RefreshHelper();
            refresh.op = GetOperator();
            refresh.isLog = CheckBox_log.IsChecked;
            refresh.isRef = CheckBox_ref.IsChecked;
            refresh.isAll = CheckBox_all.IsChecked;
            refresh.isIndex= CheckBox_index.IsChecked;
            refresh.isYoy = CheckBox_yoy.IsChecked;
            refresh.isRefreshing = true;  //so we do not get a new plot window                        
            refresh.period = TextBox_period.Text;

            try
            {
                refresh.isRefreshing = true;
                Refresh(new GraphHelper(refresh), true);
                _refresh = refresh.Clone();  //store it, so we can revert if it fails
            }
            catch
            {
                refresh = _refresh.Clone();
                refresh.isRefreshing = true;
                refresh.period = StringPeriod(_graphOptions);
                Refresh(new GraphHelper(refresh), true);  //using the old refresh object that should work.
                _graphOptions.code = refresh.op;
            };

            O.GetPeriods(_graphOptions.tStart.freq, refresh.period, out _graphOptions.tStart, out _graphOptions.tEnd);
            Globals.disableRadioButtons = 1;
            try
            {
                SetControls(_graphOptions, refresh);
            }
            finally
            {
                Globals.disableRadioButtons = 0;
            }
        }

        private string Refresh(GraphHelper gh, bool updatePlotWindow)
        {
            string fileName = Globals.printStorageAsFunc[this._graphOptions.printStorageAsFuncCounter](gh);
            if(updatePlotWindow) webBrowser.Source = new Uri(fileName);
            return fileName;
        }

        private string GetOperator()
        {
            string op = "n";
            if (radioButton_n1.IsChecked == true) op = "n1";
            else if (radioButton_n2.IsChecked == true) op = "n2";
            else if (radioButton_d.IsChecked == true) op = "d";
            else if (radioButton_p.IsChecked == true) op = "p";
            else if (radioButton_dp.IsChecked == true) op = "dp";
            else if (radioButton_m.IsChecked == true) op = "m";
            else if (radioButton_q.IsChecked == true) op = "q";
            else if (radioButton_mp.IsChecked == true) op = "mp";
            return op;
        }

        private void CheckBox_ref_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Globals.disableRadioButtons = 1;
                try
                {
                    CheckBox_all.IsChecked = false;
                }
                finally
                {
                    Globals.disableRadioButtons = 0;
                }
                Refresh();
            }
        }

        private void CheckBox_ref_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_all_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Globals.disableRadioButtons = 1;
                try
                {
                    CheckBox_ref.IsChecked = false;
                }
                finally
                {
                    Globals.disableRadioButtons = 0;
                }
                Refresh();
            }
        }

        private void CheckBox_all_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_shares_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_shares_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_index_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_index_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_yoy_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_yoy_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_log_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_log_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_n1_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_d_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_p_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_dp_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_n2_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_m_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_q_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void radioButton_mp_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }
    }
}
