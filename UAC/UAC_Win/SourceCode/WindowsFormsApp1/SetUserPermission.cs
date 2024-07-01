using Bunifu.Framework.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SetUserPermission : Form
    {
        private BunifuCustomDataGrid createUserDataGridView1;
        DataTable permissionsTable;
        private string activation_module = null;
        private string activation_permission = null;
        private bool activation_isEnable = false;
        private int _rollsId = -1;

        public SetUserPermission()
        {
            InitializeComponent();
            panel5.Show();
            createUserDataGridView1 = new BunifuCustomDataGrid();
            Shared.InitializeBunifuDataGridView(createUserDataGridView1);
            panel6.Controls.Add(createUserDataGridView1);
            string[] columnsNames = new string[] { "Module", "Permission" };
            Shared.AddColumnsToDataGridView(columnsNames, createUserDataGridView1);
            LoadDataForRollsId(-1);
        }

        private void SetUserPermission_Load(object sender, EventArgs e)
        {
            string query = "SELECT RollsId, RollsDesc FROM UserRolls";
            this.cmbRolls.SelectedIndexChanged -= new System.EventHandler(this.cmbRolls_SelectedIndexChanged);
            Shared.hp.PopulateCombo(cmbRolls, query, "RollsDesc", "RollsId");
            this.cmbRolls.SelectedIndexChanged += new System.EventHandler(this.cmbRolls_SelectedIndexChanged);
        }

        private void cmbRolls_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRollsId = Convert.ToInt32(cmbRolls.SelectedValue);
            LoadDataForRollsId(selectedRollsId);
            createUserDataGridView1.Show();
        }

        private void LoadDataForRollsId(int rollsId)
        {
            _rollsId = rollsId;
            try
            {
                if(rollsId == -1)
                    rollsId = 1;

                string query = $@"
                            SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = {rollsId}";
                DataSet ds = Shared.hp.GetDataset(query);
                if (ds != null && ds.Tables.Count > 0)
                {
                    permissionsTable = new DataTable();
                    permissionsTable = ds.Tables[0];
                }

                // Check if there are no rows for the given RollsId
                if (permissionsTable.Rows.Count == 0 && _rollsId != -1)
                {
                    // Fetch default data (for example, for RollsId = 1) but with unchecked values
                    query = $@"
                            SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = {1}";
                    ds = Shared.hp.GetDataset(query);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        permissionsTable = new DataTable();
                        permissionsTable = ds.Tables[0];
                    }

                    // Set IsEnable to false for all rows
                    foreach (DataRow row in permissionsTable.Rows)
                    {
                        row["IsEnable"] = false;
                    }

                    // Insert default data for the new RollsId
                    foreach (DataRow row in permissionsTable.Rows)
                    {
                        int isEnable = (bool)row["IsEnable"] ? 1 : 0;
                        query = $@"
                            INSERT INTO UserRollsPermission (Module, Permission, IsEnable, RollsId, CreatedBy) 
                            VALUES ('{row["Module"]}', '{row["Permission"]}', {isEnable}, {rollsId}, {Shared.UserId})";// Replace 'admin' with the actual createdBy value
                        bool flag = Shared.hp.PostDataset(query);
                    }

                    // Reload the permissions for the new RollsId
                    query = $@"
                            SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = {rollsId}";
                    ds = Shared.hp.GetDataset(query);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        permissionsTable = new DataTable();
                        permissionsTable = ds.Tables[0];
                    }
                }

                // Creating a new DataTable with only the required columns
                DataTable newDt = new DataTable();
                newDt.Columns.Add("Module");
                newDt.Columns.Add("Permission");
                newDt.Columns.Add("Activation", typeof(bool));

                // Populating the new DataTable with data from the fetched DataTable
                foreach (DataRow row in permissionsTable.Rows)
                {
                    newDt.Rows.Add(row["Module"], row["Permission"], row["IsEnable"]);
                }
                if (_rollsId == -1)
                {
                    // Set IsEnable to false for all rows
                    foreach (DataRow row in newDt.Rows)
                    {
                        row["Activation"] = false;
                    }
                }
                // Setting the DataGridView's DataSource to the new DataTable
                createUserDataGridView1.DataSource = newDt;
                createUserDataGridView1.Show();

                // Convert the IsEnable column to DataGridViewCheckBoxColumn
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "Activation";
                checkBoxColumn.Name = "Activation";
                checkBoxColumn.DataPropertyName = "Activation";
                checkBoxColumn.Width = 1000;
                createUserDataGridView1.Columns.Remove("Activation");
                createUserDataGridView1.Columns.Insert(2, checkBoxColumn);
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage(ex.Message);
            }
        }

        private void createUserDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == createUserDataGridView1.Columns["Activation"].Index)
                {
                    DataGridViewCheckBoxCell cell = createUserDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    activation_module = createUserDataGridView1.Rows[e.RowIndex].Cells["Module"].Value.ToString();
                    activation_permission = createUserDataGridView1.Rows[e.RowIndex].Cells["Permission"].Value.ToString();
                    activation_isEnable = Convert.ToBoolean(cell.Value);
                }
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage(ex.Message);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRolls.Text != "")
                {
                    bool flag = false;
                    foreach (DataGridViewRow row in createUserDataGridView1.Rows)
                    {
                        if (row != null)
                        {
                            activation_isEnable = (bool)row.Cells["Activation"].Value;
                            activation_module = row.Cells["Module"].Value.ToString();
                            activation_permission = row.Cells["Permission"].Value.ToString();
                            string query = "UPDATE UserRollsPermission " +
                               "SET IsEnable = '" + activation_isEnable + "' " +
                               "WHERE Module = '" + activation_module + "' " +
                               "AND Permission = '" + activation_permission + "' " +
                               "AND RollsId = '" + _rollsId + "'";
                            flag = Shared.hp.PostDataset(query);
                        }
                    }
                    if (flag)
                    {
                        Shared.hp.InfoMessage("Permissions updated successfully");
                    }
                }
                else
                {
                    Shared.hp.InfoMessage("Select user roll.");
                }
            }
            catch (Exception ex)
            {
                Shared.hp.ErrorMessage("btnDone_Click: " + ex.Message.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbRolls.Text = "";
            if (createUserDataGridView1.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["Activation"] = false;
                }
            }
        }
    }
}
