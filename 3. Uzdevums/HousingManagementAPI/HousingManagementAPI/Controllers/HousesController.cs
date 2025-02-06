using HousingManagementAPI.DTOs;
using HousingManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private readonly IHouseService _houseService;

        public HousesController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpGet("GetAllHouses")]
        public async Task<ActionResult<IEnumerable<HouseDTO>>> GetAllHouses()
        {
            var houses = await _houseService.GetAllHousesAsync();
            return Ok(houses);
        }

        [HttpGet("GetHouse/{id}")]
        public async Task<ActionResult<HouseDetailsModel>> GetHouse(int id)
        {
            var house = await _houseService.GetHouseByIdAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            return Ok(house);
        }

        [HttpPost("CreateHouse")]
        public async Task<ActionResult<HouseDTO>> CreateHouse(HouseCreateModel house)
        {
            var createdHouse = await _houseService.CreateHouseAsync(house);
            return CreatedAtAction(nameof(GetHouse), new { id = createdHouse.HouseId }, createdHouse);
        }

        [HttpPut("EditHouse")]
        public async Task<ActionResult> EditHouse(HouseDTO house)
        {
            await _houseService.UpdateHouseAsync(house);
            return NoContent();
        }

        [HttpDelete("RemoveHouse/{id}")]
        public async Task<ActionResult> RemoveHouse(int id)
        {
            await _houseService.DeleteHouseAsync(id);
            return NoContent();
        }
    }
}
