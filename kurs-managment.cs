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
        Console.WriteLine("1: Kurs erstellen");
        Console.WriteLine("2: Kurs löschen");
        Console.Write("Eingabe: ");
        string input = Console.ReadLine();

        if(input == "1")
        {
            Console.WriteLine("Nenne die Kurs ID: ");
            string kursId = Console.ReadLine();
            Console.WriteLine("Nenne den Kurs Namen: ");
            string kursName = Console.ReadLine();
            Console.WriteLine("Nenne die maximale Teilnehmerzahl: ");
            string kursMaxTeilnehmer = Console.ReadLine();

            Console.WriteLine("Kurs wird erstellt...");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO `kurse` (`k_ID`, `k_Name`, `k_MaxTeilnehmer`) VALUES ({kursId}, '{kursName}', {kursMaxTeilnehmer});";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                Console.WriteLine("Finished with EXEC Code: " + cmd.ExecuteNonQuery());
            }
        }
        else if(input == "2")
        {
            Console.Write("Geben sie die Kurs ID ein die sie löschen wollen: ");
            string kurswahl = Console.ReadLine();
            
            Console.WriteLine("Kurs wird gelöscht...");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"DELETE FROM `kurse` WHERE k_ID = {kurswahl};";
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
