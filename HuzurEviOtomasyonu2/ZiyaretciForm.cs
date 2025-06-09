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
    public partial class ZiyaretciForm : Form
    {
        private TextBox txtTC;
        private TextBox txtAd;
        private TextBox txtSoyad;
        private TextBox txtTelefon;
        private ComboBox cmbYasli;
        private TextBox txtYakinlik;
        private DateTimePicker dtpTarih;
        private TextBox txtGirisSaat;
        private TextBox txtCikisSaat;
        private Button btnKaydet;
        private DataGridView dgvZiyaretler;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;

        public ZiyaretciForm()
        {
            InitializeComponent();
            YaslilariYukle();
            ZiyaretleriListele();
        }

        private void InitializeComponent()
        {
            // Form elemanlarını oluştur
            this.txtTC = new TextBox();
            this.txtAd = new TextBox();
            this.txtSoyad = new TextBox();
            this.txtTelefon = new TextBox();
            this.cmbYasli = new ComboBox();
            this.txtYakinlik = new TextBox();
            this.dtpTarih = new DateTimePicker();
            this.txtGirisSaat = new TextBox();
            this.txtCikisSaat = new TextBox();
            this.btnKaydet = new Button();
            this.dgvZiyaretler = new DataGridView();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();

            // Form
            this.Text = "Ziyaretçi Kayıt";
            this.Size = new System.Drawing.Size(800, 600);

            // Label ve TextBox'ları yerleştir
            // TC
            this.label1.Text = "TC:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Size = new System.Drawing.Size(80, 20);

            this.txtTC.Location = new System.Drawing.Point(120, 20);
            this.txtTC.Size = new System.Drawing.Size(150, 20);
            this.txtTC.MaxLength = 11;
            this.txtTC.KeyPress += new KeyPressEventHandler(SadeceRakam_KeyPress);

            // Ad
            this.label2.Text = "Ad:";
            this.label2.Location = new System.Drawing.Point(20, 50);
            this.label2.Size = new System.Drawing.Size(80, 20);

            this.txtAd.Location = new System.Drawing.Point(120, 50);
            this.txtAd.Size = new System.Drawing.Size(150, 20);
            this.txtAd.KeyPress += new KeyPressEventHandler(SadeceHarf_KeyPress);

            // Soyad
            this.label3.Text = "Soyad:";
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Size = new System.Drawing.Size(80, 20);

            this.txtSoyad.Location = new System.Drawing.Point(120, 80);
            this.txtSoyad.Size = new System.Drawing.Size(150, 20);
            this.txtSoyad.KeyPress += new KeyPressEventHandler(SadeceHarf_KeyPress);

            // Telefon
            this.label4.Text = "Telefon:";
            this.label4.Location = new System.Drawing.Point(20, 110);
            this.label4.Size = new System.Drawing.Size(80, 20);

            this.txtTelefon.Location = new System.Drawing.Point(120, 110);
            this.txtTelefon.Size = new System.Drawing.Size(150, 20);
            this.txtTelefon.MaxLength = 11;
            this.txtTelefon.KeyPress += new KeyPressEventHandler(SadeceRakam_KeyPress);

            // Ziyaret Edilen Yaşlı
            this.label5.Text = "Ziyaret Edilen:";
            this.label5.Location = new System.Drawing.Point(20, 140);
            this.label5.Size = new System.Drawing.Size(80, 20);

            this.cmbYasli.Location = new System.Drawing.Point(120, 140);
            this.cmbYasli.Size = new System.Drawing.Size(150, 20);
            this.cmbYasli.DropDownStyle = ComboBoxStyle.DropDownList;

            // Yakınlık
            this.label6.Text = "Yakınlık:";
            this.label6.Location = new System.Drawing.Point(20, 170);
            this.label6.Size = new System.Drawing.Size(80, 20);

            this.txtYakinlik.Location = new System.Drawing.Point(120, 170);
            this.txtYakinlik.Size = new System.Drawing.Size(150, 20);

            // Tarih
            this.label7.Text = "Tarih:";
            this.label7.Location = new System.Drawing.Point(20, 200);
            this.label7.Size = new System.Drawing.Size(80, 20);

            this.dtpTarih.Location = new System.Drawing.Point(120, 200);
            this.dtpTarih.Size = new System.Drawing.Size(150, 20);
            this.dtpTarih.Format = DateTimePickerFormat.Short;

            // Giriş Saati
            this.label8.Text = "Giriş Saati:";
            this.label8.Location = new System.Drawing.Point(20, 230);
            this.label8.Size = new System.Drawing.Size(80, 20);

            this.txtGirisSaat.Location = new System.Drawing.Point(120, 230);
            this.txtGirisSaat.Size = new System.Drawing.Size(150, 20);
            this.txtGirisSaat.Text = DateTime.Now.ToString("HH:mm");

            // Çıkış Saati
            this.label9.Text = "Çıkış Saati:";
            this.label9.Location = new System.Drawing.Point(20, 260);
            this.label9.Size = new System.Drawing.Size(80, 20);

            this.txtCikisSaat.Location = new System.Drawing.Point(120, 260);
            this.txtCikisSaat.Size = new System.Drawing.Size(150, 20);

            // Kaydet Butonu
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Location = new System.Drawing.Point(120, 290);
            this.btnKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnKaydet.Click += new EventHandler(btnKaydet_Click);

            // DataGridView
            this.dgvZiyaretler.Location = new System.Drawing.Point(20, 330);
            this.dgvZiyaretler.Size = new System.Drawing.Size(750, 220);
            this.dgvZiyaretler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvZiyaretler.ReadOnly = true;

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                this.label1, this.txtTC,
                this.label2, this.txtAd,
                this.label3, this.txtSoyad,
                this.label4, this.txtTelefon,
                this.label5, this.cmbYasli,
                this.label6, this.txtYakinlik,
                this.label7, this.dtpTarih,
                this.label8, this.txtGirisSaat,
                this.label9, this.txtCikisSaat,
                this.btnKaydet,
                this.dgvZiyaretler
            });
        }

        private void SadeceRakam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SadeceHarf_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void YaslilariYukle()
        {
            string query = "SELECT TC, Ad + ' ' + Soyad as AdSoyad FROM Yaslilar";
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cmbYasli.Items.Add(new ComboBoxItem
                            {
                                Value = dr["TC"].ToString(),
                                Text = dr["AdSoyad"].ToString()
                            });
                        }
                    }
                }
            }
        }

        private void ZiyaretleriListele()
        {
            string query = @"SELECT z.ZiyaretciAd + ' ' + z.ZiyaretciSoyad as ZiyaretciAdSoyad,
                           y.Ad + ' ' + y.Soyad as YasliAdSoyad,
                           z.ZiyaretTarihi, z.GirisSaati, z.CikisSaati, z.Yakinlik
                           FROM Ziyaretciler z
                           INNER JOIN Yaslilar y ON z.ZiyaretEdilenYasliTC = y.TC
                           ORDER BY z.ZiyaretTarihi DESC";

            DataTable dt = DatabaseConnection.ExecuteQuery(query);
            dgvZiyaretler.DataSource = dt;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTC.Text) || string.IsNullOrEmpty(txtAd.Text) ||
                string.IsNullOrEmpty(txtSoyad.Text) || cmbYasli.SelectedItem == null)
            {
                MessageBox.Show("Lütfen zorunlu alanları doldurunuz!");
                return;
            }

            string query = @"INSERT INTO Ziyaretciler 
                           (ZiyaretciAd, ZiyaretciSoyad, TC, TelefonNo, 
                            ZiyaretEdilenYasliTC, ZiyaretTarihi, GirisSaati, 
                            CikisSaati, Yakinlik)
                           VALUES 
                           (@ad, @soyad, @tc, @tel, @yasliTC, @tarih, 
                            @giris, @cikis, @yakinlik)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ad", txtAd.Text),
                new SqlParameter("@soyad", txtSoyad.Text),
                new SqlParameter("@tc", txtTC.Text),
                new SqlParameter("@tel", txtTelefon.Text),
                new SqlParameter("@yasliTC", ((ComboBoxItem)cmbYasli.SelectedItem).Value),
                new SqlParameter("@tarih", dtpTarih.Value.Date),
                new SqlParameter("@giris", txtGirisSaat.Text),
                new SqlParameter("@cikis", txtCikisSaat.Text),
                new SqlParameter("@yakinlik", txtYakinlik.Text)
            };

            if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
            {
                MessageBox.Show("Ziyaretçi kaydı başarıyla eklendi!");
                FormuTemizle();
                ZiyaretleriListele();
            }
        }

        private void FormuTemizle()
        {
            txtTC.Clear();
            txtAd.Clear();
            txtSoyad.Clear();
            txtTelefon.Clear();
            cmbYasli.SelectedIndex = -1;
            txtYakinlik.Clear();
            dtpTarih.Value = DateTime.Now;
            txtGirisSaat.Text = DateTime.Now.ToString("HH:mm");
            txtCikisSaat.Clear();
        }
    }
}