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
namespace finance_manager
{
    /// <summary>
    /// Interaktionslogik für addgoals.xaml
    /// </summary>
    public partial class addgoals : Window
    {
        public addgoals()
        {
            InitializeComponent();
        }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      goals mainApp = new goals();
      mainApp.Show();
      this.Close();
    }
    private void AddGoalButton_Click(object sender, RoutedEventArgs e)
    {
      string title = TitleBox.Text;
      string targetAmountText = TargetAmountBox.Text;

      if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(targetAmountText))
      {
        MessageBox.Show("Bitte alle Felder ausfüllen.");
        return;
      }

      if (!double.TryParse(targetAmountText, NumberStyles.Any, CultureInfo.InvariantCulture, out double targetAmount))
      {
        MessageBox.Show("Ungültiger Betrag.");
        return;
      }

      int userId = Userdb.LoggedInUserId;
      GoalDb db = new GoalDb();
      db.AddGoal(userId, title, targetAmount);

      MessageBox.Show("Ziel erfolgreich hinzugefügt!");
      goals mainApp = new goals();
      mainApp.Show();
      this.Close();
    }
  }
}
