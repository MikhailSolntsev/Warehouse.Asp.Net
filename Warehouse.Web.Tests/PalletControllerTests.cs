using Warehouse.Data;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using Warehouse.Web.Models;
using Warehouse.Web.Api.Controllers;
using FluentAssertions;
using AutoMapper;
using Warehouse.Web.Api.Infrastructure.Mapping;
using Warehouse.Web.Api.Infrastructure.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Web.Api
{
    public class PalletControllerTests
    {
        private readonly PalletController controller;
        private readonly DateTime today;
        public PalletControllerTests()
        {
            string fileName = Path.GetRandomFileName();
            IWarehouseContext context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
                cfg.AddProfile(typeof(DtoMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            IPalletStorage storage = new PalletStorage(context, mapper);
            controller = new(storage, mapper);

            today = DateTime.Today;
        }

        [Fact(DisplayName = "Can create Pallet without Boxes")]
        public async Task CanCreatePalletWithoutBoxes()
        {
            PalletCreateDto model = new()
            {
                Length = 3,
                Width = 3,
                Height = 3
            };
            var response = await controller.CreatePallet(model);

            response.Should().BeAssignableTo<ObjectResult>();

            var value = ((ObjectResult)response).Value;
            value.Should().NotBeNull().And.BeAssignableTo<PalletResponseDto>();

        }

        [Fact(DisplayName = "Can create Pallet(without Id) with Boxes(without Id)")]
        public async Task CanCreatePalletWithBoxesWithoutId()
        {
            // Arrange
            PalletCreateDto model = new()
            {
                Length = 13,
                Width = 13,
                Height = 13,
                Boxes = new List<BoxCreateDto>()
                {
                    new BoxCreateDto()
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
            response.Should().BeAssignableTo<ObjectResult>();

            var value = ((ObjectResult)response).Value;
            value.Should().NotBeNull().And.BeAssignableTo<PalletResponseDto>();

        }

        [Fact(DisplayName = "Can create Pallet(with Id) with Boxes(wit Id)")]
        public async Task CanCreatePalletWithBoxesWithId()
        {
            // Arrange
            PalletCreateDto model = new()
            {
                Id = 17,
                Length = 13,
                Width = 15,
                Height = 11,
                Boxes = new List<BoxCreateDto>()
                {
                    new ()
                    {
                        Id = 7,
                        Length = 3,
                        Width = 5,
                        Height = 2,
                        Weight = 1,
                        ExpirationDate = today
                    }
                }
            };

            // Act
            var response = await controller.CreatePallet(model);

            // Assert
            response.Should().BeAssignableTo<ObjectResult>();

            var value = ((ObjectResult)response).Value;
            value.Should().NotBeNull().And.BeAssignableTo<PalletResponseDto>();

            var dto = (PalletResponseDto?)value;
            dto.Should().NotBeNull();
            dto?.Id.Should().Be(17);
            dto?.Length.Should().Be(13);
            dto?.Width.Should().Be(15);
            dto?.Height.Should().Be(11);
            dto?.Boxes.Should().NotBeNull().And.HaveCount(1);
            dto?.Boxes?[0].Id.Should().Be(7);
            dto?.Boxes?[0].Length.Should().Be(3);
            dto?.Boxes?[0].Width.Should().Be(5);
            dto?.Boxes?[0].Height.Should().Be(2);
            dto?.Boxes?[0].Weight.Should().Be(1);
            dto?.Boxes?[0].ExpirationDate.Should().Be(today);
        }

        [Fact(DisplayName = "Can retrieve all Pallets")]
        public async Task CanRetrieveAllPallets()
        {
            PalletCreateDto model = new()
            {
                Length = 3,
                Width = 3,
                Height = 3
            };

            await controller.CreatePallet(model);
            await controller.CreatePallet(model);

            var response = await controller.GetPallets(2, 0);
            response.Should().NotBeNull().And.BeAssignableTo<IList<PalletResponseDto>>().And.HaveCount(2);
        }

        [Fact(DisplayName = "Can retrieve Pallet by Id")]
        public async Task CanRetrievePalletById()
        {
            // Arrange
            PalletCreateDto model = new()
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
            response.Should().BeAssignableTo<ObjectResult>();

            var value = ((ObjectResult)response).Value;
            value.Should().NotBeNull().And.BeAssignableTo<PalletResponseDto>();

            var dto = (PalletResponseDto?)value;
            dto.Should().NotBeNull();
            dto?.Length.Should().Be(5);
        }

    }
}
