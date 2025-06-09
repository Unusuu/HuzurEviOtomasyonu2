using System;
using System.Windows.Forms;

namespace HuzurEviOtomasyonu
{
    public partial class MainForm : Form
    {
        private Form activeForm = null;
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuYaslilar;
        private ToolStripMenuItem menuPersonel;
        private ToolStripMenuItem menuIlacTakip;
        private ToolStripMenuItem menuYemekListesi;
        private ToolStripMenuItem menuEtkinlikler;
        private ToolStripMenuItem menuOdaYonetimi;
        private ToolStripMenuItem menuZiyaretci;
        private Panel panelChildForm;

        public MainForm()
        {
            InitializeComponent();
            YetkileriKontrolEt();
        }

        private void InitializeComponent()
        {
            this.Text = "Huzurevi Otomasyonu";
            this.WindowState = FormWindowState.Maximized;

            // Panel oluştur
            panelChildForm = new Panel();
            panelChildForm.Dock = DockStyle.Fill;
            this.Controls.Add(panelChildForm);

            // MenuStrip oluştur
            menuStrip = new MenuStrip();
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Menü öğelerini oluştur
            menuYaslilar = new ToolStripMenuItem("Yaşlı İşlemleri");
            menuPersonel = new ToolStripMenuItem("Personel İşlemleri");
            menuIlacTakip = new ToolStripMenuItem("İlaç Takip");
            menuYemekListesi = new ToolStripMenuItem("Yemek Listesi");
            menuEtkinlikler = new ToolStripMenuItem("Etkinlikler");
            menuOdaYonetimi = new ToolStripMenuItem("Oda Yönetimi");
            menuZiyaretci = new ToolStripMenuItem("Ziyaretçi İşlemleri");

            // Menü öğelerini MenuStrip'e ekle
            menuStrip.Items.AddRange(new ToolStripItem[] {
                menuYaslilar,
                menuPersonel,
                menuIlacTakip,
                menuYemekListesi,
                menuEtkinlikler,
                menuOdaYonetimi,
                menuZiyaretci
            });

            // Click eventlerini ekle
            menuYaslilar.Click += new EventHandler(menuYaslilar_Click);
            menuPersonel.Click += new EventHandler(menuPersonel_Click);
            menuIlacTakip.Click += new EventHandler(menuIlacTakip_Click);
            menuYemekListesi.Click += new EventHandler(menuYemekListesi_Click);
            menuEtkinlikler.Click += new EventHandler(menuEtkinlikler_Click);
            menuOdaYonetimi.Click += new EventHandler(menuOdaYonetimi_Click);
            menuZiyaretci.Click += new EventHandler(menuZiyaretci_Click);
        }

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void menuYaslilar_Click(object sender, EventArgs e)
        {
            openChildForm(new YasliIslemleriForm());
        }

        private void menuPersonel_Click(object sender, EventArgs e)
        {
            openChildForm(new PersonelListesiForm());
        }

        private void menuIlacTakip_Click(object sender, EventArgs e)
        {
            openChildForm(new IlacTakipListesiForm());
        }

        private void menuYemekListesi_Click(object sender, EventArgs e)
        {
            openChildForm(new YemekListeleriGoruntuleForm());
        }

        private void menuEtkinlikler_Click(object sender, EventArgs e)
        {
            openChildForm(new EtkinlikListesiForm());
        }

        private void menuOdaYonetimi_Click(object sender, EventArgs e)
        {
            openChildForm(new OdaYonetimForm());
        }

        private void menuZiyaretci_Click(object sender, EventArgs e)
        {
            openChildForm(new ZiyaretciForm());
        }

        private void YetkileriKontrolEt()
        {
            if (Program.YetkiSeviyesi != 2) // Admin değilse
            {
                menuPersonel.Visible = false;
                // Diğer yetki kısıtlamaları buraya eklenebilir
            }
        }
    }
}