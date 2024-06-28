 using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace WindowsFormsApp1
{
    public partial class ChangePassword : Form
    {
        Helper hp = new Helper();

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            try
            {
                //btnNext.Visible = false;
                txtUserName.Text = Shared.Username.ToString();
                txtUserId.Text = Shared.UserId.ToString();
                txtNewPass.Text = Shared.DecryptPassword(Shared.Password);
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message.ToString());
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPass.Text != "")
                    changePassword();
                else
                    hp.InfoMessage("Enter valid password.");
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message.ToString());
            }
        }

        private void changePassword()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString))
            {
                string query = "UPDATE UserProfile SET Password = @NewPassword WHERE UserName = @UserName";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NewPassword", Shared.EncryptPassword(txtNewPass.Text));
                cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Successfully Changed Password", "Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        resetForm();
                    }
                    else
                    {
                        MessageBox.Show("Password change failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    hp.ErrorMessage(ex.Message.ToString());
                }
            }
        }

        private void resetForm()
        {
            txtNewPass.Text = "";
            txtNewPass.Focus();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            resetForm();
        }
        private void btnShowPass_MouseHover(object sender, EventArgs e)
        {
            txtNewPass.UseSystemPasswordChar = false;
        }

        private void btnShowPass_MouseLeave(object sender, EventArgs e)
        {
            txtNewPass.UseSystemPasswordChar = true;
        }
    }
}
