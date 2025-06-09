using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class YasliListesiForm : Form
    {
        private DataGridView dgvYaslilar;
        private Button btnYenile;
        private Button btnSil;
        private TextBox txtArama;
        private Label lblArama;

        public YasliListesiForm()
        {
            InitializeComponent();
            YaslilariListele();
        }

        private void InitializeComponent()
        {
            this.Text = "Yaşlı Listesi";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Arama Label
            lblArama = new Label();
            lblArama.Text = "Ara:";
            lblArama.Location = new System.Drawing.Point(20, 20);
            lblArama.Size = new System.Drawing.Size(50, 20);

            // Arama TextBox
            txtArama = new TextBox();
            txtArama.Location = new System.Drawing.Point(70, 20);
            txtArama.Size = new System.Drawing.Size(200, 20);
            txtArama.TextChanged += new EventHandler(txtArama_TextChanged);

            // Yenile Butonu
            btnYenile = new Button();
            btnYenile.Text = "Yenile";
            btnYenile.Location = new System.Drawing.Point(280, 20);
            btnYenile.Size = new System.Drawing.Size(75, 23);
            btnYenile.Click += new EventHandler(btnYenile_Click);

            // Sil Butonu
            btnSil = new Button();
            btnSil.Text = "Sil";
            btnSil.Location = new System.Drawing.Point(360, 20);
            btnSil.Size = new System.Drawing.Size(75, 23);
            btnSil.Click += new EventHandler(btnSil_Click);

            // DataGridView
            dgvYaslilar = new DataGridView();
            dgvYaslilar.Location = new System.Drawing.Point(20, 50);
            dgvYaslilar.Size = new System.Drawing.Size(750, 500);
            dgvYaslilar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvYaslilar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvYaslilar.MultiSelect = false;
            dgvYaslilar.ReadOnly = true;
            dgvYaslilar.AllowUserToAddRows = false;

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                lblArama, txtArama, btnYenile, btnSil, dgvYaslilar
            });
        }

        private void YaslilariListele(string aramaMetni = "")
        {
            string query = @"SELECT y.TC, y.Ad, y.Soyad, y.DogumTarihi, y.KanGrubu, 
                           y.Cinsiyet, y.TelefonNo, y.GirisTarihi, y.OdaNo, 
                           y.YakinAdi, y.YakinTelefon
                           FROM Yaslilar y
                           WHERE y.Durum = 1";

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                query += @" AND (y.TC LIKE @arama OR y.Ad LIKE @arama OR 
                           y.Soyad LIKE @arama OR y.YakinAdi LIKE @arama)";
            }

            query += " ORDER BY y.Ad, y.Soyad";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(aramaMetni))
                    {
                        cmd.Parameters.AddWithValue("@arama", "%" + aramaMetni + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvYaslilar.DataSource = dt;
                }
            }

            // Kolon başlıklarını düzenle
            if (dgvYaslilar.Columns.Count > 0)
            {
                dgvYaslilar.Columns["TC"].HeaderText = "TC Kimlik No";
                dgvYaslilar.Columns["Ad"].HeaderText = "Ad";
                dgvYaslilar.Columns["Soyad"].HeaderText = "Soyad";
                dgvYaslilar.Columns["DogumTarihi"].HeaderText = "Doğum Tarihi";
                dgvYaslilar.Columns["KanGrubu"].HeaderText = "Kan Grubu";
                dgvYaslilar.Columns["Cinsiyet"].HeaderText = "Cinsiyet";
                dgvYaslilar.Columns["TelefonNo"].HeaderText = "Telefon";
                dgvYaslilar.Columns["GirisTarihi"].HeaderText = "Giriş Tarihi";
                dgvYaslilar.Columns["OdaNo"].HeaderText = "Oda No";
                dgvYaslilar.Columns["YakinAdi"].HeaderText = "Yakın Adı";
                dgvYaslilar.Columns["YakinTelefon"].HeaderText = "Yakın Telefonu";
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            YaslilariListele(txtArama.Text);
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            txtArama.Clear();
            YaslilariListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvYaslilar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek yaşlıyı seçin!");
                return;
            }

            string tc = dgvYaslilar.SelectedRows[0].Cells["TC"].Value.ToString();
            string ad = dgvYaslilar.SelectedRows[0].Cells["Ad"].Value.ToString();
            string soyad = dgvYaslilar.SelectedRows[0].Cells["Soyad"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"{ad} {soyad} isimli yaşlıyı silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "UPDATE Yaslilar SET Durum = 0 WHERE TC = @tc";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@tc", tc)
                };

                if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
                {
                    MessageBox.Show("Yaşlı başarıyla silindi!");
                    YaslilariListele(txtArama.Text);
                }
            }
        }
    }
}