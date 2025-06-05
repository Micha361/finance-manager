using Xunit;
using finance_manager;

public class BalancedbTests
{
  [Fact]
  public void CreateBalanceForUser_ShouldStoreZero_WhenNoStartValueGiven()
  {
    // Arrange
    var db = new Balancedb();
    int testUserId = 9999;
    db.CreateBalanceForUser(testUserId, 0);

    // Act
    double balance = db.GetBalance(testUserId);

    // Assert
    Assert.Equal(0, balance);
  }

  [Fact]
  public void UpdateBalance_ShouldStoreNewValue()
  {
    // Arrange
    var db = new Balancedb();
    int testUserId = 9999;
    db.CreateBalanceForUser(testUserId, 0);

    // Act
    db.UpdateBalance(testUserId, 1337.77);
    double updated = db.GetBalance(testUserId);

    // Assert
    Assert.Equal(1337.77, updated, 2); // mit Genauigkeit
  }
}
