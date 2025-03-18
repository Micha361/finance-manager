using finance_manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace finance_manager
{

    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
    private int currentUserId;
    private Balancedb balancedb;
    public Window1()
    {
      InitializeComponent();
      balancedb = new Balancedb();

      if (!string.IsNullOrEmpty(Userdb.LoggedInUser))
      {
        welcomelbl.Content = $"Willkommen, {Userdb.LoggedInUser}!";
      }
      else
      {
        welcomelbl.Content = "Willkommen!";
      }

     
        currentUserId = Userdb.LoggedInUserId; // Benutzer-ID aus der Userdb holen
      

      LoadBalance();
    }

    private void transactionsbtn_Click(object sender, RoutedEventArgs e)
    {
      transactions transactionsApp = new transactions();
      transactionsApp.Show();

      this.Close();
    }

    private void budgetbtn_Click(object sender, RoutedEventArgs e)
    {
    budget budgetApp = new budget();
      budgetApp.Show();

      this.Close();
    }

    private void logoutbtn_Click_1(object sender, RoutedEventArgs e)
    {
      MainWindow LoginApp = new MainWindow();
      LoginApp.Show();

      this.Close();
    }

    private void accountbtn_Click(object sender, RoutedEventArgs e)
    {
      account accountApp = new account();
      accountApp.Show();

      this.Close();
    }
    private void LoadBalance()
    {

        double saldo = balancedb.GetBalance(currentUserId);
        saldolbl.Content = $"Your saldo stands at: {saldo:F2} CHF";

    }

  }
}
