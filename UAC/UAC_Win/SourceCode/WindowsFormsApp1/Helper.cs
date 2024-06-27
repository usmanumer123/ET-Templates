using WindowsFormsApp1;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{

    public class Helper
    {
        public string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.ToString();

        SqlConnection cn;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        UACEntities context = new UACEntities();
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

        public DataSet GetDataset2(String queryString, String ConnectionString, String TableName)
        {
            DataSet ds = new DataSet();
            cn = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    da = new SqlDataAdapter(command);
                    da.Fill(ds, TableName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Critical Error");
                }
            }
            return ds;
        }
        public void PopulateCombo(ComboBox comboBox, String StrSql, String displayMember, String valueMember)
        {
            DataSet ds = new DataSet();
            cn = new SqlConnection(connectionString);
            try
            {
                ds = GetDataset2(StrSql, cn.ConnectionString.ToString(), "dataTable");

                if (ds.Tables["dataTable"].Rows.Count > 0)
                {
                    comboBox.DataSource = ds.Tables["dataTable"];
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                    comboBox.SelectedIndex = -1;
                }
                else
                {
                    comboBox.DataSource = null;
                }
            }
            catch
            {
            }
        }
        public string ExecuteScalar(String strQuery)
        {
            SqlConnection objConn = new SqlConnection();
            SqlCommand objCmd = new SqlCommand();
            SqlTransaction objTrans = null;

            try
            {
                objConn.ConnectionString = connectionString;
                objConn.Open();

                objTrans = objConn.BeginTransaction();
                objCmd.Connection = objConn;
                objCmd.Transaction = objTrans;
                objCmd.CommandText = strQuery;
                objCmd.ExecuteScalar();

                objTrans.Commit();

                return Convert.ToString(objCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                throw new Exception("In ExecuteScalar: ", ex);
            }
            finally
            {
                objConn.Close();
                objCmd.Dispose();
                objConn.Dispose();
            }
        }

        public string ExecuteNonQuery(String strQuery)
        {
            SqlConnection objConn = new SqlConnection();
            SqlCommand objCmd = new SqlCommand();
            SqlTransaction objTrans = null;

            try
            {
                objConn.ConnectionString = connectionString;
                objConn.Open();

                objTrans = objConn.BeginTransaction();
                objCmd.Connection = objConn;
                objCmd.Transaction = objTrans;
                objCmd.CommandText = strQuery;
                objCmd.ExecuteNonQuery();

                objTrans.Commit();

                return Convert.ToString(objCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                throw new Exception("In ExecuteScalar: ", ex);
            }
            finally
            {
                objConn.Close();
                objCmd.Dispose();
                objConn.Dispose();
            }
        }


    }
}
