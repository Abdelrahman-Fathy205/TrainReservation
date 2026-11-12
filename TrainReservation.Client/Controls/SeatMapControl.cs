using System.ComponentModel; // for designer attributes (DefaultValue, Browsable)
using System.Drawing.Drawing2D; // advanced drawing library
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Controls; // namespace for the custom controls

/// <summary>GDI+ drawn coach: 2 + 2 seats per row with an aisle; click free seats to select one or more.</summary>
public class SeatMapControl : Control // control that shows the seat map of a coach
{
    // sizes are in 96-DPI pixels, scaled to the screen so seats grow with the (point-sized) font
    int S(int px) => (int)(px * DeviceDpi / 96f); // scale sizes based on screen DPI
    int SeatSize => S(40); // seat size
    int Gap => S(8); // space between seats
    int Aisle => S(30); // aisle width

    static Color TakenColor => Theme.Dark ? Color.FromArgb(75, 85, 99) : Color.FromArgb(203, 213, 225); // color of a booked seat

    int seatCount = 40, cursor, hover; // cursor = seat the arrow keys are on
    HashSet<int> taken = new(), picked = new(); // booked seats, and seats the user picked

    public event EventHandler SelectedSeatChanged; // event that runs when the selection changes

    public SeatMapControl() // create the control
    {
        DoubleBuffered = true; // draw without flicker
        ResizeRedraw = true; // redraw when the size changes
        SetStyle(ControlStyles.Selectable, true); // arrow keys pick a seat
        TabStop = true; // can be reached with the Tab key
        BackColor = Color.White; // white background
        Font = new Font("Segoe UI", 9F, FontStyle.Bold); // font for seat numbers
    }

    [DefaultValue(40)] // default value is 40 seats
    public int SeatCount // number of seats
    {
        get => seatCount; // return the count
        set { seatCount = Math.Max(4, value); picked.Clear(); cursor = 0; Invalidate(); } // change the count (at least 4), clear the selection and redraw
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // hide it in the designer
    public IReadOnlyList<int> SelectedSeats => picked.Order().ToList(); // the picked seats, sorted

    public void SetTaken(IEnumerable<int> seats) // set the booked seats
    {
        taken = new HashSet<int>(seats); // save the booked seats
        // a new coach (or a refresh) starts a fresh selection
        picked.Clear(); // clear the selection
        cursor = 0; // remove the cursor position
        Invalidate(); // redraw
        SelectedSeatChanged?.Invoke(this, EventArgs.Empty); // say that the selection changed
    }

    /// <summary>Clicking or pressing Space on a free seat adds it to the selection, again removes it.</summary>
    void Toggle(int seat) // select or unselect a seat
    {
        cursor = seat; // move the cursor to this seat
        if (!picked.Remove(seat)) picked.Add(seat); // if it is picked remove it, if not add it
        Invalidate(); // redraw
        SelectedSeatChanged?.Invoke(this, EventArgs.Empty); // say that the selection changed
    }

    int Columns => (seatCount + 3) / 4; // number of columns (4 seats per column)
    Size GridSize => new(Columns * (SeatSize + Gap) - Gap, 4 * (SeatSize + Gap) - Gap + Aisle); // size of the whole seat grid
    Point Origin => new((Width - GridSize.Width) / 2, (Height - GridSize.Height - S(40)) / 2); // start point so the grid is centered

    Rectangle SeatRect(int seat) // position and size of one seat
    {
        int i = seat - 1, col = i / 4, pos = i % 4; // column number and place in the column
        return new Rectangle(Origin.X + col * (SeatSize + Gap), // horizontal position
                             Origin.Y + pos * (SeatSize + Gap) + (pos >= 2 ? Aisle : 0), // vertical position (after the aisle if in the bottom half)
                             SeatSize, SeatSize); // width and height
    }

    int HitTest(Point p) // find which seat the mouse is over
    {
        for (int s = 1; s <= seatCount; s++) // loop over all seats
            if (SeatRect(s).Contains(p)) return s; // if the point is inside the seat, return its number
        return 0; // not over any seat
    }

    protected override void OnPaint(PaintEventArgs e) // draw the control
    {
        var g = e.Graphics; // the drawing tool
        g.SmoothingMode = SmoothingMode.AntiAlias; // smooth edges

        // coach body with a rounded nose
        var body = new Rectangle(Origin.X - S(30), Origin.Y - S(24), GridSize.Width + S(60), GridSize.Height + S(48)); // coach body
        using (var path = Theme.RoundedRect(body, S(26))) // shape with rounded corners
        using (var fill = new SolidBrush(Theme.SurfaceAlt)) // background brush
        using (var pen = new Pen(Theme.Border, 2)) // border pen
        {
            g.FillPath(fill, path); // coach body color
            g.DrawPath(pen, path); // draw the border
        }

        using var center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }; // center the text
        var aisle = new Rectangle(body.X, Origin.Y + 2 * (SeatSize + Gap) - Gap, body.Width, Aisle + Gap); // aisle position
        using (var muted = new SolidBrush(Theme.Muted)) // faded brush
        using (var small = new Font("Segoe UI", 8F)) // small font
            g.DrawString("A I S L E", small, muted, aisle, center); // write "aisle"

        for (int s = 1; s <= seatCount; s++) // for each seat
        {
            var r = SeatRect(s); // its position
            Color back = taken.Contains(s) ? TakenColor // booked: gray
                       : picked.Contains(s) ? Theme.Accent // picked: orange
                       : s == hover ? Theme.PrimaryLight // mouse over it: light blue
                       : Theme.Surface; // free: normal color
            Color fore = taken.Contains(s) ? Theme.Muted : picked.Contains(s) ? Color.White : Theme.PrimaryText; // number color based on the state
            using var path = Theme.RoundedRect(r, S(8)); // seat shape
            using var b = new SolidBrush(back); // background brush
            using var p = new Pen(taken.Contains(s) ? TakenColor : Theme.PrimaryText, 1.5f); // border pen
            g.FillPath(b, path); // seat color
            g.DrawPath(p, path); // draw its border
            using var fb = new SolidBrush(fore); // number brush
            g.DrawString(s.ToString(), Font, fb, r, center); // write the seat number
            if (s == cursor && Focused) { r.Inflate(S(3), S(3)); ControlPaint.DrawFocusRectangle(g, r); } // draw the focus box around the current seat
        }

        DrawLegend(g, new Point(body.X, body.Bottom + S(14))); // draw the legend under the coach
    }

