using AutoMapper;
using Warehouse.Data.Infrastructure;
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
                cfg.AddProfile(typeof(EntityMappingProfile));
                cfg.AddProfile(typeof(DtoMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
