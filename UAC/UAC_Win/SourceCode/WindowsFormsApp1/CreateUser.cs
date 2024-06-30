using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Configuration;
using Bunifu.Framework.UI;

namespace WindowsFormsApp1
{
    public partial class CreateUser : Form
    {
        private int UserId;
        private BunifuCustomDataGrid createUserDataGridView1;
        private DataSet ds;
        private static string UserName;

        public CreateUser()
        {
            InitializeComponent();
            createUserDataGridView1 = new BunifuCustomDataGrid();
            Shared.InitializeBunifuDataGridView(createUserDataGridView1);
            panel2.Controls.Add(createUserDataGridView1);
            string[] columnsNames = new string[] { "UserId", "UserName", "CreatedBy", "RollsId", "Rolls", "Password"};
            Shared.AddColumnsToDataGridView(columnsNames, createUserDataGridView1);
            createUserDataGridView1.CellClick += createUserDataGridView1_CellClick;

            Shared.SetButtonState(btnInsertUser, CheckUserPermission(Shared.RollsId, "Create User", "create"));
            Shared.SetButtonState(btnUpdateUser, CheckUserPermission(Shared.RollsId, "Create User", "edit"));
            Shared.SetButtonState(btnDeleteUser, CheckUserPermission(Shared.RollsId, "Create User", "delete"));
        }
        
        private bool CheckUserPermission(int roleId, string module, string permission)
        {
            var permissionRecord = Shared.context.UserRollsPermissions.FirstOrDefault(p => p.RollsId == roleId && p.Module == module && p.Permission == permission);
            return permissionRecord != null && permissionRecord.IsEnable;
        }

        private void CreateUser_Load(object sender, EventArgs e)
        {
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
                                r.RollsId,
                                r.RollsDesc AS Rolls,
                                u.Password, 
                                u.IsEnabled AS Activation
                            FROM 
                                UserProfile u
                            JOIN 
                                UserRolls r ON u.RollsID = r.RollsId";
            ds = Shared.hp.GetDataset(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                if (!dt.Columns.Contains("Activation"))
                {
                    dt.Columns.Add("Activation", typeof(bool));
                }

                // Decrypt passwords
                foreach (DataRow row in dt.Rows)
                {
                    row["Password"] = Shared.DecryptPassword(row["Password"].ToString());
                }

                createUserDataGridView1.DataSource = dt;
            }
            createUserDataGridView1.Show();
        }
      
        private void btnInsertUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid("Insert") && comboBox1.Text != "")
                {
                    int selectedRollsId = Convert.ToInt32(comboBox1.SelectedValue);
                    if (UserId == 0) // Ensure UserId is 0, which means it's a new user
                    {
                        int isEnabled = enableCheckbox.Checked ? 1 : 0;
                        string query = $@"
                            Insert into UserProfile (UserName, CreatedBy, RollsID, Password, IsEnabled) 
                            Values ('{txtUserName.Text}', {Shared.UserId}, {selectedRollsId}, '{Shared.EncryptPassword(txtPassword.Text)}', {isEnabled})";
                        Shared.hp.PostDataset(query);

                        Shared.hp.InfoMessage("New user is successfully added");
                        GetUserRecord();
                        ResetUserControls();
                    }
                    else
                    {
                        Shared.hp.WarningMessage("Please use the Update button to modify existing users.");
                    }
                }
                else if(comboBox1.Text == "")
                {
                    Shared.hp.InfoMessage("Select user role.");
                }
            }
            catch(Exception ex) 
            {
                Shared.hp.ErrorMessage("On Create User save button" + ex.Message.ToString());
            }
        }

        private bool IsValid(string str)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                Shared.hp.ErrorMessage("Username is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                Shared.hp.ErrorMessage("Password is required.");
                return false;
            }

            if (str == "Insert")
            {
                string query = $@"
                    SELECT COUNT(*)
                    FROM UserProfile 
                    WHERE UserName = '{txtUserName.Text}' 
                    AND UserId = {UserId}";
                ds = Shared.hp.GetDataset(query);
                if (ds.Tables != null && (int)ds.Tables[0].Rows[0][0] == 0)
                {
                    Shared.hp.WarningMessage("User already exists. Please insert a new user.");
                    return false;
                }
            }
            else if (str == "Update")
            {
                string query = $@"
                    SELECT COUNT(*)
                    FROM UserProfile 
                    WHERE UserName = '{UserName}' 
                    AND UserId = {UserId}";
                ds = Shared.hp.GetDataset(query);
                if (ds.Tables != null && (int)ds.Tables[0].Rows[0][0] > 0)
                {
                    return true;
                }
                else
                {
                    Shared.hp.WarningMessage("User doesn't exists. Please insert a new user.");
                }
            }

            return true;
        }

        private void btnResetUser_Click(object sender, EventArgs e)
        {
            ResetUserControls();
            createUserDataGridView1.ClearSelection();
        }

        private void ResetUserControls()
        {
            UserId = 0;
            txtUserName.Text = "";
            txtPassword.Text = "";
            enableCheckbox.Checked = false;
            txtUserName.Focus();
            comboBox1.Text = "";
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid("Update"))
                {
                    int selectedRollsId = Convert.ToInt32(comboBox1.SelectedValue);
                    if (UserId > 0)
                    {
                        int isEnabled = enableCheckbox.Checked ? 1 : 0;
                        string query = $@"
                            UPDATE UserProfile SET UserName='{txtUserName.Text}', RollsID={selectedRollsId},
                            Password='{Shared.EncryptPassword(txtPassword.Text)}', IsEnabled={isEnabled} 
                            WHERE UserId={this.UserId}";
                        
                        bool flag = Shared.hp.PostDataset(query);

                        if(flag)
                            Shared.hp.InfoMessage("User information successfully updated");
                        else
                        {
                            Shared.hp.ErrorMessage("User information not updated");
                        }
                        GetUserRecord();
                        ResetUserControls();
                    }
                    else
                    {
                        Shared.hp.ErrorMessage("Please select a user to update their information.");
                    }
                }
            }
            catch(Exception ex)
            {
                Shared.hp.ErrorMessage("On Create User update button" + ex.Message.ToString());
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserId > 0)
                {
                    string query = $@"
                            DELETE FROM UserProfile WHERE UserId={this.UserId}";
                    bool flag = Shared.hp.PostDataset(query);

                    if (flag)
                        Shared.hp.InfoMessage("User information successfully deleted");
                    else
                        Shared.hp.InfoMessage("User information not deleted");
                    GetUserRecord();
                    ResetUserControls();
                }

                else
                {
                    Shared.hp.ErrorMessage("Please select user to delete his information");
                }
            }
            catch(Exception ex)
            {
                Shared.hp.ErrorMessage("On Create User delete button" + ex.Message.ToString());
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
                    UserName = txtUserName.Text;

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
            comboBox1.DisplayMember = "RollsDesc";
            comboBox1.ValueMember = "RollsId";

            string query = $@"SELECT RollsId, RollsDesc FROM UserRolls";

            ds = Shared.hp.GetDataset(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                comboBox1.DataSource = dt;
                comboBox1.Text = "";
            }
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
