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

namespace UmfrageBlatt
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ausgabe;

        private void BtnAusgeben_Click(object sender, RoutedEventArgs e)
        {
            string vorname = txtVorname.Text;
            string nachname = txtNachname.Text;
            string klassenstufe = txtKlassenstufe.Text;
            string username = txtUsername.Text;
            string erstwahlNr = txtErstwahlNr.Text;
            string zweitwahlNr = txtZweitwahlNr.Text;
            string drittwahlNr = txtDrittwahlNr.Text;
            string passwort = password.Password;

            string ausgabe =
                $"Vorname: {vorname}\r\n" +
                $"Nachname: {nachname}\r\n" +
                $"Klassenstufe: {klassenstufe}\r\n" +
                $"Username: {username}\r\n" +
                $"Passwort: {passwort}\r\n" +
                $"Erstwahl Nr.: {erstwahlNr}\r\n" +
                $"Zweitwahl Nr.: {zweitwahlNr}\r\n" +
                $"Drittwahl Nr.: {drittwahlNr}";

            txtAusgabe.Text = ausgabe;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = string.IsNullOrEmpty(password.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
