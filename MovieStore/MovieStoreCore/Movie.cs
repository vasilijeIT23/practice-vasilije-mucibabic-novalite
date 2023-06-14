namespace MovieStoreCore
{
    public abstract class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public LicencingType LicencingType { get; set; }
        public string Description { get; set; } = string.Empty;
        public double GetPrice(double modifier)
        {
            return GetBasePrice() * modifier;
        }
        protected abstract double GetBasePrice();
    }
}
