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
using System.Collections.Generic;
using System.Collections.ObjectModel;



namespace Gekko
{
       
    
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowDecomp : Window
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
        private ObservableCollection<Task> _list = new ObservableCollection<Task>();
        ListViewDragDropManager<Task> dragMgr;

        public DecompOptions2 decompOptions2 = null;

        public List<string> Customers = new List<string>();

        public ObservableCollection<Task> list
        {
            get
            {
                return _list;
            }

            set
            {
                _list = value;
            }
        }


        private void RefreshList()
        {
            list.Clear();
            List<string> banks2 = new List<string>();
            banks2.Add("Local");
            foreach (Databank db in Program.databanks.storage)
            {
                banks2.Add(db.name);
            }
            banks2.Add("Global");

            for (int ii = 0; ii < banks2.Count; ii++)
            {
                string s = banks2[ii];
                int i = ii - 1;
                Databank databank = Program.databanks.GetDatabank(s);

                if (databank.storage.Count == 0)
                {
                    if (G.Equal(s, Globals.Local))
                    {
                        i = -12345;
                        continue;
                    }
                    else if (G.Equal(s, Globals.Global))
                    {
                        i = -12345;
                        continue;
                    }
                    else if (G.Equal(databank.name, Globals.Ref) && i == 1) continue;
                }

                string c = "";

                string i1, i2; Program.GetYearPeriod(databank.yearStart, databank.yearEnd, out i1, out i2);

                string period = i1 + "-" + i2;


                if (databank.yearStart == -12345 || databank.yearEnd == -12345) period = "";
                string prot = null;
                if (databank.editable) prot = Globals.protectSymbol;
                else prot = "";
                list.Add(new Task(s, Program.GetDatabankFilename(databank), databank.FileNameWithPath, databank.storage.Count.ToString(), period, databank.info1, databank.date, c, prot, i));

            }
            //unswap.IsEnabled = Program.AreDatabanksSwapped();
            //unswap.IsEnabled = false;  //FIXME
        }

        void WindowDecomp_Loaded(object sender, RoutedEventArgs e)
        {
            // Give the ListView an ObservableCollection of Task
            // as a data source.  Note, the ListViewDragManager MUST
            // be bound to an ObservableCollection, where the collection's
            // type parameter matches the ListViewDragManager's type
            // parameter (in this case, both have a type parameter of Task).

            list = new ObservableCollection<Task>();
            RefreshList();

            this.listView.ItemsSource = list;

            // This is all that you need to do, in order to use the ListViewDragManager.
            this.dragMgr = new ListViewDragDropManager<Task>(this.listView);
            this.dragMgr.ListView = this.listView;
            this.dragMgr.ShowDragAdorner = true;
            this.dragMgr.DragAdornerOpacity = 0.5d;  //so that e.g. "Work" can still be seen underneath
            this.listView.ItemContainerStyle = this.FindResource("ItemContStyle") as Style;

            this.dragMgr.ProcessDrop += dragMgr_ProcessDrop;

            // Hook up events on both ListViews to that we can drag-drop
            // items between them.
            this.listView.DragEnter += OnListViewDragEnter;
            this.listView.Drop += OnListViewDrop;
        }

        void OnListViewDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        void OnListViewDrop(object sender, DragEventArgs e)
        {
            if (e.Effects == DragDropEffects.None)
                return;

            Task task = e.Data.GetData(typeof(Task)) as Task;
            if (sender == this.listView)
            {
                if (this.dragMgr.IsDragInProgress)
                    return;

                // An item was dragged from the bottom ListView into the top ListView
                // so remove that item from the bottom ListView.
                //(this.listView2.ItemsSource as ObservableCollection<Task>).Remove( task );
            }
            else
            {
                //if( this.dragMgr2.IsDragInProgress )
                //    return;

                //// An item was dragged from the top ListView into the bottom ListView
                //// so remove that item from the top ListView.
                //(this.listView.ItemsSource as ObservableCollection<Task>).Remove( task );
            }
        }


