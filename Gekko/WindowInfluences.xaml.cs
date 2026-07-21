using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowInfluences.xaml
    /// </summary>
    public partial class WindowInfluences : Window
    {
        public DecompFind decompFind = null;

        public List<HyperlinkItem> Items { get; set; }

        public WindowInfluences(List<string> names, List<string> tooltips, DecompFind decompFindHere)
        {            
            this.decompFind = new DecompFind(EDecompFindNavigation.Unknown, 0, decompFindHere.decompOptions2.Clone(), null, decompFindHere.model);
            InitializeComponent();            
            Items = names.Select((name, index) => new HyperlinkItem
            {
                Name = name,
                ToolTip = index < tooltips.Count ? tooltips[index] : string.Empty
            }).ToList();            
            this.DataContext = this;
        }
                
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            if (hyperlink != null)
            {                
                var dataItem = hyperlink.DataContext as HyperlinkItem;
                WindowDecomp.DecompLinkClicked(dataItem.Name, this.decompFind);
                this.Close();
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
