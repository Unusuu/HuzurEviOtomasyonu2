using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class YemekListeleriGoruntuleForm : Form
    {
        private DataGridView dgvYemekler;
        private Button btnYenile;
        private Button btnSil;
        private DateTimePicker dtpBaslangic;
        private DateTimePicker dtpBitis;
        private Label lblTarihAraligi;
        private Button btnAra;

        public YemekListeleriGoruntuleForm()
        {
            InitializeComponent();
            YemekleriListele();
        }

        private void InitializeComponent()
        {
            this.Text = "Yemek Listeleri";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // DataGridView
            dgvYemekler = new DataGridView();
            dgvYemekler.Dock = DockStyle.Bottom;
            dgvYemekler.Height = 450;
            dgvYemekler.AllowUserToAddRows = false;
            dgvYemekler.ReadOnly = true;
            dgvYemekler.MultiSelect = false;
            dgvYemekler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Tarih Seçiciler
            lblTarihAraligi = new Label();
            lblTarihAraligi.Text = "Tarih Aralığı:";
            lblTarihAraligi.Location = new System.Drawing.Point(20, 20);
            lblTarihAraligi.AutoSize = true;

            dtpBaslangic = new DateTimePicker();
            dtpBaslangic.Location = new System.Drawing.Point(120, 20);
            dtpBaslangic.Format = DateTimePickerFormat.Short;

            dtpBitis = new DateTimePicker();
            dtpBitis.Location = new System.Drawing.Point(250, 20);
            dtpBitis.Format = DateTimePickerFormat.Short;

            // Butonlar
            btnAra = new Button();
            btnAra.Text = "Ara";
            btnAra.Location = new System.Drawing.Point(380, 20);
            btnAra.Click += new EventHandler(btnAra_Click);

            btnYenile = new Button();
            btnYenile.Text = "Yenile";
            btnYenile.Location = new System.Drawing.Point(480, 20);
            btnYenile.Click += new EventHandler(btnYenile_Click);

            btnSil = new Button();
            btnSil.Text = "Sil";
            btnSil.Location = new System.Drawing.Point(580, 20);
            btnSil.Click += new EventHandler(btnSil_Click);

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                dgvYemekler, lblTarihAraligi, dtpBaslangic, dtpBitis,
                btnAra, btnYenile, btnSil
            });
        }

        private void YemekleriListele()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT * FROM YemekListesi ORDER BY Tarih DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvYemekler.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yemek listesi yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT * FROM YemekListesi WHERE Tarih BETWEEN @baslangic AND @bitis ORDER BY Tarih DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@baslangic", dtpBaslangic.Value.Date);
                    adapter.SelectCommand.Parameters.AddWithValue("@bitis", dtpBitis.Value.Date.AddDays(1).AddSeconds(-1));
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvYemekler.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama yapılırken hata oluştu: " + ex.Message);
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            YemekleriListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvYemekler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek bir kayıt seçin.");
                return;
            }

            if (MessageBox.Show("Seçili kaydı silmek istediğinize emin misiniz?", "Onay",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvYemekler.SelectedRows[0].Cells["ID"].Value);
                    using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                    {
                        conn.Open();
                        string query = "DELETE FROM YemekListesi WHERE ID = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        YemekleriListele();
                        MessageBox.Show("Kayıt başarıyla silindi.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message);
                }
            }
        }
    }
}