using TrainReservation.Client.Models;
using TrainReservation.Client.Pages;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Forms;

public partial class MainForm : Form
{
    public bool LoggedOut { get; private set; }

    public MainForm()
    {
        InitializeComponent();
        var user = SampleData.CurrentUser;
        lblUser.Text = $"{user.FullName}  ·  {user.Role}";

        // one form, two roles: only the role's buttons are shown, stacked from the top
        var all = new[] { btnDashboard, btnStations, btnTrains, btnTrips, btnReports, btnSearch, btnMyTickets };
        var mine = user.Role == UserRole.Admin
            ? new[] { btnDashboard, btnStations, btnTrains, btnTrips, btnReports }
            : new[] { btnDashboard, btnSearch, btnMyTickets };
        int top = btnDashboard.Top, step = btnStations.Top - btnDashboard.Top; // already DPI-scaled by the designer
        foreach (var b in all) b.Visible = mine.Contains(b);
        for (int i = 0; i < mine.Length; i++) mine[i].Top = top + i * step;

        Navigate(btnDashboard);
    }

    private void Nav_Click(object sender, EventArgs e) => Navigate((Button)sender);

    private void Navigate(Button nav)
    {
        UserControl page =
            nav == btnStations ? new StationsPage() :
            nav == btnTrains ? new TrainsPage() :
            nav == btnTrips ? new TripsPage() :
            nav == btnReports ? new ReportsPage() :
            nav == btnSearch ? new SearchTripsPage() :
            nav == btnMyTickets ? new MyTicketsPage() :
            new DashboardPage();

        foreach (Control c in pnlContent.Controls) c.Dispose();
        pnlContent.Controls.Clear();
        page.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(page);
        lblPageTitle.Text = nav.Text.Replace("&&", "&");

        foreach (var b in pnlSidebar.Controls.OfType<Button>().Where(b => b != btnLogout))
            b.BackColor = b == nav ? Theme.Primary : pnlSidebar.BackColor;
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        if (!Theme.Confirm("Log out of your account?")) return;
        LoggedOut = true;
        Close();
    }
}
