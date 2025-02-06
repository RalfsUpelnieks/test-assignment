using AutoMapper;
using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Services
{
    public class ResidentService : IResidentService
    {
        private readonly IRepository<Resident> _residentRepository;
        private readonly IMapper _mapper;

        public ResidentService(IRepository<Resident> residentRepository, IMapper mapper)
        {
            _residentRepository = residentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResidentDTO>> GetAllResidentsAsync()
        {
            var residents = await _residentRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<ResidentDTO>>(residents);
        }

        public async Task<ResidentDetailsModel?> GetResidentByIdAsync(int id)
        {
            var resident = await _residentRepository.Get(r => r.ResidentId == id)
                .Include(r => r.ApartmentResidents!)
                .ThenInclude(ar => ar.Apartment)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResidentDetailsModel?>(resident);
        }

        public async Task<ResidentDTO> CreateResidentAsync(ResidentCreateModel resident)
        {
            var entity = _mapper.Map<Resident>(resident);
            _residentRepository.Add(entity);

            await _residentRepository.SaveAsync();

            return _mapper.Map<ResidentDTO>(entity);
        }

        public async Task UpdateResidentAsync(ResidentDTO apartment)
        {
            var residentExists = await _residentRepository.Get(r => r.ResidentId == apartment.ResidentId).AnyAsync();
            if (!residentExists)
            {
                throw new InvalidOperationException("The specified resident does not exist.");
            }

            var entity = _mapper.Map<Resident>(apartment);
            _residentRepository.Update(entity);

            await _residentRepository.SaveAsync();
        }

        public async Task DeleteResidentAsync(int id)
        {
            var resident = await _residentRepository.Get(r => r.ResidentId == id)
                .Include(r => r.ApartmentResidents!)
                .ThenInclude(ar => ar.Apartment)
                .FirstOrDefaultAsync();

            if (resident == null)
            {
                throw new InvalidOperationException("The specified resident does not exist.");
            }

            foreach (var apartmentResident in resident.ApartmentResidents!)
            {
                apartmentResident.Apartment!.ResidentCount -= 1;
            }

            _residentRepository.Delete(resident);

            await _residentRepository.SaveAsync();
        }
    }
}
