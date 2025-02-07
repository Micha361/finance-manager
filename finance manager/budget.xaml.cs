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
    /// Interaktionslogik für budget.xaml
    /// </summary>
    public partial class budget : Window
    {
        public budget()
        {
            InitializeComponent();
        }

    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      Window1 mainApp = new Window1();
      mainApp.Show();

      this.Close();
    }
    }
}
