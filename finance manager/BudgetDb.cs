using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace finance_manager
{
  internal class BudgetDb
  {
    private string dbPath;
    private string connectionString;

    public BudgetDb()
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

        string createBudgetTable = @"
                    CREATE TABLE IF NOT EXISTS budget (
                        budget_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        description TEXT NOT NULL,
                        total_price REAL NOT NULL,
                        monthly_price REAL NOT NULL,
                        fk_userid INTEGER NOT NULL,
                        FOREIGN KEY(fk_userid) REFERENCES users(id) ON DELETE CASCADE
                    );";

        using (SQLiteCommand cmd = new SQLiteCommand(createBudgetTable, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void AddBudget(int userId, string description, double totalPrice, double monthlyPrice)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string insertSql = @"
                    INSERT INTO budget (description, total_price, monthly_price, fk_userid)
                    VALUES (@desc, @total, @monthly, @userId);";

        using (SQLiteCommand cmd = new SQLiteCommand(insertSql, conn))
        {
          cmd.Parameters.AddWithValue("@desc", description);
          cmd.Parameters.AddWithValue("@total", totalPrice);
          cmd.Parameters.AddWithValue("@monthly", monthlyPrice);
          cmd.Parameters.AddWithValue("@userId", userId);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public List<(int Id, string Description, double Total, double Monthly)> GetBudgetsForUser(int userId)
    {
      List<(int, string, double, double)> budgets = new();

      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string query = "SELECT budget_id, description, total_price, monthly_price FROM budget WHERE fk_userid = @userId";

        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@userId", userId);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int id = reader.GetInt32(0);
              string desc = reader.GetString(1);
              double total = reader.GetDouble(2);
              double monthly = reader.GetDouble(3);

              budgets.Add((id, desc, total, monthly));
            }
          }
        }
      }

      return budgets;
    }

    public void DeleteBudget(int budgetId)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string sql = "DELETE FROM budget WHERE budget_id = @id";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@id", budgetId);
          cmd.ExecuteNonQuery();
        }
      }
    }
  }
}
