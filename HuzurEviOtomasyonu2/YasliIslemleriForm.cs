using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace HuzurEviOtomasyonu
{
    public partial class YasliIslemleriForm : Form
    {
        private Panel panelEkleme;
        private Panel panelListeleme;
        private DataGridView dgvYaslilar;
        private TextBox txtTC;
        private TextBox txtAd;
        private TextBox txtSoyad;
        private DateTimePicker dtpDogumTarihi;
        private TextBox txtOdaNo;
        private TextBox txtYakinAdi;
        private TextBox txtYakinTelefon;
        private Button btnKaydet;
        private Button btnTemizle;
        private TextBox txtArama;

        public YasliIslemleriForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Form ayarları
            this.Text = "Yaşlı İşlemleri";
            this.Size = new System.Drawing.Size(1000, 600);

            // Panelleri oluştur
            panelEkleme = new Panel();
            panelListeleme = new Panel();

            // Panel ayarları
            panelEkleme.Dock = DockStyle.Top;
            panelEkleme.Height = 200;
            panelEkleme.Padding = new Padding(10);

            panelListeleme.Dock = DockStyle.Fill;
            panelListeleme.Padding = new Padding(10);

            // Kontrolleri oluştur
            // TC
            Label lblTC = new Label();
            lblTC.Text = "TC:";
            lblTC.Location = new Point(20, 20);
            lblTC.AutoSize = true;

            txtTC = new TextBox();
            txtTC.Location = new Point(120, 20);
            txtTC.Width = 150;

            // Ad
            Label lblAd = new Label();
            lblAd.Text = "Ad:";
            lblAd.Location = new Point(320, 20);
            lblAd.AutoSize = true;

            txtAd = new TextBox();
            txtAd.Location = new Point(420, 20);
            txtAd.Width = 150;

            // Soyad
            Label lblSoyad = new Label();
            lblSoyad.Text = "Soyad:";
            lblSoyad.Location = new Point(620, 20);
            lblSoyad.AutoSize = true;

            txtSoyad = new TextBox();
            txtSoyad.Location = new Point(720, 20);
            txtSoyad.Width = 150;

            // Doğum Tarihi
            Label lblDogumTarihi = new Label();
            lblDogumTarihi.Text = "Doğum Tarihi:";
            lblDogumTarihi.Location = new Point(20, 60);
            lblDogumTarihi.AutoSize = true;

            dtpDogumTarihi = new DateTimePicker();
            dtpDogumTarihi.Location = new Point(120, 60);
            dtpDogumTarihi.Width = 150;

            // Oda No
            Label lblOdaNo = new Label();
            lblOdaNo.Text = "Oda No:";
            lblOdaNo.Location = new Point(320, 60);
            lblOdaNo.AutoSize = true;

            txtOdaNo = new TextBox();
            txtOdaNo.Location = new Point(420, 60);
            txtOdaNo.Width = 150;

            // Yakın Adı
            Label lblYakinAdi = new Label();
            lblYakinAdi.Text = "Yakın Adı:";
            lblYakinAdi.Location = new Point(20, 100);
            lblYakinAdi.AutoSize = true;

            txtYakinAdi = new TextBox();
            txtYakinAdi.Location = new Point(120, 100);
            txtYakinAdi.Width = 150;

            // Yakın Telefon
            Label lblYakinTelefon = new Label();
            lblYakinTelefon.Text = "Yakın Telefon:";
            lblYakinTelefon.Location = new Point(320, 100);
            lblYakinTelefon.AutoSize = true;

            txtYakinTelefon = new TextBox();
            txtYakinTelefon.Location = new Point(420, 100);
            txtYakinTelefon.Width = 150;

            // Butonlar
            btnKaydet = new Button();
            btnKaydet.Text = "Kaydet";
            btnKaydet.Location = new Point(120, 140);
            btnKaydet.Width = 100;

            btnTemizle = new Button();
            btnTemizle.Text = "Temizle";
            btnTemizle.Location = new Point(230, 140);
            btnTemizle.Width = 100;

            // Arama paneli
            Panel panelArama = new Panel();
            panelArama.Dock = DockStyle.Top;
            panelArama.Height = 40;
            txtArama = new TextBox();
            txtArama.Dock = DockStyle.Fill;

            // DataGridView
            dgvYaslilar = new DataGridView();
            dgvYaslilar.Dock = DockStyle.Fill;
            dgvYaslilar.AllowUserToAddRows = false;
            dgvYaslilar.ReadOnly = true;
            dgvYaslilar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvYaslilar.MultiSelect = false;
            dgvYaslilar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Event handlers
            btnKaydet.Click += new EventHandler(btnKaydet_Click);
            btnTemizle.Click += new EventHandler(btnTemizle_Click);
            txtArama.TextChanged += new EventHandler(txtArama_TextChanged);
            dgvYaslilar.CellClick += new DataGridViewCellEventHandler(dgvYaslilar_CellClick);

            // Kontrolleri panellere ekle
            panelEkleme.Controls.AddRange(new Control[] {
                lblTC, txtTC,
                lblAd, txtAd,
                lblSoyad, txtSoyad,
                lblDogumTarihi, dtpDogumTarihi,
                lblOdaNo, txtOdaNo,
                lblYakinAdi, txtYakinAdi,
                lblYakinTelefon, txtYakinTelefon,
                btnKaydet, btnTemizle
            });

            panelArama.Controls.Add(txtArama);
            panelListeleme.Controls.Add(dgvYaslilar);
            panelListeleme.Controls.Add(panelArama);

            // Panelleri forma ekle
            this.Controls.Add(panelListeleme);
            this.Controls.Add(panelEkleme);

            // Form yüklendikten sonra verileri listele
            this.Load += new EventHandler(YasliIslemleriForm_Load);
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTC.Text) || string.IsNullOrWhiteSpace(txtAd.Text) ||
                    string.IsNullOrWhiteSpace(txtSoyad.Text))
                {
                    MessageBox.Show("Lütfen zorunlu alanları doldurunuz!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"INSERT INTO Yaslilar (TC, Ad, Soyad, DogumTarihi, OdaNo, YakinAdi, YakinTelefon, GirisTarihi, Durum) 
                           VALUES (@TC, @Ad, @Soyad, @DogumTarihi, @OdaNo, @YakinAdi, @YakinTelefon, GETDATE(), 1)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TC", txtTC.Text);
                        cmd.Parameters.AddWithValue("@Ad", txtAd.Text);
                        cmd.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
                        cmd.Parameters.AddWithValue("@DogumTarihi", dtpDogumTarihi.Value);
                        cmd.Parameters.AddWithValue("@OdaNo", string.IsNullOrEmpty(txtOdaNo.Text) ? DBNull.Value : (object)int.Parse(txtOdaNo.Text));
                        cmd.Parameters.AddWithValue("@YakinAdi", txtYakinAdi.Text);
                        cmd.Parameters.AddWithValue("@YakinTelefon", txtYakinTelefon.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Yaşlı başarıyla kaydedildi.");
                FormuTemizle();
                YaslilariListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında bir hata oluştu: " + ex.Message);
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            FormuTemizle();
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"SELECT * FROM Yaslilar 
                           WHERE Durum = 1 AND (TC LIKE @Arama + '%' 
                           OR Ad LIKE @Arama + '%' 
                           OR Soyad LIKE @Arama + '%')";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@Arama", txtArama.Text);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvYaslilar.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama sırasında bir hata oluştu: " + ex.Message);
            }
        }

        private void dgvYaslilar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvYaslilar.Rows[e.RowIndex];
                txtTC.Text = row.Cells["TC"].Value.ToString();
                txtAd.Text = row.Cells["Ad"].Value.ToString();
                txtSoyad.Text = row.Cells["Soyad"].Value.ToString();
                dtpDogumTarihi.Value = Convert.ToDateTime(row.Cells["DogumTarihi"].Value);
                txtOdaNo.Text = row.Cells["OdaNo"].Value?.ToString();
                txtYakinAdi.Text = row.Cells["YakinAdi"].Value?.ToString();
                txtYakinTelefon.Text = row.Cells["YakinTelefon"].Value?.ToString();
            }
        }

        private void YaslilariListele()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT * FROM Yaslilar WHERE Durum = 1";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvYaslilar.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yaşlılar listelenirken bir hata oluştu: " + ex.Message);
            }
        }

        private void FormuTemizle()
        {
            txtTC.Clear();
            txtAd.Clear();
            txtSoyad.Clear();
            dtpDogumTarihi.Value = DateTime.Now;
            txtOdaNo.Clear();
            txtYakinAdi.Clear();
            txtYakinTelefon.Clear();
        }

        private void YasliIslemleriForm_Load(object sender, EventArgs e)
        {
            YaslilariListele();
        }

        // Diğer metodlar aynı kalacak...
    }
}