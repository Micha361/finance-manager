using System;
using System.Data.SQLite;
using System.IO;

namespace finance_manager
{
    public class TransactionDb
    {
        private string dbPath;
        private string connectionString;

        public TransactionDb()
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

                string createTransactionsTable = @"
                    CREATE TABLE IF NOT EXISTS transactions (
                        transaction_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        user_id INTEGER NOT NULL,
                        amount REAL NOT NULL,
                        category TEXT NOT NULL,
                        description TEXT,
                        date TEXT NOT NULL,
                        FOREIGN KEY(user_id) REFERENCES users(id) ON DELETE CASCADE
                    );";

                using (SQLiteCommand cmd = new SQLiteCommand(createTransactionsTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

    public void AddTransaction(int userId, double amount, string category, string description, DateTime date)
    {
      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string insertTransaction = @"
            INSERT INTO transactions (user_id, amount, category, description, date)
            VALUES (@userId, @amount, @category, @description, @date);";

        using (SQLiteCommand cmd = new SQLiteCommand(insertTransaction, conn))
        {
          cmd.Parameters.AddWithValue("@userId", userId);
          cmd.Parameters.AddWithValue("@amount", amount);
          cmd.Parameters.AddWithValue("@category", category);
          cmd.Parameters.AddWithValue("@description", description);
          cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd HH:mm:ss"));
          cmd.ExecuteNonQuery();
        }

        
        Balancedb balanceDb = new Balancedb();
        double currentBalance = balanceDb.GetBalance(userId);

       
        double newBalance = category.ToLower() == "income"
            ? currentBalance + amount
            : currentBalance - amount;

        balanceDb.UpdateBalance(userId, newBalance);
      }
    }
    public List<(int Id, int UserId, double Amount, string Category, string Description, DateTime Date)> GetTransactionsSortedByDate(int userId)
    {
      List<(int, int, double, string, string, DateTime)> transactions = new();

      using (SQLiteConnection conn = new SQLiteConnection(connectionString))
      {
        conn.Open();

        string query = @"
            SELECT transaction_id, user_id, amount, category, description, date 
            FROM transactions
            WHERE user_id = @userId
            ORDER BY date DESC;";

        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@userId", userId);
          using (SQLiteDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int id = reader.GetInt32(0);
              int uId = reader.GetInt32(1);
              double amount = reader.GetDouble(2);
              string category = reader.GetString(3);
              string description = reader.GetString(4);
              DateTime date = DateTime.Parse(reader.GetString(5));

              transactions.Add((id, uId, amount, category, description, date));
            }
          }
        }
      }

      return transactions;
    }

      
    
  }
}
