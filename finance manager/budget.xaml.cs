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


      addbudget main = new addbudget();
      main.Show();
      this.Close();

    }
  }
}