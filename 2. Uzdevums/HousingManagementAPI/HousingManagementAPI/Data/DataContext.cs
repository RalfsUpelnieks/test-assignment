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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<House>().HasKey(u => u.HouseId);

            builder.Entity<Apartment>().HasKey(u => u.ApartmentId);

            builder.Entity<Resident>().HasKey(u => u.ResidentId);

            base.OnModelCreating(builder);
        }
    }
}
