namespace HousingManagementAPI.Models
{
    public class Apartment
    {
        public int ApartmentId { get; set; }
        public string Number { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int RoomCount { get; set; }
        public int ResidentCount { get; set; }
        public double TotalArea { get; set; }
        public double LivingArea { get; set; }
        public int HouseId { get; set; }
        public House? House { get; set; }
        public ICollection<ApartmentResident>? ApartmentResidents { get; set; }
    }
}
