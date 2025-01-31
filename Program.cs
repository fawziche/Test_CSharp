using System;
using MySql.Data.MySqlClient;

public class Principale
{
    public static void Main(string[] args)
    {
        try
        {
            string host = "localhost";
            string BDD = "societe";
            string user = "root";
            string pass = "Saadia04";

            // Ouverture de connexion
            string connectionString = $"server={host};database={BDD};user={user};password={pass};";
            MySqlConnection conn = new MySqlConnection (connectionString);
            conn.Open();
            
            // Insertion
            string query = "INSERT INTO tSociete (Nom) VALUES ('EDF')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            int rowsAffected = cmd.ExecuteNonQuery();

            // Lecture
            query = "SELECT Id, Nom FROM tSociete ORDER BY Nom ASC";
            cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Id = {reader["Id"]} / Nom = {reader["Nom"]}");
            }

            // Fermeture d'une BDD
            conn.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}