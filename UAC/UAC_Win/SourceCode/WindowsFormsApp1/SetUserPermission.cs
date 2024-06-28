using Bunifu.Framework.UI;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class SetUserPermission : Form
    {
        Helper hp = new Helper();
        private BunifuCustomDataGrid createUserDataGridView1;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString);
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
            hp.PopulateCombo(cmbRolls, query, "RollsDesc", "RollsId");
            this.cmbRolls.SelectedIndexChanged += new System.EventHandler(this.cmbRolls_SelectedIndexChanged);
            //panel5.Hide();
            // Load data with unchecked permissions initially
            //LoadDataWithUncheckedPermissions();
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
                SqlCommand cmd = new SqlCommand("SELECT Module, Permission, IsEnable FROM UserRollsPermission WHERE RollsId = @rollsId", con);
                cmd.Parameters.AddWithValue("@rollsId", rollsId);
                permissionsTable = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                permissionsTable.Load(sdr);
                con.Close();

                // Check if there are no rows for the given RollsId
                if (permissionsTable.Rows.Count == 0 && _rollsId != -1)
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
                            flag = hp.PostDataset(query);
                        }
                    }
                    if (flag)
                    {
                        hp.InfoMessage("Permissions updated successfully");
                    }
                }
                else
                {
                    hp.InfoMessage("Select user roll.");
                }
            }
            catch (Exception ex)
            {
                hp.ErrorMessage("btnDone_Click: " + ex.Message.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbRolls.Text = "";//string.Empty;
            //cmbRolls.DropDownStyle = ComboBoxStyle.DropDownList;
            //cmbRolls.Enabled = true;
            if (createUserDataGridView1.DataSource is DataTable dt)
            {
                //dt.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    row["Activation"] = false;
                }
            }
            //else
            //{
            //    createUserDataGridView1.Rows.Clear();
            //}
            //panel5.Hide();
        }
    }
}
