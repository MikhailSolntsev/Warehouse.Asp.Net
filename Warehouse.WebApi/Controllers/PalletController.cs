using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Web.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class PalletController : ControllerBase
    {
        private readonly IPalletStorage storage;
        private readonly IMapper mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="mapper"></param>
        public PalletController(IPalletStorage storage, IMapper mapper)
        {
            this.storage = storage;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets $take pallets with pagination, can skip pallets
        /// </summary>
        /// <param name="skip">Pallets to skip</param>
        /// <param name="take">Pallets to get. All if 0</param>
        /// <returns></returns>
        [HttpGet("Pallets")]
        [ProducesResponseType(200, Type = typeof(IList<PalletDto>))]
        public async Task<IList<PalletDto>> GetPallets([BindRequired]int take, int? skip)
        {
            var pallets = await storage.GetAllPalletsAsync(take, skip);

            return pallets.Select(pallet => mapper.Map<PalletDto>(pallet)).ToList();
        }

        /// <summary>
        /// Gets specific pallet with Id
        /// </summary>
        /// <param name="id">Id to find Pallet</param>
        /// <returns></returns>
        [HttpGet("Pallet/{id}", Name = nameof(GetPallet))]
        [ProducesResponseType(200, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PalletDto>> GetPallet(int id)
        {
            if (id == 0)
            {
                ResponseMessage response = new("Id parameter is not set");
                return BadRequest(response);
            }

            var pallet = await storage.GetPalletAsync(id);

            if (pallet is null)
            {
                ResponseMessage response = new($"Can't find pallet wit id = {id}");
                return NotFound(response);
            }
            return mapper.Map<PalletDto>(pallet);
        }

        /// <summary>
        /// Creates or updates pallet from model
        /// </summary>
        /// <param name="palletDto">model to create pallet</param>
        /// <returns></returns>
        [HttpPost("Pallet")]
        [ProducesResponseType(200, Type = typeof(PalletDto))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        public async Task<ActionResult<PalletDto>> CreatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new("Bad model for pallet");
                return BadRequest(response);
            }

            PalletModel? pallet = mapper.Map<PalletModel>(palletDto);
            pallet = await storage.AddPalletAsync(pallet);

            if (pallet is null)
            {
                ResponseMessage response = new("Error during creating/updating pallet");
                return BadRequest(response);
            }
            return mapper.Map<PalletDto>(pallet);
        }

        /// <summary>
        /// Updates pallet
        /// </summary>
        /// <param name="palletDto">model to update pallet</param>
        /// <returns></returns>
        [HttpPut("Pallet")]
        [ProducesResponseType(204, Type = typeof(PalletDto))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        public async Task<ActionResult<PalletDto>> UpdatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new("Bad model for pallet");
                return BadRequest(response);
            }

            var pallet = await storage.UpdatePalletAsync(mapper.Map<PalletModel>(palletDto));

            return mapper.Map<PalletDto>(pallet);
        }

        /// <summary>
        /// Deletes pallet wit Id
        /// </summary>
        /// <param name="id">Id to find pallet</param>
        /// <returns></returns>
        [HttpDelete("Pallet/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> DeletePallet(int id)
        {
            bool deleted = await storage.DeletePalletAsync(id);
            
            if (deleted)
            {
                return NoContent();
            }

            ResponseMessage response = new($"Box with Id={id} was not found");
            return NotFound(response);
        }

    }
}
