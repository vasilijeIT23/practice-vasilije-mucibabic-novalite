using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreCore;

namespace MovieStoreInfrastructure
{
    public class PurchasedMovieConfiguration : IEntityTypeConfiguration<PurchasedMovie>
    {
        public void Configure(EntityTypeBuilder<PurchasedMovie> builder)
        {
            builder.HasKey(pm => pm.Id);
            builder.Property(pm => pm.PurchaseDate)
                .IsRequired();
            builder.Property(pm => pm.Price)
                .IsRequired();
        }
    }
}
