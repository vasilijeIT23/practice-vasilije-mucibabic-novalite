using Microsoft.EntityFrameworkCore;
using MovieStoreCore;
using System.Reflection;

namespace MovieStoreInfrastructure
{
    public class MovieStoreContext : DbContext
    {
        public MovieStoreContext(DbContextOptions<MovieStoreContext> options) : base(options)
        {
            //Customers.Include(o => o.PurchasedMovies).Load();
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PurchasedMovie> PurchasedMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Movie>()
                .HasDiscriminator(x => x.LicencingType)
                .HasValue<TwoDayMovie>(LicencingType.TwoDay)
                .HasValue<LifelongMovie>(LicencingType.LifeLong);
        }
    }
}