    void DrawLegend(Graphics g, Point at) // draw the legend
    {
        var items = new (string Text, Color Fill)[] { ("Available", Theme.Surface), ("Selected", Theme.Accent), ("Taken", TakenColor) }; // free, picked, booked
        using var textBrush = new SolidBrush(Theme.Text); // text brush
        using var font = new Font("Segoe UI", 9F); // the font
        using var pen = new Pen(Theme.PrimaryText); // border pen
        int x = at.X, box = S(16); // horizontal position and box size
        foreach (var (text, fill) in items) // for each item
        {
            using var b = new SolidBrush(fill); // color brush
            g.FillRectangle(b, x, at.Y, box, box); // color box
            g.DrawRectangle(pen, x, at.Y, box, box); // its border
            g.DrawString(text, font, textBrush, x + S(22), at.Y - 1); // the name next to it
            x += S(120); // move to the next item
        }
    }

    protected override void OnMouseMove(MouseEventArgs e) // when the mouse moves
    {
        int h = HitTest(e.Location); // which seat the mouse is over
        if (h != 0 && taken.Contains(h)) h = 0; // booked seats do not react to the mouse
        Cursor = h == 0 ? Cursors.Default : Cursors.Hand; // hand cursor over a free seat
        if (h != hover) { hover = h; Invalidate(); } // if the seat changed, redraw
        base.OnMouseMove(e); // keep the normal behavior
    }

    protected override void OnMouseLeave(EventArgs e) // when the mouse leaves the control
    {
        hover = 0; // no seat under the mouse
        Invalidate(); // redraw
        base.OnMouseLeave(e); // keep the normal behavior
    }

    protected override bool IsInputKey(Keys keyData) => // let arrow keys reach the control
        keyData is Keys.Left or Keys.Right or Keys.Up or Keys.Down || base.IsInputKey(keyData); // the four arrow keys

    /// <summary>Arrow keys move the cursor to the next free seat in that direction (rows of 4 run top to bottom);
    /// Space adds / removes the cursor seat.</summary>
    protected override void OnKeyDown(KeyEventArgs e) // when a key is pressed
    {
        if (e.KeyCode == Keys.Space && cursor > 0) { Toggle(cursor); e.Handled = true; } // Space: pick/unpick the current seat
        int step = e.KeyCode switch { Keys.Left => -4, Keys.Right => 4, Keys.Up => -1, Keys.Down => 1, _ => 0 }; // move the cursor based on the arrow
        if (step != 0) // if an arrow was pressed
        {
            int s = cursor == 0 ? (step > 0 ? 0 : seatCount + 1) : cursor; // start point
            if (cursor == 0) step = Math.Sign(step); // no cursor yet: start from the first / last free seat
            do s += step; while (s >= 1 && s <= seatCount && taken.Contains(s)); // move until a free seat is found
            if (s >= 1 && s <= seatCount) { cursor = s; Invalidate(); } // if a seat was found, put the cursor on it
            e.Handled = true; // the key was handled
        }
        base.OnKeyDown(e); // keep the normal behavior
    }

    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); } // redraw when it gets focus
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); } // redraw when it loses focus

    protected override void OnMouseClick(MouseEventArgs e) // when the user clicks with the mouse
    {
        Focus(); // take focus
        int s = HitTest(e.Location); // which seat was clicked
        if (s != 0 && !taken.Contains(s)) Toggle(s); // if free, pick/unpick it
        base.OnMouseClick(e); // keep the normal behavior
    }
}
