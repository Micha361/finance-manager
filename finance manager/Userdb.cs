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
    public static string LoggedInUser { get; set; } //Eine globale Variable um den angemeldeten Benutzer zu speichern
    public static int LoggedInUserId { get; private set; } // Variabel um UserId zu benutzen

    public Userdb()
    {
      
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
        string sql = "SELECT id FROM users WHERE username=@username AND password=@password";
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@username", username);
          cmd.Parameters.AddWithValue("@password", password);
          object result = cmd.ExecuteScalar();

          if (result != null)
          {
            LoggedInUser = username;
            LoggedInUserId = Convert.ToInt32(result); 

            
            Balancedb balanceDb = new Balancedb();
            double currentBalance = balanceDb.GetBalance(LoggedInUserId);

            
            string checkSql = "SELECT COUNT(*) FROM balance WHERE fk_userid = @userId";
            using (SQLiteCommand checkCmd = new SQLiteCommand(checkSql, conn))
            {
              checkCmd.Parameters.AddWithValue("@userId", LoggedInUserId);
              object countResult = checkCmd.ExecuteScalar();
              int count = Convert.ToInt32(countResult);
              if (count == 0)
              {
                balanceDb.CreateBalanceForUser(LoggedInUserId, 0); 
              }
            }

            return true;
          }
          else
          {
            return false; 
          }
        }
      }
    }

    // Passwort ändern
    public bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string checkSql = "SELECT id FROM users WHERE username=@username AND password=@oldPassword";
        using (SQLiteCommand checkCmd = new SQLiteCommand(checkSql, conn))
        {
          checkCmd.Parameters.AddWithValue("@username", username);
          checkCmd.Parameters.AddWithValue("@oldPassword", oldPassword);
          object result = checkCmd.ExecuteScalar();
          if (result == null)
            return false;
        }

        string updateSql = "UPDATE users SET password=@newPassword WHERE username=@username";
        using (SQLiteCommand updateCmd = new SQLiteCommand(updateSql, conn))
        {
          updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
          updateCmd.Parameters.AddWithValue("@username", username);
          return updateCmd.ExecuteNonQuery() > 0;
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
