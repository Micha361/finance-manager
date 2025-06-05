using System;
using System.Windows;
using System.Windows.Controls;

namespace finance_manager
{
  /// <summary>
  /// Interaktionslogik für addtransaction.xaml
  /// </summary>
  public partial class addBudget : Window
  {
    public addBudget()
    {
      InitializeComponent();
    }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      transactions transactionsApp = new transactions();
      transactionsApp.Show();
      this.Close();
    }

    private void addBtn_Click(object sender, RoutedEventArgs e)
    {
      string amountText = amountBox.Text;
      string description = descriptionBox.Text;
      DateTime? date = datePicker.SelectedDate;
      string type = (typeBox.SelectedItem as ComboBoxItem)?.Content.ToString();

      if (string.IsNullOrWhiteSpace(amountText) || string.IsNullOrWhiteSpace(description) || date == null || string.IsNullOrWhiteSpace(type))
      {
        MessageBox.Show("Bitte alle Felder ausfüllen.");
        return;
      }

      if (!double.TryParse(amountText, out double amount))
      {
        MessageBox.Show("Ungültiger Betrag.");
        return;
      }

      int userId = Userdb.LoggedInUserId;

      TransactionDb db = new TransactionDb();
      db.AddTransaction(userId, amount, type, description, date.Value);

      MessageBox.Show("Transaktion erfolgreich hinzugefügt!");


      transactions transactionsApp = new transactions();
      transactionsApp.Show();


      this.Close();
    }
  }
}
