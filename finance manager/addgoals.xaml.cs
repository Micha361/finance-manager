using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace finance_manager
{
  public partial class addgoals : Window
  {
    public addgoals()
    {
      InitializeComponent();
      LoadGoalsInComboBox();
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
      db.AddGoal(title, targetAmount, userId);

      MessageBox.Show("Ziel erfolgreich hinzugefügt!");
      goals mainApp = new goals();
      mainApp.Show();
      this.Close();
    }

    private void LoadGoalsInComboBox()
    {
      GoalDb db = new GoalDb();
      int userId = Userdb.LoggedInUserId;
      var goalsList = db.GetGoalsForUser(userId);

      GoalComboBox.ItemsSource = goalsList;
      GoalComboBox.DisplayMemberPath = "Title";
      GoalComboBox.SelectedValuePath = "Id";
    }

    private void SaveMoneyBtn_Click(object sender, RoutedEventArgs e)
    {
      if (GoalComboBox.SelectedItem == null)
      {
        MessageBox.Show("Bitte ein Ziel auswählen.");
        return;
      }

      if (!double.TryParse(TargetAmountBox1.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double savingAmount))
      {
        MessageBox.Show("Ungültiger Sparbetrag.");
        return;
      }

      Goal selectedGoal = (Goal)GoalComboBox.SelectedItem;
      int userId = Userdb.LoggedInUserId;
      Balancedb balanceDb = new Balancedb();
      double currentBalance = balanceDb.GetBalance(userId);

      if (savingAmount > currentBalance)
      {
        MessageBox.Show("Nicht genügend Saldo.");
        return;
      }

      double newSaved = selectedGoal.Saved + savingAmount;

      GoalDb db = new GoalDb();
      db.UpdateSavedAmount(selectedGoal.Id, newSaved);

      balanceDb.UpdateBalance(userId, currentBalance - savingAmount);

      MessageBox.Show("Sparbetrag erfolgreich hinzugefügt!");
      goals mainApp = new goals();
      mainApp.Show();
      this.Close();
    }
    private void GoalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  }
}
