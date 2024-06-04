

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Drawing;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class CreateUserRole : Form
    {
        Helper hp = new Helper();

        CFL_CV_PracticeEntities1 context = new CFL_CV_PracticeEntities1();
        SqlConnection con = new SqlConnection("Data Source=HU\\MSSQLSERVER2019;Initial Catalog=CFL_CV_Practice;User ID=sa;Password=123;Encrypt=False");
        public int RollsId;

        public CreateUserRole()
        {
            InitializeComponent();

            //Permissions getting in the basis of roleid
            var isAdmin = IsAdmin(Shared.RollsId);
            SetButtonState(btnDone, isAdmin && CheckUserPermission(Shared.RollsId, "Create User Role", "create"));
            SetButtonState(btnDone, isAdmin && CheckUserPermission(Shared.RollsId, "Create User Role", "edit"));
            SetButtonState(btnDeleteUser, isAdmin && CheckUserPermission(Shared.RollsId, "Create User Role", "delete"));

        }

        private bool IsAdmin(int roleId)
        {
            return true;
        }



        private bool CheckUserPermission(int roleId, string module, string permission)
        {
            var permissionRecord = context.UserRollsPermissions.FirstOrDefault(p => p.RollsId == roleId && p.Module == module && p.Permission == permission);
            return permissionRecord != null && permissionRecord.IsEnable;
        }


        private void SetButtonState(Button button, bool isEnabled)
        {
            button.Enabled = isEnabled;
            button.BackColor = isEnabled ? SystemColors.Control : Color.LightGray;
            button.ForeColor = isEnabled ? SystemColors.ControlText : Color.LightGray;
        }

        private void CreateUserRole_Load(object sender, EventArgs e)
        {
            txtCreated.Text = Shared.RoleDesc.ToString();
            GetUserRecord();
        }

        
        private bool IsValid()
        {
            if (txtRolesDesc.Text == string.Empty)
            {
                MessageBox.Show("Fill All Your Fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM UserRolls WHERE RollsDesc = @RollsDesc", con);
            cmd.Parameters.AddWithValue("@RollsDesc", txtRolesDesc.Text);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            if (count > 0)
            {
                MessageBox.Show("Roll has been already exist. Please Insert new Roll", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void GetUserRecord()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM UserRolls", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            userRollsDataGridView1.DataSource = dt;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO UserRolls (RollsDesc, CreatedBy) VALUES (@RollsDesc, @CreatedBy)", con))
                    {
                        cmd.Parameters.AddWithValue("@RollsDesc", txtRolesDesc.Text);
                        cmd.Parameters.AddWithValue("@CreatedBy", Shared.UserId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("New User is Successfully Added", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetUserRecord();
                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtRolesDesc.Text = "";
            enableCheckbox.Checked = false;

            txtRolesDesc.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {


            try
            {
                if (IsValid())
                {
                    con.Open();
                    foreach (DataGridViewRow row in userRollsDataGridView1.Rows)
                    {
                        if (row.Cells["RollsId"].Value != null /*&& row.Cells["IsEnabled"].Value != null*/)
                        {
                            int rollsId = Convert.ToInt32(row.Cells["RollsId"].Value);
                            // bool isEnabled = Convert.ToBoolean(row.Cells["IsEnabled"].Value);

                            SqlCommand cmd = new SqlCommand("UPDATE UserRolls SET RollsDesc=@rolldesc WHERE RollsId = @id", con);

                            cmd.Parameters.AddWithValue("@rolldesc", txtRolesDesc.Text);
                            //cmd.Parameters.AddWithValue("@isenable", enableCheckbox.Checked);
                            cmd.Parameters.AddWithValue("@id", this.RollsId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                    GetUserRecord();

                    MessageBox.Show("User Information Successfully Updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetUserRecord(); // Refresh the DataGridView

                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message.ToString());
            }


        }





        private void createUserDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (userRollsDataGridView1.SelectedRows.Count > 0)
            {
                RollsId = Convert.ToInt32(userRollsDataGridView1.SelectedRows[0].Cells[0].Value);
                txtRolesDesc.Text = userRollsDataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtCreated.Text = userRollsDataGridView1.SelectedRows[0].Cells[2].Value.ToString();

               // enableCheckbox.Checked = bool.Parse(userRollsDataGridView1.SelectedRows[0].Cells[2].Value.ToString());

            }
        }

        private void userRollsDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (RollsId > 0)
            {
                using (SqlCommand cmd = new SqlCommand("DeleteUserAndPermissions", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RollsId", this.RollsId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("User Information Successfully Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetUserRecord();
            }
            else
            {
                MessageBox.Show("Please Select User to Delete his Information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}




