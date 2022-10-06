using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for WindowEquationBrowser.xaml
    /// </summary>


    public partial class WindowFind
    {

        public string _activeEquation = null; //this always has a non-null value
        public string _activeVariable = null; //this may be null, if no variable button is active, else it has a value.
        //public GekkoTime _t1 = GekkoTime.tNull;
        //public GekkoTime _t2 = GekkoTime.tNull;
        public GekkoDictionary<string, ToggleButton> _buttons = new GekkoDictionary<string, ToggleButton>(StringComparer.OrdinalIgnoreCase);
        public DecompFind decompFind = null;
        //public DecompOptions2 decompOptions2 = null;

        public WindowFind(O.Find o)
        {            
            this.decompFind = o.decompFind;

            //HACKY:
            if (this.decompFind.decompOptions2.t0.IsNull()) this.decompFind.decompOptions2.t0 = o.t0;
            if (this.decompFind.decompOptions2.t1.IsNull()) this.decompFind.decompOptions2.t1 = o.t1;
            if (this.decompFind.decompOptions2.t2.IsNull()) this.decompFind.decompOptions2.t2 = o.t2;
            this.decompFind.decompOptions2.iv = o.iv;
            if (o.opt_prtcode != null) this.decompFind.decompOptions2.prtOptionLower = o.opt_prtcode.ToLower();
                        
            InitializeComponent();
            this.windowEquationBrowserListView.Focus();
            //
            //
            // HMMM, very difficult to get the first line in focus, even if it is selected in wpf (cf. #jk8dsfa7yauewfh)
            //
            //
            //this.windowEquationBrowserListView.ScrollIntoView(this.windowEquationBrowserListView.Items[0]);
            //this.windowEquationBrowserListView.SelectedIndex = 0;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            //var viewModel = DataContext as YourViewModel;
            //viewModel.YourCommand.Execute(null);
            if (e.Key == Key.Return)
            {
                CallDecomp(this._activeEquation);
            }
        }

        public void OnVariableButtonToggle(object sender, RoutedEventArgs e)
        {
            ToggleButton b = sender as ToggleButton;
            string s = ((TextBlock)b.Content).Text;
            this.EquationBrowserSetLabel(s);
            this._activeVariable = s;

            foreach (object o in this.windowEquationBrowserButtons.Children)
            {
                ToggleButton tb = o as ToggleButton;
                if (tb == null) continue;
                if (((TextBlock)tb.Content).Text == s) continue;
                tb.IsChecked = false;
            }

        }

        public void OnVariableButtonUntoggle(object sender, RoutedEventArgs e)
        {
            this._activeVariable = null;
            this.EquationBrowserSetEquation(_activeEquation, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t0);
        }

        public void OnVariableButtonEnter(object sender, MouseEventArgs e)
        {
            ToggleButton b = sender as ToggleButton;
            string s = ((TextBlock)b.Content).Text;
            if (s.Contains("[-"))
            {
                //lag
                int idx = s.IndexOf("[-");
                s = G.Substring(s, 0, idx - 1);
            }
            else if (s.Contains("[+"))
            {
                //lag
                int idx = s.IndexOf("[+");
                s = G.Substring(s, 0, idx - 1);
            }
            this.EquationBrowserSetLabel(s);
        }

        public void OnVariableButtonLeave(object sender, MouseEventArgs e)
        {
            ToggleButton b = sender as ToggleButton;
            string s = ((TextBlock)b.Content).Text;
            string ss = null;
            if (_activeVariable != null)
            {
                this.EquationBrowserSetLabel(_activeVariable);
            }
            else
            {
                this.EquationBrowserSetEquation(_activeEquation, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t0);
            }
        }

        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void OnEquationListLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            //if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            //{
            //    //ignore if combined with ctrl or alt, only a clean left-clik is a link
            //    return;
            //}
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            EquationListItem item = fe.DataContext as EquationListItem;            
            CallDecomp(item.fullName);
        }

        private void CallDecomp(string fullName)
        {
            string eqName = G.Chop_DimensionRemoveLast(fullName);

            O.Decomp2 decomp = new O.Decomp2();
            decomp.opt_prtcode = this.decompFind.decompOptions2.prtOptionLower;
            decomp.t1 = this.decompFind.decompOptions2.t1;
            decomp.t2 = this.decompFind.decompOptions2.t2;
            string varName = this.decompFind.decompOptions2.iv.list[0].ConvertToString();

            List select = new List(new List<string>() { varName });
            decomp.select = new List<IVariable>() { select };
            List from = new List(new List<string>() { eqName });
            decomp.from = new List<IVariable>() { from };
            List endo = new List(new List<string>() { varName });
            decomp.endo = new List<IVariable>() { endo };
            decomp.name = new ScalarString(eqName);

            //d.decompFind = this.decompFind;
            decomp.decompFind = this.decompFind.CreateChild(this.decompFind.decompOptions2.Clone(false), EDecompFindNavigation.Decomp, null);

            decomp.type = "ASTDECOMP3";  //else old style decomp is used...
            decomp.decompFind.decompOptions2.code = "decomp3 " + varName + " from " + eqName + " endo " + varName + ";";

            decomp.Exe();
        }

        private void OnEquationListSelectLine(object sender, SelectionChangedEventArgs e)
        {
            EquationListItem item = e.AddedItems[0] as EquationListItem;
            this.EquationBrowserSetButtons(item.fullName, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t0);
            this._activeEquation = item.fullName;
        }

        private void OnEquationListMouseEnter(object sender, MouseEventArgs e)
        {
            this.windowFindStatusBar.Text = Globals.windowFindStatusBarText;
            ListViewItem x = sender as ListViewItem;
            EquationListItem item = x.Content as EquationListItem;
            this.EquationBrowserSetButtons(item.fullName, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t0);
        }

        private void OnEquationListMouseLeave(object sender, MouseEventArgs e)
        {
            this.windowFindStatusBar.Text = "";
            bool showTime = false;
            GekkoTime t0 = this.decompFind.decompOptions2.t1;
            this.EquationBrowserSetButtons(_activeEquation, this.decompFind.decompOptions2.showTime, this.decompFind.decompOptions2.t0);
            this._activeVariable = null;  //if a variable is selected/fixed, this is removed when hovering over equ list            
        }

        private void EquationBrowserSetButtons(string eqName, bool showTime, GekkoTime t0)
        {
            this.EquationBrowserSetEquation(eqName, showTime, t0);
            int eqNumber = Program.model.modelGamsScalar.GetEqNumber(eqName);
            List<string> precedents = Program.model.modelGamsScalar.GetPrecedentsNames(eqNumber, showTime, t0);
            this.EquationBrowserSetButtons(eqName, precedents);
        }

        public void EquationBrowserSetEquation(string eq, bool showTime, GekkoTime t0)
        {
            string s = Model.GetEquationText(new List<string>() { eq }, showTime, t0);
            this.windowEquationBrowserLabel.Text = s;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        public void EquationBrowserSetLabel(string variableName)
        {
            List<string> ss = Program.GetVariableExplanation(variableName, variableName, true, true, this.decompFind.decompOptions2.t1, this.decompFind.decompOptions2.t2, null);
            string s7 = Stringlist.ExtractTextFromLines(ss).ToString();
            this.windowEquationBrowserLabel.Text = s7;
        }

        public void EquationBrowserSetButtons(string eqName, List<string> firstList)
        {
            EquationBrowserSetButtons1(eqName, firstList);
            //Dispatching the color update
            //So first non-colored buttons are shown, and then the background thread colors them
            //If the coloring is very time-consuming, scrolling down with arrows may freeze a bit. To solve this,
            //a background worker thread that is no longer relevant would need to be killed
            //or we could wait 0.5 second before any coloring?
            if (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() => EquationBrowserSetEquationButtonsColors(eqName)), System.Windows.Threading.DispatcherPriority.Background);
            }
            try
            {

            }
            catch
            {

            }

        }

        public void EquationBrowserSetEquationButtonsColors(string eqName)
        {
            try
            {
                if (G.GetModelType() == EModelType.GAMSScalar)
                {

                    string residualName = "residual___";
                    int funcCounter = 0;
                    DecompOperator operatorTemp = new DecompOperator(this.decompFind.decompOptions2.prtOptionLower);

                    //!!! a bit of a waste of time, but is probably not significantly slowing
                    //    down the FIND window.
                    DecompOptions2 decompOptionsTemp = this.decompFind.decompOptions2.Clone();

                    //decompOptionsTemp.link.Clear();
                    //decompOptionsTemp.link.Add(new Link());

                    decompOptionsTemp.new_from = new List<string>() { G.Chop_DimensionRemoveLast(eqName) };
                    Decomp.PrepareEquations(decompOptionsTemp.t1, decompOptionsTemp.t2, operatorTemp, decompOptionsTemp, false);

                    //HMMMM [0]
                    //HMMMM [0]
                    //HMMMM [0]
                    //HMMMM [0] link[0] is ok, but the other [0] is not.
                    //HMMMM [0]
                    //HMMMM [0]                
                    DecompStartHelper dsh = decompOptionsTemp.link[0].GAMS_dsh[0];

                    //fixme: [0] must be counter

                    GekkoTime gt1, gt2;
                    DecompOperator op = Decomp.DecompMainInit(out gt1, out gt2, this.decompFind.decompOptions2.t0, this.decompFind.decompOptions2.t0, decompOptionsTemp.prtOptionLower);
                    DecompData dd = Decomp.DecompLowLevelScalar(gt1, gt2, 0, dsh, operatorTemp, residualName, ref funcCounter);

                    double max = 0d;

                    foreach (KeyValuePair<string, Series> kvp in Decomp.GetDecompDatas(dd, op.type).storage)
                    {
                        double v = kvp.Value.GetDataSimple(this.decompFind.decompOptions2.t0);
                        if (G.isNumericalError(v)) v = 0d;
                        else v = Math.Abs(v);
                        max = Math.Max(v, max);
                    }

                    foreach (KeyValuePair<string, Series> kvp in Decomp.GetDecompDatas(dd, op.type).storage)
                    {
                        string ss5 = G.ReplaceTurtle(Program.DecompGetNameFromContrib(kvp.Key));
                        double v = kvp.Value.GetDataSimple(this.decompFind.decompOptions2.t0);

                        ToggleButton b = null;
                        _buttons.TryGetValue(ss5, out b);
                        if (b != null)
                        {
                            int i1 = 240;
                            int i2 = 255;
                            int ii = i2;
                            if (max == 0d)
                            {
                                //do nothing
                            }
                            else
                            {
                                ii = i2 - (int)((i2 - i1) * Math.Abs(v) / max);
                            }
                            if (ii < 0 || ii > 255)
                            {
                                if (Globals.runningOnTTComputer) MessageBox.Show("ERROR: byte value is " + ii);
                            }
                            b.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(ii), Convert.ToByte(ii), Convert.ToByte(ii)));
                        }
                    }
                }
                else if (G.GetModelType() == EModelType.GAMSRaw)
                {
                    List<ModelGamsEquation> equations = Program.model.modelGams.equationsByEqname[eqName];
                    ModelGamsEquation equation = equations[0]; //always only 1

                    {

                        DecompOperator op = new DecompOperator("d");

                        string residualName = "residual___";
                        int funcCounter = 0;

                        string s1 = Program.EquationLhsRhs(equation.lhs, equation.rhs, true) + ";";
                        if (equation.expressions == null || equation.expressions.Count == 0)
                        {
                            Globals.expressions = null;  //maybe not necessary
                            Program.CallEval(equation.conditionals, s1);
                            equation.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here
                            Globals.expressions = null;  //maybe not necessary   
                        }

                        if (equation.expressions.Count != equation.expressionVariablesWithSets.Count)
                        {
                            new Error("Internal error #8973428374");
                        }

                        //fixme: [0] must be counter
                        DecompData dd = Gekko.Decomp.DecompLowLevel(this.decompFind.decompOptions2.t0, this.decompFind.decompOptions2.t0, equation.expressions[0], Gekko.Decomp.DecompBanks_OLDREMOVESOON(op), residualName, ref funcCounter);

                        double max = 0d;
                        foreach (KeyValuePair<string, Series> kvp in Decomp.GetDecompDatas(dd, op.type).storage)
                        {
                            double v = kvp.Value.GetDataSimple(this.decompFind.decompOptions2.t0);
                            if (G.isNumericalError(v)) v = 0d;
                            else v = Math.Abs(v);
                            max = Math.Max(v, max);
                        }

                        foreach (KeyValuePair<string, Series> kvp in Decomp.GetDecompDatas(dd, op.type).storage)
                        {
                            string ss5 = G.ReplaceTurtle(Program.DecompGetNameFromContrib(kvp.Key));
                            double v = kvp.Value.GetDataSimple(this.decompFind.decompOptions2.t0);

                            ToggleButton b = null;
                            _buttons.TryGetValue(ss5, out b);
                            if (b != null)
                            {
                                int i1 = 240;
                                int i2 = 255;
                                int ii = i2 - (int)((i2 - i1) * Math.Abs(v) / max);
                                b.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(ii), Convert.ToByte(ii), Convert.ToByte(ii)));
                            }
                        }
                    }
                }
            }
            catch
            {
                if (Globals.runningOnTTComputer) new Writeln("TTH: Button color problem...");
            }
        }

        public void EquationBrowserSetButtons1(string eqName, List<string> firstList)
        {

            this.windowEquationBrowserButtons.Children.Clear();
            this._buttons.Clear();
            TextBlock txt = new TextBlock();
            txt.Text = "Variables: ";
            txt.VerticalAlignment = VerticalAlignment.Center;
            this.windowEquationBrowserButtons.Children.Add(txt);

            foreach (string s in firstList)
            {
                if (s == "residual___") continue;
                string ss5 = G.ReplaceTurtle(s);

                TextBlock tb = new TextBlock();
                tb.Text = ss5;
                ToggleButton b = new ToggleButton();
                b.Content = tb;

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
                //b.Click += this.OnButtonClick;

                b.MouseEnter += this.OnVariableButtonEnter;
                b.MouseLeave += this.OnVariableButtonLeave;
                b.Checked += this.OnVariableButtonToggle;
                b.Unchecked += this.OnVariableButtonUntoggle;

                this.windowEquationBrowserButtons.Children.Add(b);
                _buttons.Add(ss5, b);
            }


        }
    }

    public class EquationListItem
    {
        public EquationListItem(string name, string sub, string dep, string lhs, string per, string vars, string lineColor, string textColor, bool isSelected, string fullName2)
        {
            Name = name;
            Sub = sub;
            Dep = dep;
            Lhs = lhs;
            Per = per;
            Vars = vars;
            LineColor = lineColor;
            TextColor = textColor;
            IsSelected = isSelected;
            fullName = fullName2;
        }

        public string Name { get; set; }

        public string Sub { get; set; }

        public string Dep { get; set; }

        public string Lhs { get; set; }

        public string Per { get; set; }

        public string Vars { get; set; }

        public string LineColor { get; set; }

        public string TextColor { get; set; }

        public bool IsSelected { get; set; }

        public string fullName = null;
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
            _itemHandler = Globals.itemHandler;
        }

        public List<EquationListItem> Items
        {
            get { return _itemHandler.Items; }
        }
    }
}
