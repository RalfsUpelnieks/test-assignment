using HousingManagementAPI.Data;
using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using HousingManagementAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var allowedCorsOrigin = builder.Configuration["AllowedCorsOrigin"];
                    if (!string.IsNullOrEmpty(allowedCorsOrigin))
                    {
                        policy.WithOrigins(allowedCorsOrigin)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IRepository<House>, Repository<House>>();
            builder.Services.AddScoped<IRepository<Apartment>, Repository<Apartment>>();
            builder.Services.AddScoped<IRepository<Resident>, Repository<Resident>>();
            builder.Services.AddScoped<IRepository<ApartmentResident>, Repository<ApartmentResident>>();

            builder.Services.AddScoped<IHouseService, HouseService>();
            builder.Services.AddScoped<IApartmentService, ApartmentService>();
            builder.Services.AddScoped<IResidentService, ResidentService>();
            builder.Services.AddScoped<IApartmentResidentService, ApartmentResidentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
