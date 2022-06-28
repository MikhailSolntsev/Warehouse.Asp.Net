using Warehouse.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Warehouse.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private IScalableStorage storage;
        private IMapper mapper;

        public StorageController(IScalableStorage injectedStorage, IMapper injectedMapper)
        {
            storage = injectedStorage;
            mapper = injectedMapper;
        }

    }
}
