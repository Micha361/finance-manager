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
  /// Interaktionslogik für goals.xaml
  /// </summary>
  public partial class goals : Window
  {
    public goals()
    {
      InitializeComponent();
      LoadGoals();
    }

    private void LoadGoals()
    {
      GoalDb db = new GoalDb();
      int userId = Userdb.LoggedInUserId;
      var goals = db.GetGoalsForUser(userId);

      goalGrid.ItemsSource = goals.Select(g => new
      {
        g.Title,
        TargetAmount = $"{g.Target:F2} CHF",
        SavedAmount = $"{g.Saved:F2} CHF"
      }).ToList();
    }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      Window1 mainApp = new Window1();
      mainApp.Show();
      this.Close();
    }
    private void addbtn_Click(object sender, RoutedEventArgs e)
    {
      addgoals mainApp = new addgoals();
      mainApp.Show();
      this.Close();
    }

    private void goalGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
  }
}
