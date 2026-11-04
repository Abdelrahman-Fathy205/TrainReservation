namespace TrainReservation.Client.Pages;

partial class ReportsPage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        lblStatus = new Label();
        progress = new ProgressBar();
        btnRefresh = new Button();
        dtpTo = new DateTimePicker();
        lblTo = new Label();
        dtpFrom = new DateTimePicker();
        lblFrom = new Label();
        pnlSummary = new Panel();
        lblSummary = new Label();
        tblCharts = new TableLayoutPanel();
        chartRevenue = new TrainReservation.Client.Controls.ChartControl();
        chartClass = new TrainReservation.Client.Controls.ChartControl();
        chartDaily = new TrainReservation.Client.Controls.ChartControl();
        chartOccupancy = new TrainReservation.Client.Controls.ChartControl();
        pnlTop.SuspendLayout();
        pnlSummary.SuspendLayout();
        tblCharts.SuspendLayout();
        SuspendLayout();
        //
        // pnlTop
        //
        pnlTop.Controls.Add(lblStatus);
        pnlTop.Controls.Add(progress);
        pnlTop.Controls.Add(btnRefresh);
        pnlTop.Controls.Add(dtpTo);
        pnlTop.Controls.Add(lblTo);
        pnlTop.Controls.Add(dtpFrom);
        pnlTop.Controls.Add(lblFrom);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Name = "pnlTop";
        pnlTop.Size = new Size(1000, 52);
        pnlTop.TabIndex = 0;
        //
        // lblFrom
        //
        lblFrom.AutoSize = true;
        lblFrom.Location = new Point(0, 12);
        lblFrom.Name = "lblFrom";
        lblFrom.Text = "From";
        //
        // dtpFrom
        //
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(50, 8);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(140, 30);
        dtpFrom.TabIndex = 0;
        //
        // lblTo
        //
        lblTo.AutoSize = true;
        lblTo.Location = new Point(204, 12);
        lblTo.Name = "lblTo";
        lblTo.Text = "To";
        //
        // dtpTo
        //
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(236, 8);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(140, 30);
        dtpTo.TabIndex = 1;
        //
        // btnRefresh
        //
        btnRefresh.BackColor = Color.FromArgb(30, 58, 138);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(392, 6);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(140, 34);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "Generate report";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        //
        // progress
        //
        progress.Location = new Point(548, 14);
        progress.MarqueeAnimationSpeed = 30;
        progress.Name = "progress";
        progress.Size = new Size(140, 18);
        progress.Style = ProgressBarStyle.Marquee;
        progress.Visible = false;
        //
        // lblStatus
        //
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = Color.FromArgb(107, 114, 128);
        lblStatus.Location = new Point(700, 12);
        lblStatus.Name = "lblStatus";
        lblStatus.Text = "";
        //
        // pnlSummary
        //
        pnlSummary.BackColor = Color.White;
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Top;
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Padding = new Padding(16, 0, 0, 0);
        pnlSummary.Size = new Size(1000, 48);
        //
        // lblSummary
        //
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(30, 58, 138);
        lblSummary.Name = "lblSummary";
        lblSummary.Text = "—";
        lblSummary.TextAlign = ContentAlignment.MiddleLeft;
        //
        // tblCharts
        //
        tblCharts.ColumnCount = 2;
        tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        tblCharts.Controls.Add(chartRevenue, 0, 0);
        tblCharts.Controls.Add(chartClass, 1, 0);
        tblCharts.Controls.Add(chartDaily, 0, 1);
        tblCharts.Controls.Add(chartOccupancy, 1, 1);
        tblCharts.Dock = DockStyle.Fill;
        tblCharts.Name = "tblCharts";
        tblCharts.Padding = new Padding(0, 10, 0, 0);
        tblCharts.RowCount = 2;
        tblCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tblCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tblCharts.TabIndex = 3;
        //
        // chartRevenue
        //
        chartRevenue.Dock = DockStyle.Fill;
        chartRevenue.Name = "chartRevenue";
        chartRevenue.Text = "Revenue by route (EGP)";
        //
        // chartClass
        //
        chartClass.Dock = DockStyle.Fill;
        chartClass.Kind = TrainReservation.Client.Controls.ChartKind.Pie;
        chartClass.Name = "chartClass";
        chartClass.Text = "Tickets by class";
        //
        // chartDaily
        //
        chartDaily.Dock = DockStyle.Fill;
        chartDaily.Name = "chartDaily";
        chartDaily.Text = "Tickets per day (last 7 days of range)";
        //
        // chartOccupancy
        //
        chartOccupancy.Dock = DockStyle.Fill;
        chartOccupancy.Name = "chartOccupancy";
        chartOccupancy.Text = "Average occupancy by train (%)";
        //
        // ReportsPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(tblCharts);
        Controls.Add(pnlSummary);
        Controls.Add(pnlTop);
        Font = new Font("Segoe UI", 10F);
        Name = "ReportsPage";
        Size = new Size(1000, 640);
        Load += ReportsPage_Load;
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlSummary.ResumeLayout(false);
        tblCharts.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblFrom;
    private DateTimePicker dtpFrom;
    private Label lblTo;
    private DateTimePicker dtpTo;
    private Button btnRefresh;
    private ProgressBar progress;
    private Label lblStatus;
    private Panel pnlSummary;
    private Label lblSummary;
    private TableLayoutPanel tblCharts;
    private TrainReservation.Client.Controls.ChartControl chartRevenue;
    private TrainReservation.Client.Controls.ChartControl chartClass;
    private TrainReservation.Client.Controls.ChartControl chartDaily;
    private TrainReservation.Client.Controls.ChartControl chartOccupancy;
}
