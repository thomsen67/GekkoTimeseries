using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gekko
{

    public class RefreshHelper
    {
        public string op = "";  //can also be n1/n2 (left or right n)
        public bool? isLog = false;
        public bool? isYoy = false;
        public bool? isPoints = true;  //This is default
        public string period = "";
        public string scaleCode = "";
        public bool? isIndex = null;
        public bool? isRef = false;
        public bool? isAll = false;
        public double fontScaling = 1d;
        public double sizeScaling = 1d;
        public bool isRefreshing = false;
        public bool isButton = false;
        public string fileName = null;  //svg, emf, png, pdf

        public RefreshHelper Clone()
        {
            RefreshHelper r = new RefreshHelper();
            r.op = this.op;
            r.isLog = this.isLog;
            r.isYoy = this.isYoy;
            r.isPoints = this.isPoints;
            r.period = this.period;
            r.scaleCode = this.scaleCode;
            r.isIndex = this.isIndex;
            r.isRef = this.isRef;
            r.isAll = this.isAll;
            r.fontScaling = this.fontScaling;
            r.isRefreshing = this.isRefreshing;
            r.fileName = this.fileName;
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
                ScaleWindow(graphOptions.scaleGeneral);                
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

            try
            {
                GetRefreshValuesFromControls(_refresh);  //To get some sensible values into this, so that if the PLOT window crashes now, it can be restored to a sensible state.
            }
            catch
            {
                //Should normally not crash
            }
            O.DatabankSearchHelper2(this);
        }

        /// <summary>
        /// Use 1 for 100%, 1 is what the PLOT design is built around, but other sizes work ok, too.
        /// </summary>
        /// <param name="scale"></param>
        private void ScaleWindow(double scale)
        {            
            this.svgfile.Width = (scale * Globals.guiPlotSvgWidth + Globals.guiPlotExtraWidth);
            this.svgfile.Height = (scale * Globals.guiPlotSvgHeight + Globals.guiPlotExtraHeight);
            this.PlotMain.Width = Math.Max(this.svgfile.Width + Globals.guiPlotWindowWidth - Globals.guiPlotSvgWidth, Globals.guiPlotExtraMinimumWidth);
            this.PlotMain.Height = Math.Max(this.svgfile.Height + Globals.guiPlotWindowHeight - Globals.guiPlotSvgHeight, Globals.guiPlotExtraMinimumHeigth);
            SetZoomText();
        }

        private void SetControls(GraphOptions graphOptions, RefreshHelper refresh)
        {
            bool isQOrM = false;
            if (graphOptions == null) isQOrM = true;  //We must assume so to activate YoY
            else if (graphOptions.tStart.freq == EFreq.Q || graphOptions.tStart.freq == EFreq.M) isQOrM = true;            

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

            CheckBox_points.IsChecked = false;
            CheckBox_points.IsEnabled = true;
            CheckBox_points.Opacity = 1d;

            TextBox_period.Text = StringPeriod(graphOptions);
            TextBox_scaleCode.Text = StringScale(graphOptions);

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
            bool? isPoints = null;

            if (refresh != null)
            {
                isR = refresh.isRef == true;
                isA = refresh.isAll == true;
                isL = refresh.isLog == true;
                isI = refresh.isIndex == true;
                isYoy = refresh.isYoy == true;
                isPoints = refresh.isPoints;
                opHere = refresh.op.ToLower();
            }
            else
            {
                if (opRawLowerStart.EndsWith("l")) isL = true;
                if (opRawLowerStart.StartsWith("r")) isR = true;
                if (opRawLowerStart.StartsWith("a")) isA = true;
                if (!graphOptions.index.IsNull()) isI = true;  //also true for <i=...>.
                if (graphOptions.yoy) isYoy = true;  //also true for <i=...>.
                isPoints = graphOptions.points;
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
            if (isPoints != false) CheckBox_points.IsChecked = true;
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
                //Too pedantic
                //radioButton_p.IsEnabled = false;
                //radioButton_p.Opacity = 0.5;
                //radioButton_dp.IsEnabled = false;
                //radioButton_dp.Opacity = 0.5;
                //radioButton_q.IsEnabled = false;
                //radioButton_q.Opacity = 0.5;
                //radioButton_mp.IsEnabled = false;
                //radioButton_mp.Opacity = 0.5;
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
                //Too pedantic
                //CheckBox_index.IsEnabled = false;
                //CheckBox_index.Opacity = 0.5;
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

        private string StringScale(GraphOptions graphOptions)
        {
            if (graphOptions.scaleCode == null) return "1";
            return graphOptions.scaleCode;
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
            CopySvg();
        }

        private void CopyComboBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = sender as ComboBoxItem;
            if (selectedItem != null)
            {
                // Get the content of the selected item
                string selectedValue = selectedItem.Content.ToString();

                // You can use a switch statement for more complex logic
                switch (selectedValue)
                {
                    case "Png":
                        CopyPng();
                        break;
                    case "Emf":
                        CopyEmf();
                        break;
                    case "Svg (link)":
                        CopySvg();
                        break;
                }
            }

        }

        //private void CopyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // Get the selected ComboBoxItem
        //    var selectedItem = CopyComboBox.SelectedItem as ComboBoxItem;

        //    if (selectedItem != null)
        //    {
        //        // Get the content of the selected item
        //        string selectedValue = selectedItem.Content.ToString();

        //        // You can use a switch statement for more complex logic
        //        switch (selectedValue)
        //        {
        //            case "Png":
        //                CopyPng();
        //                break;
        //            case "Emf":
        //                CopyEmf();
        //                break;
        //            case "Svg (link)":
        //                CopySvg();
        //                break;
        //        }
        //    }
        //}

        private void Button_copy_png(object sender, RoutedEventArgs e)
        {
            CopyPng();
        }

        private void CopySvg()
        {            
            try
            {
                string inputName = null;
                string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling, inputName);                
                Clipboard.SetText(plotName);
            }
            catch
            {
                MessageBox.Show("Due to errors, the plot could not be copied.");
            }
        }

        private void CopyPng()
        {
            try
            {                             
                string inputName = System.IO.Path.Combine(Globals.localTempFilesLocationGnuplot, "tempfiles", "temp.png");  //to indicate .png type                
                string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling, inputName);
                
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(plotName))
                {
                    System.Windows.Forms.Clipboard.SetImage(image);
                }
            }
            catch
            {
                MessageBox.Show("Due to errors, the plot could not be copied.");
            }
        }

        private void CopyEmf()
        {
            try
            {
                string inputName = System.IO.Path.Combine(Globals.localTempFilesLocationGnuplot, "tempfiles", "temp.emf");  //to indicate .png type               
                string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling, inputName);
                if (true)
                {
                    //Copy the .emf file to the clipboard for use in e.g. Word
                    //This is a file drop (string path on clipboard), but it works for Word and PowerPoint.
                    //It ought to be the emf itself on the clipboard, but can't get it to work.
                    string[] ss = new string[1];
                    ss[0] = plotName;
                    IDataObject iData = new DataObject(DataFormats.FileDrop, ss);
                    Clipboard.SetDataObject(iData, true);
                }                
            }
            catch
            {
                MessageBox.Show("Due to errors, the plot could not be copied.");
            }
        }

        private void SaveComboBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = sender as ComboBoxItem;
            if (selectedItem != null)                
            {
                // Get the content of the selected item
                string selectedValue = selectedItem.Content.ToString();

                // You can use a switch statement for more complex logic
                switch (selectedValue)
                {
                    case "Save":
                        Save();
                        break;
                    case "Save As":
                        SaveAs();
                        break;                    
                }
            }
        }

        private void Button_save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            //Copy the .svg file to file for later use in e.g. Word
            string input = "gekkoplot";
            string inputLast = "svg";
            string name2 = Program.Add1ToFileName(input, inputLast, Program.options.folder_working);
            string enddir = Program.options.folder_working + "\\" + name2;
            try
            {
                string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling, null);
                Program.WaitForFileCopy(plotName, enddir);
                this.label1.Text = name2;
                Program.DelayAction(4000, new Action(() => { try { if (this.label1.Text == name2) this.label1.Text = ""; } catch { } }));
            }
            catch
            {
                MessageBox.Show("Due to errors, the plot could not be saved.");
            }
        }

        private void Button_search(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("One or more variables in the plot were found in a databank in position 2 or lower in the databank list (F2 window). To switch databank searching off, use 'option databank search = no', in which case an error would have been issued instead. Beware of unintended missing variables in the first-position databank, which in this case is not empty. (Note that 'Ref' variables are never searched for, but are always taken from the databank corresponding to the 'REF' position in the F2 databank list).");
        }

        private void SaveAs()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "svg files (*.svg)|*.svg|emf files (*.emf)|*.emf|png files (*.png)|*.png|pdf files (*.pdf)|*.pdf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                InitialDirectory = Program.options.folder_working
            };
            if (saveFileDialog1.ShowDialog() == true)
            {
                string extension = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                if (!(G.Equal(extension, ".svg") || G.Equal(extension, ".emf") || G.Equal(extension, ".png") || G.Equal(extension, ".pdf")))
                {
                    MessageBox.Show("Expected file type to be '.svg', '.emf', '.png' or '.pdf' -- not '" + extension + "'. No file produced.");
                    return;
                }
                try
                {
                    string plotName = CreatePlotFileInBackground(Globals.guiPlotFontScaling, Globals.guiPlotSizeScaling, saveFileDialog1.FileName);
                }
                catch
                {
                    MessageBox.Show("Due to errors, the plot could not be saved.");
                }
            }
        }

        private string CreatePlotFileInBackground(double fontScaling, double sizeScaling, string fileName)
        {            
            RefreshHelper refresh = new RefreshHelper();
            refresh.op = GetOperator();
            refresh.isLog = CheckBox_log.IsChecked;
            refresh.isRef = CheckBox_ref.IsChecked;
            refresh.isAll = CheckBox_all.IsChecked;
            refresh.isIndex = CheckBox_index.IsChecked;
            refresh.isYoy= CheckBox_yoy.IsChecked;
            refresh.isPoints= CheckBox_points.IsChecked;
            refresh.period = TextBox_period.Text;
            refresh.scaleCode = TextBox_scaleCode.Text;
            refresh.fontScaling = fontScaling;
            refresh.sizeScaling = sizeScaling;
            refresh.isRefreshing = true;  //so we do not get a new plot window
            refresh.isButton = true; 
            refresh.fileName = fileName;            
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
            GetRefreshValuesFromControls(refresh);

            try
            {
                refresh.isRefreshing = true;
                Refresh(new GraphHelper(refresh), true);
                _refresh = refresh.Clone();  //store it, so we can revert if it fails
            }
            catch
            {
                //When we get here Refresh(GraphHelper, bool) has crashed, and if so, no new  webBrowser.Source will
                //have been set. So there is no need for a new Refresh(GraphHelper, bool) here, because the svg file
                //and the window will not have been changed.
                refresh = _refresh.Clone();
                refresh.isRefreshing = true;
                refresh.period = StringPeriod(_graphOptions);
                refresh.scaleCode = StringScale(_graphOptions);
                _graphOptions.code = refresh.op;
            };

            O.GetPeriods(_graphOptions.tStart.freq, refresh.period, out _graphOptions.tStart, out _graphOptions.tEnd);
            O.GetScale(refresh.scaleCode, out _graphOptions.scaleCode);
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

        private void GetRefreshValuesFromControls(RefreshHelper refresh)
        {
            refresh.op = GetOperator();
            refresh.isLog = CheckBox_log.IsChecked;
            refresh.isRef = CheckBox_ref.IsChecked;
            refresh.isAll = CheckBox_all.IsChecked;
            refresh.isIndex = CheckBox_index.IsChecked;
            refresh.isYoy = CheckBox_yoy.IsChecked;
            refresh.isPoints = CheckBox_points.IsChecked;
            refresh.isRefreshing = true;  //so we do not get a new plot window                        
            refresh.period = TextBox_period.Text;
            refresh.scaleCode = TextBox_scaleCode.Text;
        }

        private string Refresh(GraphHelper gh, bool updatePlotWindow)
        {            
            string fileName = Globals.printStorageAsFunc[this._graphOptions.printStorageAsFuncCounter](gh);
            if (updatePlotWindow)
            {
                webBrowser.Source = new Uri(fileName);                
                O.DatabankSearchHelper2(this);
            }
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

        private void CheckBox_points_Checked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
        }

        private void CheckBox_points_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Globals.disableRadioButtons == 0)
            {
                Refresh();
            }
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


        // Called when Enter is pressed in the editable box
        private void TextBox_period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }

        private void TextBox_scaleCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Refresh();
            }
        }

        private void ZoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ZoomComboBox.IsDropDownOpen && ZoomComboBox.SelectedItem is ComboBoxItem item)
            {
                ApplyZoomFromText(item.Content.ToString());
            }
        }

        // Called when Enter is pressed in the editable box
        private void ZoomComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = ZoomComboBox.Text;
                ApplyZoomFromText(input);                
            }
        }

        private void ApplyZoomFromText(string text)
        {
            // Remove percent sign if present
            string numericPart = text.Replace("%", "").Trim();

            if (double.TryParse(numericPart, out double zoom))
            {
                if (zoom >= 10 && zoom <= 1000)
                {
                    Globals.guiGraphZoom = (int)zoom;
                    Refresh();
                    ScaleWindow(Globals.guiGraphZoom / 100d);                    
                }
                else
                {
                    MessageBox.Show("Zoom must be between 10% and 1000%");
                }
            }
            else
            {
                MessageBox.Show("Invalid zoom format");
            }
        }

        private void SetZoomText()
        {
            ZoomComboBox.Text = Globals.guiGraphZoom + "%";
        }        
    }
}
