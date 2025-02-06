using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentService _apartmentService;

        public ApartmentsController(IApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [HttpGet("GetAllApartments")]
        public async Task<ActionResult<IEnumerable<ApartmentDTO>>> GetAllApartments()
        {
            var apartments = await _apartmentService.GetAllApartmentsAsync();
            return Ok(apartments);
        }

        [HttpGet("GetApartment/{id}")]
        public async Task<ActionResult<ApartmentDetailsModel>> GetApartment(int id)
        {
            var apartment = await _apartmentService.GetApartmentByIdAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            return Ok(apartment);
        }

        [HttpPost("CreateApartment")]
        public async Task<ActionResult<ApartmentDTO>> CreateApartment(ApartmentCreateModel apartment)
        {
            var createdApartment = await _apartmentService.CreateApartmentAsync(apartment);
            return CreatedAtAction(nameof(GetApartment), new { id = createdApartment.ApartmentId }, createdApartment);
        }

        [HttpPut("EditApartment")]
        public async Task<ActionResult> EditApartment(ApartmentDTO apartment)
        {
            await _apartmentService.UpdateApartmentAsync(apartment);
            return NoContent();
        }

        [HttpDelete("RemoveApartment/{id}")]
        public async Task<ActionResult> RemoveApartment(int id)
        {
            await _apartmentService.DeleteApartmentAsync(id);
            return NoContent();
        }
    }
}
