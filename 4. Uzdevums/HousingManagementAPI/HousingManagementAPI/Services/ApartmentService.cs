using AutoMapper;
using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<House> _houseRepository;
        private readonly IMapper _mapper;

        public ApartmentService(IRepository<Apartment> apartmentRepository, IRepository<House> houseRepository, IMapper mapper)
        {
            _apartmentRepository = apartmentRepository;
            _houseRepository = houseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApartmentDTO>> GetAllApartmentsAsync()
        {
            var apartments = await _apartmentRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<ApartmentDTO>>(apartments);
        }

        public async Task<ApartmentDetailsModel?> GetApartmentByIdAsync(int id)
        {
            var apartment = await _apartmentRepository.Get(a => a.ApartmentId == id)
                .Include(a => a.ApartmentResidents!)
                .ThenInclude(ar => ar.Resident)
                .FirstOrDefaultAsync();

            return _mapper.Map<ApartmentDetailsModel?>(apartment);
        }

        public async Task<ApartmentDTO> CreateApartmentAsync(ApartmentCreateModel apartment)
        {
            var houseExists = await _houseRepository.Get(h => h.HouseId == apartment.HouseId).AnyAsync();
            if (!houseExists)
            {
                throw new InvalidOperationException("The specified house does not exist.");
            }

            var entity = _mapper.Map<Apartment>(apartment);
            _apartmentRepository.Add(entity);

            await _apartmentRepository.SaveAsync();

            return _mapper.Map<ApartmentDTO>(entity);
        }

        public async Task UpdateApartmentAsync(ApartmentDTO apartment)
        {
            var apartmentExists = await _houseRepository.Get(h => h.HouseId == apartment.HouseId).AnyAsync();
            if (!apartmentExists)
            {
                throw new InvalidOperationException("The specified apartment does not exist.");
            }

            var houseExists = await _houseRepository.Get(h => h.HouseId == apartment.HouseId).AnyAsync();
            if (!houseExists)
            {
                throw new InvalidOperationException("The specified house does not exist.");
            }

            var entity = _mapper.Map<Apartment>(apartment);
            _apartmentRepository.Update(entity);

            await _apartmentRepository.SaveAsync();
        }

        public async Task DeleteApartmentAsync(int id)
        {
            var apartment = await _apartmentRepository.Get(a => a.ApartmentId == id).FirstOrDefaultAsync();

            if (apartment == null)
            {
                throw new InvalidOperationException("The specified apartment does not exist.");
            }

            _apartmentRepository.Delete(apartment);

            await _apartmentRepository.SaveAsync();
        }
    }
}
