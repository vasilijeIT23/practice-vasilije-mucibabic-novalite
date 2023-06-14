using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class CustomerModelTest
    {
        [Fact]
        public void PurchaseMovie_Should_Return_Successful_Result()
        {
            // Arrange
            var movie = new Movie { Id = Guid.NewGuid(), Title = "Movie 1", LicencingType = LicencingType.Lifetime, Price = 10 };
            var customer = new Customer("John", "john@example.com");

            // Act
            var result = customer.PurchaseMovie(movie);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("Money spent increased!");
            customer.MoneySpent.Should().Be(movie.Price);
            customer.PurchasedMovies.Should().HaveCount(1);
            customer.PurchasedMovies[0].Movie.Should().Be(movie);
            customer.PurchasedMovies[0].Price.Should().Be(movie.Price);
            customer.PurchasedMovies[0].Customer.Should().Be(customer);
        }

        [Fact]
        public void PurchaseMovie_Should_Return_Failed_Result_If_Movie_Already_Purchased_And_Expired()
        {
            // Arrange
            var movie = new Movie { Id = Guid.NewGuid(), Title = "Movie 1", LicencingType = LicencingType.TwoDay, Price = 10 };
            var purchasedMovie = new PurchasedMovie { Movie = movie, PurchaseDate = DateTime.UtcNow.AddDays(-3), ExpirationDate = DateTime.UtcNow.AddDays(-1) };
            var customer = new Customer("John", "john@example.com");
            customer.GetType().GetProperty("_purchasedMovies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(customer, new List<PurchasedMovie> { purchasedMovie });

            // Act
            var result = customer.PurchaseMovie(movie);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Reasons.Should().Contain("Transaction not possible!");
            customer.MoneySpent.Should().Be(0);
            customer.PurchasedMovies.Should().HaveCount(1);
        }

        [Fact]
        public void PurchaseMovie_Should_Return_Failed_Result_If_Movie_Already_Purchased_And_Not_Expired()
        {
            // Arrange
            var movie = new Movie { Id = Guid.NewGuid(), Title = "Movie 1", LicencingType = LicencingType.TwoDay, Price = 10 };
            var purchasedMovie = new PurchasedMovie { Movie = movie, PurchaseDate = DateTime.UtcNow.AddDays(-1), ExpirationDate = DateTime.UtcNow.AddDays(1) };
            var customer = new Customer("John", "john@example.com");
            customer.GetType().GetProperty("_purchasedMovies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(customer, new List<PurchasedMovie> { purchasedMovie });

            // Act
            var result = customer.PurchaseMovie(movie);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Reasons.Should().Contain("Transaction not possible!");
            customer.MoneySpent.Should().Be(0);
            customer.PurchasedMovies.Should().HaveCount(1);
        }
    }
}
