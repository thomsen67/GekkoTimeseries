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

        public DecompFindNavigation decompFind = null;

        public DecompDatas decompDatas = new DecompDatas(); //stores data for reuse, for instance for fast pivot selection

        public int frozenRows=0;
        public int frozenCols=0;
        
        public bool isClosing = false;
        public bool isInitializing = false; //a bit hacky, to handle radiobutton1 firing a clicked event when initializing
        
        public Grid _grid = null;

        public StatusBar _status = null;
        public TextBlock _statusText = null;
        private ObservableCollection<GekkoTask> _list = new ObservableCollection<GekkoTask>();
        ListViewDragDropManager<GekkoTask> dragMgr;
        
        public string uglyHack_name = null;

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

        public FrameLight frame;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {            
            Button cmb = sender as Button;            
            GekkoTask task = cmb.DataContext as GekkoTask;

            bool isTree = false;
            if (task.Pivot_TaskType == TaskType.Filters) isTree = true;            

            if (isTree)
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

                //foreach (FrameFilter ff in decompOptions2.filters)
                //{
                //    if (G.Equal(G.HandleInternalIdentifyer1(ff.name), task.Pivot_Text))
                //    {
                //        selectedOld = ff.selected;
                //        bool equal = true;
                //        foreach (string s in selectedOld)
                //        {
                //            if (!selected.Contains(s, StringComparer.OrdinalIgnoreCase))
                //            {
                //                equal = false;
                //                break;
                //            }
                //        }
                //        foreach (string s in selected)
                //        {
                //            if (!selectedOld.Contains(s, StringComparer.OrdinalIgnoreCase))
                //            {
                //                equal = false;
                //                break;
                //            }
                //        }
                //        if (equal == false)
                //        {
                //            ff.selected = selected;
                //            RecalcCellsWithNewType();
                //        }
                //        break;
                //    }
                //}    

                //foreach (FrameFilter ff in decompOptions2.filters)
                {
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

                            //RefreshList2(TaskType.Filters);
                            //RefreshList();
                            //RecalcCellsWithNewType();


                            //foreach (Task t in m) taskList.Add(t);
                            PutGuiPivotSelectionIntoDecompOptions(taskList);                            
                            RecalcCellsWithNewType(false);


                        }                        
                    }
                }
            }
            else
            {
                WindowDecompSortEtc w = new WindowDecompSortEtc();
                w.ShowDialog();                
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
                MessageBox.Show("*** ERROR");
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
                        filters = GetAllPossibleValuesForListFilter(chosen, this.decompFind.GetDecompOptions());  //we need to set the chosen values to all values (so the filter has no effect to begin with)
                    }
                    m.Add(new GekkoTask(chosen, "Transparent", "Visible", "Collapsed", "Visible", "Normal", type, i2++, null, filters, this.decompFind.GetDecompOptions()));
                }                
                m.Add(t);
                t.I = i2++;                
            }
            taskList.Clear();
            foreach (GekkoTask t in m) taskList.Add(t);
            PutGuiPivotSelectionIntoDecompOptions(taskList);
            RefreshList2(type);
            RecalcCellsWithNewType(false);
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

            if (Globals.isAgeHierarchy && name.EndsWith(Globals.ageHierarchyName))
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

        private void RefreshList()
        {
            RefreshList2(TaskType.None);

            taskList.Clear();
            int i = 0;
            taskList.Add(new GekkoTask(Globals.internalPivotRows, Globals.internalPivotRowColor, "Collapsed", "Visible", "Hidden", "Bold", TaskType.None, i++, this.decompFind.GetDecompOptions().free, null, this.decompFind.GetDecompOptions()));
            foreach (string s in this.decompFind.GetDecompOptions().rows)
            {
                taskList.Add(new GekkoTask(s, "Transparent", "Visible", "Collapsed", "Visible", "Normal", TaskType.Rows, i++, null, null, this.decompFind.GetDecompOptions()));
            }
            //taskList[taskList.Count - 1].LineColor = "Black";

            taskList.Add(new GekkoTask(Globals.internalPivotCols, Globals.internalPivotRowColor, "Collapsed", "Visible", "Hidden", "Bold", TaskType.None, i++, this.decompFind.GetDecompOptions().free, null, this.decompFind.GetDecompOptions()));
            foreach (string s in this.decompFind.GetDecompOptions().cols)
            {
                taskList.Add(new GekkoTask(s, "Transparent", "Visible", "Collapsed", "Visible", "Normal", TaskType.Cols, i++, null, null, this.decompFind.GetDecompOptions()));
            }
            //taskList[taskList.Count - 1].LineColor = "Black";

            taskList.Add(new GekkoTask(Globals.internalPivotFilters, Globals.internalPivotRowColor, "Collapsed", "Visible", "Hidden", "Bold", TaskType.None, i++, this.decompFind.GetDecompOptions().freeFilter, null, this.decompFind.GetDecompOptions()));
            foreach (FrameFilter ff in this.decompFind.GetDecompOptions().filters)
            {
                taskList.Add(new GekkoTask(G.HandleInternalIdentifyer1(ff.name), "Transparent", "Visible", "Collapsed", "Visible", "Normal", TaskType.Filters, i++, null, ff.selected, this.decompFind.GetDecompOptions()));
            }
            //taskList[taskList.Count - 1].LineColor = "Black";

            taskList.Add(new GekkoTask("", "Transparent", "Collapsed", "Collapsed", "Collapsed", "Normal", TaskType.Invisible, i++, null, null, this.decompFind.GetDecompOptions()));
            

            for (int i2 = 0; i2 < taskList.Count; i2++)
            {
                taskList[i2].Pivot_Text = G.HandleInternalIdentifyer1(taskList[i2].Pivot_Text);
            }            
        }

        private void RefreshList2(TaskType taskType)
        {
            List<string> fields = frame.colnames;
            this.decompFind.GetDecompOptions().all.Clear();
            foreach (string s in frame.colnames)
            {
                string s2 = G.HandleInternalIdentifyer1(s);
                //if (s2 == "value") continue;  //no need to show value, cannot be selected anyway. Could implement count at some point.
                this.decompFind.GetDecompOptions().all.Add(s2);
            }

            this.decompFind.GetDecompOptions().freeFilter.Clear();            

            foreach (string s in this.decompFind.GetDecompOptions().all)
            {
                bool isFilter = false;
                foreach (FrameFilter ff in this.decompFind.GetDecompOptions().filters)
                {
                    if (G.Equal(ff.name, G.HandleInternalIdentifyer2(s)))
                    {
                        isFilter = true;
                        break;
                    }
                }
                if (!isFilter)
                {
                    this.decompFind.GetDecompOptions().freeFilter.Add(G.HandleInternalIdentifyer1(s));
                }
            }

            this.decompFind.GetDecompOptions().free.Clear();
            foreach (string s in this.decompFind.GetDecompOptions().all)
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

                if (this.decompFind.GetDecompOptions().rows.Contains(G.HandleInternalIdentifyer2(s)) || this.decompFind.GetDecompOptions().cols.Contains(G.HandleInternalIdentifyer2(s)))
                {
                }
                else
                {
                    this.decompFind.GetDecompOptions().free.Add(G.HandleInternalIdentifyer1(s));
                }
            }


            //if (taskType == TaskType.Filters)
            //{
            //    decompOptions2.freeFilter.Clear();
            //    foreach (string s in decompOptions2.all)
            //    {
            //        if (s != "t")
            //        {
            //        }
            //        else
            //        {
            //            decompOptions2.freeFilter.Add(G.HandleInternalIdentifyer1(s));
            //        }
            //    }
            //}
            //else
            //{
            //    decompOptions2.free.Clear();
            //    foreach (string s in decompOptions2.all)
            //    {
            //        if (this.decompFind.GetDecompOptions().rows.Contains(G.HandleInternalIdentifyer2(s)) || this.decompFind.GetDecompOptions().cols.Contains(G.HandleInternalIdentifyer2(s)))
            //        {
            //        }
            //        else
            //        {
            //            decompOptions2.free.Add(G.HandleInternalIdentifyer1(s));
            //        }
            //    }
            //}
        }

        void WindowDecomp_Loaded(object sender, RoutedEventArgs e)
        {
            // Give the ListView an ObservableCollection of Task
            // as a data source.  Note, the ListViewDragManager MUST
            // be bound to an ObservableCollection, where the collection's
            // type parameter matches the ListViewDragManager's type
            // parameter (in this case, both have a type parameter of Task).

            taskList = new ObservableCollection<GekkoTask>();
            RefreshList();

            this.listView.ItemsSource = taskList;

            // This is all that you need to do, in order to use the ListViewDragManager.
            this.dragMgr = new ListViewDragDropManager<GekkoTask>(this.listView);
            this.dragMgr.ListView = this.listView;
            this.dragMgr.ShowDragAdorner = true;
            this.dragMgr.DragAdornerOpacity = 0.5d;  //so that e.g. "Work" can still be seen underneath
            this.listView.ItemContainerStyle = this.FindResource("ItemContStyle") as Style;

            this.dragMgr.ProcessDrop += DragAndDrop;

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

            GekkoTask task = e.Data.GetData(typeof(GekkoTask)) as GekkoTask;
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


        void DragAndDrop(object sender, ProcessDropEventArgs<GekkoTask> e)
        {
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
                    if (type == TaskType.None) throw new GekkoException(); //check can be removed at some point

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
            RecalcCellsWithNewType(false);            
        }

        private void PutGuiPivotSelectionIntoDecompOptions(ObservableCollection<GekkoTask> collection)
        {
            this.decompFind.GetDecompOptions().rows.Clear();
            this.decompFind.GetDecompOptions().cols.Clear();
            this.decompFind.GetDecompOptions().filters.Clear();
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
                if (state == 1 && task.Pivot_TaskType == TaskType.Rows) this.decompFind.GetDecompOptions().rows.Add(G.HandleInternalIdentifyer2(task.Pivot_Text));
                else if (state == 2 && task.Pivot_TaskType == TaskType.Cols) this.decompFind.GetDecompOptions().cols.Add(G.HandleInternalIdentifyer2(task.Pivot_Text));
                else if (state == 3 && task.Pivot_TaskType == TaskType.Filters)
                {
                    FrameFilter ff = new FrameFilter();
                    ff.name = G.HandleInternalIdentifyer2(task.Pivot_Text);
                    ff.selected = new List<string>(); foreach (string s in task.pivot_filterSelected) ff.selected.Add(s);
                    this.decompFind.GetDecompOptions().filters.Add(ff);
                }
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listView.SelectedItem = null;  --> no, then we cannot move the row
        }        

        public void SetRadioButtons() {
            if (this.decompFind.GetDecompOptions().isSubWindow)
            {
                if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw)
                {
                    if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "n")
                    {
                        radioButton1.IsChecked = true;
                    }
                    else if(this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "d")
                    {
                        radioButton2.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "p")
                    {
                        radioButton4.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "dp")
                    {
                        radioButton9.IsChecked = true;
                    }
                    //else if (this.decompOptions.guiDecompOperator == "n")
                    //{
                    //    radioButton21.IsChecked = true;
                    //}
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "m")
                    {
                        radioButton22.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "q")
                    {
                        radioButton24.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "mp")
                    {
                        radioButton29.IsChecked = true;
                    }
                }
                else {
                    if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "n")
                    {
                        radioButton5.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "d")
                    {
                        radioButton6.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "p")
                    {
                        radioButton8.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "dp")
                    {
                        radioButton10.IsChecked = true;
                    }


                    //else if (this.decompOptions.guiDecompOperator == "n")
                    //{
                    //    radioButton25.IsChecked = true;
                    //}
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "m")
                    {
                        radioButton26.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "q")
                    {
                        radioButton28.IsChecked = true;
                    }
                    else if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator == "mp")
                    {
                        radioButton30.IsChecked = true;
                    }
                }

                if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsShares)
                {
                    checkBox1.IsChecked = true;
                }

                if (this.decompFind.GetDecompOptions().decompTablesFormat.showErrors)
                {
                    checkBoxErrors.IsChecked = true;
                }
                
                if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRef)
                {
                    checkBox2.IsChecked = true;
                }
            }
            else
            {
                //All these buttons are in the "Decomp" columns, codes are from e.g. UDVALG<p> or UDVALG<q> calls from command lines/files.
                if (G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "d"))
                {
                    radioButton6.IsChecked = true;
                }
                if (G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "p"))
                {
                    radioButton8.IsChecked = true;
                }
                if (G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "m"))
                {
                    radioButton26.IsChecked = true;
                }
                if (G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "q"))
                {
                    radioButton28.IsChecked = true;
                }
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

        public WindowDecomp(DecompOptions2 decompOptions)
        {

            if (this.decompFind == null) this.decompFind = new DecompFindNavigation(decompOptions);
            this.isInitializing = true; //so that radiobuttons etc do not fire right now
            InitializeComponent();
            this.isInitializing = false;  //ready for clicking

            this.buttonSelect.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, Globals.LightBlueWord.R, Globals.LightBlueWord.G, Globals.LightBlueWord.B));

            DataContext = new ViewModel();  //MVVM style

            Canvas.SetTop(this.frezenBorder, Globals.guiTableCellHeight);
            Canvas.SetLeft(this.frezenBorder2,Globals.guiTableCellWidth);
            this.gridUpperLeft.Width = Globals.guiTableCellWidth;
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

            this.KeyDown += new KeyEventHandler(Window_KeyDown); //new System.Windows.Forms.KeyEventHandler(this.radioButton1_KeyDown);

            this.decompFind.GetDecompOptions().guiDecompChangedCells.Clear();
            this.decompFind.GetDecompOptions().guiDecompIsSelecting = false;
            this.decompFind.GetDecompOptions().guiDecompIsSelectingAll = false;

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
                this.decompFind.GetDecompOptions().guiDecompIsSelectingAll = true;
             
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

            if (this.decompFind.GetDecompOptions().guiDecompIsSelectingAll)
            {
                r0 = 1;
                r1 = this.decompFind.GetDecompOptions().guiDecompValues.GetRowMaxNumber();
                c0 = 1;
                c1 = this.decompFind.GetDecompOptions().guiDecompValues.GetColMaxNumber();
            }
            else
            {
                CoordConversion(out r0, out c0, type, this.decompFind.GetDecompOptions().guiDecompSelectedRowMin, this.decompFind.GetDecompOptions().guiDecompSelectedColMin);
                CoordConversion(out r1, out c1, type, this.decompFind.GetDecompOptions().guiDecompSelectedRowMax, this.decompFind.GetDecompOptions().guiDecompSelectedColMax);
            }

            //Cannot just copy what is seen on the screen if it is a number -- in that case decimals would get lost in Excel.
            
            for (int i = r0; i <= r1; i++)
            {
                for (int j = c0; j <= c1; j++)
                {
                    int x = i; int y = j;                    

                    Cell c = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y);
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
            if (!this.decompFind.GetDecompOptions().guiDecompIsSelecting) return;            
            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);
            Select(g, col, row);
            this.decompFind.GetDecompOptions().guiDecompIsSelecting = false;
            this.decompFind.GetDecompOptions().guiDecompIsSelectingAll = false;
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
                this.decompFind.GetDecompOptions().guiDecompIsSelecting = true;
                DefaultGrid(g);
        
                border.BorderThickness = new Thickness(3);
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 0);
                border.BorderBrush = mySolidColorBrush;

                this.decompFind.GetDecompOptions().guiDecompLastClickedRow = row;  //normal click
                this.decompFind.GetDecompOptions().guiDecompLastClickedCol = col;
                this.decompFind.GetDecompOptions().guiDecompChangedCells.Add(row + "," + col, 0);

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
            this.decompFind.GetDecompOptions().guiDecompSelectedRowMin = Math.Min(this.decompFind.GetDecompOptions().guiDecompLastClickedRow, row);
            this.decompFind.GetDecompOptions().guiDecompSelectedRowMax = Math.Max(this.decompFind.GetDecompOptions().guiDecompLastClickedRow, row);
            this.decompFind.GetDecompOptions().guiDecompSelectedColMin = Math.Min(this.decompFind.GetDecompOptions().guiDecompLastClickedCol, col);
            this.decompFind.GetDecompOptions().guiDecompSelectedColMax = Math.Max(this.decompFind.GetDecompOptions().guiDecompLastClickedCol, col);
            double sum = 0d;
            int count = 0;
            foreach (GekkoDockPanel2 d2 in g.Children)
            {
                Border b2 = (Border)d2.Children[0];
                //b2.BorderBrush = null;
                int col2 = (int)d2.GetValue(Grid.ColumnProperty);
                int row2 = (int)d2.GetValue(Grid.RowProperty);
                if (this.decompFind.GetDecompOptions().guiDecompSelectedRowMin <= row2 && row2 <= this.decompFind.GetDecompOptions().guiDecompSelectedRowMax)
                {
                    if (this.decompFind.GetDecompOptions().guiDecompSelectedColMin <= col2 && col2 <= this.decompFind.GetDecompOptions().guiDecompSelectedColMax)
                    {

                        if (!this.decompFind.GetDecompOptions().guiDecompChangedCells.ContainsKey(row2 + "," + col2))
                        {
                            this.decompFind.GetDecompOptions().guiDecompChangedCells.Add(row2 + "," + col2, 0);
                        }                        
                        
                        SetDefaultBorder(b2);                        

                        SetBorderThickness(g, row2, col2, b2);            

                        d2.Background = Brushes.White;

                        double left = 0.15;
                        double right = 0;
                        double top = 0.15;
                        double bottom = 0;
                        if (this.decompFind.GetDecompOptions().guiDecompSelectedRowMin == row2) top = 3;
                        if (this.decompFind.GetDecompOptions().guiDecompSelectedRowMax == row2) bottom = 3;
                        if (this.decompFind.GetDecompOptions().guiDecompSelectedColMin == col2) left = 3;
                        if (this.decompFind.GetDecompOptions().guiDecompSelectedColMax == col2) right = 3;

                        b2.BorderBrush = Brushes.Black;
                        b2.BorderThickness = new Thickness(left, top, right, bottom);

                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = Globals.GrayExcelSelect;
                        //border.BorderBrush = mySolidColorBrush;


                        if (row2 == this.decompFind.GetDecompOptions().guiDecompLastClickedRow && col2 == this.decompFind.GetDecompOptions().guiDecompLastClickedCol)
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
                        Cell c = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y);
                        
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
                if (!this.decompFind.GetDecompOptions().guiDecompChangedCells.ContainsKey(row2 + "," + col2)) continue;
                if (d2.originalBackgroundColor == null) d2.Background = Brushes.White;
                else d2.Background = d2.originalBackgroundColor;                
                SetDefaultBorder(b2);                   
                SetBorderThickness(g, row2, col2, b2);                
            }
            this.decompFind.GetDecompOptions().guiDecompChangedCells.Clear();
        }

        private void Cell_Leave(object sender, MouseEventArgs e)
        {
            GekkoDockPanel2 dockPanel = (GekkoDockPanel2)sender;            

            Grid g = (Grid)dockPanel.Parent;
            int col = (int)dockPanel.GetValue(Grid.ColumnProperty);
            int row = (int)dockPanel.GetValue(Grid.RowProperty);

            if (this.decompFind.GetDecompOptions().guiDecompIsSelecting)
            {            
            }
            else
            {
                //G.Writeln("cell " + row + " " + col);
                //this.equation.Background = Brushes.LightYellow;
                //this.equation.Foreground = Brushes.White;

                int x; int y;
                CoordConversion(out x, out y, dockPanel.type, row, col);
                Cell c = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y);
                string s = null;
                if (this.decompFind.GetDecompOptions().modelType == EModelType.GAMSScalar)
                {
                    s = Model.GetEquationTextHelper(this.decompFind.GetDecompOptions().link, this.decompFind.GetDecompOptions().showTime, this.decompFind.GetDecompOptions().t1);
                }
                else
                {
                    s = Model.GetEquationTextFoldedNonScalar(this.decompFind.GetDecompOptions().modelType, this.decompFind.GetDecompOptions().link);
                }
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

            Cell c = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y);
            Cell c2 = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y + 1); //#7098asfuydasfd

            if (c != null && c.cellType == CellType.Text)
            {
                // ---------------------------------------
                // FIND
                // ---------------------------------------

                string var = HiddenVariableHelper(c2);
                DecompOptions2 opt = this.decompFind.GetDecompOptions().Clone();
                O.Find o = new O.Find(opt);
                List m = new List(new List<string>() { var });
                o.iv = m;
                o.Exe();               
            }
            else
            {
                new Error("Unexpected link error");
            }
        }

        private static string HiddenVariableHelper(Cell c2)
        {
            if (c2 == null) return null;
            List<string> vars = c2.vars_hack;
            if (vars == null) MessageBox.Show("Could not find any vars");
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

            if (type == GekkoTableTypes.TableContent && this.decompFind.GetDecompOptions().guiDecompIsSelecting)
            {
                Select(g, col, row);
            }
            else
            {
                Cell c = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y);
                Cell c2 = this.decompFind.GetDecompOptions().guiDecompValues.Get(x, y + 1); //#7098asfuydasfd                

                if (dockPanel.type == GekkoTableTypes.Left)
                {
                    if (c != null)
                    {
                        if (c.cellType == CellType.Text)
                        {
                            //this.equation.Background = Brushes.LightYellow;
                            //G.Writeln(c.CellText.TextData[0]);
                            string var = c.CellText.TextData[0];

                            string var2 = G.ExtractOnlyVariableIgnoreLag(var, Globals.leftParenthesisIndicator);

                            if (G.Equal(var2, Globals.decompText0))
                            {
                                if (this.decompFind.GetDecompOptions().expressionOld != null)
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
                                if (this.decompFind.GetDecompOptions().expressionOld != null)
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
                                if (this.decompFind.GetDecompOptions().expressionOld != null)
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
                                if (this.decompFind.GetDecompOptions().expressionOld != null)
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
                                if (this.decompFind.GetDecompOptions().expressionOld != null)
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
                                //List<string> vars = c2.vars_hack;
                                //if (vars == null) MessageBox.Show("Could not find any vars");                                
                                //string var7 = vars[0];  //#dskla8asjkdfa                                
                                string var7 = HiddenVariableHelper(c2);
                                List<string> ss = Program.GetVariableExplanation(G.Chop_RemoveFreq(var7), var7, true, true, this.decompFind.GetDecompOptions().t1, this.decompFind.GetDecompOptions().t2, null);
                                string txt = Stringlist.ExtractTextFromLines(ss).ToString();
                                this.equation.Text = txt;
                            }
                        }
                    }
                }
                else
                {
                    string s = null;
                    if (this.decompFind.GetDecompOptions().modelType == EModelType.GAMSScalar)
                    {
                        s = Model.GetEquationTextHelper(this.decompFind.GetDecompOptions().link, this.decompFind.GetDecompOptions().showTime, this.decompFind.GetDecompOptions().t1);
                    }
                    else
                    {
                        s = Model.GetEquationTextFoldedNonScalar(this.decompFind.GetDecompOptions().modelType, this.decompFind.GetDecompOptions().link);
                    }
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

        /// <summary>
        /// When something in the Gekko decomp GUI is changed (for instance, a field is dragged), this method is called.
        /// </summary>
        /// <param name="refresh"></param>
        public void RecalcCellsWithNewType(bool refresh)
        {
            try
            {
                if (this.decompFind.GetDecompOptions().expression == null)
                {
                    if (equation == null) return;  //Happens during first rendering, when isChecked is set by C# on top-left radio-button (ignore it)
                }

                //SetRadioButtonDefaults();

                //string transformationCodeAugmented = null;
                if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator != null)
                {
                    //There are 4 showing options: operator, isBaseline, isRaw, isShares.
                    string transformationCodeAugmented = this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator;
                    if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRef) transformationCodeAugmented = "r" + transformationCodeAugmented;
                    if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw) transformationCodeAugmented = "x" + transformationCodeAugmented;
                    if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsShares) transformationCodeAugmented = "s" + transformationCodeAugmented;  //is put on last
                    if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw && this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsShares)
                    {
                        new Error("Cannot show decomposition with both 'raw' and 'shares' option at the same time");
                    }
                    this.decompFind.GetDecompOptions().prtOptionLower = transformationCodeAugmented;
                }

                //"x" and "s" are mutually exclusive: in raw mode shares are not meaningful
                //so "sd", "sp", "sdp" + "sm", "sq", "smp" are used                

                SetRadioButtonsDefaults();

                _statusText.Text = "";

                GekkoTime per1 = this.decompFind.GetDecompOptions().t1;
                GekkoTime per2 = this.decompFind.GetDecompOptions().t2;                

                GekkoSmpl smpl = new GekkoSmpl(per1, per2);

                IVariable y0a = null;
                IVariable y0aRef = null;

                frame = new FrameLight();
                Table table = Decomp.DecompMain(smpl, per1, per2, this.decompFind.GetDecompOptions(), frame, refresh, ref this.decompDatas);

                string s = null;
                if (this.decompFind.GetDecompOptions().modelType == EModelType.GAMSScalar)
                {
                    s = Model.GetEquationTextHelper(this.decompFind.GetDecompOptions().link, this.decompFind.GetDecompOptions().showTime, this.decompFind.GetDecompOptions().t1);
                }
                else
                {
                    s = Model.GetEquationTextFoldedNonScalar(this.decompFind.GetDecompOptions().modelType, this.decompFind.GetDecompOptions().link);
                }
                this.equation.Text = s;
                this.code.Text = this.decompFind.GetDecompOptions().code.Last() + G.NL + G.Blanks(200);  //blanks hack, also used elsewhere

                //
                // NOTE:
                //
                //flowText.Visibility = Visibility.Collapsed;

                //TODO: what is this? delete?
                //TODO: what is this? delete?
                //TODO: what is this? delete?
                this.decompFind.GetDecompOptions().guiDecompValues = table;

                if (G.IsUnitTesting() && Globals.showDecompTable == false)
                {
                    Globals.lastDecompTable = table;
                }
                else
                {
                    ClearGrid();
                    MakeGuiTable2(table, this.decompFind.GetDecompOptions());
                }

                return;

            }
            catch (Exception e)
            {                
                if (G.IsUnitTesting())
                {
                    throw e;  //we want that error for unist tests!
                }
                else
                {
                    if (Globals.runningOnTTComputer)
                    {
                        MessageBox.Show(e.Message + " --trace-> " + e.StackTrace);
                    }
                    this.isClosing = true;
                    MessageBox.Show("*** ERROR: Decomp update failed: maybe some variables or databanks are non-available?");
                    throw e;
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
                checkBox1.IsEnabled = true;
                checkBox1.Opacity = 1.0;
                checkBox2.IsEnabled = true;
                checkBox2.Opacity = 1.0;
                //flowText.Opacity = 0.5;
                //flowText.Visibility = Visibility.Visible;
            }

            if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRef)
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

            if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsShares)
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

            if (this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw)
            {
                checkBox1.IsEnabled = false;  //shares
                checkBox1.Opacity = 0.5;
                //flowText.Visibility = Visibility.Collapsed;
            }

            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            //hmmm but prtOptionLower cannot have x etc....??
            if (G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "m") || G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "xm") || G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "q") || G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "xq") || G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "mp") || G.Equal(this.decompFind.GetDecompOptions().prtOptionLower, "xmp"))
            {
                checkBox2.IsEnabled = false;  //baseline, not meaningful for multiplier types
                checkBox2.Opacity = 0.5;
            }            

            if (false)
            {
                //TODO TODO TODO TODO TODO TODO  TODO TODO
                //TODO TODO TODO TODO TODO TODO  TODO TODO
                //TODO TODO TODO TODO TODO TODO  TODO TODO greyed out for now
                //TODO TODO TODO TODO TODO TODO  TODO TODO
                //TODO TODO TODO TODO TODO TODO  TODO TODO                
                checkBox1.IsEnabled = false;
                checkBox1.Opacity = 0.5;

                radioButton9.IsEnabled = false;
                radioButton9.Opacity = 0.5;
                radioButton10.IsEnabled = false;
                radioButton10.Opacity = 0.5;
                radioButton29.IsEnabled = false;
                radioButton29.Opacity = 0.5;
                radioButton30.IsEnabled = false;
                radioButton30.Opacity = 0.5;
            }
        }

        //private void SetRadioButtonDefaults()
        //{
        //    //Setting defaults
        //    radioButton21.IsEnabled = true;
        //    radioButton21.Opacity = 1.0;
        //    radioButton22.IsEnabled = true;
        //    radioButton22.Opacity = 1.0;
        //    radioButton24.IsEnabled = true;
        //    radioButton24.Opacity = 1.0;
        //    radioButton29.IsEnabled = true;
        //    radioButton29.Opacity = 1.0;
        //    radioButton26.IsEnabled = true;
        //    radioButton26.Opacity = 1.0;
        //    radioButton28.IsEnabled = true;
        //    radioButton28.Opacity = 1.0;
        //    radioButton30.IsEnabled = true;
        //    radioButton30.Opacity = 1.0;
        //    //---
        //    radioButton1.IsEnabled = true;
        //    radioButton1.Opacity = 1.0;
        //    radioButton2.IsEnabled = true;
        //    radioButton2.Opacity = 1.0;
        //    radioButton4.IsEnabled = true;
        //    radioButton4.Opacity = 1.0;
        //    radioButton9.IsEnabled = true;
        //    radioButton9.Opacity = 1.0;
        //    radioButton6.IsEnabled = true;
        //    radioButton6.Opacity = 1.0;
        //    radioButton8.IsEnabled = true;
        //    radioButton8.Opacity = 1.0;
        //    radioButton10.IsEnabled = true;
        //    radioButton10.Opacity = 1.0;
        //    //---
        //    checkBox1.IsEnabled = true;
        //    checkBox1.Opacity = 1.0;
        //    checkBox2.IsEnabled = true;
        //    checkBox2.Opacity = 1.0;
        //    flowText.Opacity = 0.5;
        //    flowText.Visibility = Visibility.Visible;
        //}

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
                EquationHelper eh = Program.FindEquationByMeansOfVariableName(this.decompFind.GetDecompOptions().variable);
                if (eh == null) return ""; //probably only when model is changed while UDVALG window is open (this is illegal anyway, and a popup will appear)
                else return ((EquationHelper)eh).equationText;
            }
        }        

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "n";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "d";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton6_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = false;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "d";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "p";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton8_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = false;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "p";
                RecalcCellsWithNewType(false);
            }
        }        

        private void radioButton21_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "n";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton22_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "m";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton26_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = false;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "m";
                RecalcCellsWithNewType(false);
            }
        }        

        private void radioButton24_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "q";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton28_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = false;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "q";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton29_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "mp";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton30_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = false;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "mp";
                RecalcCellsWithNewType(false);
            }
        }

        private void radioButton9_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = true;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "dp";
                RecalcCellsWithNewType(false);
            }
        }       
        
        private void radioButton10_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRaw = false;
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompOperator = "dp";
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsShares = true;
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsShares = false;
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRef = true;
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().operatorHelper.guiDecompIsRef = false;
                RecalcCellsWithNewType(false);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.guiDecompWindowTopDistance = Math.Max(1, (int)this.Top);
            Globals.guiDecompWindowLeftDistance = Math.Max(1, (int)this.Left);
            Globals.guiDecompWindowHeightDistance = Math.Max(1, (int)this.Height);
            Globals.guiDecompWindowWidthDistance = Math.Max(1, (int)this.Width);
            Globals.guiDecompWindowSplitterHorizontal = Math.Max(1, (int)this.splitterHorizontal.Width.Value);
            Globals.guiDecompWindowSplitterVertical = Math.Max(1, (int)this.splitterVertical.Height.Value);

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
                this.decompFind.GetDecompOptions().localBanks = null;  //clearing this, forcing window to use vales from Gekko databanks
                this.RecalcCellsWithNewType(true);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                if (this.decompFind.GetDecompOptions().decompTablesFormat.isPercentageType)
                {
                    this.decompFind.GetDecompOptions().decompTablesFormat.decimalsPch++;
                }
                else
                {
                    this.decompFind.GetDecompOptions().decompTablesFormat.decimalsLevel++;
                }
                RecalcCellsWithNewType(false);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                if (this.decompFind.GetDecompOptions().decompTablesFormat.isPercentageType)
                {
                    this.decompFind.GetDecompOptions().decompTablesFormat.decimalsPch--;
                    if (this.decompFind.GetDecompOptions().decompTablesFormat.decimalsPch < 0) this.decompFind.GetDecompOptions().decompTablesFormat.decimalsPch = 0;
                }
                else
                {
                    this.decompFind.GetDecompOptions().decompTablesFormat.decimalsLevel--;
                    if (this.decompFind.GetDecompOptions().decompTablesFormat.decimalsLevel < 0) this.decompFind.GetDecompOptions().decompTablesFormat.decimalsLevel = 0;
                }
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBoxErrors_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().count = ECountType.Names;
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBoxErrors_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().count = ECountType.None;
                RecalcCellsWithNewType(false);                
            }
        }

        private void ListButton1_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            GekkoTask task = button.DataContext as GekkoTask;            
            MessageBox.Show("Add new row item");            
        }

        private void ListButton2_Click(object sender, RoutedEventArgs e)
        {            
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
            RecalcCellsWithNewType(false);            
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

        private void checkBoxErrors2_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().count = ECountType.Names;
                RecalcCellsWithNewType(false);
            }
        }

        private void checkBoxErrors2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isInitializing)
            {
                this.decompFind.GetDecompOptions().count = ECountType.None;
                RecalcCellsWithNewType(false);
            }
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class GekkoDockPanel2 : DockPanel
    {
        public WindowDecomp.GekkoTableTypes type = WindowDecomp.GekkoTableTypes.Unknown;
        public Brush originalBackgroundColor = null;
    }

    public enum EDecompOperatorBankTypes
    {
        Unknown,
        Work,
        Ref,
        Both
    }

    public enum EDecompOperatorHelperTypes
    {
        Unknown,
        N,
        RN,
        D,        
        RD,
        M        
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

    public class DecompTablesFormat2
    {
        public int decimalsLevel = 4;
        public int decimalsPch = 2;
        public bool isPercentageType = false;
        public bool showErrors = false;
    }

    public enum ECountType
    {
        None,
        N,
        Names
    }

    public class DecompOptions2
    {
        //FIND STUFF
        public List iv = null;
        public GekkoTime t0 = GekkoTime.tNull;

        //public List<List<PeriodAndVariable>> precedentsScalar = null;
        public DecompTablesFormat2 decompTablesFormat = new DecompTablesFormat2();
        public EModelType modelType = EModelType.Unknown;
        public bool missingAsZero = false;
        public bool showTime = false;
        public List<string> code = new List<string>();

        //-------- tranformation start --------------
        public DecompOperatorHelper operatorHelper = new DecompOperatorHelper();

        public bool isNew = false;

        public int numberOfRecalcs = 0;  //used to pause main thread until the DECOMP window has calculated.
        public string variable = null;
        public List<string> variable_subelement = null;        
        
        public string expressionOld = null;  //only != null for expressions
        public Func<GekkoSmpl, IVariable> expression = null;
        public List<Dictionary<string, string>> precedents;  //only != null for expressions
        public string type;  //not used yet (UDVALG or DECOMP)

        
        public GekkoTime t1 = GekkoTime.tNull;
        public GekkoTime t2 = GekkoTime.tNull;
        public string prtOptionLower;  //only used at first call of UDVALG (e.g. UDVALG<p>): when isSubWindow is false.
        public bool dyn = false;
        public ECountType count = ECountType.None;

        public bool isSubWindow = false;  //when browsing/clicking, opening a new window

        public List<string> subst = new List<string>();

        public IVariable name = null;  //only active for names like x, x[a] and the like, not for expressions

        public List<string> new_select = null;
        public List<string> new_from = null;
        public List<string> new_endo = null;

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
        // --------- used for dropdown lists in gui

        //TODO
        //TODO
        //TODO Clone!!!!! ???
        //TODO
        //TODO

        public List<string> all = new List<string>();
        public ObservableCollection<string> free = new ObservableCollection<string>();
        public List<GekkoDictionary<string, string>> freeValues = null;
        public ObservableCollection<string> freeFilter = new ObservableCollection<string>();
        public List<FrameFilter> filters = null;

        public Data dataPattern = null;

        public DecompOptions2 Clone()
        {
            //clones relevant parts for new window
            DecompOptions2 d = new DecompOptions2();
            d.decompTablesFormat = new DecompTablesFormat2();
            d.decompTablesFormat.decimalsLevel = this.decompTablesFormat.decimalsLevel;
            d.decompTablesFormat.decimalsPch = this.decompTablesFormat.decimalsPch;
            d.decompTablesFormat.isPercentageType = this.decompTablesFormat.isPercentageType;
            d.decompTablesFormat.showErrors = this.decompTablesFormat.showErrors;

            d.code = new List<string>();
            foreach (string s in this.code) d.code.Add(s);

            d.modelType = this.modelType;
            
            //d.tp = this.tp;
            d.variable = this.variable;
            d.t1 = this.t1;
            d.t2 = this.t2;
            d.prtOptionLower = this.prtOptionLower;
            d.dyn = this.dyn;
            d.count = this.count;
            d.missingAsZero = this.missingAsZero;

            d.operatorHelper = new DecompOperatorHelper();
            d.operatorHelper.guiDecompOperator = this.operatorHelper.guiDecompOperator;
            d.operatorHelper.guiDecompIsShares = this.operatorHelper.guiDecompIsShares;
            d.operatorHelper.guiDecompIsRaw = this.operatorHelper.guiDecompIsRaw;
            d.operatorHelper.guiDecompIsRef = this.operatorHelper.guiDecompIsRef;

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
