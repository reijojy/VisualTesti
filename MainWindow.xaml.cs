using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Globalization;

namespace AlyTalo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public struct thermoLokiStruct
    {
        public int Lämpö { set; get; }
        public String Aika { set; get; }
    }
    public partial class MainWindow : Window
    {
        Ligths keittioLights = new Ligths();
        Ligths olohuoneLights = new Ligths();
        AsuntoLampo  asuntoLampo = new AsuntoLampo();
        Sauna sauna = new Sauna(18);
        private DispatcherTimer dispatcherTimer;
        private CultureInfo cultureInfo = new CultureInfo("fi-FI");

        public MainWindow()
        {
            InitializeComponent();
            //int val = asuntoLampo.
            lblNykLampo.Content = String.Format("Lämpötila nyt: {0}°", 18);
            lblSaunaLampo.Content = String.Format("Lämpötila nyt: {0}°", 18);
            // Kokeillaanpa astemerkkiä ° eli alt paina ja 0176
            initDGTaulu();


        }

        private void btnAsetaKeittio_Click(object sender, RoutedEventArgs e)
        {
            if (keittioLights.Status)
            {
                keittioLights.Level = 0;
                keittioLights.Status = false;
                btnAsetaKeittio.Content = "Päälle";
                lblKeittioValo.Content = "0%";
                sldKeittio.Value = 0;
                sldKeittio.IsEnabled = false;
            }
            else
            {
                btnAsetaKeittio.Content = "Pois";
                keittioLights.Status = true;
                sldKeittio.IsEnabled = true;
            }

        }
        private void BtnAsetaOloHuone_Click(object sender, RoutedEventArgs e)
        {
            if (olohuoneLights.Status)
            {
                olohuoneLights.Level = 0;
                olohuoneLights.Status = false;
                btnAsetaOloHuone.Content = "Päälle";
                lblOloHuonoValo.Content = "0%";
                sldOloHuone.Value = 0;
                sldOloHuone.IsEnabled = false;
            }
            else
            {
                btnAsetaOloHuone.Content = "Pois";
                olohuoneLights.Status = true;
                sldOloHuone.IsEnabled = true;
            }
        }
        private void SldKeittio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            keittioLights.Level = Convert.ToInt32(e.NewValue);
            string msg = String.Format("{0}%", keittioLights.Level);
            this.lblKeittioValo.Content = msg;
        }

        private void SldOloHuone_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            olohuoneLights.Level = Convert.ToInt32(e.NewValue);
            this.lblOloHuonoValo.Content = String.Format("{0}%", olohuoneLights.Level);
        }
        private Boolean isDigits(string txt)
        {
            if (txt.Length == 0) return true;
            return Regex.IsMatch(txt, "^[0-9]+$");
        }

        private void txtUusiLampoValue_changed(object sender, TextChangedEventArgs e)
        {
            if (txtUusiLampo.Text.Length == 0)
            {
                btnAsetaUusiLampo.IsEnabled = false;
                return;
            }
            if (isDigits(txtUusiLampo.Text)) {
                btnAsetaUusiLampo.IsEnabled = true;
                return;
            }
            btnAsetaUusiLampo.IsEnabled = false;
            MessageBox.Show("Vain numeroita");
        }
        private void txtSaunaLampoValue_changed(object sender, TextChangedEventArgs e)
        {
            if (txtSaunaLampo.Text.Length == 0)
            {
                btnAsetaSauna.IsEnabled = false;
                return;
            }
            if (isDigits(txtSaunaLampo.Text))
            {
                btnAsetaSauna.IsEnabled = true;
                return;
            }
            btnAsetaSauna.IsEnabled = false;
            MessageBox.Show("Vain numeroita");
        }
        private void btnAsetaUusiLampo_Click(object sender, RoutedEventArgs e)
        {
            if (txtUusiLampo.Text.Length == 0) return;
            int lampo = int.Parse(txtUusiLampo.Text);
            if(!asuntoLampo.tarkistaTavoiteLammpo(lampo))
            {
                MessageBox.Show("Kohtuus lämpötiloissa");
                txtUusiLampo.Focus();
                return;
            }
            if (asuntoLampo.getAsetettuLampo() == lampo)
            {
                MessageBox.Show("Miksi yrität vaihtaa saman lämpötilan pöljä");
                return;
            }
            
            asuntoLampo.setAsetettuLampo(lampo);
            lblEdellinenLampo.Content = String.Format("Edellinen lämpötila: {0}°", asuntoLampo.getEdellinenLampo());
            lblNykLampo.Content = String.Format("Lämpötila nyt: {0}°", asuntoLampo.getAsetettuLampo());
            // Tyhjnnetään syöttökenttä ja disabloidaan painike
            txtUusiLampo.Text = "";
            btnAsetaUusiLampo.IsEnabled = false;
            // Lokin kirjoittaminen
            dgThermoLoki.Items.Add(new thermoLokiStruct{ Lämpö = lampo, Aika = DateTime.Now.ToString(cultureInfo) });
        }
        private void dispatcherTimer_Tick(Object Sender, EventArgs e)
        {
            if (sauna.isLammitys) { 
                if (sauna.Lampo < sauna.TavoiteLampo) { sauna.Lampo += 1; }
                else { dispatcherTimer.Stop(); }
            } else
            {
                if (sauna.Lampo > asuntoLampo.getAsetettuLampo()) { sauna.Lampo -= 1; }
                else { dispatcherTimer.Stop(); }
            }
            lblSaunaLampo.Content = String.Format("Lämpötila nyt: {0}°", sauna.Lampo);
        }

        private void BtnAsetaSauna_Click(object sender, RoutedEventArgs e)
        {
            // Numeerisuus tutkittu jo
            int lampo = int.Parse(txtSaunaLampo.Text);
            if (lampo < asuntoLampo.getAsetettuLampo() || lampo > 180)
            {
                MessageBox.Show("Järkeä niihin löylyihin, senkin PÖLJÄ");
                txtSaunaLampo.Focus();
                return;
            }
            if (dispatcherTimer == null) { initDispatcherTimer(); }
    
            if (sauna.isLammitys) {
                // Otetaan lämmitys pois päältä
                btnAsetaSauna.Content = "Päälle";
                txtSaunaLampo.Text = "";
                btnAsetaSauna.IsEnabled = false;
                sauna.TavoiteLampo = 0;
                sauna.isLammitys = false;
            }
            else {
                // Laitetaan päälle
                btnAsetaSauna.Content = "Sammuta";
                sauna.TavoiteLampo = lampo;
                sauna.isLammitys = true;
            }

            dispatcherTimer.Start();
        }
        private void initDispatcherTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }
        private void initDGTaulu()
        {
            String[] columns = { "Lämpö", "Aika" };
            foreach (String s in columns)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Binding = new Binding(s);
                textColumn.Header = s;
                dgThermoLoki.Columns.Add(textColumn);
            }
        }
    }
}
