using System.Drawing.Drawing2D;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Forms;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string username = txtUsername.Text.Trim();
        if (username.Length == 0 || txtPassword.Text.Length == 0)
        {
            lblError.Text = "Please enter your username and password.";
            return;
        }

        var user = SampleData.Users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == txtPassword.Text);
        if (user == null)
        {
            lblError.Text = "Invalid username or password.";
            txtPassword.SelectAll();
            txtPassword.Focus();
            return;
        }

        SampleData.CurrentUser = user;
        Hide();
        bool loggedOut;
        using (var main = new MainForm())
        {
            main.ShowDialog();
            loggedOut = main.LoggedOut;
        }
        if (!loggedOut) { Close(); return; }

        SampleData.CurrentUser = null;
        txtPassword.Clear();
        Show();
        txtUsername.Focus();
    }

    private void chkShowPassword_CheckedChanged(object sender, EventArgs e) =>
        txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

    private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        using var form = new RegisterForm();
        if (form.ShowDialog(this) != DialogResult.OK) return;
        txtUsername.Text = form.CreatedUsername;
        txtPassword.Clear();
        txtPassword.Focus();
        lblError.Text = "";
    }

    /// <summary>GDI+ train illustration on the brand panel.</summary>
    private void pnlBrand_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float k = DeviceDpi / 96f; // drawing below is in 96-DPI units
        g.ScaleTransform(k, k);
        int x = 40, y = 90, width = (int)(pnlBrand.Width / k);

        using var rail = new Pen(Color.FromArgb(96, 165, 250), 3);
        g.DrawLine(rail, 20, y + 128, width - 20, y + 128);
        using var sleeper = new Pen(Color.FromArgb(59, 130, 246), 4);
        for (int sx = 30; sx < width - 20; sx += 22) g.DrawLine(sleeper, sx, y + 128, sx, y + 136);

        using (var body = Theme.RoundedRect(new RectangleF(x, y, 300, 100), 20))
        using (var bodyBrush = new LinearGradientBrush(new Rectangle(x, y, 300, 100), Color.White, Color.FromArgb(219, 234, 254), 90f))
            g.FillPath(bodyBrush, body);

        using var stripe = new SolidBrush(Theme.Accent);
        g.FillRectangle(stripe, x, y + 70, 300, 8);

        using var glass = new SolidBrush(Color.FromArgb(30, 58, 138));
        for (int i = 0; i < 5; i++)
            using (var w = Theme.RoundedRect(new RectangleF(x + 20 + i * 54, y + 18, 40, 36), 6))
                g.FillPath(glass, w);

        using var wheel = new SolidBrush(Color.FromArgb(17, 24, 39));
        foreach (int wx in new[] { 40, 90, 230, 280 })
            g.FillEllipse(wheel, x + wx - 14, y + 94, 28, 28);
    }
}
