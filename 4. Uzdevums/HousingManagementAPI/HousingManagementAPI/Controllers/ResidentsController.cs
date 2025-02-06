using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : ControllerBase
    {
        private readonly IResidentService _residentService;

        public ResidentsController(IResidentService residentService)
        {
            _residentService = residentService;
        }

        [HttpGet("GetAllResidents")]
        public async Task<ActionResult<IEnumerable<ResidentDTO>>> GetAllResidents()
        {
            var residents = await _residentService.GetAllResidentsAsync();
            return Ok(residents);
        }

        [HttpGet("GetResident/{id}")]
        public async Task<ActionResult<ResidentDetailsModel>> GetResident(int id)
        {
            var resident = await _residentService.GetResidentByIdAsync(id);

            if (resident == null)
            {
                return NotFound();
            }

            return Ok(resident);
        }

        [HttpPost("CreateResident")]
        public async Task<ActionResult<ResidentDTO>> CreateResident(ResidentCreateModel resident)
        {
            var createdResident = await _residentService.CreateResidentAsync(resident);
            return CreatedAtAction(nameof(GetResident), new { id = createdResident.ResidentId }, createdResident);
        }

        [HttpPut("EditResident")]
        public async Task<ActionResult> EditResident(ResidentDTO resident)
        {
            await _residentService.UpdateResidentAsync(resident);
            return NoContent();
        }

        [HttpDelete("RemoveResident/{id}")]
        public async Task<ActionResult> RemoveResident(int id)
        {
            await _residentService.DeleteResidentAsync(id);
            return NoContent();
        }
    }
}
