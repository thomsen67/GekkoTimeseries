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
using System.Xml;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for XML_To_TreeView.xaml
    /// </summary>
    public partial class XML_To_TreeView : Window
    {
        public XML_To_TreeView()
        {
            InitializeComponent();
        }
        private void cmdLoadXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Use the Win32 OpenFileDialog to allow the user to pick a file ...
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.DefaultExt = ".xml";
                ofd.Filter = "XML Documents (*.xml)|*.xml|All Files (*.*)|*.*";
                Nullable<bool> fUserPickedFile = ofd.ShowDialog(this);
                if (fUserPickedFile == true)
                {
                    //Create a new XDoc ...
                    XmlDocument doc = new XmlDocument();
                    //... and load the file that the user picked
                    
                    doc.Load(ofd.FileName);

                    //Use the XDP that has been created as one of the Window's resources ...
                    XmlDataProvider dp = (XmlDataProvider)this.FindResource("xmlDP");
                    //... and assign the XDoc to it, using the XDoc's root.
                    dp.Document = doc;
                    dp.XPath = "*";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdChangeDisplayRootNode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Get a reference to the XDP that has been created as one of the Window's resources
                XmlDataProvider dp = (XmlDataProvider)this.FindResource("xmlDP");
                if (txt.Text == "")
                    //(Reset to root)
                    dp.XPath = "*";
                else
                    //Use the specified path as the new root display-node.
                    dp.XPath = txt.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdReset_Click(object sender, RoutedEventArgs e)
        {
            txt.Text = "";
            cmdChangeDisplayRootNode_Click(sender, e);
        }

        private void cmdExpandAll_Click(object sender, RoutedEventArgs e)
        {
            tv.Style = (Style)this.FindResource("TV_AllExpanded");
        }

        private void cmdCollapse_Click(object sender, RoutedEventArgs e)
        {
            tv.Style = (Style)this.FindResource("TV_AllCollapsed");
        }

        private void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }

        
    }


    //[with lines] class TreeViewLineConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        TreeViewItem item = (TreeViewItem)value;
    //        ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
    //        return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }
    //}

}
