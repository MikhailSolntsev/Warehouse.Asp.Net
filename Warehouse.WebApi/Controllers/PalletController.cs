using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Web.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Warehouse.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class PalletController : ControllerBase
    {
        private IScalableStorage storage;
        private IMapper mapper;

        public PalletController(IScalableStorage injectedStorage, IMapper injectedMapper)
        {
            storage = injectedStorage;
            mapper = injectedMapper;
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
        public async Task<IEnumerable<PalletDto>> GetPallets(int skip = 0, int count = 0)
        {
            var pallets = await storage.GetAllPalletsAsync(skip, count);

            return pallets.Select(pallet => mapper.Map<PalletDto>(pallet)).AsEnumerable();
        }

        [HttpGet("Pallet/{id}", Name = nameof(GetPallet))]
        [ProducesResponseType(200, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PalletDto>> GetPallet(int id)
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
            return mapper.Map<PalletDto>(pallet);
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
                pallet = mapper.Map<Pallet>(palletDto);
                pallet = await storage.AddPalletAsync(pallet);
            }
            else
            {
                pallet = mapper.Map<Pallet>(palletDto);
                pallet = await storage.UpdatePalletAsync(pallet);
            }

            if (pallet is null)
            {
                ResponseMessage response = new() { Message = "Error during creating/updating pallet" };
                return BadRequest(response);
            }
            return Ok(mapper.Map<PalletDto>(pallet));
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

            var pallet = await storage.UpdatePalletAsync(mapper.Map<Pallet>(palletDto));

            return Ok(mapper.Map<PalletDto>(pallet));
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
