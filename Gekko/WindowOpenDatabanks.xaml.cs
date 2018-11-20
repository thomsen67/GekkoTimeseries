using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using WPF.JoshSmith.ServiceProviders.UI;

namespace Gekko
{
	/// <summary>
	/// Demonstrates how to use the ListViewDragManager class.
	/// </summary>
	public partial class WindowOpenDatabanks : System.Windows.Window
	{
        private ObservableCollection<Task> _list = new ObservableCollection<Task>();
        ListViewDragDropManager<Task> dragMgr;
        //Dictionary<string, string> databankAliases = new Dictionary<string, string>();

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

        public WindowOpenDatabanks()
		{
			InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
            this.Loaded += WindowOpenDatabanks_Loaded;

            if (Program.options.interface_databank_swap)
            {
                yellow.Text = "You may drag databanks to swap their places. This changes their position in the list. Swapping is intended for viewing purposes, and you may return the Work and Ref databanks to their 'normal' positions by means of clicking the 'Unswap' button or typing the UNSWAP command.";
            }
            else
            {
                yellow.Text = "Databank dragging/swapping deactivated, cf. 'OPTION interface databank swap = '. You may use OPEN<edit> or OPEN<ref> instead.";
            }
		}

		#region Window1_Loaded

        void WindowOpenDatabanks_Loaded(object sender, RoutedEventArgs e)
		{
			// Give the ListView an ObservableCollection of Task
			// as a data source.  Note, the ListViewDragManager MUST
			// be bound to an ObservableCollection, where the collection's
			// type parameter matches the ListViewDragManager's type
			// parameter (in this case, both have a type parameter of Task).

            list = new ObservableCollection<Task>();
            RefreshList();

			this.listView.ItemsSource = list;            

			//this.listView2.ItemsSource = new ObservableCollection<Task>();

			// This is all that you need to do, in order to use the ListViewDragManager.
			this.dragMgr = new ListViewDragDropManager<Task>( this.listView );			
            this.dragMgr.ListView = this.listView;
            this.dragMgr.ShowDragAdorner = true;
            this.dragMgr.DragAdornerOpacity = 0.5d;  //so that e.g. "Work" can still be seen underneath
            this.listView.ItemContainerStyle = this.FindResource("ItemContStyle") as Style;
            
            this.dragMgr.ProcessDrop += dragMgr_ProcessDrop;

            //// Turn the ListViewDragManager on and off.
            //this.chkManageDragging.Checked += delegate { this.dragMgr.ListView = this.listView; };
            //this.chkManageDragging.Unchecked += delegate { this.dragMgr.ListView = null; };
            //// Show and hide the drag adorner.
            //this.chkDragAdorner.Checked += delegate { this.dragMgr.ShowDragAdorner = true; };
            //this.chkDragAdorner.Unchecked += delegate { this.dragMgr.ShowDragAdorner = false; };
            //// Change the opacity of the drag adorner.
            //this.sldDragOpacity.ValueChanged += delegate { this.dragMgr.DragAdornerOpacity = this.sldDragOpacity.Value; };
            //// Apply or remove the item container style, which responds to changes
            //// in the attached properties of ListViewItemDragState.
            //this.chkApplyContStyle.Checked += delegate { this.listView.ItemContainerStyle = this.FindResource( "ItemContStyle" ) as Style; };
            //this.chkApplyContStyle.Unchecked += delegate { this.listView.ItemContainerStyle = null; };
            //// Use or do not use custom drop logic.
            //this.chkSwapDroppedItem.Checked += delegate { this.dragMgr.ProcessDrop += dragMgr_ProcessDrop; };
            //this.chkSwapDroppedItem.Unchecked += delegate { this.dragMgr.ProcessDrop -= dragMgr_ProcessDrop; };
            //// Show or hide the lower ListView.
            //this.chkShowOtherListView.Checked += delegate { this.listView2.Visibility = Visibility.Visible; };
            //this.chkShowOtherListView.Unchecked += delegate { this.listView2.Visibility = Visibility.Collapsed; };
            
			// Hook up events on both ListViews to that we can drag-drop
			// items between them.
			this.listView.DragEnter += OnListViewDragEnter;			
			this.listView.Drop += OnListViewDrop;			
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

        

        #endregion // Window1_Loaded

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            //only work with showdialog ........ HMMMMMMMMMMMMMMMM!
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

		#region dragMgr_ProcessDrop

		// Performs custom drop logic for the top ListView.
		void dragMgr_ProcessDrop( object sender, ProcessDropEventArgs<Task> e )
		{
			// This shows how to customize the behavior of a drop.
			// Here we perform a swap, instead of just moving the dropped item.

            if (!Program.options.interface_databank_swap)
            {
                MessageBox.Show("*** ERROR: Databank swapping not allowed, since 'OPTION interface databank swap = no'");
                return;
            }

            string text = "";

            int higherIdx = Math.Max(e.OldIndex, e.NewIndex);
            int lowerIdx = Math.Min(e.OldIndex, e.NewIndex);

            Task t_from = list[e.OldIndex];
            Task t_to = list[e.NewIndex];

            string aliasFromOld = t_from.AliasName;
            string aliasToOld = t_to.AliasName;
            
            string s = null;

			if( lowerIdx < 0 )
			{
				// The item came from the lower ListView
				// so just insert it.
				e.ItemsSource.Insert( higherIdx, e.DataItem );
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

        private static string MaybeAddStuffToAliasDatabankName(string s)
        {
            if (Program.databanks.GetDatabank(s) == null)
            {
                //ok, do nothing
            }
            else
            {
                for (int i = 1; i < int.MaxValue; i++)
                {
                    if (Program.databanks.GetDatabank(s + "_" + i) != null) continue;
                    s = s + "_" + i;
                    break;
                }
            }
            return s;
        }

        //private static void SwapBankAliases(string b1, string b2)
        //{
        //    Databank db1 = Program.databanks.GetDatabank(b1);            
        //    Databank db2 = Program.databanks.GetDatabank(b2);            
        //    string temp = db1.aliasName;
        //    db1.aliasName = db2.aliasName;
        //    db2.aliasName = temp;
        //    Program.databanks.storage.Remove(b1);
        //    Program.databanks.storage.Remove(b2);
        //    Program.databanks.AddDatabank(db1);
        //    Program.databanks.AddDatabank(db2);
        //}        

		#endregion // dragMgr_ProcessDrop

		#region OnListViewDragEnter

		// Handles the DragEnter event for both ListViews.
		void OnListViewDragEnter( object sender, DragEventArgs e )
		{
			e.Effects = DragDropEffects.Move;
		}

		#endregion // OnListViewDragEnter

		#region OnListViewDrop

		// Handles the Drop event for both ListViews.
		void OnListViewDrop( object sender, DragEventArgs e )
		{            
            if( e.Effects == DragDropEffects.None )
				return;

			Task task = e.Data.GetData( typeof( Task ) ) as Task;
			if( sender == this.listView )
			{
				if( this.dragMgr.IsDragInProgress )
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

		#endregion // OnListViewDrop

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //listView.SelectedItem = null;  --> no, then we cannot move the row
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            //Program.ShowPeriodInStatusField("");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Program.Unswap(false);
            RefreshList();
            //string s = Program.UnswapMessageLong();
            //yellow.Text = s;
            //Program.ShowPeriodInStatusField("");
            //G.Writeln();
            //G.Writeln(Program.UnswapMessage());
        }        

	}
}
