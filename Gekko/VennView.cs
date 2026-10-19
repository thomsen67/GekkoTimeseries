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
    ///   the hide/show link in a box        -> HideClicked(slot)
    ///   the focus/unfocus link in a box    -> FocusClicked(slot)
    ///
    /// The boxes are measured first, and the circles leave just enough room for the widest one at the sides.
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

        // Room for the caption below. The room for the boxes on the left and right (sideMargin) follows the widest box.
        const double TopMargin = 4, BottomMargin = 20;
        const double DefaultSideMargin = 200;

        /// <summary>Bank and folder names in the boxes are shortened to this many characters (middle replaced by "...").</summary>
        public const int MaxNameLength = 25;

        /// <summary>Gap between the bottom (third) circle and its box, in radii (keeps the box clear of the top-right circle).</summary>
        const double BottomBoxGap = 0.3;

        /// <summary>How far below the bottom circle's center the middle of its box is, in radii.</summary>
        const double BottomBoxDrop = 0.45;

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
        static readonly Brush LinkBrush = BankColors.FromRgb(0x2E, 0x6D, 0xB4);

        IVennModel model;
        double sideMargin = DefaultSideMargin;
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

        /// <summary>
        /// The circle radius the diagram uses for a canvas of this size (with the room the boxes took in the last drawing).
        /// </summary>
        public double RadiusFor(double width, double height, int slotCount)
        {
            double r = Math.Min((width - 2 * sideMargin) / 3.0, (height - TopMargin - BottomMargin) / (YMax(slotCount) - YMin));
            return Math.Max(40, Math.Min(r, 200));
        }

        /// <summary>
        /// At most maxLength characters: a longer name keeps its start and end, with "..." in the middle
        /// (e.g. "forecast_base_scen...ision_2025.gbk").
        /// </summary>
        public static string ShortName(string name, int maxLength = MaxNameLength)
        {
            if (name == null) return "";
            if (name.Length <= maxLength || maxLength < 5) return name;
            int keep = maxLength - 3;
            int head = (keep + 1) / 2;
            return name.Substring(0, head) + "..." + name.Substring(name.Length - (keep - head));
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

            // The boxes first: the widest one decides how much room the circles leave at the sides.
            var tags = new Border[model.SlotCount];
            double widest = 0;
            for (int s = 0; s < model.SlotCount; s++)
            {
                tags[s] = BuildBankTag(s);
                tags[s].Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                widest = Math.Max(widest, tags[s].DesiredSize.Width);
            }
            sideMargin = widest + 14;

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

            // Boxes (banks or folders) with the hide/show and focus/unfocus links
            double topRightBottom = double.NegativeInfinity;
            for (int s = 0; s < model.SlotCount; s++) PlaceBankTag(tags[s], s, centers[s], r, width, height, ref topRightBottom);

            string word = model.SlotWord;
            idleCaption = focusSlot >= 0 && focusSlot < model.SlotCount
                ? "Focus on " + word + " " + (focusSlot + 1) + ": the other " + word + "s are greyed out. Click unfocus (or Alt+" + (focusSlot + 1) + ") to show them again."
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

        /// <summary>
        /// A box: the number badge, then, one under the other, the name (at most MaxNameLength characters), the
        /// model's info lines (e.g. "1990–2025" and "260 series", or "812 data files") and the hide/show and
        /// focus/unfocus links.
        /// </summary>
        Border BuildBankTag(int s)
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
                Text = ShortName(model.SlotName(s)),
                FontWeight = FontWeights.SemiBold,
                Foreground = on ? TotalBrush : MutedBrush
            });
            string info = model.SlotInfo(s);
            if (!string.IsNullOrEmpty(info))
            {
                // One TextBlock per line ("\n" separates them), so every line has the same spacing.
                foreach (string line in info.Split('\n'))
                    lines.Children.Add(new TextBlock { Text = line, FontSize = 11, Foreground = MutedBrush });
            }
            if (!on)
            {
                lines.Children.Add(new TextBlock
                {
                    Text = anyFocus ? "greyed out by the focus" : "hidden",
                    FontSize = 11,
                    FontStyle = FontStyles.Italic,
                    Foreground = MutedBrush
                });
            }

            // hide/show this bank; focus on it (grey out the others) or end the focus.
            string hideTip;
            bool hideWorks;
            if (!on)
            {
                hideTip = anyFocus ? "Show this " + word + " again (ends the focus)" : "Show this " + word + " again";
                hideWorks = true;
            }
            else if (focused)
            {
                hideTip = "This " + word + " is in focus. Click unfocus to show the other " + word + "s again.";
                hideWorks = false;
            }
            else if (model.ActiveCount > 1)
            {
                hideTip = "Hide this " + word;
                hideWorks = true;
            }
            else
            {
                hideTip = "This is the only " + word + " shown, so it cannot be hidden";
                hideWorks = false;
            }
            string key = " (Alt+" + (s + 1) + ")";
            string focusTip = focused
                ? "End the focus: show the other " + word + "s again" + key
                : "Focus on this " + word + ": grey out the others" + key;

            var links = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2, 0, 0) };
            links.Children.Add(Link(on ? "hide" : "show", hideTip, hideWorks ? LinkBrush : MutedBrush,
                                    () => HideClicked?.Invoke(slot)));
            TextBlock focusLink = Link(focused ? "unfocus" : "focus", focusTip, LinkBrush, () => FocusClicked?.Invoke(slot));
            focusLink.Margin = new Thickness(12, 0, 0, 0);
            links.Children.Add(focusLink);
            lines.Children.Add(links);

            var row = new StackPanel { Orientation = Orientation.Horizontal };
            row.Children.Add(badge);
            row.Children.Add(lines);

            var tag = new Border
            {
                Child = row,
                Padding = new Thickness(6, 4, 8, 4),
                CornerRadius = new CornerRadius(3),
                BorderThickness = new Thickness(focused ? 2 : 1),
                BorderBrush = on ? BankColors.Solid(s) : HiddenStroke,
                Background = Brushes.White
            };

            string tip = model.SlotName(s) + "\n" + model.SlotTip(s);
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
                tip += "\nClick show to show this " + word + " again.";
            }
            tag.ToolTip = tip;
            return tag;
        }

        /// <summary>
        /// Box 1 left of the top-left circle, box 2 right of the top-right circle, box 3 right of the bottom circle,
        /// BottomBoxGap radii away from it and BottomBoxDrop radii down (and never on top of box 2). topRightBottom is
        /// the bottom edge of box 2.
        /// </summary>
        void PlaceBankTag(Border tag, int s, Point center, double r, double canvasWidth, double canvasHeight, ref double topRightBottom)
        {
            Size size = tag.DesiredSize;
            double left, topPos;
            if (s == 0)
            {
                left = center.X - r - 10 - size.Width;
                topPos = center.Y - 0.45 * r - size.Height / 2;
            }
            else if (s == 1)
            {
                left = center.X + r + 10;
                topPos = center.Y - 0.45 * r - size.Height / 2;
            }
            else
            {
                left = center.X + (1 + BottomBoxGap) * r;
                topPos = center.Y + BottomBoxDrop * r - size.Height / 2;
                topPos = Math.Min(topPos, canvasHeight - BottomMargin - size.Height - 2);  // above the caption
            }
            left = Math.Max(2, Math.Min(left, canvasWidth - size.Width - 2));
            topPos = Math.Max(2, topPos);
            if (s == 2) topPos = Math.Max(topPos, topRightBottom + 6);
            if (s == 1) topRightBottom = topPos + size.Height;
            Add(tag, left, topPos);
        }

        /// <summary>A small text link. Its click does not also count as a click on the box.</summary>
        static TextBlock Link(string text, string tip, Brush foreground, Action onClick)
        {
            var link = new TextBlock
            {
                Text = text,
                FontSize = 10.5,
                Foreground = foreground,
                Background = Brushes.Transparent,  // the whole text is clickable, not only the strokes
                Cursor = Cursors.Hand,
                ToolTip = tip
            };
            link.MouseEnter += (o, e) => link.TextDecorations = TextDecorations.Underline;
            link.MouseLeave += (o, e) => link.TextDecorations = null;
            link.MouseLeftButtonUp += (o, e) => { e.Handled = true; onClick(); };
            return link;
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
