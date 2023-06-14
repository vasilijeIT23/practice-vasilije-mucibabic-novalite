namespace MovieStoreCore
{
    public class PurchasedMovie
    {
        public Guid Id { get; set; }

        public Movie Movie { get; set; } = null!;

        public Customer Customer { get; set; } = null!;

        public DateTime PurchaseDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public double Price { get; set; }
    }

}
