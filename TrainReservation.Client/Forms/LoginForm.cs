using System.Drawing.Drawing2D; // advanced drawing library
using TrainReservation.Client.Services; // services (data and passwords)
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Forms; // namespace for the screens

public partial class LoginForm : Form // login screen
{
    public LoginForm() // create the screen
    {
        InitializeComponent(); // build the screen controls from the designer file
        Theme.Apply(this); // apply the colors
    }

    private void btnLogin_Click(object sender, EventArgs e) // when the login button is clicked
    {
        lblError.Text = ""; // clear the old error message
        string username = txtUsername.Text.Trim(); // username without extra spaces
        if (username.Length == 0 || txtPassword.Text.Length == 0) // if the username or password is empty
        {
            lblError.Text = "Please enter your username and password."; // ask the user to fill them in
            return; // stop
        }

        var user = SampleData.Users.FirstOrDefault(u => // look for a user
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && Passwords.Verify(txtPassword.Text, u.PasswordHash)); // the name matches (ignoring case) and the password is correct
        if (user == null) // if not found
        {
            lblError.Text = "Invalid username or password."; // error message
            txtPassword.SelectAll(); // select the password
            txtPassword.Focus(); // and put the cursor on it
            return; // stop
        }

        SampleData.CurrentUser = user; // save the current user
        Hide(); // hide the login screen
        bool loggedOut; // did the user log out?
        using (var main = new MainForm()) // open the main screen
        {
            main.ShowDialog(); // show it and wait until it closes
            loggedOut = main.LoggedOut; // check if the user logged out or closed the program
        }
        if (!loggedOut) { Close(); return; } // if the program was closed, close the login screen too

        SampleData.CurrentUser = null; // clear the current user
        txtPassword.Clear(); // clear the password
        Theme.Apply(this); // dark mode may have been switched while this window was hidden
        Show(); // show the login screen again
        txtUsername.Focus(); // put the cursor on the username box
    }

    private void chkShowPassword_CheckedChanged(object sender, EventArgs e) => // when the "show password" box changes
        txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked; // show or hide the password

    private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) // when the register link is clicked
    {
        using var form = new RegisterForm(); // open the register screen
        if (form.ShowDialog(this) != DialogResult.OK) return; // if registering was canceled, stop
        txtUsername.Text = form.CreatedUsername; // write the new account name
        txtPassword.Clear(); // clear the password
        txtPassword.Focus(); // put the cursor on the password
        lblError.Text = ""; // clear the error message
    }

    /// <summary>GDI+ train illustration on the brand panel.</summary>
    private void pnlBrand_Paint(object sender, PaintEventArgs e) // draw the train picture on the panel
    {
        var g = e.Graphics; // the drawing tool
        g.SmoothingMode = SmoothingMode.AntiAlias; // smooth edges
        float k = DeviceDpi / 96f; // drawing below is in 96-DPI units
        g.ScaleTransform(k, k); // scale the drawing based on screen DPI
        int x = 40, y = 90, width = (int)(pnlBrand.Width / k); // train position and panel width

        using var rail = new Pen(Color.FromArgb(96, 165, 250), 3); // rail pen
        g.DrawLine(rail, 20, y + 128, width - 20, y + 128); // draw the rail
        using var sleeper = new Pen(Color.FromArgb(59, 130, 246), 4); // sleepers pen
        for (int sx = 30; sx < width - 20; sx += 22) g.DrawLine(sleeper, sx, y + 128, sx, y + 136); // draw the sleepers every 22 pixels

        using (var body = Theme.RoundedRect(new RectangleF(x, y, 300, 100), 20)) // train body with rounded corners
        using (var bodyBrush = new LinearGradientBrush(new Rectangle(x, y, 300, 100), Color.White, Color.FromArgb(219, 234, 254), 90f)) // gradient from white to light blue
            g.FillPath(bodyBrush, body); // train body color

        using var stripe = new SolidBrush(Theme.Accent); // orange stripe brush
        g.FillRectangle(stripe, x, y + 70, 300, 8); // draw the stripe

        using var glass = new SolidBrush(Color.FromArgb(30, 58, 138)); // windows brush
        for (int i = 0; i < 5; i++) // 5 windows
            using (var w = Theme.RoundedRect(new RectangleF(x + 20 + i * 54, y + 18, 40, 36), 6)) // window shape
                g.FillPath(glass, w); // draw the window

        using var wheel = new SolidBrush(Color.FromArgb(17, 24, 39)); // wheel brush
        foreach (int wx in new[] { 40, 90, 230, 280 }) // wheel positions
            g.FillEllipse(wheel, x + wx - 14, y + 94, 28, 28); // draw the wheel
    }
}
