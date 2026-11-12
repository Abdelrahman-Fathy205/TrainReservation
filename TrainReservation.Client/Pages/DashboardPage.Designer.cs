namespace TrainReservation.Client.Pages;

partial class DashboardPage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        flowCards = new FlowLayoutPanel();
        card1 = new Panel();
        lblValue1 = new Label();
        lblTitle1 = new Label();
        card2 = new Panel();
        lblValue2 = new Label();
        lblTitle2 = new Label();
        card3 = new Panel();
        lblValue3 = new Label();
        lblTitle3 = new Label();
        card4 = new Panel();
        lblValue4 = new Label();
        lblTitle4 = new Label();
        pnlNextHost = new Panel();
        pnlNext = new Panel();
        lblNextTitle = new Label();
        lblNextRoute = new Label();
        lblNextDetail = new Label();
        btnNextPrint = new Button();
        lblList = new Label();
        dgv = new DataGridView();
        flowCards.SuspendLayout();
        card1.SuspendLayout();
        card2.SuspendLayout();
        card3.SuspendLayout();
        card4.SuspendLayout();
        pnlNextHost.SuspendLayout();
        pnlNext.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
        //
        // flowCards
        //
        flowCards.Controls.Add(card1);
        flowCards.Controls.Add(card2);
        flowCards.Controls.Add(card3);
        flowCards.Controls.Add(card4);
        flowCards.Dock = DockStyle.Top;
        flowCards.Name = "flowCards";
        flowCards.Size = new Size(1000, 124);
        flowCards.TabIndex = 0;
        //
        // card1
        //
        card1.BackColor = Color.White;
        card1.Controls.Add(lblValue1);
        card1.Controls.Add(lblTitle1);
        card1.Margin = new Padding(0, 0, 16, 16);
        card1.Name = "card1";
        card1.Size = new Size(230, 104);
        //
        // lblTitle1
        //
        lblTitle1.AutoSize = true;
        lblTitle1.ForeColor = Color.FromArgb(107, 114, 128);
        lblTitle1.Location = new Point(16, 14);
        lblTitle1.Name = "lblTitle1";
        lblTitle1.Text = "Title";
        //
        // lblValue1
        //
        lblValue1.AutoSize = true;
        lblValue1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblValue1.ForeColor = Color.FromArgb(30, 58, 138);
        lblValue1.Location = new Point(12, 44);
        lblValue1.Name = "lblValue1";
        lblValue1.Text = "0";
        //
        // card2
        //
        card2.BackColor = Color.White;
        card2.Controls.Add(lblValue2);
        card2.Controls.Add(lblTitle2);
        card2.Margin = new Padding(0, 0, 16, 16);
        card2.Name = "card2";
        card2.Size = new Size(230, 104);
        //
        // lblTitle2
        //
        lblTitle2.AutoSize = true;
        lblTitle2.ForeColor = Color.FromArgb(107, 114, 128);
        lblTitle2.Location = new Point(16, 14);
        lblTitle2.Name = "lblTitle2";
        lblTitle2.Text = "Title";
        //
        // lblValue2
        //
        lblValue2.AutoSize = true;
        lblValue2.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblValue2.ForeColor = Color.FromArgb(30, 58, 138);
        lblValue2.Location = new Point(12, 44);
        lblValue2.Name = "lblValue2";
        lblValue2.Text = "0";
        //
        // card3
        //
        card3.BackColor = Color.White;
        card3.Controls.Add(lblValue3);
        card3.Controls.Add(lblTitle3);
        card3.Margin = new Padding(0, 0, 16, 16);
        card3.Name = "card3";
        card3.Size = new Size(230, 104);
        //
        // lblTitle3
        //
        lblTitle3.AutoSize = true;
        lblTitle3.ForeColor = Color.FromArgb(107, 114, 128);
        lblTitle3.Location = new Point(16, 14);
        lblTitle3.Name = "lblTitle3";
        lblTitle3.Text = "Title";
        //
        // lblValue3
        //
        lblValue3.AutoSize = true;
        lblValue3.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblValue3.ForeColor = Color.FromArgb(30, 58, 138);
        lblValue3.Location = new Point(12, 44);
        lblValue3.Name = "lblValue3";
        lblValue3.Text = "0";
        //
        // card4
        //
        card4.BackColor = Color.White;
        card4.Controls.Add(lblValue4);
        card4.Controls.Add(lblTitle4);
        card4.Margin = new Padding(0, 0, 16, 16);
        card4.Name = "card4";
        card4.Size = new Size(230, 104);
        //
        // lblTitle4
        //
        lblTitle4.AutoSize = true;
        lblTitle4.ForeColor = Color.FromArgb(107, 114, 128);
        lblTitle4.Location = new Point(16, 14);
        lblTitle4.Name = "lblTitle4";
        lblTitle4.Text = "Title";
        //
        // lblValue4
        //
        lblValue4.AutoSize = true;
        lblValue4.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblValue4.ForeColor = Color.FromArgb(245, 158, 11);
        lblValue4.Location = new Point(12, 44);
        lblValue4.Name = "lblValue4";
        lblValue4.Text = "0";
        //
        // pnlNextHost
        //
        pnlNextHost.Controls.Add(pnlNext);
        pnlNextHost.Dock = DockStyle.Top;
        pnlNextHost.Name = "pnlNextHost";
        pnlNextHost.Padding = new Padding(0, 0, 0, 16);
        pnlNextHost.Size = new Size(1000, 124);
        pnlNextHost.TabIndex = 2;
        pnlNextHost.Visible = false;
        //
        // pnlNext
        //
        pnlNext.BackColor = Color.FromArgb(30, 58, 138);
        pnlNext.Controls.Add(btnNextPrint);
        pnlNext.Controls.Add(lblNextDetail);
        pnlNext.Controls.Add(lblNextRoute);
        pnlNext.Controls.Add(lblNextTitle);
        pnlNext.Dock = DockStyle.Fill;
        pnlNext.Name = "pnlNext";
        //
        // lblNextTitle
        //
        lblNextTitle.AutoSize = true;
        lblNextTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNextTitle.ForeColor = Color.FromArgb(147, 197, 253);
        lblNextTitle.Location = new Point(20, 12);
        lblNextTitle.Name = "lblNextTitle";
        lblNextTitle.Text = "YOUR NEXT TRIP";
        //
        // lblNextRoute
        //
        lblNextRoute.AutoSize = true;
        lblNextRoute.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblNextRoute.ForeColor = Color.White;
        lblNextRoute.Location = new Point(16, 30);
        lblNextRoute.Name = "lblNextRoute";
        lblNextRoute.Text = "From → To";
        //
        // lblNextDetail
        //
        lblNextDetail.AutoSize = true;
        lblNextDetail.ForeColor = Color.White;
        lblNextDetail.Location = new Point(20, 74);
        lblNextDetail.Name = "lblNextDetail";
        lblNextDetail.Text = "Details";
        //
        // btnNextPrint
        //
        btnNextPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNextPrint.BackColor = Color.FromArgb(245, 158, 11);
        btnNextPrint.FlatAppearance.BorderSize = 0;
        btnNextPrint.FlatStyle = FlatStyle.Flat;
        btnNextPrint.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnNextPrint.ForeColor = Color.White;
        btnNextPrint.Location = new Point(820, 33);
        btnNextPrint.Name = "btnNextPrint";
        btnNextPrint.Size = new Size(160, 42);
        btnNextPrint.TabIndex = 0;
        btnNextPrint.Text = "Print ticket";
        btnNextPrint.UseVisualStyleBackColor = false;
        btnNextPrint.Click += btnNextPrint_Click;
        //
        // lblList
        //
        lblList.Dock = DockStyle.Top;
        lblList.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblList.ForeColor = Color.FromArgb(17, 24, 39);
        lblList.Name = "lblList";
        lblList.Size = new Size(1000, 36);
        lblList.Text = "Upcoming departures";
        //
        // dgv
        //
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 1;
        //
        // DashboardPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(dgv);
        Controls.Add(lblList);
        Controls.Add(pnlNextHost);
        Controls.Add(flowCards);
        Font = new Font("Segoe UI", 10F);
        Name = "DashboardPage";
        Size = new Size(1000, 640);
        flowCards.ResumeLayout(false);
        card1.ResumeLayout(false);
        card1.PerformLayout();
        card2.ResumeLayout(false);
        card2.PerformLayout();
        card3.ResumeLayout(false);
        card3.PerformLayout();
        card4.ResumeLayout(false);
        card4.PerformLayout();
        pnlNextHost.ResumeLayout(false);
        pnlNext.ResumeLayout(false);
        pnlNext.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
    }

    private FlowLayoutPanel flowCards;
    private Panel card1;
    private Label lblTitle1;
    private Label lblValue1;
    private Panel card2;
    private Label lblTitle2;
    private Label lblValue2;
    private Panel card3;
    private Label lblTitle3;
    private Label lblValue3;
    private Panel card4;
    private Label lblTitle4;
    private Label lblValue4;
    private Panel pnlNextHost;
    private Panel pnlNext;
    private Label lblNextTitle;
    private Label lblNextRoute;
    private Label lblNextDetail;
    private Button btnNextPrint;
    private Label lblList;
    private DataGridView dgv;
}
