namespace TrainReservation.Client.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlSidebar = new Panel();
        btnProfile = new Button();
        btnTheme = new Button();
        btnLogout = new Button();
        btnMyTickets = new Button();
        btnSearch = new Button();
        btnReports = new Button();
        btnTrips = new Button();
        btnTrains = new Button();
        btnStations = new Button();
        btnDashboard = new Button();
        lblAppSub = new Label();
        lblAppName = new Label();
        pnlHeader = new Panel();
        lblUser = new Label();
        lblPageTitle = new Label();
        pnlContent = new Panel();
        pnlSidebar.SuspendLayout();
        pnlHeader.SuspendLayout();
        SuspendLayout();
        //
        // pnlSidebar
        //
        pnlSidebar.BackColor = Color.FromArgb(23, 37, 84);
        pnlSidebar.Controls.Add(btnProfile); // added first so it docks above Dark mode / Log out
        pnlSidebar.Controls.Add(btnTheme);
        pnlSidebar.Controls.Add(btnLogout);
        pnlSidebar.Controls.Add(btnMyTickets);
        pnlSidebar.Controls.Add(btnSearch);
        pnlSidebar.Controls.Add(btnReports);
        pnlSidebar.Controls.Add(btnTrips);
        pnlSidebar.Controls.Add(btnTrains);
        pnlSidebar.Controls.Add(btnStations);
        pnlSidebar.Controls.Add(btnDashboard);
        pnlSidebar.Controls.Add(lblAppSub);
        pnlSidebar.Controls.Add(lblAppName);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(230, 760);
        pnlSidebar.TabIndex = 0;
        //
        // lblAppName
        //
        lblAppName.AutoSize = true;
        lblAppName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblAppName.ForeColor = Color.White;
        lblAppName.Location = new Point(20, 24);
        lblAppName.Name = "lblAppName";
        lblAppName.Text = "RailBook";
        //
        // lblAppSub
        //
        lblAppSub.AutoSize = true;
        lblAppSub.Font = new Font("Segoe UI", 9F);
        lblAppSub.ForeColor = Color.FromArgb(147, 197, 253);
        lblAppSub.Location = new Point(23, 60);
        lblAppSub.Name = "lblAppSub";
        lblAppSub.Text = "Train Ticket Reservation";
        //
        // btnDashboard
        //
        btnDashboard.Cursor = Cursors.Hand;
        btnDashboard.FlatAppearance.BorderSize = 0;
        btnDashboard.FlatStyle = FlatStyle.Flat;
        btnDashboard.ForeColor = Color.White;
        btnDashboard.Location = new Point(0, 110);
        btnDashboard.Name = "btnDashboard";
        btnDashboard.Padding = new Padding(20, 0, 0, 0);
        btnDashboard.Size = new Size(230, 44);
        btnDashboard.TabIndex = 1;
        btnDashboard.Text = "Dashboard";
        btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
        btnDashboard.Click += Nav_Click;
        //
        // btnStations
        //
        btnStations.Cursor = Cursors.Hand;
        btnStations.FlatAppearance.BorderSize = 0;
        btnStations.FlatStyle = FlatStyle.Flat;
        btnStations.ForeColor = Color.White;
        btnStations.Location = new Point(0, 156);
        btnStations.Name = "btnStations";
        btnStations.Padding = new Padding(20, 0, 0, 0);
        btnStations.Size = new Size(230, 44);
        btnStations.TabIndex = 2;
        btnStations.Text = "Stations";
        btnStations.TextAlign = ContentAlignment.MiddleLeft;
        btnStations.Click += Nav_Click;
        //
        // btnTrains
        //
        btnTrains.Cursor = Cursors.Hand;
        btnTrains.FlatAppearance.BorderSize = 0;
        btnTrains.FlatStyle = FlatStyle.Flat;
        btnTrains.ForeColor = Color.White;
        btnTrains.Location = new Point(0, 202);
        btnTrains.Name = "btnTrains";
        btnTrains.Padding = new Padding(20, 0, 0, 0);
        btnTrains.Size = new Size(230, 44);
        btnTrains.TabIndex = 3;
        btnTrains.Text = "Trains";
        btnTrains.TextAlign = ContentAlignment.MiddleLeft;
        btnTrains.Click += Nav_Click;
        //
        // btnTrips
        //
        btnTrips.Cursor = Cursors.Hand;
        btnTrips.FlatAppearance.BorderSize = 0;
        btnTrips.FlatStyle = FlatStyle.Flat;
        btnTrips.ForeColor = Color.White;
        btnTrips.Location = new Point(0, 248);
        btnTrips.Name = "btnTrips";
        btnTrips.Padding = new Padding(20, 0, 0, 0);
        btnTrips.Size = new Size(230, 44);
        btnTrips.TabIndex = 4;
        btnTrips.Text = "Trips && Schedules";
        btnTrips.TextAlign = ContentAlignment.MiddleLeft;
        btnTrips.Click += Nav_Click;
        //
        // btnReports
        //
        btnReports.Cursor = Cursors.Hand;
        btnReports.FlatAppearance.BorderSize = 0;
        btnReports.FlatStyle = FlatStyle.Flat;
        btnReports.ForeColor = Color.White;
        btnReports.Location = new Point(0, 294);
        btnReports.Name = "btnReports";
        btnReports.Padding = new Padding(20, 0, 0, 0);
        btnReports.Size = new Size(230, 44);
        btnReports.TabIndex = 5;
        btnReports.Text = "Reports";
        btnReports.TextAlign = ContentAlignment.MiddleLeft;
        btnReports.Click += Nav_Click;
        //
        // btnSearch
        //
        btnSearch.Cursor = Cursors.Hand;
        btnSearch.FlatAppearance.BorderSize = 0;
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.ForeColor = Color.White;
        btnSearch.Location = new Point(0, 340);
        btnSearch.Name = "btnSearch";
        btnSearch.Padding = new Padding(20, 0, 0, 0);
        btnSearch.Size = new Size(230, 44);
        btnSearch.TabIndex = 6;
        btnSearch.Text = "Book a Ticket";
        btnSearch.TextAlign = ContentAlignment.MiddleLeft;
        btnSearch.Click += Nav_Click;
        //
        // btnMyTickets
        //
        btnMyTickets.Cursor = Cursors.Hand;
        btnMyTickets.FlatAppearance.BorderSize = 0;
        btnMyTickets.FlatStyle = FlatStyle.Flat;
        btnMyTickets.ForeColor = Color.White;
        btnMyTickets.Location = new Point(0, 386);
        btnMyTickets.Name = "btnMyTickets";
        btnMyTickets.Padding = new Padding(20, 0, 0, 0);
        btnMyTickets.Size = new Size(230, 44);
        btnMyTickets.TabIndex = 7;
        btnMyTickets.Text = "My Tickets";
        btnMyTickets.TextAlign = ContentAlignment.MiddleLeft;
        btnMyTickets.Click += Nav_Click;
        //
        // btnLogout
        //
        btnProfile.Cursor = Cursors.Hand;
        btnProfile.Dock = DockStyle.Bottom;
        btnProfile.FlatAppearance.BorderSize = 0;
        btnProfile.FlatStyle = FlatStyle.Flat;
        btnProfile.ForeColor = Color.FromArgb(147, 197, 253);
        btnProfile.Name = "btnProfile";
        btnProfile.Padding = new Padding(20, 0, 0, 0);
        btnProfile.Size = new Size(230, 44);
        btnProfile.TabIndex = 7;
        btnProfile.Text = "My profile";
        btnProfile.TextAlign = ContentAlignment.MiddleLeft;
        btnProfile.Click += btnProfile_Click;
        //
        // btnTheme
        //
        btnTheme.Cursor = Cursors.Hand;
        btnTheme.Dock = DockStyle.Bottom;
        btnTheme.FlatAppearance.BorderSize = 0;
        btnTheme.FlatStyle = FlatStyle.Flat;
        btnTheme.ForeColor = Color.FromArgb(147, 197, 253);
        btnTheme.Name = "btnTheme";
        btnTheme.Padding = new Padding(20, 0, 0, 0);
        btnTheme.Size = new Size(230, 44);
        btnTheme.TabIndex = 8;
        btnTheme.Text = "Dark mode";
        btnTheme.TextAlign = ContentAlignment.MiddleLeft;
        btnTheme.Click += btnTheme_Click;
        //
        // btnLogout
        //
        btnLogout.Cursor = Cursors.Hand;
        btnLogout.Dock = DockStyle.Bottom;
        btnLogout.FlatAppearance.BorderSize = 0;
        btnLogout.FlatStyle = FlatStyle.Flat;
        btnLogout.ForeColor = Color.FromArgb(252, 165, 165);
        btnLogout.Name = "btnLogout";
        btnLogout.Padding = new Padding(20, 0, 0, 0);
        btnLogout.Size = new Size(230, 50);
        btnLogout.TabIndex = 9;
        btnLogout.Text = "Log out";
        btnLogout.TextAlign = ContentAlignment.MiddleLeft;
        btnLogout.Click += btnLogout_Click;
        //
        // pnlHeader
        //
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblUser);
        pnlHeader.Controls.Add(lblPageTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1050, 64);
        pnlHeader.TabIndex = 1;
        //
        // lblPageTitle
        //
        lblPageTitle.AutoSize = true;
        lblPageTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblPageTitle.ForeColor = Color.FromArgb(17, 24, 39);
        lblPageTitle.Location = new Point(24, 14);
        lblPageTitle.Name = "lblPageTitle";
        lblPageTitle.Text = "Dashboard";
        //
        // lblUser
        //
        lblUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblUser.ForeColor = Color.FromArgb(107, 114, 128);
        lblUser.Location = new Point(700, 20);
        lblUser.Name = "lblUser";
        lblUser.Size = new Size(330, 24);
        lblUser.Text = "User";
        lblUser.TextAlign = ContentAlignment.MiddleRight;
        //
        // pnlContent
        //
        pnlContent.BackColor = Color.FromArgb(243, 244, 246);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(20);
        pnlContent.TabIndex = 2;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1280, 760);
        Controls.Add(pnlContent);
        Controls.Add(pnlHeader);
        Controls.Add(pnlSidebar);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1150, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Train Ticket Reservation System";
        pnlSidebar.ResumeLayout(false);
        pnlSidebar.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlSidebar;
    private Label lblAppName;
    private Label lblAppSub;
    private Button btnDashboard;
    private Button btnStations;
    private Button btnTrains;
    private Button btnTrips;
    private Button btnReports;
    private Button btnSearch;
    private Button btnMyTickets;
    private Button btnProfile;
    private Button btnTheme;
    private Button btnLogout;
    private Panel pnlHeader;
    private Label lblPageTitle;
    private Label lblUser;
    private Panel pnlContent;
}
