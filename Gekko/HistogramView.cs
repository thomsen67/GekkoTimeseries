using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gekko
{
    /// <summary>
    /// Bar chart per period over the whole universal period: the number of series found by the current check
    /// (series that differ, with missing values, or with zeroes).
    /// The selected periods (the period filter) are highlighted; bars outside them are paler.
    /// Click a bar: BarClicked(period, modifiers). Double-click: BarDoubleClicked(period).
    /// </summary>
    public sealed class HistogramView : Canvas
    {
        public event Action<int, ModifierKeys> BarClicked;
        public event Action<int> BarDoubleClicked;

        static readonly Brush DevBrush = BankColors.FromRgb(0xB8, 0x45, 0x3C);
        static readonly Brush DevHoverBrush = BankColors.FromRgb(0x7A, 0x22, 0x1C);
        static readonly Brush DevPaleBrush = BankColors.FromRgb(0xE3, 0xB5, 0xB1);
        static readonly Brush SelectionBand = BankColors.FromArgb(0x22, 0x60, 0x60, 0x60);
        static readonly Brush AxisBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        static readonly Brush GridBrush = BankColors.FromRgb(0xE6, 0xE6, 0xE6);
        static readonly Brush TextBrush = BankColors.FromRgb(0x44, 0x44, 0x44);
        static readonly Brush MutedBrush = BankColors.FromRgb(0x88, 0x88, 0x88);

        string title = "";
        CompareFreq freq;
        int start;
        int[] counts;
        string itemPhrase = "series";
        Func<int, string> extraTip;
        int? highlightFrom;
        int? highlightTo;
        string message = "Click a number, an area or a bank in the diagram.";

        public HistogramView()
        {
            Background = Brushes.Transparent;
            ClipToBounds = true;
            SizeChanged += (s, e) => Rebuild();
        }

        /// <summary>
        /// counts start at firstPeriod. phrase: what a bar counts, e.g. "series with missing values" (for tooltips).
        /// tip (may be null) adds a line to a bar's tooltip. highlightFrom/To: the selected periods, or null.
        /// </summary>
        public void SetData(string newTitle, CompareFreq newFreq, int firstPeriod, int[] newCounts, string phrase,
                            Func<int, string> tip, int? from, int? to)
        {
            itemPhrase = phrase ?? "series";
            title = newTitle ?? "";
            freq = newFreq;
            start = firstPeriod;
            counts = newCounts;
            extraTip = tip;
            highlightFrom = from;
            highlightTo = to;
            Rebuild();
        }

        public void ShowMessage(string text)
        {
            counts = null;
            message = text ?? "";
            Rebuild();
        }

        bool IsHighlighted(int period)
        {
            return !highlightFrom.HasValue || (period >= highlightFrom.Value && period <= highlightTo.Value);
        }

        void Rebuild()
        {
            Children.Clear();
            double width = ActualWidth, height = ActualHeight;
            if (width < 100 || height < 80) return;

            if (counts == null || counts.Length == 0)
            {
                AddCenteredText(message, 13, 20, width - 40, height / 2);
                return;
            }

            const double left = 52, right = 16, top = 30, bottom = 30;
            double pw = width - left - right, ph = height - top - bottom;
            if (pw < 40 || ph < 30) return;

            Add(new TextBlock
            {
                Text = title,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = TextBrush,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Width = width - left
            }, left, 4);

            int n = counts.Length;
            double slot = pw / n;

            // The selected periods as a light band behind the bars.
            if (highlightFrom.HasValue)
            {
                int a = Math.Max(0, highlightFrom.Value - start), b = Math.Min(n - 1, highlightTo.Value - start);
                if (a <= b)
                    Add(new Rectangle { Width = (b - a + 1) * slot, Height = ph, Fill = SelectionBand, IsHitTestVisible = false },
                        left + a * slot, top);
            }

            if (counts.All(c => c == 0))
            {
                double baseY = Math.Round(top + ph) + 0.5;
                Children.Add(new Line { X1 = left, X2 = left + pw, Y1 = baseY, Y2 = baseY, Stroke = AxisBrush, StrokeThickness = 1 });
                AddCenteredText("No " + itemPhrase + " in this selection.", 12, left, pw, top + ph / 2);
                DrawYears(left, top, ph, slot, n);
                return;
            }

            // Y axis: series count
            int max = Math.Max(1, counts.Max());
            double step = Math.Max(1, NiceStep(max / 5.0));
            double yMax = Math.Ceiling(max / step) * step;
            for (double v = 0; v <= yMax + 1e-9; v += step)
            {
                double y = Math.Round(top + ph - v / yMax * ph) + 0.5;
                Children.Add(new Line { X1 = left, X2 = left + pw, Y1 = y, Y2 = y, Stroke = v == 0 ? AxisBrush : GridBrush, StrokeThickness = 1 });
                Add(new TextBlock
                {
                    Text = v.ToString("0", CultureInfo.InvariantCulture),
                    FontSize = 11,
                    Foreground = MutedBrush,
                    Width = left - 8,
                    TextAlignment = TextAlignment.Right
                }, 0, y - 8);
            }

            // Bars. A transparent full-height column per period makes thin (monthly) bars easy to hit.
            double barWidth = slot >= 4 ? slot * 0.75 : Math.Max(1, slot);
            Brush hover = DevHoverBrush;
            for (int i = 0; i < n; i++)
            {
                int period = start + i;
                Brush normal = IsHighlighted(period) ? DevBrush : DevPaleBrush;
                Rectangle bar = null;
                if (counts[i] > 0)
                {
                    double h = counts[i] / yMax * ph;
                    bar = new Rectangle { Width = barWidth, Height = Math.Max(1, h), Fill = normal, IsHitTestVisible = false };
                    Add(bar, left + i * slot + (slot - barWidth) / 2, top + ph - h);
                }

                string tip = PeriodText.Format(freq, period) + ": " + counts[i] + " " + itemPhrase;
                string more = extraTip?.Invoke(period);
                if (!string.IsNullOrEmpty(more)) tip += "\n" + more;
                tip += "\nClick: select this period (Shift: extend, Ctrl: add to the filter). Double-click: list the names.";

                var column = new Rectangle
                {
                    Width = Math.Max(1, slot),
                    Height = ph,
                    Fill = Brushes.Transparent,
                    Cursor = Cursors.Hand,
                    ToolTip = tip
                };
                if (bar != null)
                {
                    Rectangle b = bar;
                    column.MouseEnter += (o, e) => b.Fill = hover;
                    column.MouseLeave += (o, e) => b.Fill = normal;
                }
                column.MouseLeftButtonDown += (o, e) =>
                {
                    if (e.ClickCount != 2) return;
                    e.Handled = true;
                    BarDoubleClicked?.Invoke(period);
                };
                column.MouseLeftButtonUp += (o, e) => { e.Handled = true; BarClicked?.Invoke(period, Keyboard.Modifiers); };
                Add(column, left + i * slot, top);
            }

            DrawYears(left, top, ph, slot, n);
        }

        /// <summary>X axis: one label per year, thinned so labels do not overlap.</summary>
        void DrawYears(double left, double top, double ph, double slot, int n)
        {
            int periodsPerYear = PeriodText.PeriodsPerYear(freq);
            int firstYear = PeriodText.Year(freq, start);
            int lastYear = PeriodText.Year(freq, start + n - 1);
            int yearStep = NiceInt((int)Math.Ceiling(44 / (slot * periodsPerYear)));
            for (int year = firstYear; year <= lastYear; year++)
            {
                if (year % yearStep != 0) continue;
                int index = PeriodText.ToIndex(freq, year, 1) - start;
                if (index < 0 || index >= n) continue;
                double x = Math.Round(left + index * slot + (periodsPerYear == 1 ? slot / 2 : 0)) + 0.5;
                Children.Add(new Line { X1 = x, X2 = x, Y1 = top + ph, Y2 = top + ph + 4, Stroke = AxisBrush, StrokeThickness = 1 });
                Add(new TextBlock
                {
                    Text = year.ToString(CultureInfo.InvariantCulture),
                    FontSize = 11,
                    Foreground = MutedBrush,
                    Width = 50,
                    TextAlignment = TextAlignment.Center
                }, x - 25, top + ph + 5);
            }
        }

        void AddCenteredText(string text, double fontSize, double left, double width, double centerY)
        {
            var info = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                Foreground = MutedBrush,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Width = Math.Max(20, width),
                IsHitTestVisible = false
            };
            info.Measure(new Size(info.Width, double.PositiveInfinity));
            Add(info, left, centerY - info.DesiredSize.Height / 2);
        }

        internal static double NiceStep(double raw)
        {
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(raw)));
            double f = raw / magnitude;
            double nice = f <= 1 ? 1 : f <= 2 ? 2 : f <= 5 ? 5 : 10;
            return nice * magnitude;
        }

        internal static int NiceInt(int k)
        {
            foreach (int c in new[] { 1, 2, 5, 10, 20, 25, 50, 100 })
                if (c >= k) return c;
            return k;
        }

        void Add(UIElement element, double left, double top)
        {
            SetLeft(element, left);
            SetTop(element, top);
            Children.Add(element);
        }
    }
}
