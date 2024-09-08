using IMT_Planner_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace IMT_Planner_DAL.Context
{
    public class IMTPlannerDbContext : DbContext
    {
        public IMTPlannerDbContext(DbContextOptions options)
            : base(options)
        {}
        public DbSet<SuperManager> SuperManagers { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<SuperManagerElement> SuperManagerElements { get; set; }
        public DbSet<Passive> Passives { get; set; }
        public DbSet<PassiveAttributeName> PassiveAttributeNames { get; set; }
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

            // Configuration for SuperManager and Passives
            modelBuilder.Entity<SuperManager>(entity =>
            {
                entity.Property(sm => sm.SuperManagerId).ValueGeneratedOnAdd();
                entity.HasKey(sm => sm.SuperManagerId);
                entity.Property(sm => sm.Name).IsRequired();
                
                entity.HasMany(sm => sm.Passives)
                    .WithOne(pa => pa.SuperManager)
                    .HasForeignKey(pa => pa.SuperManagerId);

                var rankConverter = new ValueConverter<Rank?, int>(
                    v => v.CurrentRank, // Convert from Rank to int
                    v => new Rank(v) // Convert from int to Rank
                );
                entity.Property(sm => sm.Rank)
                    .HasConversion(rankConverter);

                var rarityConverter = new ValueConverter<Rarity, string>(
                    v => v.ToString(),
                    v => (Rarity) Enum.Parse(typeof(Rarity), v));
                var areaConverter = new ValueConverter<Areas, string>(
                    v => v.ToString(),
                    v => (Areas) Enum.Parse(typeof(Areas), v));

                entity.Property(sm => sm.Promoted);
                entity.Property(sm => sm.Level);
                entity.Property(sm => sm.Rarity).HasConversion(rarityConverter);
                entity.Property(sm => sm.Area).HasConversion(areaConverter);
                entity.Property(sm => sm.Priority);
            });

            modelBuilder.Entity<Passive>(entity =>
            {
                entity.HasKey(pa => pa.Id); // Define primary key
                entity.Property(pa => pa.AttributeValue);
                entity.Property(pa => pa.PromoRequirement).IsRequired();

                // Define relationship with SuperManager
                entity.HasOne(pa => pa.SuperManager)
                    .WithMany(sm => sm.Passives)
                    .HasForeignKey(pa => pa.SuperManagerId);

                // Define relationship with PassiveAttributeName
                entity.HasOne(pa => pa.Name)
                    .WithMany(paName => paName.PassiveAttributes)
                    .HasForeignKey(pa => pa.PassiveAttributeNameId);
            });

            modelBuilder.Entity<PassiveAttributeName>().HasData(
                new PassiveAttributeName { Id = 1, Abbreviation = "MIF", Description = "Mine Income Factor"},
                new PassiveAttributeName { Id = 2, Abbreviation = "CIF", Description = "Continental Income Factor"},
                new PassiveAttributeName { Id = 3, Abbreviation = "CR", Description = "Cost reduction for current shaft lvl"},
                new PassiveAttributeName { Id = 4, Abbreviation = "SUCR", Description = "Shaft unlock cost reduction"}
                // Add more default attribute names as needed
            );

            // Elements data seeding
            modelBuilder.Entity<Element>().HasData(
                new Element { ElementId = 1, Name = "Nature"},
                new Element { ElementId = 2, Name = "Frost" },
                new Element { ElementId = 3, Name = "Flame" },
                new Element { ElementId = 4, Name = "Light" },
                new Element { ElementId = 5, Name = "Dark" },
                new Element { ElementId = 6, Name = "Wind" },
                new Element { ElementId = 7, Name = "Sand" },
                new Element { ElementId = 8, Name = "Water" }
            );
        }
    }
}