namespace TrainReservation.Client.Pages;

partial class SearchTripsPage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlSearch = new Panel();
        btnSearch = new Button();
        cboClass = new ComboBox();
        lblClass = new Label();
        dtpDate = new DateTimePicker();
        lblDate = new Label();
        btnSwap = new Button();
        cboTo = new ComboBox();
        lblTo = new Label();
        cboFrom = new ComboBox();
        lblFrom = new Label();
        pnlBottom = new Panel();
        lblResult = new Label();
        btnBook = new Button();
        dgv = new DataGridView();
        pnlSpacer = new Panel();
        pnlSearch.SuspendLayout();
        pnlBottom.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
        //
        // pnlSearch
        //
        pnlSearch.BackColor = Color.White;
        pnlSearch.Controls.Add(btnSearch);
        pnlSearch.Controls.Add(cboClass);
        pnlSearch.Controls.Add(lblClass);
        pnlSearch.Controls.Add(dtpDate);
        pnlSearch.Controls.Add(lblDate);
        pnlSearch.Controls.Add(btnSwap);
        pnlSearch.Controls.Add(cboTo);
        pnlSearch.Controls.Add(lblTo);
        pnlSearch.Controls.Add(cboFrom);
        pnlSearch.Controls.Add(lblFrom);
        pnlSearch.Dock = DockStyle.Top;
        pnlSearch.Name = "pnlSearch";
        pnlSearch.Size = new Size(1000, 96);
        pnlSearch.TabIndex = 0;
        //
        // lblFrom
        //
        lblFrom.AutoSize = true;
        lblFrom.ForeColor = Color.FromArgb(107, 114, 128);
        lblFrom.Location = new Point(20, 16);
        lblFrom.Name = "lblFrom";
        lblFrom.Text = "From";
        //
        // cboFrom
        //
        cboFrom.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFrom.Location = new Point(20, 42);
        cboFrom.Name = "cboFrom";
        cboFrom.Size = new Size(210, 31);
        cboFrom.TabIndex = 0;
        //
        // btnSwap
        //
        btnSwap.FlatStyle = FlatStyle.Flat;
        btnSwap.Location = new Point(238, 41);
        btnSwap.Name = "btnSwap";
        btnSwap.Size = new Size(42, 33);
        btnSwap.TabIndex = 1;
        btnSwap.Text = "⇄";
        btnSwap.Click += btnSwap_Click;
        //
        // lblTo
        //
        lblTo.AutoSize = true;
        lblTo.ForeColor = Color.FromArgb(107, 114, 128);
        lblTo.Location = new Point(288, 16);
        lblTo.Name = "lblTo";
        lblTo.Text = "To";
        //
        // cboTo
        //
        cboTo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTo.Location = new Point(288, 42);
        cboTo.Name = "cboTo";
        cboTo.Size = new Size(210, 31);
        cboTo.TabIndex = 2;
        //
        // lblDate
        //
        lblDate.AutoSize = true;
        lblDate.ForeColor = Color.FromArgb(107, 114, 128);
        lblDate.Location = new Point(518, 16);
        lblDate.Name = "lblDate";
        lblDate.Text = "Travel date";
        //
        // dtpDate
        //
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(518, 42);
        dtpDate.Name = "dtpDate";
        dtpDate.Size = new Size(150, 30);
        dtpDate.TabIndex = 3;
        //
        // lblClass
        //
        lblClass.AutoSize = true;
        lblClass.ForeColor = Color.FromArgb(107, 114, 128);
        lblClass.Location = new Point(686, 16);
        lblClass.Name = "lblClass";
        lblClass.Text = "Class";
        //
        // cboClass
        //
        cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
        cboClass.Location = new Point(686, 42);
        cboClass.Name = "cboClass";
        cboClass.Size = new Size(130, 31);
        cboClass.TabIndex = 4;
        //
        // btnSearch
        //
        btnSearch.BackColor = Color.FromArgb(30, 58, 138);
        btnSearch.FlatAppearance.BorderSize = 0;
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnSearch.ForeColor = Color.White;
        btnSearch.Location = new Point(836, 38);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(140, 38);
        btnSearch.TabIndex = 5;
        btnSearch.Text = "Search trains";
        btnSearch.UseVisualStyleBackColor = false;
        btnSearch.Click += btnSearch_Click;
        //
        // pnlSpacer
        //
        pnlSpacer.Dock = DockStyle.Top;
        pnlSpacer.Name = "pnlSpacer";
        pnlSpacer.Size = new Size(1000, 16);
        //
        // pnlBottom
        //
        pnlBottom.Controls.Add(lblResult);
        pnlBottom.Controls.Add(btnBook);
        pnlBottom.Dock = DockStyle.Bottom;
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Size = new Size(1000, 60);
        pnlBottom.TabIndex = 2;
        //
        // lblResult
        //
        lblResult.AutoSize = true;
        lblResult.ForeColor = Color.FromArgb(107, 114, 128);
        lblResult.Location = new Point(0, 20);
        lblResult.Name = "lblResult";
        lblResult.Text = "Choose your route and press Search.";
        //
        // btnBook
        //
        btnBook.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBook.BackColor = Color.FromArgb(245, 158, 11);
        btnBook.Enabled = false;
        btnBook.FlatAppearance.BorderSize = 0;
        btnBook.FlatStyle = FlatStyle.Flat;
        btnBook.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnBook.ForeColor = Color.White;
        btnBook.Location = new Point(800, 12);
        btnBook.Name = "btnBook";
        btnBook.Size = new Size(200, 42);
        btnBook.TabIndex = 3;
        btnBook.Text = "Choose seat && book";
        btnBook.UseVisualStyleBackColor = false;
        btnBook.Click += btnBook_Click;
        //
        // dgv
        //
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 1;
        dgv.CellDoubleClick += dgv_CellDoubleClick;
        dgv.SelectionChanged += dgv_SelectionChanged;
        //
        // SearchTripsPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(dgv);
        Controls.Add(pnlBottom);
        Controls.Add(pnlSpacer);
        Controls.Add(pnlSearch);
        Font = new Font("Segoe UI", 10F);
        Name = "SearchTripsPage";
        Size = new Size(1000, 640);
        pnlSearch.ResumeLayout(false);
        pnlSearch.PerformLayout();
        pnlBottom.ResumeLayout(false);
        pnlBottom.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlSearch;
    private Label lblFrom;
    private ComboBox cboFrom;
    private Button btnSwap;
    private Label lblTo;
    private ComboBox cboTo;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private Label lblClass;
    private ComboBox cboClass;
    private Button btnSearch;
    private Panel pnlSpacer;
    private Panel pnlBottom;
    private Label lblResult;
    private Button btnBook;
    private DataGridView dgv;
}
