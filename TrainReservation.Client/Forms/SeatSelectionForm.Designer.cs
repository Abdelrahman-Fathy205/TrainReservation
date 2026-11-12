namespace TrainReservation.Client.Forms;

partial class SeatSelectionForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlInfo = new Panel();
        lblSegment = new Label();
        lblTrip = new Label();
        pnlCoach = new Panel();
        lblFree = new Label();
        cboCoach = new ComboBox();
        lblCoach = new Label();
        pnlBottom = new Panel();
        btnCancel = new Button();
        btnConfirm = new Button();
        lblPrice = new Label();
        lblSelected = new Label();
        seatMap = new TrainReservation.Client.Controls.SeatMapControl();
        pnlInfo.SuspendLayout();
        pnlCoach.SuspendLayout();
        pnlBottom.SuspendLayout();
        SuspendLayout();
        //
        // pnlInfo
        //
        pnlInfo.BackColor = Color.FromArgb(30, 58, 138);
        pnlInfo.Controls.Add(lblSegment);
        pnlInfo.Controls.Add(lblTrip);
        pnlInfo.Dock = DockStyle.Top;
        pnlInfo.Name = "pnlInfo";
        pnlInfo.Size = new Size(860, 84);
        //
        // lblTrip
        //
        lblTrip.AutoSize = true;
        lblTrip.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTrip.ForeColor = Color.White;
        lblTrip.Location = new Point(20, 12);
        lblTrip.Name = "lblTrip";
        lblTrip.Text = "Train";
        //
        // lblSegment
        //
        lblSegment.AutoSize = true;
        lblSegment.ForeColor = Color.FromArgb(191, 219, 254);
        lblSegment.Location = new Point(22, 48);
        lblSegment.Name = "lblSegment";
        lblSegment.Text = "From → To";
        //
        // pnlCoach
        //
        pnlCoach.Controls.Add(lblFree);
        pnlCoach.Controls.Add(cboCoach);
        pnlCoach.Controls.Add(lblCoach);
        pnlCoach.Dock = DockStyle.Top;
        pnlCoach.Name = "pnlCoach";
        pnlCoach.Size = new Size(860, 56);
        //
        // lblCoach
        //
        lblCoach.AutoSize = true;
        lblCoach.Location = new Point(20, 18);
        lblCoach.Name = "lblCoach";
        lblCoach.Text = "Coach";
        //
        // cboCoach
        //
        cboCoach.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCoach.Location = new Point(80, 14);
        cboCoach.Name = "cboCoach";
        cboCoach.Size = new Size(160, 31);
        cboCoach.TabIndex = 0;
        cboCoach.SelectedIndexChanged += cboCoach_SelectedIndexChanged;
        //
        // lblFree
        //
        lblFree.AutoSize = true;
        lblFree.ForeColor = Color.FromArgb(107, 114, 128);
        lblFree.Location = new Point(256, 18);
        lblFree.Name = "lblFree";
        lblFree.Text = "0 free seats";
        //
        // seatMap
        //
        seatMap.Dock = DockStyle.Fill;
        seatMap.Name = "seatMap";
        seatMap.TabIndex = 1;
        seatMap.SelectedSeatChanged += seatMap_SelectedSeatChanged;
        //
        // pnlBottom
        //
        pnlBottom.BackColor = Color.FromArgb(243, 244, 246);
        pnlBottom.Controls.Add(btnCancel);
        pnlBottom.Controls.Add(btnConfirm);
        pnlBottom.Controls.Add(lblPrice);
        pnlBottom.Controls.Add(lblSelected);
        pnlBottom.Dock = DockStyle.Bottom;
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Size = new Size(860, 76);
        //
        // lblSelected
        //
        lblSelected.AutoSize = true;
        lblSelected.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblSelected.Location = new Point(20, 14);
        lblSelected.Name = "lblSelected";
        lblSelected.Text = "No seat selected";
        //
        // lblPrice
        //
        lblPrice.AutoSize = true;
        lblPrice.ForeColor = Color.FromArgb(107, 114, 128);
        lblPrice.Location = new Point(20, 42);
        lblPrice.Name = "lblPrice";
        lblPrice.Text = "Fare";
        //
        // btnConfirm
        //
        btnConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnConfirm.BackColor = Color.FromArgb(245, 158, 11);
        btnConfirm.Enabled = false;
        btnConfirm.FlatAppearance.BorderSize = 0;
        btnConfirm.FlatStyle = FlatStyle.Flat;
        btnConfirm.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        btnConfirm.ForeColor = Color.White;
        btnConfirm.Location = new Point(560, 16);
        btnConfirm.Name = "btnConfirm";
        btnConfirm.Size = new Size(170, 44);
        btnConfirm.TabIndex = 2;
        btnConfirm.Text = "Confirm booking";
        btnConfirm.UseVisualStyleBackColor = false;
        btnConfirm.Click += btnConfirm_Click;
        //
        // btnCancel
        //
        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(740, 16);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 44);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "Cancel";
        //
        // SeatSelectionForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        CancelButton = btnCancel;
        ClientSize = new Size(860, 580);
        Controls.Add(seatMap);
        Controls.Add(pnlBottom);
        Controls.Add(pnlCoach);
        Controls.Add(pnlInfo);
        Font = new Font("Segoe UI", 10F);
        MinimizeBox = false;
        MinimumSize = new Size(700, 480);
        Name = "SeatSelectionForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Choose your seats";
        pnlInfo.ResumeLayout(false);
        pnlInfo.PerformLayout();
        pnlCoach.ResumeLayout(false);
        pnlCoach.PerformLayout();
        pnlBottom.ResumeLayout(false);
        pnlBottom.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlInfo;
    private Label lblTrip;
    private Label lblSegment;
    private Panel pnlCoach;
    private Label lblCoach;
    private ComboBox cboCoach;
    private Label lblFree;
    private TrainReservation.Client.Controls.SeatMapControl seatMap;
    private Panel pnlBottom;
    private Label lblSelected;
    private Label lblPrice;
    private Button btnConfirm;
    private Button btnCancel;
}
