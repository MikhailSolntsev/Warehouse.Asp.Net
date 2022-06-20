using Warehouse.Data;
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
        public async Task<IEnumerable<BoxDto>> GetPallets()
        {
            var boxes = await storage.GetAllBoxesAsync();

            return boxes.Select(box => box.ToBoxDto()).AsEnumerable();
        }

    }
}
