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
        private readonly IScalableStorage storage;
        private readonly IMapper mapper;
        private readonly IValidator<PalletDto> validator;

        public PalletController(IScalableStorage storage, IMapper mapper, IValidator<PalletDto> validator)
        {
            this.storage = storage;
            this.mapper = mapper;
            this.validator = validator;
        }

        /// <summary>
        /// Gets $take pallets with pagination, can skip pallets
        /// </summary>
        /// <param name="skip">Pallets to skip</param>
        /// <param name="take">Pallets to get. All if 0</param>
        /// <returns></returns>
        [HttpGet("Pallets")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PalletDto>))]
        public async Task<IEnumerable<PalletDto>> GetPallets([BindRequired] int take, int? skip)
        {
            var pallets = await storage.GetAllPalletsAsync(take, skip);

            return pallets.Select(pallet => mapper.Map<PalletDto>(pallet)).AsEnumerable();
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

        /// <summary>
        /// Creates or updates pallet from model
        /// </summary>
        /// <param name="palletDto">model to create pallet</param>
        /// <returns></returns>
        [HttpPost("Pallet")]
        [ProducesResponseType(200, Type = typeof(PalletDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PalletDto>> CreatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for pallet" };
                return BadRequest(response);
            }

            ValidationResult result = await validator.ValidateAsync(palletDto);
            if (!result.IsValid)
            {
                return BadRequest(result.ToDictionary());
            } 

            Pallet? pallet = mapper.Map<Pallet>(palletDto);
            pallet = await storage.AddPalletAsync(pallet);

            if (pallet is null)
            {
                ResponseMessage response = new() { Message = "Error during creating/updating pallet" };
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
        [ProducesResponseType(404)]
        public async Task<ActionResult<PalletDto>> UpdatePallet([FromBody] PalletDto palletDto)
        {
            if (palletDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for pallet" };
                return BadRequest(response);
            }

            ValidationResult result = await validator.ValidateAsync(palletDto);
            if (!result.IsValid)
            {
                return BadRequest(result.ToDictionary());
            }

            var pallet = await storage.UpdatePalletAsync(mapper.Map<Pallet>(palletDto));

            return mapper.Map<PalletDto>(pallet);
        }

        /// <summary>
        /// Deletes pallet wit Id
        /// </summary>
        /// <param name="id">Id to find pallet</param>
        /// <returns></returns>
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
