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
    [Route("api/")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly IScalableStorage storage;
        private readonly IMapper mapper;
        private readonly IValidator<BoxDto> validator;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="mapper"></param>
        public BoxController(IScalableStorage storage, IMapper mapper, IValidator<BoxDto> validator)
        {
            this.storage = storage;
            this.mapper = mapper;
            this.validator = validator;
        }

        /// <summary>
        /// Gets $take boxes with pagination, can skip boxes
        /// </summary>
        /// <param name="skip">Boxes to skip</param>
        /// <param name="take">Boxes to take</param>
        /// <returns></returns>
        [HttpGet("Boxes")]
        [ProducesResponseType(200, Type = typeof(IList<BoxDto>))]
        public async Task<IList<BoxDto>> GetBoxes([BindRequired] int take, int? skip)
        {
            var boxes = await storage.GetAllBoxesAsync(take, skip);

            return boxes.Select(box => mapper.Map<BoxDto>(box)).ToList();
        }

        /// <summary>
        /// Gets specific box with Id
        /// </summary>
        /// <param name="id">Id to find Box</param>
        /// <returns></returns>
        [HttpGet("Box/{id}", Name = nameof(GetBox))]
        [ProducesResponseType(200, Type = typeof(BoxDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BoxDto>> GetBox(int id)
        {
            if (id == 0)
            {
                ResponseMessage response = new() { Message = "Id parameter is not set" };
                return BadRequest(response);
            }

            var box = await storage.GetBoxAsync(id);

            if (box is null)
            {
                ResponseMessage response = new() { Message = $"Can't find box wit id = {id}" };
                return NotFound(response);
            }
            return mapper.Map<BoxDto>(box);
        }

        /// <summary>
        /// Creates or updates box from model
        /// </summary>
        /// <param name="boxDto">model to create box</param>
        /// <returns></returns>
        [HttpPost("Box")]
        [ProducesResponseType(200, Type = typeof(BoxDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BoxDto>> CreateBox([FromBody] BoxDto boxDto)
        {
            if (boxDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for box" };
                return BadRequest(response);
            }

            ValidationResult result = await validator.ValidateAsync(boxDto);
            if (!result.IsValid)
            {
                return BadRequest(result.ToDictionary());
            }

            Box? box = await storage.AddBoxAsync(mapper.Map<Box>(boxDto));

            if (box is null)
            {
                ResponseMessage response = new() { Message = "Error during creating/updating box" };
                return BadRequest(response);
            }
            return mapper.Map<BoxDto>(box);
        }

        /// <summary>
        /// Updates Box
        /// </summary>
        /// <param name="boxDto">model to update box</param>
        /// <returns></returns>
        [HttpPut("Box")]
        [ProducesResponseType(204, Type = typeof(BoxDto))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BoxDto>> UpdateBox([FromBody] BoxDto boxDto)
        {
            if (boxDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for box" };
                return BadRequest(response);
            }

            ValidationResult result = await validator.ValidateAsync(boxDto);
            if (!result.IsValid)
            {
                return BadRequest(result.ToDictionary());
            }

            var box = await storage.UpdateBoxAsync(mapper.Map<Box>(boxDto));

            return mapper.Map<BoxDto>(box);
        }

        /// <summary>
        /// Deletes box wit Id
        /// </summary>
        /// <param name="id">Id to find box</param>
        /// <returns></returns>
        [HttpDelete("Box/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBox(int id)
        {
            var box = await storage.GetBoxAsync(id);

            if (box is null)
            {
                return NoContent();
            }

            bool deleted = await storage.DeleteBoxAsync(id);

            if (deleted)
            {
                return Ok();
            }

            ResponseMessage response = new() { Message = "Error during deleting box" };
            return BadRequest(response);
        }

    }
}
