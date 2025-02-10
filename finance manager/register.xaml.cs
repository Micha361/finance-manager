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
    /// Interaktionslogik für register.xaml
    /// </summary>
    public partial class register : Window
    {
        public register()
        {
            InitializeComponent();
        }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

        }

    private void registerbtn_Click(object sender, RoutedEventArgs e)
    {

        }

    private void loginlinkbtn_Click(object sender, RoutedEventArgs e)
    {
      MainWindow LoginApp = new MainWindow();
      LoginApp.Show();

      this.Close();
    }
    }
}
