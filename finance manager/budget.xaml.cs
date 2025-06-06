using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace finance_manager
{
  public partial class budget : Window
  {
    private BudgetDb db;
    private int userId;

    public budget()
    {
      InitializeComponent();
      db = new BudgetDb();
      userId = Userdb.LoggedInUserId;
      LoadBudgets();
    }

    private void LoadBudgets()
    {
      var budgets = db.GetBudgetsForUser(userId);
      budgetGrid.ItemsSource = budgets.Select(b => new
      {
        Description = b.Description,
        Total = $"{b.Total:F2} CHF",
        Monthly = $"{b.Monthly:F2} CHF"
      }).ToList();
    }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      Window1 main = new Window1();
      main.Show();
      this.Close();
    }

    private void addbtn_Click(object sender, RoutedEventArgs e)
    {
      addbudget mainApp = new addbudget();
      mainApp.Show();
      this.Close();
    }


    private void DeleteBudget_Click(object sender, RoutedEventArgs e)
    {
      Button deleteButton = sender as Button;
      dynamic selectedBudget = deleteButton.DataContext;

      string desc = selectedBudget.Description;

      var allBudgets = db.GetBudgetsForUser(userId);
      var budgetToDelete = allBudgets.FirstOrDefault(b => b.Description == desc);

      if (budgetToDelete.Equals(default((int, string, double, double))))
      {
        MessageBox.Show("Budget konnte nicht gefunden werden.");
        return;
      }

      MessageBoxResult result = MessageBox.Show(
        "Möchtest du dieses Budget wirklich löschen?",
        "Bestätigung",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning
      );

      if (result == MessageBoxResult.Yes)
      {
        db.DeleteBudget(budgetToDelete.Id);
        MessageBox.Show("Budget wurde gelöscht.");
        LoadBudgets();
      }
    }


  }
}