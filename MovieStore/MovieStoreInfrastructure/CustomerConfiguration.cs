using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreCore;

namespace MovieStoreInfrastructure
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(40);
            builder.Property(c => c.Status)
                .IsRequired();
            builder.Property(c => c.Role)
                .IsRequired();
            builder.Property(c => c.MoneySpent)
                .HasDefaultValue(0);

            builder.HasMany(c => c.PurchasedMovies)
                .WithOne(c => c.Customer);
        }
    }
}
