using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : ControllerBase
    {
        private readonly IRepository<Resident> _residentRepository;

        public ResidentsController(IRepository<Resident> residentRepository)
        {
            _residentRepository = residentRepository;
        }

        [HttpGet("GetAllResidents")]
        public async Task<ActionResult<IEnumerable<Resident>>> GetAllResidents()
        {
            return await _residentRepository.GetAll().ToListAsync();
        }

        [HttpGet("GetResident/{id}")]
        public async Task<ActionResult<Resident>> GetResident(int id)
        {
            var resident = await _residentRepository.Get(x => x.ResidentId == id).FirstOrDefaultAsync();

            if (resident == null)
            {
                return NotFound();
            }

            return resident;
        }

        [HttpPost("CreateResident")]
        public async Task<ActionResult<Resident>> CreateResident(Resident resident)
        {
            _residentRepository.Add(resident);
            await _residentRepository.SaveAsync();

            return CreatedAtAction(nameof(GetResident), new { id = resident.ResidentId }, resident);
        }

        [HttpPut("EditResident")]
        public async Task<ActionResult> EditResident(Resident resident)
        {
            _residentRepository.Update(resident);
            await _residentRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("RemoveResident/{id}")]
        public async Task<ActionResult> RemoveResident(int id)
        {
            var resident = await _residentRepository.Get(x => x.ResidentId == id).FirstOrDefaultAsync();

            if (resident == null)
            {
                return NotFound();
            }

            _residentRepository.Delete(resident);
            await _residentRepository.SaveAsync();

            return NoContent();
        }
    }
}
