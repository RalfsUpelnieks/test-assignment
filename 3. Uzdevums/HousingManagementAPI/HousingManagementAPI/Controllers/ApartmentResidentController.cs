using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentResidentController : ControllerBase
    {
        private readonly IApartmentResidentService _apartmentResidentService;

        public ApartmentResidentController(IApartmentResidentService apartmentResidentService)
        {
            _apartmentResidentService = apartmentResidentService;
        }

        [HttpPost("CreateApartmentResident")]
        public async Task<ActionResult> CreateApartmentResident(ApartmentResidentDTO apartmentResident)
        {
            var createdApartmentResident = await _apartmentResidentService.CreateApartmentResidentAsync(apartmentResident);
            return Ok(createdApartmentResident);
        }

        [HttpPut("EditApartmentResident")]
        public async Task<ActionResult> EditApartmentResident(ApartmentResidentDTO apartmentResident)
        {
            await _apartmentResidentService.UpdateApartmentResidentAsync(apartmentResident);
            return NoContent();
        }

        [HttpDelete("RemoveApartmentResident/{apartmentId}/{residentId}")]
        public async Task<ActionResult> RemoveApartmentResident(int apartmentId, int residentId)
        {
            await _apartmentResidentService.DeleteApartmentResidentAsync(apartmentId, residentId);
            return NoContent();
        }
    }
}
