using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace finance_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

    private void loginbtn_Click(object sender, RoutedEventArgs e)
    {
      string username = usernametxt.Text;
      string password = passwordtxt.Password; 

      if (username == "admin" && password == "123")
      {
        Window1 mainApp = new Window1();
        mainApp.Show();

        this.Close();
      }
      else
      {
        MessageBox.Show("Benutzername oder Passwort ist falsch.");
        usernametxt.Text = "";
        passwordtxt.Password = "";
      }
    }

    private void registerlinkbtn_Click(object sender, RoutedEventArgs e)
    {
      register registerApp = new register();
      registerApp.Show();

      this.Close();
    }
    }
}