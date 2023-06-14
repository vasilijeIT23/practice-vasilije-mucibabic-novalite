using FluentAssertions;
using MovieStoreCore;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void PurchaseMovie_SuccessfulPurchase_ReturnsOkResult()
        {
            // Arrange
            var customer = new Customer("Alice", "alice@example.com");
            var movie = new LifelongMovie { Id = Guid.NewGuid(), LicencingType = LicencingType.LifeLong, Title = "sdfsd" };

            // Act
            var result = customer.PurchaseMovie(movie);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("Money spent increased!");
            customer.MoneySpent.Should().Be(50d);
            customer.PurchasedMovies.Should().HaveCount(1);
            customer.PurchasedMovies[0].Movie.Should().Be(movie);
            customer.PurchasedMovies[0].Customer.Should().Be(customer);
            customer.PurchasedMovies[0].Price.Should().Be(50);
            customer.PurchasedMovies[0].PurchaseDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            customer.PurchasedMovies[0].ExpirationDate.Should().BeNull();
        }

        [Fact]
        public void PurchaseMovie_DuplicatePurchase_ReturnsFailResult()
        {
            // Arrange
            var customer = new Customer("Alice", "alice@example.com");
            var movie = new LifelongMovie { Id = Guid.NewGuid(), LicencingType = LicencingType.LifeLong, Title = "sdfsd" };
            customer.PurchaseMovie(movie);

            // Act
            var result = customer.PurchaseMovie(movie);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("Transaction not possible!");
            customer.MoneySpent.Should().Be(50);
            customer.PurchasedMovies.Should().HaveCount(1);
        }

        [Fact]
        public void PromoteCustomer_SuccessfulPromotion_ReturnsOkResult()
        {
            // Arrange
            var customer = new Customer("Alice", "alice@example.com");
            var movie1 = new LifelongMovie { Id = Guid.NewGuid(), LicencingType = LicencingType.LifeLong };
            var movie2 = new LifelongMovie { Id = Guid.NewGuid(), LicencingType = LicencingType.LifeLong };
            customer.PurchaseMovie(movie1);
            customer.PurchaseMovie(movie2);


            // Act
            var result = customer.PromoteCustomer();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("Customer successfully promoted!");
            customer.Status.Should().Be(Status.Advanced);
            customer.StatusExpirationDate.Should().BeCloseTo(DateTime.Today.AddYears(1), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void PromoteCustomer_NotEnoughPurchases_ReturnsFailResult()
        {
            // Arrange
            var customer = new Customer("Alice", "alice@example.com");
            var movie = new LifelongMovie { Id = Guid.NewGuid(), LicencingType = LicencingType.LifeLong };
            customer.PurchaseMovie(movie);

            // Act
            var result = customer.PromoteCustomer();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.Message.Should().Be("Customer doesn't satisfy requirements to be promoted!");
            customer.Status.Should().Be(Status.Regular);
            customer.StatusExpirationDate.Should().BeNull();
        }
    }
}