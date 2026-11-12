using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Pages; // the pages
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Forms; // namespace for the screens

public partial class MainForm : Form // the main screen after login
{
    public bool LoggedOut { get; private set; } // did the user log out?

    private readonly Button[] navButtons; // this role's pages, top to bottom: Ctrl+1, Ctrl+2, ...

    public MainForm() // create the screen
    {
        InitializeComponent(); // build the controls from the designer file
        var user = SampleData.CurrentUser; // the current user
        lblUser.Text = $"{user.FullName}  ·  {user.Role}"; // write their name and role

        // one form, two roles: only the role's buttons are shown, stacked from the top
        var all = new[] { btnDashboard, btnStations, btnTrains, btnTrips, btnReports, btnSearch, btnMyTickets }; // all menu buttons
        var mine = user.Role == UserRole.Admin // if admin
            ? new[] { btnDashboard, btnStations, btnTrains, btnTrips, btnReports } // admin buttons
            : new[] { btnDashboard, btnSearch, btnMyTickets }; // otherwise passenger buttons
        int top = btnDashboard.Top, step = btnStations.Top - btnDashboard.Top; // already DPI-scaled by the designer
        foreach (var b in all) b.Visible = mine.Contains(b); // show only the buttons for this role
        for (int i = 0; i < mine.Length; i++) mine[i].Top = top + i * step; // stack them from the top
        navButtons = mine; // save the visible buttons

        Theme.Apply(this); // apply the colors
        btnTheme.Text = Theme.Dark ? "Light mode" : "Dark mode"; // text of the theme button

        Navigate(btnDashboard); // open the dashboard first
    }

    private void Nav_Click(object sender, EventArgs e) => Navigate((Button)sender); // when a menu button is clicked, go to its page

    private void Navigate(Button nav) // open a page
    {
        UserControl page = // pick the page based on the button
            nav == btnStations ? new StationsPage() : // stations page
            nav == btnTrains ? new TrainsPage() : // trains page
            nav == btnTrips ? new TripsPage() : // trips page
            nav == btnReports ? new ReportsPage() : // reports page
            nav == btnSearch ? new SearchTripsPage() : // search page
            nav == btnMyTickets ? new MyTicketsPage() : // my tickets page
            new DashboardPage(); // otherwise the dashboard

        foreach (Control c in pnlContent.Controls) c.Dispose(); // free the old page from memory
        pnlContent.Controls.Clear(); // clear the content area
        Theme.Apply(page); // color of the new page
        page.Dock = DockStyle.Fill; // make it fill the area
        pnlContent.Controls.Add(page); // add it
        lblPageTitle.Text = nav.Text; // keep "&&": a label also treats a single & as a shortcut marker and hides it

        foreach (var b in navButtons) // for each menu button
            b.BackColor = b == nav ? Theme.Primary : pnlSidebar.BackColor; // color the current button, the rest stay normal
        page.SelectNextControl(null, true, true, true, false); // keyboard starts in the page, not the sidebar
    }

    private void btnProfile_Click(object sender, EventArgs e) // when the profile button is clicked
    {
        var user = SampleData.CurrentUser; // the current user
        using var form = new RegisterForm(user); // open the edit profile screen
        if (form.ShowDialog(this) == DialogResult.OK) lblUser.Text = $"{user.FullName}  ·  {user.Role}"; // if saved, update the name
    }

    private void btnTheme_Click(object sender, EventArgs e) // when the dark mode button is clicked
    {
        Theme.ToggleDark(); // switch the mode
        btnTheme.Text = Theme.Dark ? "Light mode" : "Dark mode"; // change the button text
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // keyboard shortcuts
    {
        int n = keyData - (Keys.Control | Keys.D1); // Ctrl+1 = 0, Ctrl+2 = 1 ...
        if (n >= 0 && n < navButtons.Length) { Navigate(navButtons[n]); return true; } // open page number n
        if (keyData == (Keys.Control | Keys.D)) { btnTheme.PerformClick(); return true; } // Ctrl+D switches dark mode
        return base.ProcessCmdKey(ref msg, keyData); // other keys work as normal
    }

    private void btnLogout_Click(object sender, EventArgs e) // when the logout button is clicked
    {
        if (!Theme.Confirm("Log out of your account?")) return; // ask the user to confirm, if not, stop
        LoggedOut = true; // remember that the user logged out
        Close(); // close the screen
    }
}
