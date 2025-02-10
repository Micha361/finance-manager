using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace finance_manager
{
  internal class Userdb
  {
    private string dbPath;
    private string connectionString;

    public Userdb()
    {
      // Datenbankpfad im lokalen AppData-Ordner speichern (Benutzerfreundlich)
      dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finance_manager.db");
      connectionString = $"Data Source={dbPath}; Version=3;";
      InitializeDatabase();
    }

    // Erstellt die Datenbank und die Tabelle, falls sie nicht existiert
    private void InitializeDatabase()
    {
      if (!File.Exists(dbPath))
      {
        SQLiteConnection.CreateFile(dbPath);
        Console.WriteLine("SQLite-Datenbank wurde erstellt.");
      }

      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();
        string sql = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL
                    )";
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    // Benutzer hinzufügen
    public bool InsertUser(string username, string password)
    {
      try
      {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
          conn.Open();
          string sql = "INSERT INTO users (username, password) VALUES (@username, @password)";
          using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
          {
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.ExecuteNonQuery();
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Fehler beim Einfügen: " + ex.Message);
        return false;
      }
    }

    // Alle Benutzer abrufen
    public DataTable GetUsers()
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();
        string sql = "SELECT * FROM users";
        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
        {
          DataTable dt = new DataTable();
          adapter.Fill(dt);
          return dt;
        }
      }
    }

    // Benutzer-Login prüfen
    public bool CheckLogin(string username, string password)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();
        string sql = "SELECT COUNT(*) FROM users WHERE username=@username AND password=@password";
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@username", username);
          cmd.Parameters.AddWithValue("@password", password);
          int count = Convert.ToInt32(cmd.ExecuteScalar());
          return count > 0;
        }
      }
    }

    // Benutzer löschen
    public bool DeleteUser(int userId)
    {
      try
      {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
          conn.Open();
          string sql = "DELETE FROM users WHERE id = @id";
          using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
          {
            cmd.Parameters.AddWithValue("@id", userId);
            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Fehler beim Löschen: " + ex.Message);
        return false;
      }
    }
  }
}
