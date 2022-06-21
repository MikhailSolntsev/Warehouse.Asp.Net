using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Warehouse.Web.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private IScalableStorage storage;

        public BoxController(IScalableStorage injectedStorage)
        {
            storage = injectedStorage;
        }

        [HttpGet("Boxes")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BoxDto>))]
        public async Task<IEnumerable<BoxDto>> GetBoxes()
        {
            var boxes = await storage.GetAllBoxesAsync();

            return boxes.Select(box => box.ToBoxDto()).AsEnumerable();
        }

        [HttpGet("Box/{id}", Name = nameof(GetBox))]
        [ProducesResponseType(200, Type = typeof(BoxDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBox(int id)
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
            return Ok(box.ToBoxDto());
        }

        [HttpPost("Box")]
        [ProducesResponseType(200, Type = typeof(BoxDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateBox([FromBody] BoxDto boxDto)
        {
            if (boxDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for box" };
                return BadRequest(response);
            }

            Box? box = null;

            if (boxDto.Id is not null)
            {
                box = await storage.GetBoxAsync(boxDto.Id ?? 0);
            }

            if (box is null)
            {
                box = await storage.AddBoxAsync(boxDto.ToBox());
            }
            else
            {
                box = await storage.UpdateBoxAsync(boxDto.ToBox());
            }

            if (box is null)
            {
                ResponseMessage response = new() { Message = "Error during creating/updating box" };
                return BadRequest(response);
            }
            return Ok(box.ToBoxDto());
        }

        [HttpPut("Box")]
        [ProducesResponseType(204, Type = typeof(BoxDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBox([FromBody] BoxDto boxDto)
        {
            if (boxDto is null)
            {
                ResponseMessage response = new() { Message = "Bad model for box" };
                return BadRequest(response);
            }

            var box = await storage.UpdateBoxAsync(boxDto.ToBox());

            return Ok(box?.ToBoxDto());
        }

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
