using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class ReturnRateConfigConfiguration : IEntityTypeConfiguration<ReturnRateConfig>
    {
        public void Configure(EntityTypeBuilder<ReturnRateConfig> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MinRate).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(x => x.MaxRate).IsRequired().HasColumnType("decimal(5,2)");
        }
    }
}
