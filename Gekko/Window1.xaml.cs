/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Globalization;



namespace Gekko
{

    public delegate void CloseDelegate2();
    
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        
        public enum GekkoTableTypes { 
            TableContent, Left, Top, UpperLeft, Unknown
        };

        
        
        public int frozenRows=0;
        public int frozenCols=0;
        
        public bool isClosing = false;
        public bool isInitializing = false; //a bit hacky, to handle radiobutton1 firing a clicked event when initializing
        
        public Grid _grid = null;

        public StatusBar _status = null;
        public TextBlock _statusText = null;

        public DecompOptions decompOptions = null;

        public List<string> Customers = new List<string>();        
        
        public void SetRadioButtons() {
            if (this.decompOptions.isSubWindow)
            {
                if (this.decompOptions.guiDecompIsRaw)
                {
                    if (this.decompOptions.guiDecompTransformationCode == "n")
                    {
                        radioButton1.IsChecked = true;
                    }
                    else if(this.decompOptions.guiDecompTransformationCode == "d")
                    {
                        radioButton2.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "p")
                    {
                        radioButton4.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "dp")
                    {
                        radioButton9.IsChecked = true;
                    }
                    //else if (this.decompOptions.guiDecompTransformationCode == "n")
                    //{
                    //    radioButton21.IsChecked = true;
                    //}
                    else if (this.decompOptions.guiDecompTransformationCode == "m")
                    {
                        radioButton22.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "q")
                    {
                        radioButton24.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "mp")
                    {
                        radioButton29.IsChecked = true;
                    }
                }
                else {
                    if (this.decompOptions.guiDecompTransformationCode == "n")
                    {
                        radioButton5.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "d")
                    {
                        radioButton6.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "p")
                    {
                        radioButton8.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "dp")
                    {
                        radioButton10.IsChecked = true;
                    }


                    //else if (this.decompOptions.guiDecompTransformationCode == "n")
                    //{
                    //    radioButton25.IsChecked = true;
                    //}
                    else if (this.decompOptions.guiDecompTransformationCode == "m")
                    {
                        radioButton26.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "q")
                    {
                        radioButton28.IsChecked = true;
                    }
                    else if (this.decompOptions.guiDecompTransformationCode == "mp")
                    {
                        radioButton30.IsChecked = true;
                    }
                }

                if (this.decompOptions.guiDecompIsShares)
                {
                    checkBox1.IsChecked = true;
                }

                if (this.decompOptions.showErrors)
                {
                    checkBoxErrors.IsChecked = true;
                }
                
                if (this.decompOptions.guiDecompIsBaseline)
                {
                    checkBox2.IsChecked = true;
                }
            }
            else
            {
                //All these buttons are in the "Decomp" columns, codes are from e.g. UDVALG<p> or UDVALG<q> calls from command lines/files.
                if (G.Equal(decompOptions.prtOptionLower, "d"))
                {
                    radioButton6.IsChecked = true;
                }
                if (G.Equal(decompOptions.prtOptionLower, "p"))
                {
                    radioButton8.IsChecked = true;
                }
                if (G.Equal(decompOptions.prtOptionLower, "m"))
                {
                    radioButton26.IsChecked = true;
                }
                if (G.Equal(decompOptions.prtOptionLower, "q"))
                {
                    radioButton28.IsChecked = true;
                }
            }
        }

        public Window1(DecompOptions decompOptions)
        {
            
            this.decompOptions = decompOptions;
            
            isInitializing = true;
            InitializeComponent();
            isInitializing = false;

            Canvas.SetTop(this.frezenBorder, Globals.guiTableCellHeight);
            Canvas.SetLeft(this.frezenBorder2,Globals.guiTableCellWidth);
            this.gridUpperLeft.Width = Globals.guiTableCellWidth;
            this.gridUpperLeft.Height = Globals.guiTableCellHeight;
            

            this.frozenRows = Globals.freezeDecompRows;
            this.frozenCols = Globals.freezeDecompCols;

            this.Top = Globals.guiDecompWindowTopDistance;
            this.Left = Globals.guiDecompWindowLeftDistance;
                        
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

            this.KeyDown += new KeyEventHandler(Window_KeyDown); //new System.Windows.Forms.KeyEventHandler(this.radioButton1_KeyDown);

            this.decompOptions.guiDecompChangedCells.Clear();
            this.decompOptions.guiDecompIsSelecting = false;
            this.decompOptions.guiDecompIsSelectingAll = false;

            if (true)
            {
                var fd = new FlowDocument();

                //Grid g = new Grid();
                
                _grid = grid1;

                ContextMenu mainMenu = new ContextMenu();
                MenuItem item1 = new MenuItem();
                item1.Header = "Paste";
                item1.Foreground = Brushes.Black;
                item1.Background = Brushes.Transparent;
                //mainMenu.Items.Add(item1);
                MenuItem item2 = new MenuItem();
                item2.Header = "Clear";
                //mainMenu.Items.Add(item2);
                //create image at runtime and attach to menu at runtime
                BitmapImage copyimage = new BitmapImage();
                copyimage.BeginInit();
                Uri myUri = new Uri("Copy.png", UriKind.RelativeOrAbsolute);
                copyimage.UriSource = myUri;
                copyimage.EndInit();
                Image iconImage = new Image();
                iconImage.Source = copyimage;
                MenuItem item3 = new MenuItem();
                item3.Header = "Copy";
                item3.Icon = iconImage;
                item3.Click += new RoutedEventHandler(item_Click);
                mainMenu.Items.Add(item3);

                grid1.ContextMenu = mainMenu;               

                //will be done in RecalcCellsWithNewType()
                //Table table = this.decompOptions.guiDecompValues;
                //MakeTable(table);                
            }
            _status = status;
            _statusText = statusText;
            //this.decompOptions.guiDecompTransformationCode = "n";            
        }

