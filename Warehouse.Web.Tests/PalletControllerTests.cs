using Warehouse.Data;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using Warehouse.Web.Models;
using Warehouse.Web.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Warehouse.Web.Api
{
    public class PalletControllerTests
    {
        private PalletController controller;

        public PalletControllerTests()
        {
            string fileName = Path.GetRandomFileName();
            WarehouseContext context = new WarehouseSqliteContext(fileName);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
                cfg.AddProfile(typeof(DtoMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            ScalableStorage storage = new(context, mapper);
            controller = new(storage, mapper);
        }

        [Fact(DisplayName = "Can create Pallet without Boxes")]
        public async Task CanCreatePalletWithoutBoxes()
        {
            PalletDto model = new()
            {
                Length = 3,
                Width = 3,
                Height = 3
            };
            var response = await controller.CreatePallet(model);

            response.Should().BeAssignableTo<OkObjectResult>();

            var result = response as OkObjectResult;

            result?.Value.Should().NotBeNull().And.BeAssignableTo<PalletDto>();
        }

        [Fact(DisplayName = "Can create Pallet(without Id) with Boxes(without Id)")]
        public async Task CanCreatePalletWithBoxesWithoutId()
        {
            // Assign
            PalletDto model = new()
            {
                Length = 13,
                Width = 13,
                Height = 13,
                Boxes = new List<BoxDto>()
                {
                    new BoxDto()
                    {
                        Length = 3,
                        Width = 3,
                        Height = 3,
                        Weight = 3,
                        ExpirationDate = DateTime.Today
                    }
                }
            };

            // Act
            var response = await controller.CreatePallet(model);

            // Assert
            response.Should().BeAssignableTo<OkObjectResult>();

            (response as OkObjectResult)?.Value.Should().NotBeNull().And.BeAssignableTo<PalletDto>();
        }

        [Fact(DisplayName = "Can create Pallet(with Id) with Boxes(wit Id)")]
        public async Task CanCreatePalletWithBoxesWithId()
        {
            // Assign
            PalletDto model = new()
            {
                Id = 17,
                Length = 13,
                Width = 13,
                Height = 13,
                Boxes = new List<BoxDto>()
                {
                    new BoxDto()
                    {
                        Id = 17,
                        Length = 3,
                        Width = 3,
                        Height = 3,
                        Weight = 3,
                        ExpirationDate = DateTime.Today
                    }
                }
            };

            // Act
            var response = await controller.CreatePallet(model);

            // Assert
            response.Should().BeAssignableTo<OkObjectResult>();

            (response as OkObjectResult)?.Value.Should().NotBeNull().And.BeAssignableTo<PalletDto>();
        }

        [Fact(DisplayName = "Can retrieve all Pallets")]
        public async Task CanRetrieveAllPallets()
        {
            PalletDto model = new()
            {
                Length = 3,
                Width = 3,
                Height = 3
            };

            await controller.CreatePallet(model);
            await controller.CreatePallet(model);

            var response = await controller.GetPallets(0, 0);
            response.Should().NotBeNull().And.BeAssignableTo<IEnumerable<PalletDto>>().And.HaveCount(2);
        }

        [Fact(DisplayName = "Can retrieve Pallet by Id")]
        public async Task CanRetrievePalletById()
        {
            // Assign
            PalletDto model = new()
            {
                Length = 3,
                Width = 3,
                Height = 3
            };
            await controller.CreatePallet(model);
            model = new()
            {
                Length = 5,
                Width = 5,
                Height = 5
            };
            await controller.CreatePallet(model);
            model = new()
            {
                Length = 5,
                Width = 5,
                Height = 5
            };
            await controller.CreatePallet(model);

            // Act
            var response = await controller.GetPallet(2);

            // Assert
            response.Value.Length.Should().Be(5);
        }

    }
}
