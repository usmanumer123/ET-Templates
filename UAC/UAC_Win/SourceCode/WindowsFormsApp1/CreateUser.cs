using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using Bunifu.Framework.UI;

namespace WindowsFormsApp1
{
    public partial class CreateUser : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString);
        public int UserId;
        private BunifuCustomDataGrid createUserDataGridView1;
        Helper hp = new Helper();
        UACEntities context = new UACEntities();

        public CreateUser()
        {
            InitializeComponent();
            createUserDataGridView1 = new BunifuCustomDataGrid();
            Shared.InitializeBunifuDataGridView(createUserDataGridView1);
            panel2.Controls.Add(createUserDataGridView1);
            //string[] columnsNames = new string[] { "Module", "Permission" };
            //Shared.AddColumnsToDataGridView(columnsNames, createUserDataGridView1);
            createUserDataGridView1.Show();
            createUserDataGridView1.CellClick += createUserDataGridView1_CellClick;

            SetButtonState(btnInsertUser, CheckUserPermission(Shared.RollsId, "Create User", "create"));
            SetButtonState(btnUpdateUser, CheckUserPermission(Shared.RollsId, "Create User", "edit"));
            SetButtonState(btnDeleteUser, CheckUserPermission(Shared.RollsId, "Create User", "delete"));
            // SetButtonState(createUserDataGridView1, isAdmin && CheckUserPermission(rol, "CreateUser", "view"));
            // btnUserPermission.Enabled = isAdmin && CheckUserPermission(Shared.RollsId, "CreateUser", "view");
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

        private void CreateUser_Load(object sender, EventArgs e)
        {
            txtCreatedBy.Text = Shared.RoleDesc.ToString();
            PopulateComboBox();
            GetUserRecord();
        }

        private void GetUserRecord()
        {
            string query = @" 
                            SELECT 
                                u.UserId, 
                                u.UserName, 
                                u.CreatedBy, 
                                r.RollsId,  -- Changed from RollsDesc to RollsId to ensure we get the correct value
                                r.RollsDesc AS Rolls,  -- Keep the alias for display purposes
                                u.Password, 
                                u.IsEnabled AS Activation
                            FROM 
                                UserProfile u
                            JOIN 
                                UserRolls r ON u.RollsID = r.RollsId";

            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            // Decrypt passwords
            foreach (DataRow row in dt.Rows)
            {
                row["Password"] = Shared.DecryptPassword(row["Password"].ToString());
            }

            createUserDataGridView1.DataSource = dt;
        }
      
        private void btnInsertUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    int selectedRollsId = Convert.ToInt32(comboBox1.SelectedValue);
                    if (UserId == 0) // Ensure UserId is 0, which means it's a new user
                    {
                        SqlCommand cmd = new SqlCommand("Insert into UserProfile (UserName, CreatedBy, RollsID, Password, IsEnabled) Values (@name, @created, @rolls, @password, @isenable)", con);

                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@name", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@created", Shared.UserId);
                        cmd.Parameters.AddWithValue("@rolls", selectedRollsId);
                        cmd.Parameters.AddWithValue("@password", Shared.EncryptPassword(txtPassword.Text));
                        cmd.Parameters.AddWithValue("@isenable", enableCheckbox.Checked);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("New user is successfully added", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GetUserRecord();
                        ResetUserControls();
                    }
                    else
                    {
                        MessageBox.Show("Please use the Update button to modify existing users.", "Operation not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch(Exception ex) 
            {
                hp.ErrorMessage("On Create User save button" + ex.Message.ToString());
            }    
        }

        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM UserProfile WHERE UserName = @UserName AND UserId != @UserId", con);
            cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            if (count > 0)
            {
                MessageBox.Show("User already exists. Please insert a new user.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnResetUser_Click(object sender, EventArgs e)
        {
            ResetUserControls();
        }

        private void ResetUserControls()
        {
            UserId = 0;
            txtUserName.Text = "";
            //txtCreatedBy.Text = "";
            txtRollsId.Text = "";
            txtPassword.Text = "";
            enableCheckbox.Checked = false;
            txtUserName.Focus();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    int selectedRollsId = Convert.ToInt32(comboBox1.SelectedValue);
                    if (UserId > 0)
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE UserProfile SET UserName=@name, CreatedBy=@created, RollsID=@rolls, Password=@password, IsEnabled=@isenable WHERE UserId=@Id", con);

                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@name", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@created", txtCreatedBy.Text);
                        cmd.Parameters.AddWithValue("@rolls", selectedRollsId);
                        cmd.Parameters.AddWithValue("@password", Shared.EncryptPassword(txtPassword.Text));
                        cmd.Parameters.AddWithValue("@isenable", enableCheckbox.Checked);
                        cmd.Parameters.AddWithValue("@Id", this.UserId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("User information successfully updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GetUserRecord();
                        ResetUserControls();
                    }
                    else
                    {
                        MessageBox.Show("Please select a user to update their information.", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch(Exception ex)
            {
                hp.ErrorMessage("On Create User update button" + ex.Message.ToString());
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserId > 0)
                {

                    SqlCommand cmd = new SqlCommand("DELETE FROM UserProfile WHERE UserId=@Id", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", this.UserId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("User information successfully deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetUserRecord();
                    ResetUserControls();
                }

                else
                {
                    MessageBox.Show("Please select user to delete his information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                hp.ErrorMessage("On Create User delete button" + ex.Message.ToString());
            }
        }

        private void createUserDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (createUserDataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    UserId = Convert.ToInt32(createUserDataGridView1.SelectedRows[0].Cells["UserId"].Value);
                    txtUserName.Text = createUserDataGridView1.SelectedRows[0].Cells["UserName"].Value.ToString();
                    txtCreatedBy.Text = createUserDataGridView1.SelectedRows[0].Cells["CreatedBy"].Value.ToString();
                    
                    // Retrieve and validate RollsDesc
                    string rollsDesc = createUserDataGridView1.SelectedRows[0].Cells["Rolls"].Value.ToString();
                    int rollsId = (int)createUserDataGridView1.SelectedRows[0].Cells["RollsId"].Value;
                    if (!string.IsNullOrEmpty(rollsDesc))
                    {
                        bool found = false;
                        foreach (DataRowView item in comboBox1.Items)
                        {
                            if (item["RollsDesc"].ToString() == rollsDesc && (int)item["RollsId"] == rollsId)
                            {
                                comboBox1.SelectedItem = item;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            MessageBox.Show($"No matching RollsDesc found for: '{rollsDesc}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Invalid Rolls format: '{rollsDesc}'. Unable to set Rolls.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    txtPassword.Text = createUserDataGridView1.SelectedRows[0].Cells["Password"].Value.ToString();
                    enableCheckbox.Checked = bool.Parse(createUserDataGridView1.SelectedRows[0].Cells["Activation"].Value.ToString());
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Error parsing data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PopulateComboBox()
        {
            comboBox1.DisplayMember = "RollsDesc"; // Set the display member to RollsDesc
            comboBox1.ValueMember = "RollsId"; // Set the value member to RollsId

            string query = "SELECT RollsId, RollsDesc FROM UserRolls";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            comboBox1.DataSource = dataTable; // Bind the DataTable to the ComboBox
            reader.Close();
            con.Close();
        }

        private void btnShowPass_MouseHover(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }

        private void btnShowPass_MouseLeave(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
