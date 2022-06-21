using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Warehouse.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class PalletController : ControllerBase
    {
        private IScalableStorage storage;

        public PalletController(IScalableStorage injectedStorage)
        {
            storage = injectedStorage;
        }

        //[HttpGet("Pallets")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<PalletDto>))]
        //public async Task<IEnumerable<PalletDto>> GetPallets()
        //{
        //    var pallets = await storage.GetAllPalletsAsync();

        //    return pallets.Select(pallet => pallet.ToPalletDto()).AsEnumerable();
        //}

        [HttpGet("Pallets")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PalletDto>))]
        public async Task<IEnumerable<PalletDto>> GetPallets(int? skip, int? count)
        {
            var pallets = await storage.GetAllPalletsAsync(skip ?? 0, count ?? 0);

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
        [ProducesResponseType(200, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for pallet" };
                return BadRequest(response);
            }

            Pallet? pallet = null;

            if (palletDto.Id is not null)
            {
                pallet = await storage.GetPalletAsync(palletDto.Id ?? 0);
            }
            
            if (pallet is null)
            {
                pallet = palletDto.ToPallet();
                pallet = await storage.AddPalletAsync(pallet);
            }
            else
            {
                pallet = palletDto.ToPallet();
                pallet = await storage.UpdatePalletAsync(pallet);
            }

            if (pallet is null)
            {
                ResponseMessage response = new() { Message = "Error during creating/updating pallet" };
                return BadRequest(response);
            }
            return Ok(pallet.ToPalletDto());
        }

        [HttpPut("Pallet")]
        [ProducesResponseType(204, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for pallet" };
                return BadRequest(response);
            }

            var pallet = await storage.UpdatePalletAsync(palletDto.ToPallet());

            return Ok(pallet.ToPalletDto());
        }

        [HttpDelete("Pallet/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePallet(int id)
        {
            var pallet = await storage.GetPalletAsync(id);

            if (pallet is null)
            {
                return NoContent();
            }

            bool deleted = await storage.DeletePalletAsync(id);
            
            if (deleted)
            {
                return Ok();
            }

            ResponseMessage response = new() { Message = "Error during deleting pallet" };
            return BadRequest(response);
        }

    }
}
