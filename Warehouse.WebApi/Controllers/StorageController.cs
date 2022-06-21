using Warehouse.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private IScalableStorage storage;

        public StorageController(IScalableStorage injectedStorage)
        {
            storage = injectedStorage;
        }

    }
}
