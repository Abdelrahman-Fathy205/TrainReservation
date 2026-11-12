using System.ComponentModel; // for DefaultValue
using System.Drawing.Drawing2D; // advanced drawing library
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Controls; // namespace for the custom controls

public enum ChartKind { Bar, Pie } // chart type: bars or pie

/// <summary>Small GDI+ chart: horizontal bars or a pie with legend. The control's Text is the chart title.</summary>
public class ChartControl : Control // chart control
{
    static readonly Color[] Palette = // the colors used in the chart
    {
        Color.FromArgb(30, 58, 138), Color.FromArgb(245, 158, 11), Color.FromArgb(22, 163, 74), // blue, orange, green
        Color.FromArgb(220, 38, 38), Color.FromArgb(124, 58, 237), Color.FromArgb(8, 145, 178), // red, purple, cyan
    };

    List<(string Label, double Value)> data = new(); // the data: a name and a value

    public ChartControl() // create the control
    {
        DoubleBuffered = true; // draw without flicker
        ResizeRedraw = true; // redraw when the size changes
        BackColor = Color.White; // white background
        Font = new Font("Segoe UI", 9F); // the font
    }

    [DefaultValue(ChartKind.Bar)] // default value: bars
    public ChartKind Kind { get; set; } = ChartKind.Bar; // chart type

    [DefaultValue("N0")] // default format: whole number
    public string ValueFormat { get; set; } = "N0"; // how numbers are written

    public void SetData(IEnumerable<(string Label, double Value)> points) // set the data
    {
        data = points.ToList(); // save the data
        Invalidate(); // redraw
    }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); } // redraw when the title changes

    protected override void OnPaint(PaintEventArgs e) // draw the control
    {
        var g = e.Graphics; // the drawing tool
        g.SmoothingMode = SmoothingMode.AntiAlias; // smooth edges
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; // clear text
        using var titleFont = new Font("Segoe UI", 12F, FontStyle.Bold); // title font
        using var textBrush = new SolidBrush(Theme.Text); // text brush
        g.DrawString(Text, titleFont, textBrush, 16, 12); // draw the title

        var area = new Rectangle(16, 50, Width - 32, Height - 66); // the space left for drawing under the title
        if (data.Count == 0 || data.All(d => d.Value <= 0)) // if there is no data
        {
            using var muted = new SolidBrush(Theme.Muted); // faded brush
            g.DrawString("No data for the selected period", Font, muted, area.X, area.Y); // write "no data"
            return; // stop
        }
        if (Kind == ChartKind.Bar) DrawBars(g, area, textBrush); // if bars, draw bars
        else DrawPie(g, area, textBrush); // otherwise draw a pie
    }

    void DrawBars(Graphics g, Rectangle area, Brush textBrush) // draw the horizontal bars
    {
        int labelWidth = (int)data.Max(d => g.MeasureString(d.Label, Font).Width) + 10; // width of the longest name
        double max = data.Max(d => d.Value); // biggest value
        float rowH = Math.Min(42, (float)area.Height / data.Count); // height of each row (max 42)
        float barMax = area.Width - labelWidth - 90; // max bar length
        using var track = new SolidBrush(Theme.Background); // brush for the gray bar background
        for (int i = 0; i < data.Count; i++) // for each item
        {
            float y = area.Y + i * rowH; // vertical position of the row
            var (label, value) = data[i]; // the name and value
            g.DrawString(label, Font, textBrush, area.X, y + rowH / 2 - 9); // draw the name
            var full = new RectangleF(area.X + labelWidth, y + 6, barMax, rowH - 12); // rectangle of the full bar
            using (var tp = Theme.RoundedRect(full, 4)) g.FillPath(track, tp); // draw the gray background
            float w = (float)(value / max * barMax); // bar length based on value compared to the biggest value
            if (w >= 8) // if the bar is not too small
            {
                using var bp = Theme.RoundedRect(new RectangleF(full.X, full.Y, w, full.Height), 4); // bar shape
                using var b = new SolidBrush(Palette[i % Palette.Length]); // bar color
                g.FillPath(b, bp); // draw the bar
            }
            g.DrawString(value.ToString(ValueFormat), Font, textBrush, full.X + w + 6, y + rowH / 2 - 9); // write the value next to the bar
        }
    }

    void DrawPie(Graphics g, Rectangle area, Brush textBrush) // draw the pie
    {
        double total = data.Sum(d => d.Value); // sum of the values
        int size = Math.Min(area.Height, area.Width / 2) - 10; // pie size
        var pie = new Rectangle(area.X + 10, area.Y + (area.Height - size) / 2, size, size); // pie position
        float start = -90; // start from the top
        using var surface = new SolidBrush(Theme.Surface); // brush with the background color
        using var surfacePen = new Pen(Theme.Surface); // pen with the background color (gap between slices)
        for (int i = 0; i < data.Count; i++) // for each item
        {
            float sweep = (float)(data[i].Value / total * 360); // slice angle based on its share
            using var b = new SolidBrush(Palette[i % Palette.Length]); // slice color
            g.FillPie(b, pie, start, sweep); // draw the slice
            g.DrawPie(surfacePen, pie, start, sweep); // draw its border
            start += sweep; // the next slice starts here
        }
        // donut hole
        int hole = size / 2; // size of the hole in the middle
        g.FillEllipse(surface, pie.X + (size - hole) / 2, pie.Y + (size - hole) / 2, hole, hole); // draw the hole

        int lx = pie.Right + 30, ly = area.Y + (area.Height - data.Count * 28) / 2; // position of the legend
        for (int i = 0; i < data.Count; i++) // for each item
        {
            using var b = new SolidBrush(Palette[i % Palette.Length]); // its color
            g.FillRectangle(b, lx, ly + i * 28 + 3, 14, 14); // small colored square
            g.DrawString($"{data[i].Label}  {data[i].Value.ToString(ValueFormat)}  ({data[i].Value / total:P0})", Font, textBrush, lx + 22, ly + i * 28); // the name, value and percent
        }
    }
}
