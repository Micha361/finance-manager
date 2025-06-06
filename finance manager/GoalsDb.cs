using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace finance_manager
{
  public class Goal
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public double Target { get; set; }
    public double Saved { get; set; }
    public double Needed => Target - Saved;
  }

  public class GoalDb
  {
    private string connectionString = "Data Source=finance.db";

    public GoalDb()
    {
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
                    needed_amount REAL NOT NULL,
                    fk_userid INTEGER NOT NULL,
                    FOREIGN KEY(fk_userid) REFERENCES users(id) ON DELETE CASCADE
                );";

        using (SQLiteCommand command = new SQLiteCommand(createTable, conn))
        {
          command.ExecuteNonQuery();
        }
      }
    }

    public void AddGoal(string title, double targetAmount, int userId)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string insertSql = @"
                INSERT INTO goals (title, target_amount, saved_amount, needed_amount, fk_userid)
                VALUES (@title, @target, 0, @target, @userId);";

        using (SQLiteCommand cmd = new SQLiteCommand(insertSql, conn))
        {
          cmd.Parameters.AddWithValue("@title", title);
          cmd.Parameters.AddWithValue("@target", targetAmount);
          cmd.Parameters.AddWithValue("@userId", userId);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public List<Goal> GetGoalsForUser(int userId)
    {
      List<Goal> goals = new List<Goal>();

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
              goals.Add(new Goal
              {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Target = reader.GetDouble(2),
                Saved = reader.GetDouble(3)
              });
            }
          }
        }
      }

      return goals;
    }

    public Goal GetGoalById(int goalId)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();
        string query = "SELECT goal_id, title, target_amount, saved_amount FROM goals WHERE goal_id = @id";

        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", goalId);

          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return new Goal
              {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Target = reader.GetDouble(2),
                Saved = reader.GetDouble(3)
              };
            }
          }
        }
      }

      return null;
    }

    public void UpdateSavedAmount(int goalId, double newSavedAmount)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string updateSql = @"
                UPDATE goals 
                SET saved_amount = @saved, 
                    needed_amount = target_amount - @saved
                WHERE goal_id = @id";

        using (SQLiteCommand command = new SQLiteCommand(updateSql, conn))
        {
          command.Parameters.AddWithValue("@saved", newSavedAmount);
          command.Parameters.AddWithValue("@id", goalId);
          command.ExecuteNonQuery();
        }
      }
    }

    public void UpdateGoalTitle(int goalId, string newTitle)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();
        string updateSql = "UPDATE goals SET title = @title WHERE goal_id = @id";

        using (SQLiteCommand cmd = new SQLiteCommand(updateSql, conn))
        {
          cmd.Parameters.AddWithValue("@title", newTitle);
          cmd.Parameters.AddWithValue("@id", goalId);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void DeleteGoal(int goalId)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();
        string deleteSql = "DELETE FROM goals WHERE goal_id = @id";

        using (SQLiteCommand cmd = new SQLiteCommand(deleteSql, conn))
        {
          cmd.Parameters.AddWithValue("@id", goalId);
          cmd.ExecuteNonQuery();
        }
      }
    }
  }
}
