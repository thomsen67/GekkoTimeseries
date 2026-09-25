using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gekko
{
    /// <summary>Colors for bank 1, 2 and 3, used both in the list and in the diagram.</summary>
    public static class BankColors
    {
        static readonly Color[] Palette =
        {
            Color.FromRgb(0x2E, 0x6D, 0xB4),  // bank 1: blue
            Color.FromRgb(0xC9, 0x7A, 0x1E),  // bank 2: amber
            Color.FromRgb(0x3D, 0x8C, 0x4E)   // bank 3: green
        };
        static readonly Color Grey = Color.FromRgb(0xA6, 0xA6, 0xA6);

        public static Color ColorOf(int slot)
        {
            return slot >= 0 && slot < Palette.Length ? Palette[slot] : Grey;
        }

        public static Brush Solid(int slot)
        {
            return Frozen(new SolidColorBrush(ColorOf(slot)));
        }

        public static Brush Translucent(int slot, byte alpha)
        {
            Color c = ColorOf(slot);
            return Frozen(new SolidColorBrush(Color.FromArgb(alpha, c.R, c.G, c.B)));
        }

        public static Brush FromRgb(byte r, byte g, byte b)
        {
            return Frozen(new SolidColorBrush(Color.FromRgb(r, g, b)));
        }

        public static Brush FromArgb(byte a, byte r, byte g, byte b)
        {
            return Frozen(new SolidColorBrush(Color.FromArgb(a, r, g, b)));
        }

        static Brush Frozen(SolidColorBrush brush)
        {
            brush.Freeze();
            return brush;
        }
    }

    /// <summary>
    /// Venn diagram for 2 or 3 banks. Bank 1 is bottom-left, bank 2 bottom-right, bank 3 on top,
    /// so the 2-bank diagram is the 3-bank diagram without the top circle.
    /// Raises AreaClicked(mask, deviationsClicked) and BankClicked(slot).
    /// </summary>
    public sealed class VennView : Canvas
    {
        public event Action<int, bool> AreaClicked;
        public event Action<int> BankClicked;

        // Circle geometry in units of the radius, centered on the centroid of the circle centers.
        const double H3 = 0.288675;  // 1 / (2 * sqrt 3)
        const double T3 = 0.577350;  // 1 / sqrt 3

        static readonly Brush HoverFill = BankColors.FromArgb(0x2A, 0, 0, 0);
        static readonly Brush SelectedFill = BankColors.FromArgb(0x16, 0, 0, 0);
        static readonly Brush SelectedStroke = BankColors.FromRgb(0x33, 0x33, 0x33);
        static readonly Brush HiddenFill = BankColors.FromArgb(0x14, 0x80, 0x80, 0x80);
        static readonly Brush HiddenStroke = BankColors.FromRgb(0xB4, 0xB4, 0xB4);
        static readonly Brush TotalBrush = BankColors.FromRgb(0x1F, 0x4E, 0x79);
        static readonly Brush DeviationBrush = BankColors.FromRgb(0xB0, 0x30, 0x30);
        static readonly Brush MutedBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        static readonly Brush TextBrush = BankColors.FromRgb(0x44, 0x44, 0x44);
        static readonly Brush SelectedTextBack = BankColors.FromRgb(0xFF, 0xF0, 0xB0);

        VennResult result;
        string emptyMessage = "";
        int selectedMask;
        bool selectedDeviations;
        readonly Dictionary<int, Path> areaPaths = new Dictionary<int, Path>();
        TextBlock caption;
        string idleCaption = "";

        public VennView()
        {
            Background = Brushes.Transparent;
            ClipToBounds = true;
            SizeChanged += (s, e) => Rebuild();
        }

        public void SetResult(VennResult newResult, string messageWhenEmpty)
        {
            result = newResult;
            emptyMessage = messageWhenEmpty ?? "";
            Rebuild();
        }

        public void SetSelection(int mask, bool deviations)
        {
            selectedMask = mask;
            selectedDeviations = deviations;
            Rebuild();
        }

        void Rebuild()
        {
            Children.Clear();
            areaPaths.Clear();
            caption = null;

            double width = ActualWidth, height = ActualHeight;
            if (width < 120 || height < 120) return;
            if (result == null)
            {
                AddCenteredMessage(emptyMessage, width, height);
                return;
            }

            bool three = result.SlotCount == 3;
            Point[] unit = three
                ? new[] { new Point(-0.5, H3), new Point(0.5, H3), new Point(0, -T3) }
                : new[] { new Point(-0.5, 0), new Point(0.5, 0) };
            double yMin = three ? -(T3 + 1) : -1;
            double yMax = three ? H3 + 1 : 1;

            // Room for bank labels left/right (banks 1 and 2) and on top (bank 3).
            const double side = 230, bottom = 30;
            double top = three ? 62 : 30;
            double r = Math.Min((width - 2 * side) / 3.0, (height - top - bottom) / (yMax - yMin));
            r = Math.Max(45, Math.Min(r, 200));
            double ox = width / 2;
            double oy = top + ((height - top - bottom) - (yMax - yMin) * r) / 2 - yMin * r;
            Func<Point, Point> toPixel = p => new Point(ox + p.X * r, oy + p.Y * r);
            Point[] centers = unit.Select(toPixel).ToArray();

            string freqName = result.Freq.ToString();
            Add(new TextBlock { Text = freqName + " series, universal period " + result.SpanText, FontSize = 12.5, Foreground = TextBrush }, 2, 0);

            // Circles
            for (int s = 0; s < result.SlotCount; s++)
            {
                bool on = result.Active[s];
                var circle = new Ellipse
                {
                    Width = 2 * r,
                    Height = 2 * r,
                    Fill = on ? BankColors.Translucent(s, 0x30) : HiddenFill,
                    Stroke = on ? BankColors.Solid(s) : HiddenStroke,
                    StrokeThickness = on ? 2 : 1.5,
                    IsHitTestVisible = false
                };
                if (!on) circle.StrokeDashArray = new DoubleCollection { 4, 3 };
                Add(circle, centers[s].X - r, centers[s].Y - r);
            }

            // Areas as exact shapes, so hovering shows precisely which region a number belongs to.
            Geometry[] circles = centers.Select(c => (Geometry)new EllipseGeometry(c, r, r)).ToArray();
            foreach (VennArea area in result.Areas.Values)
            {
                int mask = area.Mask;
                bool selected = mask == selectedMask;
                var path = new Path
                {
                    Data = AreaGeometry(mask, circles),
                    Fill = selected ? SelectedFill : Brushes.Transparent,
                    Stroke = selected ? SelectedStroke : null,
                    StrokeThickness = 2,
                    Cursor = Cursors.Hand
                };
                path.MouseEnter += (o, e) => Hover(mask, true);
                path.MouseLeave += (o, e) => Hover(mask, false);
                path.MouseLeftButtonUp += (o, e) => { e.Handled = true; AreaClicked?.Invoke(mask, false); };
                areaPaths[mask] = path;
                Children.Add(path);
            }

            // Numbers
            foreach (VennArea area in result.Areas.Values)
            {
                Point p = toPixel(LabelPoint(three, area.Mask));
                FrameworkElement label = BuildAreaLabel(area);
                label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Add(label, p.X - label.DesiredSize.Width / 2, p.Y - label.DesiredSize.Height / 2);
            }

            // Bank labels
            for (int s = 0; s < result.SlotCount; s++) AddBankTag(s, centers[s], r, three);

            idleCaption = three
                ? "Hover over an area to see what it holds. Click a number for details. Click a bank label to hide or show that bank."
                : "Hover over an area to see what it holds. Click a number for details.";
            caption = new TextBlock { FontSize = 12, Foreground = TextBrush, TextTrimming = TextTrimming.CharacterEllipsis, Width = width - 4 };
            caption.Text = SelectionCaption() ?? idleCaption;
            Add(caption, 2, height - 20);
        }

        Geometry AreaGeometry(int mask, Geometry[] circles)
        {
            Geometry g = null;
            for (int s = 0; s < circles.Length; s++)
            {
                if ((mask & (1 << s)) == 0) continue;
                g = g == null ? circles[s] : new CombinedGeometry(GeometryCombineMode.Intersect, g, circles[s]);
            }
            for (int s = 0; s < circles.Length; s++)
            {
                if ((mask & (1 << s)) != 0 || !result.Active[s]) continue;
                g = new CombinedGeometry(GeometryCombineMode.Exclude, g, circles[s]);
            }
            return g;
        }

        /// <summary>Label position of each area, in radius units relative to the centroid.</summary>
        static Point LabelPoint(bool three, int mask)
        {
            if (!three)
            {
                switch (mask)
                {
                    case 1: return new Point(-1.0, 0);
                    case 2: return new Point(1.0, 0);
                    default: return new Point(0, 0);
                }
            }
            switch (mask)
            {
                case 1: return new Point(-0.935, 0.54);
                case 2: return new Point(0.935, 0.54);
                case 4: return new Point(0, -1.08);
                case 3: return new Point(0, 0.78);
                case 5: return new Point(-0.675, -0.39);
                case 6: return new Point(0.675, -0.39);
                default: return new Point(0, 0.02);
            }
        }

        FrameworkElement BuildAreaLabel(VennArea area)
        {
            int mask = area.Mask;
            bool selected = mask == selectedMask;
            var panel = new StackPanel { Background = Brushes.Transparent, Cursor = Cursors.Hand };

            int total = area.Names.Count;
            var totalText = new TextBlock
            {
                Text = total.ToString(CultureInfo.InvariantCulture),
                FontSize = 17,
                FontWeight = FontWeights.SemiBold,
                Foreground = TotalBrush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(4, 0, 4, 0),
                ToolTip = total + " series. Click to list their names."
            };
            if (selected && !selectedDeviations) totalText.Background = SelectedTextBack;
            MakeClickable(totalText, () => AreaClicked?.Invoke(mask, false));
            panel.Children.Add(totalText);

            if (area.IsComparison)
            {
                int dev = area.Deviations.Count;
                var devText = new TextBlock
                {
                    Text = dev + " dev.",
                    FontSize = 12,
                    Foreground = dev > 0 ? DeviationBrush : MutedBrush,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(4, 0, 4, 1)
                };
                if (dev > 0)
                {
                    devText.ToolTip = area.Slots.Length == 3
                        ? dev + " of " + total + " series are not equal in all three banks. Click for details."
                        : dev + " of " + total + " series deviate between bank " + (area.Slots[0] + 1) + " and bank " + (area.Slots[1] + 1) + ". Click for details.";
                    if (selected && selectedDeviations) devText.Background = SelectedTextBack;
                    MakeClickable(devText, () => AreaClicked?.Invoke(mask, true));
                }
                else
                {
                    devText.ToolTip = "No deviations";
                }
                panel.Children.Add(devText);
            }

            panel.MouseEnter += (o, e) => Hover(mask, true);
            panel.MouseLeave += (o, e) => Hover(mask, false);
            panel.MouseLeftButtonUp += (o, e) => { e.Handled = true; AreaClicked?.Invoke(mask, false); };
            return panel;
        }

        static void MakeClickable(TextBlock text, Action onClick)
        {
            text.Cursor = Cursors.Hand;
            text.MouseEnter += (o, e) => text.TextDecorations = TextDecorations.Underline;
            text.MouseLeave += (o, e) => text.TextDecorations = null;
            text.MouseLeftButtonUp += (o, e) => { e.Handled = true; onClick(); };
        }

        void AddBankTag(int s, Point center, double r, bool three)
        {
            bool on = result.Active[s];
            CompareBank bank = result.Banks[s];

            var badge = new Border
            {
                Width = 20,
                Height = 20,
                CornerRadius = new CornerRadius(10),
                Background = on ? BankColors.Solid(s) : HiddenStroke,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 1, 7, 0),
                Child = new TextBlock
                {
                    Text = (s + 1).ToString(CultureInfo.InvariantCulture),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            var lines = new StackPanel();
            lines.Children.Add(new TextBlock
            {
                Text = bank.Name,
                FontWeight = FontWeights.SemiBold,
                Foreground = on ? TotalBrush : MutedBrush,
                MaxWidth = 165,
                TextTrimming = TextTrimming.CharacterEllipsis
            });
            lines.Children.Add(new TextBlock
            {
                Text = on ? result.BankSpanText(s) : "hidden, click to show",
                FontSize = 11,
                Foreground = MutedBrush
            });
            var row = new StackPanel { Orientation = Orientation.Horizontal };
            row.Children.Add(badge);
            row.Children.Add(lines);

            var tag = new Border
            {
                Child = row,
                Padding = new Thickness(6, 4, 9, 4),
                CornerRadius = new CornerRadius(3),
                BorderThickness = new Thickness(1),
                BorderBrush = on ? BankColors.Solid(s) : HiddenStroke,
                Background = Brushes.White
            };

            string tip = bank.FilePath ?? (bank.Name + " (artificial data)");
            if (three)
            {
                if (!on) tip += "\nClick to show this bank again.";
                else if (result.ActiveCount == 3) tip += "\nClick to hide this bank.";
                else tip += "\nClick to hide this bank and show the hidden one instead.";
                tag.Cursor = Cursors.Hand;
                int slot = s;
                tag.MouseLeftButtonUp += (o, e) => { e.Handled = true; BankClicked?.Invoke(slot); };
            }
            tag.ToolTip = tip;

            tag.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size size = tag.DesiredSize;
            double left, topPos;
            if (s == 0)
            {
                left = center.X - r - 14 - size.Width;
                topPos = center.Y - size.Height / 2;
            }
            else if (s == 1)
            {
                left = center.X + r + 14;
                topPos = center.Y - size.Height / 2;
            }
            else
            {
                left = center.X - size.Width / 2;
                topPos = center.Y - r - 8 - size.Height;
            }
            Add(tag, Math.Max(2, left), Math.Max(2, topPos));
        }

        void Hover(int mask, bool entering)
        {
            Path path;
            if (!areaPaths.TryGetValue(mask, out path)) return;
            path.Fill = entering ? HoverFill : (mask == selectedMask ? SelectedFill : Brushes.Transparent);
            if (caption != null) caption.Text = entering ? Caption(mask) : (SelectionCaption() ?? idleCaption);
        }

        string SelectionCaption()
        {
            if (selectedMask == 0 || result == null || !result.Areas.ContainsKey(selectedMask)) return null;
            return "Selected: " + Caption(selectedMask);
        }

        string Caption(int mask)
        {
            VennArea area = result.Areas[mask];
            string text = CompareEngine.DescribeArea(result, mask) + ": " + area.Names.Count + " series";
            if (area.IsComparison)
                text += ", " + area.Deviations.Count + (area.Slots.Length == 3 ? " not equal in all three" : " deviating");
            return text;
        }

        void AddCenteredMessage(string text, double width, double height)
        {
            var message = new TextBlock
            {
                Text = text,
                FontSize = 14,
                Foreground = MutedBrush,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Width = Math.Max(50, width - 40)
            };
            message.Measure(new Size(message.Width, double.PositiveInfinity));
            Add(message, 20, (height - message.DesiredSize.Height) / 2);
        }

        void Add(UIElement element, double left, double top)
        {
            SetLeft(element, left);
            SetTop(element, top);
            Children.Add(element);
        }
    }
}
