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
    public partial class IlacTakipListesiForm : Form
    {
        private DataGridView dgvIlaclar;
        private Button btnYenile;
        private Button btnSil;
        private TextBox txtArama;
        private Label lblArama;
        private DateTimePicker dtpBaslangic;
        private DateTimePicker dtpBitis;
        private Label lblTarih;
        private CheckBox chkTarihFiltre;

        public IlacTakipListesiForm()
        {
            InitializeComponent();
            IlaclariListele();
        }

        private void InitializeComponent()
        {
            this.Text = "İlaç Takip Listesi";
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

            // Tarih Filtresi CheckBox
            chkTarihFiltre = new CheckBox();
            chkTarihFiltre.Text = "Tarihe Göre Filtrele";
            chkTarihFiltre.Location = new System.Drawing.Point(280, 20);
            chkTarihFiltre.Size = new System.Drawing.Size(120, 20);
            chkTarihFiltre.CheckedChanged += new EventHandler(chkTarihFiltre_CheckedChanged);

            // Tarih Label
            lblTarih = new Label();
            lblTarih.Text = "Tarih Aralığı:";
            lblTarih.Location = new System.Drawing.Point(410, 20);
            lblTarih.Size = new System.Drawing.Size(80, 20);

            // Başlangıç DateTimePicker
            dtpBaslangic = new DateTimePicker();
            dtpBaslangic.Location = new System.Drawing.Point(490, 20);
            dtpBaslangic.Size = new System.Drawing.Size(100, 20);
            dtpBaslangic.Format = DateTimePickerFormat.Short;
            dtpBaslangic.Enabled = false;
            dtpBaslangic.ValueChanged += new EventHandler(dtp_ValueChanged);

            // Bitiş DateTimePicker
            dtpBitis = new DateTimePicker();
            dtpBitis.Location = new System.Drawing.Point(600, 20);
            dtpBitis.Size = new System.Drawing.Size(100, 20);
            dtpBitis.Format = DateTimePickerFormat.Short;
            dtpBitis.Enabled = false;
            dtpBitis.ValueChanged += new EventHandler(dtp_ValueChanged);

            // Yenile Butonu
            btnYenile = new Button();
            btnYenile.Text = "Yenile";
            btnYenile.Location = new System.Drawing.Point(710, 20);
            btnYenile.Size = new System.Drawing.Size(75, 23);
            btnYenile.Click += new EventHandler(btnYenile_Click);

            // Sil Butonu
            btnSil = new Button();
            btnSil.Text = "Sil";
            btnSil.Location = new System.Drawing.Point(790, 20);
            btnSil.Size = new System.Drawing.Size(75, 23);
            btnSil.Click += new EventHandler(btnSil_Click);

            // DataGridView
            dgvIlaclar = new DataGridView();
            dgvIlaclar.Location = new System.Drawing.Point(20, 50);
            dgvIlaclar.Size = new System.Drawing.Size(950, 500);
            dgvIlaclar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIlaclar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIlaclar.MultiSelect = false;
            dgvIlaclar.ReadOnly = true;
            dgvIlaclar.AllowUserToAddRows = false;

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                lblArama, txtArama,
                chkTarihFiltre,
                lblTarih, dtpBaslangic, dtpBitis,
                btnYenile, btnSil,
                dgvIlaclar
            });
        }

        private void IlaclariListele(string aramaMetni = "")
        {
            string query = @"SELECT i.IlacTakipID, y.Ad + ' ' + y.Soyad as YasliAdSoyad,
                           i.IlacAdi, i.Doz, i.KullanımSaati, i.BaslangicTarihi, 
                           i.BitisTarihi, i.Aciklama,
                           p.Ad + ' ' + p.Soyad as PersonelAdSoyad
                           FROM IlacTakip i
                           INNER JOIN Yaslilar y ON i.YasliTC = y.TC
                           LEFT JOIN Personel p ON i.PersonelID = p.PersonelID
                           WHERE 1=1";

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                query += @" AND (y.Ad LIKE @arama OR y.Soyad LIKE @arama OR 
                           i.IlacAdi LIKE @arama)";
            }

            if (chkTarihFiltre.Checked)
            {
                query += @" AND ((i.BaslangicTarihi BETWEEN @baslangic AND @bitis) 
                           OR (i.BitisTarihi BETWEEN @baslangic AND @bitis)
                           OR (i.BaslangicTarihi <= @baslangic AND i.BitisTarihi >= @bitis))";
            }

            query += " ORDER BY i.BaslangicTarihi DESC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(aramaMetni))
                    {
                        cmd.Parameters.AddWithValue("@arama", "%" + aramaMetni + "%");
                    }

                    if (chkTarihFiltre.Checked)
                    {
                        cmd.Parameters.AddWithValue("@baslangic", dtpBaslangic.Value.Date);
                        cmd.Parameters.AddWithValue("@bitis", dtpBitis.Value.Date);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvIlaclar.DataSource = dt;
                }
            }

            // Kolon başlıklarını düzenle
            if (dgvIlaclar.Columns.Count > 0)
            {
                dgvIlaclar.Columns["IlacTakipID"].HeaderText = "Takip No";
                dgvIlaclar.Columns["YasliAdSoyad"].HeaderText = "Yaşlı Adı Soyadı";
                dgvIlaclar.Columns["IlacAdi"].HeaderText = "İlaç Adı";
                dgvIlaclar.Columns["Doz"].HeaderText = "Doz";
                dgvIlaclar.Columns["KullanımSaati"].HeaderText = "Kullanım Saati";
                dgvIlaclar.Columns["BaslangicTarihi"].HeaderText = "Başlangıç Tarihi";
                dgvIlaclar.Columns["BitisTarihi"].HeaderText = "Bitiş Tarihi";
                dgvIlaclar.Columns["Aciklama"].HeaderText = "Açıklama";
                dgvIlaclar.Columns["PersonelAdSoyad"].HeaderText = "Personel";
            }
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            IlaclariListele(txtArama.Text);
        }

        private void chkTarihFiltre_CheckedChanged(object sender, EventArgs e)
        {
            dtpBaslangic.Enabled = chkTarihFiltre.Checked;
            dtpBitis.Enabled = chkTarihFiltre.Checked;
            IlaclariListele(txtArama.Text);
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            if (chkTarihFiltre.Checked)
            {
                IlaclariListele(txtArama.Text);
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            txtArama.Clear();
            chkTarihFiltre.Checked = false;
            dtpBaslangic.Value = DateTime.Now;
            dtpBitis.Value = DateTime.Now;
            IlaclariListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvIlaclar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek kaydı seçin!");
                return;
            }

            int ilacTakipID = Convert.ToInt32(dgvIlaclar.SelectedRows[0].Cells["IlacTakipID"].Value);
            string ilacAdi = dgvIlaclar.SelectedRows[0].Cells["IlacAdi"].Value.ToString();
            string yasliAdi = dgvIlaclar.SelectedRows[0].Cells["YasliAdSoyad"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"{yasliAdi} için {ilacAdi} ilacını silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM IlacTakip WHERE IlacTakipID = @id";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@id", ilacTakipID)
                };

                if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
                {
                    MessageBox.Show("İlaç kaydı başarıyla silindi!");
                    IlaclariListele(txtArama.Text);
                }
            }
        }
    }
}