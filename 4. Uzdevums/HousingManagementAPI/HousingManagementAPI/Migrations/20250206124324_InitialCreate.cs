using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HousingManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "House",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_House", x => x.HouseId);
                });

            migrationBuilder.CreateTable(
                name: "Resident",
                columns: table => new
                {
                    ResidentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resident", x => x.ResidentId);
                });

            migrationBuilder.CreateTable(
                name: "Apartment",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    ResidentCount = table.Column<int>(type: "int", nullable: false),
                    TotalArea = table.Column<double>(type: "float", nullable: false),
                    LivingArea = table.Column<double>(type: "float", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartment", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "FK_Apartment_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "HouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentResident",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    ResidentId = table.Column<int>(type: "int", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentResident", x => new { x.ApartmentId, x.ResidentId });
                    table.ForeignKey(
                        name: "FK_ApartmentResident_Apartment_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartment",
                        principalColumn: "ApartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApartmentResident_Resident_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Resident",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "House",
                columns: new[] { "HouseId", "City", "Country", "Number", "PostalCode", "Street" },
                values: new object[,]
                {
                    { 1, "Riga", "Latvia", "1", "LV-1010", "Main St" },
                    { 2, "Riga", "Latvia", "2", "LV-1020", "Second St" }
                });

            migrationBuilder.InsertData(
                table: "Resident",
                columns: new[] { "ResidentId", "BirthDate", "Email", "FirstName", "LastName", "PersonalCode", "Phone" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "John", "Doe", "010101-12345", "12345678" },
                    { 2, new DateTime(1990, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "Jane", "Smith", "020202-67890", "87654321" },
                    { 3, new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice.johnson@example.com", "Alice", "Johnson", "030303-54321", "12312312" },
                    { 4, new DateTime(1995, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob.brown@example.com", "Bob", "Brown", "040404-98765", "32132132" }
                });

            migrationBuilder.InsertData(
                table: "Apartment",
                columns: new[] { "ApartmentId", "Floor", "HouseId", "LivingArea", "Number", "ResidentCount", "RoomCount", "TotalArea" },
                values: new object[,]
                {
                    { 1, 1, 1, 60.0, "1A", 2, 3, 80.0 },
                    { 2, 2, 1, 40.0, "2A", 1, 2, 50.0 },
                    { 3, 1, 2, 70.0, "1B", 1, 4, 90.0 },
                    { 4, 2, 2, 45.0, "2B", 1, 2, 60.0 },
                    { 5, 3, 2, 100.0, "3B", 1, 5, 120.0 }
                });

            migrationBuilder.InsertData(
                table: "ApartmentResident",
                columns: new[] { "ApartmentId", "ResidentId", "IsOwner" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 1, 2, false },
                    { 2, 2, false },
                    { 3, 3, false },
                    { 4, 4, false },
                    { 5, 1, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartment_HouseId",
                table: "Apartment",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentResident_ResidentId",
                table: "ApartmentResident",
                column: "ResidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentResident");

            migrationBuilder.DropTable(
                name: "Apartment");

            migrationBuilder.DropTable(
                name: "Resident");

            migrationBuilder.DropTable(
                name: "House");
        }
    }
}
