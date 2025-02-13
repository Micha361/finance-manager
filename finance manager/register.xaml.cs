using System.Windows;

namespace finance_manager
{
  public partial class register : Window
  {
    private Userdb userdb; 

    public register()
    {
      InitializeComponent();
      userdb = new Userdb();
    }

    private void registerbtn_Click(object sender, RoutedEventArgs e)
    {
      string username = registertxt.Text;
      string password = passworttxt.Password;

      if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
      {
        MessageBox.Show("Bitte füllen Sie alle Felder aus.");
        return;
      }

      bool success = userdb.InsertUser(username, password);
      if (success)
      {
        MessageBox.Show("Registrierung erfolgreich! Sie können sich nun anmelden.");
        MainWindow loginWindow = new MainWindow();
        loginWindow.Show();
        this.Close();
      }
      else
      {
        MessageBox.Show("Fehler bei der Registrierung! Benutzername könnte bereits existieren.");
      }
    }

    private void loginlinkbtn_Click(object sender, RoutedEventArgs e)
    {
      MainWindow LoginApp = new MainWindow();
      LoginApp.Show();
      this.Close();
    }
  }
}
