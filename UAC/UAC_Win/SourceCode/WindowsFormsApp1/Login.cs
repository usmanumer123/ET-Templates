using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";  
            txtPassword.Text = "";
        }

        private void usernameText_TextChanged(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                usererror.Visible = true;
            }
            else
            {
                usererror.Visible = false;
            }
        }

        private void passwordText_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                passerror.Visible = true;
            }
            else
            {
                passerror.Visible = false;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {         
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                usererror.Visible = true;
                Shared.hp.ErrorMessage("Please enter a user name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                passerror.Visible = true;
                Shared.hp.ErrorMessage("Please enter a password.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                usererror.Visible = true;
                passerror.Visible = true;
                Shared.hp.ErrorMessage("Please enter both username and password.");
                return;
            }

            //var user = context.UserProfiles.Where(a => a.UserName == txtUserName.Text).FirstOrDefault();
            var user = Shared.context.UserProfiles.FirstOrDefault(a => a.UserName == txtUserName.Text);

            if (user == null)
            {
                Shared.hp.ErrorMessage("User not found.");
                return;
            }

            if (!user.IsEnabled)
            {
             
                Shared.hp.WarningMessage("Your account is disabled. Please contact the administrator.");
                return;
            }

            if (!Shared.DecryptPassword(user.Password).Equals(txtPassword.Text))
            {
                Shared.hp.ErrorMessage("Incorrect password.");
                return;
            }

            this.Hide();
            var form = new Menu(user.RollsID);
            Shared.Username = user.UserName;
            Shared.Password = user.Password;
            Shared.UserId = user.UserId;
            Shared.RollsId = user.RollsID;
            Shared.CreatedBy = user.CreatedBy;
            form.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUserName.Select();
            usererror.Visible = true;
            passerror.Visible = true;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginBtn_Click(sender, e);
            }
        }

        private void btnShowPass_MouseLeave(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnShowPass_MouseHover(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }
    }
}
