using HousingManagementAPI.DTOs;

namespace HousingManagementAPI.Interfaces
{
    public interface IResidentService
    {
        Task<IEnumerable<ResidentDTO>> GetAllResidentsAsync();
        Task<ResidentDetailsModel?> GetResidentByIdAsync(int id);
        Task<ResidentDTO> CreateResidentAsync(ResidentCreateModel resident);
        Task UpdateResidentAsync(ResidentDTO resident);
        Task DeleteResidentAsync(int id);
    }
}
