using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;

namespace finance_manager
{
  public partial class transactions : Window
  {
    public transactions()
    {
      InitializeComponent();
      LoadTransactions(); // Daten laden beim Start
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
  }
}