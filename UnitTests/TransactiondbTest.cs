using finance_manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
  public class TransactiondbTest
  {
    [Fact]
    public void AddTransaction_ShouldNotThrow()
    {
      
      var db = new TransactionDb();
      int testUserId = 9999;
      new Balancedb().CreateBalanceForUser(testUserId, 1000);

      
      var exception = Record.Exception(() =>
          db.AddTransaction(testUserId, 100, "income", "Testeintrag", DateTime.Now)
      );

      
      Assert.Null(exception); 
    }
  }
}
