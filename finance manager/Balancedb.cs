using System;
using System.Data.SQLite;
using System.IO;

namespace finance_manager
{
  internal class Balancedb
  {
    private string dbPath;
    private string connectionString;

    public Balancedb()
    {
      // Datenbankpfad im lokalen AppData-Ordner speichern (anscheinend benutzerfreundlicher)
      dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finance_manager.db");
      connectionString = $"Data Source={dbPath}; Version=3;";

      // create table if not exist
      InitializeDatabase();
    }

    /// <summary>
    /// Erstellt die Tabelle "balance", falls sie noch nicht existiert.
    /// Jeder Benutzer hat 
    /// </summary>
    private void InitializeDatabase()
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string createBalanceTable = @"
                    CREATE TABLE IF NOT EXISTS balance (
                        balanceid INTEGER PRIMARY KEY AUTOINCREMENT, 
                        balance REAL NOT NULL DEFAULT 0,            
                        fk_userid INTEGER NOT NULL UNIQUE,           
                        FOREIGN KEY(fk_userid) REFERENCES users(id) ON DELETE CASCADE 
                    )";

        using (SQLiteCommand cmd = new SQLiteCommand(createBalanceTable, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    /// <summary>
    /// Erstellt eine Balance für einen Benutzer mit einem Startbetrag.
    /// </summary>
    /// <param name="userId">fk userid</param>
    /// <param name="startBalance">Startbetrag für das Konto (Standard: 0)</param>
    public void CreateBalanceForUser(int userId, double startBalance = 0)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string sql = "INSERT INTO balance (balance, fk_userid) VALUES (@balance, @userId)";
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@balance", startBalance);
          cmd.Parameters.AddWithValue("@userId", userId);
          cmd.ExecuteNonQuery();
        }
      }
    }

    /// <summary>
    /// Gibt den Kontostand an
    /// </summary>
    /// <returns>Kontostand als Double</returns>
    public double GetBalance(int userId)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string sql = "SELECT balance FROM balance WHERE fk_userid = @userId";
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@userId", userId);
          object result = cmd.ExecuteScalar();

          return result != null ? Convert.ToDouble(result) : 0; // Falls kein Eintrag existiert, wird 0 zurückgegeben
        }
      }
    }

    /// <summary>
    /// Aktualisiert den Kontostand eines Benutzers.
    /// </summary>
    /// <param name="userId">ID des Benutzers</param>
    /// <param name="newBalance">Neuer Kontostand</param>
    public void UpdateBalance(int userId, double newBalance)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string sql = "UPDATE balance SET balance = @newBalance WHERE fk_userid = @userId";
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@newBalance", newBalance);
          cmd.Parameters.AddWithValue("@userId", userId);
          cmd.ExecuteNonQuery();
        }
      }
    }
  }
}
