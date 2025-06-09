using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class YasliEkleForm : Form
    {
        private TextBox txtTC;
        private TextBox txtAd;
        private TextBox txtSoyad;
        private TextBox txtOdaNo;
        private TextBox txtYakinTel;
        private Button btnKaydet;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        public YasliEkleForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtTC = new TextBox();
            this.txtAd = new TextBox();
            this.txtSoyad = new TextBox();
            this.txtOdaNo = new TextBox();
            this.txtYakinTel = new TextBox();
            this.btnKaydet = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();

            // Form
            this.Text = "Yaşlı Ekle";
            this.Size = new System.Drawing.Size(400, 300);

            // Label1 - TC
            this.label1.Text = "TC:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Size = new System.Drawing.Size(80, 20);

            // txtTC
            this.txtTC.Location = new System.Drawing.Point(120, 20);
            this.txtTC.Size = new System.Drawing.Size(200, 20);
            this.txtTC.MaxLength = 11;
            this.txtTC.KeyPress += new KeyPressEventHandler(txtTC_KeyPress);

            // Label2 - Ad
            this.label2.Text = "Ad:";
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Size = new System.Drawing.Size(80, 20);

            // txtAd
            this.txtAd.Location = new System.Drawing.Point(120, 60);
            this.txtAd.Size = new System.Drawing.Size(200, 20);
            this.txtAd.KeyPress += new KeyPressEventHandler(txtAd_KeyPress);

            // Label3 - Soyad
            this.label3.Text = "Soyad:";
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Size = new System.Drawing.Size(80, 20);

            // txtSoyad
            this.txtSoyad.Location = new System.Drawing.Point(120, 100);
            this.txtSoyad.Size = new System.Drawing.Size(200, 20);
            this.txtSoyad.KeyPress += new KeyPressEventHandler(txtAd_KeyPress);

            // Label4 - Oda No
            this.label4.Text = "Oda No:";
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.Size = new System.Drawing.Size(80, 20);

            // txtOdaNo
            this.txtOdaNo.Location = new System.Drawing.Point(120, 140);
            this.txtOdaNo.Size = new System.Drawing.Size(200, 20);
            this.txtOdaNo.KeyPress += new KeyPressEventHandler(txtTC_KeyPress);

            // Label5 - Yakın Telefon
            this.label5.Text = "Yakın Tel:";
            this.label5.Location = new System.Drawing.Point(20, 180);
            this.label5.Size = new System.Drawing.Size(80, 20);

            // txtYakinTel
            this.txtYakinTel.Location = new System.Drawing.Point(120, 180);
            this.txtYakinTel.Size = new System.Drawing.Size(200, 20);
            this.txtYakinTel.MaxLength = 11;
            this.txtYakinTel.KeyPress += new KeyPressEventHandler(txtTC_KeyPress);

            // btnKaydet
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Location = new System.Drawing.Point(120, 220);
            this.btnKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnKaydet.Click += new EventHandler(btnKaydet_Click);

            // Controls
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSoyad);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtOdaNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtYakinTel);
            this.Controls.Add(this.btnKaydet);
        }

        private void txtTC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTC.Text) || string.IsNullOrEmpty(txtAd.Text) ||
                string.IsNullOrEmpty(txtSoyad.Text) || string.IsNullOrEmpty(txtOdaNo.Text))
            {
                MessageBox.Show("Lütfen zorunlu alanları doldurunuz!");
                return;
            }

            // Aynı isim soyisim kontrolü
            string kontrolQuery = "SELECT COUNT(*) FROM Yaslilar WHERE Ad = @ad AND Soyad = @soyad";
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(kontrolQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                    cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                    int sayi = (int)cmd.ExecuteScalar();
                    if (sayi > 0)
                    {
                        MessageBox.Show("Bu isim ve soyisimde yaşlı zaten var!");
                        return;
                    }
                }
            }

            // Yaşlı ekleme
            string query = @"INSERT INTO Yaslilar (TC, Ad, Soyad, OdaNo, YakinTelefon, GirisTarihi) 
                           VALUES (@tc, @ad, @soyad, @odaNo, @yakinTel, @girisTarihi)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@tc", txtTC.Text),
                new SqlParameter("@ad", txtAd.Text),
                new SqlParameter("@soyad", txtSoyad.Text),
                new SqlParameter("@odaNo", Convert.ToInt32(txtOdaNo.Text)),
                new SqlParameter("@yakinTel", txtYakinTel.Text),
                new SqlParameter("@girisTarihi", DateTime.Now)
            };

            if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
            {
                MessageBox.Show("Yaşlı kaydı başarıyla eklendi!");
                txtTC.Clear();
                txtAd.Clear();
                txtSoyad.Clear();
                txtOdaNo.Clear();
                txtYakinTel.Clear();
            }
        }
    }
}
