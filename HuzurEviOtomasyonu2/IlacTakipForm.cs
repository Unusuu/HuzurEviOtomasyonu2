using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class IlacTakipForm : Form
    {
        private ComboBox cmbYasli;
        private TextBox txtIlacAdi;
        private TextBox txtDoz;
        private TextBox txtSaat;
        private Button btnKaydet;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;

        public IlacTakipForm()
        {
            InitializeComponent();
            YaslilariYukle();
        }

        private void InitializeComponent()
        {
            this.cmbYasli = new ComboBox();
            this.txtIlacAdi = new TextBox();
            this.txtDoz = new TextBox();
            this.txtSaat = new TextBox();
            this.btnKaydet = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();

            // Form
            this.Text = "İlaç Takip";
            this.Size = new System.Drawing.Size(400, 300);

            // Label1 - Yaşlı
            this.label1.Text = "Yaşlı:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Size = new System.Drawing.Size(80, 20);

            // cmbYasli
            this.cmbYasli.Location = new System.Drawing.Point(120, 20);
            this.cmbYasli.Size = new System.Drawing.Size(200, 20);
            this.cmbYasli.DropDownStyle = ComboBoxStyle.DropDownList;

            // Label2 - İlaç Adı
            this.label2.Text = "İlaç Adı:";
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Size = new System.Drawing.Size(80, 20);

            // txtIlacAdi
            this.txtIlacAdi.Location = new System.Drawing.Point(120, 60);
            this.txtIlacAdi.Size = new System.Drawing.Size(200, 20);

            // Label3 - Doz
            this.label3.Text = "Doz:";
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Size = new System.Drawing.Size(80, 20);

            // txtDoz
            this.txtDoz.Location = new System.Drawing.Point(120, 100);
            this.txtDoz.Size = new System.Drawing.Size(200, 20);

            // Label4 - Saat
            this.label4.Text = "Saat:";
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.Size = new System.Drawing.Size(80, 20);

            // txtSaat
            this.txtSaat.Location = new System.Drawing.Point(120, 140);
            this.txtSaat.Size = new System.Drawing.Size(200, 20);

            // btnKaydet
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Location = new System.Drawing.Point(120, 180);
            this.btnKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnKaydet.Click += new EventHandler(btnKaydet_Click);

            // Controls
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbYasli);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIlacAdi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDoz);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSaat);
            this.Controls.Add(this.btnKaydet);
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

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (cmbYasli.SelectedItem == null || string.IsNullOrEmpty(txtIlacAdi.Text) ||
                string.IsNullOrEmpty(txtDoz.Text) || string.IsNullOrEmpty(txtSaat.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
                return;
            }

            string yasliTC = ((ComboBoxItem)cmbYasli.SelectedItem).Value;

            string query = @"INSERT INTO IlacTakip (YasliTC, IlacAdi, Doz, KullanımSaati, BaslangicTarihi) 
                           VALUES (@yasliTC, @ilacAdi, @doz, @saat, @tarih)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@yasliTC", yasliTC),
                new SqlParameter("@ilacAdi", txtIlacAdi.Text),
                new SqlParameter("@doz", txtDoz.Text),
                new SqlParameter("@saat", txtSaat.Text),
                new SqlParameter("@tarih", DateTime.Now)
            };

            if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
            {
                MessageBox.Show("İlaç kaydı başarıyla eklendi!");
                txtIlacAdi.Clear();
                txtDoz.Clear();
                txtSaat.Clear();
                cmbYasli.SelectedIndex = -1;
            }
        }
    }

    public class ComboBoxItem
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}