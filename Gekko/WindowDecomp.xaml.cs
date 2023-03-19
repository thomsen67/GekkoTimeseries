/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gekko
{


    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowDecomp : Window
    {

        public enum GekkoTableTypes
        {
            TableContent, Left, Top, UpperLeft, Unknown
        }

        public enum TaskType
        {
            None, Rows, Cols, Filters, Invisible
        }

        public DecompFind decompFind = null;

        //public BitmapImage green = null;
        //public BitmapImage yellow = null;
        //public BitmapImage red = null;

        public DecompDatas decompDatas = new DecompDatas(); //stores data for reuse, for instance for fast pivot selection

        public int frozenRows=0;
        public int frozenCols=0;
        
        public bool isClosing = false;
        public bool isInitializing = false; //a bit hacky, to handle radiobutton1 firing a clicked event when initializing
        
        public Grid _grid = null;
        public string _activeVariable = null;

        private ObservableCollection<GekkoTask> _list = new ObservableCollection<GekkoTask>();
        ListViewDragDropManager<GekkoTask> dragMgr;
        
        public string uglyHack_name = null;
        
        private int _numValue = 0;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue < 100) NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue > 0) NumValue--;
        }

        /// <summary>
        /// Invalid input is ignored. It accepts "", which is interpreted as 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            string s = txtNum.Text.Trim();
            int i = 0;
            bool b = false;
            if (s == null || s == "")
            {
                b = true;
                i = 0;
            }
            else
            {
                if (s.StartsWith("00"))
                {
                    //do not accept, b will be = false
                }
                else
                {
                    b = int.TryParse(s, out i);
                }
            }

            if (b && i >= 0 && i <= 100)
            {
                if (!isInitializing)
                {
                    this.decompFind.decompOptions2.ignore = i;
                    _numValue = i;
                    RecalcCellsWithNewType(decompFind.model);
                }
            }
            else
            {
                txtNum.Text = _numValue.ToString();
            }
        }        

        public ObservableCollection<GekkoTask> taskList
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

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
            Button cmb = sender as Button;            
            GekkoTask task = cmb.DataContext as GekkoTask;

            bool isTree = false;
            if (task.Pivot_TaskType == TaskType.Filters) isTree = true;

            if (false && isTree)
            {
                WindowTreeViewWithCheckBoxes w = new WindowTreeViewWithCheckBoxes();
                w.ShowDialog();

                System.Windows.Controls.TreeView tree = w.tree;
                List<FooViewModel> items = tree.ItemsSource as List<FooViewModel>;
                FooViewModel model = items[0];
                List<string> selected = new List<string>();
                Walk(model, selected, 0);
                //                                

                List<string> selectedOld = null;


                //if (G.Equal(G.HandleInternalIdentifyer1(ff.name), task.Pivot_Text))
                {

                    selectedOld = task.pivot_filterSelected;
                    bool equal = true;
                    foreach (string s in selectedOld)
                    {
                        if (!selected.Contains(s, StringComparer.OrdinalIgnoreCase))
                        {
                            equal = false;
                            break;
                        }
                    }
                    foreach (string s in selected)
                    {
                        if (!selectedOld.Contains(s, StringComparer.OrdinalIgnoreCase))
                        {
                            equal = false;
                            break;
                        }
                    }
                    if (equal == false)
                    {
                        task.pivot_filterSelected = selected;  //pivotfix
                        PutGuiPivotSelectionIntoDecompOptions(taskList);
                        RecalcCellsWithNewType(decompFind.model);
                    }
                }
            }
            else
            {
                if (false)
                {
                    WindowDecompSortEtc w = new WindowDecompSortEtc();
                    w.ShowDialog();
                }
                else
                {
                    MessageBox.Show("In a later Gekko version, sorting and filtering etc. of the pivot rows/colums will be possible here.");
                }
            }
        }

        public static void Walk(FooViewModel node, List<string>selected, int d)
        {
            //pivotfix
            //G.Writeln(G.Blanks(2 * d) + node.Name + "   " + node.IsInitiallySelected + " --> " + node.IsChecked);
            if (node == null) return;
            if (node.Children == null || node.Children.Count == 0)
            {
                if (node.IsChecked == true)
                {
                    selected.Add(node.Name);
                }
                return;
            }
            foreach (var child in node.Children) Walk(child, selected, d + 1);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
            //add new item from list (in rows, cols or filters), the name is 'chosen'
            if (e.AddedItems.Count < 1) return;  //why does this happen?????????
            ComboBox cmb = sender as ComboBox;
            string chosen = e.AddedItems[0] as string;
            GekkoTask task = cmb.DataContext as GekkoTask;
            string text = task.Pivot_Text;
            int ii = -12345;
            TaskType type = TaskType.None;
            if (text == Globals.internalPivotRows)
            {
                type = TaskType.Rows;
                for (int i = 0; i < taskList.Count; i++)
                {
                    if (taskList[i].Pivot_Text == Globals.internalPivotCols)
                    {
                        ii = i;
                    }
                }
                //decompOptions2.rows.Add(G.HandleInternalIdentifyer2(chosen));
            }
            else if (text == Globals.internalPivotCols)
            {
                type = TaskType.Cols;
                for (int i = 0; i < taskList.Count; i++)
                {
                    if (taskList[i].Pivot_Text == Globals.internalPivotFilters)
                    {
                        ii = i;
                    }
                }
                //decompOptions2.cols.Add(G.HandleInternalIdentifyer2(chosen));
            }
            else if (text == Globals.internalPivotFilters)
            {
                type = TaskType.Filters;
                ii = taskList.Count - 1;
            }
            else
            {
                new Error("Selection problem.");
            }
            List<GekkoTask> m = new List<GekkoTask>();
            int i2 = 0;
            foreach (GekkoTask t in taskList)
            {
                if (t.I == ii)
                {
                    //string x = "Collapsed";
                    //if (type == TaskType.Filters) x = "Visible";
                    List<string> filters = null;
                    if (type == TaskType.Filters)
                    {
                        filters = GetAllPossibleValuesForListFilter(chosen, this.decompFind.decompOptions2);  //we need to set the chosen values to all values (so the filter has no effect to begin with)
                    }
                    m.Add(new GekkoTask(chosen, "Transparent", "Visible", "Collapsed", "Visible", "Normal", type, i2++, null, filters, this.decompFind.decompOptions2));
                }                
                m.Add(t);
                t.I = i2++;                
            }
            taskList.Clear();
            foreach (GekkoTask t in m) taskList.Add(t);
            PutGuiPivotSelectionIntoDecompOptions(taskList);
            RefreshList2(type);
            RecalcCellsWithNewType(decompFind.model);
        }        

        public static List<string> GetAllPossibleValuesForListFilter(string name, DecompOptions2 decompOptions2)
        {
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO do this when looping all the rows in the dataframe, instead of looking for variables externally
            //TODO TODO TODO
            //TODO TODO TODO

            List<GekkoDictionary<string, string>> xx = decompOptions2.freeValues;

            List<string> list = new List<string>();

            for (int i = 0; i < xx.Count; i++)
            {                
                if (G.Equal(decompOptions2.all[i], name))  //.all has same members as colnames in frame.
                {
                    foreach (string ss in xx[i].Keys) list.Add(ss);
                    break;
                }
            }

            if (decompOptions2.ageHierarchy && name.EndsWith(Globals.ageHierarchyName))
            {
                //List<string> list = Stringlist.GetListOfStringsFromList(Program.databanks.GetFirst().GetIVariable("#" + Globals.ageName));
                SortedDictionary<string, List<string>> m1 = new SortedDictionary<string, List<string>>();
                List<string> m2 = new List<string>();
                GetHierarchyAggregateNames(list, m1, m2);
                List<string> names = new List<string>();
                foreach (string s in m1.Keys)
                {
                    names.Add(s);
                }
                foreach (string s in m2)
                {
                    names.Add(s);
                }
                return names;
            }
            else
            {
                //return Stringlist.GetListOfStringsFromList(Program.databanks.GetFirst().GetIVariable(name));
                return list;
            }
        }

        public static void GetHierarchyAggregateNames(List<string> list, SortedDictionary<string, List<string>> m1, List<string> m2)
        {
            foreach (string s in list)
            {
                //string s = iv.ConvertToString();
                int i = -12345;
                if (int.TryParse(s, out i))
                {
                    string s2 = G.GroupBy10(i);
                    if (!m1.ContainsKey(s2))
                    {
                        m1.Add(s2, new List<string>());
                    }
                    m1[s2].Add(s);
                }
                else
                {
                    m2.Add(s);
                }
            }
        }

        private void RefreshRowsColsFiltersList()
        {
            RefreshList2(TaskType.None);
            taskList.Clear();
            int i = 0;
            taskList.Add(new GekkoTask(Globals.internalPivotRows, Globals.internalPivotRowColor, "Collapsed", "Visible", "Hidden", "Bold", TaskType.None, i++, this.decompFind.decompOptions2.free, null, this.decompFind.decompOptions2));
            foreach (string s in this.decompFind.decompOptions2.rows)
            {
                taskList.Add(new GekkoTask(s, "Transparent", "Visible", "Collapsed", "Visible", "Normal", TaskType.Rows, i++, null, null, this.decompFind.decompOptions2));
            }
            //taskList[taskList.Count - 1].LineColor = "Black";

            taskList.Add(new GekkoTask(Globals.internalPivotCols, Globals.internalPivotRowColor, "Collapsed", "Visible", "Hidden", "Bold", TaskType.None, i++, this.decompFind.decompOptions2.free, null, this.decompFind.decompOptions2));
            foreach (string s in this.decompFind.decompOptions2.cols)
            {
                taskList.Add(new GekkoTask(s, "Transparent", "Visible", "Collapsed", "Visible", "Normal", TaskType.Cols, i++, null, null, this.decompFind.decompOptions2));
            }
            //taskList[taskList.Count - 1].LineColor = "Black";

            taskList.Add(new GekkoTask(Globals.internalPivotFilters, Globals.internalPivotRowColor, "Collapsed", "Visible", "Hidden", "Bold", TaskType.None, i++, this.decompFind.decompOptions2.freeFilter, null, this.decompFind.decompOptions2));
            foreach (FrameFilter ff in this.decompFind.decompOptions2.filters)
            {
                taskList.Add(new GekkoTask(G.HandleInternalIdentifyer1(ff.name), "Transparent", "Visible", "Collapsed", "Visible", "Normal", TaskType.Filters, i++, null, ff.selected, this.decompFind.decompOptions2));
            }
            //taskList[taskList.Count - 1].LineColor = "Black";

            taskList.Add(new GekkoTask("", "Transparent", "Collapsed", "Collapsed", "Collapsed", "Normal", TaskType.Invisible, i++, null, null, this.decompFind.decompOptions2));
            

            for (int i2 = 0; i2 < taskList.Count; i2++)
            {
                taskList[i2].Pivot_Text = G.HandleInternalIdentifyer1(taskList[i2].Pivot_Text);
            }            
        }

        private void RefreshList2(TaskType taskType)
        {   
            this.decompFind.decompOptions2.freeFilter.Clear();
            this.decompFind.decompOptions2.free.Clear();
            foreach (string s in this.decompFind.decompOptions2.all)
            {
                if (s == "value") continue;
                if (s == "valueAlternative") continue;
                if (s == "valueLevel") continue;
                if (s == "valueLevelLag") continue;
                if (s == "valueLevelLag2") continue;
                if (s == "valueLevelRef") continue;
                if (s == "valueLevelRefLag") continue;
                if (s == "valueLevelRefLag2") continue;
                if (s == "fullVariableName") continue;
                if (s == "equ") continue;
                bool isFilter = false;
                foreach (FrameFilter ff in this.decompFind.decompOptions2.filters)
                {
                    if (G.Equal(ff.name, G.HandleInternalIdentifyer2(s)))
                    {
                        isFilter = true;
                        break;
                    }
                }
                if (!isFilter)
                {
                    this.decompFind.decompOptions2.freeFilter.Add(G.HandleInternalIdentifyer1(s));
                }

                if (this.decompFind.decompOptions2.rows.Contains(G.HandleInternalIdentifyer2(s)) || this.decompFind.decompOptions2.cols.Contains(G.HandleInternalIdentifyer2(s)))
                {

                }
                else
                {
                    this.decompFind.decompOptions2.free.Add(G.HandleInternalIdentifyer1(s));
                }
            }            
        }

        void WindowDecomp_Loaded(object sender, RoutedEventArgs e)
        {
            // Give the ListView an ObservableCollection of Task
            // as a data source.  Note, the ListViewDragManager MUST
            // be bound to an ObservableCollection, where the collection's
            // type parameter matches the ListViewDragManager's type
            // parameter (in this case, both have a type parameter of Task).

            taskList = new ObservableCollection<GekkoTask>();
            RefreshRowsColsFiltersList();

            this.listViewRowsColsFilters.ItemsSource = taskList;
            // This is all that you need to do, in order to use the ListViewDragManager.
            this.dragMgr = new ListViewDragDropManager<GekkoTask>(this.listViewRowsColsFilters);
            this.dragMgr.ListView = this.listViewRowsColsFilters;
            this.dragMgr.ShowDragAdorner = true;
            this.dragMgr.DragAdornerOpacity = 0.5d;  //so that something can still be seen underneath
            this.listViewRowsColsFilters.ItemContainerStyle = this.FindResource("ItemContStyle") as Style;
            this.dragMgr.ProcessDrop += DragAndDrop;
            // Hook up events on both ListViews to that we can drag-drop
            // items between them.
            this.listViewRowsColsFilters.DragEnter += OnListViewDragEnter;
            this.listViewRowsColsFilters.Drop += OnListViewDrop;
        }

        void OnListViewDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        void OnListViewDrop(object sender, DragEventArgs e)
        {
            if (e.Effects == DragDropEffects.None)
                return;

            GekkoTask task = e.Data.GetData(typeof(GekkoTask)) as GekkoTask;
            if (sender == this.listViewRowsColsFilters)
            {
                if (this.dragMgr.IsDragInProgress) return;
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

        void DragAndDrop(object sender, ProcessDropEventArgs<GekkoTask> e)
        {
            //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
            e.Effects = DragDropEffects.Move;            
            List<GekkoTask> m = new List<GekkoTask>();
            TaskType type = TaskType.None;

            for (int i = 0; i < e.ItemsSource.Count; i++)
            {
                if (i == e.OldIndex) continue;

                if (i == e.NewIndex)
                {                   

                    for (int ii = i - 1; ii >= 0; ii--)
                    {
                        //go backwards to find new tasktype (may be a move to another type)
                        if (e.ItemsSource[ii].Pivot_Text == Globals.internalPivotRows)
                        {
                            type = TaskType.Rows;
                            break;
                        }
                        else if (e.ItemsSource[ii].Pivot_Text == Globals.internalPivotCols)
                        {
                            type = TaskType.Cols;
                            break;
                        }
                        else if (e.ItemsSource[ii].Pivot_Text == Globals.internalPivotFilters)
                        {
                            type = TaskType.Filters;
                            break;
                        }                        
                    }
                    if (type == TaskType.None || type == TaskType.Filters)
                    {
                        //this.decompFind.decompOptions2 = this.decompFind.decompOptions2Previous;
                        MessageBox.Show("Illegal Rows/Cols/Filters drag");
                        return;
                    }

                    e.ItemsSource[e.OldIndex].Pivot_TaskType = type;
                    if(type!=TaskType.Filters) e.ItemsSource[e.OldIndex].Pivot_ButtonVisible3 = "Collapse";                    
                    m.Add(e.ItemsSource[e.OldIndex]);
                }
                m.Add(e.ItemsSource[i]);
            }
            int i2 = 0;
            e.ItemsSource.Clear();
            foreach (GekkoTask t in m)
            {
                e.ItemsSource.Add(t); //must clear and reuse existing object, a new object will fail to update
                t.I = i2++;
            }            
            PutGuiPivotSelectionIntoDecompOptions(e.ItemsSource);
            RefreshList2(type);
            RecalcCellsWithNewType(decompFind.model);            
        }

        private void PutGuiPivotSelectionIntoDecompOptions(ObservableCollection<GekkoTask> collection)
        {
            this.decompFind.decompOptions2.rows.Clear();
            this.decompFind.decompOptions2.cols.Clear();
            this.decompFind.decompOptions2.filters.Clear();
            if (collection[0].Pivot_Text != Globals.internalPivotRows) throw new GekkoException();  //check can be removed at some point
            int state = 1;
            for (int i = 0; i < collection.Count; i++)
            {
                GekkoTask task = collection[i];
                if (i != task.I) throw new GekkoException();  //check can be removed at some point
                if (task.Pivot_Text == Globals.internalPivotCols)
                {
                    if (state != 1) throw new GekkoException(); //check can be removed at some point                    
                    state = 2;
                }
                if (task.Pivot_Text == Globals.internalPivotFilters)
                {
                    if (state != 2) throw new GekkoException(); //check can be removed at some point
                    state = 3;
                }
                if (task.Pivot_Text != "")
                {
                    if (state == 1 && task.Pivot_Text != Globals.internalPivotRows && task.Pivot_TaskType != TaskType.Rows) throw new GekkoException(); //check can be removed at some point
                    if (state == 2 && task.Pivot_Text != Globals.internalPivotCols && task.Pivot_TaskType != TaskType.Cols) throw new GekkoException(); //check can be removed at some point
                    if (state == 3 && task.Pivot_Text != Globals.internalPivotFilters && task.Pivot_TaskType != TaskType.Filters) throw new GekkoException(); //check can be removed at some point
                }
                else
                {
                    //this is the last invisible member of type .Invisible
                }
                if (state == 1 && task.Pivot_TaskType == TaskType.Rows) this.decompFind.decompOptions2.rows.Add(G.HandleInternalIdentifyer2(task.Pivot_Text));
                else if (state == 2 && task.Pivot_TaskType == TaskType.Cols) this.decompFind.decompOptions2.cols.Add(G.HandleInternalIdentifyer2(task.Pivot_Text));
                else if (state == 3 && task.Pivot_TaskType == TaskType.Filters)
                {
                    if (true)
                    {
                        FrameFilter ff = new FrameFilter();
                        ff.name = G.HandleInternalIdentifyer2(task.Pivot_Text);
                        ff.selected = new List<string>(); foreach (string s in task.pivot_filterSelected) ff.selected.Add(s);
                        this.decompFind.decompOptions2.filters.Add(ff);
                    }
                }
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listView.SelectedItem = null;  --> no, then we cannot move the row
        }

        public void SetRadioButtons()
        {
            if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xn" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xrn")
            {
                radioButton1.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xd" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xrd")
            {
                radioButton2.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xp" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xrp")
            {
                radioButton4.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xdp" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xrdp")
            {
                radioButton9.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xm")
            {
                radioButton22.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xq")
            {
                radioButton24.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "xmp")
            {
                radioButton29.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "n" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "rn")
            {
                radioButton5.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "d" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "rd")
            {
                radioButton6.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "p" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "rp")
            {
                radioButton8.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "dp" || this.decompFind.decompOptions2.decompOperator.OperatorLower() == "rdp")
            {
                radioButton10.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "m")
            {
                radioButton26.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "q")
            {
                radioButton28.IsChecked = true;
            }
            else if (this.decompFind.decompOptions2.decompOperator.OperatorLower() == "mp")
            {
                radioButton30.IsChecked = true;
            }            

            if (this.decompFind.decompOptions2.isShares)
            {
                checkBoxShares.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.OnlyRef)
            {
                checkRef.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.dyn)
            {
                checkBoxDyn.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.count == ECountType.N)
            {
                checkBoxCount.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.count == ECountType.Names)
            {
                checkBoxNames.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.missingAsZero)
            {
                if (false)
                {
                    //We allow this without complaints here
                    MessageBox.Show(MissingProblemText());
                }
                checkMZero.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.showErrors)
            {
                checkBoxErrors.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.sort)
            {
                checkBoxSort.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.plot)
            {
                checkBoxPlot.IsChecked = true;
            }

            if (this.decompFind.decompOptions2.expand)
            {
                checkBoxExpand.IsChecked = true;
            }

            if (!double.IsNaN(this.decompFind.decompOptions2.ignore))
            {
                this.NumValue = (int)this.decompFind.decompOptions2.ignore;
            }
        }

        public class ViewModel
        {
            public ObservableCollection<string> ComboBoxContent { get; private set; }

            public ViewModel()
            {
                ComboBoxContent = new ObservableCollection<string> { "test 1", "test 2" };
            }
        }        

        public WindowDecomp(DecompFind df)
        {
            //if (this.decompFind == null)
            //{
            //    this.decompFind = new DecompFind(EDecompFindNavigation.Decomp, 0, decompOptions2, this);
            //}
            //else
            //{
            //    this.decompFind.CreateChild(decompOptions2, EDecompFindNavigation.Decomp, this);
            //}            

            this.decompFind = df;
            this.isInitializing = true; //so that radiobuttons etc do not fire right now

            InitializeComponent();            

            this.textMerge.Visibility = Visibility.Collapsed;
            //this.buttonMergeHide.Visibility = Visibility.Collapsed;

            txtNum.Text = _numValue.ToString();
            this.scrollViewerDecomp1.Background = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));
            this.scrollViewerDecomp2.Background = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));

            this.isInitializing = false;  //ready for clicking

            //this.buttonMerge.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, Globals.LightBlueWord.R, Globals.LightBlueWord.G, Globals.LightBlueWord.B));
            this.buttonMerge.Background = new SolidColorBrush(Globals.GekkoModeBlue);
            if (this.decompFind.depth < 2) this.buttonMerge.Visibility = Visibility.Collapsed;

            DataContext = new ViewModel();  //MVVM style

            Canvas.SetTop(this.frezenBorder, Globals.guiTableCellHeight);
            Canvas.SetLeft(this.frezenBorder2, Globals.guiTableCellWidthFirst);
            this.gridUpperLeft.Width = Globals.guiTableCellWidthFirst;
            this.gridUpperLeft.Height = Globals.guiTableCellHeight;            

            this.frozenRows = Globals.freezeDecompRows;
            this.frozenCols = Globals.freezeDecompCols;

            this.Top = Globals.guiDecompWindowTopDistance;
            this.Left = Globals.guiDecompWindowLeftDistance;
            this.Height = Globals.guiDecompWindowHeightDistance;
            this.Width = Globals.guiDecompWindowWidthDistance;
            this.splitterHorizontal.Width = new GridLength(Globals.guiDecompWindowSplitterHorizontal);
            this.splitterVertical.Height = new GridLength(Globals.guiDecompWindowSplitterVertical);

            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

            //this.KeyDown += new KeyEventHandler(Window_KeyDown); //new System.Windows.Forms.KeyEventHandler(this.radioButton1_KeyDown);

            this.decompFind.decompOptions2.guiDecompChangedCells.Clear();
            this.decompFind.decompOptions2.guiDecompIsSelecting = false;
            this.decompFind.decompOptions2.guiDecompIsSelectingAll = false;

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
            }
            this.Loaded += WindowDecomp_Loaded;
        }

        private void CreateGridRowsAndColumns(Grid g, DecompOutput decompOutput, GekkoTableTypes type)
        {
            if (type == GekkoTableTypes.TableContent)
            {
                for (int i = 1 + this.frozenRows; i <= decompOutput.table.GetRowMaxNumber(); i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
                for (int j = 1 + this.frozenCols; j <= decompOutput.table.GetColMaxNumber(); j++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Globals.guiTableCellWidth) });
                }

                g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Globals.guiTableCellHeight) });  //otherwise, the last row does not show up in gui...
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Globals.guiTableCellWidthFirst) });  //otherwise, the last col does not show up in gui...
            }
            else if (type == GekkoTableTypes.Top)
            {
                for (int i = 1; i <= 1; i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
                for (int j = 1 + this.frozenCols; j <= decompOutput.table.GetColMaxNumber(); j++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Globals.guiTableCellWidth) });
                }
            }
            else if (type == GekkoTableTypes.Left)
            {
                for (int i = 1 + frozenRows; i <= decompOutput.table.GetRowMaxNumber(); i++)
                {
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                }
                for (int j = 1; j <= 1; j++)
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Globals.guiTableCellWidthFirst) });
                }
            }
            else if (type == GekkoTableTypes.UpperLeft)
            {
                g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Globals.guiTableCellWidthFirst) });
            }
        }
        
        private void PutTableIntoGrid2(Grid g, DecompOutput decompOutput, GekkoTableTypes type, DecompOptions2 decompOptions)
        {
            int offsetRow = 0;
            int offsetCol = 0;
            int startRow = 0;
            int endRow = 0;
            int startCol = 0;
            int endCol = 0;

            Decomp.ERowsCols variablesAreOnRows = Decomp.VariablesOnRowsOrCols(decompOptions);

            if (type == GekkoTableTypes.TableContent)
            {
                startRow = this.frozenRows + 1;
                startCol = this.frozenCols + 1;
                endRow = decompOutput.table.GetRowMaxNumber();
                endCol = decompOutput.table.GetColMaxNumber();
                offsetRow = this.frozenRows;
                offsetCol = this.frozenCols;
            }
            else if (type == GekkoTableTypes.Top)
            {
                startRow = 1;
                startCol = frozenCols + 1;
                endRow = this.frozenRows;
                endCol = decompOutput.table.GetColMaxNumber();
                offsetRow = 0;
                offsetCol = frozenCols;
            }
            else if (type == GekkoTableTypes.Left)
            {
                startRow = frozenRows + 1;
                startCol = 1;
                endRow = decompOutput.table.GetRowMaxNumber();
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

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    Cell c = decompOutput.table.Get(i, j);
                    if (c == null)
                    {
                        AddCell(g, i - 1 - offsetRow, j - 1 - offsetCol, "", false, type, null, variablesAreOnRows, decompOutput.red, decompOptions.decompOperator);  //transparent
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
                        if (c.numberShouldShowAsN) s = "N";
                    }
                    else if (c.cellType == CellType.Date) s = c.date;
                                        
                    string v = c.vars_hack?[0];
                    if (v == Globals.decompErrorName) v = null;
                    if (v == Globals.decompIgnoreName) v = null;
                    if (v != null && this.decompFind.decompOptions2.mergeNewVariables != null && this.decompFind.decompOptions2.mergeNewVariables.Contains(v, StringComparer.OrdinalIgnoreCase))
                    {
                        c.backgroundColor = Globals.decompBlueColor;
                    }

                    AddCell(g, i - 1 - offsetRow, j - 1 - offsetCol, s, leftAlign, type, c.backgroundColor, variablesAreOnRows, decompOutput.red, decompOptions.decompOperator);
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
                this.windowDecompStatusBar.Text = Globals.windowDecompStatusBarText2;  //because sums are not meaningful anyway
                DefaultGrid(this.grid1);
                this.canvasBorder.BorderBrush = Brushes.Black;
                this.canvasBorder.BorderThickness = new Thickness(3);
                this.decompFind.decompOptions2.guiDecompIsSelectingAll = true;
             
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

            if (this.decompFind.decompOptions2.guiDecompIsSelectingAll)
            {
                r0 = 1;
                r1 = this.decompFind.decompOptions2.guiDecompValues.GetRowMaxNumber();
                c0 = 1;
                c1 = this.decompFind.decompOptions2.guiDecompValues.GetColMaxNumber();
            }
            else
            {
                CoordConversion(out r0, out c0, type, this.decompFind.decompOptions2.guiDecompSelectedRowMin, this.decompFind.decompOptions2.guiDecompSelectedColMin);
                CoordConversion(out r1, out c1, type, this.decompFind.decompOptions2.guiDecompSelectedRowMax, this.decompFind.decompOptions2.guiDecompSelectedColMax);
            }

            //Cannot just copy what is seen on the screen if it is a number -- in that case decimals would get lost in Excel.
            
            for (int i = r0; i <= r1; i++)
            {
                for (int j = c0; j <= c1; j++)
                {
                    int x = i; int y = j;                    

                    Cell c = this.decompFind.decompOptions2.guiDecompValues.Get(x, y);
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
                s += "\r\n";
            }
            Clipboard.SetText(s, TextDataFormat.Text);            
        }

        private void AddCell(Grid g, int i, int j, string s, bool leftAlign, GekkoTableTypes type, string backgroundColor, Decomp.ERowsCols isRowOrCol, List<double> red, DecompOperator decompOperator)
        {

            GekkoDockPanel2 dockPanel = new GekkoDockPanel2();
            int w = Globals.guiTableCellWidth;
            if (type == GekkoTableTypes.UpperLeft || type == GekkoTableTypes.Left)
            {
                w = Globals.guiTableCellWidthFirst;
            }
            dockPanel.Width = w;
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
                if ((isRowOrCol == Decomp.ERowsCols.Rows && type == GekkoTableTypes.Left) || (isRowOrCol == Decomp.ERowsCols.Cols && type == GekkoTableTypes.Top))
                {

                    //
                    // LINKS etc.
                    // TODO: This is hacky. Better to look at adjacent cell content (like what happens when link is actually clicked)
                    //       
                    //
                    //                    

                    //TODO: offsets...
                    //2 below because the row or col labels all start in coord (0, 0), and guiDecompValues is 1-based. So first coord will be (2, 2).
                    Cell c = this.decompFind.decompOptions2.guiDecompValues.Get(i + 2, j + 2);
                    string v = c.vars_hack?[0];
                    if (v == Globals.decompErrorName) v = null;
                    if (v == Globals.decompIgnoreName) v = null;

                    bool isEndogenous = false;
                    if (v != null)
                    {
                        if (!Program.IsDecompResidualName(v))
                        {
                            if (decompFind.model.DecompType() == EModelType.GAMSScalar)
                            {
                                isEndogenous = true;
                            }
                            else if (decompFind.model.DecompType() == EModelType.GAMSRaw)
                            {
                                if (Program.HasGamsEquation(v)) isEndogenous = true;
                            }
                            if (decompFind.model.DecompType() == EModelType.Gekko)
                            {
                                EEndoOrExo e = Program.VariableTypeEndoExo(v);
                                isEndogenous = e == EEndoOrExo.Endo;
                            }
                            else
                            {
                                //strange...
                            }
                        }
                    }

                    if (isEndogenous || s == Globals.decompText0)
                    {
                        textBlock.MouseEnter += Mouse_Enter;
                        textBlock.MouseLeave += Mouse_Leave;
                        textBlock.MouseDown += Mouse_Down;
                        textBlock.Foreground = new SolidColorBrush(Globals.MediumBlueDecompLink);
                        if (isRowOrCol == Decomp.ERowsCols.Rows)
                        {
                            if (i == 0) textBlock.FontWeight = FontWeights.Bold;
                        }
                        else if (isRowOrCol == Decomp.ERowsCols.Cols)
                        {
                            if (j == 0) textBlock.FontWeight = FontWeights.Bold;
                        }
                        else
                        {
                            //no boldness, since there are no chosen vars.
                        }
                    }
                }

                textBlock.HorizontalAlignment = HorizontalAlignment.Right;
                if (leftAlign) textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.FontFamily = Globals.decompFontFamily;
                textBlock.FontSize = Globals.decompFontSize;
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

                if (backgroundColor == Globals.decompBlueColor)
                {
                    //overrides                
                    dockPanel.originalBackgroundColor = new SolidColorBrush(G.Lighter(Globals.GekkoModeBlue, 0.80));
                    dockPanel.Background = dockPanel.originalBackgroundColor;
                }
                else if (backgroundColor == Globals.decompResidualColor)
                {
                    //overrides                
                    dockPanel.originalBackgroundColor = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.80));
                    dockPanel.Background = dockPanel.originalBackgroundColor;
                }
                else if (backgroundColor == Globals.decompErrorColor)
                {
                    //overrides                                    
                    dockPanel.originalBackgroundColor = new SolidColorBrush(Globals.LightRed);
                    dockPanel.Background = dockPanel.originalBackgroundColor;
                }
                else if (backgroundColor == Globals.decompIgnoredColor)
                {
                    //overrides                                    
                    dockPanel.originalBackgroundColor = new SolidColorBrush(G.Lighter(Colors.LightGreen, 0.85));
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

            bool b1 = !decompOperator.isRaw;
            bool b2 = decompFind.decompOptions2.count == ECountType.None;
            bool b3 = (isRowOrCol == Decomp.ERowsCols.Rows && type == GekkoTableTypes.Top) || (isRowOrCol == Decomp.ERowsCols.Cols && type == GekkoTableTypes.Left);            
            
            if (b1 && b2 && b3 && Decomp.VarsAndTimeDimensionsAreSeparate(decompFind.decompOptions2))
            {
                //to do red lamp, there must be both vars and time, and they must be on separate row/col.
                SetRedCircle(g, i, j, type, isRowOrCol, red, decompFind.decompOptions2);
            }
        }


        //public static double delete = 0.15;

        private static void SetRedCircle(Grid g, int i, int j, GekkoTableTypes type, Decomp.ERowsCols isRowOrCol, List<double> errorValues, DecompOptions2 decompOptions2)
        {
            int ij = 0;
            if (isRowOrCol == Decomp.ERowsCols.Rows && type == GekkoTableTypes.Top) ij = j;
            else if (isRowOrCol == Decomp.ERowsCols.Cols && type == GekkoTableTypes.Left) ij = i;

            SolidColorBrush brush = new SolidColorBrush();
            double d = 0;
            if (errorValues != null)
            {
                d = Math.Abs((double)errorValues[ij]);

                if (Globals.runningOnTTComputer)
                {
                    //the method is probably never called with .isRaw==true, but never mind.
                    if (!decompOptions2.decompOperator.isRaw && decompOptions2.showErrors)
                    {
                        if (d > 0.001d)
                        {
                            MessageBox.Show("TTH: Error regarding red circles: value = " + errorValues[ij]);
                        }
                    }
                }

                if (d > 1d) d = 1d;

                //d = delete;                

                if (d <= Globals.redThresholds[0]) { /* do nothing */ }
                else if (d > Globals.redThresholds[0] && d <= Globals.redThresholds[1]) brush.Color = Globals.yellow;
                else if (d > Globals.redThresholds[1] && d <= Globals.redThresholds[2]) brush.Color = Globals.orange;
                else if (d > Globals.redThresholds[2]) brush.Color = Globals.red;

                //delete += 0.20;
            }

            Ellipse r = new Ellipse();
            r.Width = 9;
            r.Height = 9;
            r.Fill = brush;
            r.HorizontalAlignment = HorizontalAlignment.Right;
            if (d > Globals.redThresholds[0])
            {
                //border
                r.Stroke = new SolidColorBrush(Colors.Gray);
                r.StrokeThickness = 1;
            }

            DockPanel dp = new DockPanel();
            dp.Width = 15; dp.Height = 15;
            dp.Margin = new Thickness(0, 0, 6, 0);
            dp.SetValue(Grid.ColumnProperty, j);
            dp.SetValue(Grid.RowProperty, i);
            dp.Children.Add(r);
            dp.HorizontalAlignment = HorizontalAlignment.Right;
            string xx = "row";
            if (isRowOrCol == Decomp.ERowsCols.Cols) xx = "col";
            dp.ToolTip = "The relative difference between the value of " + xx + " #1 and the " + Environment.NewLine + "sum of the rest of the " + xx + "s is = " + (errorValues[ij] * 100d).ToString("0.00") + "%" + Environment.NewLine + "Try to click the 'Errors' checkbox.\nThe colors are yellow " + (100 * Globals.redThresholds[0]) + "-" + (100 * Globals.redThresholds[1]) + "%, orange " + (100 * Globals.redThresholds[1]) + "-" + (100 * Globals.redThresholds[2]) + "%, red > " + (100 * Globals.redThresholds[2]) + "%.";
            g.Children.Add(dp);
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
            if (!this.decompFind.decompOptions2.guiDecompIsSelecting) return;            
            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);
            Select(g, col, row);
            this.decompFind.decompOptions2.guiDecompIsSelecting = false;
            this.decompFind.decompOptions2.guiDecompIsSelectingAll = false;
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

            bool flowChart = false;
            if (flowChart)
            {                
                DecompOptions2 decompOptions = this.decompFind.decompOptions2;
                string code = "sp";                
                TextBlock textBlock = (TextBlock)border.Child;
                string s2 = textBlock.Text.Trim();
                string variable = "y";
                if (MessageBox.Show("Flowchart?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Program.FlowChart(variable, code, GekkoTime.FromStringToGekkoTime(s2));
                    }
                    catch (Exception err)
                    {
                        new Error("Flowchart failed");
                    }
                }
                else
                {
                    // Do not close the window
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
                this.decompFind.decompOptions2.guiDecompIsSelecting = true;
                DefaultGrid(g);
        
                border.BorderThickness = new Thickness(3);
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 0);
                border.BorderBrush = mySolidColorBrush;

                this.decompFind.decompOptions2.guiDecompLastClickedRow = row;  //normal click
                this.decompFind.decompOptions2.guiDecompLastClickedCol = col;
                this.decompFind.decompOptions2.guiDecompChangedCells.Add(row + "," + col, 0);

                TextBlock textBlock = (TextBlock)border.Child;
                textBlock.Padding = new Thickness(0, 0, 1, 0);          
            }            
        }

        private void Select(Grid g, int col, int row)
        {
            DefaultGrid(g);
            this.decompFind.decompOptions2.guiDecompSelectedRowMin = Math.Min(this.decompFind.decompOptions2.guiDecompLastClickedRow, row);
            this.decompFind.decompOptions2.guiDecompSelectedRowMax = Math.Max(this.decompFind.decompOptions2.guiDecompLastClickedRow, row);
            this.decompFind.decompOptions2.guiDecompSelectedColMin = Math.Min(this.decompFind.decompOptions2.guiDecompLastClickedCol, col);
            this.decompFind.decompOptions2.guiDecompSelectedColMax = Math.Max(this.decompFind.decompOptions2.guiDecompLastClickedCol, col);
            double sum = 0d;
            int count = 0;
            foreach (GekkoDockPanel2 d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                //b2.BorderBrush = null;
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (this.decompFind.decompOptions2.guiDecompSelectedRowMin <= row2 && row2 <= this.decompFind.decompOptions2.guiDecompSelectedRowMax)
                {
                    if (this.decompFind.decompOptions2.guiDecompSelectedColMin <= col2 && col2 <= this.decompFind.decompOptions2.guiDecompSelectedColMax)
                    {

                        if (!this.decompFind.decompOptions2.guiDecompChangedCells.ContainsKey(row2 + "," + col2))
                        {
                            this.decompFind.decompOptions2.guiDecompChangedCells.Add(row2 + "," + col2, 0);
                        }                        
                        
                        SetDefaultBorder(b2);                        

                        SetBorderThickness(g, row2, col2, b2);            

                        d2.Background = Brushes.White;

                        double left = 0.15;
                        double right = 0;
                        double top = 0.15;
                        double bottom = 0;
                        if (this.decompFind.decompOptions2.guiDecompSelectedRowMin == row2) top = 3;
                        if (this.decompFind.decompOptions2.guiDecompSelectedRowMax == row2) bottom = 3;
                        if (this.decompFind.decompOptions2.guiDecompSelectedColMin == col2) left = 3;
                        if (this.decompFind.decompOptions2.guiDecompSelectedColMax == col2) right = 3;

                        b2.BorderBrush = Brushes.Black;
                        b2.BorderThickness = new Thickness(left, top, right, bottom);

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = Globals.GrayExcelSelect;
                        //border.BorderBrush = mySolidColorBrush;


                        if (row2 == this.decompFind.decompOptions2.guiDecompLastClickedRow && col2 == this.decompFind.decompOptions2.guiDecompLastClickedCol)
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
                        Cell c = this.decompFind.decompOptions2.guiDecompValues.Get(x, y);
                        
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
            //TextBlock text = _statusText;
            //text.FontFamily = new FontFamily("Calibri");
            //text.FontSize = 13d;
            this.windowDecompStatusBar.Text = "Sum: " + sum + "   Count: " + count + "   Avg: " + (sum / (double)count);
        }

        private void DefaultGrid(Grid g)
        {            
            foreach (GekkoDockPanel2 d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (!this.decompFind.decompOptions2.guiDecompChangedCells.ContainsKey(row2 + "," + col2)) continue;
                if (d2.originalBackgroundColor == null) d2.Background = Brushes.White;
                else d2.Background = d2.originalBackgroundColor;                
                SetDefaultBorder(b2);                   
                SetBorderThickness(g, row2, col2, b2);                
            }
            this.decompFind.decompOptions2.guiDecompChangedCells.Clear();
        }

        private void Cell_Leave(object sender, MouseEventArgs e)
        {
            GekkoDockPanel2 dockPanel = (GekkoDockPanel2)sender;            

            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);

            if (this.decompFind.decompOptions2.guiDecompIsSelecting)
            {            
            }
            else
            {
                this.windowDecompStatusBar.Text = "";

                int x; int y;
                CoordConversion(out x, out y, dockPanel.type, row, col);
                Cell c = this.decompFind.decompOptions2.guiDecompValues.Get(x, y);
                string s = null;
                if (this.decompFind.model.DecompType() == EModelType.GAMSScalar)
                {
                    s = Model.GetEquationTextHelper(this.decompFind.decompOptions2.link, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t1, this.decompFind.model);
                }
                else
                {
                    s = Model.GetEquationTextFoldedNonScalar(this.decompFind.model.DecompType(), this.decompFind.decompOptions2.link);
                }
                RichSetText(equation, Decomp.GetColoredEquations(s));
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

        private void Mouse_Down(object sender, MouseButtonEventArgs e) // MouseEventArgs e)
        {
            //#98732498724    
            //if (e.ClickCount != 2) return;

            TextBlock tb = (TextBlock)sender;
            DockPanel dp = G.FindParent<DockPanel>(tb);

            int col = (int)dp.GetValue(Grid.ColumnProperty);
            int row = (int)dp.GetValue(Grid.RowProperty);

            Cell c, c2;
            GetTwoCells(row, col, out c, out c2);

            if (c != null && c.cellType == CellType.Text)
            {
                // ---------------------------------------
                // FIND
                // ---------------------------------------

                string var = HiddenVariableHelper(c2);
                if (var == null)
                {                    
                    new Error(Decomp.Text1(1));
                }

                if (e.ClickCount == 1)
                {
                    _activeVariable = var;
                    return;
                }

                O.Find o = new O.Find(this.decompFind);
                List m = new List(new List<string>() { var });
                o.iv = m;
                o.Exe();
            }
            else
            {
                new Error("Unexpected link error");
            }
        }

        /// <summary>
        /// Used for links in table (clickable variales).
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="c"></param>
        /// <param name="c2"></param>
        private void GetTwoCells(int row, int col, out Cell c, out Cell c2)
        {
            int x = -12345;
            int y = -12345;
            c = null;
            c2 = null;
            if (Decomp.VariablesOnRowsOrCols(this.decompFind.decompOptions2) == Decomp.ERowsCols.Rows)
            {
                CoordConversion(out x, out y, GekkoTableTypes.Left, row, col);
                c = this.decompFind.decompOptions2.guiDecompValues.Get(x, y);
                c2 = this.decompFind.decompOptions2.guiDecompValues.Get(x, y + 1); //#7098asfuydasfd
            }
            else if (Decomp.VariablesOnRowsOrCols(this.decompFind.decompOptions2) == Decomp.ERowsCols.Cols)
            {
                CoordConversion(out x, out y, GekkoTableTypes.Top, row, col);
                c = this.decompFind.decompOptions2.guiDecompValues.Get(x, y);
                c2 = this.decompFind.decompOptions2.guiDecompValues.Get(x + 1, y); //#7098asfuydasfd
            }
            else
            {
                //there are no links.
                //do nothing, return null
            }
        }

        private static string HiddenVariableHelper(Cell c2)
        {
            if (c2 == null) return null;
            List<string> vars = c2.vars_hack;
            if (vars == null)
            {
                return null;
            }
            string var = vars[0];  //#dskla8asjkdfa
            int lag; string name;
            Decomp.ConvertFromTurtleName(var, false, out name, out lag);
            return name;
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

            if (type == GekkoTableTypes.TableContent && this.decompFind.decompOptions2.guiDecompIsSelecting)
            {
                //ScrollViewer p5 = G.FindParent<ScrollViewer>(g);                
                //double x1 = p5.ContentVerticalOffset;
                //double x2 = p5.ContentHorizontalOffset;
                //double x3 = p5.ViewportWidth;
                //double x4 = p5.ViewportHeight;
                Select(g, col, row);
            }
            else
            {
                Decomp.ERowsCols rowOrCol = Decomp.VariablesOnRowsOrCols(decompFind.decompOptions2);

                Cell c = null;
                Cell c2 = null;

                c = this.decompFind.decompOptions2.guiDecompValues.Get(x, y);
                if (rowOrCol == Decomp.ERowsCols.Rows)
                {
                    c2 = this.decompFind.decompOptions2.guiDecompValues.Get(x, y + 1); //#7098asfuydasfd                
                }
                else if (rowOrCol == Decomp.ERowsCols.Cols)
                {
                    c2 = this.decompFind.decompOptions2.guiDecompValues.Get(x + 1, y); //#7098asfuydasfd                
                }
                else
                {
                    //do nothing
                }

                if ((rowOrCol == Decomp.ERowsCols.Rows && dockPanel.type == GekkoTableTypes.Left) || (rowOrCol == Decomp.ERowsCols.Cols && dockPanel.type == GekkoTableTypes.Top))
                {
                    if (c != null)
                    {
                        if (c.cellType == CellType.Text)
                        {
                            string var = c.CellText.TextData[0];
                            string var2 = G.ExtractOnlyVariableIgnoreLag(var, Globals.leftParenthesisIndicator);

                            if (G.Equal(var2, Globals.decompText0))
                            {
                                if (this.decompFind.decompOptions2.expressionOld != null)
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This value corresponds to evaluating the expression."));
                                }
                                else
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This value corresponds to evaluating the right-hand side of the equation."));
                                }
                            }
                            else if (G.Equal(var2, Globals.decompText1))
                            {
                                if (this.decompFind.decompOptions2.expressionOld != null)
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This difference is always 0 for expressions."));
                                }
                                else
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This difference is the databank value minus the result of evaluating the right-hand side of the equation."));
                                }
                            }
                            else if (G.Equal(var2, Globals.decompText1a))  //raw
                            {
                                if (this.decompFind.decompOptions2.expressionOld != null)
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This difference between the two rows above is always 0 for expressions."));
                                }
                                else
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This difference is the databank value minus the result of evaluating the right-hand side of the equation."));
                                }

                            }
                            else if (G.Equal(var2, Globals.decompText2))
                            {
                                if (this.decompFind.decompOptions2.expressionOld != null)
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This value is the result of evaluating the expression minus the sum of decomposed contributions." + G.NL + "If the equation is linear, this number is very small (in principle: zero)."));
                                }
                                else
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("This value is the result of evaluating the right-hand side of the equation minus the sum of decomposed contributions." + G.NL + "If the equation is linear, this number is very small (in principle: zero)."));
                                }
                            }
                            else if (G.Equal(var2, Globals.decompText2a))  //raw
                            {
                                if (this.decompFind.decompOptions2.expressionOld != null)
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("These values correspond to evaluating the expression (always equal to the row above for expressions)."));
                                }
                                else
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("These values correspond to evaluating the right-hand side of the equation."));
                                }
                            }
                            else
                            {
                                this.windowDecompStatusBar.Text = Globals.windowDecompStatusBarText;
                                string var7 = HiddenVariableHelper(c2);

                                int number = -12345;
                                try { number = int.Parse(var7.Substring(Globals.decompResidualName.Length)); } catch { };
                                if (var7 == Globals.decompResidualName) number = 0;
                                //"Residual" --> number = 0
                                //"Residual1" --> number = 1
                                //"Residual2" --> number = 2
                                //"Residual0" --> number = -12345
                                //"Residualsomething" --> number = -12345

                                if (var7 == null)
                                {
                                    RichSetText(equation, Decomp.GetColoredEquations("--> " + Decomp.Text1(1)));
                                }
                                else
                                {
                                    if (var7 == Globals.decompErrorName)
                                    {
                                        RichSetText(equation, Decomp.GetColoredEquations("Errors originating from possible non-linearities in the equation (for a linear equation, these errors are = 0). When variables are shown on rows, the error value is computed so that the first row equals the sum of the rest of the rows."));
                                    }
                                    else if (var7 == Globals.decompIgnoreName)
                                    {                                        
                                        RichSetText(equation, Decomp.GetColoredEquations("Ignored contributions (" + this.textBlockIgnore.Text + "), cf. the 'Ignore' option."));
                                    }
                                    else if (var7.StartsWith(Globals.decompResidualName) && number >= 0)
                                    {
                                        string more = "";
                                        if (number > 0) more = " #" + number;
                                        RichSetText(equation, Decomp.GetColoredEquations("Data residual in equation" + more + " (difference between left-hand and right-hand side). The data residual should normally be = 0 for simulated values."));
                                    }
                                    else
                                    {
                                        List<string> ss = Program.GetVariableExplanation(G.Chop_RemoveFreq(var7), var7, true, true, this.decompFind.decompOptions2.t1, this.decompFind.decompOptions2.t2, null);
                                        string txt = Stringlist.ExtractTextFromLines(ss).ToString() + Program.SetBlanks();
                                        RichSetText(equation, Decomp.GetColoredEquations(txt));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string s = null;
                    if (this.decompFind.model.DecompType() == EModelType.GAMSScalar)
                    {
                        s = Model.GetEquationTextHelper(this.decompFind.decompOptions2.link, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t1, this.decompFind.model);
                    }
                    else
                    {
                        s = Model.GetEquationTextFoldedNonScalar(this.decompFind.model.DecompType(), this.decompFind.decompOptions2.link);
                    }
                    RichSetText(equation, Decomp.GetColoredEquations(s));
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

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        //private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key.ToString() == "Escape")
        //    {
        //        this.Close();
        //    }
        //}

        /// <summary>
        /// When something in the Gekko decomp GUI is changed (for instance, a field is dragged), this method is called.
        /// </summary>
        /// <param name="refresh"></param>
        public void RecalcCellsWithNewType(Model model)
        {
            if (false)
            {
                //See #f8kd8sfdgksldgjf
                //this will put green lights one after one. But impossible to make it appear
                //at one, it always waits for recalc to finish, all sorts of threads tried.
                BitmapImage bi = new BitmapImage(new Uri(System.Windows.Forms.Application.StartupPath + "\\images\\green.png"));
                System.Windows.Controls.Image i3 = new System.Windows.Controls.Image();
                i3.Height = 12;
                i3.Margin = new Thickness(0, 0, 25, 0);
                i3.Source = bi;
                //this.colors.Children.Add(i3);
                //can remove child[0] if > 1 children, when this works.
            }
            
            this.Cursor = Cursors.Wait;

            int remember = Globals.guiTableCellWidth;
            try
            {
                if (this.decompFind.decompOptions2.expression == null)
                {
                    if (equation == null) return;  //Happens during first rendering, when isChecked is set by C# on top-left radio-button (ignore it)
                }
                if (this.decompFind.decompOptions2.count == ECountType.Names) Globals.guiTableCellWidth = 3 * remember;                
                RecalcCellsWithNewTypeHelper(model);
                return;
            }
            catch (Exception e)
            {
                if (G.IsUnitTesting()) throw;                
                if (this.decompFind.decompOptions2Previous != null)
                {
                    this.decompFind.decompOptions2 = this.decompFind.decompOptions2Previous;
                    RecalcCellsWithNewTypeHelper(model);
                    RefreshRowsColsFiltersList();
                    this.isInitializing = true;  //so we don't get a recalc here because of setting radio buttons
                    this.SetRadioButtons();  //revert buttons
                    this.isInitializing = false;
                }
            }
            finally
            {
                Globals.guiTableCellWidth = remember;                                
                this.Cursor = Cursors.Arrow;
                this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone(true);
            }
        }

        private void RecalcCellsWithNewTypeHelper(Model model)
        {
            this.decompFind.decompOptions2.code = this.decompFind.decompOptions2.ToCode();
            SetRadioButtonsDefaults();

            if (!this.isInitializing) this.windowDecompStatusBar.Text = "";

            GekkoTime per1 = this.decompFind.decompOptions2.t1;
            GekkoTime per2 = this.decompFind.decompOptions2.t2;
            GekkoSmpl smpl = new GekkoSmpl(per1, per2);

            DecompOutput decompOutput = Decomp.DecompMain(smpl, per1, per2, this.decompFind.decompOptions2, ref this.decompDatas, model);
            textBlockIgnore.Text = decompOutput.ignore;

            string s = null;            
            if (this.decompFind.model.DecompType() == EModelType.GAMSScalar)
            {                
                s = Model.GetEquationTextHelper(this.decompFind.decompOptions2.link, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t1, model);                
            }
            else
            {
                s = Model.GetEquationTextFoldedNonScalar(this.decompFind.model.DecompType(), this.decompFind.decompOptions2.link);
            }            
            RichSetText(equation, Decomp.GetColoredEquations(s));
            RichSetText(this.code, this.decompFind.decompOptions2.code.AddTemporarily(Program.SetBlanks()));

            this.decompFind.decompOptions2.guiDecompValues = decompOutput.table;

            if (decompFind.decompOptions2.plot)
            {

                if (!Decomp.VarsAndTimeDimensionsAreSeparate(this.decompFind.decompOptions2))
                {
                    new Error("Cannot show this as a plot, because variables and time are not both selected and on different rows/cols.");
                }
                Decomp.ERowsCols variablesAreOnRows = Decomp.VariablesOnRowsOrCols(this.decompFind.decompOptions2);
                if (variablesAreOnRows == Decomp.ERowsCols.Rows && decompFind.decompOptions2.cols.Count > 1)
                {
                    new Error("Cannot show this as a plot, because the time field is not the sole field on columns.");
                }
                if (variablesAreOnRows == Decomp.ERowsCols.Cols && decompFind.decompOptions2.rows.Count > 1)
                {
                    new Error("Cannot show this as a plot, because the time field is not the sole field on rows.");
                }

                webBrowser.Visibility = Visibility.Visible;
                scrollView1.Visibility = Visibility.Collapsed;

                string fileName = "gekko.svg"; //just to signal the file type

                O.Prt o = new O.Prt();
                o.prtType = "plot";
                o.t1 = per1;
                o.t2 = per2;
                o.opt_filename = fileName;
        
                List<O.Prt.Element> container = new List<O.Prt.Element>();                

                PlotTable plotTable = new PlotTable();
                plotTable.dates = new List<List<double>>();
                plotTable.values = new List<List<double>>();
                
                if (variablesAreOnRows == Decomp.ERowsCols.Rows)
                {
                    for (int i = 2; i <= decompOutput.table.GetRowMaxNumber(); i++)
                    {
                    
                        Cell cName = decompOutput.table.Get(i, 1);
                        string name = cName.CellText.TextData[0];
                        List<double> dates = new List<double>();
                        List<double> values = new List<double>();
                        for (int j = 2; j <= decompOutput.table.GetColMaxNumber(); j++)
                        {
                            Cell cDate = decompOutput.table.Get(1, j);
                            Cell c = decompOutput.table.Get(i, j);                            
                            GekkoTime date = cDate.date_hack;
                            dates.Add(Program.PlotTableTime(date.freq, date));
                            values.Add(c.number);                            
                        }
                        plotTable.dates.Add(dates);
                        plotTable.values.Add(values);
                        O.Prt.Element element = new O.Prt.Element();
                        element.labelOLD = new List<string>() { name };
                        if (i == 2) element.linewidth = 6;  //double
                        container.Add(element);
                    }
                }
                else
                {
                    for (int j = 2; j <= decompOutput.table.GetColMaxNumber(); j++)
                    {
                        Cell cName = decompOutput.table.Get(1, j);
                        string name = cName.CellText.TextData[0];
                        List<double> dates = new List<double>();
                        List<double> values = new List<double>();
                        for (int i = 2; i <= decompOutput.table.GetRowMaxNumber(); i++)
                        {
                            Cell cDate = decompOutput.table.Get(i, 1);
                            Cell c = decompOutput.table.Get(i, j);
                            GekkoTime date = cDate.date_hack;
                            dates.Add(Program.PlotTableTime(date.freq, date));
                            values.Add(c.number);
                        }
                        plotTable.dates.Add(dates);
                        plotTable.values.Add(values);
                        O.Prt.Element element = new O.Prt.Element();
                        element.labelOLD = new List<string>() { name };
                        if (j == 2) element.linewidth = 6;  //double
                        container.Add(element);
                    }
                }

                //note, maybe just take name from o object?                

                PlotHelper plotHelper1 = new PlotHelper();
                plotHelper1.isDecompPlot = true;
                string svgFile1 = Plot.CallGnuplot(plotTable, o, container, model.modelCommon.GetFreq(), plotHelper1, smpl.p);

                if (Globals.decompPlotFix)
                {
                    //A bit hacky, but we count the gnuplot key (legend) cols by looking into the svg file
                    //Below, we first find "gnuplot_plot_1". Then we get 121796.6 from "translate(121796.6,45.0)".
                    //The numbers like 121796.6 are looked at, and each different number represents a col.
                    /*
                    	<g id="gnuplot_plot_1" ><title>fy | [0]   </title>
                        <g fill="none" color="white" stroke="black" stroke-width="4.00" stroke-linecap="butt" stroke-linejoin="miter">
                        </g>
                        <g fill="none" color="black" stroke="currentColor" stroke-width="4.00" stroke-linecap="butt" stroke-linejoin="miter">
	                    <g transform="translate(121796.6,45.0)" stroke="none" fill="black" font-family="Verdana" font-size="17.48"  text-anchor="start">
		                <text xml:space="preserve"><tspan font-family="Verdana"  xml:space="preserve">fy | [0]   </tspan></text>
	                    
                    */

                    string svg = Program.GetTextFromFileWithWait(svgFile1);
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    int start = 0;
                    for (int i = 1; i < int.MaxValue; i++)
                    {
                        string id = "gnuplot_plot_" + i;
                        int idx1 = svg.IndexOf(id, start);
                        if (idx1 == -1) break;
                        int idx2 = svg.IndexOf("translate(", idx1);
                        if (idx2 == -1) break;
                        int idx3 = svg.IndexOf(",", idx2);
                        if (idx3 == -1) break;
                        string number = G.Substring(svg, idx2 + "translate(".Length, idx3 - 1).Trim();
                        if (!dict.ContainsKey(number)) dict.Add(number, null);
                        start = idx3;  //no need to start all over                        
                    }                    

                    PlotHelper plotHelper2 = new PlotHelper();
                    plotHelper2.isDecompPlot = true;
                    plotHelper2.decompPlotCallNumber = 1;
                    plotHelper2.decompPlotNumberOfKeyColumns = dict.Count;  //better than guesstimating
                    string svgFile2 = Plot.CallGnuplot(plotTable, o, container, model.modelCommon.GetFreq(), plotHelper2, smpl.p);
                    webBrowser.Source = new Uri(svgFile2);
                }
                else
                {
                    webBrowser.Source = new Uri(svgFile1);
                }

                
            }
            else
            {                
                webBrowser.Visibility = Visibility.Collapsed;
                scrollView1.Visibility = Visibility.Visible;

                if (G.IsUnitTesting() && Globals.showDecompTable == false)
                {
                    Globals.lastDecompTable = decompOutput.table;
                }
                else
                {
                    string more = null;
                    if (this.decompFind.decompOptions2.new_from.Count > 1) more = " (+" + (this.decompFind.decompOptions2.new_from.Count - 1) + " more)";
                    if (this.decompFind.window != null) (this.decompFind.window as WindowDecomp).Title = this.decompFind.decompOptions2.new_from[0] + more + " - Gekko decomp";
                    ClearGrid();
                    MakeGuiTable2(decompOutput, this.decompFind.decompOptions2);
                }
            }
        }
        
        private void SetRadioButtonsDefaults()
        {
            if (true)
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

                checkBoxShares.IsEnabled = true;
                checkBoxShares.Opacity = 1.0;

                checkRef.IsEnabled = true;
                checkRef.Opacity = 1.0;

                checkBoxDyn.IsEnabled = true;
                checkBoxDyn.Opacity = 1.0;

                checkBoxCount.IsEnabled = true;
                checkBoxCount.Opacity = 1.0;

                checkMZero.IsEnabled = true;
                checkMZero.Opacity = 1.0;

                checkBoxErrors.IsEnabled = true;
                checkBoxErrors.Opacity = 1.0;

                checkBoxSort.IsEnabled = true;
                checkBoxSort.Opacity = 1.0;

                txtNum.IsEnabled = true;
                txtNum.Opacity = 1.0;
                cmdUp.IsEnabled = true;
                cmdUp.Opacity = 1.0;
                cmdDown.IsEnabled = true;
                cmdDown.Opacity = 1.0;
            }

            if (this.decompFind.decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.OnlyRef)
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

            if (this.decompFind.decompOptions2.isShares)
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

            if (this.decompFind.decompOptions2.decompOperator.isRaw)
            {
                checkBoxShares.IsEnabled = false;  //shares
                checkBoxShares.Opacity = 0.5;
            }
            
            if (this.decompFind.decompOptions2.decompOperator.lowLevel == Decomp.ELowLevel.Multiplier)
            {
                checkRef.IsEnabled = false;  //baseline, not meaningful for multiplier types
                checkRef.Opacity = 0.5;
            }            
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

        private void MakeGuiTable2(DecompOutput decompOutput, DecompOptions2 decompOptions)
        {
            CreateGridRowsAndColumns(this.gridUpperLeft, decompOutput, GekkoTableTypes.UpperLeft);
            PutTableIntoGrid2(this.gridUpperLeft, decompOutput, GekkoTableTypes.UpperLeft, decompOptions);
            CreateGridRowsAndColumns(this.grid1Top, decompOutput, GekkoTableTypes.Top);
            PutTableIntoGrid2(this.grid1Top, decompOutput, GekkoTableTypes.Top, decompOptions);
            CreateGridRowsAndColumns(this.grid1Left, decompOutput, GekkoTableTypes.Left);
            PutTableIntoGrid2(this.grid1Left, decompOutput, GekkoTableTypes.Left, decompOptions);
            CreateGridRowsAndColumns(this.grid1, decompOutput, GekkoTableTypes.TableContent);
            PutTableIntoGrid2(this.grid1, decompOutput, GekkoTableTypes.TableContent, decompOptions);
        }

        private string FindEquationText(DecompOptions decompOptions)
        {
            if (decompOptions.expressionOld != null)
            {
                return decompOptions.expressionOld;
            }
            else
            {
                EquationHelper eh = Program.FindEquationByMeansOfVariableName(this.decompFind.decompOptions2.variable);
                if (eh == null) return ""; //probably only when model is changed while UDVALG window is open (this is illegal anyway, and a popup will appear)
                else return ((EquationHelper)eh).equationText;
            }
        }        

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                //string r = null; if(this.decompFind.decompOptions2.decompOperator.operatorLower.Contains(StartsWith("r", StringComparison.OrdinalIgnoreCase)
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("xrn");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("xn");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("xrd");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("xd");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton6_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("rd");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("d");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("xrp");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("xp");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton8_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("rp");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("p");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton9_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("xrdp");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("xdp");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton10_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("rdp");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("dp");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton21_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isReference) this.decompFind.decompOptions2.decompOperator = new DecompOperator("xrn");
                else this.decompFind.decompOptions2.decompOperator = new DecompOperator("xn");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton22_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.decompOperator = new DecompOperator("xm");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton26_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.decompOperator = new DecompOperator("m");
                RecalcCellsWithNewType(decompFind.model);
            }
        }        

        private void radioButton24_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.decompOperator = new DecompOperator("xq");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton28_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.decompOperator = new DecompOperator("q");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton29_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.decompOperator = new DecompOperator("xmp");
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void radioButton30_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.decompOperator = new DecompOperator("mp");
                RecalcCellsWithNewType(decompFind.model);
            }
        }
        

        private void CheckBoxShares_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.isShares = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxShares_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.isShares = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void checkBoxMZero_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                MessageBox.Show(MissingProblemText());
                this.decompFind.decompOptions2.missingAsZero = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private static string MissingProblemText()
        {
            return "Option <missing=zero> does not work yet. It must be determined what\nthe option really means (missing input data represented as 0, missing decomp results represented as 0, ...?).";
        }

        private void checkBoxMZero_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {                
                this.decompFind.decompOptions2.missingAsZero = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                string xx = this.decompFind.decompOptions2.decompOperator.OperatorLower();
                if (xx.StartsWith("x")) xx = "xr" + xx.Substring(1);
                else xx = "r" + xx;
                this.decompFind.decompOptions2.decompOperator = new DecompOperator(xx);
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                string xx = this.decompFind.decompOptions2.decompOperator.OperatorLower();
                if (xx.StartsWith("xr")) xx = "x" + xx.Substring(2);
                else if (xx.StartsWith("r")) xx = xx.Substring(1);
                this.decompFind.decompOptions2.decompOperator = new DecompOperator(xx);
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.guiDecompWindowTopDistance = Math.Max(1, (int)this.Top);
            Globals.guiDecompWindowLeftDistance = Math.Max(1, (int)this.Left);
            Globals.guiDecompWindowHeightDistance = Math.Max(1, (int)this.ActualHeight);
            Globals.guiDecompWindowWidthDistance = Math.Max(1, (int)this.ActualWidth);
            Globals.guiDecompWindowSplitterHorizontal = Math.Max(1, (int)this.splitterHorizontal.Width.Value);
            Globals.guiDecompWindowSplitterVertical = Math.Max(1, (int)this.splitterVertical.Height.Value);

            this.decompFind.closed = true;

            try
            {
                if (Globals.windowsDecomp2 != null && this != null) Globals.windowsDecomp2.Remove(this);                
            }
            catch (Exception e2)
            {
            }
        }        
        
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            UpdateDecomp();
        }

        public void UpdateDecomp()
        {
            if (!this.isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.localBanks = null;  //clearing this, forcing window to use vales from Gekko databanks
                this.RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isPercentageType || this.decompFind.decompOptions2.isShares)
                {
                    this.decompFind.decompOptions2.decimalsPch++;
                }
                else
                {
                    this.decompFind.decompOptions2.decimalsLevel++;
                }
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                if (this.decompFind.decompOptions2.decompOperator.isPercentageType || this.decompFind.decompOptions2.isShares)
                {
                    this.decompFind.decompOptions2.decimalsPch--;
                    if (this.decompFind.decompOptions2.decimalsPch < 0) this.decompFind.decompOptions2.decimalsPch = 0;
                }
                else
                {
                    this.decompFind.decompOptions2.decimalsLevel--;
                    if (this.decompFind.decompOptions2.decimalsLevel < 0) this.decompFind.decompOptions2.decimalsLevel = 0;
                }
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        

        private void ListButton1_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            GekkoTask task = button.DataContext as GekkoTask;            
            new Error("Add new row item");            
        }

        private void ListButton2_Click(object sender, RoutedEventArgs e)
        {
            //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
            ToggleButton button = sender as ToggleButton;
            GekkoTask task = button.DataContext as GekkoTask;
            string s = G.HandleInternalIdentifyer2(task.Pivot_Text);            
            if (task.Pivot_TaskType == TaskType.Rows)
            {
                //decompOptions2.rows.Remove(s);
                RemoveFromObservableCollection(task);
            }
            else if (task.Pivot_TaskType == TaskType.Cols)
            {
                //decompOptions2.cols.Remove(s);
                RemoveFromObservableCollection(task);
            }
            else
            {                
                RemoveFromObservableCollection(task);
            }
            PutGuiPivotSelectionIntoDecompOptions(taskList);
            RefreshList2(task.Pivot_TaskType);
            RecalcCellsWithNewType(decompFind.model);            
        }

        private void RemoveFromObservableCollection(GekkoTask task)
        {
            List<GekkoTask> m = new List<GekkoTask>();
            int i = 0;
            foreach (GekkoTask t in taskList)
            {
                if (task.I == t.I) continue;
                m.Add(t);
                t.I = i++;
            }
            taskList.Clear();
            foreach (GekkoTask t in m) taskList.Add(t);
        }

        //private void checkBoxErrors2_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (!isInitializing)
        //    {
        //        this.decompFind.decompOptions2.count = ECountType.Names;
        //        RecalcCellsWithNewType(false, decompFind.model);
        //    }
        //}

        //private void checkBoxErrors2_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (!isInitializing)
        //    {
        //        this.decompFind.decompOptions2.count = ECountType.None;
        //        RecalcCellsWithNewType(false, decompFind.model);
        //    }
        //}

        private void buttonMerge_Click(object sender, RoutedEventArgs e)
        {
            //
            // Merge button
            //
            DecompFind dfParentFind = this.decompFind.SearchUpwards(EDecompFindNavigation.Find);
            DecompFind dfParentDecomp = this.decompFind.SearchUpwards(EDecompFindNavigation.Decomp);            

            if (dfParentFind == null || dfParentDecomp == null) return;
            if (dfParentDecomp.closed)
            {
                new Error("Merge not possible, because the preceding DECOMP window has been closed");
                return;
            }
            WindowFind windowFindParent = dfParentFind.window as WindowFind;
            WindowDecomp windowDecompParent = dfParentDecomp.window as WindowDecomp;           

            //Close the child decomp window (= present window)
            this.Close();

            //Merge the child window (= present window) into the parent window.
            //We have to use dispatcher on the parent decomp window, else it will complain that it is the wrong thread.            
            windowDecompParent.Dispatcher.Invoke(() => { Merge(dfParentDecomp); });
        }

        /// <summary>
        /// Adds RichText after first clearing.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="input"></param>
        public static void RichSetText(RichTextBox text, Rich input)
        {
            text.Document.Blocks.Clear();
            RichAddText(text, input);
        }

        /// <summary>
        /// Adds a Rich object to a RichTextBox, possibly colored.
        /// </summary>
        /// <param name="myRichTextBox"></param>
        /// <param name="input"></param>
        public static void RichAddText(RichTextBox text, Rich input)
        {
            Paragraph paragraph = new Paragraph();
            foreach (StringAndColor sc in input.Get())
            {
                Run run = new Run(sc.s) { Foreground = new SolidColorBrush(sc.color) };
                paragraph.Inlines.Add(run);
                //if (sc.color != Colors.Black) rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                //else rangeOfText1.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
            text.Document.Blocks.Add(paragraph);
        }

        private void Merge(DecompFind dfParentDecomp)
        {
            //dfParentDecomp.decompOptions2Previous = dfParentDecomp.decompOptions2.Clone();
            DecompOptions2 remember = dfParentDecomp.decompOptions2.Clone();
            
            List<string> thisFrom = this.decompFind.decompOptions2.new_from;
            List<string> thisEndo = this.decompFind.decompOptions2.new_endo;
            dfParentDecomp.decompOptions2.new_from.AddRange(thisFrom);
            dfParentDecomp.decompOptions2.new_endo.AddRange(thisEndo);
            //
            //
            // HACK HACK HACK --> move .decompDatas inside .decompFind maybe
            //
            //

            WindowDecomp windowParentDecomp = dfParentDecomp.window as WindowDecomp;
            
            List<string> varsParent = GetDecompedVariables(windowParentDecomp.decompDatas, dfParentDecomp.decompOptions2);
            List<string> varsThis = GetDecompedVariables(this.decompDatas, this.decompFind.decompOptions2);
            List<string> varsNew = varsThis.Except(varsParent).ToList();
            var temp = varsNew.OrderBy(x => x, new G.NaturalComparer(G.NaturalComparerOptions.Default));
            varsNew = new List<string>(); varsNew.AddRange(temp);
            List<string> varsNew2 = new List<string>();
            foreach (string s in varsNew)
            {
                //TODO: there must be a method for this...
                varsNew2.Add(s.Replace(Decomp.DecompFirst() + ":", "").Replace("¤[0]", ""));  //keep the ¤ for lags
            }
            dfParentDecomp.decompOptions2.mergeNewVariables = varsNew2;
            windowParentDecomp.Activate();  //nice that this is near top so it gets focused fast, and the user can see the table change live.            

            //can be up to around 30 chars with GUI looking too bad...
            string variable = this.decompFind.decompOptions2.new_select[0];
            string txt = dfParentDecomp.decompOptions2.mergeNewVariables.Count + " var" + G.S(dfParentDecomp.decompOptions2.mergeNewVariables.Count) + " replaced" + Environment.NewLine + G.Chop_RemoveIndex(variable) + ". [";            
            windowParentDecomp.textMerge.Visibility = Visibility.Visible;            
            windowParentDecomp.textMerge.Inlines.Clear();
            windowParentDecomp.textMerge.Inlines.Add(txt);
            Hyperlink hyperLink = new Hyperlink()
            {
                NavigateUri = new Uri("http://www.t-t.dk/gekko")
            };
            hyperLink.Inlines.Add("ok");
            hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
            windowParentDecomp.textMerge.Inlines.Add(hyperLink);
            windowParentDecomp.textMerge.Inlines.Add("]");
            //windowParentDecomp.textMerge.Foreground = new SolidColorBrush(Colors.Gray);
            windowParentDecomp.textMerge.ToolTip = "Replaced variable " + variable + " with " + dfParentDecomp.decompOptions2.mergeNewVariables.Count + " new variable" + G.S(dfParentDecomp.decompOptions2.mergeNewVariables.Count) + Environment.NewLine + " (blue-colored). Click 'ok' to remove coloring.";

            //windowParentDecomp.buttonMergeHide.Visibility = Visibility.Visible;

            //TODO
            //TODO
            //TODO Put the list in a tooltip, and make a note "Last merge: 'c' removed, 2 new vars added shown in green" [OK].
            //TODO When doing GUI, look 1 col to the right and see if vars_hack contains somehing from varsNew.
            //TODO if so, mark the cell name green.
            //TODO Ungreen when [OK] is clicked. Any window that is merged TO must first be ungreened.


            windowParentDecomp.decompDatas = new DecompDatas();  //clearing it, otherwise we get problems

            dfParentDecomp.decompOptions2.code = dfParentDecomp.decompOptions2.ToCode();

            //RichSetText(windowParentDecomp.code, dfParentDecomp.decompOptions2.code.AddTemporarily(Program.SetBlanks()));

            try
            {
                Decomp.DecompGetFuncExpressionsAndRecalc(dfParentDecomp, windowParentDecomp);
            }
            catch
            {
                //reverting, so that the parent window may be ok to continue with
                dfParentDecomp.decompOptions2 = remember;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            //HACK HACK HACK
            //For some really annoying reason, CrossThreadStuff.MergeButtonOk() does not work
            //So we walk upwards, which is brittle. Better perhaps to do a walker method, is
            //this does not already exist...
            //HACK HACK HACK

            TextBlock x1 = ((Hyperlink)sender).Parent as TextBlock;
            WindowDecomp x5 = G.FindParent<WindowDecomp>(x1);
            x1.Text = "";
            x5.decompFind.decompOptions2.mergeNewVariables = null;
            x5.RecalcCellsWithNewType(x5.decompFind.model);            
        }

        private List<string> GetDecompedVariables(DecompDatas decompDatas, DecompOptions2 decompOptions2)
        {
            List<string> vars = new List<string>();
            DecompDict dd = Decomp.GetDecompDatas(decompDatas.MAIN_data, decompOptions2.decompOperator.type);
            foreach (KeyValuePair<string, Series> kvp in dd.storage)
            {
                vars.Add(kvp.Key);
            }
            return vars;
        }

        private void checkBoxCount_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                checkBoxNames.Unchecked -= CheckBoxNames_Unchecked;
                checkBoxNames.IsChecked = false;  //so it does not fire
                checkBoxNames.Unchecked += CheckBoxNames_Unchecked;
                this.decompFind.decompOptions2.count = ECountType.N;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxCount_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.count = ECountType.None;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxNames_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                checkBoxCount.Unchecked -= CheckBoxCount_Unchecked;
                checkBoxCount.IsChecked = false;  //so it does not fire
                checkBoxCount.Unchecked += CheckBoxCount_Unchecked;                
                this.decompFind.decompOptions2.count = ECountType.Names;
                RecalcCellsWithNewType(decompFind.model);                
            }
        }

        private void CheckBoxNames_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                checkBoxCount.Unchecked -= CheckBoxCount_Unchecked;
                checkBoxCount.IsChecked = false;  //so it does not fire
                checkBoxCount.Unchecked += CheckBoxCount_Unchecked;
                checkBoxCount.IsChecked = false;

                this.decompFind.decompOptions2.count = ECountType.None;
                //this.decompFind.decompOptions2.decompOperator.isShares = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxSort_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.decompOptions2.sort = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxSort_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.decompOptions2.sort = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxErrors_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {                
                this.decompFind.decompOptions2.showErrors = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxErrors_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                //this.decompFind.decompOptions2Previous = this.decompFind.decompOptions2.Clone();
                this.decompFind.decompOptions2.showErrors = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxDyn_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {                
                this.decompFind.decompOptions2.dyn = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxDyn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {                
                this.decompFind.decompOptions2.dyn = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxPlot_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.decompOptions2.plot = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void CheckBoxPlot_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.decompOptions2.plot = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void checkBoxExpand_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                MessageBox.Show("Expand is intended to expand all cells with Count > 1. Work in progress, not implemented yet...");
                this.decompFind.decompOptions2.expand = true;
                RecalcCellsWithNewType(decompFind.model);
            }
        }

        private void checkBoxExpand_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.decompOptions2.expand = false;
                RecalcCellsWithNewType(decompFind.model);
            }
        }
    }

    public class GekkoDockPanel2 : DockPanel
    {
        public WindowDecomp.GekkoTableTypes type = WindowDecomp.GekkoTableTypes.Unknown;
        public Brush originalBackgroundColor = null;
    }         

    public enum ECountType
    {
        None,
        N,
        Names
    }

    public class DecompOptions2
    {
        //remember Clone()        

        //--------------------------------------------------------------- 
        //----- These GUI elements are controllable from Gekko syntax -------- cf. #8yuads79afyghr in DecompOperator
        //--------------------------------------------------------------- 
        public GekkoTime t1 = GekkoTime.tNull;
        public GekkoTime t2 = GekkoTime.tNull;
        public DecompOperator decompOperator = null;
        public ECountType count = ECountType.None;
        public bool showErrors = false;
        public bool isShares = false;
        public int decimalsLevel = 4;  //TODO: make selectable from syntax
        public int decimalsPch = 2;  //TODO: make selectable from syntax
        public bool dyn = false;
        public bool missingAsZero = false;
        public bool sort = false;
        public double ignore = double.NaN;  //between 0 and 100.
        public bool plot = false;
        public bool expand = false;
        public List<string> new_select = null;
        public List<string> new_from = null;
        public List<string> new_endo = null;
        public List<string> rows = new List<string>();
        public List<string> cols = new List<string>();
        //--------------------------------------------------------------- 
                
        public List iv = null;
        public GekkoTime tSelected = GekkoTime.tNull;
        public bool showTime = false;
        public Rich code = null;
        public bool ageHierarchy = false;        
        public bool isNew = false;
        public int numberOfRecalcs = 0;  //is not cloned --> used to pause main thread until the DECOMP window has calculated.
        public string variable = null;        
        public string expressionOld = null;  //only != null for expressions
        public Func<GekkoSmpl, IVariable> expression = null;        
        public string type;  //not used yet (UDVALG or DECOMP)        
        public List<string> subst = new List<string>();
        public IVariable name = null;  //only active for names like x, x[a] and the like, not for expressions   
        public Data dataPattern = null;
        public string dream = null;  //experimental
        public List<Link> link = new List<Link>();
        public List<List<string>> where = new List<List<string>>();
        public List<List<string>> group = new List<List<string>>();
        //internal stuff for Rows/Colums
        public List<string> all = new List<string>();
        public ObservableCollection<string> free = new ObservableCollection<string>();
        public List<GekkoDictionary<string, string>> freeValues = new List<GekkoDictionary<string, string>>();
        public ObservableCollection<string> freeFilter = new ObservableCollection<string>();
        public List<FrameFilter> filters = new List<FrameFilter>();
        public List<string> mergeNewVariables = null;  //do clone for this

        //-------- No clone for this ----------------
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
        public LocalBanks localBanks = null;
        public string modelHash = null;
        //-------- GUI stuff end ----------------        
        
        /// <summary>
        /// Obtain the object as code like "decomp &lt;2010 2020> x from e1 endo x;"
        /// </summary>
        /// <returns></returns>
        public Rich ToCode()
        {
            Color color = Colors.Blue;
            Rich s = new Rich();
            s.Add("decomp", color);
            s.Add(" <", color);
            s.Add(this.t1.ToString() + " " + this.t2.ToString());
            s.Add(" " + decompOperator.OperatorLower());
            if (this.isShares) s.Add(" shares");
            if (this.count == ECountType.N) s.Add(" count");
            if (this.count == ECountType.Names) s.Add(" names");
            if (this.showErrors) s.Add(" errors");
            if (this.dyn) s.Add(" dyn");
            if (this.missingAsZero) s.Add(" missing=zero");
            if (this.sort) s.Add(" sort");
            if (!double.IsNaN(this.ignore) && ignore > 0d && ignore <= 100d) s.Add(" ignore=" + this.ignore);
            if (this.expand) s.Add(" expand");
            if (this.plot) s.Add(" plot");
            s.Add(">", color);
            s.Add(" " + Stringlist.GetListWithCommas(this.new_select));
            s.Add(" from", color);
            s.Add(" " + Stringlist.GetListWithCommas(this.new_from));
            s.Add(" endo", color);
            s.Add(" " + Stringlist.GetListWithCommas(this.new_endo));
            
            if (this.rows.Count > 0)
            {
                List<string> temp = new List<string>();
                foreach (string x in this.rows)
                {
                    temp.Add(G.HandleInternalIdentifyer1(x));
                }
                s.Add(" rows", color);
                s.Add(" " + Stringlist.GetListWithCommas(temp));
            }
            if (this.cols.Count > 0)
            {
                List<string> temp = new List<string>();
                foreach (string x in this.cols)
                {
                    temp.Add(G.HandleInternalIdentifyer1(x));
                }
                s.Add(" cols", color);
                s.Add(" " + Stringlist.GetListWithCommas(temp));
            }

            s.Add(";");
            return s;
        }

        /// <summary>
        /// Clone, including Rows/Columns selection
        /// </summary>
        /// <returns></returns>
        public DecompOptions2 Clone()
        {
            return Clone(true);
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="includeRowsColumnsFilters"></param>
        /// <returns></returns>
        public DecompOptions2 Clone(bool includeRowsColumnsFilters)
        {
            //clones relevant parts for new window
            DecompOptions2 d = new DecompOptions2();

            if (this.decompOperator != null) d.decompOperator = this.decompOperator.Clone();
                        
            d.decimalsLevel = this.decimalsLevel;
            d.decimalsPch = this.decimalsPch;
            d.showErrors = this.showErrors;

            if (this.code != null) d.code = this.code.Clone();
            
            //d.modelType = this.modelType;

            d.ageHierarchy = this.ageHierarchy;

            //d.tp = this.tp;
            d.variable = this.variable;
            d.t1 = this.t1;
            d.t2 = this.t2;
            
            d.dyn = this.dyn;
            d.count = this.count;
            d.missingAsZero = this.missingAsZero;
            d.isShares = this.isShares;
            d.sort = this.sort;
            d.plot = this.plot;
            d.expand = this.expand;
            d.ignore = this.ignore;
            
            d.modelHash = this.modelHash;
            d.type = this.type;

            //d.decimalsLevel = this.decimalsLevel;

            d.dream = this.dream;

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
                temp.expressions = x1.expressions; //shallow copy
                d.link.Add(temp);
            }

            if (includeRowsColumnsFilters)
            {

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

                d.all.AddRange(this.all);
                foreach (string s in this.free)
                {
                    d.free.Add(s);
                }
                foreach (GekkoDictionary<string, string> x in this.freeValues)
                {
                    GekkoDictionary<string, string> xx = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (KeyValuePair<string, string> kvp in x)
                    {
                        xx.Add(kvp.Key, kvp.Value);
                    }
                    d.freeValues.Add(xx);
                }
                foreach (string s in this.freeFilter)
                {
                    d.freeFilter.Add(s);
                }
                foreach (FrameFilter ff in this.filters)
                {
                    FrameFilter ff2 = new FrameFilter();
                    ff2.active = ff.active;
                    ff2.name = ff.name;
                    ff2.selected.AddRange(ff.selected);
                    d.filters.Add(ff2);
                }
            }
            else
            {
                Decomp.ResetRowsColsSelection(d);
            }

            if (this.new_select != null)
            {
                List<string> tempSelect = new List<string>();
                foreach (string s in this.new_select)
                {
                    tempSelect.Add(s);
                }
                d.new_select = tempSelect;
            }

            if (this.new_from != null)
            {
                List<string> tempFrom = new List<string>();
                foreach (string s in this.new_from)
                {
                    tempFrom.Add(s);
                }
                d.new_from = tempFrom;
            }

            if (this.new_endo != null)
            {
                List<string> tempEndo = new List<string>();
                foreach (string s in this.new_endo)
                {
                    tempEndo.Add(s);
                }
                d.new_endo = tempEndo;
            }
            
            if (this.dataPattern != null)
            {
                d.dataPattern = new Data();
                d.dataPattern.dataCellsGradQuo = (Series)this.dataPattern.dataCellsGradQuo.DeepClone(null);
                d.dataPattern.dataCellsGradRef = (Series)this.dataPattern.dataCellsGradRef.DeepClone(null);
            }

            return d;
        }
    }

}
