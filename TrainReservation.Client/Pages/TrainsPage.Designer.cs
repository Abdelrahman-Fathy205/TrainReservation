namespace TrainReservation.Client.Pages;

partial class TrainsPage
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
        lblCapacity = new Label();
        btnClear = new Button();
        btnDelete = new Button();
        btnUpdate = new Button();
        btnAdd = new Button();
        numSeats = new NumericUpDown();
        lblSeats = new Label();
        numEconomy = new NumericUpDown();
        lblEconomy = new Label();
        numSecond = new NumericUpDown();
        lblSecond = new Label();
        numFirst = new NumericUpDown();
        lblFirst = new Label();
        txtName = new TextBox();
        lblName = new Label();
        txtNumber = new TextBox();
        lblNumber = new Label();
        lblEditor = new Label();
        dgv = new DataGridView();
        errorProvider = new ErrorProvider(components);
        pnlTop.SuspendLayout();
        pnlEditor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numSeats).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numEconomy).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSecond).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numFirst).BeginInit();
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
        txtSearch.PlaceholderText = "Train number or name...";
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
        lblCount.Text = "0 trains";
        //
        // pnlEditor
        //
        pnlEditor.BackColor = Color.White;
        pnlEditor.Controls.Add(lblCapacity);
        pnlEditor.Controls.Add(btnClear);
        pnlEditor.Controls.Add(btnDelete);
        pnlEditor.Controls.Add(btnUpdate);
        pnlEditor.Controls.Add(btnAdd);
        pnlEditor.Controls.Add(numSeats);
        pnlEditor.Controls.Add(lblSeats);
        pnlEditor.Controls.Add(numEconomy);
        pnlEditor.Controls.Add(lblEconomy);
        pnlEditor.Controls.Add(numSecond);
        pnlEditor.Controls.Add(lblSecond);
        pnlEditor.Controls.Add(numFirst);
        pnlEditor.Controls.Add(lblFirst);
        pnlEditor.Controls.Add(txtName);
        pnlEditor.Controls.Add(lblName);
        pnlEditor.Controls.Add(txtNumber);
        pnlEditor.Controls.Add(lblNumber);
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
        lblEditor.Text = "Train details";
        //
        // lblNumber
        //
        lblNumber.AutoSize = true;
        lblNumber.Location = new Point(20, 60);
        lblNumber.Name = "lblNumber";
        lblNumber.Text = "Train number";
        //
        // txtNumber
        //
        txtNumber.Location = new Point(20, 84);
        txtNumber.MaxLength = 5;
        txtNumber.Name = "txtNumber";
        txtNumber.Size = new Size(260, 30);
        txtNumber.TabIndex = 1;
        //
        // lblName
        //
        lblName.AutoSize = true;
        lblName.Location = new Point(20, 124);
        lblName.Name = "lblName";
        lblName.Text = "Train name";
        //
        // txtName
        //
        txtName.Location = new Point(20, 148);
        txtName.MaxLength = 50;
        txtName.Name = "txtName";
        txtName.Size = new Size(260, 30);
        txtName.TabIndex = 2;
        //
        // lblFirst
        //
        lblFirst.AutoSize = true;
        lblFirst.Location = new Point(20, 196);
        lblFirst.Name = "lblFirst";
        lblFirst.Text = "First class coaches";
        //
        // numFirst
        //
        numFirst.Location = new Point(200, 192);
        numFirst.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        numFirst.Name = "numFirst";
        numFirst.Size = new Size(80, 30);
        numFirst.TabIndex = 3;
        numFirst.ValueChanged += Capacity_Changed;
        //
        // lblSecond
        //
        lblSecond.AutoSize = true;
        lblSecond.Location = new Point(20, 236);
        lblSecond.Name = "lblSecond";
        lblSecond.Text = "Second class coaches";
        //
        // numSecond
        //
        numSecond.Location = new Point(200, 232);
        numSecond.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        numSecond.Name = "numSecond";
        numSecond.Size = new Size(80, 30);
        numSecond.TabIndex = 4;
        numSecond.ValueChanged += Capacity_Changed;
        //
        // lblEconomy
        //
        lblEconomy.AutoSize = true;
        lblEconomy.Location = new Point(20, 276);
        lblEconomy.Name = "lblEconomy";
        lblEconomy.Text = "Economy coaches";
        //
        // numEconomy
        //
        numEconomy.Location = new Point(200, 272);
        numEconomy.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        numEconomy.Name = "numEconomy";
        numEconomy.Size = new Size(80, 30);
        numEconomy.TabIndex = 5;
        numEconomy.ValueChanged += Capacity_Changed;
        //
        // lblSeats
        //
        lblSeats.AutoSize = true;
        lblSeats.Location = new Point(20, 316);
        lblSeats.Name = "lblSeats";
        lblSeats.Text = "Seats per coach";
        //
        // numSeats
        //
        numSeats.Increment = new decimal(new int[] { 4, 0, 0, 0 });
        numSeats.Location = new Point(200, 312);
        numSeats.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
        numSeats.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
        numSeats.Name = "numSeats";
        numSeats.Size = new Size(80, 30);
        numSeats.TabIndex = 6;
        numSeats.Value = new decimal(new int[] { 40, 0, 0, 0 });
        numSeats.ValueChanged += Capacity_Changed;
        //
        // lblCapacity
        //
        lblCapacity.AutoSize = true;
        lblCapacity.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCapacity.ForeColor = Color.FromArgb(30, 58, 138);
        lblCapacity.Location = new Point(20, 356);
        lblCapacity.Name = "lblCapacity";
        lblCapacity.Text = "Total capacity: 0 seats";
        //
        // btnAdd
        //
        btnAdd.BackColor = Color.FromArgb(30, 58, 138);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(20, 396);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(125, 38);
        btnAdd.TabIndex = 7;
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
        btnUpdate.Location = new Point(155, 396);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(125, 38);
        btnUpdate.TabIndex = 8;
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
        btnDelete.Location = new Point(20, 444);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(125, 38);
        btnDelete.TabIndex = 9;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        //
        // btnClear
        //
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.Location = new Point(155, 444);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(125, 38);
        btnClear.TabIndex = 10;
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
        // TrainsPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(dgv);
        Controls.Add(pnlEditor);
        Controls.Add(pnlTop);
        Font = new Font("Segoe UI", 10F);
        Name = "TrainsPage";
        Size = new Size(1000, 640);
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlEditor.ResumeLayout(false);
        pnlEditor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numSeats).EndInit();
        ((System.ComponentModel.ISupportInitialize)numEconomy).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSecond).EndInit();
        ((System.ComponentModel.ISupportInitialize)numFirst).EndInit();
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
    private Label lblNumber;
    private TextBox txtNumber;
    private Label lblName;
    private TextBox txtName;
    private Label lblFirst;
    private NumericUpDown numFirst;
    private Label lblSecond;
    private NumericUpDown numSecond;
    private Label lblEconomy;
    private NumericUpDown numEconomy;
    private Label lblSeats;
    private NumericUpDown numSeats;
    private Label lblCapacity;
    private Button btnAdd;
    private Button btnUpdate;
    private Button btnDelete;
    private Button btnClear;
    private DataGridView dgv;
    private ErrorProvider errorProvider;
}
