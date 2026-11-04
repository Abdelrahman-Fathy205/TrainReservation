using System.ComponentModel;
using System.Drawing.Drawing2D;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Controls;

public enum ChartKind { Bar, Pie }

/// <summary>Small GDI+ chart: horizontal bars or a pie with legend. The control's Text is the chart title.</summary>
public class ChartControl : Control
{
    static readonly Color[] Palette =
    {
        Color.FromArgb(30, 58, 138), Color.FromArgb(245, 158, 11), Color.FromArgb(22, 163, 74),
        Color.FromArgb(220, 38, 38), Color.FromArgb(124, 58, 237), Color.FromArgb(8, 145, 178),
    };

    List<(string Label, double Value)> data = new();

    public ChartControl()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F);
    }

    [DefaultValue(ChartKind.Bar)]
    public ChartKind Kind { get; set; } = ChartKind.Bar;

    [DefaultValue("N0")]
    public string ValueFormat { get; set; } = "N0";

    public void SetData(IEnumerable<(string Label, double Value)> points)
    {
        data = points.ToList();
        Invalidate();
    }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        using var titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        using var textBrush = new SolidBrush(Theme.Text);
        g.DrawString(Text, titleFont, textBrush, 16, 12);

        var area = new Rectangle(16, 50, Width - 32, Height - 66);
        if (data.Count == 0 || data.All(d => d.Value <= 0))
        {
            using var muted = new SolidBrush(Theme.Muted);
            g.DrawString("No data for the selected period", Font, muted, area.X, area.Y);
            return;
        }
        if (Kind == ChartKind.Bar) DrawBars(g, area, textBrush);
        else DrawPie(g, area, textBrush);
    }

    void DrawBars(Graphics g, Rectangle area, Brush textBrush)
    {
        int labelWidth = (int)data.Max(d => g.MeasureString(d.Label, Font).Width) + 10;
        double max = data.Max(d => d.Value);
        float rowH = Math.Min(42, (float)area.Height / data.Count);
        float barMax = area.Width - labelWidth - 90;
        using var track = new SolidBrush(Theme.Background);
        for (int i = 0; i < data.Count; i++)
        {
            float y = area.Y + i * rowH;
            var (label, value) = data[i];
            g.DrawString(label, Font, textBrush, area.X, y + rowH / 2 - 9);
            var full = new RectangleF(area.X + labelWidth, y + 6, barMax, rowH - 12);
            using (var tp = Theme.RoundedRect(full, 4)) g.FillPath(track, tp);
            float w = (float)(value / max * barMax);
            if (w >= 8)
            {
                using var bp = Theme.RoundedRect(new RectangleF(full.X, full.Y, w, full.Height), 4);
                using var b = new SolidBrush(Palette[i % Palette.Length]);
                g.FillPath(b, bp);
            }
            g.DrawString(value.ToString(ValueFormat), Font, textBrush, full.X + w + 6, y + rowH / 2 - 9);
        }
    }

    void DrawPie(Graphics g, Rectangle area, Brush textBrush)
    {
        double total = data.Sum(d => d.Value);
        int size = Math.Min(area.Height, area.Width / 2) - 10;
        var pie = new Rectangle(area.X + 10, area.Y + (area.Height - size) / 2, size, size);
        float start = -90;
        for (int i = 0; i < data.Count; i++)
        {
            float sweep = (float)(data[i].Value / total * 360);
            using var b = new SolidBrush(Palette[i % Palette.Length]);
            g.FillPie(b, pie, start, sweep);
            g.DrawPie(Pens.White, pie, start, sweep);
            start += sweep;
        }
        // donut hole
        int hole = size / 2;
        g.FillEllipse(Brushes.White, pie.X + (size - hole) / 2, pie.Y + (size - hole) / 2, hole, hole);

        int lx = pie.Right + 30, ly = area.Y + (area.Height - data.Count * 28) / 2;
        for (int i = 0; i < data.Count; i++)
        {
            using var b = new SolidBrush(Palette[i % Palette.Length]);
            g.FillRectangle(b, lx, ly + i * 28 + 3, 14, 14);
            g.DrawString($"{data[i].Label}  {data[i].Value.ToString(ValueFormat)}  ({data[i].Value / total:P0})", Font, textBrush, lx + 22, ly + i * 28);
        }
    }
}
