using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowTreeViewWithTable.xaml
    /// </summary>

    public class TreeGridElement : ContentElement
    {
        private const string NullItemError = "The item added to the collection cannot be null.";

        public static readonly RoutedEvent ExpandingEvent;
        public static readonly RoutedEvent ExpandedEvent;
        public static readonly RoutedEvent CollapsingEvent;
        public static readonly RoutedEvent CollapsedEvent;
        public static readonly DependencyProperty HasChildrenProperty;
        public static readonly DependencyProperty IsExpandedProperty;
        public static readonly DependencyProperty LevelProperty;

        public TreeGridElement Parent { get; private set; }
        public TreeGridModel Model { get; private set; }
        public ObservableCollection<TreeGridElement> Children { get; private set; }

        static TreeGridElement()
        {
            // Register dependency properties
            HasChildrenProperty = RegisterHasChildrenProperty();
            IsExpandedProperty = RegisterIsExpandedProperty();
            LevelProperty = RegisterLevelProperty();

            // Register routed events
            ExpandingEvent = RegisterExpandingEvent();
            ExpandedEvent = RegisterExpandedEvent();
            CollapsingEvent = RegisterCollapsingEvent();
            CollapsedEvent = RegisterCollapsedEvent();
        }

        public TreeGridElement()
        {
            // Initialize the element
            Children = new ObservableCollection<TreeGridElement>();

            // Attach events
            Children.CollectionChanged += OnChildrenChanged;
        }

        internal void SetModel(TreeGridModel model, TreeGridElement parent = null)
        {
            // Set the element information
            Model = model;
            Parent = parent;
            Level = ((parent != null) ? parent.Level + 1 : 0);

            // Iterate through all child elements
            foreach (TreeGridElement child in Children)
            {
                // Set the model for the child
                child.SetModel(model, this);
            }
        }

        protected virtual void OnExpanding()
        {

        }
        protected virtual void OnExpanded() { }
        protected virtual void OnCollapsing() { }
        protected virtual void OnCollapsed() { }

        private void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            // Process the event
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    // Process added child
                    OnChildAdded(args.NewItems[0]);
                    break;

                case NotifyCollectionChangedAction.Replace:

                    // Process replaced child
                    OnChildReplaced((TreeGridElement)args.OldItems[0], args.NewItems[0], args.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:

                    // Process removed child
                    OnChildRemoved((TreeGridElement)args.OldItems[0]);
                    break;

                case NotifyCollectionChangedAction.Reset:

                    // Process cleared children
                    OnChildrenCleared(args.OldItems);
                    break;
            }
        }

        private void OnChildAdded(object item)
        {
            // Verify the new child
            TreeGridElement child = VerifyItem(item);

            // Set the model for the child
            child.SetModel(Model, this);

            // Notify the model that a child was added to the item
            Model?.OnChildAdded(child);
        }

        private void OnChildReplaced(TreeGridElement oldChild, object item, int index)
        {
            // Verify the new child
            TreeGridElement child = VerifyItem(item);

            // Clear the model for the old child
            oldChild.SetModel(null);

            // Notify the model that a child was replaced
            Model?.OnChildReplaced(oldChild, child, index);
        }

        private void OnChildRemoved(TreeGridElement child)
        {
            // Clear the model for the child
            child.SetModel(null);

            // Notify the model that a child was removed from the item
            Model?.OnChildRemoved(child);
        }

        private void OnChildrenCleared(IList children)
        {
            // Iterate through all of the children
            foreach (TreeGridElement child in children)
            {
                // Clear the model for the child
                child.SetModel(null);
            }

            // Notify the model that all of the children were removed from the item
            Model?.OnChildrenRemoved(this, children);
        }

        internal static TreeGridElement VerifyItem(object item)
        {
            // Is the item valid?
            if (item == null)
            {
                // The item is not valid
                throw new ArgumentNullException(nameof(item), NullItemError);
            }

            // Return the element
            return (TreeGridElement)item;
        }

        private static void OnIsExpandedChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            // Get the tree item
            TreeGridElement item = (TreeGridElement)element;

            // Is the item being expanded?
            if ((bool)args.NewValue)
            {
                // Raise expanding event
                item.RaiseEvent(new RoutedEventArgs(ExpandingEvent, item));

                // Execute derived expanding handler
                item.OnExpanding();

                // Expand the item in the model
                item.Model?.Expand(item);

                // Raise expanded event
                item.RaiseEvent(new RoutedEventArgs(ExpandedEvent, item));

                // Execute derived expanded handler
                item.OnExpanded();
            }
            else
            {
                // Raise collapsing event
                item.RaiseEvent(new RoutedEventArgs(CollapsingEvent, item));

                // Execute derived collapsing handler
                item.OnCollapsing();

                // Collapse the item in the model
                item.Model?.Collapse(item);

                // Raise collapsed event
                item.RaiseEvent(new RoutedEventArgs(CollapsedEvent, item));

                // Execute derived collapsed handler
                item.OnCollapsed();
            }
        }

        private static DependencyProperty RegisterHasChildrenProperty()
        {
            // Create the property metadata
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = false
            };

            // Register the property
            return DependencyProperty.Register(nameof(HasChildren), typeof(bool), typeof(TreeGridElement), metadata);
        }

        private static DependencyProperty RegisterIsExpandedProperty()
        {
            // Create the property metadata
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                PropertyChangedCallback = OnIsExpandedChanged
            };

            // Register the property
            return DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(TreeGridElement), metadata);
        }

        private static DependencyProperty RegisterLevelProperty()
        {
            // Create the property metadata
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = 0
            };

            // Register the property
            return DependencyProperty.Register(nameof(Level), typeof(int), typeof(TreeGridElement), metadata);
        }

        private static RoutedEvent RegisterExpandingEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Expanding), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeGridElement));
        }

        private static RoutedEvent RegisterExpandedEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Expanded), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeGridElement));
        }

        private static RoutedEvent RegisterCollapsingEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Collapsing), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeGridElement));
        }

        private static RoutedEvent RegisterCollapsedEvent()
        {
            // Register the event
            return EventManager.RegisterRoutedEvent(nameof(Collapsed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeGridElement));
        }

        public bool HasChildren
        {
            get { return (bool)GetValue(HasChildrenProperty); }
            set { SetValue(HasChildrenProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            private set { SetValue(LevelProperty, value); }
        }

        public event RoutedEventHandler Expanding
        {
            add { AddHandler(ExpandingEvent, value); }
            remove { RemoveHandler(ExpandingEvent, value); }
        }

        public event RoutedEventHandler Expanded
        {
            add { AddHandler(ExpandedEvent, value); }
            remove { RemoveHandler(ExpandedEvent, value); }
        }

        public event RoutedEventHandler Collapsing
        {
            add { AddHandler(CollapsingEvent, value); }
            remove { RemoveHandler(CollapsingEvent, value); }
        }

        public event RoutedEventHandler Collapsed
        {
            add { AddHandler(CollapsedEvent, value); }
            remove { RemoveHandler(CollapsedEvent, value); }
        }
    }

    public class TreeGridFlatModel : ObservableCollection<TreeGridElement>
    {
        private const string ModificationError = "The collection cannot be modified by the user.";

        private bool modification;
        private HashSet<TreeGridElement> keys;

        public TreeGridFlatModel()
        {
            // Initialize the model
            keys = new HashSet<TreeGridElement>();
        }

        internal bool ContainsKey(TreeGridElement item)
        {
            // Return a value indicating if the item is within the model
            return keys.Contains(item);
        }

        internal void PrivateInsert(int index, TreeGridElement item)
        {
            // Set the modification flag
            modification = true;

            // Add the item to the model
            Insert(index, item);

            // Add the item to the keys
            keys.Add(item);

            // Clear the modification flag
            modification = false;
        }

        internal void PrivateInsertRange(int index, IList<TreeGridElement> items)
        {
            // Set the modification flag
            modification = true;

            // Iterate through all of the children within the items
            foreach (TreeGridElement child in items)
            {
                // Add the child to the model
                Insert(index++, child);

                // Add the child to the keys
                keys.Add(child);
            }

            // Clear the modification flag
            modification = false;
        }

        internal void PrivateRemoveRange(int index, int count)
        {
            // Set the modification flag
            modification = true;

            // Iterate through all of the items to remove from the model
            for (int itemIndex = 0; itemIndex < count; itemIndex++)
            {
                // Remove the item from the keys
                keys.Remove(Items[index]);

                // Remove the item from the model
                RemoveAt(index);
            }

            // Clear the modification flag
            modification = false;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            // Is the modification flag set?
            if (!modification)
            {
                // The collection is for internal use only
                throw new InvalidOperationException(ModificationError);
            }

            // Call base method
            base.OnCollectionChanged(args);
        }
    }

    public class TreeGridModel : ObservableCollection<TreeGridElement>
    {
        public TreeGridFlatModel FlatModel { get; private set; }

        private List<TreeGridElement> itemCache;

        public TreeGridModel()
        {
            // Initialize the model
            itemCache = new List<TreeGridElement>();
            FlatModel = new TreeGridFlatModel();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            // Process the event
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    // Process added item
                    OnRootAdded(args.NewItems[0]);
                    break;
            }
        }

        internal void Expand(TreeGridElement item)
        {
            // Do we need to expand the item?
            if (!FlatModel.ContainsKey(item) || !item.IsExpanded)
            {
                // We do not need to expand the item
                return;
            }

            // Clear the item cache
            itemCache.Clear();

            // Cache the flat children for the item
            CacheFlatChildren(item);

            // Get the insertion index for the item
            int index = (FlatModel.IndexOf(item) + 1);

            // Add the flat children to the flat model
            FlatModel.PrivateInsertRange(index, itemCache);
        }

        internal void Collapse(TreeGridElement item)
        {
            // Do we need to collapse the item?
            if (!FlatModel.ContainsKey(item))
            {
                // We do not need to collapse the item
                return;
            }

            // Get the collapse information
            int index = (FlatModel.IndexOf(item) + 1);
            int count = CountFlatChildren(item);

            // Remove the items from the flat model to collapse them
            FlatModel.PrivateRemoveRange(index, count);
        }

        internal void OnChildAdded(TreeGridElement child)
        {
            // Get the parent of the child
            TreeGridElement parent = child.Parent;

            // Check if the parent is expanded
            if (!FlatModel.ContainsKey(parent) || !parent.IsExpanded)
            {
                // We don't need to update the flat model
                return;
            }

            // Find the insertion index for the child into the flat model
            int index = FindFlatInsertionIndex(child);

            // Insert the child into the flat model
            FlatModel.PrivateInsert(index, child);

            // Expand the child
            Expand(child);
        }

        internal void OnChildReplaced(TreeGridElement oldChild, TreeGridElement child, int index)
        {

        }

        internal void OnChildRemoved(TreeGridElement child)
        {

        }

        internal void OnChildrenRemoved(TreeGridElement parent, IList children)
        {

        }

        private void OnRootAdded(object item)
        {
            // Verify the root item
            TreeGridElement root = TreeGridElement.VerifyItem(item);

            // Set the model for the root
            root.SetModel(this);

            // Find the index for insertion into the flat model
            int index = FindFlatInsertionIndex(root);

            // Insert the root into the flat model
            FlatModel.PrivateInsert(index, root);

            // Expand the root
            Expand(root);
        }

        private void CacheFlatChildren(TreeGridElement item)
        {
            // Iterate through all of the children within the item
            foreach (TreeGridElement child in item.Children)
            {
                // Add the child to the item cache
                itemCache.Add(child);

                // Is the child expanded?
                if (child.IsExpanded)
                {
                    // Recursively cache the children within the child
                    CacheFlatChildren(child);
                }
            }
        }

        private int CountFlatChildren(TreeGridElement item)
        {
            // Initialize child count
            int children = item.Children.Count;

            // Iterate through each child
            foreach (TreeGridElement child in item.Children)
            {
                // Is the child expanded?
                if (child.IsExpanded)
                {
                    // Recursively count the children
                    children += CountFlatChildren(child);
                }
            }

            // Return the number of flat children
            return children;
        }

        private int FindFlatInsertionIndex(TreeGridElement item)
        {
            // Get the search information
            TreeGridElement parent = item.Parent;
            IList<TreeGridElement> items = ((parent != null) ? parent.Children : this);
            int index = items.IndexOf(item);
            int lastIndex = (items.Count - 1);

            // Determine if the item is the last item in the items
            if (index < lastIndex)
            {
                // Return the insertion index using the items
                return FlatModel.IndexOf(items[(index + 1)]);
            }

            // Is the parent valid?
            else if (parent != null)
            {
                // Determine the number of flat children the parent has
                int children = CountFlatChildren(parent);

                // Return the insertion index using the number of flat children
                return (FlatModel.IndexOf(parent) + children);
            }
            else
            {
                // Return the flat model count
                return FlatModel.Count;
            }
        }
    }

    public class TestProgram
    {
        [STAThread]
        public static void Main2(string[] args)
        {
            // Create the app
            Application app = new Application();

            // Create the main window
            app.MainWindow = new WindowTreeViewWithTable();

            // Show the main window
            app.MainWindow.Show();

            // Run the app
            app.Run();
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the item has children, then show the checkbox, otherwise hide it
            return ((bool)value ? Visibility.Visible : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LevelConverter : IValueConverter
    {
        public GridLength LevelWidth { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return the width multiplied by the level
            return ((int)value * LevelWidth.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Item : TreeGridElement
    {
        public int Value { get; private set; }
        public string Name { get; private set; }

        public Item(string name, int value, bool hasChildren)
        {
            // Initialize the item
            Name = name;
            Value = value;
            HasChildren = hasChildren;
        }
    }

    public partial class WindowTreeViewWithTable : Window
    {
        private const int Levels = 3;
        private const int Roots = 100;
        private const int ItemsPerLevel = 5;
        private int value;
        public TreeGridModel model;

        public WindowTreeViewWithTable()
        {
            // Initialize the component
            InitializeComponent();

            // Initialize the model
            InitModel();

            // Set the model for the grid
            grid.ItemsSource = model.FlatModel;            
        }

        private void InitModel()
        {
            //TreeGridModel model = new TreeGridModel();
            //Trace2.PrintTraceHelper(this.startTrace, 0, true, model);

            //// Create the model
            //model = new TreeGridModel();

            //// Add a bunch of items at the root
            //for (int count = 0; count < Roots; count++)
            //{
            //    // Create the root item
            //    Item root = new Item(String.Format("Root {0}", count), value++, true);

            //    // Add children to the root
            //    AddChildren(root);

            //    // Add the root to the model
            //    model.Add(root);
            //}
        }

        private int c(Item i)
        {
            int cnt = i.Children.Count;

            foreach (Item child in i.Children)
            {
                cnt += c(child);
            }

            return cnt;
        }

        private void AddChildren(Item item, int level = 0)
        {
            // Determine if the item will have children
            bool hasChildren = (level < Levels);

            // Create children for the item
            for (int count = 0; count < ItemsPerLevel; count++)
            {
                // Create the child
                Item child = new Item(String.Format("Child {0}, Level {1}", count, level), value++, hasChildren);

                // Does the child have children?
                if (hasChildren)
                {
                    // Recursively add children to the child
                    AddChildren(child, (level + 1));
                }

                // Add the child to the item
                item.Children.Add(child);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //object o = ((DataGrid)grid.SelectedItem).SelectedItem;
            Item item = (sender as DataGrid).SelectedItem as Item;
            //MessageBox.Show("Selected " + item.Name + "¤" + item.Level);
            text.Text = "Selected " + item.Name + "¤" + item.Level;
        }

        private void SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //Item item = (sender as DataGrid).SelectedItem as Item;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DataGridRow row = sender as DataGridRow;			
            //Item xx = row.Item as Item;
        }
    }

}
