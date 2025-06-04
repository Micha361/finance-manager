using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace finance_manager
{
  internal class GoalDb
  {
    private string dbPath;
    private string connectionString;

    public GoalDb()
    {
      dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finance_manager.db");
      connectionString = $"Data Source={dbPath}; Version=3;";
      InitializeDatabase();
    }

    private void InitializeDatabase()
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string createTable = @"
                    CREATE TABLE IF NOT EXISTS goals (
                        goal_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL,
                        target_amount REAL NOT NULL,
                        saved_amount REAL NOT NULL DEFAULT 0,
                        fk_userid INTEGER NOT NULL,
                        FOREIGN KEY(fk_userid) REFERENCES users(id) ON DELETE CASCADE
                    );";

        using (SQLiteCommand cmd = new SQLiteCommand(createTable, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void AddGoal(int userId, string title, double targetAmount)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string insertSql = @"
                    INSERT INTO goals (title, target_amount, saved_amount, fk_userid)
                    VALUES (@title, @target, 0, @userId);";

        using (SQLiteCommand cmd = new SQLiteCommand(insertSql, conn))
        {
          cmd.Parameters.AddWithValue("@title", title);
          cmd.Parameters.AddWithValue("@target", targetAmount);
          cmd.Parameters.AddWithValue("@userId", userId);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void UpdateSavedAmount(int goalId, double newSavedAmount)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string sql = "UPDATE goals SET saved_amount = @saved WHERE goal_id = @id";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@saved", newSavedAmount);
          cmd.Parameters.AddWithValue("@id", goalId);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public List<(int Id, string Title, double Target, double Saved)> GetGoalsForUser(int userId)
    {
      List<(int, string, double, double)> goals = new();

      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string query = "SELECT goal_id, title, target_amount, saved_amount FROM goals WHERE fk_userid = @userId";

        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@userId", userId);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int id = reader.GetInt32(0);
              string title = reader.GetString(1);
              double target = reader.GetDouble(2);
              double saved = reader.GetDouble(3);

              goals.Add((id, title, target, saved));
            }
          }
        }
      }

      return goals;
    }

    public void DeleteGoal(int goalId)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string sql = "DELETE FROM goals WHERE goal_id = @id";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", goalId);
          cmd.ExecuteNonQuery();
        }
      }
    }
  }
}
