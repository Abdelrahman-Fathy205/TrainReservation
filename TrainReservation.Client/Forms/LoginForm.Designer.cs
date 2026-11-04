namespace TrainReservation.Client.Forms;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlBrand = new Panel();
        lblBrandSub = new Label();
        lblBrandTitle = new Label();
        lblWelcome = new Label();
        lblHint = new Label();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        chkShowPassword = new CheckBox();
        lblError = new Label();
        btnLogin = new Button();
        lnkRegister = new LinkLabel();
        pnlBrand.SuspendLayout();
        SuspendLayout();
        //
        // pnlBrand
        //
        pnlBrand.BackColor = Color.FromArgb(30, 58, 138);
        pnlBrand.Controls.Add(lblBrandSub);
        pnlBrand.Controls.Add(lblBrandTitle);
        pnlBrand.Dock = DockStyle.Left;
        pnlBrand.Location = new Point(0, 0);
        pnlBrand.Name = "pnlBrand";
        pnlBrand.Size = new Size(380, 500);
        pnlBrand.TabIndex = 0;
        pnlBrand.Paint += pnlBrand_Paint;
        //
        // lblBrandTitle
        //
        lblBrandTitle.AutoSize = true;
        lblBrandTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblBrandTitle.ForeColor = Color.White;
        lblBrandTitle.Location = new Point(32, 280);
        lblBrandTitle.Name = "lblBrandTitle";
        lblBrandTitle.Text = "Train Ticket\r\nReservation";
        //
        // lblBrandSub
        //
        lblBrandSub.AutoSize = true;
        lblBrandSub.Font = new Font("Segoe UI", 10.5F);
        lblBrandSub.ForeColor = Color.FromArgb(191, 219, 254);
        lblBrandSub.Location = new Point(36, 380);
        lblBrandSub.Name = "lblBrandSub";
        lblBrandSub.Text = "Pick a route, choose your seat, travel.";
        //
        // lblWelcome
        //
        lblWelcome.AutoSize = true;
        lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblWelcome.ForeColor = Color.FromArgb(17, 24, 39);
        lblWelcome.Location = new Point(425, 60);
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Text = "Welcome back";
        //
        // lblHint
        //
        lblHint.AutoSize = true;
        lblHint.ForeColor = Color.FromArgb(107, 114, 128);
        lblHint.Location = new Point(429, 108);
        lblHint.Name = "lblHint";
        lblHint.Text = "Sign in to your account to continue";
        //
        // lblUsername
        //
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(429, 160);
        lblUsername.Name = "lblUsername";
        lblUsername.Text = "Username";
        //
        // txtUsername
        //
        txtUsername.Location = new Point(429, 184);
        txtUsername.MaxLength = 20;
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(350, 30);
        txtUsername.TabIndex = 1;
        //
        // lblPassword
        //
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(429, 228);
        lblPassword.Name = "lblPassword";
        lblPassword.Text = "Password";
        //
        // txtPassword
        //
        txtPassword.Location = new Point(429, 252);
        txtPassword.MaxLength = 50;
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(350, 30);
        txtPassword.TabIndex = 2;
        txtPassword.UseSystemPasswordChar = true;
        //
        // chkShowPassword
        //
        chkShowPassword.AutoSize = true;
        chkShowPassword.ForeColor = Color.FromArgb(107, 114, 128);
        chkShowPassword.Location = new Point(429, 290);
        chkShowPassword.Name = "chkShowPassword";
        chkShowPassword.TabIndex = 3;
        chkShowPassword.Text = "Show password";
        chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
        //
        // lblError
        //
        lblError.ForeColor = Color.FromArgb(220, 38, 38);
        lblError.Location = new Point(429, 322);
        lblError.Name = "lblError";
        lblError.Size = new Size(350, 24);
        //
        // btnLogin
        //
        btnLogin.BackColor = Color.FromArgb(30, 58, 138);
        btnLogin.Cursor = Cursors.Hand;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnLogin.ForeColor = Color.White;
        btnLogin.Location = new Point(429, 352);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(350, 44);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "Sign in";
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += btnLogin_Click;
        //
        // lnkRegister
        //
        lnkRegister.AutoSize = true;
        lnkRegister.LinkColor = Color.FromArgb(30, 58, 138);
        lnkRegister.Location = new Point(429, 414);
        lnkRegister.Name = "lnkRegister";
        lnkRegister.TabIndex = 5;
        lnkRegister.TabStop = true;
        lnkRegister.Text = "Don't have an account? Register";
        lnkRegister.LinkClicked += lnkRegister_LinkClicked;
        //
        // LoginForm
        //
        AcceptButton = btnLogin;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(830, 500);
        Controls.Add(lnkRegister);
        Controls.Add(btnLogin);
        Controls.Add(lblError);
        Controls.Add(chkShowPassword);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtUsername);
        Controls.Add(lblUsername);
        Controls.Add(lblHint);
        Controls.Add(lblWelcome);
        Controls.Add(pnlBrand);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Train Ticket Reservation - Sign in";
        pnlBrand.ResumeLayout(false);
        pnlBrand.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlBrand;
    private Label lblBrandTitle;
    private Label lblBrandSub;
    private Label lblWelcome;
    private Label lblHint;
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblPassword;
    private TextBox txtPassword;
    private CheckBox chkShowPassword;
    private Label lblError;
    private Button btnLogin;
    private LinkLabel lnkRegister;
}
