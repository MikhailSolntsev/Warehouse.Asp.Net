using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Web.Models;
using Warehouse.Web.Api.Infrastructure.Validators;
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
        private readonly IValidationService validationService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="mapper"></param>
        public BoxController(IBoxStorage storage, IMapper mapper, IValidationService validationService)
        {
            this.storage = storage;
            this.mapper = mapper;
            this.validationService = validationService;
        }

        /// <summary>
        /// Gets $take boxes with pagination, can skip boxes
        /// </summary>
        /// <param name="skip">Boxes to skip</param>
        /// <param name="take">Boxes to take</param>
        /// <returns></returns>
        [HttpGet("Boxes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<BoxResponseDto>))]
        public async Task<IList<BoxResponseDto>> GetBoxes([BindRequired] int take, int? skip, CancellationToken token)
        {
            var boxes = await storage.GetAllBoxesAsync(take, skip, token);

            return boxes.Select(box => mapper.Map<BoxResponseDto>(box)).ToList();
        }

        /// <summary>
        /// Gets specific box with Id
        /// </summary>
        /// <param name="id">Id to find Box</param>
        /// <returns></returns>
        [HttpGet("Box/{id}", Name = nameof(GetBox))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoxResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> GetBox([BindRequired] int id, CancellationToken token)
        {
            if (id == 0)
            {
                ResponseMessage response = new("Id parameter should be greater than 0");
                return BadRequest(response);
            }

            var box = await storage.GetBoxAsync(id, token);

            if (box is null)
            {
                ResponseMessage response = new($"Can't find box wit id = {id}");
                return NotFound(response);
            }
            return Ok(mapper.Map<BoxResponseDto>(box));
        }

        /// <summary>
        /// Creates or updates box from model
        /// </summary>
        /// <param name="boxDto">model to create box</param>
        /// <returns></returns>
        [HttpPost("Box")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BoxResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> CreateBox([FromBody] BoxCreateDto boxDto, CancellationToken token)
        {
            var result = validationService.Validate<BoxCreateDto>(boxDto);
            if (!result.IsValid)
            {
                throw new ArgumentException("Box create model is invalid.");
            }

            BoxModel? box = await storage.AddBoxAsync(mapper.Map<BoxModel>(boxDto), token);

            return Created($"{Request.Path}/{box?.Id}", mapper.Map<BoxResponseDto>(box));
        }

        /// <summary>
        /// Updates Box
        /// </summary>
        /// <param name="boxDto">model to update box</param>
        /// <returns></returns>
        [HttpPut("Box")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoxResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> UpdateBox([FromBody] BoxUpdateDto boxDto, CancellationToken token)
        {
            var box = await storage.UpdateBoxAsync(mapper.Map<BoxModel>(boxDto), token);

            return Ok(mapper.Map<BoxResponseDto>(box));
        }

        /// <summary>
        /// Deletes box wit Id
        /// </summary>
        /// <param name="id">Id to find box</param>
        /// <returns></returns>
        [HttpDelete("Box/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> DeleteBox(int id, CancellationToken token)
        {
            bool deleted = await storage.DeleteBoxAsync(id, token);

            if (deleted)
            {
                return NoContent();
            }

            ResponseMessage response = new($"Box with Id={id} was not found");
            return NotFound(response);
        }

    }
}
