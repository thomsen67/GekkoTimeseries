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
            Color.FromRgb(0x3D, 0x8C, 0x4E),  // bank/folder 1 (top left): green
            Color.FromRgb(0x2E, 0x6D, 0xB4),  // bank/folder 2 (top right): blue
            Color.FromRgb(0xC9, 0x6A, 0x1E)   // bank/folder 3 (bottom): amber (red is used for what the check finds)
        };
        static readonly Color Grey = Color.FromRgb(0xA6, 0xA6, 0xA6);

        /// <summary>Dark red of the "found" numbers, also used for found names/files in the Names/Files tab.</summary>
        public static readonly Brush DeviationText = FromRgb(0xB0, 0x30, 0x30);

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

    /// <summary>What was clicked in an area: the area itself, or one of its two numbers.</summary>
    public enum AreaClickKind { Area, Total, Deviating }

    /// <summary>
    /// Venn diagram for 2 or 3 banks or folders (anything implementing IVennModel). Slot 1 is top-left,
    /// slot 2 top-right, slot 3 at the bottom. The 2-slot diagram is the 3-slot diagram without the bottom
    /// circle: same positions, same size, but without the vertical room the bottom circle needs (the window
    /// gives that room to the tabs).
    ///
    /// Each area shows two numbers:
    ///   black: all items (series or files) in the area,
    ///   red:   the items found by the current check (e.g. series that differ, files that differ).
    ///
    /// Clicks (the window turns them into drill-down filters):
    ///   inside an area                     -> AreaClicked(mask, Area, ctrl)
    ///   black / red number                 -> AreaClicked(mask, Total / Deviating, ctrl)
    ///   a box (bank or folder)             -> BankSelected(slot): the whole bank/folder
    ///   the eye icon on a box              -> HideClicked(slot)
    ///   the target icon on a box           -> FocusClicked(slot)
    /// </summary>
    public sealed class VennView : Canvas
    {
        public event Action<int, AreaClickKind, bool> AreaClicked;
        public event Action<int> BankSelected;
        public event Action<int> HideClicked;
        public event Action<int> FocusClicked;

        // Circle geometry in units of the radius, centered on the centroid of the circle centers.
        const double H3 = 0.288675;  // 1 / (2 * sqrt 3)
        const double T3 = 0.577350;  // 1 / sqrt 3

        // Room for bank labels on the left and right, and for the caption below.
        const double SideMargin = 245, TopMargin = 4, BottomMargin = 20;

        static readonly Brush HoverFill = BankColors.FromArgb(0x2A, 0, 0, 0);
        static readonly Brush SelectedFill = BankColors.FromArgb(0x16, 0, 0, 0);
        static readonly Brush SelectedStroke = BankColors.FromArgb(0x50, 0x33, 0x33, 0x33);
        static readonly Brush HiddenFill = BankColors.FromArgb(0x14, 0x80, 0x80, 0x80);
        static readonly Brush HiddenStroke = BankColors.FromRgb(0xB4, 0xB4, 0xB4);
        static readonly Brush HiddenLine = BankColors.FromArgb(0x70, 0xB4, 0xB4, 0xB4);
        static readonly Brush TotalBrush = BankColors.FromRgb(0x1F, 0x4E, 0x79);
        static readonly Brush NumberBrush = Brushes.Black;
        static readonly Brush DeviationBrush = BankColors.DeviationText;
        static readonly Brush MutedBrush = BankColors.FromRgb(0x88, 0x88, 0x88);
        static readonly Brush TextBrush = BankColors.FromRgb(0x44, 0x44, 0x44);
        static readonly Brush IconBrush = BankColors.FromRgb(0x66, 0x66, 0x66);
        static readonly Brush IconHoverBack = BankColors.FromArgb(0x1C, 0, 0, 0);

        IVennModel model;
        string emptyMessage = "";
        int selectedMask;
        int selectedBank = -1;
        FlagFilter foundFilter;
        int focusSlot = -1;
        readonly Dictionary<int, Path> areaPaths = new Dictionary<int, Path>();
        Ellipse[] circleShapes = new Ellipse[0];
        TextBlock caption;
        string idleCaption = "";

        public VennView()
        {
            Background = Brushes.Transparent;
            ClipToBounds = true;
            SizeChanged += (s, e) => Rebuild();
        }

        /// <summary>
        /// Everything the diagram shows, in one call (one redraw). The selection is either an area (mask != 0)
        /// or a whole bank/folder (bank >= 0). The found filter decides which number of the selected area is bold.
        /// </summary>
        public void SetState(IVennModel newModel, string messageWhenEmpty, int mask, int bank, FlagFilter found, int focus)
        {
            model = newModel;
            emptyMessage = messageWhenEmpty ?? "";
            selectedMask = mask;
            selectedBank = bank;
            foundFilter = found;
            focusSlot = focus;
            Rebuild();
        }

        // ---------------------------------------------------------------- geometry (public for the window)

        // Banks 1 and 2 are the top circles, bank 3 the bottom circle.
        const double YMin = -(H3 + 1);

        /// <summary>Bottom of the diagram in radius units: the bottom circle (3 banks) or the top circles (2 banks).</summary>
        static double YMax(int slotCount)
        {
            return slotCount == 3 ? T3 + 1 : 1 - H3;
        }

        /// <summary>The circle radius the diagram uses for a canvas of this size.</summary>
        public static double RadiusFor(double width, double height, int slotCount)
        {
            double r = Math.Min((width - 2 * SideMargin) / 3.0, (height - TopMargin - BottomMargin) / (YMax(slotCount) - YMin));
            return Math.Max(40, Math.Min(r, 200));
        }

        /// <summary>Vertical room that the bottom (third) circle adds to the diagram, for a given radius.</summary>
        public static double ThirdCircleHeight(double radius)
        {
            return (YMax(3) - YMax(2)) * radius;
        }

        // ---------------------------------------------------------------- drawing

        void Rebuild()
        {
            Children.Clear();
            areaPaths.Clear();
            circleShapes = new Ellipse[0];
            caption = null;

            double width = ActualWidth, height = ActualHeight;
            if (width < 120 || height < 120) return;
            if (model == null)
            {
                AddCenteredMessage(emptyMessage, width, height);
                return;
            }

            bool three = model.SlotCount == 3;
            // Horizontally the layout is always the 3-circle layout, so circles keep their size and place in 2-bank mode.
            Point[] unit3 = { new Point(-0.5, -H3), new Point(0.5, -H3), new Point(0, T3) };
            Point[] unit = unit3.Take(model.SlotCount).ToArray();
            double yMax = YMax(model.SlotCount);

            double r = RadiusFor(width, height, model.SlotCount);
            double ox = width / 2;
            double oy = TopMargin + ((height - TopMargin - BottomMargin) - (yMax - YMin) * r) / 2 - YMin * r;
            Func<Point, Point> toPixel = p => new Point(ox + p.X * r, oy + p.Y * r);
            Point[] centers = unit.Select(toPixel).ToArray();

            // Circles. A selected whole bank gets a solid, thicker outline in its color.
            circleShapes = new Ellipse[model.SlotCount];
            for (int s = 0; s < model.SlotCount; s++)
            {
                bool on = model.Active[s];
                var circle = new Ellipse
                {
                    Width = 2 * r,
                    Height = 2 * r,
                    Fill = on ? BankColors.Translucent(s, 0x30) : HiddenFill,
                    IsHitTestVisible = false
                };
                if (on) SetCircleOutline(circle, s, s == selectedBank);
                else
                {
                    circle.Stroke = HiddenLine;
                    circle.StrokeThickness = 1;
                    circle.StrokeDashArray = new DoubleCollection { 4, 3 };
                }
                circleShapes[s] = circle;
                Add(circle, centers[s].X - r, centers[s].Y - r);
            }

            // Areas as exact shapes, so hovering shows precisely which region a number belongs to.
            Geometry[] circles = centers.Select(c => (Geometry)new EllipseGeometry(c, r, r)).ToArray();
            foreach (int areaMask in model.AreaMasks)
            {
                int mask = areaMask;
                bool selected = mask == selectedMask;
                var path = new Path
                {
                    Data = AreaGeometry(mask, circles),
                    Fill = selected ? SelectedFill : Brushes.Transparent,
                    Stroke = selected ? SelectedStroke : null,
                    StrokeThickness = 1.5,
                    Cursor = Cursors.Hand
                };
                path.MouseEnter += (o, e) => Hover(mask, true);
                path.MouseLeave += (o, e) => Hover(mask, false);
                path.MouseLeftButtonUp += (o, e) => { e.Handled = true; RaiseArea(mask, AreaClickKind.Area); };
                areaPaths[mask] = path;
                Children.Add(path);
            }

            // Numbers
            foreach (int mask in model.AreaMasks)
            {
                Point p = toPixel(LabelPoint(three, mask));
                FrameworkElement label = BuildAreaLabel(mask);
                label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Add(label, p.X - label.DesiredSize.Width / 2, p.Y - label.DesiredSize.Height / 2);
            }

            // Boxes (banks or folders) with the eye (hide/show) and target (focus) icons
            for (int s = 0; s < model.SlotCount; s++) AddBankTag(s, centers[s], r, width);

            string word = model.SlotWord;
            idleCaption = focusSlot >= 0 && focusSlot < model.SlotCount
                ? "Focus on " + word + " " + (focusSlot + 1) + ": the other " + word + "s are greyed out. Click its target (or Alt+" + (focusSlot + 1) + ") to show them again."
                : "Black: all " + model.ItemWord + ". Red: " + model.FoundPhrase + ". Click a number or an area. Click a " + word + "'s box for the whole " + word + ".";
            caption = new TextBlock { FontSize = 12, Foreground = TextBrush, TextTrimming = TextTrimming.CharacterEllipsis, Width = width - 4 };
            caption.Text = SelectionCaption() ?? idleCaption;
            Add(caption, 2, height - 20);
        }

        void RaiseArea(int mask, AreaClickKind kind)
        {
            bool add = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            AreaClicked?.Invoke(mask, kind, add);
        }

        static void SetCircleOutline(Ellipse circle, int slot, bool strong)
        {
            circle.Stroke = strong ? BankColors.Solid(slot) : BankColors.Translucent(slot, 0x60);
            circle.StrokeThickness = strong ? 2.5 : 1;
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
                if ((mask & (1 << s)) != 0 || !model.Active[s]) continue;
                g = new CombinedGeometry(GeometryCombineMode.Exclude, g, circles[s]);
            }
            return g;
        }

        /// <summary>
        /// Label position of each area, in radius units relative to the centroid.
        /// Mask bits: 1 = bank 1 (top left), 2 = bank 2 (top right), 4 = bank 3 (bottom).
        /// </summary>
        static Point LabelPoint(bool three, int mask)
        {
            if (!three)
            {
                switch (mask)
                {
                    case 1: return new Point(-1.0, -H3);
                    case 2: return new Point(1.0, -H3);
                    default: return new Point(0, -H3);
                }
            }
            switch (mask)
            {
                case 1: return new Point(-0.935, -0.54);
                case 2: return new Point(0.935, -0.54);
                case 4: return new Point(0, 1.08);
                case 3: return new Point(0, -0.78);
                case 5: return new Point(-0.675, 0.39);
                case 6: return new Point(0.675, 0.39);
                default: return new Point(0, -0.02);
            }
        }

        /// <summary>
        /// Black number on top; below it the red number in parentheses (what the current check finds; not shown
        /// where the check has nothing to look at, e.g. differences in a one-bank area).
        /// Numbers are normal weight; in the selected area the number matching the found filter is bold.
        /// </summary>
        FrameworkElement BuildAreaLabel(int mask)
        {
            bool selected = mask == selectedMask;
            AreaCounts counts = model.AreaCountsOf(mask);

            var panel = new StackPanel { Background = Brushes.Transparent, Cursor = Cursors.Hand };

            var totalText = new TextBlock
            {
                Text = counts.Total.ToString(CultureInfo.InvariantCulture),
                FontWeight = selected && foundFilter != FlagFilter.Yes ? FontWeights.Bold : FontWeights.Normal,
                Foreground = NumberBrush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(4, 0, 4, 0),
                ToolTip = counts.Total + " " + model.ItemWord + ". Click to list all of them."
            };
            MakeClickable(totalText, () => RaiseArea(mask, AreaClickKind.Total));
            panel.Children.Add(totalText);

            if (model.ShowsFound(mask))
            {
                var foundText = new TextBlock
                {
                    Text = "(" + counts.Found.ToString(CultureInfo.InvariantCulture) + ")",
                    FontWeight = selected && foundFilter == FlagFilter.Yes ? FontWeights.Bold : FontWeights.Normal,
                    Foreground = counts.Found > 0 ? DeviationBrush : MutedBrush,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(4, 0, 4, 1),
                    ToolTip = model.FoundText(counts) + ". Click to list only these."
                };
                MakeClickable(foundText, () => RaiseArea(mask, AreaClickKind.Deviating));
                panel.Children.Add(foundText);
            }

            panel.MouseEnter += (o, e) => Hover(mask, true);
            panel.MouseLeave += (o, e) => Hover(mask, false);
            panel.MouseLeftButtonUp += (o, e) => { e.Handled = true; RaiseArea(mask, AreaClickKind.Area); };
            return panel;
        }

        static void MakeClickable(TextBlock text, Action onClick)
        {
            text.Cursor = Cursors.Hand;
            text.MouseEnter += (o, e) => text.TextDecorations = TextDecorations.Underline;
            text.MouseLeave += (o, e) => text.TextDecorations = null;
            text.MouseLeftButtonUp += (o, e) => { e.Handled = true; onClick(); };
        }

        // ---------------------------------------------------------------- bank labels

        void AddBankTag(int s, Point center, double r, double canvasWidth)
        {
            bool on = model.Active[s];
            bool focused = focusSlot == s;
            bool anyFocus = focusSlot >= 0;
            string word = model.SlotWord;
            int slot = s;

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
                Text = model.SlotName(s),
                FontWeight = FontWeights.SemiBold,
                Foreground = on ? TotalBrush : MutedBrush,
                MaxWidth = 140,
                TextTrimming = TextTrimming.CharacterEllipsis
            });
            lines.Children.Add(new TextBlock
            {
                Text = on ? model.SlotInfo(s) : (anyFocus ? "greyed out by the focus" : "hidden"),
                FontSize = 11,
                Foreground = MutedBrush,
                MaxWidth = 140,
                TextTrimming = TextTrimming.CharacterEllipsis
            });

            // Eye: hide or show this bank. Target: focus on this bank (grey out the others) or end the focus.
            string eyeTip;
            if (!on) eyeTip = anyFocus ? "Show this " + word + " again (ends the focus)" : "Show this " + word + " again";
            else if (focused) eyeTip = "This " + word + " is in focus. Click its target to show the other " + word + "s again.";
            else if (model.ActiveCount > 1) eyeTip = "Hide this " + word;
            else eyeTip = "This is the only " + word + " shown, so it cannot be hidden";
            string key = " (Alt+" + (s + 1) + ")";
            string targetTip = focused
                ? "End the focus: show the other " + word + "s again" + key
                : "Focus on this " + word + ": grey out the others" + key;

            var icons = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(6, 0, 0, 0) };
            icons.Children.Add(IconButton(EyeIcon(!on, on ? IconBrush : MutedBrush), eyeTip, null, () => HideClicked?.Invoke(slot)));
            icons.Children.Add(IconButton(TargetIcon(focused ? BankColors.Solid(s) : IconBrush, focused),
                                          targetTip, focused ? BankColors.Translucent(s, 0x30) : null,
                                          () => FocusClicked?.Invoke(slot)));

            var row = new StackPanel { Orientation = Orientation.Horizontal };
            row.Children.Add(badge);
            row.Children.Add(lines);
            row.Children.Add(icons);

            var tag = new Border
            {
                Child = row,
                Padding = new Thickness(6, 4, 5, 4),
                CornerRadius = new CornerRadius(3),
                BorderThickness = new Thickness(focused ? 2 : 1),
                BorderBrush = on ? BankColors.Solid(s) : HiddenStroke,
                Background = Brushes.White
            };

            string tip = model.SlotTip(s);
            if (on)
            {
                tip += "\nClick to select the whole " + word + ".";
                tag.Cursor = Cursors.Hand;
                tag.MouseEnter += (o, e) => HoverBank(slot, true);
                tag.MouseLeave += (o, e) => HoverBank(slot, false);
                tag.MouseLeftButtonUp += (o, e) => { e.Handled = true; BankSelected?.Invoke(slot); };
            }
            else
            {
                tip += "\nUse the eye to show this " + word + " again.";
            }
            tag.ToolTip = tip;

            tag.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size size = tag.DesiredSize;
            double left, topPos;
            if (s == 0)
            {
                // Left of the top-left circle, somewhat above its center
                left = center.X - r - 10 - size.Width;
                topPos = center.Y - 0.45 * r - size.Height / 2;
            }
            else if (s == 1)
            {
                // Right of the top-right circle, somewhat above its center
                left = center.X + r + 10;
                topPos = center.Y - 0.45 * r - size.Height / 2;
            }
            else
            {
                // Right of the bottom circle and somewhat below its center, clear of the top-right circle
                left = center.X + 1.1 * r;
                topPos = center.Y + 0.3 * r - size.Height / 2;
            }
            left = Math.Min(left, canvasWidth - size.Width - 2);
            Add(tag, Math.Max(2, left), Math.Max(2, topPos));
        }

        /// <summary>A small flat icon button. Its click does not also count as a click on the box.</summary>
        static FrameworkElement IconButton(UIElement icon, string tip, Brush activeBack, Action onClick)
        {
            Brush normalBack = activeBack ?? Brushes.Transparent;
            var button = new Border
            {
                Width = 20,
                Height = 20,
                CornerRadius = new CornerRadius(3),
                Background = normalBack,
                Margin = new Thickness(2, 0, 0, 0),
                Cursor = Cursors.Hand,
                ToolTip = tip,
                Child = icon
            };
            button.MouseEnter += (o, e) => button.Background = activeBack ?? IconHoverBack;
            button.MouseLeave += (o, e) => button.Background = normalBack;
            button.MouseLeftButtonUp += (o, e) => { e.Handled = true; onClick(); };
            return button;
        }

        static UIElement EyeIcon(bool crossed, Brush stroke)
        {
            var g = new GeometryGroup();
            g.Children.Add(Geometry.Parse("M0.5,6 C3,1.5 9,1.5 11.5,6 C9,10.5 3,10.5 0.5,6 Z"));
            g.Children.Add(new EllipseGeometry(new Point(6, 6), 1.8, 1.8));
            if (crossed) g.Children.Add(Geometry.Parse("M1.5,11 L10.5,1"));
            return new Path
            {
                Data = g,
                Stroke = stroke,
                StrokeThickness = 1.2,
                Width = 12,
                Height = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        static UIElement TargetIcon(Brush stroke, bool filledDot)
        {
            var ring = new GeometryGroup();
            ring.Children.Add(new EllipseGeometry(new Point(6, 6), 4.5, 4.5));
            ring.Children.Add(Geometry.Parse("M6,0 L6,3 M6,9 L6,12 M0,6 L3,6 M9,6 L12,6"));
            var grid = new Grid
            {
                Width = 12,
                Height = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(new Path { Data = ring, Stroke = stroke, StrokeThickness = 1.2 });
            grid.Children.Add(new Path
            {
                Data = new EllipseGeometry(new Point(6, 6), filledDot ? 2.2 : 1.5, filledDot ? 2.2 : 1.5),
                Fill = stroke
            });
            return grid;
        }

        // ---------------------------------------------------------------- hover and captions

        void Hover(int mask, bool entering)
        {
            Path path;
            if (!areaPaths.TryGetValue(mask, out path)) return;
            path.Fill = entering ? HoverFill : (mask == selectedMask ? SelectedFill : Brushes.Transparent);
            if (caption != null) caption.Text = entering ? model.Summarize(mask) : (SelectionCaption() ?? idleCaption);
        }

        /// <summary>Hovering a box previews the whole-bank/folder outline.</summary>
        void HoverBank(int slot, bool entering)
        {
            if (slot >= circleShapes.Length || !model.Active[slot]) return;
            SetCircleOutline(circleShapes[slot], slot, entering || slot == selectedBank);
            if (caption != null) caption.Text = entering ? BankCaption(slot) : (SelectionCaption() ?? idleCaption);
        }

        string BankCaption(int slot)
        {
            return model.SummarizeWhole(slot);
        }

        string SelectionCaption()
        {
            if (model == null) return null;
            if (selectedBank >= 0 && selectedBank < model.SlotCount && model.Active[selectedBank])
                return "Selected: " + BankCaption(selectedBank);
            if (selectedMask == 0 || !model.AreaMasks.Contains(selectedMask)) return null;
            return "Selected: " + model.Summarize(selectedMask);
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
