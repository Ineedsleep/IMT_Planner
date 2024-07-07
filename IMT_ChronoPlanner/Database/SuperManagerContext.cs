using IMT_ChronoPlanner.Models;
using Microsoft.EntityFrameworkCore;



namespace IMT_ChronoPlanner
{
    public class SuperManagerContext : DbContext
    {
        public DbSet<SuperManager> SuperManagers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=IMT.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SuperManager>(entity => 
            {
                entity.HasKey(sm => sm.Name);
        
                entity.Property(sm => sm.Rank);
        
                entity.Property(sm => sm.Promoted);
        
                entity.Property(sm => sm.Level);
        
                entity.Property(sm => sm.Equipment);
        
                entity.Property(sm => sm.Elements)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(str => Enum.Parse<Element>(str))
                            .ToList());
            });
        }
    }
}