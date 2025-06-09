using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace HuzurEviOtomasyonu
{
    public static class Validators
    {
        // TC Kimlik kontrolü
        public static bool ValidateTC(string tcKimlik)
        {
            if (string.IsNullOrEmpty(tcKimlik) || tcKimlik.Length != 11)
                return false;

            return Regex.IsMatch(tcKimlik, @"^[0-9]{11}$");
        }

        // Sayısal değer kontrolü
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]+$");
        }

        // Ad Soyad kontrolü (sadece harf ve boşluk)
        public static bool ValidateNameSurname(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return Regex.IsMatch(name, @"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$");
        }

        // Personel tekrar kontrolü
        public static bool IsPersonelExists(string ad, string soyad)
        {
            string query = "SELECT COUNT(*) FROM Personel WHERE Ad = @ad AND Soyad = @soyad";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ad", ad),
                new SqlParameter("@soyad", soyad)
            };

            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // Telefon numarası kontrolü
        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;

            return Regex.IsMatch(phone, @"^[0-9]{10,11}$");
        }

        // Email kontrolü
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return Regex.IsMatch(email,
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }
    }
}