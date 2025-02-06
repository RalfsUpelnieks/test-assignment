using AutoMapper;
using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _houseRepository;
        private readonly IMapper _mapper;

        public HouseService(IRepository<House> houseRepository, IMapper mapper)
        {
            _houseRepository = houseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HouseDTO>> GetAllHousesAsync()
        {
            var houses = await _houseRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<HouseDTO>>(houses);
        }

        public async Task<HouseDetailsModel?> GetHouseByIdAsync(int id)
        {
            var house = await _houseRepository.Get(h => h.HouseId == id)
                .Include(h => h.Apartments)
                .FirstOrDefaultAsync();

            return _mapper.Map<HouseDetailsModel?>(house);
        }

        public async Task<HouseDTO> CreateHouseAsync(HouseCreateModel house)
        {
            var entity = _mapper.Map<House>(house);
            _houseRepository.Add(entity);

            await _houseRepository.SaveAsync();

            return _mapper.Map<HouseDTO>(entity);
        }

        public async Task UpdateHouseAsync(HouseDTO apartment)
        {
            var houseExists = await _houseRepository.Get(h => h.HouseId == apartment.HouseId).AnyAsync();
            if (!houseExists)
            {
                throw new InvalidOperationException("The specified house does not exist.");
            }

            var entity = _mapper.Map<House>(apartment);
            _houseRepository.Update(entity);

            await _houseRepository.SaveAsync();
        }

        public async Task DeleteHouseAsync(int id)
        {
            var house = await _houseRepository.Get(h => h.HouseId == id).FirstOrDefaultAsync();

            if (house == null)
            {
                throw new InvalidOperationException("The specified house does not exist.");
            }

            _houseRepository.Delete(house);

            await _houseRepository.SaveAsync();
        }
    }
}
