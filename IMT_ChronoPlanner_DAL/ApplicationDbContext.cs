using IMT_ChronoPlanner_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


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
                entity.HasKey(sm => sm.SuperManagerId);

                entity.Property(sm => sm.Name).IsRequired();
                var rankConverter = new ValueConverter<Rank, int>(
                    v => v.CurrentRank, // Convert from Rank to int
                    v => new Rank(v) // Convert from int to Rank
                );
                // You might need to define a ValueConverter for Rank class
                // depending on how you have implemented it
                entity.Property(sm => sm.Rank)
                    .HasConversion(rankConverter);

                entity.Property(sm => sm.Promoted);

                entity.Property(sm => sm.Level);

                var rarityConverter = new ValueConverter<Rarity, string>(
                    v => v.ToString(),
                    v => (Rarity) Enum.Parse(typeof(Rarity), v));

                var areaConverter = new ValueConverter<Areas, string>(
                    v => v.ToString(),
                    v => (Areas) Enum.Parse(typeof(Areas), v));
                // Similar to Rank, you may need a ValueConverter for Rarity and Areas
                entity.Property(sm => sm.Rarity)
                    .HasConversion(rarityConverter);
                entity.Property(sm => sm.Area)
                    .HasConversion(areaConverter);

          
                // ValueConversion could be needed for Equipment, depending on its definition
                var equipmentConverter = new ValueConverter<Equipment, string>(
                    v => v.ToString(),
                    v => (Equipment) Enum.Parse(typeof(Equipment), v));
                entity.Property(sm => sm.Equipment).HasConversion(equipmentConverter);

                entity.Property(sm => sm.PassiveMultiplier);

                entity.Property(sm => sm.Priority);
                
            });
            
            
        }
    }
}