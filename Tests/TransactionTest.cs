using Domain.Aggregates;
using Xunit;

namespace Unit.Tests
{
    public class TransactionTest
    {
        private static Transaction TransactionEntity => new();

        [Fact]
        public void Authorize_Transaction_Return()
        {
            //Arrange
            var transaction = TransactionEntity;

            //Act
            var result = transaction.Amount;

            //Assert
            Assert.Equal(0, result);
        }

    }
}
