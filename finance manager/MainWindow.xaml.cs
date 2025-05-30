using System.Windows;

namespace finance_manager
{
  public partial class MainWindow : Window
  {
    private Userdb userdb;

    public MainWindow()
    {
      InitializeComponent();
      userdb = new Userdb();
    }

    private void loginbtn_Click(object sender, RoutedEventArgs e)
    {
      string username = usernametxt.Text.ToLower();
      string password = passwordtxt.Password;

      if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
      {
        MessageBox.Show("Bitte geben Sie Benutzername und Passwort ein.");
        return;
      }

      bool isValidUser = userdb.CheckLogin(username, password);

      if (isValidUser)
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

    private void passwordtxt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == System.Windows.Input.Key.Enter)
      {
        loginbtn_Click(sender, e);
      }
    }

  }
}
