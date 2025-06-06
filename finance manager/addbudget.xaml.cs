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
using System.Globalization;
using finance_manager;

namespace finance_manager
{
    /// <summary>
    /// Interaktionslogik für addbudget.xaml
    /// </summary>
    public partial class addbudget : Window
    {
        public addbudget()
        {
            InitializeComponent();
        }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      Window1 mainApp = new Window1();
      mainApp.Show();
      this.Close();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      string category = CategoryBox.Text;
      string amountText = AmountBox.Text;

      if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(amountText))
      {
        MessageBox.Show("Bitte alle Felder ausfüllen.");
        return;
      }

      if (!double.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out double amount))
      {
        MessageBox.Show("Ungültiger Betrag.");
        return;
      }

      int userId = Userdb.LoggedInUserId;
      BudgetDb db = new BudgetDb();
      db.AddBudget(userId, category, amount, amount); 

      MessageBox.Show("Budget erfolgreich hinzugefügt!");
      Window1 mainApp = new Window1();
      mainApp.Show();
      this.Close();
    }
  }
}
