using HousingManagementAPI.Data;
using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                var databaseName = builder.Configuration.GetSection("Database:Name").Value ?? "InMemoryDb";
                options.UseInMemoryDatabase(databaseName);
            });

            builder.Services.AddScoped<IRepository<House>, Repository<House>>();
            builder.Services.AddScoped<IRepository<Apartment>, Repository<Apartment>>();
            builder.Services.AddScoped<IRepository<Resident>, Repository<Resident>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
