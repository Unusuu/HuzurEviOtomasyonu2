using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    static class Program
    {
        // Aktif kullanıcının ID'sini tutacak statik değişken
        public static int AktifKullaniciID { get; set; }
        public static string AktifKullaniciAdi { get; set; }
        public static int YetkiSeviyesi { get; set; }

        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Önce login formunu göster
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Login başarılı ise ana formu aç
                Application.Run(new MainForm());
            }
            else
            {
                // Login başarısız veya iptal edildi ise uygulamayı kapat
                Application.Exit();
            }
        }
    }
}