using Microsoft.Reporting.WinForms;
using QRCoder;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        SqlConnection connection = new SqlConnection("Data Source=HP\\HASSAN;Initial Catalog=WMS;Persist Security Info=True;User ID=sa;Password=123;Encrypt=True;TrustServerCertificate=True");

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM UserProfile", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            // Add a new column for the QR code image
            dt.Columns.Add("QRCodeImage", typeof(byte[]));

            foreach (DataRow row in dt.Rows)
            {
                string userId = row["UserId"].ToString();
                string userName = row["UserName"].ToString();
                string qrCodeText = $"{userId}";//-{userName}
                row["QRCodeImage"] = GenerateQRCode(qrCodeText);
            }

            ReportDataSource source = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportPath = "Report2.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();
        }

        private byte[] GenerateQRCode(string qrCodeText)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        using (Bitmap qrCodeImage = qrCode.GetGraphic(7))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                qrCodeImage.Save(ms, ImageFormat.Png);
                                return ms.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
}
