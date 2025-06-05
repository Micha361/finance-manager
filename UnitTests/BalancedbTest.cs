using Xunit;
using finance_manager;
using System.Data.SQLite;

public class BalancedbTests
{
  
  [Fact]
  public void CreateBalanceForUser_ShouldStoreZero_WhenNoStartValueGiven()
  {
    int testUserId = 9999;

    
    using (var conn = new SQLiteConnection("Data Source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finance_manager.db")))
    {
      conn.Open();
      new SQLiteCommand("DELETE FROM balance WHERE fk_userid = @id", conn)
      {
        Parameters = { new SQLiteParameter("@id", testUserId) }
      }.ExecuteNonQuery();
    }

    var db = new Balancedb();
    db.CreateBalanceForUser(testUserId, 0);
    double balance = db.GetBalance(testUserId);

    Assert.Equal(0, balance);
  }

  [Fact]
  public void UpdateBalance_ShouldStoreNewValue()
  {
    
    int testUserId = 9999;

    
    using (var conn = new SQLiteConnection("Data Source=" + Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finance_manager.db")))
    {
      conn.Open();
      var cmd = new SQLiteCommand("DELETE FROM balance WHERE fk_userid = @id", conn);
      cmd.Parameters.AddWithValue("@id", testUserId);
      cmd.ExecuteNonQuery();
    }

    
    var db = new Balancedb();
    db.CreateBalanceForUser(testUserId, 0);
    db.UpdateBalance(testUserId, 1337.77);

    
    double updated = db.GetBalance(testUserId);

    
    Assert.Equal(1337.77, updated, 2); 
  }
}
