using System.Drawing.Drawing2D; // advanced drawing library
using System.Runtime.CompilerServices; // for ConditionalWeakTable
using TrainReservation.Client.Services; // services (data file path)

namespace TrainReservation.Client.UI; // namespace for the UI

public static class Theme // class for the colors and general look of the program
{
    // the choice is remembered as an empty marker file next to the saved data
    static readonly string DarkFlag = Path.Combine(Path.GetDirectoryName(DataStore.FilePath), "dark"); // path of the dark mode flag file
    public static bool Dark { get; private set; } = File.Exists(DarkFlag); // dark mode is on if the file exists

    public static readonly Color Primary = Color.FromArgb(30, 58, 138); // main color (dark blue)
    public static readonly Color Accent = Color.FromArgb(245, 158, 11); // accent color (orange)
    public static readonly Color Success = Color.FromArgb(22, 163, 74); // success color (green)
    public static Color PrimaryLight => Dark ? Color.FromArgb(30, 64, 120) : Color.FromArgb(219, 234, 254); // light blue (changes with the mode)
    /// <summary>Primary used as text or outline on a surface: lighter in dark mode so it stays readable.</summary>
    public static Color PrimaryText => Dark ? Color.FromArgb(147, 197, 253) : Primary; // blue text color (lighter in dark mode so it is readable)
    public static Color Background => Dark ? Color.FromArgb(17, 24, 39) : Color.FromArgb(243, 244, 246); // screen background color
    public static Color Surface => Dark ? Color.FromArgb(31, 41, 55) : Color.White; // card background color
    public static Color SurfaceAlt => Dark ? Color.FromArgb(39, 50, 66) : Color.FromArgb(249, 250, 251); // alternate background color (alternating rows)
    public static Color Text => Dark ? Color.FromArgb(229, 231, 235) : Color.FromArgb(17, 24, 39); // normal text color
    public static Color Muted => Dark ? Color.FromArgb(156, 163, 175) : Color.FromArgb(107, 114, 128); // faded text color
    public static Color Border => Dark ? Color.FromArgb(55, 65, 81) : Color.FromArgb(209, 213, 219); // border color

    static int Rgb(int r, int g, int b) => Color.FromArgb(r, g, b).ToArgb(); // turn a color into a number to use it as a key

    // designer (light) colour → the Theme colour that replaces it; anything not listed is left alone
    static readonly Dictionary<int, Func<Color>> BackMap = new() // table for swapping background colors
    {
        [Rgb(255, 255, 255)] = () => Surface, // white becomes the card color
        [Rgb(243, 244, 246)] = () => Background, // light gray becomes the background
        [SystemColors.Control.ToArgb()] = () => Background, // the default Windows color becomes the background
        [Rgb(249, 250, 251)] = () => SurfaceAlt, // very light gray becomes the alternate color
        [Rgb(248, 250, 252)] = () => SurfaceAlt, // same thing
        [Rgb(219, 234, 254)] = () => PrimaryLight, // light blue becomes its own version
    };
    static readonly Dictionary<int, Func<Color>> ForeMap = new() // table for swapping text colors
    {
        [Rgb(17, 24, 39)] = () => Text, // dark text becomes the text color
        [Rgb(0, 0, 0)] = () => Text, // black becomes the text color
        [Rgb(107, 114, 128)] = () => Muted, // gray becomes the faded color
        [Rgb(30, 58, 138)] = () => PrimaryText, // blue becomes the readable blue
    };

    static readonly ConditionalWeakTable<Control, object> Originals = new(); // stores the original colors of each control

