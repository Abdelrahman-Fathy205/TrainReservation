using System.Text.RegularExpressions;
using TrainReservation.Client.Models;
using TrainReservation.Client.Services;

namespace TrainReservation.Client.Forms;

public partial class RegisterForm : Form
{
    public string CreatedUsername { get; private set; }

    public RegisterForm()
    {
        InitializeComponent();
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        errorProvider.Clear();
        string name = txtFullName.Text.Trim(), user = txtUsername.Text.Trim();
        string email = txtEmail.Text.Trim(), phone = txtPhone.Text.Trim();
        bool ok = true;

        void Check(bool valid, Control c, string message)
        {
            if (valid) return;
            errorProvider.SetError(c, message);
            if (ok) c.Focus();
            ok = false;
        }

        Check(name.Length >= 3, txtFullName, "Enter your full name (at least 3 characters).");
        Check(Regex.IsMatch(user, @"^\w{3,20}$"), txtUsername, "3-20 letters, digits or underscore.");
        Check(!SampleData.Users.Any(u => u.Username.Equals(user, StringComparison.OrdinalIgnoreCase)), txtUsername, "This username is already taken.");
        Check(Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"), txtEmail, "Enter a valid email address.");
        Check(Regex.IsMatch(phone, @"^01[0125]\d{8}$"), txtPhone, "Egyptian mobile: 11 digits starting with 010/011/012/015.");
        Check(txtPassword.Text.Length >= 6, txtPassword, "Password must be at least 6 characters.");
        Check(txtConfirm.Text == txtPassword.Text, txtConfirm, "Passwords do not match.");
        if (!ok) return;

        var newUser = new User
        {
            Id = SampleData.NextId(SampleData.Users, u => u.Id),
            FullName = name, Username = user, Email = email, Phone = phone,
            Password = txtPassword.Text, Role = UserRole.Passenger
        };
        Db.Save(newUser);
        SampleData.Users.Add(newUser);
        CreatedUsername = user;
        MessageBox.Show("Account created. You can sign in now.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
    }
}
