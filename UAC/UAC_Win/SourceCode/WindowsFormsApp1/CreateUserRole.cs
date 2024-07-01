using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace WindowsFormsApp1
{
    public partial class CreateUserRole : Form
    {
        private int RollsId;
        private BunifuCustomDataGrid userRollsDataGridView1;

        public CreateUserRole()
        {
            InitializeComponent();
            userRollsDataGridView1 = new BunifuCustomDataGrid();
            Shared.InitializeBunifuDataGridView(userRollsDataGridView1);
            panel6.Controls.Add(userRollsDataGridView1);
            string[] columnsNames = new string[] { "RollsId", "RollsDesc", "CreatedBy" };
            Shared.AddColumnsToDataGridView(columnsNames, userRollsDataGridView1);
            userRollsDataGridView1.Show();
            userRollsDataGridView1.CellClick += userRollsDataGridView1_CellClick;
            //Permissions getting in the basis of roleid
            Shared.SetButtonState(btnDone, CheckUserPermission(Shared.RollsId, "Create User Role", "create"));
            Shared.SetButtonState(btnUpdate, CheckUserPermission(Shared.RollsId, "Create User Role", "edit"));
            Shared.SetButtonState(btnDeleteUser, CheckUserPermission(Shared.RollsId, "Create User Role", "delete"));
        }

        private bool CheckUserPermission(int roleId, string module, string permission)
        {
            var permissionRecord = Shared.context.UserRollsPermissions.FirstOrDefault(p => p.RollsId == roleId && p.Module == module && p.Permission == permission);
            return permissionRecord != null && permissionRecord.IsEnable;
        }

        private void CreateUserRole_Load(object sender, EventArgs e)
        {
            GetUserRecord();
        }
        
        private bool IsValid()
        {
            if (txtRolesDesc.Text == string.Empty)
            {
                Shared.hp.ErrorMessage("Please fill your field");
                return false;
            }

            string query = $@"SELECT COUNT(*) FROM UserRolls WHERE RollsDesc = '{txtRolesDesc.Text}'";
            DataSet ds = Shared.hp.GetDataset(query);
            int count = 0;
            if (ds != null && ds.Tables.Count > 0)
            {
                count = (int)ds.Tables[0].Rows[0][0];
            }

            if (count > 0)
            {
                Shared.hp.WarningMessage("Roll has been already exist. Please Insert new Roll");
                return false;
            }

            return true;
        }

        private void GetUserRecord()
        {
            string query = @"SELECT RollsId, RollsDesc, CreatedBy FROM UserRolls";
            DataSet ds = Shared.hp.GetDataset(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                userRollsDataGridView1.DataSource = dt;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    string query = $@"INSERT INTO UserRolls (RollsDesc, CreatedBy) VALUES ('{txtRolesDesc.Text}', {Shared.UserId})";
                    bool flag = Shared.hp.PostDataset(query);
                    if (flag)
                        Shared.hp.InfoMessage("New user role is successfully added");
                    else
                        Shared.hp.InfoMessage("New user role doesn't added");
                    GetUserRecord();
                    txtRolesDesc.Text = "";
                    txtRolesDesc.Focus();
                }
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage("On Create User Role save button"+ex.Message.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtRolesDesc.Text = "";
            txtRolesDesc.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    string query = $@"UPDATE UserRolls SET RollsDesc = '{txtRolesDesc.Text}' WHERE RollsId = {this.RollsId}";
                    bool flag = Shared.hp.PostDataset(query);
                    GetUserRecord();
                    Shared.hp.InfoMessage("User role successfully updated");
                }
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage("On Create User Role update button"+ex.Message.ToString());
            }
        }

        private void userRollsDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (userRollsDataGridView1.SelectedRows.Count > 0)
            {
                RollsId = Convert.ToInt32(userRollsDataGridView1.SelectedRows[0].Cells[0].Value);
                txtRolesDesc.Text = userRollsDataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (RollsId > 0)
                {
                    string query = $@"EXEC DeleteUserAndPermissions @RollsId = {this.RollsId}";
                    bool flag = Shared.hp.PostDataset(query);
                    if (flag)
                        Shared.hp.InfoMessage("User role deleted Successfully");
                    else
                        Shared.hp.InfoMessage("User role doesn't deleted.");
                    GetUserRecord();
                }
                else
                {
                    Shared.hp.ErrorMessage("Please select user role to delete");
                }

            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage("On Create User Role delete button: "+ex.Message.ToString());
            }
        }
    }
}
