
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SetUserPermission : Form
    {
        Helper hp = new Helper();
        SqlConnection con = new SqlConnection("Data Source=HU\\MSSQLSERVER2019;Initial Catalog=UAC;User ID=sa;Password=123;Encrypt=False");
        DataTable permissionsTable; // DataTable to hold the permissions data

        public SetUserPermission()
        {
            InitializeComponent();
        }

        private void SetUserPermission_Load(object sender, EventArgs e)
        {
            string query = "SELECT RollsId, RollsDesc FROM UserRolls";
            this.cmbRolls.SelectedIndexChanged -= new System.EventHandler(this.cmbRolls_SelectedIndexChanged);
            hp.PopulateCombo(cmbRolls, query, "RollsDesc", "RollsId");
            this.cmbRolls.SelectedIndexChanged += new System.EventHandler(this.cmbRolls_SelectedIndexChanged);

            // Load data with unchecked permissions initially
            LoadDataWithUncheckedPermissions();
        }

        private void cmbRolls_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRollsId = Convert.ToInt32(cmbRolls.SelectedValue);
            LoadDataForRollsId(selectedRollsId);
        }

        private void LoadDataForRollsId(int rollsId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = @rollsId", con);
                cmd.Parameters.AddWithValue("@rollsId", rollsId);
                permissionsTable = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                permissionsTable.Load(sdr);
                con.Close();

                // Check if there are no rows for the given RollsId
                if (permissionsTable.Rows.Count == 0)
                {
                    // Fetch default data (for example, for RollsId = 1) but with unchecked values
                    cmd = new SqlCommand("SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = 1", con);
                    permissionsTable = new DataTable();
                    con.Open();
                    sdr = cmd.ExecuteReader();
                    permissionsTable.Load(sdr);
                    con.Close();

                    // Set IsEnable to false for all rows
                    foreach (DataRow row in permissionsTable.Rows)
                    {
                        row["IsEnable"] = false;
                    }

                    // Insert default data for the new RollsId
                    foreach (DataRow row in permissionsTable.Rows)
                    {
                        cmd = new SqlCommand("INSERT INTO UserRollsPermission (Module, Permission, IsEnable, RollsId, CreatedBy) VALUES (@module, @permission, @isEnable, @rollsId, @createdBy)", con);
                        cmd.Parameters.AddWithValue("@module", row["Module"]);
                        cmd.Parameters.AddWithValue("@permission", row["Permission"]);
                        cmd.Parameters.AddWithValue("@isEnable", row["IsEnable"]);
                        cmd.Parameters.AddWithValue("@rollsId", rollsId);
                        cmd.Parameters.AddWithValue("@createdBy", Shared.UserId); // Replace 'admin' with the actual createdBy value
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    // Reload the permissions for the new RollsId
                    cmd = new SqlCommand("SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = @rollsId", con);
                    cmd.Parameters.AddWithValue("@rollsId", rollsId);
                    permissionsTable = new DataTable();
                    con.Open();
                    sdr = cmd.ExecuteReader();
                    permissionsTable.Load(sdr);
                    con.Close();
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

                // Setting the DataGridView's DataSource to the new DataTable
                createUserDataGridView1.DataSource = newDt;

                // Convert the IsEnable column to DataGridViewCheckBoxColumn
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "Activation";
                checkBoxColumn.Name = "Activation";
                checkBoxColumn.DataPropertyName = "Activation";
                createUserDataGridView1.Columns.Remove("Activation");
                createUserDataGridView1.Columns.Insert(2, checkBoxColumn);
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message);
            }
        }

        private void createUserDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == createUserDataGridView1.Columns["Activation"].Index)
                {
                    DataGridViewCheckBoxCell cell = createUserDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    string module = createUserDataGridView1.Rows[e.RowIndex].Cells["Module"].Value.ToString();
                    string permission = createUserDataGridView1.Rows[e.RowIndex].Cells["Permission"].Value.ToString();
                    bool isEnable = Convert.ToBoolean(cell.Value);

                    SqlCommand cmd = new SqlCommand("IF EXISTS (SELECT * FROM UserRollsPermission WHERE Module = @module AND Permission = @permission AND RollsId = @rollsId) " +
                        "UPDATE UserRollsPermission SET IsEnable = @isEnable WHERE Module = @module AND Permission = @permission AND RollsId = @rollsId " +
                        "ELSE " +
                        "INSERT INTO UserRollsPermission (Module, Permission, IsEnable, RollsId, CreatedBy) VALUES (@module, @permission, @isEnable, @rollsId, @createdBy)", con);
                    cmd.Parameters.AddWithValue("@isEnable", isEnable);
                    cmd.Parameters.AddWithValue("@module", module);
                    cmd.Parameters.AddWithValue("@permission", permission);
                    cmd.Parameters.AddWithValue("@rollsId", Convert.ToInt32(cmbRolls.SelectedValue));
                    cmd.Parameters.AddWithValue("@createdBy", "admin"); // Replace 'admin' with the actual createdBy value

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message);
            }
        }

        private void LoadDataWithUncheckedPermissions()
        {
            try
            {
                // Fetching only data for RollsId = 1
                SqlCommand cmd = new SqlCommand("SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = 1", con);

                permissionsTable = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                permissionsTable.Load(sdr);
                con.Close();

                // Creating a new DataTable with only the required columns
                DataTable newDt = new DataTable();
                newDt.Columns.Add("Module");
                newDt.Columns.Add("Permission");
                newDt.Columns.Add("Activation", typeof(bool));

                // Populating the new DataTable with data from the fetched DataTable
                foreach (DataRow row in permissionsTable.Rows)
                {
                    newDt.Rows.Add(row["Module"], row["Permission"], false); // Set Activation to false
                }

                // Setting the DataGridView's DataSource to the new DataTable
                createUserDataGridView1.DataSource = newDt;

                // Convert the IsEnable column to DataGridViewCheckBoxColumn
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "Activation";
                checkBoxColumn.Name = "Activation";
                checkBoxColumn.DataPropertyName = "Activation";
                createUserDataGridView1.Columns.Remove("Activation");
                createUserDataGridView1.Columns.Insert(2, checkBoxColumn);
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Permissions updated successfully","", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
    }
}

