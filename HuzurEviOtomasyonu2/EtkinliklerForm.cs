using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class EtkinliklerForm : Form
    {
        private TextBox txtEtkinlikAdi;
        private DateTimePicker dtpTarih;
        private TextBox txtYer;
        private TextBox txtAciklama;
        private ComboBox cmbSorumlu;
        private ListBox lstKatilimcilar;
        private Button btnKaydet;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;

        public EtkinliklerForm()
        {
            InitializeComponent();
            PersonelleriYukle();
            YaslilariYukle();
        }

        private void InitializeComponent()
        {
            this.txtEtkinlikAdi = new TextBox();
            this.dtpTarih = new DateTimePicker();
            this.txtYer = new TextBox();
            this.txtAciklama = new TextBox();
            this.cmbSorumlu = new ComboBox();
            this.lstKatilimcilar = new ListBox();
            this.btnKaydet = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();

            // Form
            this.Text = "Etkinlik Ekle";
            this.Size = new System.Drawing.Size(600, 500);

            // Label1 - Etkinlik Adı
            this.label1.Text = "Etkinlik Adı:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Size = new System.Drawing.Size(100, 20);

            // txtEtkinlikAdi
            this.txtEtkinlikAdi.Location = new System.Drawing.Point(120, 20);
            this.txtEtkinlikAdi.Size = new System.Drawing.Size(200, 20);

            // Label2 - Tarih
            this.label2.Text = "Tarih:";
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Size = new System.Drawing.Size(100, 20);

            // dtpTarih
            this.dtpTarih.Location = new System.Drawing.Point(120, 60);
            this.dtpTarih.Size = new System.Drawing.Size(200, 20);
            this.dtpTarih.Format = DateTimePickerFormat.Short;

            // Label3 - Yer
            this.label3.Text = "Yer:";
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Size = new System.Drawing.Size(100, 20);

            // txtYer
            this.txtYer.Location = new System.Drawing.Point(120, 100);
            this.txtYer.Size = new System.Drawing.Size(200, 20);

            // Label4 - Açıklama
            this.label4.Text = "Açıklama:";
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.Size = new System.Drawing.Size(100, 20);

            // txtAciklama
            this.txtAciklama.Location = new System.Drawing.Point(120, 140);
            this.txtAciklama.Size = new System.Drawing.Size(200, 60);
            this.txtAciklama.Multiline = true;

            // Label5 - Sorumlu
            this.label5.Text = "Sorumlu:";
            this.label5.Location = new System.Drawing.Point(20, 220);
            this.label5.Size = new System.Drawing.Size(100, 20);

            // cmbSorumlu
            this.cmbSorumlu.Location = new System.Drawing.Point(120, 220);
            this.cmbSorumlu.Size = new System.Drawing.Size(200, 20);
            this.cmbSorumlu.DropDownStyle = ComboBoxStyle.DropDownList;

            // Label6 - Katılımcılar
            this.label6.Text = "Katılımcılar:";
            this.label6.Location = new System.Drawing.Point(350, 20);
            this.label6.Size = new System.Drawing.Size(100, 20);

            // lstKatilimcilar
            this.lstKatilimcilar.Location = new System.Drawing.Point(350, 40);
            this.lstKatilimcilar.Size = new System.Drawing.Size(200, 200);
            this.lstKatilimcilar.SelectionMode = SelectionMode.MultiSimple;

            // btnKaydet
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.Location = new System.Drawing.Point(120, 260);
            this.btnKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnKaydet.Click += new EventHandler(btnKaydet_Click);

            // Controls
            this.Controls.AddRange(new Control[] {
                this.label1, this.txtEtkinlikAdi,
                this.label2, this.dtpTarih,
                this.label3, this.txtYer,
                this.label4, this.txtAciklama,
                this.label5, this.cmbSorumlu,
                this.label6, this.lstKatilimcilar,
                this.btnKaydet
            });
        }

        private void PersonelleriYukle()
        {
            string query = "SELECT PersonelID, Ad + ' ' + Soyad as AdSoyad FROM Personel";
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cmbSorumlu.Items.Add(new ComboBoxItem
                            {
                                Value = dr["PersonelID"].ToString(),
                                Text = dr["AdSoyad"].ToString()
                            });
                        }
                    }
                }
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
                            lstKatilimcilar.Items.Add(new ComboBoxItem
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
            if (string.IsNullOrEmpty(txtEtkinlikAdi.Text) || string.IsNullOrEmpty(txtYer.Text) ||
                cmbSorumlu.SelectedItem == null)
            {
                MessageBox.Show("Lütfen zorunlu alanları doldurunuz!");
                return;
            }

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Etkinlik ekleme
                        string etkinlikQuery = @"INSERT INTO Etkinlikler (EtkinlikAdi, Tarih, Yer, Aciklama, SorumluPersonelID) 
                                               VALUES (@ad, @tarih, @yer, @aciklama, @sorumlu); SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(etkinlikQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@ad", txtEtkinlikAdi.Text);
                        cmd.Parameters.AddWithValue("@tarih", dtpTarih.Value);
                        cmd.Parameters.AddWithValue("@yer", txtYer.Text);
                        cmd.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
                        cmd.Parameters.AddWithValue("@sorumlu", ((ComboBoxItem)cmbSorumlu.SelectedItem).Value);

                        int etkinlikID = Convert.ToInt32(cmd.ExecuteScalar());

                        // Katılımcıları ekleme
                        foreach (int index in lstKatilimcilar.SelectedIndices)
                        {
                            string katilimQuery = @"INSERT INTO EtkinlikKatilim (EtkinlikID, YasliTC) 
                                                  VALUES (@etkinlikID, @yasliTC)";

                            cmd = new SqlCommand(katilimQuery, conn, transaction);
                            cmd.Parameters.AddWithValue("@etkinlikID", etkinlikID);
                            cmd.Parameters.AddWithValue("@yasliTC", ((ComboBoxItem)lstKatilimcilar.Items[index]).Value);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Etkinlik başarıyla kaydedildi!");
                        FormuTemizle();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Etkinlik kaydedilirken bir hata oluştu: " + ex.Message);
                    }
                }
            }
        }

        private void FormuTemizle()
        {
            txtEtkinlikAdi.Clear();
            txtYer.Clear();
            txtAciklama.Clear();
            dtpTarih.Value = DateTime.Now;
            cmbSorumlu.SelectedIndex = -1;
            for (int i = 0; i < lstKatilimcilar.Items.Count; i++)
            {
                lstKatilimcilar.SetSelected(i, false);
            }
        }
    }
}