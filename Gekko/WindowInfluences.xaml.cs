using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowInfluences.xaml
    /// </summary>
    public partial class WindowInfluences : Window
    {
        // Property the XAML binds to
        public List<HyperlinkItem> Items { get; set; }

        public WindowInfluences(List<string> names, List<string> tooltips)
        {
            InitializeComponent();

            // Zip the two lists together into our view model
            Items = names.Select((name, index) => new HyperlinkItem
            {
                Name = name,
                // Fallback in case the tooltips list is shorter than the names list
                ToolTip = index < tooltips.Count ? tooltips[index] : string.Empty
            }).ToList();

            // Set DataContext so XAML can see 'Items'
            this.DataContext = this;
        }

        // Click event for the hyperlinks
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            if (hyperlink != null)
            {
                // Get the data item bound to this link
                var dataItem = hyperlink.DataContext as HyperlinkItem;

                // Show the "Hello" popup (using a standard MessageBox for simplicity, 
                // or you can instantiate another Window here)
                MessageBox.Show($"Hello! You clicked: {dataItem.Name}", "Popup", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
        }
    }
}
