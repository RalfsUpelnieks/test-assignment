namespace HousingManagementAPI.Models
{
    public class ApartmentResident
    {
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public int ResidentId { get; set; }
        public Resident? Resident { get; set; }
        public bool IsOwner { get; set; }
    }
}
