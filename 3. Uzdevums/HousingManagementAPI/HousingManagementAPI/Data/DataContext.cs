using Microsoft.EntityFrameworkCore;
using HousingManagementAPI.Models;

namespace HousingManagementAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<House> House { get; set; }
        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<Resident> Resident { get; set; }
        public DbSet<ApartmentResident> ApartmentResident { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Model configuration
            builder.Entity<House>()
                .HasKey(h => h.HouseId);
            builder.Entity<House>()
                .Property(h => h.HouseId)
                .ValueGeneratedOnAdd();

            builder.Entity<Apartment>()
                .HasKey(a => a.ApartmentId);
            builder.Entity<Apartment>()
                .Property(a => a.ApartmentId)
                .ValueGeneratedOnAdd();

            builder.Entity<Apartment>()
                .HasOne(a => a.House)
                .WithMany(h => h.Apartments)
                .HasForeignKey(a => a.HouseId);

            builder.Entity<Resident>()
                .HasKey(r => r.ResidentId);
            builder.Entity<Resident>()
                .Property(r => r.ResidentId)
                .ValueGeneratedOnAdd();

            builder.Entity<ApartmentResident>()
                .HasKey(ar => new { ar.ApartmentId, ar.ResidentId });

            builder.Entity<ApartmentResident>()
                .HasOne(ar => ar.Apartment)
                .WithMany(a => a.ApartmentResidents)
                .HasForeignKey(ar => ar.ApartmentId);

            builder.Entity<ApartmentResident>()
                .HasOne(ar => ar.Resident)
                .WithMany(r => r.ApartmentResidents)
                .HasForeignKey(ar => ar.ResidentId);

            // Seed data
            builder.Entity<House>().HasData(
                new House { HouseId = 1, Number = "1", Street = "Main St", City = "Riga", Country = "Latvia", PostalCode = "LV-1010" },
                new House { HouseId = 2, Number = "2", Street = "Second St", City = "Riga", Country = "Latvia", PostalCode = "LV-1020" }
            );

            builder.Entity<Apartment>().HasData(
                new Apartment { ApartmentId = 1, Number = "1A", Floor = 1, RoomCount = 3, ResidentCount = 2, TotalArea = 80, LivingArea = 60, HouseId = 1 },
                new Apartment { ApartmentId = 2, Number = "2A", Floor = 2, RoomCount = 2, ResidentCount = 1, TotalArea = 50, LivingArea = 40, HouseId = 1 },
                new Apartment { ApartmentId = 3, Number = "1B", Floor = 1, RoomCount = 4, ResidentCount = 1, TotalArea = 90, LivingArea = 70, HouseId = 2 },
                new Apartment { ApartmentId = 4, Number = "2B", Floor = 2, RoomCount = 2, ResidentCount = 1, TotalArea = 60, LivingArea = 45, HouseId = 2 },
                new Apartment { ApartmentId = 5, Number = "3B", Floor = 3, RoomCount = 5, ResidentCount = 1, TotalArea = 120, LivingArea = 100, HouseId = 2 }
            );

            builder.Entity<Resident>().HasData(
                new Resident { ResidentId = 1, FirstName = "John", LastName = "Doe", PersonalCode = "010101-12345", BirthDate = new DateTime(1980, 1, 1), Phone = "12345678", Email = "john.doe@example.com" },
                new Resident { ResidentId = 2, FirstName = "Jane", LastName = "Smith", PersonalCode = "020202-67890", BirthDate = new DateTime(1990, 2, 2), Phone = "87654321", Email = "jane.smith@example.com" },
                new Resident { ResidentId = 3, FirstName = "Alice", LastName = "Johnson", PersonalCode = "030303-54321", BirthDate = new DateTime(2000, 3, 3), Phone = "12312312", Email = "alice.johnson@example.com"  },
                new Resident { ResidentId = 4, FirstName = "Bob", LastName = "Brown", PersonalCode = "040404-98765", BirthDate = new DateTime(1995, 4, 4), Phone = "32132132", Email = "bob.brown@example.com" }
            );

            builder.Entity<ApartmentResident>().HasData(
                new ApartmentResident { IsOwner = true, ApartmentId = 1, ResidentId = 1 },
                new ApartmentResident { IsOwner = false, ApartmentId = 1, ResidentId = 2 },
                new ApartmentResident { IsOwner = false, ApartmentId = 2, ResidentId = 2 },
                new ApartmentResident { IsOwner = false, ApartmentId = 3, ResidentId = 3 },
                new ApartmentResident { IsOwner = false, ApartmentId = 4, ResidentId = 4 },
                new ApartmentResident { IsOwner = true, ApartmentId = 5, ResidentId = 1 }
            );

            base.OnModelCreating(builder);
        }
    }
}
