

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;


namespace WindowsFormsApp1
{
    public partial class CreateUserRole : Form
    {
        Helper hp = new Helper();

        UACEntities context = new UACEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString);
        public int RollsId;

        public CreateUserRole()
        {
            InitializeComponent();

            //Permissions getting in the basis of roleid
            var isAdmin = IsAdmin(Shared.RollsId);
            SetButtonState(btnDone, isAdmin && CheckUserPermission(Shared.RollsId, "Create User Role", "create"));
            SetButtonState(btnUpdate, isAdmin && CheckUserPermission(Shared.RollsId, "Create User Role", "edit"));
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
                MessageBox.Show("Please fill your field", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("New user role is successfully added", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetUserRecord();
                    txtRolesDesc.Text = "";
                    txtRolesDesc.Focus();
                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage("On Create User Role save button"+ex.Message.ToString());
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

                    MessageBox.Show("User role successfully updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetUserRecord(); // Refresh the DataGridView

                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage("On Create User Role update button"+ex.Message.ToString());
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

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
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

                    MessageBox.Show("User role deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetUserRecord();
                }
                else
                {
                    MessageBox.Show("Please select user role to delete", "Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                hp.ErrorMessage("On Create User Role delete button: "+ex.Message.ToString());
            }
            
        }


    }
}




