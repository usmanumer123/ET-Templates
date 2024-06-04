using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;


namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        public static string Username;
        public static string OldPassword;
        public Login()
        {
            InitializeComponent();
        }

        CFL_CV_PracticeEntities1 context = new CFL_CV_PracticeEntities1();


        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            usernameText.Text = "";
            passwordText.Text = "";
            errormsg.Text = "Please Fill Your Details";
        }

        private void usernameText_TextChanged(object sender, EventArgs e)
        {
            if (usernameText.Text == "")
            {
                usererror.Visible = true;
                errormsg.Text = "Please Fill Your Username";
            }
            else
            {
                usererror.Visible = false;
            }
        }

        private void usernameText_Leave(object sender, EventArgs e)
        {

            if (usernameText.Text == "")
            {
                usernameText.Text = "Enter Your Username ...";
                usernameText.ForeColor = Color.Silver;
             
            }

        }

        private void usernameText_Enter(object sender, EventArgs e)
        {

            if (usernameText.Text == "Enter Your Username ...")
            {
                usernameText.Text = "";
                usernameText.ForeColor = Color.Blue;

            }

        }

        private void passwordText_Enter(object sender, EventArgs e)
        {
            if (passwordText.Text == "Enter Your Password ...")
            {
                passwordText.Text = "";
                passwordText.ForeColor = Color.Blue;

            }
        }

        private void passwordText_Leave(object sender, EventArgs e)
        {
            if (passwordText.Text == "")
            {
                passwordText.Text = "Enter Your Password ...";
                passwordText.ForeColor = Color.Silver;
               


            }

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        //Autheticate Method
        private bool AuthenticateUser()
        {
            var ListOfUser = context.UserProfiles.ToList();//.Where(e => e.UserName == txtUser.Text.ToString() && e.Password == txtPassword.Text.ToString());
            if (ListOfUser.Count() > 0)
            {
                var v = ListOfUser.ElementAt(0).UserId;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void passwordText_TextChanged(object sender, EventArgs e)
        {

            if (passwordText.Text == "")
            {
                passerror.Visible = true;
                errormsg.Text = "Please Fill Your Password";
            }
            else
            {
                passerror.Visible = false;
            }
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            //Username = usernameText.Text;
            //OldPassword = passwordText.Text;


            if (usernameText.Text != "" && passwordText.Text != "")
            {
                usererror.Visible = true;
                passerror.Visible= true;
                errormsg.Text = "Please Fill Your Details";
                var user = context.UserProfiles.Where(a => a.UserName == usernameText.Text.ToString()).FirstOrDefault();
                if (user!=null && user.IsEnabled)
                {
                    if (user.Password.Equals(passwordText.Text)) {

                        this.Hide();
                       
                        var form = new Menu(user.RollsID);
                        Shared.Username = user.UserName.ToString();
                        Shared.UserId=user.UserId;
                        Shared.RollsId = user.RollsID;
                        Shared.RoleDesc = user.CreatedBy;
                       // Shared.CreatedBy= user.CreatedBy;   
                        
                        
                         form.Show();

                    }
                    else
                    {
                        MessageBox.Show("Password Not Correct...");

                    }
                }

                else
                {
                    MessageBox.Show("You are unable to login...", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   //  MessageBox.Show("Your Credentials Not Matched...");
                }
            }

            else
            {
                MessageBox.Show("Not Found user");
                //errormsg.Text = "Please Fill Your Details !";
                //usererror.Visible = true;
                //passerror.Visible = true;
               
            }
        }

        private void usernameText_Click(object sender, EventArgs e)
        {
            usererror.Visible = false;
            passerror.Visible = true;
            //errormsg.Visible = false;
        }

        private void passwordText_Click(object sender, EventArgs e)
        {
            passerror.Visible = false;
            usererror.Visible = true;
            //errormsg.Visible = false;
        }


    }
}
