using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.IsoCode).IsRequired().HasMaxLength(3);
            builder.HasMany(x => x.Indicators)
                   .WithOne(i => i.Country)
                   .HasForeignKey(i => i.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
