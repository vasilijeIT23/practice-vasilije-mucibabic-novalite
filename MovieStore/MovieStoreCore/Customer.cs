using FluentResults;

namespace MovieStoreCore
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Status Status { get; private set; }

        public Role Role { get; set; }

        public DateTime? StatusExpirationDate { get; private set; }

        public double MoneySpent { get; private set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private readonly List<PurchasedMovie> _purchasedMovies;

        public IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList().AsReadOnly();

        public Result<string> PurchaseMovie(Movie movie)
        {
            if (_purchasedMovies.FirstOrDefault(x => x.Movie.Id == movie.Id && (!x.ExpirationDate.HasValue || x.ExpirationDate.Value > DateTime.UtcNow)) != null)
            {
                return Result.Fail("Transaction not possible!");
            }

            var modifier = IsAdvanced ? 0.8d : 1d;
            var price = movie.GetPrice(modifier);

            _purchasedMovies.Add(new PurchasedMovie
            {
                Movie = movie,
                Customer = this,
                PurchaseDate = DateTime.Now,
                ExpirationDate = movie.LicencingType == LicencingType.TwoDay ? DateTime.Now.AddDays(2) : null,
                Price = price
            });

            MoneySpent += price;

            return Result.Ok("Money spent increased!");
        }

        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
            Status = Status.Regular;
            Role = Role.Regular;
            StatusExpirationDate = null;
            MoneySpent = 0;
            _purchasedMovies = new List<PurchasedMovie>();
        }

        public Result<string> PromoteCustomer()
        {
            if (PurchasedMovies.Count(x => x.PurchaseDate >= DateTime.UtcNow.AddDays(-60)) < 2)
            {
                return Result.Fail("Customer doesn't satisfy requirements to be promoted!");
            }
            if (IsAdvanced || MoneySpent <= 99)
            {
                return Result.Fail("Customer doesn't satisfy requirements to be promoted!");
            }
            Status = Status.Advanced;
            StatusExpirationDate = DateTime.Today.AddYears(1);
            return Result.Ok("Customer successfully promoted!");
        }

        public bool IsAdvanced => Status == Status.Advanced && StatusExpirationDate.HasValue && StatusExpirationDate.Value > DateTime.Now;
    }
}
