using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class IndicatorConfiguration : IEntityTypeConfiguration<CountryIndicator>
    {
        public void Configure(EntityTypeBuilder<CountryIndicator> builder)
        {
            builder.ToTable("Indicators");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Value).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(i => i.Year).IsRequired();
            builder.HasIndex(i => new { i.CountryId, i.MacroIndicatorId, i.Year }).IsUnique();

            builder.HasOne(i => i.Country)
                   .WithMany(c => c.Indicators)
                   .HasForeignKey(i => i.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.MacroIndicator)
                   .WithMany(m => m.CountryIndicators)
                   .HasForeignKey(i => i.MacroIndicatorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
