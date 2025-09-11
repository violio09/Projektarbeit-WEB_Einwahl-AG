using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace LehrerCommands4._8
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String ausgabe;
        private void BTNmaxteilnehmer_Click(object sender, RoutedEventArgs e)
        {
            // Angenommen, maxteilnehmer ist ein TextBox-Steuerelement.
            string maxteilnehmer = "10"; //muss aus DB ausgelesen werden
            string maxTeilnehmerAGNr = maxteilnehmerAGNr.Text;
            txtAusgabe.Text = $"Maximale Teilnehmerzahl für AG Nr: {maxTeilnehmerAGNr} ist {maxteilnehmer}";
        }
        private void BTNtatsteilnehmer_Click(object sender, RoutedEventArgs e)
        {   
            string tatsteilnehmer = "4"; //muss aus DB ausgelesen werden
            string tatsTeilnehmerAGNr = tatsteilnehmerAGNr.Text;
            txtAusgabe.Text = $"Tatsächliche Teilnehmerzahl für AG Nr: {tatsTeilnehmerAGNr} ist {tatsteilnehmer}";
        }
        private void BTNlisteAGs_Click(object sender, RoutedEventArgs e)
        {
            string listeAGs = "Fußball, Schach, Theater, Kunst";
            txtAusgabe.Text = $"Liste der AGs: {listeAGs}";
        }
        private void BTNsusohnewahl_Click(object sender, RoutedEventArgs e)
        {
            string susohnewahl = "Max Mustermann, Erika Musterfrau";
            txtAusgabe.Text = $"Schüler ohne AG-Wahl: {susohnewahl}";
        }
        private void BTNwahlloeschen_Click(object sender, RoutedEventArgs e)
        {
            string wahlloeschen = "AG-Wahlen für Schüler xy wurden gelöscht.";
            txtAusgabe.Text = wahlloeschen;
        }
        private void BTNpwusername_Click(object sender, RoutedEventArgs e)
        {
            string pwusername = "Zeilen für PW und Username wurden erstellt.";
            txtAusgabe.Text = pwusername;
        }
        private void BTNpwuserloeschen_Click(object sender, RoutedEventArgs e)
        {
            string loeschenText = loeschen.Text;

            txtAusgabe.Text = $"Benutzer {loeschenText} wurde gelöscht.";
        }
        private void BTNcreateuser_Click(object sender, RoutedEventArgs e)
        {
            string erstellen = erstellenuser.Text;
            
            txtAusgabe.Text = $"Benutzername: {erstellen}\r\n";
        }
        private void BTNcreatepw_Click(object sender, RoutedEventArgs e)
        {
            string user = username.Text;
            string pw = password.Text;

            txtAusgabe.Text = $"Benutzername: {user}\r\nPasswort: {pw}\r\n";
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
