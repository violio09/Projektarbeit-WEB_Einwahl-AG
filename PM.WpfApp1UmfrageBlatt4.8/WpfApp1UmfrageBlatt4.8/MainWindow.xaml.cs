using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient;

namespace UmfrageBlatt
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ausgabe;
        int schuhlerId;
        string vorname = "";
        string nachname = "";
        int klassenstufe;
        string benutzer = "";
        int erstwahl;
        int zweitwahl;
        int drittwahl;
        string passwort = "";

        public void BtnAusgeben_Click(object sender, RoutedEventArgs e)
        {
            schuhlerId = Convert.ToInt32(txtSchuhlerId.Text);
            vorname = txtVorname.Text;
            nachname = txtNachname.Text;
            klassenstufe = Convert.ToInt32(txtKlassenstufe.Text);
            benutzer = txtUsername.Text;
            passwort = txtPasswort.Text;
            erstwahl = Convert.ToInt32(cmbErstwahl.SelectedItem);
            zweitwahl = Convert.ToInt32(cmbZweitwahl.SelectedItem);
            drittwahl = Convert.ToInt32(cmbDrittwahl.SelectedItem);


            // Auswahl als String speichern
            string erstwahlAuswahl = cmbErstwahl.SelectedItem as string;
            string zweitwahlAuswahl = cmbZweitwahl.SelectedItem as string;
            string drittwahlAuswahl = cmbDrittwahl.SelectedItem as string;
            // Beispielhafte Ausgabe im Textfeld

            string ausgabe =
                $"Vorname: {vorname}\r\n" +
                $"Nachname: {nachname}\r\n" +
                $"Klassenstufe: {klassenstufe}\r\n" +
                $"Username: {benutzer}\r\n" +
                $"Passwort: {passwort}\r\n" +
                $"Erstwahl Nr.: {erstwahl}\r\n" +
                $"Zweitwahl Nr.: {zweitwahl}\r\n" +
                $"Drittwahl Nr.: {drittwahl}";

            txtAusgabe.Text = ausgabe;

        }

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                cmbErstwahl.Items.Add(i.ToString());
            }

            for (int j = 1; j <= 10; j++)
            {
                cmbZweitwahl.Items.Add(j.ToString());
            }

            for (int l = 1; l <= 10; l++)
            {
                cmbDrittwahl.Items.Add(l.ToString());
            }

            string server = "34.32.109.127";
            string port = "3306";
            string dbusername = "lehrer-access";
            string dbpassword = "dlEM43RTfB0ea?";
            string database = "einwahlkurs";

            string connectionString = $"SERVER={server};PORT={port};DATABASE={database};UID={dbusername};PASSWORD={dbpassword};";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT k_ID as ID, k_Name as Fachname FROM kurse;";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sb.Append(reader.GetName(i).PadRight(15));
                        sb.Append("| ");
                    }
                    sb.AppendLine();
                    sb.AppendLine(new string('-', reader.FieldCount * 17));

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            sb.Append(reader[i].ToString().PadRight(15));
                            sb.Append("| ");
                        }
                        sb.AppendLine();
                    }


                    TextBox1.Text = sb.ToString();
                }
            }
        }

        private void PushDBButton_Click(object sender, RoutedEventArgs e)
        {
            string server = "34.32.109.127";
            string port = "3306";
            string dbusername = "lehrer-access";
            string dbpassword = "dlEM43RTfB0ea?";
            string database = "einwahlkurs";

            string connectionString = $"SERVER={server};PORT={port};DATABASE={database};UID={dbusername};PASSWORD={dbpassword};";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO `schuhler` (`s_ID`, `s_Name`, `s_Vorname`, `s_Klasse`, `s_erstWahl`, `s_zweitWahl`, `s_drittWahl`, `s_Username`, `s_Password`) VALUES ({schuhlerId}, '{nachname}', '{vorname}',{klassenstufe},{erstwahl},{zweitwahl},{drittwahl}, '{benutzer}', '{passwort}');";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Daten wurden in die Datenbank eingetragen.");
        }

    }
}
