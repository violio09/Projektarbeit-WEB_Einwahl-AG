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

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
                connection.Open();

                string query = "INSERT INTO `kurse` (`k_ID`, `k_Name`, `k_MaxTeilnehmer`) VALUES (8, 'PASCAL Programmierung', 10);";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                
                Console.WriteLine(cmd.ExecuteNonQuery());
        }
    }
}
