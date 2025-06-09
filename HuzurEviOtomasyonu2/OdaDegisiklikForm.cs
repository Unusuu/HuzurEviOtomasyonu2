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
    public partial class OdaDegisiklikForm : Form
    {
        private ComboBox cmbYasli;
        private ComboBox cmbYeniOda;
        private Label lblYasli;
        private Label lblYeniOda;
        private Label lblMevcutOda;
        private TextBox txtMevcutOda;
        private Button btnDegistir;
        private DataGridView dgvOdaGecmisi;

        public OdaDegisiklikForm()
        {
            InitializeComponent();
            YaslilariYukle();
            OdalariYukle();
        }

        private void InitializeComponent()
        {
            this.Text = "Oda Değişikliği";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Yaşlı seçimi
            lblYasli = new Label();
            lblYasli.Text = "Yaşlı:";
            lblYasli.Location = new System.Drawing.Point(20, 20);
            lblYasli.AutoSize = true;

            cmbYasli = new ComboBox();
            cmbYasli.Location = new System.Drawing.Point(120, 20);
            cmbYasli.Width = 200;
            cmbYasli.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYasli.SelectedIndexChanged += new EventHandler(cmbYasli_SelectedIndexChanged);

            // Mevcut oda
            lblMevcutOda = new Label();
            lblMevcutOda.Text = "Mevcut Oda:";
            lblMevcutOda.Location = new System.Drawing.Point(20, 50);
            lblMevcutOda.AutoSize = true;

            txtMevcutOda = new TextBox();
            txtMevcutOda.Location = new System.Drawing.Point(120, 50);
            txtMevcutOda.Width = 100;
            txtMevcutOda.ReadOnly = true;

            // Yeni oda seçimi
            lblYeniOda = new Label();
            lblYeniOda.Text = "Yeni Oda:";
            lblYeniOda.Location = new System.Drawing.Point(20, 80);
            lblYeniOda.AutoSize = true;

            cmbYeniOda = new ComboBox();
            cmbYeniOda.Location = new System.Drawing.Point(120, 80);
            cmbYeniOda.Width = 100;
            cmbYeniOda.DropDownStyle = ComboBoxStyle.DropDownList;

            // Değiştir butonu
            btnDegistir = new Button();
            btnDegistir.Text = "Oda Değiştir";
            btnDegistir.Location = new System.Drawing.Point(120, 110);
            btnDegistir.Click += new EventHandler(btnDegistir_Click);

            // Oda değişiklik geçmişi grid
            dgvOdaGecmisi = new DataGridView();
            dgvOdaGecmisi.Location = new System.Drawing.Point(20, 150);
            dgvOdaGecmisi.Size = new System.Drawing.Size(740, 380);
            dgvOdaGecmisi.AllowUserToAddRows = false;
            dgvOdaGecmisi.ReadOnly = true;
            dgvOdaGecmisi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                lblYasli, cmbYasli,
                lblMevcutOda, txtMevcutOda,
                lblYeniOda, cmbYeniOda,
                btnDegistir,
                dgvOdaGecmisi
            });
        }

        private void YaslilariYukle()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT TC, Ad + ' ' + Soyad AS AdSoyad FROM Yaslilar WHERE Durum = 1 ORDER BY Ad, Soyad";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbYasli.DisplayMember = "AdSoyad";
                    cmbYasli.ValueMember = "TC";
                    cmbYasli.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yaşlı listesi yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void OdalariYukle()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT OdaNo FROM Odalar WHERE DoluMu = 0 ORDER BY OdaNo";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbYeniOda.DisplayMember = "OdaNo";
                    cmbYeniOda.ValueMember = "OdaNo";
                    cmbYeniOda.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oda listesi yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void cmbYasli_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbYasli.SelectedValue != null)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                    {
                        conn.Open();
                        string query = "SELECT OdaNo FROM Yaslilar WHERE TC = @TC";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@TC", cmbYasli.SelectedValue);
                        object result = cmd.ExecuteScalar();
                        txtMevcutOda.Text = result != null ? result.ToString() : "";

                        // Oda değişiklik geçmişini göster
                        string gecmisQuery = @"SELECT 
                            DegisiklikTarihi,
                            EskiOdaNo,
                            YeniOdaNo,
                            DegisenPersonel
                            FROM OdaDegisiklikleri 
                            WHERE YasliTC = @TC 
                            ORDER BY DegisiklikTarihi DESC";
                        SqlDataAdapter adapter = new SqlDataAdapter(gecmisQuery, conn);
                        adapter.SelectCommand.Parameters.AddWithValue("@TC", cmbYasli.SelectedValue);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvOdaGecmisi.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Yaşlı bilgileri yüklenirken hata oluştu: " + ex.Message);
                }
            }
        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            if (cmbYasli.SelectedValue == null || cmbYeniOda.SelectedValue == null)
            {
                MessageBox.Show("Lütfen yaşlı ve yeni oda seçin.");
                return;
            }

            if (txtMevcutOda.Text == cmbYeniOda.Text)
            {
                MessageBox.Show("Seçilen oda mevcut oda ile aynı!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConnection.GetConnectionString()))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Eski odayı boşalt
                            string updateEskiOda = "UPDATE Odalar SET DoluMu = 0 WHERE OdaNo = @eskiOdaNo";
                            SqlCommand cmdEskiOda = new SqlCommand(updateEskiOda, conn, transaction);
                            cmdEskiOda.Parameters.AddWithValue("@eskiOdaNo", txtMevcutOda.Text);
                            cmdEskiOda.ExecuteNonQuery();

                            // Yeni odayı dolu yap
                            string updateYeniOda = "UPDATE Odalar SET DoluMu = 1 WHERE OdaNo = @yeniOdaNo";
                            SqlCommand cmdYeniOda = new SqlCommand(updateYeniOda, conn, transaction);
                            cmdYeniOda.Parameters.AddWithValue("@yeniOdaNo", cmbYeniOda.SelectedValue);
                            cmdYeniOda.ExecuteNonQuery();

                            // Yaşlının oda bilgisini güncelle
                            string updateYasli = "UPDATE Yaslilar SET OdaNo = @yeniOdaNo WHERE TC = @TC";
                            SqlCommand cmdYasli = new SqlCommand(updateYasli, conn, transaction);
                            cmdYasli.Parameters.AddWithValue("@yeniOdaNo", cmbYeniOda.SelectedValue);
                            cmdYasli.Parameters.AddWithValue("@TC", cmbYasli.SelectedValue);
                            cmdYasli.ExecuteNonQuery();

                            // Oda değişiklik kaydı ekle
                            string insertDegisiklik = @"INSERT INTO OdaDegisiklikleri 
                                (YasliTC, EskiOdaNo, YeniOdaNo, DegisiklikTarihi, DegisenPersonel) 
                                VALUES (@TC, @eskiOdaNo, @yeniOdaNo, GETDATE(), @personel)";
                            SqlCommand cmdDegisiklik = new SqlCommand(insertDegisiklik, conn, transaction);
                            cmdDegisiklik.Parameters.AddWithValue("@TC", cmbYasli.SelectedValue);
                            cmdDegisiklik.Parameters.AddWithValue("@eskiOdaNo", txtMevcutOda.Text);
                            cmdDegisiklik.Parameters.AddWithValue("@yeniOdaNo", cmbYeniOda.SelectedValue);
                            cmdDegisiklik.Parameters.AddWithValue("@personel", Program.AktifKullaniciID);
                            cmdDegisiklik.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Oda değişikliği başarıyla yapıldı.");

                            // Formun verilerini yenile
                            OdalariYukle();
                            cmbYasli_SelectedIndexChanged(null, null);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oda değişikliği yapılırken hata oluştu: " + ex.Message);
            }
        }
    }
}