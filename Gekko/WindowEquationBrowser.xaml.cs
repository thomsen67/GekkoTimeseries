using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowEquationBrowser.xaml
    /// </summary>
        

    public partial class WindowEquationBrowser
    {
        
        public WindowEquationBrowser()
        {            
            InitializeComponent();
        }

        public void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        public void OnHyperlinkMouseEnter(object sender, MouseEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;            
            Program.EquationBrowserSetLabel((link.Inlines.FirstInline as Run).Text, this);            
        }

        public void OnHyperlinkMouseLeave(object sender, MouseEventArgs e)
        {            
            //windowEquationBrowserLabel.Inlines.Clear();
            //windowEquationBrowserLabel.Inlines.Add("Leave");
        }

        private void ActiveCasesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EquationListItem item = e.AddedItems[0] as EquationListItem;
            windowEquationBrowserText.Inlines.Clear();
            windowEquationBrowserText.Inlines.Add(item.Name);

            List<ModelGamsEquation> xx2 = Program.modelGams.equationsByEqname[item.Name];
            string s = xx2[0].lhs + " = " + xx2[0].rhs;

            List m = Program.databanks.GetFirst().GetIVariable("#m") as List;
            List m3 = null;
            foreach (List m1 in m.list)
            {
                if (G.Equal((m1.list[0] as ScalarString).string2, item.Name))
                {
                    m3 = m1.list[1] as List;
                }
            }
            if (m3 != null)
            {
                List<string> yy = Program.GetListOfStringsFromListOfIvariables((m3 as List).list.ToArray()).ToList();
                //string xx = G.GetListWithCommas(yy).Replace("¤[0]", "").Replace("¤", "").Replace(", residual___", "");
                Program.EquationBrowserSetEquationText(s, yy, this);
            }
            
            //windowEquationBrowserText.Inlines.Add(s);
            //firstList.AddRange(yy);          
        }
    }

    public class EquationListItem
    {
        public EquationListItem(string name, string sub, bool dep, bool lhs, string per, string vars, string lineColor)
        {
            Name = name;
            Sub = sub;
            Dep = dep;
            Lhs = lhs;
            Per = per;
            Vars = vars;
            LineColor = lineColor;
        }

        public string Name { get; set; }

        public string Sub { get; set; }

        public bool Dep { get; set; }
        
        public bool Lhs { get; set; }

        public string Per { get; set; }

        public string Vars { get; set; }

        public string LineColor { get; set; }
    }

    public class ItemHandler
    {
        public ItemHandler()
        {
            Items = new List<EquationListItem>();
        }

        public List<EquationListItem> Items { get; private set; }

        public void Add(EquationListItem item)
        {
            Items.Add(item);
        }
    }

    public class MainWindowViewModel
    {
        private readonly ItemHandler _itemHandler;

        public MainWindowViewModel()
        {
            //_itemHandler=WindowEquationBrowser.ite

            _itemHandler = Globals.itemHandler;            

        }

        public List<EquationListItem> Items
        {
            get { return _itemHandler.Items; }
        }
    }
}
