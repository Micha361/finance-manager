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
        public Window1()
        {
            InitializeComponent();
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
  }
}
