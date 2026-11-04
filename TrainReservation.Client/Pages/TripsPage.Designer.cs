namespace TrainReservation.Client.Pages;

partial class TripsPage
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
        lblCount = new Label();
        cboFilterTrain = new ComboBox();
        lblFilterTrain = new Label();
        dtpFilter = new DateTimePicker();
        lblFilterDate = new Label();
        pnlEditor = new Panel();
        btnClear = new Button();
        btnDelete = new Button();
        btnUpdate = new Button();
        btnAdd = new Button();
        btnRemoveStop = new Button();
        dgvStops = new DataGridView();
        colStation = new DataGridViewComboBoxColumn();
        colKm = new DataGridViewTextBoxColumn();
        colMinutes = new DataGridViewTextBoxColumn();
        lblStops = new Label();
        dtpDeparture = new DateTimePicker();
        lblDeparture = new Label();
        cboTrain = new ComboBox();
        lblTrain = new Label();
        lblEditor = new Label();
        dgv = new DataGridView();
        pnlTop.SuspendLayout();
        pnlEditor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStops).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
        //
        // pnlTop
        //
        pnlTop.Controls.Add(lblCount);
        pnlTop.Controls.Add(cboFilterTrain);
        pnlTop.Controls.Add(lblFilterTrain);
        pnlTop.Controls.Add(dtpFilter);
        pnlTop.Controls.Add(lblFilterDate);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Name = "pnlTop";
        pnlTop.Size = new Size(1000, 52);
        pnlTop.TabIndex = 0;
        //
        // lblFilterDate
        //
        lblFilterDate.AutoSize = true;
        lblFilterDate.Location = new Point(0, 12);
        lblFilterDate.Name = "lblFilterDate";
        lblFilterDate.Text = "Date";
        //
        // dtpFilter
        //
        dtpFilter.Format = DateTimePickerFormat.Short;
        dtpFilter.Location = new Point(48, 8);
        dtpFilter.Name = "dtpFilter";
        dtpFilter.ShowCheckBox = true;
        dtpFilter.Size = new Size(170, 30);
        dtpFilter.TabIndex = 0;
        dtpFilter.ValueChanged += Filter_Changed;
        //
        // lblFilterTrain
        //
        lblFilterTrain.AutoSize = true;
        lblFilterTrain.Location = new Point(236, 12);
        lblFilterTrain.Name = "lblFilterTrain";
        lblFilterTrain.Text = "Train";
        //
        // cboFilterTrain
        //
        cboFilterTrain.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFilterTrain.Location = new Point(284, 8);
        cboFilterTrain.Name = "cboFilterTrain";
        cboFilterTrain.Size = new Size(240, 31);
        cboFilterTrain.TabIndex = 1;
        cboFilterTrain.SelectedIndexChanged += Filter_Changed;
        //
        // lblCount
        //
        lblCount.AutoSize = true;
        lblCount.ForeColor = Color.FromArgb(107, 114, 128);
        lblCount.Location = new Point(540, 12);
        lblCount.Name = "lblCount";
        lblCount.Text = "0 trips";
        //
        // pnlEditor
        //
        pnlEditor.BackColor = Color.White;
        pnlEditor.Controls.Add(btnClear);
        pnlEditor.Controls.Add(btnDelete);
        pnlEditor.Controls.Add(btnUpdate);
        pnlEditor.Controls.Add(btnAdd);
        pnlEditor.Controls.Add(btnRemoveStop);
        pnlEditor.Controls.Add(dgvStops);
        pnlEditor.Controls.Add(lblStops);
        pnlEditor.Controls.Add(dtpDeparture);
        pnlEditor.Controls.Add(lblDeparture);
        pnlEditor.Controls.Add(cboTrain);
        pnlEditor.Controls.Add(lblTrain);
        pnlEditor.Controls.Add(lblEditor);
        pnlEditor.Dock = DockStyle.Right;
        pnlEditor.Name = "pnlEditor";
        pnlEditor.Size = new Size(400, 588);
        pnlEditor.TabIndex = 2;
        //
        // lblEditor
        //
        lblEditor.AutoSize = true;
        lblEditor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblEditor.Location = new Point(20, 16);
        lblEditor.Name = "lblEditor";
        lblEditor.Text = "Trip schedule";
        //
        // lblTrain
        //
        lblTrain.AutoSize = true;
        lblTrain.Location = new Point(20, 56);
        lblTrain.Name = "lblTrain";
        lblTrain.Text = "Train";
        //
        // cboTrain
        //
        cboTrain.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrain.Location = new Point(20, 80);
        cboTrain.Name = "cboTrain";
        cboTrain.Size = new Size(360, 31);
        cboTrain.TabIndex = 1;
        //
        // lblDeparture
        //
        lblDeparture.AutoSize = true;
        lblDeparture.Location = new Point(20, 120);
        lblDeparture.Name = "lblDeparture";
        lblDeparture.Text = "Departure from first station";
        //
        // dtpDeparture
        //
        dtpDeparture.CustomFormat = "dddd dd/MM/yyyy  HH:mm";
        dtpDeparture.Format = DateTimePickerFormat.Custom;
        dtpDeparture.Location = new Point(20, 144);
        dtpDeparture.Name = "dtpDeparture";
        dtpDeparture.Size = new Size(360, 30);
        dtpDeparture.TabIndex = 2;
        //
        // lblStops
        //
        lblStops.AutoSize = true;
        lblStops.Location = new Point(20, 186);
        lblStops.Name = "lblStops";
        lblStops.Text = "Stops in order (km and minutes from first station)";
        //
        // dgvStops
        //
        dgvStops.Columns.AddRange(new DataGridViewColumn[] { colStation, colKm, colMinutes });
        dgvStops.Location = new Point(20, 210);
        dgvStops.Name = "dgvStops";
        dgvStops.Size = new Size(360, 230);
        dgvStops.TabIndex = 3;
        dgvStops.DataError += dgvStops_DataError;
        //
        // colStation
        //
        colStation.FillWeight = 60F;
        colStation.HeaderText = "Station";
        colStation.Name = "colStation";
        //
        // colKm
        //
        colKm.FillWeight = 20F;
        colKm.HeaderText = "Km";
        colKm.Name = "colKm";
        //
        // colMinutes
        //
        colMinutes.FillWeight = 20F;
        colMinutes.HeaderText = "Min";
        colMinutes.Name = "colMinutes";
        //
        // btnRemoveStop
        //
        btnRemoveStop.FlatStyle = FlatStyle.Flat;
        btnRemoveStop.Location = new Point(20, 446);
        btnRemoveStop.Name = "btnRemoveStop";
        btnRemoveStop.Size = new Size(160, 32);
        btnRemoveStop.TabIndex = 4;
        btnRemoveStop.Text = "Remove stop";
        btnRemoveStop.Click += btnRemoveStop_Click;
        //
        // btnAdd
        //
        btnAdd.BackColor = Color.FromArgb(30, 58, 138);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(20, 490);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(85, 38);
        btnAdd.TabIndex = 5;
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
        btnUpdate.Location = new Point(111, 490);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new Size(85, 38);
        btnUpdate.TabIndex = 6;
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
        btnDelete.Location = new Point(202, 490);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(85, 38);
        btnDelete.TabIndex = 7;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        //
        // btnClear
        //
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.Location = new Point(293, 490);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(87, 38);
        btnClear.TabIndex = 8;
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
        // TripsPage
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 244, 246);
        Controls.Add(dgv);
        Controls.Add(pnlEditor);
        Controls.Add(pnlTop);
        Font = new Font("Segoe UI", 10F);
        Name = "TripsPage";
        Size = new Size(1000, 640);
        pnlTop.ResumeLayout(false);
        pnlTop.PerformLayout();
        pnlEditor.ResumeLayout(false);
        pnlEditor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStops).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlTop;
    private Label lblFilterDate;
    private DateTimePicker dtpFilter;
    private Label lblFilterTrain;
    private ComboBox cboFilterTrain;
    private Label lblCount;
    private Panel pnlEditor;
    private Label lblEditor;
    private Label lblTrain;
    private ComboBox cboTrain;
    private Label lblDeparture;
    private DateTimePicker dtpDeparture;
    private Label lblStops;
    private DataGridView dgvStops;
    private DataGridViewComboBoxColumn colStation;
    private DataGridViewTextBoxColumn colKm;
    private DataGridViewTextBoxColumn colMinutes;
    private Button btnRemoveStop;
    private Button btnAdd;
    private Button btnUpdate;
    private Button btnDelete;
    private Button btnClear;
    private DataGridView dgv;
}
