using System.ComponentModel;
using System.Drawing.Drawing2D;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Controls;

/// <summary>GDI+ drawn coach: 2 + 2 seats per row with an aisle; click a free seat to select it.</summary>
public class SeatMapControl : Control
{
    // sizes are in 96-DPI pixels, scaled to the screen so seats grow with the (point-sized) font
    int S(int px) => (int)(px * DeviceDpi / 96f);
    int SeatSize => S(40);
    int Gap => S(8);
    int Aisle => S(30);

    int seatCount = 40, selected, hover;
    HashSet<int> taken = new();

    public event EventHandler SelectedSeatChanged;

    public SeatMapControl()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    [DefaultValue(40)]
    public int SeatCount
    {
        get => seatCount;
        set { seatCount = Math.Max(4, value); selected = 0; Invalidate(); }
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedSeat => selected;

    public void SetTaken(IEnumerable<int> seats)
    {
        taken = new HashSet<int>(seats);
        if (taken.Contains(selected)) Select(0);
        Invalidate();
    }

    void Select(int seat)
    {
        selected = seat;
        Invalidate();
        SelectedSeatChanged?.Invoke(this, EventArgs.Empty);
    }

    int Columns => (seatCount + 3) / 4;
    Size GridSize => new(Columns * (SeatSize + Gap) - Gap, 4 * (SeatSize + Gap) - Gap + Aisle);
    Point Origin => new((Width - GridSize.Width) / 2, (Height - GridSize.Height - S(40)) / 2);

    Rectangle SeatRect(int seat)
    {
        int i = seat - 1, col = i / 4, pos = i % 4;
        return new Rectangle(Origin.X + col * (SeatSize + Gap),
                             Origin.Y + pos * (SeatSize + Gap) + (pos >= 2 ? Aisle : 0),
                             SeatSize, SeatSize);
    }

    int HitTest(Point p)
    {
        for (int s = 1; s <= seatCount; s++)
            if (SeatRect(s).Contains(p)) return s;
        return 0;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // coach body with a rounded nose
        var body = new Rectangle(Origin.X - S(30), Origin.Y - S(24), GridSize.Width + S(60), GridSize.Height + S(48));
        using (var path = Theme.RoundedRect(body, S(26)))
        using (var fill = new SolidBrush(Color.FromArgb(248, 250, 252)))
        using (var pen = new Pen(Theme.Border, 2))
        {
            g.FillPath(fill, path);
            g.DrawPath(pen, path);
        }

        using var center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        var aisle = new Rectangle(body.X, Origin.Y + 2 * (SeatSize + Gap) - Gap, body.Width, Aisle + Gap);
        using (var muted = new SolidBrush(Theme.Muted))
        using (var small = new Font("Segoe UI", 8F))
            g.DrawString("A I S L E", small, muted, aisle, center);

        for (int s = 1; s <= seatCount; s++)
        {
            var r = SeatRect(s);
            Color back = taken.Contains(s) ? Color.FromArgb(203, 213, 225)
                       : s == selected ? Theme.Accent
                       : s == hover ? Theme.PrimaryLight
                       : Color.White;
            Color fore = taken.Contains(s) ? Theme.Muted : s == selected ? Color.White : Theme.Primary;
            using var path = Theme.RoundedRect(r, S(8));
            using var b = new SolidBrush(back);
            using var p = new Pen(taken.Contains(s) ? Color.FromArgb(203, 213, 225) : Theme.Primary, 1.5f);
            g.FillPath(b, path);
            g.DrawPath(p, path);
            using var fb = new SolidBrush(fore);
            g.DrawString(s.ToString(), Font, fb, r, center);
        }

        DrawLegend(g, new Point(body.X, body.Bottom + S(14)));
    }

    void DrawLegend(Graphics g, Point at)
    {
        var items = new (string Text, Color Fill)[] { ("Available", Color.White), ("Selected", Theme.Accent), ("Taken", Color.FromArgb(203, 213, 225)) };
        using var textBrush = new SolidBrush(Theme.Text);
        using var font = new Font("Segoe UI", 9F);
        using var pen = new Pen(Theme.Primary);
        int x = at.X, box = S(16);
        foreach (var (text, fill) in items)
        {
            using var b = new SolidBrush(fill);
            g.FillRectangle(b, x, at.Y, box, box);
            g.DrawRectangle(pen, x, at.Y, box, box);
            g.DrawString(text, font, textBrush, x + S(22), at.Y - 1);
            x += S(120);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int h = HitTest(e.Location);
        if (h != 0 && taken.Contains(h)) h = 0;
        Cursor = h == 0 ? Cursors.Default : Cursors.Hand;
        if (h != hover) { hover = h; Invalidate(); }
        base.OnMouseMove(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        hover = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        int s = HitTest(e.Location);
        if (s != 0 && !taken.Contains(s)) Select(s);
        base.OnMouseClick(e);
    }
}
