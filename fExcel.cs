using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ExcelDataReader;
using Npgsql;
using Excel = Microsoft.Office.Interop.Excel;

namespace denemeFord
{
    public partial class fExcel : Form
    {
        public fExcel()
        {
            InitializeComponent();
        }
        DataTable tablo = new DataTable();
        string connectionString = "Host=localhost;Port=5442;Username=postgres;Password=saadet2000;Database=Ford";
        void denetim()
        {
            try
            {
                DataTable dt = tableCollection[comboBox1.SelectedItem.ToString()];
                if (dt != null)
                {
                    List<arabalar> sorun = new List<arabalar>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        arabalar araba = new arabalar();
                        araba.marka = dt.Rows[i]["marka"].ToString();
                        araba.model_yılı = dt.Rows[i]["model_yılı"].GetHashCode();
                        araba.yakıt_türü = dt.Rows[i]["yakıt_türü"].ToString();
                        araba.vites_türü = dt.Rows[i]["vites_türü"].ToString();
                        araba.motor_hacmi = dt.Rows[i]["motor_hacmi"].ToString();
                        araba.beygir_gücü = dt.Rows[i]["beygir_gücü"].GetHashCode();

                        sorun.Add(araba);
                    }
                    soruBindingSource.DataSource = sorun;
                }
            }
            catch (Exception)
            {
                label2.Text = "i";
                button3.Visible = false;
                MessageBox.Show("Arabalar listesi haricinde başka liste kullanılamaz,lütfen soru listesi yükleyiniz", "SİSTEM", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        DataTableCollection tableCollection;
        private void button1_Click(object sender, EventArgs e)
        {
            using (var o = new OpenFileDialog() { Filter = "Excel 2007 Workbook|*.xlsx|Excel Workbook|*.xls" })
            {
                if (o.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = o.FileName;
                    using (var stream = File.Open(o.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }

                            });
                            tableCollection = result.Tables;
                            comboBox1.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                comboBox1.Items.Add(table.TableName);
                        }
                    }
                }

            }
        }

        private void fExcel_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[comboBox1.SelectedItem.ToString()];
            denetim();
            if (label2.Text == "i")
            {
                label2.Text = "2";
            }
            else
            {
                dataGridView1.DataSource = dt;

                string b = dataGridView1.Rows[0].Cells[1].Value.ToString();
                string c = dataGridView1.Rows[0].Cells[2].Value.ToString();
                string d = dataGridView1.Rows[0].Cells[3].Value.ToString();
                string q = dataGridView1.Rows[0].Cells[4].Value.ToString();
                string f = dataGridView1.Rows[0].Cells[5].Value.ToString();
                string g = dataGridView1.Rows[0].Cells[0].Value.ToString();

                if (b == "" || c == "" || d == "" || q == "" || f == "" || g == "" )
                {
                    MessageBox.Show("Lütfen boş sütunları doldurup tekrar deneyiniz");
                    label3.Visible = true;
                    label3.Text = "Lütfen hataları düzeltip tekrar deneyin";
                }
                else
                {
                    button4.Visible = true;
                    label3.Text = " işlemi onaylayabilirsiniz";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                OleDbConnection baglanexcel = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + textBox1.Text + "; Extended Properties ='Excel 12.0 Xml;HDR=Yes' ");
                baglanexcel.Open();
                int kayıtsayısı = 0;
                OleDbCommand komut = new OleDbCommand("Select * from[" + comboBox1.Text + "$]", baglanexcel);
                OleDbDataReader okuyucu = komut.ExecuteReader();
                NpgsqlConnection baglan2 = new NpgsqlConnection(connectionString);
                baglan2.Open();
                while (okuyucu.Read())
                {


                    NpgsqlConnection baglan = new NpgsqlConnection(connectionString);
                    baglan.Open();
                    NpgsqlCommand komut2 = new NpgsqlCommand(" INSERT INTO tb_car(marka,model_yılı, yakıt_türü,vites_türü, motor_hacmi,beygir_gücü)VALUES('" + okuyucu["marka"].ToString() + "','" + okuyucu["model_yılı"].ToString() + "','" + okuyucu["yakıt_türü"].ToString() + "','" + okuyucu["vites_türü"].ToString() + "','" + okuyucu["motor_hacmi"].ToString() + "','" + okuyucu["beygir_gücü"].ToString() + "')", baglan);
                    komut2.ExecuteNonQuery();
                    kayıtsayısı++;
                    baglan.Close();

                }
                baglanexcel.Close();
                baglan2.Close();
                MessageBox.Show("toplam: " + kayıtsayısı + "tane kayıt başarılı şekilde kayıt edildi.", "SİSTEM", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (OleDbException HATA)
            {
                MessageBox.Show("hata kodu : " + HATA.ErrorCode.ToString() + " hata mesajı : " + HATA.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fControl f = new fControl();
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sutun = 1;
            int satir = 1;
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Workbooks.Add();
            ExcelApp.Visible = true;
            ExcelApp.Worksheets[1].Activate();
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                ExcelApp.Cells[satir, sutun + j].Value = dataGridView1.Columns[j].HeaderText;

            }
            satir++;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    ExcelApp.Cells[satir + i, sutun + j].Value = dataGridView1[j, i].Value;
                }
            }
        }
    }
}
