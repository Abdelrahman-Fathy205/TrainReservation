namespace TrainReservation.Client.Forms;

partial class RegisterForm
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
        lblTitle = new Label();
        lblFullName = new Label();
        txtFullName = new TextBox();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        lblConfirm = new Label();
        txtConfirm = new TextBox();
        btnRegister = new Button();
        btnCancel = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 58, 138);
        lblTitle.Location = new Point(30, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "Create passenger account";
        //
        // lblFullName
        //
        lblFullName.AutoSize = true;
        lblFullName.Location = new Point(32, 80);
        lblFullName.Name = "lblFullName";
        lblFullName.Text = "Full name";
        //
        // txtFullName
        //
        txtFullName.Location = new Point(32, 104);
        txtFullName.MaxLength = 60;
        txtFullName.Name = "txtFullName";
        txtFullName.Size = new Size(380, 30);
        txtFullName.TabIndex = 0;
        //
        // lblUsername
        //
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(32, 144);
        lblUsername.Name = "lblUsername";
        lblUsername.Text = "Username (3-20 letters, digits or _)";
        //
        // txtUsername
        //
        txtUsername.Location = new Point(32, 168);
        txtUsername.MaxLength = 20;
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(380, 30);
        txtUsername.TabIndex = 1;
        //
        // lblEmail
        //
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(32, 208);
        lblEmail.Name = "lblEmail";
        lblEmail.Text = "Email";
        //
        // txtEmail
        //
        txtEmail.Location = new Point(32, 232);
        txtEmail.MaxLength = 80;
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(380, 30);
        txtEmail.TabIndex = 2;
        //
        // lblPhone
        //
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(32, 272);
        lblPhone.Name = "lblPhone";
        lblPhone.Text = "Mobile (e.g. 01012345678)";
        //
        // txtPhone
        //
        txtPhone.Location = new Point(32, 296);
        txtPhone.MaxLength = 11;
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(380, 30);
        txtPhone.TabIndex = 3;
        //
        // lblPassword
        //
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(32, 336);
        lblPassword.Name = "lblPassword";
        lblPassword.Text = "Password (min 6 characters)";
        //
        // txtPassword
        //
        txtPassword.Location = new Point(32, 360);
        txtPassword.MaxLength = 50;
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(380, 30);
        txtPassword.TabIndex = 4;
        txtPassword.UseSystemPasswordChar = true;
        //
        // lblConfirm
        //
        lblConfirm.AutoSize = true;
        lblConfirm.Location = new Point(32, 400);
        lblConfirm.Name = "lblConfirm";
        lblConfirm.Text = "Confirm password";
        //
        // txtConfirm
        //
        txtConfirm.Location = new Point(32, 424);
        txtConfirm.MaxLength = 50;
        txtConfirm.Name = "txtConfirm";
        txtConfirm.Size = new Size(380, 30);
        txtConfirm.TabIndex = 5;
        txtConfirm.UseSystemPasswordChar = true;
        //
        // btnRegister
        //
        btnRegister.BackColor = Color.FromArgb(30, 58, 138);
        btnRegister.Cursor = Cursors.Hand;
        btnRegister.FlatAppearance.BorderSize = 0;
        btnRegister.FlatStyle = FlatStyle.Flat;
        btnRegister.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        btnRegister.ForeColor = Color.White;
        btnRegister.Location = new Point(32, 480);
        btnRegister.Name = "btnRegister";
        btnRegister.Size = new Size(240, 42);
        btnRegister.TabIndex = 6;
        btnRegister.Text = "Create account";
        btnRegister.UseVisualStyleBackColor = false;
        btnRegister.Click += btnRegister_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Location = new Point(282, 480);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(130, 42);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "Cancel";
        //
        // errorProvider
        //
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        //
        // RegisterForm
        //
        AcceptButton = btnRegister;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        CancelButton = btnCancel;
        ClientSize = new Size(450, 550);
        Controls.Add(btnCancel);
        Controls.Add(btnRegister);
        Controls.Add(txtConfirm);
        Controls.Add(lblConfirm);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtPhone);
        Controls.Add(lblPhone);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(txtUsername);
        Controls.Add(lblUsername);
        Controls.Add(txtFullName);
        Controls.Add(lblFullName);
        Controls.Add(lblTitle);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "RegisterForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Register";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitle;
    private Label lblFullName;
    private TextBox txtFullName;
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblPhone;
    private TextBox txtPhone;
    private Label lblPassword;
    private TextBox txtPassword;
    private Label lblConfirm;
    private TextBox txtConfirm;
    private Button btnRegister;
    private Button btnCancel;
    private ErrorProvider errorProvider;
}