        private void CreateGridRowsAndColumns(Grid g, Table table, GekkoTableTypes type)
        {
            if (type == GekkoTableTypes.TableContent)
            {
                for (int i = 1 + this.frozenRows; i <= table.GetRowMaxNumber(); i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
                for (int j = 1 + this.frozenCols; j <= table.GetColMaxNumber(); j++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }

                g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Globals.guiTableCellHeight) });  //otherwise, the last row does not show up in gui...
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Globals.guiTableCellWidth) });  //otherwise, the last col does not show up in gui...


            }
            else if (type == GekkoTableTypes.Top)
            {
                for (int i = 1; i <= 1; i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
                for (int j = 1 + this.frozenCols; j <= table.GetColMaxNumber(); j++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }
            }
            else if (type == GekkoTableTypes.Left)
            {
                for (int i = 1 + frozenRows; i <= table.GetRowMaxNumber(); i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
                for (int j = 1; j <= 1; j++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }
            }
            else if (type == GekkoTableTypes.UpperLeft)
            {                
                g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });                
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });                
            }
        }

        private void PutTableIntoGrid(Grid g, Table table, GekkoTableTypes type, DecompOptions decompOptions)
        {
            int offsetRow = 0;
            int offsetCol = 0;
            int startRow = 0;
            int endRow = 0;
            int startCol = 0;
            int endCol = 0;            

            if (type == GekkoTableTypes.TableContent)
            {
                startRow = this.frozenRows + 1;
                startCol = this.frozenCols + 1;
                endRow = table.GetRowMaxNumber();
                endCol = table.GetColMaxNumber();
                offsetRow = this.frozenRows;
                offsetCol = this.frozenCols;
            }
            else if (type == GekkoTableTypes.Top)
            {
                startRow = 1;
                startCol = frozenCols + 1;
                endRow = this.frozenRows;
                endCol = table.GetColMaxNumber();
                offsetRow = 0;
                offsetCol = frozenCols;
            }
            else if (type == GekkoTableTypes.Left)
            {
                startRow = frozenRows + 1;
                startCol = 1;
                endRow = table.GetRowMaxNumber();
                endCol = this.frozenCols;
                offsetRow = frozenRows;
                offsetCol = 0;
            }
            else if (type == GekkoTableTypes.UpperLeft)
            {
                startRow = 1;
                endRow = 1;
                startCol = 1;
                endCol = 1;                
            }

            if (true)
            {
                for (int i = startRow; i <= endRow; i++)
                {
                    for (int j = startCol; j <= endCol; j++)
                    {
                        Cell c = table.Get(i, j);
                        if (c == null)
                        {
                            AddCell(g, i - 1 - offsetRow, j - 1 - offsetCol, "", false, type, null);  //transparent
                            continue;
                        }
                        string s = "";
                        bool leftAlign = false;

                        string ss = c.numberFormat;
                        string[] sss = ss.Split('.');
                        string ssss = sss[sss.Length - 1];
                        int xx = int.Parse(ssss);

                        if (c.cellType == CellType.Text)
                        {
                            s = c.CellText.TextData[0];
                            leftAlign = true;
                        }
                        else if (c.cellType == CellType.Number)
                        {
                            s = G.UpdprtFormat(c.number, xx, false);
                        }
                        else if (c.cellType == CellType.Date) s = c.date;

                        if (type == GekkoTableTypes.TableContent && decompOptions.dream != null && (i == endRow || j == endCol)) c.backgroundColor = "Linen";

                        AddCell(g, i - 1 - offsetRow, j - 1 - offsetCol, s, leftAlign, type, c.backgroundColor);
                    }
                }
            }
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {            
            // Ctrl + C
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.C))
            {                
                CopyToClipboard(GekkoTableTypes.TableContent);  //well, seems it is only possible to Ctrl+C on this part of table anyway
            }
            else if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.A))
            {
                this.scrollView1.ScrollToHome();                
                _statusText.Text = "Use Ctrl-C and Ctrl-V to copy-paste into e.g. Excel";  //because sums are not meaningful anyway
                
                DefaultGrid(this.grid1);
                this.canvasBorder.BorderBrush = Brushes.Black;
                this.canvasBorder.BorderThickness = new Thickness(3);
                this.decompOptions.guiDecompIsSelectingAll = true;
             
                //this.decompOptions.guiDecompLastClickedRow = 0;
                //this.decompOptions.guiDecompLastClickedCol = 0;
                //Select(_grid, _grid.ColumnDefinitions.Count - 1, _grid.RowDefinitions.Count - 1);
            }
        }

        void item_Click(object sender, RoutedEventArgs e)
        {            
            CopyToClipboard(GekkoTableTypes.TableContent);  //well, seems it is only possible to right-click on this part of table anyway
        }

        private void CopyToClipboard(GekkoTableTypes type)
        {
            string s = "";
            int r0 = -12345;
            int r1 = -12345;
            int c0 = -12345;
            int c1 = -12345;

            if (this.decompOptions.guiDecompIsSelectingAll)
            {
                r0 = 1;
                r1 = this.decompOptions.guiDecompValues.GetRowMaxNumber();
                c0 = 1;
                c1 = this.decompOptions.guiDecompValues.GetColMaxNumber();
            }
            else
            {
                CoordConversion(out r0, out c0, type, this.decompOptions.guiDecompSelectedRowMin, this.decompOptions.guiDecompSelectedColMin);
                CoordConversion(out r1, out c1, type, this.decompOptions.guiDecompSelectedRowMax, this.decompOptions.guiDecompSelectedColMax);
            }

            //Cannot just copy what is seen on the screen if it is a number -- in that case decimals would get lost in Excel.
            
            for (int i = r0; i <= r1; i++)
            {
                for (int j = c0; j <= c1; j++)
                {
                    int x = i; int y = j;                    

                    Cell c = this.decompOptions.guiDecompValues.Get(x, y);
                    if (c == null)
                    {
                    }
                    else
                    {
                        if (c.cellType == CellType.Number)
                        {
                            s += Program.PrepareDataForClipboard(c.number);
                        }
                        else if (c.cellType == CellType.Date)
                        {
                            s += c.date;
                        }
                        else if (c.cellType == CellType.Text)
                        {
                            s += c.CellText.TextData[0];
                        }
                    }
                    if (j < c1) s += "\t";
                }
                s += "\r";
            }

            Clipboard.SetDataObject(s);
        }

        private void AddCell(Grid g, int i, int j, string s, bool leftAlign, GekkoTableTypes type, string backgroundColor)
        {
            GekkoDockPanel dockPanel = new GekkoDockPanel();
            dockPanel.Width = Globals.guiTableCellWidth;
            dockPanel.Height = Globals.guiTableCellHeight;
            var border = new Border();
            TextBlock textBlock = new TextBlock();
            textBlock.Text = s;

            if (type == GekkoTableTypes.UpperLeft)
            {
                textBlock.HorizontalAlignment = HorizontalAlignment.Right;                
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.FontFamily = new FontFamily("Calibri");
                textBlock.FontSize = 13d;
                textBlock.Padding = new Thickness(2, 2, 4, 3);
            }
            else 
            {
                if (type == GekkoTableTypes.Left)
                {
                    string ss = G.ExtractOnlyVariableIgnoreLag(s, Globals.leftParenthesisIndicator);
                    ss = G.PrettifyTimeseriesHash(ss, true, true);

                    bool isEndogenous = false;

                    if (G.Equal(Program.options.model_type, "gams"))
                    {
                        if (Program.HasGamsEquation(ss)) isEndogenous = true;
                    }
                    else
                    {
                        EEndoOrExo e = Program.VariableTypeEndoExo(ss);
                        isEndogenous = e == EEndoOrExo.Endo;
                    }
                    

                    if (isEndogenous || s == Globals.decompText0)
                    {
                        textBlock.MouseEnter += Mouse_Enter;
                        textBlock.MouseLeave += Mouse_Leave;
                        textBlock.MouseDown += Mouse_Down;
                        textBlock.Foreground = new SolidColorBrush(Globals.MediumBlueDecompLink);
                        if (i == 0) textBlock.FontWeight = FontWeights.Bold;
                    }
                }
                textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                if (leftAlign) textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.FontFamily = Globals.decompFontFamily;
                textBlock.FontSize = 13d;
                textBlock.Padding = new Thickness(2, 2, 4, 3);

                dockPanel.type = type;
                dockPanel.MouseDown += Cell_MouseDown;
                dockPanel.MouseUp += Cell_MouseUp;
                dockPanel.MouseEnter += Cell_Enter;
                dockPanel.MouseLeave += Cell_Leave;

                if (type == GekkoTableTypes.TableContent)
                {
                    dockPanel.Background = Brushes.White;
                }
                else dockPanel.Background = new SolidColorBrush(Globals.LightGray);

                if (backgroundColor == "LightYellow")
                {
                    //overrides                
                    dockPanel.originalBackgroundColor = Brushes.LightYellow;                    
                    dockPanel.Background = dockPanel.originalBackgroundColor;
                }
                else if (backgroundColor == "LightRed")
                {
                    //overrides                                    
                    dockPanel.originalBackgroundColor = new SolidColorBrush(Globals.LightRed);
                    dockPanel.Background = dockPanel.originalBackgroundColor;
                }
                else if (backgroundColor == "Linen")
                {
                    //overrides                
                    dockPanel.originalBackgroundColor = Brushes.Ivory;
                    dockPanel.Background = dockPanel.originalBackgroundColor;
                }

                SetDefaultBorder(border);
                SetBorderThickness(g, i, j, border);
            }

            border.Child = textBlock;
            dockPanel.Children.Add(border);
            dockPanel.SetValue(Grid.ColumnProperty, j);
            dockPanel.SetValue(Grid.RowProperty, i);            
            g.Children.Add(dockPanel);            
        }

       

        private void SetBorderThickness(Grid g, int i, int j, Border border)
        {
            int xyz = 0;
            int zyx = 0;
            if (i + 1 == g.RowDefinitions.Count) xyz = 1;
            if (j + 1 == g.ColumnDefinitions.Count) zyx = 1;
            border.BorderThickness = new Thickness(1, 1, zyx, xyz);
        }

        private void SetDefaultBorder(Border border)        {
            //border.BorderBrush = Globals.decompSolidColorBrush;

            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Globals.GrayExcelLine;
            border.BorderBrush = mySolidColorBrush;


            if (border.Child != null)
            {
                TextBlock textBlock = (TextBlock)border.Child;
                textBlock.Padding = new Thickness(2, 2, 4, 3);
            }
        }

        private void Cell_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.canvasBorder.BorderThickness = new Thickness(0);
            GekkoDockPanel dockPanel = (GekkoDockPanel)sender;
            if (dockPanel.type != GekkoTableTypes.TableContent) return;
            if (!this.decompOptions.guiDecompIsSelecting) return;            
            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);
            Select(g, col, row);
            this.decompOptions.guiDecompIsSelecting = false;
            this.decompOptions.guiDecompIsSelectingAll = false;
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GekkoDockPanel dockPanel = (GekkoDockPanel)sender;            
            
            bool shiftPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool controlPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool rightClick = false;
            Border border = (Border)dockPanel.Children[0];
            
            if (e.ChangedButton == MouseButton.Right)
            {
                rightClick = true;                
            }

            if (!(rightClick || dockPanel.type == GekkoTableTypes.TableContent)) return; //a pure click and shift-click (and ctrl-click that has no special meaning...) on a cells on the frozen borders does not get selected, making a mess of further selections 

            if (dockPanel.type != GekkoTableTypes.TableContent)
            {
                if (dockPanel.type == GekkoTableTypes.Top)
                {
                    Window ww = GetWindow(dockPanel);
                    DecompOptions decompOptions = (DecompOptions)ww.Tag;
                    if (decompOptions.guiDecompIsRaw) return;

                    string s = decompOptions.variable;  //FIXME                    

                    string s5 = null;
                    string code = decompOptions.guiDecompTransformationCode;
                    if (code.Contains("m") || code.Contains("q"))
                    {
                        code = "sq";
                        s5 = "multiplier";
                    }
                    else
                    {
                        code = "sp";
                        s5 = "time-change";                        
                    }

                    TextBlock textBlock = (TextBlock)border.Child;
                    string s2 = textBlock.Text.Trim();                    

                    if (MessageBox.Show("Do you want to open a " + s5 + " flowchart for '" + s + "' for the period " + s2 + "?\nPlease note that these flowcharts are experimental. If the flowchart \nis cluttered, try to augment the cut-off value.", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            Program.FlowChart(s, code, G.FromStringToDate(s2));
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("*** ERROR: Flowchart failed");
                        }
                    }
                    else
                    {
                        // Do not close the window
                    }
                }
                return;
            }     

            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);

            if (rightClick)
            {
                //Do nothing, is handled elsewhere               
            }
            else if (shiftPressed)
            {
                Select(g, col, row);
            }
            else  //normal click
            {
                this.decompOptions.guiDecompIsSelecting = true;
                DefaultGrid(g);
        
                border.BorderThickness = new Thickness(3);
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 0);
                border.BorderBrush = mySolidColorBrush;

                this.decompOptions.guiDecompLastClickedRow = row;  //normal click
                this.decompOptions.guiDecompLastClickedCol = col;
                this.decompOptions.guiDecompChangedCells.Add(row + "," + col, 0);

                TextBlock textBlock = (TextBlock)border.Child;
                textBlock.Padding = new Thickness(0, 0, 1, 0);          
            }            
        }        

        private static Window GetWindow(GekkoDockPanel dockPanel)
        {
            Grid p = (Grid)dockPanel.Parent;
            Border pp = (Border)p.Parent;
            Canvas ppp = (Canvas)pp.Parent;
            Border pppp = (Border)ppp.Parent;
            ScrollViewer ppppp = (ScrollViewer)pppp.Parent;
            StackPanel pppppp = (StackPanel)ppppp.Parent;
            DockPanel ppppppp = (DockPanel)pppppp.Parent;
            Window ww = (Window)ppppppp.Parent;
            return ww;
        }

        private void Select(Grid g, int col, int row)
        {
            DefaultGrid(g);
            this.decompOptions.guiDecompSelectedRowMin = Math.Min(this.decompOptions.guiDecompLastClickedRow, row);
            this.decompOptions.guiDecompSelectedRowMax = Math.Max(this.decompOptions.guiDecompLastClickedRow, row);
            this.decompOptions.guiDecompSelectedColMin = Math.Min(this.decompOptions.guiDecompLastClickedCol, col);
            this.decompOptions.guiDecompSelectedColMax = Math.Max(this.decompOptions.guiDecompLastClickedCol, col);
            double sum = 0d;
            int count = 0;
            foreach (GekkoDockPanel d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                //b2.BorderBrush = null;
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (this.decompOptions.guiDecompSelectedRowMin <= row2 && row2 <= this.decompOptions.guiDecompSelectedRowMax)
                {
                    if (this.decompOptions.guiDecompSelectedColMin <= col2 && col2 <= this.decompOptions.guiDecompSelectedColMax)
                    {

                        if (!this.decompOptions.guiDecompChangedCells.ContainsKey(row2 + "," + col2))
                        {
                            this.decompOptions.guiDecompChangedCells.Add(row2 + "," + col2, 0);
                        }                        
                        
                        SetDefaultBorder(b2);                        

                        SetBorderThickness(g, row2, col2, b2);            

                        d2.Background = Brushes.White;

                        double left = 0.15;
                        double right = 0;
                        double top = 0.15;
                        double bottom = 0;
                        if (this.decompOptions.guiDecompSelectedRowMin == row2) top = 3;
                        if (this.decompOptions.guiDecompSelectedRowMax == row2) bottom = 3;
                        if (this.decompOptions.guiDecompSelectedColMin == col2) left = 3;
                        if (this.decompOptions.guiDecompSelectedColMax == col2) right = 3;

                        b2.BorderBrush = Brushes.Black;
                        b2.BorderThickness = new Thickness(left, top, right, bottom);

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = Globals.GrayExcelSelect;
                        //border.BorderBrush = mySolidColorBrush;


                        if (row2 == this.decompOptions.guiDecompLastClickedRow && col2 == this.decompOptions.guiDecompLastClickedCol)
                        {
                        }
                        else
                        {
                            d2.Background = mySolidColorBrush;
                        }

                        //The below logic is almost perfect, so that the numbers in selected
                        //cells do not move visually when selection changes.
                        if (right == 3 && bottom != 3)
                        {
                            TextBlock textBlock = (TextBlock)b2.Child;
                            textBlock.Padding = new Thickness(0, 0, 1, 1);
                        }
                        else if (bottom == 3 && right != 3)
                        {
                            TextBlock textBlock = (TextBlock)b2.Child;
                            textBlock.Padding = new Thickness(0, 0, 4, 1);
                        }
                        else if (bottom == 3 && right == 3)
                        {
                            TextBlock textBlock = (TextBlock)b2.Child;
                            textBlock.Padding = new Thickness(0, 0, 1, 0);
                        }

                        int x; int y;
                        CoordConversion(out x, out y, d2.type, row2, col2);
                        Cell c = this.decompOptions.guiDecompValues.Get(x, y);
                        
                        if (c == null)
                        {
                        }
                        else
                        {
                            if (c.cellType == CellType.Number)
                            {
                                sum += c.number;
                                count++;
                            }
                            else sum = double.NaN;                            
                        }
                    }
                }                
            }            
            TextBlock text = _statusText;
            text.FontFamily = new FontFamily("Calibri");
            text.FontSize = 13d;
            text.Text = "Sum: " + sum + "   Count: " + count + "   Avg: " + (sum / (double)count);
        }

        private void DefaultGrid(Grid g)
        {            
            foreach (GekkoDockPanel d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (!this.decompOptions.guiDecompChangedCells.ContainsKey(row2 + "," + col2)) continue;
                if (d2.originalBackgroundColor == null) d2.Background = Brushes.White;
                else d2.Background = d2.originalBackgroundColor;                
                SetDefaultBorder(b2);                   
                SetBorderThickness(g, row2, col2, b2);                
            }
            this.decompOptions.guiDecompChangedCells.Clear();
        }

        private void Cell_Leave(object sender, MouseEventArgs e)
        {
            GekkoDockPanel dockPanel = (GekkoDockPanel)sender;            

            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);

            if (this.decompOptions.guiDecompIsSelecting)
            {            
            }
            else
            {
                //G.Writeln("cell " + row + " " + col);
                this.equation.Background = Brushes.White;
                //this.equation.Foreground = Brushes.White;

                int x; int y;
                CoordConversion(out x, out y, dockPanel.type, row, col);
                Cell c = this.decompOptions.guiDecompValues.Get(x, y);
                string s = FindEquationText(this.decompOptions);
                if (s.Contains("___CHOU")) s = "frml _i M['CHOU'] = myFM['CHOU'] * F['CHOU'] * ((PM['CHOU'] / PFF['CHOU']) * (PM['CHOU'] / PFF['CHOU'])) ** (-EF['CHOU'] / 2)";
                equation.Text = s;
            }
        }

        private void Mouse_Enter(object sender, MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            tb.TextDecorations = TextDecorations.Underline;            
        }

        private void Mouse_Leave(object sender, MouseEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            tb.TextDecorations = null;
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            //#98732498724
            TextBlock tb = (TextBlock)sender;
            Border b = (Border)(tb.Parent);
            DockPanel dp = (DockPanel)(b.Parent);
            Grid g = (Grid)(dp.Parent);
            
            int col = (int)dp.GetValue(Grid.ColumnProperty);
            int row = (int)dp.GetValue(Grid.RowProperty);
            
            int x = -12345;
            int y = -12345;
            CoordConversion(out x, out y, GekkoTableTypes.Left, row, col);

            Cell c = this.decompOptions.guiDecompValues.Get(x, y);

            if (c != null && c.cellType == CellType.Text)
            {
                string var = c.CellText.TextData[0];

                string var2 = G.PrettifyTimeseriesHash(G.ExtractOnlyVariableIgnoreLag(var, Globals.leftParenthesisIndicator), true, true);
                                
                DecompOptions d = this.decompOptions.Clone();                                

                if (d.isSubst)
                {
                    //d.variable is the same
                    d.subst.Add(var2);
                    d.isSubWindow = true;
                }
                else
                {
                    d.variable = var2;
                    d.isSubWindow = true;
                }

                Program.Decomp(d);
            }
            else
            {
                G.Writeln("*** ERROR: Unexpected link error");
                throw new GekkoException();

            }
        }    

        private void Cell_Enter(object sender, MouseEventArgs e)
        {
            GekkoDockPanel dockPanel = (GekkoDockPanel)sender;
            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);  //0-based
            int row = (int)dockPanel.GetValue(Grid.RowProperty);  //0-based

            GekkoTableTypes type = dockPanel.type;
            int x; int y;
            CoordConversion(out x, out y, type, row, col);

            if (type == GekkoTableTypes.TableContent && this.decompOptions.guiDecompIsSelecting)
            {
                Select(g, col, row);
            }
            else
            {
                Cell c = this.decompOptions.guiDecompValues.Get(x, y);

                if (dockPanel.type == GekkoTableTypes.Left)
                {
                    if (c != null)
                    {
                        if (c.cellType == CellType.Text)
                        {
                            this.equation.Background = Brushes.LightYellow;
                            //G.Writeln(c.CellText.TextData[0]);
                            string var = c.CellText.TextData[0];

                            string var2 = G.ExtractOnlyVariableIgnoreLag(var, Globals.leftParenthesisIndicator);

                            if (G.Equal(var2, Globals.decompText0))
                            {
                                if (decompOptions.expressionOld != null)
                                {
                                    this.equation.Text = "This value corresponds to evaluating the expression.";
                                }
                                else
                                {
                                    this.equation.Text = "This value corresponds to evaluating the right-hand side of the equation.";
                                }
                            }
                            else if (G.Equal(var2, Globals.decompText1))
                            {
                                if (decompOptions.expressionOld != null)
                                {
                                    this.equation.Text = "This difference is always 0 for expressions.";
                                }
                                else
                                {
                                    this.equation.Text = "This difference is the databank value minus the result of evaluating the right-hand side of the equation.";
                                }

                            }
                            else if (G.Equal(var2, Globals.decompText1a))  //raw
                            {
                                if (decompOptions.expressionOld != null)
                                {
                                    this.equation.Text = "This difference between the two rows above is always 0 for expressions.";
                                }
                                else
                                {
                                    this.equation.Text = "This difference is the databank value minus the result of evaluating the right-hand side of the equation.";
                                }

                            }
                            else if (G.Equal(var2, Globals.decompText2))
                            {
                                if (decompOptions.expressionOld != null)
                                {
                                    this.equation.Text = "This value is the result of evaluating the expression minus the sum of decomposed contributions." + G.NL + "If the equation is linear, this number is very small (in principle: zero).";
                                }
                                else
                                {
                                    this.equation.Text = "This value is the result of evaluating the right-hand side of the equation minus the sum of decomposed contributions." + G.NL + "If the equation is linear, this number is very small (in principle: zero).";
                                }
                            }
                            else if (G.Equal(var2, Globals.decompText2a))  //raw
                            {
                                if (decompOptions.expressionOld != null)
                                {
                                    this.equation.Text = "These values correspond to evaluating the expression (always equal to the row above for expressions).";
                                }
                                else
                                {
                                    this.equation.Text = "These values correspond to evaluating the right-hand side of the equation.";
                                }
                            }
                            else
                            {
                                this.equation.Text = Program.GetVariableExplanationAugmented(var2, G.ExtractOnlyVariableIgnoreLag(var2, Globals.leftParenthesisIndicator)).Trim();                                                                                                 
                            }
                        }
                    }
                }
                else
                {
                    string s = FindEquationText(this.decompOptions);
                    if (s.Contains("___CHOU")) s = "frml _i M['CHOU'] = myFM['CHOU'] * F['CHOU'] * ((PM['CHOU'] / PFF['CHOU']) * (PM['CHOU'] / PFF['CHOU'])) ** (-EF['CHOU'] / 2)";
                    equation.Text = s;
                }
            }
        }

        private static void CoordConversion(out int x, out int y, GekkoTableTypes type, int row, int col)
        {
            x = -12345;
            y = -12345;
            if (type == GekkoTableTypes.TableContent)
            {
                x = row + 2;
                y = col + 2;
            }
            else if (type == GekkoTableTypes.Left)
            {
                x = row + 2;
                y = col + 1;
            }
            else if (type == GekkoTableTypes.Top)
            {
                x = row + 1;
                y = col + 2;
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                this.Close();
            }
        }

        public void RecalcCellsWithNewType()
        {
            try
            {
                if (this.decompOptions.expression == null)
                {
                    if (equation == null) return;  //Happens during first rendering, when isChecked is set by C# on top-left radio-button (ignore it)

                    //if (this.decompOptions.modelHash == null) this.decompOptions.modelHash = Program.model.modelHashTrue; //To make sure that decomp is not clicked and results shown, after a new model has been loaded
                    //if (this.decompOptions.modelHash != Program.model.modelHashTrue)
                    //{
                    //    MessageBox.Show("*** ERROR: A new model seems to have been loaded." + "\n" + "Please reload the old model, or close this window" + "\n" + "and open it again from the command prompt");
                    //    return;
                    //}
                }
                                                          
                //Setting defaults
                radioButton21.IsEnabled = true;
                radioButton21.Opacity = 1.0;
                radioButton22.IsEnabled = true;
                radioButton22.Opacity = 1.0;
                radioButton24.IsEnabled = true;
                radioButton24.Opacity = 1.0;
                radioButton29.IsEnabled = true;
                radioButton29.Opacity = 1.0;
                radioButton26.IsEnabled = true;
                radioButton26.Opacity = 1.0;
                radioButton28.IsEnabled = true;
                radioButton28.Opacity = 1.0;
                radioButton30.IsEnabled = true;
                radioButton30.Opacity = 1.0;
                //---
                radioButton1.IsEnabled = true;
                radioButton1.Opacity = 1.0;
                radioButton2.IsEnabled = true;
                radioButton2.Opacity = 1.0;
                radioButton4.IsEnabled = true;
                radioButton4.Opacity = 1.0;
                radioButton9.IsEnabled = true;
                radioButton9.Opacity = 1.0;
                radioButton6.IsEnabled = true;
                radioButton6.Opacity = 1.0;
                radioButton8.IsEnabled = true;
                radioButton8.Opacity = 1.0;
                radioButton10.IsEnabled = true;
                radioButton10.Opacity = 1.0;
                //---
                checkBox1.IsEnabled = true;
                checkBox1.Opacity = 1.0;
                checkBox2.IsEnabled = true;
                checkBox2.Opacity = 1.0;
                flowText.Opacity = 0.5;
                flowText.Visibility = Visibility.Visible;

                string transformationCodeAugmented = this.decompOptions.guiDecompTransformationCode;

                if (this.decompOptions.guiDecompIsRaw && this.decompOptions.guiDecompIsShares)
                {
                    G.Writeln2("*** ERROR: Cannot show decomposition with both 'raw' and 'shares' option at the same time");
                    throw new GekkoException();
                }

                //"x" and "s" are mutually exclusive: in raw mode shares are not meaningful

                //so "sd", "sp", "sdp" + "sm", "sq", "smp" are used

                if (this.decompOptions.guiDecompIsBaseline) transformationCodeAugmented = "r" + transformationCodeAugmented;
                if (this.decompOptions.guiDecompIsRaw) transformationCodeAugmented = "x" + transformationCodeAugmented;
                if (this.decompOptions.guiDecompIsShares) transformationCodeAugmented = "s" + transformationCodeAugmented;  //is put on last

                if (this.decompOptions.isSubst) subst.IsChecked = true;
                if (this.decompOptions.isSort) sort.IsChecked = true;
                if (this.decompOptions.isPool) pool.IsChecked = true;


                if (this.decompOptions.guiDecompIsBaseline)
                {
                    radioButton22.IsEnabled = false;
                    radioButton22.Opacity = 0.5;
                    radioButton24.IsEnabled = false;
                    radioButton24.Opacity = 0.5;
                    radioButton29.IsEnabled = false;
                    radioButton29.Opacity = 0.5;
                    radioButton26.IsEnabled = false;
                    radioButton26.Opacity = 0.5;
                    radioButton28.IsEnabled = false;
                    radioButton28.Opacity = 0.5;
                    radioButton30.IsEnabled = false;
                    radioButton30.Opacity = 0.5;
                }

                if (this.decompOptions.guiDecompIsShares)
                {
                    radioButton21.IsEnabled = false;
                    radioButton21.Opacity = 0.5;
                    radioButton22.IsEnabled = false;
                    radioButton22.Opacity = 0.5;
                    radioButton24.IsEnabled = false;
                    radioButton24.Opacity = 0.5;
                    radioButton29.IsEnabled = false;
                    radioButton29.Opacity = 0.5;
                    //---
                    radioButton1.IsEnabled = false;
                    radioButton1.Opacity = 0.5;
                    radioButton2.IsEnabled = false;
                    radioButton2.Opacity = 0.5;
                    radioButton4.IsEnabled = false;
                    radioButton4.Opacity = 0.5;
                    radioButton9.IsEnabled = false;
                    radioButton9.Opacity = 0.5;
                }

                if (this.decompOptions.guiDecompIsRaw)
                {
                    checkBox1.IsEnabled = false;  //shares
                    checkBox1.Opacity = 0.5;
                    flowText.Visibility = Visibility.Collapsed;
                }

                if (G.Equal(transformationCodeAugmented, "m") || G.Equal(transformationCodeAugmented, "xm") || G.Equal(transformationCodeAugmented, "q") || G.Equal(transformationCodeAugmented, "xq") || G.Equal(transformationCodeAugmented, "mp") || G.Equal(transformationCodeAugmented, "xmp"))
                {
                    checkBox2.IsEnabled = false;  //baseline, not meaningful for multiplier types
                    checkBox2.Opacity = 0.5;
                }

                _statusText.Text = "";

                bool useLocalData = false;

                //if (this.decompOptions.expression == null)
                //{
                //    this.decompOptions.modelHash = Program.model.modelHashTrue;  //To make sure that decomp is not clicked and results shown, after a new model has been loaded
                //}

                Table table = null;
                //table = Program.DecompHelper2(this.decompOptions, transformationCodeAugmented, useLocalData);

                this.decompOptions.prtOptionLower = transformationCodeAugmented;


                table = Program.Decompose(this.decompOptions);

                if (Globals.runningOnTTComputer)
                {
                    DecompTables d = Program.DecomposeNEW(this.decompOptions.expression, EDecompBanks.Both, this.decompOptions.t1, this.decompOptions.t2);
                }

                if (this.decompOptions.isSubst && this.decompOptions.subst.Count > 0)
                {
                    foreach (string var in this.decompOptions.subst)
                    {
                        table = DecompSubstitute(table, var);
                    }
                }

                if (this.decompOptions.isSort)
                {                    
                    table = TableSort(table);
                }

                if (this.decompOptions.isPool)
                {
                    table = TablePool(table);
                }

                string s = FindEquationText(this.decompOptions);
                equation.Text = s;

                //
                // NOTE:
                //
                flowText.Visibility = Visibility.Collapsed;

                this.decompOptions.guiDecompValues = table;
                ClearGrid();
                MakeTable(table, this.decompOptions);


            }
            catch (Exception e)
            {
                this.isClosing = true;

                MessageBox.Show("*** ERROR: Decomp update failed: maybe some variables or databanks are non-available?");


            }
        }

        private Table DecompSubstitute(Table table, string var2)
        {
            EquationHelper found = Program.DecompEval(var2);
            DecompOptions d3 = this.decompOptions.Clone();
            d3.variable = var2;
            d3.expression = Globals.expression;
            d3.expressionOld = found.equationText;
            Table table2 = Program.Decompose(d3);
            Table table3 = TableSubstitute(table, var2, table2, true);
            return table3;
        }

        private static Table TableSubstitute(Table table1, string var, Table table2, bool scale)
        {
            //We have a table like (for x)
            //        2020  2021
            //  expr   500   600
            //  x1     100   200
            //  x2     400   400

            //and a table like (for x1)

            //        2020  2021
            //  expr   100   100
            //  x11     20    30
            //  x12     80    70

            //We insert:

            //        2020  2021
            //  expr   500   600
            //  x11     20    30
            //  x12     80    70
            //  x2     400   400
                        
            Table rv = new Table();
            rv.writeOnce = true;

            var = var.Trim();

            int irv = 0;

            if (table1.GetColMaxNumber() != table2.GetColMaxNumber())
            {
                G.Writeln2("*** ERROR: Table merge problem");
                throw new GekkoException();
            }

            for (int i = 1; i <= table1.GetRowMaxNumber(); i++)
            {
                irv++;
                
                if (i > 1)
                {
                    //not first row
                    Cell c1temp = table1.Get(i, 1);
                    if (c1temp.CellText.TextData[0].Trim() == var)
                    {
                        int rowStart = 3;
                        int colStart = 2;

                        List<double> scalings = null;
                        if (scale)
                        {                            

                            scalings = new List<double>();
                            for (int jj = colStart; jj <= table2.GetColMaxNumber(); jj++)
                            {
                                Cell temp1 = table1.Get(i, jj);
                                if (temp1 == null || temp1.cellType != CellType.Number)
                                {
                                    G.Writeln2("*** ERROR: Table merge problem");
                                    throw new GekkoException();
                                }
                                double original = temp1.number;

                                double rowsum = 0d;
                                for (int ii = rowStart; ii <= table2.GetRowMaxNumber(); ii++)
                                {
                                    Cell temp2 = table2.Get(ii, jj);
                                    if (temp2 == null || temp2.cellType != CellType.Number)
                                    {
                                        G.Writeln2("*** ERROR: Table merge problem");
                                        throw new GekkoException();
                                    }
                                    rowsum += temp2.number;
                                }
                                scalings.Add(original / rowsum);
                            }                            
                        }
                        
                        //insert rows from table 2
                        for (int ii = rowStart; ii <= table2.GetRowMaxNumber(); ii++)
                        {                            
                            irv++;
                            for (int jj = 1; jj <= table2.GetColMaxNumber(); jj++)
                            {
                                Cell temp = table2.Get(ii, jj);
                                if (jj == 1) temp.CellText.TextData[0] = "    " + temp.CellText.TextData[0];
                                else
                                {
                                    if (temp != null)
                                    {
                                        if (scale) temp.number = temp.number * scalings[jj - 2];

                                    }
                                }
                                rv.Set(new Coord(irv - 1, jj), temp);
                            }
                        }
                        i++;  //skip this row in table1
                    }
                }

                for (int j = 1; j <= table1.GetColMaxNumber(); j++)
                {
                    Cell c1 = table1.Get(i, j);
                    Cell c2 = table2.Get(i, j);                    

                    if (i == 1 && j > 1)
                    {
                        //first row and not first column
                        //test that dates conform, only row merging                        
                        if (c1 == null || c2 == null || c1.cellType != CellType.Date || c2.cellType != CellType.Date || c1.date != c2.date)
                        {
                            G.Writeln2("*** ERROR: Table merge problem");
                            throw new GekkoException();
                        }
                    }
                    if (c1 != null) rv.Set(new Coord(irv, j), c1);
                    
                    
                }
            }

            return rv;
        }

        private static Table TableSort(Table table1)
        {
            //We have a table like (for x)
            //        2020  2021
            //  a      500   600
            //  c      100   200
            //  b      400   400

            //Sort:

            //        2020  2021
            //  a      500   600
            //  b      400   400
            //  c      100   200

            //a¤2, c¤3, b¤4 --> a¤2, b¤4, c¤3
            //tofrom: (2, 2), (3, 4), (4, 3)

            Table rv = new Table();
            rv.writeOnce = true;

            List<string> names = new List<string>();

            for (int i = 2; i <= table1.GetRowMaxNumber(); i++)
            {
                Cell c = table1.Get(i, 1);
                names.Add(c.CellText.TextData[0].Trim() + "¤" + i);  //remove indentation here
            }
            names.Sort(StringComparer.OrdinalIgnoreCase);
            //names.Reverse();

            List<int> fromTo = new List<int>();


            for (int i = 0; i < names.Count; i++)
            {
                fromTo.Add(-12345);
            }
            for (int i = 0; i < names.Count; i++)
            {
                string[] ss = names[i].Split('¤');
                fromTo[int.Parse(ss[1]) - 2] = i + 2;
            }


            for (int i = 1; i <= table1.GetRowMaxNumber(); i++)
            {
                for (int j = 1; j <= table1.GetColMaxNumber(); j++)
                {
                    Cell c = table1.Get(i, j);

                    if (c == null) continue;
                    if (i == 1) rv.Set(new Coord(i, j), c);
                    else
                    {
                        rv.Set(new Coord(fromTo[i - 2], j), c);
                    }
                }
            }

            return rv;
        }

        private static Table TablePool(Table table1)
        {
            //We have a table like (for x)
            //        2020  2021
            //  a1     500   600
            //  a2     100   200

            //Becomes
            //        2020  2021
            //  a      600   200

            table1 = new Table();
            table1.Set(new Coord(1, 2), null, double.NaN, CellType.Date, null);
            table1.Set(new Coord(2, 1), "a", double.NaN, CellType.Text, null);
            table1.Set(new Coord(2, 2), null, 500d, CellType.Number, null);
            table1.Set(new Coord(3, 1), "a", double.NaN, CellType.Text, null);
            table1.Set(new Coord(3, 2), null, 100d, CellType.Number, null);


            Table rv = new Table();
            rv.writeOnce = true;

            List<string> names = new List<string>();

            string current = "";
            int iCurrent = 1;

            for (int i = 1; i <= table1.GetRowMaxNumber(); i++)
            {
                string name = null;
                Cell cc = table1.Get(i, 1);
                if (cc != null) name = cc.CellText.TextData[0];
                if (i > 1 && name != current)
                {
                    current = name;
                    iCurrent++;
                }
                for (int j = 1; j <= table1.GetColMaxNumber(); j++)
                {
                    Cell c = table1.Get(i, j);
                    if (i == 1)
                    {
                        if (c != null) rv.Set(new Coord(i, j), c);  //first line
                    }
                    else
                    {
                        if (name == current)
                        {
                            if(j>1)
                            {
                                Cell cellCurrent = table1.Get(iCurrent, j);
                                double dd = c.number;
                                cellCurrent.number += dd;
                            }
                        }
                        else
                        {                            
                            rv.Set(new Coord(iCurrent, j), c);
                        }
                    }                    
                }
            }
            
            return rv;
        }

        private void ClearGrid()
        {
            this.grid1.RowDefinitions.Clear();
            this.grid1.ColumnDefinitions.Clear();
            this.grid1.Children.Clear();
            this.grid1Left.RowDefinitions.Clear();
            this.grid1Left.ColumnDefinitions.Clear();
            this.grid1Left.Children.Clear();
            this.grid1Top.RowDefinitions.Clear();
            this.grid1Top.ColumnDefinitions.Clear();
            this.grid1Top.Children.Clear();
            this.gridUpperLeft.RowDefinitions.Clear();
            this.gridUpperLeft.ColumnDefinitions.Clear();
            this.gridUpperLeft.Children.Clear();
        }

        private void MakeTable(Table table, DecompOptions decompOptions)
        {                       
            
            CreateGridRowsAndColumns(this.grid1, table, GekkoTableTypes.TableContent);
            
            //DateTime t0 = DateTime.Now;
            PutTableIntoGrid(this.grid1, table, GekkoTableTypes.TableContent, decompOptions);
            //MessageBox.Show("Took " + (DateTime.Now - t0).TotalMilliseconds);
            
            CreateGridRowsAndColumns(this.grid1Left, table, GekkoTableTypes.Left);
            PutTableIntoGrid(this.grid1Left, table, GekkoTableTypes.Left, decompOptions);
            CreateGridRowsAndColumns(this.grid1Top, table, GekkoTableTypes.Top);
            PutTableIntoGrid(this.grid1Top, table, GekkoTableTypes.Top, decompOptions);
            CreateGridRowsAndColumns(this.gridUpperLeft, table, GekkoTableTypes.UpperLeft);
            PutTableIntoGrid(this.gridUpperLeft, table, GekkoTableTypes.UpperLeft, decompOptions);            
        }

        private string FindEquationText(DecompOptions decompOptions)
        {
            if (decompOptions.expressionOld != null)
            {
                return decompOptions.expressionOld;
            }
            else
            {
                EquationHelper eh = Program.FindEquationByMeansOfVariableName(this.decompOptions.variable);
                if (eh == null) return ""; //probably only when model is changed while UDVALG window is open (this is illegal anyway, and a popup will appear)
                else return ((EquationHelper)eh).equationText;
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "n";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "d";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton6_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = false;
                this.decompOptions.guiDecompTransformationCode = "d";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "p";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton8_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = false;
                this.decompOptions.guiDecompTransformationCode = "p";
                RecalcCellsWithNewType();
            }
        }        

        private void radioButton21_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "n";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton22_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "m";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton26_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = false;
                this.decompOptions.guiDecompTransformationCode = "m";
                RecalcCellsWithNewType();
            }
        }        

        private void radioButton24_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "q";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton28_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = false;
                this.decompOptions.guiDecompTransformationCode = "q";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton29_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "mp";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton30_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = false;
                this.decompOptions.guiDecompTransformationCode = "mp";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton9_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = true;
                this.decompOptions.guiDecompTransformationCode = "dp";
                RecalcCellsWithNewType();
            }
        }       
        
        private void radioButton10_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsRaw = false;
                this.decompOptions.guiDecompTransformationCode = "dp";
                RecalcCellsWithNewType();
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsShares = true;
                RecalcCellsWithNewType();
            }
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsShares = false;
                RecalcCellsWithNewType();
            }
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsBaseline = true;
                RecalcCellsWithNewType();
            }
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.guiDecompIsBaseline = false;
                RecalcCellsWithNewType();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.guiDecompWindowTopDistance = Math.Max(1, (int)this.Top);
            Globals.guiDecompWindowLeftDistance = Math.Max(1, (int)this.Left);
            try
            {
                if (Globals.windowsDecomp != null && this != null) Globals.windowsDecomp.Remove(this);
            }
            catch { }
            //if (!G.IsUnitTesting()) Program.ShowPeriodInStatusField("");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            UpdateDecomp();
        }

        public void UpdateDecomp()
        {
            if (!this.isInitializing)
            {
                this.decompOptions.localBanks = null;  //clearing this, forcing window to use vales from Gekko databanks
                this.RecalcCellsWithNewType();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                if (this.decompOptions.isPercentageType)
                {
                    this.decompOptions.decimalsPch++;
                }
                else
                {
                    this.decompOptions.decimalsLevel++;
                }
                RecalcCellsWithNewType();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                if (this.decompOptions.isPercentageType)
                {
                    this.decompOptions.decimalsPch--;
                    if (this.decompOptions.decimalsPch < 0) this.decompOptions.decimalsPch = 0;
                }
                else
                {
                    this.decompOptions.decimalsLevel--;
                    if (this.decompOptions.decimalsLevel < 0) this.decompOptions.decimalsLevel = 0;
                }
                RecalcCellsWithNewType();
            }
        }

        private void checkBoxErrors_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.showErrors = true;
                RecalcCellsWithNewType();
            }
        }

        private void checkBoxErrors_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions.showErrors = false;
                RecalcCellsWithNewType();                
            }
        }

        private void Sort_Checked(object sender, RoutedEventArgs e)
        {
            this.decompOptions.isSort = false;
            if (sort.IsChecked == true) this.decompOptions.isSort = true;
        }

        private void Pool_Checked(object sender, RoutedEventArgs e)
        {
            this.decompOptions.isPool = false;
            if (pool.IsChecked == true) this.decompOptions.isPool = true;
        }

        private void Subst_Checked(object sender, RoutedEventArgs e)
        {
            this.decompOptions.isSubst = false;
            if (subst.IsChecked == true) this.decompOptions.isSubst = true;
        }

        private void Sort_Unchecked(object sender, RoutedEventArgs e)
        {
            this.decompOptions.isSort = false;
            if (sort.IsChecked == true) this.decompOptions.isSort = true;
        }

        private void Pool_Unchecked(object sender, RoutedEventArgs e)
        {
            this.decompOptions.isPool = false;
            if (pool.IsChecked == true) this.decompOptions.isPool = true;
        }

        private void Subst_Unchecked(object sender, RoutedEventArgs e)
        {
            this.decompOptions.isSubst = false;
            if (subst.IsChecked == true) this.decompOptions.isSubst = true;
        }
    }

    public class GekkoDockPanel : DockPanel
    {
        public Window1.GekkoTableTypes type = Window1.GekkoTableTypes.Unknown;
        public Brush originalBackgroundColor = null;
    }

    public class DecompTables
    {           
        public DecompDict cellsQuo = null;
        public DecompDict cellsGradQuo = null;
        public DecompDict cellsContribD = null;
        // -------------------------------------
        public DecompDict cellsRef = null;
        public DecompDict cellsGradRef = null;
        public DecompDict cellsContribDRef = null;
        // -------------------------------------
        public DecompDict cellsContribM = null;
    }

    public class DecompOptions
    {
        public bool isSubst = false;
        public bool isPool = false;
        public bool isSort = false;

        //public bool onlyTable = false;
        //public Table table = null;

        public int numberOfRecalcs = 0;  //used to pause main thread until the DECOMP window has calculated.
        public string variable = null;
        public List<string> variable_subelement = null;
        public bool isPercentageType = false;
        //public bool isExpression = false; //true for UDVALG fy+1 etc.
        public string expressionOld = null;  //only != null for expressions
        public Func<GekkoSmpl, IVariable> expression = null;
        public List<Dictionary<string, string>> precedents;  //only != null for expressions
        public string type;  //not used yet (UDVALG or DECOMP)
        //public GekkoParserTimePeriod tp;
        public GekkoTime t1 = GekkoTime.tNull;
        public GekkoTime t2 = GekkoTime.tNull;
        public string prtOptionLower;  //only used at first call of UDVALG (e.g. UDVALG<p>): when isSubWindow is false.
        //public List<string> vars;
        public bool isSubWindow = false;  //when browsing/clicking, opening a new window
        public bool showErrors = false;
        //public GekkoSmpl smplForFunc = null;

        public List<string> subst = new List<string>();

        public IVariable name = null;  //only active for names like x, x[a] and the like, not for expressions
        

        //-------- tranformation start --------------
        public string guiDecompTransformationCode = "n";
        public bool guiDecompIsShares = false;
        public bool guiDecompIsRaw = true;
        public bool guiDecompIsBaseline = false;        
        //-------- tranformation end ----------------
        public int guiDecompLastClickedRow = 0;
        public int guiDecompLastClickedCol = 0;
        public int guiDecompSelectedColMin = 0;
        public int guiDecompSelectedColMax = 0;
        public int guiDecompSelectedRowMin = 0;
        public int guiDecompSelectedRowMax = 0;
        public bool guiDecompIsSelecting = false;
        public bool guiDecompIsSelectingAll = false;
        public Dictionary<string, int> guiDecompChangedCells = new Dictionary<string, int>();        
        public Table guiDecompValues = new Table();
        //public bool isCalledFromDecompWindow = true;
        public LocalBanks localBanks = null;
        public string modelHash = null;
        //public int decimals = 4;
        public int decimalsLevel = 4;
        public int decimalsPch = 2;
        public string dream = null;  //experimental

        public DecompTables decompTables = null;
        public bool hasCalculatedQuo = false;
        public bool hasCalculatedRef = false;

        public List<string> vars2 = null;

        public DecompOptions Clone()
        {
            //clones relevant parts for new window
            DecompOptions d = new DecompOptions();
            //d.tp = this.tp;
            d.variable = this.variable;
            d.t1 = this.t1;
            d.t2 = this.t2;
            d.prtOptionLower = this.prtOptionLower;            
            d.guiDecompIsShares = this.guiDecompIsShares;
            d.guiDecompIsRaw = this.guiDecompIsRaw;
            d.guiDecompIsBaseline = this.guiDecompIsBaseline;
            d.guiDecompTransformationCode = this.guiDecompTransformationCode;
            d.modelHash = this.modelHash;
            d.showErrors = this.showErrors;
            //d.decimalsLevel = this.decimalsLevel;
            d.decimalsPch = this.decimalsPch;  //these are inherited in sub-windows. But .decimalsLevel are not (some vars like prices really need 4 decimals).
            d.dream = this.dream;

            d.isSort = this.isSort;
            d.isSubst = this.isSubst;
            d.isPool = this.isPool;
            foreach(string s in this.subst)
            {
                d.subst.Add(s);
            }

            return d;
        }
    }
}
