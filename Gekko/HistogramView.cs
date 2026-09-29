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
    /// Bar chart per period. Red: number of deviating series (bars can be clicked, raising BarClicked(period)).
    /// Blue: number of series with an observation, for areas with one bank only.
    /// </summary>
    public sealed class HistogramView : Canvas
    {
        public event Action<int> BarClicked;

        static readonly Brush BarBrush = BankColors.FromRgb(0xB8, 0x45, 0x3C);
        static readonly Brush BarHoverBrush = BankColors.FromRgb(0x7A, 0x22, 0x1C);
        static readonly Brush ObsBrush = BankColors.FromRgb(0x2E, 0x6D, 0xB4);
        static readonly Brush ObsHoverBrush = BankColors.FromRgb(0x1A, 0x44, 0x75);
        static readonly Brush AxisBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        static readonly Brush GridBrush = BankColors.FromRgb(0xE6, 0xE6, 0xE6);
        static readonly Brush TextBrush = BankColors.FromRgb(0x44, 0x44, 0x44);
        static readonly Brush MutedBrush = BankColors.FromRgb(0x88, 0x88, 0x88);

        string title = "";
        CompareFreq freq;
        int start;
        int[] counts;
        bool deviations = true;
        string message = "Click a deviation number in the diagram to see deviations per period.";

        public HistogramView()
        {
            Background = Brushes.Transparent;
            ClipToBounds = true;
            SizeChanged += (s, e) => Rebuild();
        }

        /// <summary>showsDeviations: red clickable bars (deviations) or blue bars (observations).</summary>
        public void SetData(string newTitle, CompareFreq newFreq, int firstPeriod, int[] newCounts, bool showsDeviations)
        {
            deviations = showsDeviations;
            title = newTitle ?? "";
            freq = newFreq;
            start = firstPeriod;
            counts = newCounts;
            Rebuild();
        }

        public void ShowMessage(string text)
        {
            counts = null;
            message = text ?? "";
            Rebuild();
        }

        void Rebuild()
        {
            Children.Clear();
            double width = ActualWidth, height = ActualHeight;
            if (width < 100 || height < 80) return;

            if (counts == null || counts.Length == 0)
            {
                var info = new TextBlock
                {
                    Text = message,
                    FontSize = 13,
                    Foreground = MutedBrush,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Width = width - 40
                };
                info.Measure(new Size(info.Width, double.PositiveInfinity));
                Add(info, 20, (height - info.DesiredSize.Height) / 2);
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
            int n = counts.Length;
            double slot = pw / n;
            double barWidth = slot >= 4 ? slot * 0.75 : Math.Max(1, slot);
            for (int i = 0; i < n; i++)
            {
                if (counts[i] == 0) continue;
                int period = start + i;
                double h = counts[i] / yMax * ph;
                Brush normal = deviations ? BarBrush : ObsBrush;
                Brush hover = deviations ? BarHoverBrush : ObsHoverBrush;
                var bar = new Rectangle { Width = barWidth, Height = Math.Max(1, h), Fill = normal, IsHitTestVisible = false };
                Add(bar, left + i * slot + (slot - barWidth) / 2, top + ph - h);

                var column = new Rectangle
                {
                    Width = Math.Max(1, slot),
                    Height = ph,
                    Fill = Brushes.Transparent,
                    ToolTip = PeriodText.Format(freq, period) + ": " + counts[i]
                              + (deviations
                                  ? (counts[i] == 1 ? " series deviates" : " series deviate") + "\nClick to show them in the Deviations tab"
                                  : (counts[i] == 1 ? " series with an observation" : " series with observations"))
                };
                column.MouseEnter += (o, e) => bar.Fill = hover;
                column.MouseLeave += (o, e) => bar.Fill = normal;
                if (deviations)
                {
                    column.Cursor = Cursors.Hand;
                    column.MouseLeftButtonUp += (o, e) => { e.Handled = true; BarClicked?.Invoke(period); };
                }
                Add(column, left + i * slot, top);
            }

            // X axis: one label per year, thinned so labels do not overlap
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

        static double NiceStep(double raw)
        {
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(raw)));
            double f = raw / magnitude;
            double nice = f <= 1 ? 1 : f <= 2 ? 2 : f <= 5 ? 5 : 10;
            return nice * magnitude;
        }

        static int NiceInt(int k)
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
