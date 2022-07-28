using AutoMapper;
using Warehouse.EntityContext.Models;
using Warehouse.Web.Api.Infrastructure.Mapping;

namespace Warehouse.IntegrationTests
{
    public class AutomapperTests
    {
        [Fact(DisplayName ="Automapper profiles test")]
        public void Test1()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
                cfg.AddProfile(typeof(DtoMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
