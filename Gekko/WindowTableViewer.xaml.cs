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
using System.Data;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowTableViewer.xaml
    /// </summary>
    public partial class WindowTableViewer : Window
    {
        public WindowTableViewer(DataTable dt)
        {
            InitializeComponent();
            grid1.DataContext = dt.DefaultView;
        }
    }
}
