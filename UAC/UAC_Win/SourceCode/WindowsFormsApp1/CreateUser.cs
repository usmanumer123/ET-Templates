using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Linq;



namespace WindowsFormsApp1
{
    public partial class CreateUser : Form
    {
        CFL_CV_PracticeEntities1 context = new CFL_CV_PracticeEntities1();
        Helper hp = new Helper();
        string connectionString = "Data Source=HU\\MSSQLSERVER2019;Initial Catalog=CFL_CV_Practice;User ID=sa;Password=123;Encrypt=False";

        
        public CreateUser()
        {

            
            InitializeComponent();
            //Permissions getting in the basis of roleid
            var isAdmin = IsAdmin(Shared.RollsId);
            SetButtonState(btnInsertUser, isAdmin && CheckUserPermission(Shared.RollsId, "Create User", "create"));

            SetButtonState(btnUpdateUser, isAdmin && CheckUserPermission(Shared.RollsId, "Create User", "edit"));
            SetButtonState(btnDeleteUser, isAdmin && CheckUserPermission(Shared.RollsId, "Create User", "delete"));
            
           // SetButtonState(createUserDataGridView1, isAdmin && CheckUserPermission(rol, "CreateUser", "view"));
            // btnUserPermission.Enabled = isAdmin && CheckUserPermission(Shared.RollsId, "CreateUser", "view");


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


        SqlConnection con = new SqlConnection("Data Source=HU\\MSSQLSERVER2019;Initial Catalog=CFL_CV_Practice;User ID=sa;Password=123;Encrypt=False");
        public int UserId;


        private void CreateUser_Load(object sender, EventArgs e)
        {
            txtCreatedBy.Text = Shared.RoleDesc.ToString();
            PopulateComboBox();
            GetUserRecord();
        }

        private void GetUserRecord()
        {
           
            SqlCommand cmd = new SqlCommand("select * from UserProfile ", con);
            DataTable dt = new DataTable();
                con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            createUserDataGridView1.DataSource = dt;

        }
        private void btnInsertUser_Click(object sender, EventArgs e)
        {
            int selectedRollsId = Convert.ToInt32(comboBox1.SelectedValue);
            if (IsValid())
            {
                if (UserId == 0) // Ensure UserId is 0, which means it's a new user
                {
                    SqlCommand cmd = new SqlCommand("Insert into UserProfile Values (@name,@created,@rolls,@password,@isenable)", con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@created", Shared.UserId);
                    cmd.Parameters.AddWithValue("@rolls", selectedRollsId);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@isenable", enableCheckbox.Checked);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("New User is Successfully Added", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetUserRecord();
                    ResetUserControls();
                }
                else
                {
                    MessageBox.Show("Please use the Update button to modify existing users.", "Operation not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        
        private bool IsValid()
        {
            if (txtUserName.Text == string.Empty)
            {
                MessageBox.Show("Fill All Your Fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
           // txtCreatedBy.Text = "";
            txtRollsId.Text = "";
            txtPassword.Text = "";
            enableCheckbox.Checked = false;
       

            txtUserName.Focus();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            try {
                if (IsValid())
                {
                    int selectedRollsId = Convert.ToInt32(comboBox1.SelectedValue);
                    if (UserId > 0)
                    {

                        SqlCommand cmd = new SqlCommand("UPDATE UserProfile SET UserName=@name ,CreatedBy=@created,RollsID=@rolls,Password=@password,IsEnabled=@isenable WHERE UserId=@Id", con);

                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@name", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@created", txtCreatedBy.Text);
                        cmd.Parameters.AddWithValue("@rolls", selectedRollsId);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@isenable", enableCheckbox.Checked);
                        cmd.Parameters.AddWithValue("@Id", this.UserId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("User Information Successfully Updated", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GetUserRecord();
                        ResetUserControls();
                    }

                    else
                    {
                        MessageBox.Show("Please Select User to Update his Information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
              
                   
            }
            catch (Exception ex)
            {
                hp.ErrorMessage(ex.Message.ToString());
            }

        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (UserId > 0)
            {

                SqlCommand cmd = new SqlCommand("DELETE FROM UserProfile WHERE UserId=@Id", con);
                cmd.CommandType = CommandType.Text;
               
                cmd.Parameters.AddWithValue("@Id", this.UserId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("User Information Successfully Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetUserRecord();
                ResetUserControls();

            }

            else
            {
                MessageBox.Show("Please Select User to Delete his Information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtRollsId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCreatedBy_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIsEnable_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void createUserDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //    UserId = Convert.ToInt32(createUserDataGridView.SelectedRows[0].Cells[0].Value);
            //    txtUserName.Text = createUserDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            //    txtCreatedBy.Text = createUserDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            //    txtRollsId.Text = createUserDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            //    txtPassword.Text = createUserDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            //    txtIsEnable.Text = createUserDataGridView.SelectedRows[0].Cells[5].Value.ToString();
            
        }

        private void createUserDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (createUserDataGridView1.SelectedRows.Count > 0)
            {
                UserId = Convert.ToInt32(createUserDataGridView1.SelectedRows[0].Cells[0].Value);
                txtUserName.Text = createUserDataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtCreatedBy.Text = createUserDataGridView1.SelectedRows[0].Cells[2].Value.ToString();
               // txtRollsId.Text = createUserDataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                   comboBox1.SelectedValue= createUserDataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtPassword.Text = createUserDataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                enableCheckbox.Checked = bool.Parse(createUserDataGridView1.SelectedRows[0].Cells[5].Value.ToString());
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // PopulateComboBox(); 
           
        }

        //private void PopulateComboBox()
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string query = "SELECT RollsId, RollsDesc FROM UserRolls";
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            connection.Open();
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    int rollsId = reader.GetInt32(0);
        //                    string rollsDesc = reader.GetString(1);
        //                    comboBox1.Items.Add(new ComboboxItem { Text = rollsDesc, Value = rollsId });
        //                }
        //            }
        //        }
        //    }
        //}

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


        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
