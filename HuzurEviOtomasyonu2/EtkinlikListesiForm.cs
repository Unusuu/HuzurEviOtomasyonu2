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
    public partial class EtkinlikListesiForm : Form
    {
        private DataGridView dgvEtkinlikler;
        private Button btnYenile;
        private Button btnSil;
        private DateTimePicker dtpBaslangic;
        private DateTimePicker dtpBitis;
        private Label lblTarihAraligi;
        private Button btnAra;
        private ComboBox cmbSorumlu;
        private Label lblSorumlu;

        public EtkinlikListesiForm()
        {
            InitializeComponent();
            SorumluPersonelleriYukle();
            EtkinlikleriListele();
        }

        private void InitializeComponent()
        {
            this.Text = "Etkinlik Listesi";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // DataGridView
            dgvEtkinlikler = new DataGridView();
            dgvEtkinlikler.Dock = DockStyle.Bottom;
            dgvEtkinlikler.Height = 450;
            dgvEtkinlikler.AllowUserToAddRows = false;
            dgvEtkinlikler.ReadOnly = true;
            dgvEtkinlikler.MultiSelect = false;
            dgvEtkinlikler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

            // Sorumlu Personel ComboBox
            lblSorumlu = new Label();
            lblSorumlu.Text = "Sorumlu:";
            lblSorumlu.Location = new System.Drawing.Point(20, 50);
            lblSorumlu.AutoSize = true;

            cmbSorumlu = new ComboBox();
            cmbSorumlu.Location = new System.Drawing.Point(120, 50);
            cmbSorumlu.Width = 200;
            cmbSorumlu.DropDownStyle = ComboBoxStyle.DropDownList;

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
                dgvEtkinlikler, lblTarihAraligi, dtpBaslangic, dtpBitis,
                lblSorumlu, cmbSorumlu, btnAra, btnYenile, btnSil
            });
        }

        private void SorumluPersonelleriYukle()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT PersonelID, Ad + ' ' + Soyad AS AdSoyad FROM Personel ORDER BY Ad, Soyad";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbSorumlu.DisplayMember = "AdSoyad";
                    cmbSorumlu.ValueMember = "PersonelID";
                    cmbSorumlu.DataSource = dt;

                    // Boş seçenek ekle
                    DataRow dr = dt.NewRow();
                    dr["PersonelID"] = DBNull.Value;
                    dr["AdSoyad"] = "Tümü";
                    dt.Rows.InsertAt(dr, 0);
                    cmbSorumlu.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personel listesi yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void EtkinlikleriListele()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"SELECT e.EtkinlikID, e.EtkinlikAdi, e.Tarih, e.Yer, e.Aciklama,
                                   p.Ad + ' ' + p.Soyad AS Sorumlu
                                   FROM Etkinlikler e
                                   LEFT JOIN Personel p ON e.SorumluID = p.PersonelID
                                   ORDER BY e.Tarih DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvEtkinlikler.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Etkinlik listesi yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"SELECT e.EtkinlikID, e.EtkinlikAdi, e.Tarih, e.Yer, e.Aciklama,
                                   p.Ad + ' ' + p.Soyad AS Sorumlu
                                   FROM Etkinlikler e
                                   LEFT JOIN Personel p ON e.SorumluID = p.PersonelID
                                   WHERE e.Tarih BETWEEN @baslangic AND @bitis";

                    if (cmbSorumlu.SelectedIndex > 0)
                    {
                        query += " AND e.SorumluID = @sorumluID";
                    }

                    query += " ORDER BY e.Tarih DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@baslangic", dtpBaslangic.Value.Date);
                    adapter.SelectCommand.Parameters.AddWithValue("@bitis", dtpBitis.Value.Date.AddDays(1).AddSeconds(-1));

                    if (cmbSorumlu.SelectedIndex > 0)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@sorumluID", cmbSorumlu.SelectedValue);
                    }

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvEtkinlikler.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama yapılırken hata oluştu: " + ex.Message);
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            EtkinlikleriListele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvEtkinlikler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek bir kayıt seçin.");
                return;
            }

            if (MessageBox.Show("Seçili etkinliği silmek istediğinize emin misiniz?", "Onay",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvEtkinlikler.SelectedRows[0].Cells["EtkinlikID"].Value);
                    using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                    {
                        conn.Open();
                        string query = "DELETE FROM Etkinlikler WHERE EtkinlikID = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        EtkinlikleriListele();
                        MessageBox.Show("Etkinlik başarıyla silindi.");
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