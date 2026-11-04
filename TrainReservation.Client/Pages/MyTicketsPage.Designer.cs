namespace TrainReservation.Client.Pages;

partial class MyTicketsPage
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
        cboWhen = new ComboBox();
        lblWhen = new Label();
        txtSearch = new TextBox();
        lblSearch = new Label();
        pnlBottom = new Panel();
        lblTotal = new Label();
        btnPrint = new Button();
        dgv = new DataGridView();
        pnlTop.SuspendLayout();
        pnlBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
        //
        // pnlTop
        //
        pnlTop.Controls.Add(cboWhen);
        pnlTop.Controls.Add(lblWhen);
        pnlTop.Controls.Add(txtSearch);
        pnlTop.Controls.Add(lblSearch);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Name = "pnlTop";
        pnlTop.Size = new Size(1000, 52);
        pnlTop.TabIndex = 0;
        //
        // lblSearch
        //
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(0, 12);
        lblSearch.Name = "lblSearch";
        lblSearch.Text = "Search";
        //
        // txtSearch
        //
        txtSearch.Location = new Point(64, 8);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Ticket code, station or train...";
        txtSearch.Size = new Size(300, 30);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += Filter_Changed;
        //
        // lblWhen
        //
        lblWhen.AutoSize = true;
        lblWhen.Location = new Point(384, 12);
        lblWhen.Name = "lblWhen";
        lblWhen.Text = "Show";
        //
        // cboWhen
        //
        cboWhen.DropDownStyle = ComboBoxStyle.DropDownList;
        cboWhen.Items.AddRange(new object[] { "All tickets", "Upcoming", "Past" });
        cboWhen.Location = new Point(434, 8);
        cboWhen.Name = "cboWhen";
        cboWhen.Size = new Size(160, 31);
        cboWhen.TabIndex = 1;
        cboWhen.SelectedIndexChanged += Filter_Changed;
        //
        // pnlBottom
        //
        pnlBottom.Controls.Add(lblTotal);
        pnlBottom.Controls.Add(btnPrint);
        pnlBottom.Dock = DockStyle.Bottom;
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Size = new Size(1000, 60);
        pnlBottom.TabIndex = 2;
        //
        // lblTotal
        //
        lblTotal.AutoSize = true;
        lblTotal.ForeColor = Color.FromArgb(107, 114, 128);
        lblTotal.Location = new Point(0, 20);
        lblTotal.Name = "lblTotal";
        lblTotal.Text = "0 tickets";
        //
        // btnPrint
        //
        btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPrint.BackColor = Color.FromArgb(30, 58, 138);
        btnPrint.FlatAppearance.BorderSize = 0;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnPrint.ForeColor = Color.White;
        btnPrint.Location = new Point(820, 12);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(180, 42);
        btnPrint.TabIndex = 3;
        btnPrint.Text = "Print ticket";
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        //
        // dgv
        //
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 1;
        dgv.CellDoubleClick += dgv_CellDoubleClick;
        //
        // MyTicketsPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(dgv);
        Controls.Add(pnlBottom);
        Controls.Add(pnlTop);
        Font = new Font("Segoe UI", 10F);
        Name = "MyTicketsPage";
        Size = new Size(1000, 640);
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlBottom.ResumeLayout(false);
        pnlBottom.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblWhen;
    private ComboBox cboWhen;
    private Panel pnlBottom;
    private Label lblTotal;
    private Button btnPrint;
    private DataGridView dgv;
}
