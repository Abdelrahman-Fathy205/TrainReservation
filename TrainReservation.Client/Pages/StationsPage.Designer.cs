namespace TrainReservation.Client.Pages;

partial class StationsPage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlTop = new Panel();
        lblCount = new Label();
        txtSearch = new TextBox();
        lblSearch = new Label();
        pnlEditor = new Panel();
        btnClear = new Button();
        btnDelete = new Button();
        btnUpdate = new Button();
        btnAdd = new Button();
        txtCity = new TextBox();
        lblCity = new Label();
        txtName = new TextBox();
        lblName = new Label();
        txtCode = new TextBox();
        lblCode = new Label();
        lblEditor = new Label();
        dgv = new DataGridView();
        errorProvider = new ErrorProvider(components);
        pnlTop.SuspendLayout();
        pnlEditor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        //
        // pnlTop
        //
        pnlTop.Controls.Add(lblCount);
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
        txtSearch.PlaceholderText = "Code, name or city...";
        txtSearch.Size = new Size(300, 30);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += txtSearch_TextChanged;
        //
        // lblCount
        //
        lblCount.AutoSize = true;
        lblCount.ForeColor = Color.FromArgb(107, 114, 128);
        lblCount.Location = new Point(380, 12);
        lblCount.Name = "lblCount";
        lblCount.Text = "0 stations";
        //
        // pnlEditor
        //
        pnlEditor.BackColor = Color.White;
        pnlEditor.Controls.Add(btnClear);
        pnlEditor.Controls.Add(btnDelete);
        pnlEditor.Controls.Add(btnUpdate);
        pnlEditor.Controls.Add(btnAdd);
        pnlEditor.Controls.Add(txtCity);
        pnlEditor.Controls.Add(lblCity);
        pnlEditor.Controls.Add(txtName);
        pnlEditor.Controls.Add(lblName);
        pnlEditor.Controls.Add(txtCode);
        pnlEditor.Controls.Add(lblCode);
        pnlEditor.Controls.Add(lblEditor);
        pnlEditor.Dock = DockStyle.Right;
        pnlEditor.Name = "pnlEditor";
        pnlEditor.Size = new Size(320, 588);
        pnlEditor.TabIndex = 2;
        //
        // lblEditor
        //
        lblEditor.AutoSize = true;
        lblEditor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblEditor.Location = new Point(20, 16);
        lblEditor.Name = "lblEditor";
        lblEditor.Text = "Station details";
        //
        // lblCode
        //
        lblCode.AutoSize = true;
        lblCode.Location = new Point(20, 60);
        lblCode.Name = "lblCode";
        lblCode.Text = "Code (3 letters)";
        //
        // txtCode
        //
        txtCode.CharacterCasing = CharacterCasing.Upper;
        txtCode.Location = new Point(20, 84);
        txtCode.MaxLength = 3;
        txtCode.Name = "txtCode";
        txtCode.Size = new Size(260, 30);
        txtCode.TabIndex = 1;
        //
        // lblName
        //
        lblName.AutoSize = true;
        lblName.Location = new Point(20, 124);
        lblName.Name = "lblName";
        lblName.Text = "Station name";
        //
        // txtName
        //
        txtName.Location = new Point(20, 148);
        txtName.MaxLength = 50;
        txtName.Name = "txtName";
        txtName.Size = new Size(260, 30);
        txtName.TabIndex = 2;
        //
        // lblCity
        //
        lblCity.AutoSize = true;
        lblCity.Location = new Point(20, 188);
        lblCity.Name = "lblCity";
        lblCity.Text = "City / Governorate";
        //
        // txtCity
        //
        txtCity.Location = new Point(20, 212);
        txtCity.MaxLength = 50;
        txtCity.Name = "txtCity";
        txtCity.Size = new Size(260, 30);
        txtCity.TabIndex = 3;
        //
        // btnAdd
        //
        btnAdd.BackColor = Color.FromArgb(30, 58, 138);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(20, 268);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(125, 38);
        btnAdd.TabIndex = 4;
        btnAdd.Text = "Add";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
        //
        // btnUpdate
        //
        btnUpdate.BackColor = Color.FromArgb(22, 163, 74);
        btnUpdate.FlatAppearance.BorderSize = 0;
        btnUpdate.FlatStyle = FlatStyle.Flat;
        btnUpdate.ForeColor = Color.White;
        btnUpdate.Location = new Point(155, 268);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(125, 38);
        btnUpdate.TabIndex = 5;
        btnUpdate.Text = "Update";
        btnUpdate.UseVisualStyleBackColor = false;
        btnUpdate.Click += btnUpdate_Click;
        //
        // btnDelete
        //
        btnDelete.BackColor = Color.FromArgb(220, 38, 38);
        btnDelete.FlatAppearance.BorderSize = 0;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.ForeColor = Color.White;
        btnDelete.Location = new Point(20, 316);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(125, 38);
        btnDelete.TabIndex = 6;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        //
        // btnClear
        //
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.Location = new Point(155, 316);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(125, 38);
        btnClear.TabIndex = 7;
        btnClear.Text = "Clear";
        btnClear.Click += btnClear_Click;
        //
        // dgv
        //
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 1;
        dgv.CellClick += dgv_CellClick;
        //
        // errorProvider
        //
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        //
        // StationsPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(dgv);
        Controls.Add(pnlEditor);
        Controls.Add(pnlTop);
        Font = new Font("Segoe UI", 10F);
        Name = "StationsPage";
        Size = new Size(1000, 640);
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlEditor.ResumeLayout(false);
        pnlEditor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblCount;
    private Panel pnlEditor;
    private Label lblEditor;
    private Label lblCode;
    private TextBox txtCode;
    private Label lblName;
    private TextBox txtName;
    private Label lblCity;
    private TextBox txtCity;
    private Button btnAdd;
    private Button btnUpdate;
    private Button btnDelete;
    private Button btnClear;
    private DataGridView dgv;
    private ErrorProvider errorProvider;
}
