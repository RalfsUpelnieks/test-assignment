using HousingManagementAPI.DTOs;

namespace HousingManagementAPI.Interfaces
{
    public interface IApartmentService
    {
        Task<IEnumerable<ApartmentDTO>> GetAllApartmentsAsync();
        Task<ApartmentDetailsModel?> GetApartmentByIdAsync(int id);
        Task<ApartmentDTO> CreateApartmentAsync(ApartmentCreateModel apartment);
        Task UpdateApartmentAsync(ApartmentDTO apartment);
        Task DeleteApartmentAsync(int id);
    }
}
