using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        public static string Username;
        public static string OldPassword;
        UACEntities context = new UACEntities();
        Helper hp=new Helper();

        public Login()
        {
            InitializeComponent();
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void usernameText_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text == "Enter Your Username ...")
            {
                txtUserName.Text = "";
                txtUserName.ForeColor = Color.Blue;
            }
        }

        private void passwordText_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Enter Your Password ...")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Blue;
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

        private void loginbtn_Click(object sender, EventArgs e)
        {         
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                usererror.Visible = true;
                hp.ErrorMessage("Please enter a user name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                passerror.Visible = true;
                hp.ErrorMessage("Please enter a password.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                usererror.Visible = true;
                passerror.Visible = true;
                hp.ErrorMessage("Please enter both username and password.");
                return;
            }

            //var user = context.UserProfiles.Where(a => a.UserName == txtUserName.Text).FirstOrDefault();
            var user = context.UserProfiles.FirstOrDefault(a => a.UserName == txtUserName.Text);

            if (user == null)
            {
                hp.ErrorMessage("User not found.");
                return;
            }

            if (!user.IsEnabled)
            {
             
                hp.WarningMessage("Your account is disabled. Please contact the administrator.");
                return;
            }

            if (!Shared.DecryptPassword(user.Password).Equals(txtPassword.Text))
            {
                hp.ErrorMessage("Incorrect password.");
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

        private void usernameText_Click(object sender, EventArgs e)
        {
            usererror.Visible = false;
            passerror.Visible = true;
        }

        private void passwordText_Click(object sender, EventArgs e)
        {
            passerror.Visible = false;
            usererror.Visible = true;
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
                // Simulate button click
                loginbtn_Click(sender, e);
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
