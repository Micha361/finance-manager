using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Data.SQLite;
using System.IO;


namespace finance_manager
{
  public partial class transactions : Window
  {
    public transactions()
    {
      InitializeComponent();
      LoadTransactions(); 
    }

    private void LoadTransactions()
    {
      int userId = Userdb.LoggedInUserId;

      TransactionDb db = new TransactionDb();
      var transactions = db.GetTransactionsSortedByDate(userId);

      transactionGrid.ItemsSource = transactions.Select(t => new
      {
        Date = t.Date.ToString("dd.MM.yyyy"),
        t.Category,
        Amount = $"{t.Amount:F2} CHF",
        t.Description
      }).ToList();
    }


    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      Window1 mainApp = new Window1();
      mainApp.Show();
      this.Close();
    }

    private void addnewtransactionbtn_Click(object sender, RoutedEventArgs e)
    {
      addtransaction addnewtransactionApp = new addtransaction();
      addnewtransactionApp.Show();
      this.Close();
    }

    private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
    {
      Button deleteButton = sender as Button;
      dynamic selectedTransaction = deleteButton.DataContext;

      string desc = selectedTransaction.Description;
      string category = selectedTransaction.Category;
      string dateString = selectedTransaction.Date;

      TransactionDb transactionDb = new TransactionDb();
      int userId = Userdb.LoggedInUserId;

      var allTransactions = transactionDb.GetTransactionsSortedByDate(userId);
      var transactionToDelete = allTransactions.FirstOrDefault(t =>
        t.Description == desc &&
        t.Category == category &&
        t.Date.ToString("dd.MM.yyyy") == dateString
      );

      if (transactionToDelete.Equals(default((int, int, double, string, string, DateTime))))
      {
        MessageBox.Show("Transaktion konnte nicht gefunden werden.");
        return;
      }

      
      Balancedb balanceDb = new Balancedb();
      double currentBalance = balanceDb.GetBalance(userId);

      double newBalance = category.ToLower() == "income"
          ? currentBalance - transactionToDelete.Amount
          : currentBalance + transactionToDelete.Amount;

      balanceDb.UpdateBalance(userId, newBalance);

      
      string deleteSql = "DELETE FROM transactions WHERE transaction_id = @id";
      using (var conn = new SQLiteConnection("Data Source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finance_manager.db")))
      {
        conn.Open();
        using (var cmd = new SQLiteCommand(deleteSql, conn))
        {
          cmd.Parameters.AddWithValue("@id", transactionToDelete.Id);
          cmd.ExecuteNonQuery();
        }
      }

      MessageBox.Show("Transaktion wurde gelöscht und Saldo aktualisiert.");
      LoadTransactions();
    }


  }
}