﻿using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System;

namespace Gekko
{
    public partial class WindowTreeViewWithCheckBoxes : Window
    {
        public WindowTreeViewWithCheckBoxes()
        {
            InitializeComponent();

            FooViewModel root = this.tree.Items[0] as FooViewModel;

            base.CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.Undo,
                    (sender, e) => // Execute
                    {
                        e.Handled = true;
                        root.IsChecked = false;
                        this.tree.Focus();
                    },
                    (sender, e) => // CanExecute
                    {
                        e.Handled = true;
                        e.CanExecute = (root.IsChecked != false);
                    }));

            this.tree.Focus();
        }
    }

    public static class VirtualToggleButton
    {
        #region attached properties

        #region IsChecked

        /// <summary>
        /// IsChecked Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterAttached("IsChecked", typeof(Nullable<bool>), typeof(VirtualToggleButton),
                new FrameworkPropertyMetadata((Nullable<bool>)false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                    new PropertyChangedCallback(OnIsCheckedChanged)));

        /// <summary>
        /// Gets the IsChecked property.  This dependency property 
        /// indicates whether the toggle button is checked.
        /// </summary>
        public static Nullable<bool> GetIsChecked(DependencyObject d)
        {
            return (Nullable<bool>)d.GetValue(IsCheckedProperty);
        }

        /// <summary>
        /// Sets the IsChecked property.  This dependency property 
        /// indicates whether the toggle button is checked.
        /// </summary>
        public static void SetIsChecked(DependencyObject d, Nullable<bool> value)
        {
            d.SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsChecked property.
        /// </summary>
        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement pseudobutton = d as UIElement;
            if (pseudobutton != null)
            {
                Nullable<bool> newValue = (Nullable<bool>)e.NewValue;
                if (newValue == true)
                {
                    RaiseCheckedEvent(pseudobutton);
                }
                else if (newValue == false)
                {
                    RaiseUncheckedEvent(pseudobutton);
                }
                else
                {
                    RaiseIndeterminateEvent(pseudobutton);
                }
            }
        }

        #endregion

        #region IsThreeState

        /// <summary>
        /// IsThreeState Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsThreeStateProperty =
            DependencyProperty.RegisterAttached("IsThreeState", typeof(bool), typeof(VirtualToggleButton),
                new FrameworkPropertyMetadata((bool)false));

        /// <summary>
        /// Gets the IsThreeState property.  This dependency property 
        /// indicates whether the control supports two or three states.  
        /// IsChecked can be set to null as a third state when IsThreeState is true.
        /// </summary>
        public static bool GetIsThreeState(DependencyObject d)
        {
            return (bool)d.GetValue(IsThreeStateProperty);
        }

        /// <summary>
        /// Sets the IsThreeState property.  This dependency property 
        /// indicates whether the control supports two or three states. 
        /// IsChecked can be set to null as a third state when IsThreeState is true.
        /// </summary>
        public static void SetIsThreeState(DependencyObject d, bool value)
        {
            d.SetValue(IsThreeStateProperty, value);
        }

        #endregion

        #region IsVirtualToggleButton

        /// <summary>
        /// IsVirtualToggleButton Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsVirtualToggleButtonProperty =
            DependencyProperty.RegisterAttached("IsVirtualToggleButton", typeof(bool), typeof(VirtualToggleButton),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnIsVirtualToggleButtonChanged)));

        /// <summary>
        /// Gets the IsVirtualToggleButton property.  This dependency property 
        /// indicates whether the object to which the property is attached is treated as a VirtualToggleButton.  
        /// If true, the object will respond to keyboard and mouse input the same way a ToggleButton would.
        /// </summary>
        public static bool GetIsVirtualToggleButton(DependencyObject d)
        {
            return (bool)d.GetValue(IsVirtualToggleButtonProperty);
        }

        /// <summary>
        /// Sets the IsVirtualToggleButton property.  This dependency property 
        /// indicates whether the object to which the property is attached is treated as a VirtualToggleButton.  
        /// If true, the object will respond to keyboard and mouse input the same way a ToggleButton would.
        /// </summary>
        public static void SetIsVirtualToggleButton(DependencyObject d, bool value)
        {
            d.SetValue(IsVirtualToggleButtonProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsVirtualToggleButton property.
        /// </summary>
        private static void OnIsVirtualToggleButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IInputElement element = d as IInputElement;
            if (element != null)
            {
                if ((bool)e.NewValue)
                {
                    element.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    element.KeyDown += OnKeyDown;
                }
                else
                {
                    element.MouseLeftButtonDown -= OnMouseLeftButtonDown;
                    element.KeyDown -= OnKeyDown;
                }
            }
        }

        #endregion

        #endregion

        #region routed events

        #region Checked

        /// <summary>
        /// A static helper method to raise the Checked event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        internal static RoutedEventArgs RaiseCheckedEvent(UIElement target)
        {
            if (target == null) return null;

            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = ToggleButton.CheckedEvent;
            RaiseEvent(target, args);
            return args;
        }

        #endregion

        #region Unchecked

        /// <summary>
        /// A static helper method to raise the Unchecked event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        internal static RoutedEventArgs RaiseUncheckedEvent(UIElement target)
        {
            if (target == null) return null;

            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = ToggleButton.UncheckedEvent;
            RaiseEvent(target, args);
            return args;
        }

        #endregion

        #region Indeterminate

        /// <summary>
        /// A static helper method to raise the Indeterminate event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        internal static RoutedEventArgs RaiseIndeterminateEvent(UIElement target)
        {
            if (target == null) return null;

            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = ToggleButton.IndeterminateEvent;
            RaiseEvent(target, args);
            return args;
        }

        #endregion

        #endregion

        #region private methods

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            UpdateIsChecked(sender as DependencyObject);
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.OriginalSource == sender)
            {
                if (e.Key == Key.Space)
                {
                    // ignore alt+space which invokes the system menu
                    if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) return;

                    UpdateIsChecked(sender as DependencyObject);
                    e.Handled = true;

                }
                else if (e.Key == Key.Enter && (bool)(sender as DependencyObject).GetValue(KeyboardNavigation.AcceptsReturnProperty))
                {
                    UpdateIsChecked(sender as DependencyObject);
                    e.Handled = true;
                }
            }
        }

        private static void UpdateIsChecked(DependencyObject d)
        {
            Nullable<bool> isChecked = GetIsChecked(d);
            if (isChecked == true)
            {
                SetIsChecked(d, GetIsThreeState(d) ? (Nullable<bool>)null : (Nullable<bool>)false);
            }
            else
            {
                SetIsChecked(d, isChecked.HasValue);
            }
        }

        private static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
        {
            if (target is UIElement)
            {
                (target as UIElement).RaiseEvent(args);
            }
            else if (target is ContentElement)
            {
                (target as ContentElement).RaiseEvent(args);
            }
        }

        #endregion
    }


    public class FooViewModel : INotifyPropertyChanged
    {
        #region Data

        bool? _isChecked = false;
        FooViewModel _parent;

        #endregion // Data

        #region CreateFoos

        public static List<FooViewModel> CreateFoos()
        {
            FooViewModel root = new FooViewModel("Weapons")
            {
                IsInitiallySelected = true,
                Children =
                {
                    new FooViewModel("Blades")
                    {
                        Children =
                        {
                            new FooViewModel("Dagger"),
                            new FooViewModel("Machete"),
                            new FooViewModel("Sword"),
                        }
                    },
                    new FooViewModel("Vehicles")
                    {
                        Children =
                        {
                            new FooViewModel("Apache Helicopter"),
                            new FooViewModel("Submarine"),
                            new FooViewModel("Tank"),
                        }
                    },
                    new FooViewModel("Guns")
                    {
                        Children =
                        {
                            new FooViewModel("AK 47"),
                            new FooViewModel("Beretta"),
                            new FooViewModel("Uzi"),
                        }
                    },
                }
            };

            root.Initialize();
            return new List<FooViewModel> { root };
        }

        FooViewModel(string name)
        {
            this.Name = name;
            this.Children = new List<FooViewModel>();
        }

        void Initialize()
        {
            foreach (FooViewModel child in this.Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        #endregion // CreateFoos

        #region Properties

        public List<FooViewModel> Children { get; private set; }

        public bool IsInitiallySelected { get; private set; }

        public string Name { get; private set; }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child FooViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { this.SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                this.Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null)
                _parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        #endregion // IsChecked

        #endregion // Properties

        #region INotifyPropertyChanged Members

        void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

}