    /// <summary>Colours a form or page for the current mode and gives its flat buttons hover/pressed feedback.
    /// Call once after InitializeComponent; safe to call again after the mode changes.</summary>
    public static void Apply(Control root) // apply the colors to a screen and everything inside it
    {
        var all = new List<Control> { root }; // a list with the screen itself
        for (int i = 0; i < all.Count; i++) all.AddRange(all[i].Controls.Cast<Control>()); // add all controls inside it (and inside those)

        // remember the designer colours first: children inherit, so they must be read before any parent changes
        foreach (var c in all) Originals.GetValue(c, x => (x.BackColor, x.ForeColor)); // save the original colors once

        foreach (var c in all) // for each item
        {
            Originals.TryGetValue(c, out object o); // get its original colors
            var (back, fore) = ((Color, Color))o; // split the background color and the text color
            if (BackMap.TryGetValue(back.ToArgb(), out var b)) c.BackColor = b(); // if the background is in the table, swap it
            if (ForeMap.TryGetValue(fore.ToArgb(), out var f)) c.ForeColor = f(); // if the text is in the table, swap it

            switch (c) // based on the control type
            {
                case DataGridView g: ColorGrid(g); break; // table: color it its own way
                case LinkLabel l: l.LinkColor = PrimaryText; break; // link: make it a readable blue
                // the standard drop-down list ignores BackColor, which leaves light text on a light box in dark mode
                case ComboBox cb: cb.FlatStyle = Dark ? FlatStyle.Flat : FlatStyle.Standard; break; // drop-down list: flat look in dark mode
                case Button { FlatStyle: FlatStyle.Flat } btn: // flat button
                    Color shade = btn.BackColor.GetBrightness() < 0.5f ? Color.White : Color.Black; // if the button is dark make it lighter, if light make it darker
                    btn.FlatAppearance.MouseOverBackColor = Blend(btn.BackColor, shade, 0.12); // its color when the mouse is over it
                    btn.FlatAppearance.MouseDownBackColor = Blend(btn.BackColor, shade, 0.24); // its color when it is pressed
                    btn.Cursor = Cursors.Hand; // hand cursor
                    break; // done
            }
        }
        root.Invalidate(true); // redraw the whole screen
    }

