using HousingManagementAPI.Interfaces;
using HousingManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private readonly IRepository<House> _houseRepository;

        public HousesController(IRepository<House> houseRepository)
        {
            _houseRepository = houseRepository;
        }

        [HttpGet("GetAllHouses")]
        public async Task<ActionResult<IEnumerable<House>>> GetAllHouses()
        {
            return await _houseRepository.GetAll().ToListAsync();
        }

        [HttpGet("GetHouse/{id}")]
        public async Task<ActionResult<House>> GetHouse(int id)
        {
            var house = await _houseRepository.Get(x => x.HouseId == id).FirstOrDefaultAsync();

            if (house == null)
            {
                return NotFound();
            }

            return house;
        }

        [HttpPost("CreateHouse")]
        public async Task<ActionResult<House>> CreateHouse(House house)
        {
            _houseRepository.Add(house);
            await _houseRepository.SaveAsync();

            return CreatedAtAction(nameof(GetHouse), new { id = house.HouseId }, house);
        }

        [HttpPut("EditHouse")]
        public async Task<ActionResult> EditHouse(House house)
        {
            _houseRepository.Update(house);
            await _houseRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("RemoveHouse")]
        public async Task<ActionResult> RemoveHouse(int id)
        {
            var house = await _houseRepository.Get(x => x.HouseId == id).FirstOrDefaultAsync();

            if (house == null)
            {
                return NotFound();
            }

            _houseRepository.Delete(house);
            await _houseRepository.SaveAsync();

            return NoContent();
        }
    }
}
