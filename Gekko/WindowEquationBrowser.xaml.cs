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

        public string _activeEquation = null;
        public GekkoDictionary<string, Button> _buttons = new GekkoDictionary<string, Button>(StringComparer.OrdinalIgnoreCase);

        public WindowEquationBrowser()
        {            
            InitializeComponent();
        }        

        public void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string s = b.Content.ToString();
            Program.Tell("find " + s, false);
        }

        public void OnButtonMouseEnter(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            string s = b.Content.ToString();
            this.EquationBrowserSetLabel(s);
        }

        public void OnButtonMouseLeave(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            string s = b.Content.ToString();
            string ss = GetEquationText(_activeEquation);
            this.windowEquationBrowserLabel.Inlines.Clear();
            this.windowEquationBrowserLabel.Inlines.Add(ss);
        }

        private void ActiveCasesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EquationListItem item = e.AddedItems[0] as EquationListItem;
            windowEquationBrowserLabel.Inlines.Clear();
            windowEquationBrowserLabel.Inlines.Add(item.Name);

            string s = GetEquationText(item.Name);

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
                this.EquationBrowserSetEquationButtons(item.Name, s, yy);
            }
        }

        private static string GetEquationText(string name)
        {
            List<ModelGamsEquation> xx2 = Program.modelGams.equationsByEqname[name];
            string s = xx2[0].lhs + " = " + xx2[0].rhs;
            return s;
        }

        public void EquationBrowserSetLabel(string variableName)
        {
            IVariable iv = O.GetIVariableFromString(variableName, O.ECreatePossibilities.NoneReturnNull, false);
            this.windowEquationBrowserLabel.Inlines.Clear();
            this.windowEquationBrowserLabel.Inlines.Add("Name: " + variableName + G.NL);

            if (iv != null)
            {
                Series ts = iv as Series;
                if (ts != null)
                {
                    if (ts.type == ESeriesType.Normal)
                    {
                        if (!G.NullOrBlanks(ts.meta.label))
                        {
                            this.windowEquationBrowserLabel.Inlines.Add("Label: " + ts.meta.label + G.NL);
                        }
                        else
                        {
                            if (ts.mmi.parent != null)
                            {
                                if (!G.NullOrBlanks(ts.mmi.parent.meta.label))
                                {
                                    this.windowEquationBrowserLabel.Inlines.Add("Label: " + ts.mmi.parent.meta.label + G.NL);
                                }
                            }
                        }
                    }

                    GekkoTime tStart = new GekkoTime(EFreq.A, 2015, 1);
                    GekkoTime tEnd = new GekkoTime(EFreq.A, 2025, 1);

                    this.windowEquationBrowserLabel.Inlines.Add("-----------------------------------------------" + G.NL);
                    this.windowEquationBrowserLabel.Inlines.Add("Period        value        %" + G.NL);

                    int counter = 0;

                    //must be able to handle TIME where freq does not match the series freq
                    foreach (GekkoTime gt in new GekkoTimeIterator(Program.ConvertFreqs(tStart, tEnd, ts.freq)))
                    {
                        counter++;
                        this.windowEquationBrowserLabel.Inlines.Add(gt.ToString() + " ");

                        double n1 = ts.GetDataSimple(gt);
                        double n0 = ts.GetDataSimple(gt.Add(-1));

                        double level1 = n1;
                        double pch1 = ((n1 / n0 - 1) * 100d);

                        if (n1 == n0) pch1 = 0d;

                        string levelFormatted;
                        string pchFormatted;
                        Program.ConvertToPrintFormat(level1, pch1, out levelFormatted, out pchFormatted);

                        this.windowEquationBrowserLabel.Inlines.Add(levelFormatted + " " + pchFormatted + " ");
                        this.windowEquationBrowserLabel.Inlines.Add(G.NL);
                    }
                }
            }
        }


        public void EquationBrowserSetEquationButtons(string eqName, string firstText, List<string> firstList)
        {
            this._activeEquation = eqName;
            this.windowEquationBrowserLabel.Inlines.Clear();

            List<ModelGamsEquation> equations = Program.modelGams.equationsByEqname[eqName];
            ModelGamsEquation equation = equations[0]; //always only 1

            //TODO: pooling a sum of ages into x[18..100] with the right aggregate color
            //TODO: do the coloring in parallel, so the colored list is shown when it is finished (shown all gray first)

            this.windowEquationBrowserLabel.Inlines.Add(firstText);
            //eb.windowEquationBrowserText.LineHeight = 12d;
            //eb.windowEquationBrowserText.LineStackingStrategy = System.Windows.LineStackingStrategy.BlockLineHeight;

            Random r = new Random();            

            this.windowEquationBrowserButtons.Children.Clear();
            this._buttons.Clear();
            TextBlock txt = new TextBlock();
            txt.Text = "Variables: ";
            txt.VerticalAlignment = VerticalAlignment.Center;
            this.windowEquationBrowserButtons.Children.Add(txt);
            
            foreach (string s in firstList)
            {
                if (s == "residual___") continue;
                string ss5 = s.Replace("¤[0]", "").Replace("¤", "");

                Button b = new Button();
                b.Content = ss5;
                var cStyle = new System.Windows.Style(typeof(Border));
                cStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(4)));
                b.Resources.Add(typeof(Border), cStyle);
                b.Padding = new Thickness(3 + 1, 1 + .5, 3 + 1, 2 + .5);
                b.Margin = new Thickness(4, 2.5, 4, 2.5);
                
                int ii = 255;                                
                b.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(ii), Convert.ToByte(ii), Convert.ToByte(ii)));

                int ii2 = 200;
                b.BorderBrush = new SolidColorBrush(Color.FromRgb(Convert.ToByte(ii2), Convert.ToByte(ii2), Convert.ToByte(ii2)));
                b.BorderThickness = new Thickness(1);
                b.Click += this.OnButtonClick;

                b.MouseEnter += this.OnButtonMouseEnter;
                b.MouseLeave += this.OnButtonMouseLeave;
                this.windowEquationBrowserButtons.Children.Add(b);
                _buttons.Add(ss5, b);
            }

            if (true)
            {
                string op = "d";
                GekkoTime per1 = new GekkoTime(EFreq.A, 2020, 1);                
                string residualName = "residual___";
                int funcCounter = 0;

                //fixme: [0] must be counter
                DecompData dd = Gekko.Decomp.DecompLowLevel(per1, per1, equation.expressions[0], Gekko.Decomp.DecompBanks(op), residualName, ref funcCounter);

                double max = 0d;
                foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                {
                    double v = kvp.Value.GetDataSimple(per1);
                    if (G.isNumericalError(v)) v = 0d;
                    else v = Math.Abs(v);
                    max = Math.Max(v, max);
                }

                foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                {
                    string ss5 = Program.DecompGetNameFromContrib(kvp.Key);
                    double v = kvp.Value.GetDataSimple(per1);
                    Button b = _buttons[ss5];
                    int i1 = 240;
                    int i2 = 255;
                    int ii = i1 + (int)((i2 - i1) * Math.Abs(v) / max);
                    b.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(ii), Convert.ToByte(ii), Convert.ToByte(ii)));
                }
            }


        }
    }

    public class EquationListItem
    {
        public EquationListItem(string name, string sub, string dep, string lhs, string per, string vars, string lineColor)
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

        public string Dep { get; set; }
        
        public string Lhs { get; set; }

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
