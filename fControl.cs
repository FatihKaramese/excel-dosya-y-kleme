using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace denemeFord
{
    public partial class fControl : Form
    {
        public fControl()
        {
            InitializeComponent();
        }
        DataTable tablo = new DataTable();

        string connectionString = "Host=localhost;Port=5442;Username=postgres;Password=saadet2000;Database=Ford";
        void listele()
        {

            richTextBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            tablo.Clear();

            NpgsqlConnection baglan = new NpgsqlConnection("Host=localhost;Port=5442;Username=postgres;Password=saadet2000;Database=Ford");
            baglan.Open();
            NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("select id,marka,model_yılı, yakıt_türü,vites_türü, motor_hacmi,beygir_gücü from tb_car order by id", baglan);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            adtr.Dispose();
            baglan.Close();

        }
        void sayac()
        {
            NpgsqlConnection baglan = new NpgsqlConnection("Host=localhost;Port=5442;Username=postgres;Password=saadet2000;Database=Ford");
            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("select count(*) from tb_car", baglan);
            NpgsqlDataReader okuyucu = komut.ExecuteReader();
            while (okuyucu.Read())
            {
                label10.Text = okuyucu[0].ToString();
            }
        }
        private void Form17_Load(object sender, EventArgs e)
        {
            listele();
            sayac();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NpgsqlConnection baglan = new NpgsqlConnection(connectionString);
            baglan.Open();
            NpgsqlCommand cmd2 = new NpgsqlCommand();
            cmd2.Connection = baglan;
            cmd2.CommandText = "DELETE FROM tb_car";
            cmd2.ExecuteNonQuery();
            baglan.Close();
            listele();
            sayac();
            MessageBox.Show("Veri Silme İşlemi Tamamlandı", "SİSTEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            NpgsqlConnection baglan = new NpgsqlConnection(connectionString);
            baglan.Open();
            NpgsqlCommand komut1 = new NpgsqlCommand();
            komut1.Connection = baglan;
            komut1.CommandText = "INSERT INTO tb_car(marka,model_yılı, yakıt_türü,vites_türü, motor_hacmi,beygir_gücü)VALUES('" + richTextBox1.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "')";
            komut1.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Veri kayıt altına alındı", "SİSTEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            sayac();
            listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NpgsqlConnection baglan = new NpgsqlConnection(connectionString);
            baglan.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = baglan;
            cmd.CommandText = "UPDATE tb_car Set marka ='" + richTextBox1.Text + "',model_yılı ='" + textBox1.Text + "',yakıt_türü ='" + textBox2.Text + "',vites_türü ='" + textBox3.Text + "',motor_hacmi ='" + textBox4.Text + "',beygir_gücü ='" + textBox5.Text + "' where  id=@id";
            cmd.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells[0].Value.GetHashCode());
            cmd.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Veri Güncelleme İşlemi Tamamlandı", "SİSTEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            sayac();
            listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            richTextBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NpgsqlConnection baglan = new NpgsqlConnection(connectionString);
            baglan.Open();
            NpgsqlCommand cmd2 = new NpgsqlCommand();
            cmd2.Connection = baglan;
            cmd2.CommandText = "DELETE FROM tb_car WHERE id = @id";
            cmd2.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells[0].Value.GetHashCode());
            cmd2.ExecuteNonQuery();
            baglan.Close();
            listele();
            sayac();
            MessageBox.Show("Veri Silme İşlemi Tamamlandı", "SİSTEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fExcel f = new fExcel();
            f.Show();
            this.Hide();
        }
    }
}
