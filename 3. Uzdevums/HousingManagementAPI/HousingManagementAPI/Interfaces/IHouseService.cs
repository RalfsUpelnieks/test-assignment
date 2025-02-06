using HousingManagementAPI.DTOs;

namespace HousingManagementAPI.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseDTO>> GetAllHousesAsync();
        Task<HouseDetailsModel?> GetHouseByIdAsync(int id);
        Task<HouseDTO> CreateHouseAsync(HouseCreateModel house);
        Task UpdateHouseAsync(HouseDTO house);
        Task DeleteHouseAsync(int id);
    }
}
