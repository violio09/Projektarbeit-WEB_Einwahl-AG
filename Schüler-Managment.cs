using MySql.Data.MySqlClient;
using System;

class Program
{
    static void Main(string[] args)
    {
        string server   = "SERVER-IP";
        string port     = "SERVER-PORT";
        string username = "USERNAME";
        string password = "PASSWORD";
        string database = "DB"; 

        string connectionString = $"SERVER={server};PORT={port};DATABASE={database};UID={username};PASSWORD={password};";

        Console.WriteLine("Was wollen sie tun?");
        Console.WriteLine("1. Alle Schüler einsehen");
        Console.WriteLine("2. Schüler Attribute ändern");
        Console.WriteLine("3. Schüler hinzufügen");
        Console.WriteLine("4. Schüler löschen");
        Console.Write("Eingabe: ");
        string input = Console.ReadLine();

        if(input == "1")
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                    connection.Open();

                    string query = "SELECT * FROM schuhler;";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader[i]} ");
                            }
                            Console.WriteLine();
                        }
                    }
            }
        }
        else if(input == "2")
        {
            Console.Write("Geben sie die Schüler ID ein die sie ändern wollen: ");
            string schuhlerId = Console.ReadLine();
            Console.Write("Geben sie den neuen Namen ein: ");
            string neuerName = Console.ReadLine();
            Console.Write("Geben sie die neue Vorname ein: ");
            string neuerVorname = Console.ReadLine();
            Console.Write("Geben sie die neue Klasse ein: ");
            string neueKlasse = Console.ReadLine();
            Console.Write("Geben sie das neue Passwort ein: ");
            string neuesPasswort = Console.ReadLine();

            Console.WriteLine("Schüler wird geändert...");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"UPDATE `schuhler` SET s_Name = '{neuerName}', s_Vorname = '{neuerVorname}', s_Klasse = '{neueKlasse}', s_Passwort = '{neuesPasswort}' WHERE s_ID = {schuhlerId};";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                Console.WriteLine("Finished with EXEC Code: " + cmd.ExecuteNonQuery());
            }
        }

        else if(input == "3")
        {
            Console.WriteLine("Nenne die Schüler ID: ");
            string schuhlerId = Console.ReadLine();
            Console.WriteLine("Nenne den Schüler Namen: ");
            string schuhlerName = Console.ReadLine();
            Console.WriteLine("Nenne den Schüler Vornamen: ");
            string schuhlerVorname = Console.ReadLine();
            Console.WriteLine("Nenne die Schüler Klasse: ");
            string schuhlerKlasse = Console.ReadLine();
            Console.WriteLine("Nenne das Schüler Passwort: ");
            string schuhlerPasswort = Console.ReadLine();

            Console.WriteLine("Schüler wird erstellt...");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO `schuhler` (`s_ID`, `s_Name`, `s_Vorname`, `s_Klasse`, `s_Password`) VALUES ({schuhlerId}, '{schuhlerName}', '{schuhlerVorname}', '{schuhlerKlasse}', '{schuhlerPasswort}');";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                Console.WriteLine("Finished with EXEC Code: " + cmd.ExecuteNonQuery());
            }
        }
        else if(input == "4")
        {
            Console.Write("Geben sie die Schüler ID ein die sie löschen wollen: ");
            string schuhlerId = Console.ReadLine();

            Console.WriteLine("Schüler wird gelöscht...");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"DELETE FROM `schuhler` WHERE s_ID = {schuhlerId};";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                Console.WriteLine("Finished with EXEC Code: " + cmd.ExecuteNonQuery());
            }
        }
        else
        {
            Console.WriteLine("Ungültige Eingabe.");
        }
       

    }
}
