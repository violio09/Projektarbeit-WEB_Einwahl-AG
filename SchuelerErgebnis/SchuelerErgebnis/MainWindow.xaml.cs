using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SchuelerErgebnis
{
    public partial class MainWindow : Window
    {
        public class Schueler
        {
            public int ID { get; set; }
            public string Vorname { get; set; } 
            public string Name { get; set; } 
            public int ErstWahl { get; set; }
            public int ZweitWahl { get; set; }
            public int DrittWahl { get; set; }
        }

        public class Kurs
        {
            public int ID { get; set; }
            public string Name { get; set; } = "";
            public int MaxTeilnehmer { get; set; }
        }

        public class ZuteilungErgebnis
        {
            public string SchuelerName { get; set; }
            public string KursName { get; set; }
        }


        private string server = "34.32.109.127";
        private string port = "3306";
        private string dbusername = "lehrer-access";
        private string dbpassword = "dlEM43RTfB0ea?";
        private string database = "einwahlkurs";
        private string connectionString;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = $"SERVER={server};PORT={port};DATABASE={database};UID={dbusername};PASSWORD={dbpassword};";
        }

        private async void BTNAusgaben_Click(object sender, RoutedEventArgs e)
        {
            BTNAusgaben.IsEnabled = false;
            ProtokollListBox.Items.Clear();
            ErgebnisDataGrid.ItemsSource = null; 
            StatusTextBlock.Text = "Starte Zuteilungsprozess...";
            MainTabControl.SelectedIndex = 0;

            try
            {
                List<string> logNachrichten = await WeiseKurseZuAsync();
                foreach (var nachricht in logNachrichten)
                {
                    ProtokollListBox.Items.Add(nachricht);
                }

                StatusTextBlock.Text = "Zuteilung abgeschlossen. Lade finale Ergebnisse aus der DB...";

                List<ZuteilungErgebnis> finaleErgebnisse = await HoleFinaleZuteilungenAsync();
                ErgebnisDataGrid.ItemsSource = finaleErgebnisse;

                StatusTextBlock.Text = "Fertig! Ergebnisse wurden erfolgreich geladen.";
                MainTabControl.SelectedItem = ErgebnisTab; 
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Ein Fehler ist aufgetreten!";
                MessageBox.Show($"Fehler bei der Zuteilung: {ex.Message}", "Datenbankfehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BTNAusgaben.IsEnabled = true;
            }
        }

        private async Task<List<string>> WeiseKurseZuAsync()
        {
            var logNachrichten = new List<string>();
            List<Schueler> schuelerListe = await HoleSortierteSchuelerAsync();
            Dictionary<int, Kurs> kursInfos = await HoleKurskapazitaetenAsync();

            logNachrichten.Add("Starte Zuteilungsprozess...");

            if (schuelerListe.Count == 0)
            {
                logNachrichten.Add("Keine Schüler zur Zuteilung gefunden.");
                return logNachrichten;
            }

            var aktuelleTeilnehmer = new Dictionary<int, int>();
            foreach (var kursId in kursInfos.Keys)
            {
                aktuelleTeilnehmer[kursId] = 0;
            }

            var finaleZuteilungen = new List<Tuple<int, int>>();
            logNachrichten.Add($"Verarbeite {schuelerListe.Count} Schüler nach Anmeldezeitpunkt...");

            foreach (var schueler in schuelerListe)
            {
                bool zugewiesen = false;
                int zugewiesenerKurs = -1;
                string schuelerName = $"{schueler.Vorname} {schueler.Name}";

                if (VersucheZuweisung(schueler.ErstWahl, aktuelleTeilnehmer, kursInfos))
                {
                    zugewiesenerKurs = schueler.ErstWahl;
                    zugewiesen = true;
                    logNachrichten.Add($"Schüler '{schuelerName}' wurde Kurs '{kursInfos[zugewiesenerKurs].Name}' (Erstwahl) zugewiesen.");
                }
                else if (VersucheZuweisung(schueler.ZweitWahl, aktuelleTeilnehmer, kursInfos))
                {
                    zugewiesenerKurs = schueler.ZweitWahl;
                    zugewiesen = true;
                    logNachrichten.Add($"Schüler '{schuelerName}' wurde Kurs '{kursInfos[zugewiesenerKurs].Name}' (Zweitwahl) zugewiesen.");
                }
                else if (VersucheZuweisung(schueler.DrittWahl, aktuelleTeilnehmer, kursInfos))
                {
                    zugewiesenerKurs = schueler.DrittWahl;
                    zugewiesen = true;
                    logNachrichten.Add($"Schüler '{schuelerName}' wurde Kurs '{kursInfos[zugewiesenerKurs].Name}' (Drittwahl) zugewiesen.");
                }

                if (zugewiesen)
                {
                    aktuelleTeilnehmer[zugewiesenerKurs]++;
                    finaleZuteilungen.Add(new Tuple<int, int>(schueler.ID, zugewiesenerKurs));
                }
                else
                {
                    logNachrichten.Add($"WARNUNG: Schüler '{schuelerName}' (ID: {schueler.ID}) konnte keinem Kurs zugewiesen werden!");
                }
            }

            await SpeichereZuteilungenAsync(finaleZuteilungen);
            logNachrichten.Add("\nZuteilung abgeschlossen und in der Datenbank gespeichert.");
            return logNachrichten;
        }


        private bool VersucheZuweisung(int kursId, Dictionary<int, int> aktuelleTeilnehmer, Dictionary<int, Kurs> kursInfos)
        {
            return kursInfos.ContainsKey(kursId) &&
                   aktuelleTeilnehmer.ContainsKey(kursId) &&
                   aktuelleTeilnehmer[kursId] < kursInfos[kursId].MaxTeilnehmer;
        }

        private async Task<List<Schueler>> HoleSortierteSchuelerAsync()
        {
            var liste = new List<Schueler>();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT s_ID, s_Name, s_Vorname, s_erstWahl, s_zweitWahl, s_drittWahl FROM schuhler ORDER BY s_Timestamp ASC;", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        liste.Add(new Schueler
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("s_ID")),
                            Name = reader.GetString(reader.GetOrdinal("s_Name")),
                            Vorname = reader.GetString(reader.GetOrdinal("s_Vorname")),
                            ErstWahl = reader.GetInt32(reader.GetOrdinal("s_erstWahl")),
                            ZweitWahl = reader.GetInt32(reader.GetOrdinal("s_zweitWahl")),
                            DrittWahl = reader.GetInt32(reader.GetOrdinal("s_drittWahl"))
                        });
                    }
                }
            }
            return liste;
        }

        private async Task<List<ZuteilungErgebnis>> HoleFinaleZuteilungenAsync()
        {
            var ergebnisse = new List<ZuteilungErgebnis>();
            string query = @"
                SELECT 
                    CONCAT(s.s_Vorname, ' ', s.s_Name) AS SchuelerName,
                    k.k_Name AS KursName
                FROM zuteilungen z
                JOIN schuhler s ON z.schueler_ID = s.s_ID
                JOIN kurse k ON z.kurs_ID = k.k_ID
                ORDER BY k.k_Name, SchuelerName;";

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand(query, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ergebnisse.Add(new ZuteilungErgebnis
                        {
                            SchuelerName = reader.GetString(reader.GetOrdinal("SchuelerName")),
                            KursName = reader.GetString(reader.GetOrdinal("KursName")) 
                        });
                    }
                }
            }
            return ergebnisse;
        }

        private async Task<Dictionary<int, Kurs>> HoleKurskapazitaetenAsync()
        {
            var kurse = new Dictionary<int, Kurs>();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT k_ID, k_Name, k_MaxTeilnehmer FROM kurse;", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int kursId = reader.GetInt32(reader.GetOrdinal("k_ID"));
                        kurse[kursId] = new Kurs
                        {
                            ID = kursId,
                            Name = reader.GetString(reader.GetOrdinal("k_Name")),
                            MaxTeilnehmer = reader.GetInt32(reader.GetOrdinal("k_MaxTeilnehmer"))
                        };
                    }
                }
            }
            return kurse;
        }

        private async Task SpeichereZuteilungenAsync(List<Tuple<int, int>> zuteilungen)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var deleteCmd = new MySqlCommand("TRUNCATE TABLE zuteilungen;", connection, (MySqlTransaction)transaction);
                    await deleteCmd.ExecuteNonQueryAsync();

                    foreach (var zuteilung in zuteilungen)
                    {
                        var insertCmd = new MySqlCommand("INSERT INTO zuteilungen (schueler_ID, kurs_ID) VALUES (@schuelerID, @kursID);", connection, (MySqlTransaction)transaction);
                        insertCmd.Parameters.AddWithValue("@schuelerID", zuteilung.Item1);
                        insertCmd.Parameters.AddWithValue("@kursID", zuteilung.Item2);
                        await insertCmd.ExecuteNonQueryAsync();
                    }
                    await transaction.CommitAsync();
                }
            }
        }
    }
}