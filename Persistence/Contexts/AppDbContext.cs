using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<MacroIndicator> MacroIndicators { get; set; }
        public DbSet<CountryIndicator> CountryIndicators { get; set; }
        public DbSet<ReturnRateConfig> ReturnRateConfigs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfigurations.CountryConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.MacroIndicatorConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.IndicatorConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.ReturnRateConfigConfiguration());
        }
    }
}
