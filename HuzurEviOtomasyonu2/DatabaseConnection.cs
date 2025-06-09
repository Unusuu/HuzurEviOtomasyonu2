using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public class DatabaseConnection
    {
        // Veritabanı bağlantı stringi
        private static string connectionString = @"Server=localhost;Database=HuzurEviDB;Trusted_Connection=True;";
        private static SqlConnection connection;
        // Bağlantı stringini döndüren metod
        public static string GetConnectionString()
        {
            return connectionString;
        }

        // Bağlantıyı test eden metod
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        // Bağlantıyı açan metod
        public static SqlConnection GetConnection()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı sırasında hata oluştu: " + ex.Message,
                    "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Bağlantıyı kapatan metod
        public static void CloseConnection()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı kapatılırken hata oluştu: " + ex.Message,
                    "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Genel sorgu çalıştırma metodu (SELECT için)
        public static DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorgu çalıştırılırken hata oluştu: " + ex.Message,
                    "Sorgu Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }

        // INSERT, UPDATE, DELETE sorguları için metod
        public static bool ExecuteNonQuery(string query)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message,
                    "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        // Parametreli sorgu çalıştırma metodu
        public static bool ExecuteParameterizedQuery(string query, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parametreli sorgu çalıştırılırken hata oluştu: " + ex.Message,
                    "Sorgu Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        // Tek değer döndüren sorgu metodu (örneğin COUNT, SUM gibi)
        public static object ExecuteScalar(string query)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorgu çalıştırılırken hata oluştu: " + ex.Message,
                    "Sorgu Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}