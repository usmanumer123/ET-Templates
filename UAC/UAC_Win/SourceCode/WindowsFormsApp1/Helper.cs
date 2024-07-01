using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Helper
    {
        public string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.ToString();
        SqlConnection cn;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        
        public void ErrorMessage(String Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button3);
        }

        public void InfoMessage(String Message)
        {
            MessageBox.Show(Message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3);
        }
        
        public void WarningMessage(String Message)
        {
            MessageBox.Show(Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
            
        }
        
        public DataSet GetDataset(String Query)
        {
            DataSet ds = new DataSet();

            try
            {
                cn = new SqlConnection(connectionString);
                cn.Open();
                cmd = new SqlCommand(Query, cn);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.ToString());
            }
            finally
            {
                cn.Close();
            }

            return ds;
        }
        
        public bool PostDataset(String Query)
        {
            try
            {
                cn = new SqlConnection(connectionString);
                cn.Open();
                cmd = new SqlCommand(Query, cn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.ToString());
                return false;
            }
            finally
            {
                cn.Close();
            }
        }
        
        public void PopulateCombo(ComboBox comboBox, String StrSql, String displayMember, String valueMember)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = StrSql;
                ds = Shared.hp.GetDataset(query);
                if (ds != null && ds.Tables.Count > 0)
                {
                    comboBox.DataSource = ds.Tables[0];
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                    comboBox.SelectedIndex = -1;
                }
                else
                {
                    comboBox.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message + "Critical Error");
            }
        }
    }
}
