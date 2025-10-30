using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace finance_manager
{
  public partial class addgoals : Window
  {
    public addgoals()
    {
      InitializeComponent();
     
      Loaded += (_, __) => LoadGoalsInComboBox();
    }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
     
      var mainApp = new goals();
      mainApp.Show();
      Close();
    }

 

    private static bool TryParseAmount(string input, out decimal value)
    {
    
      if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out value) && value >= 0) 
        return true;

     
      if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value) && value >= 0)
        return true;

      return false;
    }

    private void ReloadGoalsKeepSelection(int? goalIdToSelect = null)
    {
      var db = new GoalDb();
      int userId = Userdb.LoggedInUserId;
      var goalsList = db.GetGoalsForUser(userId); 

      GoalComboBox.ItemsSource = null;     
      GoalComboBox.ItemsSource = goalsList;
      GoalComboBox.DisplayMemberPath = "Title";
      GoalComboBox.SelectedValuePath = "Id";

      if (goalIdToSelect.HasValue)
      {
        GoalComboBox.SelectedValue = goalIdToSelect.Value;
      }
      else if (goalsList.Count > 0 && GoalComboBox.SelectedIndex < 0)
      {
        GoalComboBox.SelectedIndex = 0;
      }
    }



    private void AddGoalButton_Click(object sender, RoutedEventArgs e)
    {
      string title = TitleBox.Text?.Trim() ?? "";
      string targetAmountText = TargetAmountBox.Text?.Trim() ?? "";

      if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(targetAmountText))
      {
        MessageBox.Show("Bitte alle Felder ausfüllen.");
        return;
      }

      if (!TryParseAmount(targetAmountText, out var targetAmount) || targetAmount <= 0)
      {
        MessageBox.Show("Bitte einen gültigen Zielbetrag eingeben.");
        return;
      }

      try
      {
        int userId = Userdb.LoggedInUserId;
        var db = new GoalDb();


        int newGoalId = db.AddGoal(title, (double)targetAmount, userId);

      
        TitleBox.Clear();
        TargetAmountBox.Clear();
        ReloadGoalsKeepSelection(newGoalId);

        MessageBox.Show("Ziel erfolgreich hinzugefügt!");
      }
      catch (Exception ex)
      {
        MessageBox.Show("Fehler beim Anlegen des Ziels: " + ex.Message);
      }
    }

    private void SaveMoneyBtn_Click(object sender, RoutedEventArgs e)
    {
      if (GoalComboBox.SelectedItem is not Goal selectedGoal)
      {
        MessageBox.Show("Bitte ein Ziel auswählen.");
        return;
      }

      string amountText = TargetAmountBox1.Text?.Trim() ?? "";
      if (!TryParseAmount(amountText, out var savingAmount) || savingAmount <= 0)
      {
        MessageBox.Show("Bitte einen gültigen Sparbetrag eingeben.");
        return;
      }

      try
      {
        int userId = Userdb.LoggedInUserId;
        var balanceDb = new Balancedb();
        var db = new GoalDb();

        decimal currentBalance = (decimal)balanceDb.GetBalance(userId);
        if (savingAmount > currentBalance)
        {
          MessageBox.Show("Nicht genügend Saldo.");
          return;
        }

        decimal newSaved = (decimal)selectedGoal.Saved + savingAmount;

        db.UpdateSavedAmount(selectedGoal.Id, (double)newSaved);
        balanceDb.UpdateBalance(userId, (double)(currentBalance - savingAmount));

        TargetAmountBox1.Clear();
       
        ReloadGoalsKeepSelection(selectedGoal.Id);

        MessageBox.Show("Sparbetrag erfolgreich hinzugefügt!");
      }
      catch (Exception ex)
      {
        MessageBox.Show("Fehler beim Speichern: " + ex.Message);
      }
    }

    private void LoadGoalsInComboBox()
    {
      ReloadGoalsKeepSelection();
    }

    private void GoalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      
    }
  }
}