        void dragMgr_ProcessDrop(object sender, ProcessDropEventArgs<Task> e)
        {
            // This shows how to customize the behavior of a drop.
            // Here we perform a swap, instead of just moving the dropped item.

            MessageBox.Show("*** ERROR: Manual databank swapping not allowed anymore");
            return;

            string text = "";

            int higherIdx = Math.Max(e.OldIndex, e.NewIndex);
            int lowerIdx = Math.Min(e.OldIndex, e.NewIndex);

            Task t_from = list[e.OldIndex];
            Task t_to = list[e.NewIndex];

            string aliasFromOld = t_from.AliasName;
            string aliasToOld = t_to.AliasName;

            string s = null;

            if (lowerIdx < 0)
            {
                // The item came from the lower ListView
                // so just insert it.
                e.ItemsSource.Insert(higherIdx, e.DataItem);
            }
            else
            {
                // null values will cause an error when calling Move.
                // It looks like a bug in ObservableCollection to me.
                if (e.ItemsSource[lowerIdx] == null ||
                    e.ItemsSource[higherIdx] == null)
                {
                    //Program.ShowPeriodInStatusField("");
                    return;
                }

                // The item came from the ListView into which
                // it was dropped, so swap it with the item
                // at the target index.
                e.ItemsSource.Move(lowerIdx, higherIdx);
                e.ItemsSource.Move(higherIdx - 1, lowerIdx);

                Databank lower = Program.databanks.storage[lowerIdx];
                Databank higher = Program.databanks.storage[higherIdx];
                Program.databanks.storage[lowerIdx] = higher;
                Program.databanks.storage[higherIdx] = lower;
                //remember that higher is at lowerIdx and vice versa!
                if ((lowerIdx == 0 || lowerIdx == 1) && !(G.Equal(higher.name, Globals.Work) || G.Equal(higher.name, Globals.Ref)))
                {
                    if (higher.editable)
                    {
                        higher.editable = false;
                        s += "Note that the databank '" + higher.name + "' has been set non-editable. ";
                        list[lowerIdx].Prot = Globals.protectSymbol;
                    }

                }
                //remember that higher is at lowerIdx and vice versa!
                if ((higherIdx == 0 || higherIdx == 1) && !(G.Equal(lower.name, Globals.Work) || G.Equal(lower.name, Globals.Ref)))
                {
                    if (lower.editable)
                    {
                        lower.editable = false;
                        s += "Note that the databank '" + lower.name + "' has been set non-editable. ";
                        list[higherIdx].Prot = Globals.protectSymbol;
                    }
                }

                int counter = 0;
                foreach (var x in e.ItemsSource)
                {
                    counter++;
                    x.Number = counter.ToString();
                    if (x.Number == "2") x.LineColor = "Black";  //these numbers are 1-based and are strings!
                    else x.LineColor = "LightGray";
                }
            }

            // Set this to 'Move' so that the OnListViewDrop knows to
            // remove the item from the other ListView.
            e.Effects = DragDropEffects.Move;
            //unswap.IsEnabled = Program.AreDatabanksSwapped();
            //unswap.IsEnabled = true;  //fixme

            yellow.Text = "Databanks were swapped. " + s;
            //Program.ShowPeriodInStatusField("");            
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listView.SelectedItem = null;  --> no, then we cannot move the row
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Akaljdsf");
            RefreshList();
            //string s = Program.UnswapMessageLong();
            //yellow.Text = s;
            //Program.ShowPeriodInStatusField("");
            //G.Writeln();
            //G.Writeln(Program.UnswapMessage());
        }

        public void SetRadioButtons() {
            if (this.decompOptions2.isSubWindow)
            {
                if (this.decompOptions2.operatorHelper.guiDecompIsRaw)
                {
                    if (this.decompOptions2.operatorHelper.guiDecompOperator == "n")
                    {
                        radioButton1.IsChecked = true;
                    }
                    else if(this.decompOptions2.operatorHelper.guiDecompOperator == "d")
                    {
                        radioButton2.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "p")
                    {
                        radioButton4.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "dp")
                    {
                        radioButton9.IsChecked = true;
                    }
                    //else if (this.decompOptions.guiDecompOperator == "n")
                    //{
                    //    radioButton21.IsChecked = true;
                    //}
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "m")
                    {
                        radioButton22.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "q")
                    {
                        radioButton24.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "mp")
                    {
                        radioButton29.IsChecked = true;
                    }
                }
                else {
                    if (this.decompOptions2.operatorHelper.guiDecompOperator == "n")
                    {
                        radioButton5.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "d")
                    {
                        radioButton6.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "p")
                    {
                        radioButton8.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "dp")
                    {
                        radioButton10.IsChecked = true;
                    }


                    //else if (this.decompOptions.guiDecompOperator == "n")
                    //{
                    //    radioButton25.IsChecked = true;
                    //}
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "m")
                    {
                        radioButton26.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "q")
                    {
                        radioButton28.IsChecked = true;
                    }
                    else if (this.decompOptions2.operatorHelper.guiDecompOperator == "mp")
                    {
                        radioButton30.IsChecked = true;
                    }
                }

