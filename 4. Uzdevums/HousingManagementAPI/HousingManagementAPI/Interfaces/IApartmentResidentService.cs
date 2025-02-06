using HousingManagementAPI.DTOs;

namespace HousingManagementAPI.Interfaces
{
    public interface IApartmentResidentService
    {
        Task<ApartmentResidentDTO> CreateApartmentResidentAsync(ApartmentResidentDTO apartmentResident);
        Task UpdateApartmentResidentAsync(ApartmentResidentDTO apartmentResident);
        Task DeleteApartmentResidentAsync(int apartmentId, int residentId);
    }
}
