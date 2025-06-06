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
      budget mainApp = new budget();
      mainApp.Show();
      this.Close();
    }
  }
}
