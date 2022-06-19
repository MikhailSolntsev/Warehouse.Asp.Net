using Warehouse.Data;
using Warehouse.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Warehouse.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private IScalableStorage storage;

        public WarehouseController(IScalableStorage injectedStorage)
        {
            storage = injectedStorage;
        }

        [HttpGet("Pallets")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PalletDto>))]
        public async Task<IEnumerable<PalletDto>> GetPallets()
        {
            var pallets = await storage.GetAllPalletsAsync();

            return pallets.Select(pallet => pallet.ToPalletDto()).AsEnumerable();
        }

        [HttpGet("Pallet/{id}", Name = nameof(GetPallet))]
        [ProducesResponseType(200, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPallet(int id)
        {
            if (id == 0)
            {
                ResponseMessage response = new() { Message = "Id parameter is not set" };
                return BadRequest(response);
            }

            var pallet = await storage.GetPalletAsync(id);

            if (pallet is null)
            {
                ResponseMessage response = new() { Message = $"Can't find pallet wit id = {id}" };
                return NotFound(response);
            }
            return Ok(pallet.ToPalletDto());
        }

        [HttpPost("Pallet")]
        [ProducesResponseType(201, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for pallet" };
                return BadRequest(response);
            }

            storage.AddPalletAsync(palletDto.ToPallet());

            return Ok();
        }
    }
}
