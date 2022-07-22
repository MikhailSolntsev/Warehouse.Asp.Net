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
        private readonly IBoxStorage storage;
        private readonly IMapper mapper;
        private readonly IValidator<BoxDto> validator;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="mapper"></param>
        public BoxController(IBoxStorage storage, IMapper mapper, IValidator<BoxDto> validator)
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
        [ProducesResponseType(404, Type = typeof(ResponseMessage))]
        public async Task<ActionResult<BoxDto>> GetBox([BindRequired] int id)
        {
            if (id == 0)
            {
                ResponseMessage response = new("Id parameter should be greater than 0");
                return BadRequest(response);
            }

            var box = await storage.GetBoxAsync(id);

            if (box is null)
            {
                ResponseMessage response = new($"Can't find box wit id = {id}");
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
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        public async Task<ActionResult<BoxDto>> CreateBox([FromBody] BoxDto boxDto)
        {
            if (boxDto is null)
            {
                ResponseMessage response = new("Bad model for box");
                return BadRequest(response);
            }

            ValidationResult result = await validator.ValidateAsync(boxDto);
            if (!result.IsValid)
            {
                return BadRequest(result.ToDictionary());
            }

            BoxModel? box = await storage.AddBoxAsync(mapper.Map<BoxModel>(boxDto));

            if (box is null)
            {
                ResponseMessage response = new("Error during creating/updating box");
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
        [ProducesResponseType(200, Type = typeof(BoxDto))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        public async Task<ActionResult<BoxDto>> UpdateBox([FromBody] BoxDto boxDto)
        {
            if (boxDto is null)
            {
                ResponseMessage response = new("Bad model for box");
                return BadRequest(response);
            }

            ValidationResult result = await validator.ValidateAsync(boxDto);
            if (!result.IsValid)
            {
                var message = new ResponseMessage(result.ToDictionary());
                return BadRequest(message);
            }

            var box = await storage.UpdateBoxAsync(mapper.Map<BoxModel>(boxDto));

            return mapper.Map<BoxDto>(box);
        }

        /// <summary>
        /// Deletes box wit Id
        /// </summary>
        /// <param name="id">Id to find box</param>
        /// <returns></returns>
        [HttpDelete("Box/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> DeleteBox(int id)
        {
            bool deleted = await storage.DeleteBoxAsync(id);

            if (deleted)
            {
                return NoContent();
            }

            ResponseMessage response = new($"Box with Id={id} was not found");
            return NotFound(response);
        }

    }
}
