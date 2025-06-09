using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class YemekListesiForm : Form
    {
        private DateTimePicker dtpTarih;
        private TextBox txtSabah;
        private TextBox txtOgle;
        private TextBox txtAksam;
        private Button btnKaydet;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;

        public YemekListesiForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dtpTarih = new DateTimePicker();
            this.txtSabah = new TextBox();
            this.txtOgle = new TextBox();
            this.txtAksam = new TextBox();
            this.btnKaydet = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();

            // Form
            this.Text = "Yemek Listesi";
            this.Size = new System.Drawing.Size(500, 400);

            // Label1 - Tarih
            this.label1.Text = "Tarih:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Size = new System.Drawing.Size(80, 20);

            // dtpTarih
            this.dtpTarih.Location = new System.Drawing.Point(120, 20);
            this.dtpTarih.Size = new System.Drawing.Size(200, 20);
            this.dtpTarih.Format = DateTimePickerFormat.Short;

            // Label2 - Sabah
            this.label2.Text = "Sabah:";
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Size = new System.Drawing.Size(80, 20);

            // txtSabah
            this.txtSabah.Location = new System.Drawing.Point(120, 60);
            this.txtSabah.Size = new System.Drawing.Size(300, 80);
            this.txtSabah.Multiline = true;
            this.txtSabah.ScrollBars = ScrollBars.Vertical;

            // Label3 - Öğle
            this.label3.Text = "Öğle:";
            this.label3.Location = new System.Drawing.Point(20, 150);
            this.label3.Size = new System.Drawing.Size(80, 20);

            // txtOgle
            this.txtOgle.Location = new System.Drawing.Point(120, 150);
            this.txtOgle.Size = new System.Drawing.Size(300, 80);
            this.txtOgle.Multiline = true;
            this.txtOgle.ScrollBars = ScrollBars.Vertical;

            // Label4 - Akşam
            this.label4.Text = "Akşam:";
            this.label4.Location = new System.Drawing.Point(20, 240);
            this.label4.Size = new System.Drawing.Size(80, 20);

            // txtAksam
            this.txtAksam.Location = new System.Drawing.Point(120, 240);
            this.txtAksam.Size = new System.Drawing.Size(300, 80);
            this.txtAksam.Multiline = true;
            this.txtAksam.ScrollBars = ScrollBars.Vertical;

            // btnKaydet
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Location = new System.Drawing.Point(120, 330);
            this.btnKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnKaydet.Click += new EventHandler(btnKaydet_Click);

            // Controls
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpTarih);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSabah);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtOgle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAksam);
            this.Controls.Add(this.btnKaydet);
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSabah.Text) || string.IsNullOrEmpty(txtOgle.Text) ||
                string.IsNullOrEmpty(txtAksam.Text))
            {
                MessageBox.Show("Lütfen tüm öğünleri doldurunuz!");
                return;
            }

            // Aynı tarihte menü var mı kontrolü
            string kontrolQuery = "SELECT COUNT(*) FROM YemekListesi WHERE Tarih = @tarih";
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(kontrolQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@tarih", dtpTarih.Value.Date);
                    int sayi = (int)cmd.ExecuteScalar();
                    if (sayi > 0)
                    {
                        MessageBox.Show("Bu tarihe ait yemek listesi zaten mevcut!");
                        return;
                    }
                }
            }

            string query = @"INSERT INTO YemekListesi (Tarih, Sabah, Ogle, Aksam) 
                           VALUES (@tarih, @sabah, @ogle, @aksam)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@tarih", dtpTarih.Value.Date),
                new SqlParameter("@sabah", txtSabah.Text),
                new SqlParameter("@ogle", txtOgle.Text),
                new SqlParameter("@aksam", txtAksam.Text)
            };

            if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
            {
                MessageBox.Show("Yemek listesi başarıyla kaydedildi!");
                txtSabah.Clear();
                txtOgle.Clear();
                txtAksam.Clear();
                dtpTarih.Value = DateTime.Now;
            }
        }
    }
}
