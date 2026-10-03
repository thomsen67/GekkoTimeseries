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
    /// Bar charts per period, stacked vertically:
    /// red (only for areas with 2 or 3 banks): number of deviating series; bars can be clicked, raising BarClicked(period).
    /// blue (always): number of series with a missing value (in any of the area's banks).
    /// </summary>
    public sealed class HistogramView : Canvas
    {
        public event Action<int> BarClicked;

        static readonly Brush BarBrush = BankColors.FromRgb(0xB8, 0x45, 0x3C);
        static readonly Brush BarHoverBrush = BankColors.FromRgb(0x7A, 0x22, 0x1C);
        static readonly Brush MissingBrush = BankColors.FromRgb(0x2E, 0x6D, 0xB4);
        static readonly Brush MissingHoverBrush = BankColors.FromRgb(0x1A, 0x44, 0x75);
        static readonly Brush AxisBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        static readonly Brush GridBrush = BankColors.FromRgb(0xE6, 0xE6, 0xE6);
        static readonly Brush TextBrush = BankColors.FromRgb(0x44, 0x44, 0x44);
        static readonly Brush MutedBrush = BankColors.FromRgb(0x88, 0x88, 0x88);

        const double PanelGap = 10;

        CompareFreq freq;
        int start;
        string deviationTitle;
        int[] deviationCounts;   // null: no red chart
        string missingTitle;
        int[] missingCounts;     // null together with deviationCounts: show the message
        string message = "Click an area or a number in the diagram to see deviations and missing values per period.";

        public HistogramView()
        {
            Background = Brushes.Transparent;
            ClipToBounds = true;
            SizeChanged += (s, e) => Rebuild();
        }

        /// <summary>
        /// Both arrays start at firstPeriod. Pass devCounts = null for an area with one bank only
        /// (nothing to compare): then only the blue chart is shown, using the full height.
        /// </summary>
        public void SetData(CompareFreq newFreq, int firstPeriod, string devTitle, int[] devCounts, string missTitle, int[] missCounts)
        {
            freq = newFreq;
            start = firstPeriod;
            deviationTitle = devTitle ?? "";
            deviationCounts = devCounts;
            missingTitle = missTitle ?? "";
            missingCounts = missCounts;
            Rebuild();
        }

        public void ShowMessage(string text)
        {
            deviationCounts = null;
            missingCounts = null;
            message = text ?? "";
            Rebuild();
        }

        void Rebuild()
        {
            Children.Clear();
            double width = ActualWidth, height = ActualHeight;
            if (width < 100 || height < 80) return;

            if (deviationCounts == null && missingCounts == null)
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

            if (deviationCounts != null && missingCounts != null)
            {
                double h = (height - PanelGap) / 2;
                DrawPanel(0, h, width, deviationTitle, deviationCounts, true);
                DrawPanel(h + PanelGap, h, width, missingTitle, missingCounts, false);
            }
            else if (deviationCounts != null)
            {
                DrawPanel(0, height, width, deviationTitle, deviationCounts, true);
            }
            else
            {
                DrawPanel(0, height, width, missingTitle, missingCounts, false);
            }
        }

        /// <summary>One chart in the band y0..y0+bandHeight.</summary>
        void DrawPanel(double y0, double bandHeight, double width, string title, int[] counts, bool deviations)
        {
            const double left = 52, right = 16, top = 26, bottom = 26;
            double pw = width - left - right, ph = bandHeight - top - bottom;
            if (pw < 40 || ph < 20) return;
            double plotTop = y0 + top;

            Add(new TextBlock
            {
                Text = title,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = TextBrush,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Width = width - left
            }, left, y0 + 3);

            int n = counts.Length;
            if (n == 0 || counts.All(c => c == 0))
            {
                double baseY = Math.Round(plotTop + ph) + 0.5;
                Children.Add(new Line { X1 = left, X2 = left + pw, Y1 = baseY, Y2 = baseY, Stroke = AxisBrush, StrokeThickness = 1 });
                var none = new TextBlock
                {
                    Text = deviations ? "No deviations in the compared periods." : "No missing values in the compared periods.",
                    FontSize = 12,
                    Foreground = MutedBrush,
                    Width = pw,
                    TextAlignment = TextAlignment.Center
                };
                Add(none, left, plotTop + ph / 2 - 8);
                return;
            }

            // Y axis: series count
            int max = Math.Max(1, counts.Max());
            double step = Math.Max(1, NiceStep(max / Math.Max(2.0, Math.Min(5.0, ph / 25.0))));
            double yMax = Math.Ceiling(max / step) * step;
            for (double v = 0; v <= yMax + 1e-9; v += step)
            {
                double y = Math.Round(plotTop + ph - v / yMax * ph) + 0.5;
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
            double slot = pw / n;
            double barWidth = slot >= 4 ? slot * 0.75 : Math.Max(1, slot);
            Brush normal = deviations ? BarBrush : MissingBrush;
            Brush hover = deviations ? BarHoverBrush : MissingHoverBrush;
            for (int i = 0; i < n; i++)
            {
                if (counts[i] == 0) continue;
                int period = start + i;
                double h = counts[i] / yMax * ph;
                var bar = new Rectangle { Width = barWidth, Height = Math.Max(1, h), Fill = normal, IsHitTestVisible = false };
                Add(bar, left + i * slot + (slot - barWidth) / 2, plotTop + ph - h);

                var column = new Rectangle
                {
                    Width = Math.Max(1, slot),
                    Height = ph,
                    Fill = Brushes.Transparent,
                    ToolTip = PeriodText.Format(freq, period) + ": " + counts[i]
                              + (deviations
                                  ? (counts[i] == 1 ? " series deviates" : " series deviate") + "\nClick to show them in the Deviations tab"
                                  : (counts[i] == 1 ? " series with a missing value" : " series with missing values"))
                };
                column.MouseEnter += (o, e) => bar.Fill = hover;
                column.MouseLeave += (o, e) => bar.Fill = normal;
                if (deviations)
                {
                    column.Cursor = Cursors.Hand;
                    column.MouseLeftButtonUp += (o, e) => { e.Handled = true; BarClicked?.Invoke(period); };
                }
                Add(column, left + i * slot, plotTop);
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
                Children.Add(new Line { X1 = x, X2 = x, Y1 = plotTop + ph, Y2 = plotTop + ph + 4, Stroke = AxisBrush, StrokeThickness = 1 });
                Add(new TextBlock
                {
                    Text = year.ToString(CultureInfo.InvariantCulture),
                    FontSize = 11,
                    Foreground = MutedBrush,
                    Width = 50,
                    TextAlignment = TextAlignment.Center
                }, x - 25, plotTop + ph + 5);
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
