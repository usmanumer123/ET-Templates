 using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            try
            {
                txtUserName.Text = Shared.Username.ToString();
                txtUserId.Text = Shared.UserId.ToString();

                string query = $@"Select Password from UserProfile u where u.UserName = '{Shared.Username.ToString()}' and u.UserId = {Shared.UserId}";
                DataSet ds = Shared.hp.GetDataset(query);
                if (ds != null && ds.Tables.Count > 0)
                {
                    txtNewPass.Text = Shared.DecryptPassword(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage(ex.Message.ToString());
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPass.Text != "")
                    changePassword();
                else
                    Shared.hp.InfoMessage("Enter valid password.");
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage(ex.Message.ToString());
            }
        }

        private void changePassword()
        {
            string query = $@"UPDATE UserProfile SET Password = '{Shared.EncryptPassword(txtNewPass.Text)}' WHERE UserName = '{txtUserName.Text}'";
            bool flag = Shared.hp.PostDataset(query);
            if (flag)
            {
                Shared.hp.InfoMessage("Successfully Changed Password");
                resetForm();
            }
            else
            {
                Shared.hp.ErrorMessage("Password change failed");
            }
        }

        private void resetForm()
        {
            string query = $@"Select Password from UserProfile u where u.UserName = '{Shared.Username.ToString()}' and u.UserId = {Shared.UserId}";
            DataSet ds = Shared.hp.GetDataset(query);
            if (ds != null && ds.Tables.Count > 0)
            {
                txtNewPass.Text = Shared.DecryptPassword(ds.Tables[0].Rows[0][0].ToString());
            }
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
