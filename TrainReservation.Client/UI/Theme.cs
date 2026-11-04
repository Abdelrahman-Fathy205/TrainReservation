using System.Drawing.Drawing2D;

namespace TrainReservation.Client.UI;

public static class Theme
{
    public static readonly Color Primary = Color.FromArgb(30, 58, 138);
    public static readonly Color PrimaryLight = Color.FromArgb(219, 234, 254);
    public static readonly Color Accent = Color.FromArgb(245, 158, 11);
    public static readonly Color Background = Color.FromArgb(243, 244, 246);
    public static readonly Color Text = Color.FromArgb(17, 24, 39);
    public static readonly Color Muted = Color.FromArgb(107, 114, 128);
    public static readonly Color Border = Color.FromArgb(209, 213, 219);
    public static readonly Color Success = Color.FromArgb(22, 163, 74);

    /// <summary>Read-only list grid look used on every page.</summary>
    public static void StyleGrid(DataGridView g, bool readOnly = true)
    {
        g.BackgroundColor = Color.White;
        g.BorderStyle = BorderStyle.None;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.GridColor = Border;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersDefaultCellStyle.BackColor = Primary;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Primary;
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 38;
        g.DefaultCellStyle.SelectionBackColor = PrimaryLight;
        g.DefaultCellStyle.SelectionForeColor = Text;
        g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
        g.RowTemplate.Height = 32;
        g.RowHeadersVisible = false;
        g.AllowUserToResizeRows = false;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        if (!readOnly) return;
        g.ReadOnly = true;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.MultiSelect = false;
    }

    public static GraphicsPath RoundedRect(RectangleF r, float radius)
    {
        float d = radius * 2;
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    public static bool Confirm(string text) =>
        MessageBox.Show(text, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    public static void Info(string text) =>
        MessageBox.Show(text, "Train Reservation", MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Warn(string text) =>
        MessageBox.Show(text, "Train Reservation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
