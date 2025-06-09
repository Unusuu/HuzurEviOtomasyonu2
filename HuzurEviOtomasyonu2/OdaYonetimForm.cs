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
    public partial class OdaYonetimForm : Form
    {
        private TextBox txtOdaNo;
        private TextBox txtKapasite;
        private ComboBox cmbOdaTipi;
        private ComboBox cmbKat;
        private ListBox lstMevcutYaslilar;
        private ComboBox cmbYeniYasli;
        private Button btnOdaEkle;
        private Button btnYasliEkle;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private DataGridView dgvOdalar;

        public OdaYonetimForm()
        {
            InitializeComponent();
            OdalariListele();
            YaslilariYukle();
        }

        private void InitializeComponent()
        {
            this.txtOdaNo = new TextBox();
            this.txtKapasite = new TextBox();
            this.cmbOdaTipi = new ComboBox();
            this.cmbKat = new ComboBox();
            this.lstMevcutYaslilar = new ListBox();
            this.cmbYeniYasli = new ComboBox();
            this.btnOdaEkle = new Button();
            this.btnYasliEkle = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.dgvOdalar = new DataGridView();

            // Form
            this.Text = "Oda Yönetimi";
            this.Size = new System.Drawing.Size(800, 600);

            // Label1 - Oda No
            this.label1.Text = "Oda No:";
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Size = new System.Drawing.Size(80, 20);

            // txtOdaNo
            this.txtOdaNo.Location = new System.Drawing.Point(100, 20);
            this.txtOdaNo.Size = new System.Drawing.Size(100, 20);
            this.txtOdaNo.KeyPress += new KeyPressEventHandler(SadeceRakam_KeyPress);

            // Label2 - Kapasite
            this.label2.Text = "Kapasite:";
            this.label2.Location = new System.Drawing.Point(20, 50);
            this.label2.Size = new System.Drawing.Size(80, 20);

            // txtKapasite
            this.txtKapasite.Location = new System.Drawing.Point(100, 50);
            this.txtKapasite.Size = new System.Drawing.Size(100, 20);
            this.txtKapasite.KeyPress += new KeyPressEventHandler(SadeceRakam_KeyPress);

            // Label3 - Oda Tipi
            this.label3.Text = "Oda Tipi:";
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Size = new System.Drawing.Size(80, 20);

            // cmbOdaTipi
            this.cmbOdaTipi.Location = new System.Drawing.Point(100, 80);
            this.cmbOdaTipi.Size = new System.Drawing.Size(150, 20);
            this.cmbOdaTipi.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbOdaTipi.Items.AddRange(new string[] { "Tek Kişilik", "İki Kişilik", "Üç Kişilik" });

            // Label4 - Kat
            this.label4.Text = "Kat:";
            this.label4.Location = new System.Drawing.Point(20, 110);
            this.label4.Size = new System.Drawing.Size(80, 20);

            // cmbKat
            this.cmbKat.Location = new System.Drawing.Point(100, 110);
            this.cmbKat.Size = new System.Drawing.Size(100, 20);
            this.cmbKat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbKat.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });

            // btnOdaEkle
            this.btnOdaEkle.Text = "Oda Ekle";
            this.btnOdaEkle.Location = new System.Drawing.Point(100, 140);
            this.btnOdaEkle.Size = new System.Drawing.Size(100, 30);
            this.btnOdaEkle.Click += new EventHandler(btnOdaEkle_Click);

            // Label5 - Mevcut Yaşlılar
            this.label5.Text = "Odadaki Yaşlılar:";
            this.label5.Location = new System.Drawing.Point(300, 20);
            this.label5.Size = new System.Drawing.Size(100, 20);

            // lstMevcutYaslilar
            this.lstMevcutYaslilar.Location = new System.Drawing.Point(300, 40);
            this.lstMevcutYaslilar.Size = new System.Drawing.Size(200, 100);

            // Label6 - Yeni Yaşlı
            this.label6.Text = "Yaşlı Ekle:";
            this.label6.Location = new System.Drawing.Point(300, 150);
            this.label6.Size = new System.Drawing.Size(80, 20);

            // cmbYeniYasli
            this.cmbYeniYasli.Location = new System.Drawing.Point(380, 150);
            this.cmbYeniYasli.Size = new System.Drawing.Size(150, 20);
            this.cmbYeniYasli.DropDownStyle = ComboBoxStyle.DropDownList;

            // btnYasliEkle
            this.btnYasliEkle.Text = "Yaşlı Ekle";
            this.btnYasliEkle.Location = new System.Drawing.Point(540, 150);
            this.btnYasliEkle.Size = new System.Drawing.Size(80, 23);
            this.btnYasliEkle.Click += new EventHandler(btnYasliEkle_Click);

            // dgvOdalar
            this.dgvOdalar.Location = new System.Drawing.Point(20, 200);
            this.dgvOdalar.Size = new System.Drawing.Size(750, 350);
            this.dgvOdalar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOdalar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOdalar.MultiSelect = false;
            this.dgvOdalar.ReadOnly = true;
            this.dgvOdalar.CellClick += new DataGridViewCellEventHandler(dgvOdalar_CellClick);

            // Controls
            this.Controls.AddRange(new Control[] {
                this.label1, this.txtOdaNo,
                this.label2, this.txtKapasite,
                this.label3, this.cmbOdaTipi,
                this.label4, this.cmbKat,
                this.btnOdaEkle,
                this.label5, this.lstMevcutYaslilar,
                this.label6, this.cmbYeniYasli,
                this.btnYasliEkle,
                this.dgvOdalar
            });
        }

        private void SadeceRakam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void OdalariListele()
        {
            string query = @"SELECT o.OdaNo, o.Kapasite, o.OdaTipi, o.Kat, o.DoluYatak,
                           CASE WHEN o.DoluYatak >= o.Kapasite THEN 'Dolu' ELSE 'Müsait' END as Durum
                           FROM Odalar o ORDER BY o.OdaNo";

            DataTable dt = DatabaseConnection.ExecuteQuery(query);
            dgvOdalar.DataSource = dt;
        }

        private void YaslilariYukle()
        {
            string query = "SELECT TC, Ad + ' ' + Soyad as AdSoyad FROM Yaslilar WHERE OdaNo IS NULL";
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cmbYeniYasli.Items.Add(new ComboBoxItem
                            {
                                Value = dr["TC"].ToString(),
                                Text = dr["AdSoyad"].ToString()
                            });
                        }
                    }
                }
            }
        }

        private void btnOdaEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOdaNo.Text) || string.IsNullOrEmpty(txtKapasite.Text) ||
                cmbOdaTipi.SelectedIndex == -1 || cmbKat.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
                return;
            }

            string query = @"INSERT INTO Odalar (OdaNo, Kapasite, OdaTipi, Kat, DoluYatak) 
                           VALUES (@odaNo, @kapasite, @odaTipi, @kat, 0)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@odaNo", Convert.ToInt32(txtOdaNo.Text)),
                new SqlParameter("@kapasite", Convert.ToInt32(txtKapasite.Text)),
                new SqlParameter("@odaTipi", cmbOdaTipi.SelectedItem.ToString()),
                new SqlParameter("@kat", Convert.ToInt32(cmbKat.SelectedItem.ToString()))
            };

            if (DatabaseConnection.ExecuteParameterizedQuery(query, parameters))
            {
                MessageBox.Show("Oda başarıyla eklendi!");
                OdalariListele();
                FormuTemizle();
            }
        }

        private void dgvOdalar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int odaNo = Convert.ToInt32(dgvOdalar.Rows[e.RowIndex].Cells["OdaNo"].Value);
                YaslilariGoster(odaNo);
            }
        }

        private void YaslilariGoster(int odaNo)
        {
            lstMevcutYaslilar.Items.Clear();
            string query = "SELECT TC, Ad + ' ' + Soyad as AdSoyad FROM Yaslilar WHERE OdaNo = @odaNo";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@odaNo", odaNo);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lstMevcutYaslilar.Items.Add(new ComboBoxItem
                            {
                                Value = dr["TC"].ToString(),
                                Text = dr["AdSoyad"].ToString()
                            });
                        }
                    }
                }
            }
        }

        private void btnYasliEkle_Click(object sender, EventArgs e)
        {
            if (dgvOdalar.SelectedRows.Count == 0 || cmbYeniYasli.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir oda ve yaşlı seçiniz!");
                return;
            }

            int odaNo = Convert.ToInt32(dgvOdalar.SelectedRows[0].Cells["OdaNo"].Value);
            int kapasite = Convert.ToInt32(dgvOdalar.SelectedRows[0].Cells["Kapasite"].Value);
            int doluYatak = Convert.ToInt32(dgvOdalar.SelectedRows[0].Cells["DoluYatak"].Value);

            if (doluYatak >= kapasite)
            {
                MessageBox.Show("Bu oda dolu!");
                return;
            }

            string yasliTC = ((ComboBoxItem)cmbYeniYasli.SelectedItem).Value;

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Yaşlıyı odaya ekle
                        string updateYasli = "UPDATE Yaslilar SET OdaNo = @odaNo WHERE TC = @tc";
                        SqlCommand cmd = new SqlCommand(updateYasli, conn, transaction);
                        cmd.Parameters.AddWithValue("@odaNo", odaNo);
                        cmd.Parameters.AddWithValue("@tc", yasliTC);
                        cmd.ExecuteNonQuery();

                        // Odanın dolu yatak sayısını güncelle
                        string updateOda = "UPDATE Odalar SET DoluYatak = DoluYatak + 1 WHERE OdaNo = @odaNo";
                        cmd = new SqlCommand(updateOda, conn, transaction);
                        cmd.Parameters.AddWithValue("@odaNo", odaNo);
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("Yaşlı odaya başarıyla eklendi!");

                        OdalariListele();
                        YaslilariGoster(odaNo);
                        YaslilariYukle(); // Boştaki yaşlıları yeniden yükle
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Hata oluştu: " + ex.Message);
                    }
                }
            }
        }

        private void FormuTemizle()
        {
            txtOdaNo.Clear();
            txtKapasite.Clear();
            cmbOdaTipi.SelectedIndex = -1;
            cmbKat.SelectedIndex = -1;
            lstMevcutYaslilar.Items.Clear();
            cmbYeniYasli.SelectedIndex = -1;
        }
    }
}
