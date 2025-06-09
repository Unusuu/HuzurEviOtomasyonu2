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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Form elemanlarını oluşturalım
        public static int YetkiSeviyesi { get; private set; }
        public static string KullaniciAdi { get; private set; }
        private TextBox txtKullaniciAdi;
        private TextBox txtSifre;
        private Button btnGiris;
        private Label lblKullaniciAdi;
        private Label lblSifre;
        private PictureBox pictureBox1;

        private void InitializeComponent()
        {
            this.Text = "Huzurevi Otomasyonu - Giriş";
            this.Size = new System.Drawing.Size(400, 300);

            // PictureBox için logo
            pictureBox1 = new PictureBox();
            pictureBox1.Size = new System.Drawing.Size(100, 100);
            pictureBox1.Location = new System.Drawing.Point(150, 20);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            // Logo eklenecek: pictureBox1.Image = Properties.Resources.logo;

            // Kullanıcı Adı Label ve TextBox
            lblKullaniciAdi = new Label();
            lblKullaniciAdi.Text = "Kullanıcı Adı:";
            lblKullaniciAdi.Location = new System.Drawing.Point(50, 140);
            lblKullaniciAdi.Size = new System.Drawing.Size(100, 20);

            txtKullaniciAdi = new TextBox();
            txtKullaniciAdi.Location = new System.Drawing.Point(150, 140);
            txtKullaniciAdi.Size = new System.Drawing.Size(180, 20);

            // Şifre Label ve TextBox
            lblSifre = new Label();
            lblSifre.Text = "Şifre:";
            lblSifre.Location = new System.Drawing.Point(50, 170);
            lblSifre.Size = new System.Drawing.Size(100, 20);

            txtSifre = new TextBox();
            txtSifre.Location = new System.Drawing.Point(150, 170);
            txtSifre.Size = new System.Drawing.Size(180, 20);
            txtSifre.PasswordChar = '•';

            // Giriş Butonu
            btnGiris = new Button();
            btnGiris.Text = "Giriş Yap";
            btnGiris.Location = new System.Drawing.Point(150, 210);
            btnGiris.Size = new System.Drawing.Size(100, 30);
            btnGiris.Click += new EventHandler(btnGiris_Click);

            // Kontrolleri forma ekleyelim
            this.Controls.Add(pictureBox1);
            this.Controls.Add(lblKullaniciAdi);
            this.Controls.Add(txtKullaniciAdi);
            this.Controls.Add(lblSifre);
            this.Controls.Add(txtSifre);
            this.Controls.Add(btnGiris);
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKullaniciAdi.Text) || string.IsNullOrEmpty(txtSifre.Text))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT k.*, p.Ad, p.Soyad, p.Pozisyon 
                           FROM Kullanicilar k
                           INNER JOIN Personel p ON k.PersonelID = p.PersonelID
                           WHERE k.KullaniciAdi = @kullaniciAdi 
                           AND k.Sifre = @sifre 
                           AND k.Durum = 1";

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kullaniciAdi", txtKullaniciAdi.Text);
                    cmd.Parameters.AddWithValue("@sifre", txtSifre.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Kullanıcı bilgilerini sakla
                            YetkiSeviyesi = Convert.ToInt32(reader["YetkiSeviyesi"]);
                            KullaniciAdi = reader["KullaniciAdi"].ToString();

                            // Son giriş tarihini güncelle
                            reader.Close();
                            string updateQuery = "UPDATE Kullanicilar SET SonGirisTarihi = GETDATE() WHERE KullaniciAdi = @kullaniciAdi";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@kullaniciAdi", KullaniciAdi);
                                updateCmd.ExecuteNonQuery();
                            }

                            // Ana formu aç
                            MainForm mainForm = new MainForm();
                            this.Hide();
                            mainForm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giriş sırasında bir hata oluştu: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}