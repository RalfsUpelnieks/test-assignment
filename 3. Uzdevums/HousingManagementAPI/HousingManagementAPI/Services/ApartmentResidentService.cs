using AutoMapper;
using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Services
{
    public class ApartmentResidentService : IApartmentResidentService
    {
        private readonly IRepository<ApartmentResident> _apartmentResidentRepository;
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<Resident> _residentRepository;
        private readonly IMapper _mapper;

        public ApartmentResidentService(IRepository<ApartmentResident> apartmentResidentRepository, IRepository<Apartment> apartmentRepository, IRepository<Resident> residentRepository, IMapper mapper)
        {
            _apartmentResidentRepository = apartmentResidentRepository;
            _apartmentRepository = apartmentRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;
        }

        public async Task<ApartmentResidentDTO> CreateApartmentResidentAsync(ApartmentResidentDTO apartmentResidentDTO)
        {
            var apartment = await _apartmentRepository.Get(a => a.ApartmentId == apartmentResidentDTO.ApartmentId).FirstOrDefaultAsync();
            if (apartment == null)
            {
                throw new InvalidOperationException("The specified apartment does not exist.");
            }

            var residentExists = await _residentRepository.Get(r => r.ResidentId == apartmentResidentDTO.ResidentId).AnyAsync();
            if (!residentExists)
            {
                throw new InvalidOperationException("The specified resident does not exist.");
            }

            var apartmentResidentExists = await _apartmentResidentRepository.Get(ar => ar.ApartmentId == apartmentResidentDTO.ApartmentId && ar.ResidentId == apartmentResidentDTO.ResidentId).AnyAsync();
            if (apartmentResidentExists)
            {
                throw new InvalidOperationException("The ApartmentResident with the given keys already exists.");
            }

            // If the resident is being set as the owner, remove the owner status from all other residents in the apartment
            if (apartmentResidentDTO.IsOwner)
            {
                var apartmentResidents = await _apartmentResidentRepository.Get(ar => ar.ApartmentId == apartmentResidentDTO.ApartmentId).ToListAsync();
                foreach (var apartmentResident in apartmentResidents)
                {
                    apartmentResident.IsOwner = false;
                    _apartmentResidentRepository.Update(apartmentResident);
                }
            }

            var entity = _mapper.Map<ApartmentResident>(apartmentResidentDTO);
            _apartmentResidentRepository.Add(entity);

            apartment!.ResidentCount += 1;

            await _apartmentResidentRepository.SaveAsync();

            return apartmentResidentDTO;
        }
        public async Task UpdateApartmentResidentAsync(ApartmentResidentDTO apartmentResidentDTO)
        {
            var apartmentResidentExists = await _apartmentResidentRepository.Get(ar => ar.ApartmentId == apartmentResidentDTO.ApartmentId && ar.ResidentId == apartmentResidentDTO.ResidentId).AnyAsync();
            if (!apartmentResidentExists)
            {
                throw new InvalidOperationException("The specified ApartmentResident does not exist.");
            }

            // If the resident is being set as the owner, remove the owner status from all other residents in the apartment
            if (apartmentResidentDTO.IsOwner)
            {
                var apartmentResidents = await _apartmentResidentRepository.Get(ar => ar.ApartmentId == apartmentResidentDTO.ApartmentId && ar.ResidentId != apartmentResidentDTO.ResidentId).ToListAsync();
                foreach (var apartmentResident in apartmentResidents)
                {
                    apartmentResident.IsOwner = false;
                    _apartmentResidentRepository.Update(apartmentResident);
                }
            }

            var entity = _mapper.Map<ApartmentResident>(apartmentResidentDTO);
            _apartmentResidentRepository.Update(entity);

            await _apartmentResidentRepository.SaveAsync();
        }

        public async Task DeleteApartmentResidentAsync(int apartmentId, int residentId)
        {
            var apartmentResident = await _apartmentResidentRepository.Get(ar => ar.ApartmentId == apartmentId && ar.ResidentId == residentId)
                .Include(ar => ar.Apartment)
                .FirstOrDefaultAsync();

            if (apartmentResident == null)
            {
                throw new InvalidOperationException("The specified ApartmentResident does not exist.");
            }

            apartmentResident.Apartment!.ResidentCount -= 1;

            _apartmentResidentRepository.Delete(apartmentResident);

            await _apartmentResidentRepository.SaveAsync();
        }
    }
}
