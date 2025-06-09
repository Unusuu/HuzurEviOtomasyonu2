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
    public partial class PersonelListesiForm : Form
    {
        private DataGridView dgvPersonel;
        private Button btnYenile;
        private Button btnSil;
        private TextBox txtArama;
        private Label lblArama;
        private ComboBox cmbPozisyon;
        private Label lblPozisyon;

        public PersonelListesiForm()
        {
            InitializeComponent();
            PozisyonlariYukle();
            PersoneliListele();
        }

        private void InitializeComponent()
        {
            this.Text = "Personel Listesi";
            this.Size = new System.Drawing.Size(1000, 600);
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

            // Pozisyon Label
            lblPozisyon = new Label();
            lblPozisyon.Text = "Pozisyon:";
            lblPozisyon.Location = new System.Drawing.Point(280, 20);
            lblPozisyon.Size = new System.Drawing.Size(60, 20);

            // Pozisyon ComboBox
            cmbPozisyon = new ComboBox();
            cmbPozisyon.Location = new System.Drawing.Point(340, 20);
            cmbPozisyon.Size = new System.Drawing.Size(150, 20);
            cmbPozisyon.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPozisyon.SelectedIndexChanged += new EventHandler(cmbPozisyon_SelectedIndexChanged);

            // Yenile Butonu
            btnYenile = new Button();
            btnYenile.Text = "Yenile";
            btnYenile.Location = new System.Drawing.Point(500, 20);
            btnYenile.Size = new System.Drawing.Size(75, 23);
            btnYenile.Click += new EventHandler(btnYenile_Click);

            // Sil Butonu
            btnSil = new Button();
            btnSil.Text = "Sil";
            btnSil.Location = new System.Drawing.Point(580, 20);
            btnSil.Size = new System.Drawing.Size(75, 23);
            btnSil.Click += new EventHandler(btnSil_Click);

            // DataGridView
            dgvPersonel = new DataGridView();
            dgvPersonel.Location = new System.Drawing.Point(20, 50);
            dgvPersonel.Size = new System.Drawing.Size(950, 500);
            dgvPersonel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersonel.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersonel.MultiSelect = false;
            dgvPersonel.ReadOnly = true;
            dgvPersonel.AllowUserToAddRows = false;

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                lblArama, txtArama,
                lblPozisyon, cmbPozisyon,
                btnYenile, btnSil,
                dgvPersonel
            });
        }

        private void PozisyonlariYukle()
        {
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Tüm Pozisyonlar", Value = "" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Yönetici", Value = "Yönetici" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Doktor", Value = "Doktor" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Hemşire", Value = "Hemşire" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Bakıcı", Value = "Bakıcı" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Temizlik Görevlisi", Value = "Temizlik Görevlisi" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Aşçı", Value = "Aşçı" });
            cmbPozisyon.Items.Add(new ComboBoxItem { Text = "Güvenlik", Value = "Güvenlik" });
            cmbPozisyon.SelectedIndex = 0;
        }

        private void PersoneliListele(string aramaMetni = "", string pozisyon = "")
        {
            string query = @"SELECT p.TC, p.Ad, p.Soyad, p.DogumTarihi, p.Cinsiyet, 
                           p.TelefonNo, p.Email, p.Adres, p.Pozisyon, p.IseBaslamaTarihi, 
                           p.Maas
                           FROM Personel p
                           WHERE p.Durum = 1";

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                query += @" AND (p.TC LIKE @arama OR p.Ad LIKE @arama OR 
                           p.Soyad LIKE @arama OR p.Email LIKE @arama)";
            }

            if (!string.IsNullOrEmpty(pozisyon))
            {
                query += " AND p.Pozisyon = @pozisyon";
            }

            query += " ORDER BY p.Ad, p.Soyad";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(aramaMetni))
                    {
                        cmd.Parameters.AddWithValue("@arama", "%" + aramaMetni + "%");
                    }
                    if (!string.IsNullOrEmpty(pozisyon))
                    {
                        cmd.Parameters.AddWithValue("@pozisyon", pozisyon);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvPersonel.DataSource = dt;
                }
            }

            // Kolon başlıklarını düzenle
            if (dgvPersonel.Columns.Count > 0)
            {
                dgvPersonel.Columns["TC"].HeaderText = "TC Kimlik No";
                dgvPersonel.Columns["Ad"].HeaderText = "Ad";
                dgvPersonel.Columns["Soyad"].HeaderText = "Soyad";
                dgvPersonel.Columns["DogumTarihi"].HeaderText = "Doğum Tarihi";
                dgvPersonel.Columns["Cinsiyet"].HeaderText = "Cinsiyet";
                dgvPersonel.Columns["TelefonNo"].HeaderText = "Telefon";
                dgvPersonel.Columns["Email"].HeaderText = "E-posta";
                dgvPersonel.Columns["Adres"].HeaderText = "Adres";
                dgvPersonel.Columns["Pozisyon"].HeaderText = "Pozisyon";
                dgvPersonel.Columns["IseBaslamaTarihi"].HeaderText = "İşe Başlama Tarihi";
                dgvPersonel.Columns["Maas"].HeaderText = "Maaş";
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            string pozisyon = ((ComboBoxItem)cmbPozisyon.SelectedItem).Value;
            PersoneliListele(txtArama.Text, pozisyon);
        }

        private void cmbPozisyon_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pozisyon = ((ComboBoxItem)cmbPozisyon.SelectedItem).Value;
            PersoneliListele(txtArama.Text, pozisyon);
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            txtArama.Clear();
            cmbPozisyon.SelectedIndex = 0;
            PersoneliListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvPersonel.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek personeli seçin!");
                return;
            }

            string tc = dgvPersonel.SelectedRows[0].Cells["TC"].Value.ToString();
            string ad = dgvPersonel.SelectedRows[0].Cells["Ad"].Value.ToString();
            string soyad = dgvPersonel.SelectedRows[0].Cells["Soyad"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"{ad} {soyad} isimli personeli silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "UPDATE Personel SET Durum = 0 WHERE TC = @tc";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@tc", tc)
                };

                if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
                {
                    MessageBox.Show("Personel başarıyla silindi!");
                    string pozisyon = ((ComboBoxItem)cmbPozisyon.SelectedItem).Value;
                    PersoneliListele(txtArama.Text, pozisyon);
                }
            }
        }
    }
}