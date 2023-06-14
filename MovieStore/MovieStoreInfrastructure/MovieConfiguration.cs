using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreCore;

namespace MovieStoreInfrastructure
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(m => m.LicencingType)
                .IsRequired();
            builder.Property(m => m.Description).IsRequired();
        }
    }
}
