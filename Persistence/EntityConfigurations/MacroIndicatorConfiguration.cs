using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class MacroIndicatorConfiguration : IEntityTypeConfiguration<MacroIndicator>
    {
        public void Configure(EntityTypeBuilder<MacroIndicator> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Weight).IsRequired().HasColumnType("decimal(5,4)");
            builder.Property(x => x.IsHigherBetter).IsRequired();
            builder.HasMany(x => x.CountryIndicators)
                   .WithOne(i => i.MacroIndicator)
                   .HasForeignKey(i => i.MacroIndicatorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
