using IMT_ChronoPlanner_Model;
using Microsoft.EntityFrameworkCore;



namespace IMT_ChronoPlanner_DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<SuperManager> SuperManagers { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<SuperManagerElement> SuperManagerElements { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=IMT.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            // Configuration for many-many relation between SuperManager and Element
            modelBuilder.Entity<SuperManagerElement>()
                .HasKey(t => new { t.SuperManagerId, t.ElementId });

            modelBuilder.Entity<SuperManagerElement>()
                .HasOne(sm => sm.SuperManager)
                .WithMany(s => s.SuperManagerElements)
                .HasForeignKey(sm => sm.SuperManagerId);

            modelBuilder.Entity<SuperManagerElement>()
                .HasOne(sm => sm.Element)
                .WithMany(e => e.SuperManagerElements)
                .HasForeignKey(sm => sm.ElementId);
        
            // Elements data seeding
            modelBuilder.Entity<Element>().HasData(
                new Element { ElementId = 1, Name = "Nature", Description = "Nature description" },
                new Element { ElementId = 2, Name = "Frost", Description = "Frost description" },
                new Element { ElementId = 3, Name = "Flame", Description = "Flame description" },
                new Element { ElementId = 4, Name = "Light", Description = "Light description" },
                new Element { ElementId = 5, Name = "Dark", Description = "Dark description" },
                new Element { ElementId = 6, Name = "Wind", Description = "Wind description" },
                new Element { ElementId = 7, Name = "Sand", Description = "Sand description" },
                new Element { ElementId = 8, Name = "Water", Description = "Water description" }
            );
            
            
            modelBuilder.Entity<SuperManager>(entity => 
            {
                entity.HasKey(sm => sm.Name);
        
                entity.Property(sm => sm.Rank);
        
                entity.Property(sm => sm.Promoted);
        
                entity.Property(sm => sm.Level);
        
                entity.Property(sm => sm.Equipment);
        
                // entity.Property(sm => sm.Elements)
                //     .HasConversion(
                //         v => string.Join(',', v),
                //         v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                //             .Select(str => Enum.Parse<Element>(str))
                //             .ToList());
            });
        }
    }
}