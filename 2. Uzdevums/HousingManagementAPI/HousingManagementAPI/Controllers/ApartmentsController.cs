using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IRepository<Apartment> _apartmentRepository;

        public ApartmentsController(IRepository<Apartment> apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        [HttpGet("GetAllApartments")]
        public async Task<ActionResult<IEnumerable<Apartment>>> GetAllApartments()
        {
            return await _apartmentRepository.GetAll().ToListAsync();
        }

        [HttpGet("GetApartment/{id}")]
        public async Task<ActionResult<Apartment>> GetApartment(int id)
        {
            var apartment = await _apartmentRepository.Get(x => x.ApartmentId == id).FirstOrDefaultAsync();

            if (apartment == null)
            {
                return NotFound();
            }

            return apartment;
        }

        [HttpPost("CreateApartment")]
        public async Task<ActionResult<Apartment>> CreateApartment(Apartment apartment)
        {
            _apartmentRepository.Add(apartment);
            await _apartmentRepository.SaveAsync();

            return CreatedAtAction(nameof(GetApartment), new { id = apartment.ApartmentId }, apartment);
        }

        [HttpPut("EditApartment")]
        public async Task<ActionResult> EditApartment(Apartment apartment)
        {
            _apartmentRepository.Update(apartment);
            await _apartmentRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("RemoveApartment/{id}")]
        public async Task<ActionResult> RemoveApartment(int id)
        {
            var apartment = await _apartmentRepository.Get(x => x.ApartmentId == id).FirstOrDefaultAsync();

            if (apartment == null)
            {
                return NotFound();
            }

            _apartmentRepository.Delete(apartment);
            await _apartmentRepository.SaveAsync();

            return NoContent();
        }
    }
}
