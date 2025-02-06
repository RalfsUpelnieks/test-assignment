namespace HousingManagementAPI.Models
{
    public class Resident
    {
        public int ResidentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PersonalCode { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<ApartmentResident>? ApartmentResidents { get; set; }
    }
}
