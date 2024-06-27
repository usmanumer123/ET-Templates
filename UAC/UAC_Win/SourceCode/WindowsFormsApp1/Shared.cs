using Bunifu.Framework.UI;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Shared
    {
        public static int UserId;
        public static string Username;
        public static string Password;
        public static int RoleDesc;
        public static int RollsId;
        public static int CreatedBy;

        public static string EncryptPassword(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = ProtectedData.Protect(plainTextBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptPassword(string encryptedText)
        {
            // Check if the string is a valid Base-64 encoded string
            if (IsBase64String(encryptedText))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            else
            {
                // If the string is not Base-64 encoded, assume it is plain text
                return encryptedText;
            }
        }

        public static bool IsBase64String(string base64)
        {
            // Helper method to check if a string is a valid Base-64 encoded string
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0
                || base64.Contains(" ") || base64.Contains("\t") || base64.Contains("\r") || base64.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void InitializeBunifuDataGridView(BunifuCustomDataGrid dataGridView)
        {
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersHeight = 40;
            dataGridView.RowTemplate.Height = 40; 
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToOrderColumns = true;

            // Set default styles for the grid
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dataGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(5); // Add padding to increase header size

            dataGridView.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dataGridView.RowsDefaultCellStyle.Padding = new Padding(5); // Add padding to increase row size
            dataGridView.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(73, 102, 138);
            dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            //// Adjust the width of all columns
            //foreach (DataGridViewColumn column in dataGridView.Columns)
            //{
            //    column.Width = 150;
            //}
            dataGridView.Hide();
        }

        public static void AddColumnsToDataGridView(string[] columnNames, BunifuCustomDataGrid dataGridView)
        {
            foreach (string str in columnNames)
            {
                var Column = new DataGridViewTextBoxColumn { Name = str, DataPropertyName = str, HeaderText = str, ReadOnly = true };
                dataGridView.Columns.Add(Column);
            }
        }

        public static void SetButtonState(Button button, bool isEnabled)
        {
            button.Enabled = isEnabled;
            //button.BackColor = isEnabled ? Color.White : Color.LightGray;
            //button.ForeColor = isEnabled ? SystemColors.ControlText : Color.Gray;
            if (isEnabled == false)
            {
                button.BackColor = Color.LightGray;
                button.ForeColor = Color.Gray;
            }
        }
    }
}
