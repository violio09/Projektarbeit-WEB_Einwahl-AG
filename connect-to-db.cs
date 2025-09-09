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

                string query = "SELECT * FROM kurse;";
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
}