                if (this.decompOptions2.operatorHelper.guiDecompIsShares)
                {
                    checkBox1.IsChecked = true;
                }

                if (this.decompOptions2.decompTablesFormat.showErrors)
                {
                    checkBoxErrors.IsChecked = true;
                }
                
                if (this.decompOptions2.operatorHelper.guiDecompIsRef)
                {
                    checkBox2.IsChecked = true;
                }
            }
            else
            {
                //All these buttons are in the "Decomp" columns, codes are from e.g. UDVALG<p> or UDVALG<q> calls from command lines/files.
                if (G.Equal(decompOptions2.prtOptionLower, "d"))
                {
                    radioButton6.IsChecked = true;
                }
                if (G.Equal(decompOptions2.prtOptionLower, "p"))
                {
                    radioButton8.IsChecked = true;
                }
                if (G.Equal(decompOptions2.prtOptionLower, "m"))
                {
                    radioButton26.IsChecked = true;
                }
                if (G.Equal(decompOptions2.prtOptionLower, "q"))
                {
                    radioButton28.IsChecked = true;
                }
            }
        }

        public WindowDecomp(DecompOptions2 decompOptions)
        {
            
            this.decompOptions2 = decompOptions;

            this.isInitializing = true; //so that radiobuttons etc do not fire right now
            InitializeComponent();
            this.isInitializing = false;  //ready for clicking
            
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

            this.decompOptions2.guiDecompChangedCells.Clear();
            this.decompOptions2.guiDecompIsSelecting = false;
            this.decompOptions2.guiDecompIsSelectingAll = false;

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
            //this.decompOptions.guiDecompOperator = "n";   
            //RefreshList();
            this.Loaded += WindowDecomp_Loaded;
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

        private void PutTableIntoGrid2(Grid g, Table table, GekkoTableTypes type, DecompOptions2 decompOptions)
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
                this.decompOptions2.guiDecompIsSelectingAll = true;
             
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

            if (this.decompOptions2.guiDecompIsSelectingAll)
            {
                r0 = 1;
                r1 = this.decompOptions2.guiDecompValues.GetRowMaxNumber();
                c0 = 1;
                c1 = this.decompOptions2.guiDecompValues.GetColMaxNumber();
            }
            else
            {
                CoordConversion(out r0, out c0, type, this.decompOptions2.guiDecompSelectedRowMin, this.decompOptions2.guiDecompSelectedColMin);
                CoordConversion(out r1, out c1, type, this.decompOptions2.guiDecompSelectedRowMax, this.decompOptions2.guiDecompSelectedColMax);
            }

            //Cannot just copy what is seen on the screen if it is a number -- in that case decimals would get lost in Excel.
            
            for (int i = r0; i <= r1; i++)
            {
                for (int j = c0; j <= c1; j++)
                {
                    int x = i; int y = j;                    

                    Cell c = this.decompOptions2.guiDecompValues.Get(x, y);
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
            GekkoDockPanel2 dockPanel = new GekkoDockPanel2();
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
            GekkoDockPanel2 dockPanel = (GekkoDockPanel2)sender;
            if (dockPanel.type != GekkoTableTypes.TableContent) return;
            if (!this.decompOptions2.guiDecompIsSelecting) return;            
            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);
            Select(g, col, row);
            this.decompOptions2.guiDecompIsSelecting = false;
            this.decompOptions2.guiDecompIsSelectingAll = false;
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GekkoDockPanel2 dockPanel = (GekkoDockPanel2)sender;            
            
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
                            Program.FlowChart(s, code, GekkoTime.FromStringToGekkoTime(s2));
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
                this.decompOptions2.guiDecompIsSelecting = true;
                DefaultGrid(g);
        
                border.BorderThickness = new Thickness(3);
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 0);
                border.BorderBrush = mySolidColorBrush;

                this.decompOptions2.guiDecompLastClickedRow = row;  //normal click
                this.decompOptions2.guiDecompLastClickedCol = col;
                this.decompOptions2.guiDecompChangedCells.Add(row + "," + col, 0);

                TextBlock textBlock = (TextBlock)border.Child;
                textBlock.Padding = new Thickness(0, 0, 1, 0);          
            }            
        }        

        private static Window GetWindow(GekkoDockPanel2 dockPanel)
        {
            Grid p = (Grid)dockPanel.Parent;
            Border p2 = (Border)p.Parent;
            Canvas p3 = (Canvas)p2.Parent;
            Border p4 = (Border)p3.Parent;
            ScrollViewer p5 = (ScrollViewer)p4.Parent;
            StackPanel p6 = (StackPanel)p5.Parent;
            DockPanel p7 = (DockPanel)p6.Parent;
            Window ww = (Window)p7.Parent;
            return ww;
        }

        private void Select(Grid g, int col, int row)
        {
            DefaultGrid(g);
            this.decompOptions2.guiDecompSelectedRowMin = Math.Min(this.decompOptions2.guiDecompLastClickedRow, row);
            this.decompOptions2.guiDecompSelectedRowMax = Math.Max(this.decompOptions2.guiDecompLastClickedRow, row);
            this.decompOptions2.guiDecompSelectedColMin = Math.Min(this.decompOptions2.guiDecompLastClickedCol, col);
            this.decompOptions2.guiDecompSelectedColMax = Math.Max(this.decompOptions2.guiDecompLastClickedCol, col);
            double sum = 0d;
            int count = 0;
            foreach (GekkoDockPanel2 d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                //b2.BorderBrush = null;
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (this.decompOptions2.guiDecompSelectedRowMin <= row2 && row2 <= this.decompOptions2.guiDecompSelectedRowMax)
                {
                    if (this.decompOptions2.guiDecompSelectedColMin <= col2 && col2 <= this.decompOptions2.guiDecompSelectedColMax)
                    {

                        if (!this.decompOptions2.guiDecompChangedCells.ContainsKey(row2 + "," + col2))
                        {
                            this.decompOptions2.guiDecompChangedCells.Add(row2 + "," + col2, 0);
                        }                        
                        
                        SetDefaultBorder(b2);                        

                        SetBorderThickness(g, row2, col2, b2);            

                        d2.Background = Brushes.White;

                        double left = 0.15;
                        double right = 0;
                        double top = 0.15;
                        double bottom = 0;
                        if (this.decompOptions2.guiDecompSelectedRowMin == row2) top = 3;
                        if (this.decompOptions2.guiDecompSelectedRowMax == row2) bottom = 3;
                        if (this.decompOptions2.guiDecompSelectedColMin == col2) left = 3;
                        if (this.decompOptions2.guiDecompSelectedColMax == col2) right = 3;

                        b2.BorderBrush = Brushes.Black;
                        b2.BorderThickness = new Thickness(left, top, right, bottom);

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = Globals.GrayExcelSelect;
                        //border.BorderBrush = mySolidColorBrush;


                        if (row2 == this.decompOptions2.guiDecompLastClickedRow && col2 == this.decompOptions2.guiDecompLastClickedCol)
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
                        Cell c = this.decompOptions2.guiDecompValues.Get(x, y);
                        
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
            foreach (GekkoDockPanel2 d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (!this.decompOptions2.guiDecompChangedCells.ContainsKey(row2 + "," + col2)) continue;
                if (d2.originalBackgroundColor == null) d2.Background = Brushes.White;
                else d2.Background = d2.originalBackgroundColor;                
                SetDefaultBorder(b2);                   
                SetBorderThickness(g, row2, col2, b2);                
            }
            this.decompOptions2.guiDecompChangedCells.Clear();
        }

        private void Cell_Leave(object sender, MouseEventArgs e)
        {
            GekkoDockPanel2 dockPanel = (GekkoDockPanel2)sender;            

            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);

            if (this.decompOptions2.guiDecompIsSelecting)
            {            
            }
            else
            {
                //G.Writeln("cell " + row + " " + col);
                this.equation.Background = Brushes.White;
                //this.equation.Foreground = Brushes.White;

                int x; int y;
                CoordConversion(out x, out y, dockPanel.type, row, col);
                Cell c = this.decompOptions2.guiDecompValues.Get(x, y);
                string s = FindEquationText2(this.decompOptions2);
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

            Cell c = this.decompOptions2.guiDecompValues.Get(x, y);

            if (c != null && c.cellType == CellType.Text)
            {
                string var = c.CellText.TextData[0];

                string var2 = G.PrettifyTimeseriesHash(G.ExtractOnlyVariableIgnoreLag(var, Globals.leftParenthesisIndicator), true, true);
                                
                DecompOptions2 d = this.decompOptions2.Clone();                                

                //if (d.isSubst)
                //{
                //    //d.variable is the same
                //    d.subst.Add(var2);
                //    d.isSubWindow = true;
                //}
                //else
                //{
                    d.variable = var2;
                    d.isSubWindow = true;
                //}
                                
                CrossThreadStuff.Decomp2(d);
            }
            else
            {
                G.Writeln("*** ERROR: Unexpected link error");
                throw new GekkoException();

            }
        }    

        private void Cell_Enter(object sender, MouseEventArgs e)
        {
            GekkoDockPanel2 dockPanel = (GekkoDockPanel2)sender;
            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);  //0-based
            int row = (int)dockPanel.GetValue(Grid.RowProperty);  //0-based

            GekkoTableTypes type = dockPanel.type;
            int x; int y;
            CoordConversion(out x, out y, type, row, col);

            if (type == GekkoTableTypes.TableContent && this.decompOptions2.guiDecompIsSelecting)
            {
                Select(g, col, row);
            }
            else
            {
                Cell c = this.decompOptions2.guiDecompValues.Get(x, y);

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
                                if (decompOptions2.expressionOld != null)
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
                                if (decompOptions2.expressionOld != null)
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
                                if (decompOptions2.expressionOld != null)
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
                                if (decompOptions2.expressionOld != null)
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
                                if (decompOptions2.expressionOld != null)
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
                    string s = FindEquationText2(this.decompOptions2);
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
                if (this.decompOptions2.expression == null)
                {
                    if (equation == null) return;  //Happens during first rendering, when isChecked is set by C# on top-left radio-button (ignore it)
                }

                //SetRadioButtonDefaults();

                //string transformationCodeAugmented = null;
                if (this.decompOptions2.operatorHelper.guiDecompOperator != null)
                {
                    //There are 4 showing options: operator, isBaseline, isRaw, isShares.
                    string transformationCodeAugmented = this.decompOptions2.operatorHelper.guiDecompOperator;
                    if (this.decompOptions2.operatorHelper.guiDecompIsRef) transformationCodeAugmented = "r" + transformationCodeAugmented;
                    if (this.decompOptions2.operatorHelper.guiDecompIsRaw) transformationCodeAugmented = "x" + transformationCodeAugmented;
                    if (this.decompOptions2.operatorHelper.guiDecompIsShares) transformationCodeAugmented = "s" + transformationCodeAugmented;  //is put on last
                    if (this.decompOptions2.operatorHelper.guiDecompIsRaw && this.decompOptions2.operatorHelper.guiDecompIsShares)
                    {
                        G.Writeln2("*** ERROR: Cannot show decomposition with both 'raw' and 'shares' option at the same time");
                        throw new GekkoException();
                    }
                    this.decompOptions2.prtOptionLower = transformationCodeAugmented;
                }                

                //"x" and "s" are mutually exclusive: in raw mode shares are not meaningful
                //so "sd", "sp", "sdp" + "sm", "sq", "smp" are used                

                SetRadioButtonsDefaults();

                _statusText.Text = "";

                string operator1, isShares;
                DecompIsSharesOrPercentageType(out operator1, out isShares);

                GekkoTime per1 = this.decompOptions2.t1;
                GekkoTime per2 = this.decompOptions2.t2;

                bool isRaw = false;
                if (this.decompOptions2.prtOptionLower.StartsWith("x")) isRaw = true;

                GekkoSmpl smpl = new GekkoSmpl(per1, per2);
                
                IVariable y0a = null;
                IVariable y0aRef = null;
                                
                Table table = Decomp.DecompMain(smpl, per1, per2, operator1, isShares, this.decompOptions2);

                string s = FindEquationText2(this.decompOptions2);
                equation.Text = s;

                //
                // NOTE:
                //
                flowText.Visibility = Visibility.Collapsed;

                //TODO: what is this? delete?
                //TODO: what is this? delete?
                //TODO: what is this? delete?
                this.decompOptions2.guiDecompValues = table;

                if (G.IsUnitTesting() && Globals.showDecompTable == false)
                {
                    Globals.lastDecompTable = table;
                }
                else
                {
                    ClearGrid();
                    MakeGuiTable2(table, this.decompOptions2);
                }
                
                return;

            }
            catch (Exception e)
            {
                if (!G.IsUnitTesting())
                {
                    this.isClosing = true;
                    MessageBox.Show("*** ERROR: Decomp update failed: maybe some variables or databanks are non-available?");
                }
            }
        }
         
        private void DecompIsSharesOrPercentageType(out string operator1, out string isShares)
        {
            operator1 = this.decompOptions2.prtOptionLower;
            isShares = null;
            if (operator1.StartsWith("s"))
            {
                operator1 = operator1.Substring(1);
                isShares = "s";
            }

            this.decompOptions2.decompTablesFormat.isPercentageType = false;
            if (operator1.Contains("p") || operator1.Contains("q") || isShares == "s")
            {
                this.decompOptions2.decompTablesFormat.isPercentageType = true;
            }
        }

        private void SetRadioButtonsDefaults()
        {
            //if (this.decompOptions2.isSubst) subst.IsChecked = true;
            //if (this.decompOptions2.isSort) sort.IsChecked = true;
            //if (this.decompOptions2.isPool) pool.IsChecked = true;

            if (this.decompOptions2.operatorHelper.guiDecompIsRef)
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

            if (this.decompOptions2.operatorHelper.guiDecompIsShares)
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

            if (this.decompOptions2.operatorHelper.guiDecompIsRaw)
            {
                checkBox1.IsEnabled = false;  //shares
                checkBox1.Opacity = 0.5;
                flowText.Visibility = Visibility.Collapsed;
            }

            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            if (G.Equal(this.decompOptions2.prtOptionLower, "m") || G.Equal(this.decompOptions2.prtOptionLower, "xm") || G.Equal(this.decompOptions2.prtOptionLower, "q") || G.Equal(this.decompOptions2.prtOptionLower, "xq") || G.Equal(this.decompOptions2.prtOptionLower, "mp") || G.Equal(this.decompOptions2.prtOptionLower, "xmp"))
            {
                checkBox2.IsEnabled = false;  //baseline, not meaningful for multiplier types
                checkBox2.Opacity = 0.5;
            }
        }

        private void SetRadioButtonDefaults()
        {
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
            PutTableIntoGrid(this.grid1, table, GekkoTableTypes.TableContent, decompOptions);            
            CreateGridRowsAndColumns(this.grid1Left, table, GekkoTableTypes.Left);
            PutTableIntoGrid(this.grid1Left, table, GekkoTableTypes.Left, decompOptions);
            CreateGridRowsAndColumns(this.grid1Top, table, GekkoTableTypes.Top);
            PutTableIntoGrid(this.grid1Top, table, GekkoTableTypes.Top, decompOptions);
            CreateGridRowsAndColumns(this.gridUpperLeft, table, GekkoTableTypes.UpperLeft);
            PutTableIntoGrid(this.gridUpperLeft, table, GekkoTableTypes.UpperLeft, decompOptions);            
        }

        private void MakeGuiTable2(Table table, DecompOptions2 decompOptions)
        {
            CreateGridRowsAndColumns(this.grid1, table, GekkoTableTypes.TableContent);
            PutTableIntoGrid2(this.grid1, table, GekkoTableTypes.TableContent, decompOptions);
            CreateGridRowsAndColumns(this.grid1Left, table, GekkoTableTypes.Left);
            PutTableIntoGrid2(this.grid1Left, table, GekkoTableTypes.Left, decompOptions);
            CreateGridRowsAndColumns(this.grid1Top, table, GekkoTableTypes.Top);
            PutTableIntoGrid2(this.grid1Top, table, GekkoTableTypes.Top, decompOptions);
            CreateGridRowsAndColumns(this.gridUpperLeft, table, GekkoTableTypes.UpperLeft);
            PutTableIntoGrid2(this.gridUpperLeft, table, GekkoTableTypes.UpperLeft, decompOptions);
        }

        private string FindEquationText(DecompOptions decompOptions)
        {
            if (decompOptions.expressionOld != null)
            {
                return decompOptions.expressionOld;
            }
            else
            {
                EquationHelper eh = Program.FindEquationByMeansOfVariableName(this.decompOptions2.variable);
                if (eh == null) return ""; //probably only when model is changed while UDVALG window is open (this is illegal anyway, and a popup will appear)
                else return ((EquationHelper)eh).equationText;
            }
        }

        private string FindEquationText2(DecompOptions2 decompOptions)
        {
            if (decompOptions.expressionOld != null)
            {
                return decompOptions.expressionOld;
            }
            else
            {
                EquationHelper eh = Program.FindEquationByMeansOfVariableName(this.decompOptions2.variable);
                if (eh == null) return ""; //probably only when model is changed while UDVALG window is open (this is illegal anyway, and a popup will appear)
                else return ((EquationHelper)eh).equationText;
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "n";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "d";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton6_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = false;
                this.decompOptions2.operatorHelper.guiDecompOperator = "d";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "p";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton8_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = false;
                this.decompOptions2.operatorHelper.guiDecompOperator = "p";
                RecalcCellsWithNewType();
            }
        }        

        private void radioButton21_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "n";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton22_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "m";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton26_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = false;
                this.decompOptions2.operatorHelper.guiDecompOperator = "m";
                RecalcCellsWithNewType();
            }
        }        

        private void radioButton24_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "q";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton28_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = false;
                this.decompOptions2.operatorHelper.guiDecompOperator = "q";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton29_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "mp";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton30_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = false;
                this.decompOptions2.operatorHelper.guiDecompOperator = "mp";
                RecalcCellsWithNewType();
            }
        }

        private void radioButton9_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = true;
                this.decompOptions2.operatorHelper.guiDecompOperator = "dp";
                RecalcCellsWithNewType();
            }
        }       
        
        private void radioButton10_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRaw = false;
                this.decompOptions2.operatorHelper.guiDecompOperator = "dp";
                RecalcCellsWithNewType();
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsShares = true;
                RecalcCellsWithNewType();
            }
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsShares = false;
                RecalcCellsWithNewType();
            }
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRef = true;
                RecalcCellsWithNewType();
            }
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.operatorHelper.guiDecompIsRef = false;
                RecalcCellsWithNewType();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.guiDecompWindowTopDistance = Math.Max(1, (int)this.Top);
            Globals.guiDecompWindowLeftDistance = Math.Max(1, (int)this.Left);
            try
            {
                if (Globals.windowsDecomp2 != null && this != null) Globals.windowsDecomp2.Remove(this);
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
                this.decompOptions2.localBanks = null;  //clearing this, forcing window to use vales from Gekko databanks
                this.RecalcCellsWithNewType();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                if (this.decompOptions2.decompTablesFormat.isPercentageType)
                {
                    this.decompOptions2.decompTablesFormat.decimalsPch++;
                }
                else
                {
                    this.decompOptions2.decompTablesFormat.decimalsLevel++;
                }
                RecalcCellsWithNewType();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                if (this.decompOptions2.decompTablesFormat.isPercentageType)
                {
                    this.decompOptions2.decompTablesFormat.decimalsPch--;
                    if (this.decompOptions2.decompTablesFormat.decimalsPch < 0) this.decompOptions2.decompTablesFormat.decimalsPch = 0;
                }
                else
                {
                    this.decompOptions2.decompTablesFormat.decimalsLevel--;
                    if (this.decompOptions2.decompTablesFormat.decimalsLevel < 0) this.decompOptions2.decompTablesFormat.decimalsLevel = 0;
                }
                RecalcCellsWithNewType();
            }
        }

        private void checkBoxErrors_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.decompTablesFormat.showErrors = true;
                RecalcCellsWithNewType();
            }
        }

        private void checkBoxErrors_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompOptions2.decompTablesFormat.showErrors = false;
                RecalcCellsWithNewType();                
            }
        }

        //private void Sort_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.decompOptions2.isSort = false;
        //    if (sort.IsChecked == true) this.decompOptions2.isSort = true;
        //}

        //private void Pool_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.decompOptions2.isPool = false;
        //    if (pool.IsChecked == true) this.decompOptions2.isPool = true;
        //}

        //private void Subst_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.decompOptions2.isSubst = false;
        //    if (subst.IsChecked == true) this.decompOptions2.isSubst = true;
        //}

        //private void Sort_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    this.decompOptions2.isSort = false;
        //    if (sort.IsChecked == true) this.decompOptions2.isSort = true;
        //}

        //private void Pool_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    this.decompOptions2.isPool = false;
        //    if (pool.IsChecked == true) this.decompOptions2.isPool = true;
        //}

        //private void Subst_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    this.decompOptions2.isSubst = false;
        //    if (subst.IsChecked == true) this.decompOptions2.isSubst = true;
        //}
    }

    public class GekkoDockPanel2 : DockPanel
    {
        public WindowDecomp.GekkoTableTypes type = WindowDecomp.GekkoTableTypes.Unknown;
        public Brush originalBackgroundColor = null;
    }

    public class DecompOperatorHelper
    {
        //this object contains the important operators regarding how
        //the cells are shown.
        public string guiDecompOperator = null;  //used to be "n"
        public bool guiDecompIsShares = false;
        public bool guiDecompIsRaw = true;
        public bool guiDecompIsRef = false;
    }

    public class DecompOptions2
    {
        public DecompTablesFormat decompTablesFormat = new DecompTablesFormat();

        //-------- tranformation start --------------
        public DecompOperatorHelper operatorHelper = new DecompOperatorHelper();

        public bool isNew = false;

        //public bool isSubst = false;
        //public bool isPool = false;
        //public bool isSort = false;

        //public bool onlyTable = false;
        //public Table table = null;

        public int numberOfRecalcs = 0;  //used to pause main thread until the DECOMP window has calculated.
        public string variable = null;
        public List<string> variable_subelement = null;
        
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
        
        //public GekkoSmpl smplForFunc = null;

        public List<string> subst = new List<string>();

        public IVariable name = null;  //only active for names like x, x[a] and the like, not for expressions

        
        
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
        
        public string dream = null;  //experimental

        //public DecompData decompData = null;
        public bool hasCalculatedQuo = false;
        public bool hasCalculatedRef = false;

        public List<string> vars2 = null;

        public List<Link> link = new List<Link>();
        public List<List<string>> where = new List<List<string>>();
        public List<List<string>> group = new List<List<string>>();
        public List<string> rows = new List<string>();
        public List<string> cols = new List<string>();
        

        public DecompOptions2 Clone()
        {
            //clones relevant parts for new window
            DecompOptions2 d = new DecompOptions2();
            d.decompTablesFormat = new DecompTablesFormat();
            d.decompTablesFormat.decimalsLevel = this.decompTablesFormat.decimalsLevel;
            d.decompTablesFormat.decimalsPch = this.decompTablesFormat.decimalsPch;
            d.decompTablesFormat.isPercentageType = this.decompTablesFormat.isPercentageType;
            d.decompTablesFormat.showErrors = this.decompTablesFormat.showErrors;


            //d.tp = this.tp;
            d.variable = this.variable;
            d.t1 = this.t1;
            d.t2 = this.t2;
            d.prtOptionLower = this.prtOptionLower;

            d.operatorHelper = new DecompOperatorHelper();
            d.operatorHelper.guiDecompOperator = this.operatorHelper.guiDecompOperator;
            d.operatorHelper.guiDecompIsShares = this.operatorHelper.guiDecompIsShares;
            d.operatorHelper.guiDecompIsRaw = this.operatorHelper.guiDecompIsRaw;
            d.operatorHelper.guiDecompIsRef = this.operatorHelper.guiDecompIsRef;
            

            d.modelHash = this.modelHash;
            d.type = this.type;
            
            //d.decimalsLevel = this.decimalsLevel;
            
            d.dream = this.dream;

            //d.isSort = this.isSort;
            //d.isSubst = this.isSubst;
            //d.isPool = this.isPool;
            foreach (string s in this.subst)
            {
                d.subst.Add(s);
            }

            foreach (List<string> x1 in this.where)
            {
                List<string> temp = new List<string>();
                foreach (string x2 in x1)
                {
                    temp.Add(x2);
                }
                d.where.Add(temp);
            }

            foreach (List<string> x1 in this.group)
            {
                List<string> temp = new List<string>();
                foreach (string x2 in x1)
                {
                    temp.Add(x2);
                }
                d.group.Add(temp);
            }

            foreach (Link x1 in this.link)
            {
                Link temp = new Link();
                temp.varnames = x1.varnames;
                temp.eqname = x1.eqname;
                temp.expressions = x1.expressions; //probably ok not to clone
                d.link.Add(temp);
            }

            List<string> tempCols = new List<string>();
            foreach (string s in this.cols)
            {
                tempCols.Add(s);
            }
            d.cols = tempCols;

            List<string> tempRows = new List<string>();
            foreach (string s in this.rows)
            {
                tempRows.Add(s);
            }
            d.rows = tempRows;

            return d;
        }
    }

}
