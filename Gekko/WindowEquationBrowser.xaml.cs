﻿using System;
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
        

    public partial class WindowEquationBrowser
    {

        public string _activeEquation = null; //this always has a non-null value
        public string _activeVariable = null; //this may be null, if no variable button is active, else it has a value.
        //public GekkoTime _t1 = GekkoTime.tNull;
        //public GekkoTime _t2 = GekkoTime.tNull;
        public GekkoDictionary<string, ToggleButton> _buttons = new GekkoDictionary<string, ToggleButton>(StringComparer.OrdinalIgnoreCase);        
        public O.Find findOptions = null;
        //public DecompOptions2 decompOptions2 = null;

        public WindowEquationBrowser(O.Find o)
        {
            this.findOptions = o;
            InitializeComponent();
            this.windowEquationBrowserListView.SelectedIndex = 0;
            this.windowEquationBrowserListView.Focus();            
        }

        public void OnVariableButtonToggle(object sender, RoutedEventArgs e)
        {
            ToggleButton b = sender as ToggleButton;
            string s = b.Content.ToString();
            this.EquationBrowserSetLabel(s);
            this._activeVariable = s;
        }

        public void OnVariableButtonUntoggle(object sender, RoutedEventArgs e)
        {            
            this._activeVariable = null;
            this.EquationBrowserSetEquation(_activeEquation, this.findOptions.decompOptions2.showTime, this.findOptions.t0);
        }

        public void OnVariableButtonEnter(object sender, MouseEventArgs e)
        {            
            ToggleButton b = sender as ToggleButton;
            string s = b.Content.ToString();
            this.EquationBrowserSetLabel(s);
        }

        public void OnVariableButtonLeave(object sender, MouseEventArgs e)
        {            
            ToggleButton b = sender as ToggleButton;
            string s = b.Content.ToString();
            string ss = null;
            if (_activeVariable != null)
            {
                this.EquationBrowserSetLabel(_activeVariable);
            }
            else
            {                
                this.EquationBrowserSetEquation(_activeEquation, this.findOptions.decompOptions2.showTime, this.findOptions.t0);
            }
        }

        private void OnEquationListSelectLine(object sender, SelectionChangedEventArgs e)
        {
            EquationListItem item = e.AddedItems[0] as EquationListItem;            
            this.EquationBrowserSetEquationAndButtons(item.fullName, this.findOptions.decompOptions2.showTime, this.findOptions.t0);
            this._activeEquation = item.fullName;            
        }

        private void EquationBrowserSetEquationAndButtons(string eqName, bool showTime, GekkoTime t0)
        {            
            string s = Program.model.modelGamsScalar.GetEquationText(eqName, showTime, t0);
            this.EquationBrowserSetEquation(eqName, showTime, t0);
            int eqNumber = Program.model.modelGamsScalar.GetEqNumber(eqName);
            List<string> precedents = Program.model.modelGamsScalar.GetPrecedentsNames(eqNumber, showTime, t0);
            this.EquationBrowserSetEquationButtons(eqName, s, precedents);
        }

        private void OnEquationListMouseEnter(object sender, MouseEventArgs e)
        {
            ListViewItem x = sender as ListViewItem;
            EquationListItem item = x.Content as EquationListItem;                        
            this.EquationBrowserSetEquationAndButtons(item.fullName, this.findOptions.decompOptions2.showTime, this.findOptions.t0);
        }

        private void OnEquationListMouseLeave(object sender, MouseEventArgs e)
        {
            bool showTime = false;
            GekkoTime t0 = this.findOptions.decompOptions2.t1;            
            this.EquationBrowserSetEquationAndButtons(_activeEquation, this.findOptions.decompOptions2.showTime, this.findOptions.t0);
            this._activeVariable = null;  //if a variable is selected/fixed, this is removed when hovering over equ list            
        }

        public void EquationBrowserSetEquation(string eq, bool showTime, GekkoTime t0)
        {            
            string s = Model.LayoutEquationText(eq, Program.model.modelGamsScalar.GetEquationText(eq, showTime, t0), showTime, t0);
            this.windowEquationBrowserLabel.Inlines.Clear();
            this.windowEquationBrowserLabel.Inlines.Add(s);
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
            List<string> ss = Program.GetVariableExplanation(variableName, variableName, true, true, this.findOptions.decompOptions2.t1, this.findOptions.decompOptions2.t2, null);
            this.windowEquationBrowserLabel.Inlines.Clear();
            foreach (string s in ss)
            {
                this.windowEquationBrowserLabel.Inlines.Add(s + G.NL);
            }
        }

        public void EquationBrowserSetEquationButtons(string eqName, string firstText, List<string> firstList)
        {
            EquationBrowserSetEquationButtons1(eqName, firstText, firstList);
            //Dispatching the color update
            //So first non-colored buttons are shown, and then the background thread colors them
            //If the coloring is very time-consuming, scrolling down with arrows may freeze a bit. To solve this,
            //a background worker thread that is no longer relevant would need to be killed
            //or we could wait 0.5 second before any coloring?
            if (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() => EquationBrowserSetEquationButtons2(eqName)), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        public void EquationBrowserSetEquationButtons2(string eqName)
        {
            if (G.GetModelType() == EModelType.GAMSScalar)
            {
                //return;
                
                //List<ModelGamsEquation> equations = Program.model.modelGams.equationsByEqname[eqName];
                //ModelGamsEquation equation = equations[0]; //always only 1

                string residualName = "residual___";
                int funcCounter = 0;                
                DecompOperator operatorTemp = new DecompOperator(this.findOptions.decompOptions2.prtOptionLower);

                //!!! a bit of a waste of time, but is probably not significantly slowing
                //    down the FIND window.
                DecompOptions2 decompOptionsTemp = this.findOptions.decompOptions2.Clone();

                //decompOptionsTemp.link.Clear();
                //decompOptionsTemp.link.Add(new Link());

                decompOptionsTemp.new_from = new List<string>() { G.Chop_DimensionRemoveLast(eqName) };
                Decomp.PrepareEquations(decompOptionsTemp.t1, decompOptionsTemp.t2, operatorTemp, decompOptionsTemp);

                //HMMMM [0]
                //HMMMM [0]
                //HMMMM [0]
                //HMMMM [0]
                //HMMMM [0]
                //HMMMM [0]
                DecompStartHelper dsh = decompOptionsTemp.link[0].GAMS_dsh[0];

                //fixme: [0] must be counter

                GekkoTime gt1, gt2;
                DecompOperator op = Decomp.DecompMainInit(out gt1, out gt2, this.findOptions.t0, this.findOptions.t0, decompOptionsTemp.prtOptionLower);
                DecompData dd = Decomp.DecompLowLevelScalar(gt1, gt2, 0, dsh, operatorTemp, residualName, ref funcCounter);

                double max = 0d;
                foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                {
                    double v = kvp.Value.GetDataSimple(this.findOptions.t0);
                    if (G.isNumericalError(v)) v = 0d;
                    else v = Math.Abs(v);
                    max = Math.Max(v, max); 
                }

                foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                {
                    string ss5 = G.ReplaceTurtle(Program.DecompGetNameFromContrib(kvp.Key));
                    double v = kvp.Value.GetDataSimple(this.findOptions.t0);

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
                    DecompData dd = Gekko.Decomp.DecompLowLevel(this.findOptions.t0, this.findOptions.t0, equation.expressions[0], Gekko.Decomp.DecompBanks_OLDREMOVESOON(op), residualName, ref funcCounter);

                    double max = 0d;
                    foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                    {
                        double v = kvp.Value.GetDataSimple(this.findOptions.t0);
                        if (G.isNumericalError(v)) v = 0d;
                        else v = Math.Abs(v);
                        max = Math.Max(v, max);
                    }

                    foreach (KeyValuePair<string, Series> kvp in dd.cellsContribD.storage)
                    {
                        string ss5 = G.ReplaceTurtle(Program.DecompGetNameFromContrib(kvp.Key));
                        double v = kvp.Value.GetDataSimple(this.findOptions.t0);

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

        public void EquationBrowserSetEquationButtons1(string eqName, string firstText, List<string> firstList)
        {
            //this._activeEquation = eqName;
            //this._activeVariable = null;
            this.windowEquationBrowserLabel.Inlines.Clear();

            string s7 = Model.LayoutEquationText(eqName, firstText, true, GekkoTime.tNull);
            this.windowEquationBrowserLabel.Inlines.Add(s7);

            //this.windowEquationBrowserLabel.Inlines.Add(firstText);

            //TODO: pooling a sum of ages into x[18..100] with the right aggregate color
            //TODO: do the coloring in parallel, so the colored list is shown when it is finished (shown all gray first)


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
                string ss5 = G.ReplaceTurtle(s);

                //Button b = new Button();

                ToggleButton b = new ToggleButton();

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
        public EquationListItem(string name, string sub, string dep, string lhs, string per, string vars, string lineColor, bool isSelected, string fullName2)
        {
            Name = name;
            Sub = sub;
            Dep = dep;
            Lhs = lhs;
            Per = per;
            Vars = vars;
            LineColor = lineColor;
            isSelected = isSelected;
            fullName = fullName2;
        }

        public string Name { get; set; }

        public string Sub { get; set; }

        public string Dep { get; set; }
        
        public string Lhs { get; set; }

        public string Per { get; set; }

        public string Vars { get; set; }

        public string LineColor { get; set; }

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
