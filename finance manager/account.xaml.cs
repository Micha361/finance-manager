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
  /// Interaktionslogik für account.xaml
  /// </summary>
  public partial class account : Window
  {
    public account()
    {
      InitializeComponent();

      if (!string.IsNullOrEmpty(Userdb.LoggedInUser))
      {
        usernamelbl.Content = $"username: {Userdb.LoggedInUser}!";
      }
      else
      {
        usernamelbl.Content = "username: ";
      }
    }
    private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
    {
      string oldPassword = OldPasswordBox.Password;
      string newPassword = NewPasswordBox.Password;
      string confirmPassword = ConfirmPasswordBox.Password;

      if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
      {
        MessageBox.Show("Alle Felder müssen ausgefüllt werden.");
        return;
      }

      if (newPassword != confirmPassword)
      {
        MessageBox.Show("Das neue Passwort stimmt nicht mit der Bestätigung überein.");
        return;
      }

      Userdb db = new Userdb();
      bool success = db.ChangePassword(Userdb.LoggedInUser, oldPassword, newPassword);

      if (success)
        MessageBox.Show("Passwort erfolgreich geändert.");
      else
        MessageBox.Show("Altes Passwort ist falsch.");
    }
    private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
    {
      MessageBoxResult result = MessageBox.Show("Bist du sicher, dass du deinen Account löschen möchtest?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning);

      if (result == MessageBoxResult.Yes)
      {
        Userdb db = new Userdb();
        bool success = db.DeleteUser(Userdb.LoggedInUserId);

        if (success)
        {
          MessageBox.Show("Account erfolgreich gelöscht.");
          Userdb.LoggedInUser = null;

          Application.Current.Shutdown(); 
        }
        else
        {
          MessageBox.Show("Fehler beim Löschen des Accounts.");
        }
      }
    }


    private void backbtn_Click(object sender, RoutedEventArgs e)
    {
      Window1 mainApp = new Window1();
      mainApp.Show();

      this.Close();
    }
    }
}
