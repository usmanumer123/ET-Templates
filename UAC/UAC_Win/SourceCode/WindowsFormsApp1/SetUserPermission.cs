
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SetUserPermission : Form
    {
        private int RollId = -1;
        Helper hp = new Helper();
        SqlConnection con = new SqlConnection("Data Source=HP\\HASSAN;Initial Catalog=UAC;User ID=sa;Password=123;Encrypt=False");
        DataTable permissionsTable; // DataTable to hold the permissions data
        private string activation_module = null;
        private string activation_permission = null;
        private bool activation_isEnable = false;

        public SetUserPermission(int rollId)
        {
            RollId = rollId;
            InitializeComponent();
        }

        private void SetUserPermission_Load(object sender, EventArgs e)
        {
            string query = "SELECT RollsId, RollsDesc FROM UserRolls";
            this.cmbRolls.SelectedIndexChanged -= new System.EventHandler(this.cmbRolls_SelectedIndexChanged);
            hp.PopulateCombo(cmbRolls, query, "RollsDesc", "RollsId");
            this.cmbRolls.SelectedIndexChanged += new System.EventHandler(this.cmbRolls_SelectedIndexChanged);
            panel5.Hide();
            // Load data with unchecked permissions initially
            //LoadDataWithUncheckedPermissions();
        }

        private void cmbRolls_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel5.Show();
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
                    activation_module = createUserDataGridView1.Rows[e.RowIndex].Cells["Module"].Value.ToString();
                    activation_permission = createUserDataGridView1.Rows[e.RowIndex].Cells["Permission"].Value.ToString();
                    activation_isEnable = Convert.ToBoolean(cell.Value);
                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message);
            }
        }

        //private void LoadDataWithUncheckedPermissions()
        //{
        //    try
        //    {
        //        // Fetching only data for RollsId = 1
        //        SqlCommand cmd = new SqlCommand("SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = @RollId", con);
        //        cmd.Parameters.AddWithValue("@RollId", RollId);

        //        permissionsTable = new DataTable();
        //        con.Open();
        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        permissionsTable.Load(sdr);
        //        con.Close();

        //        // Creating a new DataTable with only the required columns
        //        DataTable newDt = new DataTable();
        //        newDt.Columns.Add("Module");
        //        newDt.Columns.Add("Permission");
        //        newDt.Columns.Add("Activation", typeof(bool));

        //        // Populating the new DataTable with data from the fetched DataTable
        //        foreach (DataRow row in permissionsTable.Rows)
        //        {
        //            newDt.Rows.Add(row["Module"], row["Permission"], false); // Set Activation to false
        //        }

        //        // Setting the DataGridView's DataSource to the new DataTable
        //        createUserDataGridView1.DataSource = newDt;

        //        // Convert the IsEnable column to DataGridViewCheckBoxColumn
        //        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
        //        checkBoxColumn.HeaderText = "Activation";
        //        checkBoxColumn.Name = "Activation";
        //        checkBoxColumn.DataPropertyName = "Activation";
        //        createUserDataGridView1.Columns.Remove("Activation");
        //        createUserDataGridView1.Columns.Insert(2, checkBoxColumn);
        //    }
        //    catch (Exception ex)
        //    {
        //        hp.ErrorMessage(ex.Message);
        //    }
        //}

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("IF EXISTS (SELECT * FROM UserRollsPermission WHERE Module = @module AND Permission = @permission AND RollsId = @rollsId) " +
                    "UPDATE UserRollsPermission SET IsEnable = @isEnable WHERE Module = @module AND Permission = @permission AND RollsId = @rollsId " +
                    "ELSE " +
                    "INSERT INTO UserRollsPermission (Module, Permission, IsEnable, RollsId, CreatedBy) VALUES (@module, @permission, @isEnable, @rollsId, @createdBy)", con))
                {
                    cmd.Parameters.AddWithValue("@isEnable", activation_isEnable);
                    cmd.Parameters.AddWithValue("@module", activation_module);
                    cmd.Parameters.AddWithValue("@permission", activation_permission);
                    cmd.Parameters.AddWithValue("@rollsId", Convert.ToInt32(cmbRolls.SelectedValue));
                    cmd.Parameters.AddWithValue("@createdBy", "admin"); // Replace 'admin' with the actual createdBy value

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Permissions updated successfully", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close(); // Ensure the connection is closed even if an exception occurs
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbRolls.Text = "Select User Roll";//string.Empty;
            //cmbRolls.DropDownStyle = ComboBoxStyle.DropDownList;
            //cmbRolls.Enabled = true;
            if (createUserDataGridView1.DataSource is DataTable dt)
            {
                dt.Rows.Clear();
            }
            else
            {
                createUserDataGridView1.Rows.Clear();
            }
            panel5.Hide();
        }
    }
}

