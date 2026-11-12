using System.Text.RegularExpressions; // regex library to check formats
using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services

namespace TrainReservation.Client.Forms; // namespace for the screens

public partial class RegisterForm : Form // register / edit profile screen
{
    public string CreatedUsername { get; private set; } // name of the account that was created

    private readonly User editing; // null = new account, otherwise "My profile" for this user

    public RegisterForm() // create a new register screen
    {
        InitializeComponent(); // build the controls
        UI.Theme.Apply(this); // apply the colors
    }

    /// <summary>Edit an existing account: username is fixed, leaving the password empty keeps the old one.</summary>
    public RegisterForm(User user) : this() // create the screen to edit an existing account
    {
        editing = user; // the user being edited
        Text = lblTitle.Text = "My profile"; // the title "My Profile"
        btnRegister.Text = "Save changes"; // the button text "Save"
        txtFullName.Text = user.FullName; // fill the name
        txtUsername.Text = user.Username; // fill the username
        txtUsername.ReadOnly = true; // the username cannot be changed
        txtEmail.Text = user.Email; // fill the email
        txtPhone.Text = user.Phone; // fill the phone
        lblPassword.Text = "New password (leave empty to keep)"; // new password (leave empty to keep the old one)
        lblConfirm.Text = "Confirm new password"; // confirm password
    }

    private void btnRegister_Click(object sender, EventArgs e) // when the register/save button is clicked
    {
        errorProvider.Clear(); // clear the old error marks
        string name = txtFullName.Text.Trim(), user = txtUsername.Text.Trim(); // name and username
        string email = txtEmail.Text.Trim(), phone = txtPhone.Text.Trim(); // email and phone
        bool ok = true; // is all the data valid?

        void Check(bool valid, Control c, string message) // small function to check one field
        {
            if (valid) return; // if it is valid, done
            errorProvider.SetError(c, message); // put an error mark next to the field
            if (ok) c.Focus(); // put the cursor on the first wrong field
            ok = false; // the data is not valid
        }

        Check(name.Length >= 3, txtFullName, "Enter your full name (at least 3 characters)."); // name is at least 3 letters
        Check(Regex.IsMatch(user, @"^\w{3,20}$"), txtUsername, "3-20 letters, digits or underscore."); // username is 3 to 20 letters or digits
        Check(editing != null || !SampleData.Users.Any(u => u.Username.Equals(user, StringComparison.OrdinalIgnoreCase)), txtUsername, "This username is already taken."); // username is not taken
        Check(Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"), txtEmail, "Enter a valid email address."); // email format is correct
        Check(Regex.IsMatch(phone, @"^01[0125]\d{8}$"), txtPhone, "Egyptian mobile: 11 digits starting with 010/011/012/015."); // Egyptian mobile number
        bool keepPassword = editing != null && txtPassword.Text.Length == 0; // when editing, an empty password keeps the old one
        Check(keepPassword || txtPassword.Text.Length >= 6, txtPassword, "Password must be at least 6 characters."); // password is at least 6 characters
        Check(txtConfirm.Text == txtPassword.Text, txtConfirm, "Passwords do not match."); // confirmation matches the password
        if (!ok) return; // if there are errors, stop

        if (editing != null) // if editing an existing account
        {
            editing.FullName = name; // update the name
            editing.Email = email; // update the email
            editing.Phone = phone; // update the phone
            if (!keepPassword) editing.PasswordHash = Passwords.Hash(txtPassword.Text); // if a new password was typed, hash it and save it
            UI.Theme.Info("Your profile has been updated."); // success message
            DialogResult = DialogResult.OK; // close the screen with success
            return; // stop
        }

        SampleData.Users.Add(new User // add a new user
        {
            Id = SampleData.NextId(SampleData.Users, u => u.Id), // new id
            FullName = name, Username = user, Email = email, Phone = phone, // the data
            PasswordHash = Passwords.Hash(txtPassword.Text), Role = UserRole.Passenger // the password is hashed, and the role is passenger
        });
        CreatedUsername = user; // save the new account name
        MessageBox.Show("Account created. You can sign in now.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information); // "account created" message
        DialogResult = DialogResult.OK; // close the screen with success
    }
}