    public static void ToggleDark() // switch between dark and light mode
    {
        Dark = !Dark; // flip the mode
        try // try to save the choice
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DarkFlag)); // create the folder if it does not exist
            if (Dark) File.WriteAllBytes(DarkFlag, Array.Empty<byte>()); else File.Delete(DarkFlag); // dark: create the file, light: delete it
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { } // only the remembered choice is lost
        foreach (Form f in Application.OpenForms) Apply(f); // apply the new colors to all open screens
    }

    static Color Blend(Color a, Color b, double t) => Color.FromArgb( // mix two colors by amount t
        (int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t)); // for each channel (red, green, blue)

    /// <summary>List grid look used on every page. Set <c>Tag</c> to a string to show it while the grid is empty.</summary>
    public static void StyleGrid(DataGridView g, bool readOnly = true) // style the table
    {
        g.BorderStyle = BorderStyle.None; // no border
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // only horizontal lines between rows
        g.EnableHeadersVisualStyles = false; // turn off the Windows style so we can color the headers ourselves
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; // column headers without borders
        g.ColumnHeadersDefaultCellStyle.BackColor = Primary; // blue header background
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // white header text
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Primary; // the header color does not change when selected
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold); // bold header font
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; // header height cannot be changed
        g.ColumnHeadersHeight = 38; // header height
        g.RowTemplate.Height = 32; // height of each row
        g.RowHeadersVisible = false; // hide the column on the left
        g.AllowUserToResizeRows = false; // row height cannot be changed
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // columns fill the table width
        ColorGrid(g); // table color
        g.Paint += (s, e) => // when the table is drawn
        {
            if (g.RowCount > 0 || g.Tag is not string message) return; // if there are rows or no message, do nothing
            var area = new Rectangle(0, g.ColumnHeadersHeight, g.Width, Math.Min(g.Height - g.ColumnHeadersHeight, 160)); // the space under the headers
            TextRenderer.DrawText(e.Graphics, message, g.Font, area, Muted, // write the "table is empty" message
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak); // in the middle
        };
        if (!readOnly) return; // if the table is editable, stop here
        g.ReadOnly = true; // read only
        g.AllowUserToAddRows = false; // rows cannot be added
        g.AllowUserToDeleteRows = false; // rows cannot be deleted
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // select the whole row
        g.MultiSelect = false; // only one row can be selected
    }

    static void ColorGrid(DataGridView g) // color the table based on the mode
    {
        g.BackgroundColor = Surface; // table background
        g.GridColor = Border; // line color
        g.DefaultCellStyle.BackColor = Surface; // cell background
        g.DefaultCellStyle.ForeColor = Text; // cell text color
        g.DefaultCellStyle.SelectionBackColor = PrimaryLight; // selected row background
        g.DefaultCellStyle.SelectionForeColor = Text; // selected row text
        g.AlternatingRowsDefaultCellStyle.BackColor = SurfaceAlt; // alternating row color
    }

    public static GraphicsPath RoundedRect(RectangleF r, float radius) // make a rectangle shape with rounded corners
    {
        float d = radius * 2; // corner diameter
        var p = new GraphicsPath(); // new drawing path
        p.AddArc(r.X, r.Y, d, d, 180, 90); // top left corner
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90); // top right corner
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90); // bottom right corner
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90); // bottom left corner
        p.CloseFigure(); // close the shape
        return p; // return the shape
    }

    public static bool Confirm(string text) => // confirm message (yes/no)
        MessageBox.Show(text, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; // returns true if yes was clicked

    static Toast toast; // the small notification shown now

    /// <summary>Success note in the corner of the active window; it goes away by itself and never takes focus.</summary>
    public static void Info(string text) // show a small success notification
    {
        var owner = Form.ActiveForm; // the active screen now
        if (owner == null) // if there is no active screen
        {
            MessageBox.Show(text, "Train Reservation", MessageBoxButtons.OK, MessageBoxIcon.Information); // show a normal message
            return; // and stop
        }
        toast?.Close(); // close the old notification if there is one
        toast = new Toast(owner, text); // make a new notification
        toast.FormClosed += (s, e) => { if (toast == s) toast = null; }; // when it closes, clear the variable
        toast.Show(owner); // show it
    }

    public static void Warn(string text) => // warning message
        MessageBox.Show(text, "Train Reservation", MessageBoxButtons.OK, MessageBoxIcon.Warning); // with a warning icon

    sealed class Toast : Form // small window for the notification
    {
        readonly System.Windows.Forms.Timer timer = new() { Interval = 3000 }; // 3 second timer

        protected override bool ShowWithoutActivation => true; // shows without taking focus from the screen

        public Toast(Form owner, string text) // create the notification
        {
            FormBorderStyle = FormBorderStyle.None; // no border
            ShowInTaskbar = false; // do not show it in the taskbar
            StartPosition = FormStartPosition.Manual; // we set its position ourselves
            BackColor = Success; // green background
            var label = new Label // the text
            {
                Text = "✓  " + text, AutoSize = true, ForeColor = Color.White, // check mark + the message in white
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), Location = new Point(18, 12) // font and position
            };
            Controls.Add(label); // add the text to the notification
            ClientSize = new Size(label.PreferredWidth + 36, label.PreferredHeight + 24); // notification size fits the text
            // top-right, so it never covers the action buttons along the bottom of a page
            var corner = owner.PointToScreen(new Point(owner.ClientSize.Width, 0)); // top right corner of the screen
            Location = new Point(corner.X - Width - 24, corner.Y + 12); // put the notification there
            label.Click += (s, e) => Close(); // clicking the text closes it
            Click += (s, e) => Close(); // clicking the notification closes it
            timer.Tick += (s, e) => Close(); // close after 3 seconds
            timer.Start(); // start the timer
        }

        protected override void Dispose(bool disposing) // clean up memory
        {
            if (disposing) timer.Dispose(); // delete the timer
            base.Dispose(disposing); // continue the normal cleanup
        }
    }
}
