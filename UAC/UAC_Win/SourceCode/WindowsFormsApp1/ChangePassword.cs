 using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing;


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

                changePassword();
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message.ToString());
            }
        }

     

        private void changePassword()

            
        {
            string connectionString = "Data Source=HU\\MSSQLSERVER2019;Initial Catalog=UAC;User ID=sa;Password=123;Encrypt=False";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE UserProfile SET Password = @NewPassword WHERE UserName = @UserName";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NewPassword", txtNewPass.Text);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


