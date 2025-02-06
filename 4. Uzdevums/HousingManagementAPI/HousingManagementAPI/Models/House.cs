﻿namespace HousingManagementAPI.Models
{
    public class House
    {
        public int HouseId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public ICollection<Apartment>? Apartments { get; set; }
    }
}
