using Warehouse.Data;
using Warehouse.Data.Infrastructure;
using Warehouse.Web.Dto;
using Warehouse.Web.Api.Controllers;
using FluentAssertions;
using AutoMapper;
using Warehouse.Web.Api.Infrastructure.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Web.Tests;

public class PalletControllerTests : IClassFixture<DataContextFixture>
{
    private readonly PalletController controller;
    private readonly DateTime today;
    private readonly CancellationToken token = CancellationToken.None;

    public PalletControllerTests(DataContextFixture fixture)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(EntityMappingProfile));
            cfg.AddProfile(typeof(DtoMappingProfile));
        });

        IMapper mapper = config.CreateMapper();

        IPalletStorage storage = new PalletStorage(fixture.Context, mapper);
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
        var response = await controller.CreatePallet(model, token);

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
        var response = await controller.CreatePallet(model, token);

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
        var response = await controller.CreatePallet(model, token);

        // Assert
        response.Should().BeAssignableTo<ObjectResult>();

        var value = ((ObjectResult)response).Value;
        value.Should().NotBeNull().And.BeAssignableTo<PalletResponseDto>();

        var dto = (PalletResponseDto?)value;
        dto.Should().NotBeNull();
        dto.Should().BeEquivalentTo(
            new {
                Id = 17,
                Length = 13,
                Width = 15,
                Height = 11,
                Boxes = new[] {
                    new {
                        Id = 7,
                        Length = 3,
                        Width = 5,
                        Height = 2,
                        Weight = 1,
                        ExpirationDate = today
                    }
                }
            });
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

        await controller.CreatePallet(model, token);
        await controller.CreatePallet(model, token);

        var filter = new PaginationFilter { Take = 2, Skip = 0 };
        var response = await controller.GetPallets(filter, token);

        response.Should().NotBeNull().And.BeAssignableTo<IList<PalletResponseDto>>().And.HaveCount(2);
    }

    [Fact(DisplayName = "Can retrieve Pallet by Id")]
    public async Task CanRetrievePalletById()
    {
        // Arrange
        PalletCreateDto model = new()
        {
            Id = 151,
            Length = 3,
            Width = 3,
            Height = 3
        };
        await controller.CreatePallet(model, token);
        model = new()
        {
            Id = 157,
            Length = 5,
            Width = 5,
            Height = 5
        };
        await controller.CreatePallet(model, token);
        model = new()
        {
            Id = 163,
            Length = 5,
            Width = 5,
            Height = 5
        };
        await controller.CreatePallet(model, token);

        // Act
        var response = await controller.GetPallet(157, token);

        // Assert
        response.Should().BeAssignableTo<ObjectResult>();

        var value = ((ObjectResult)response).Value;
        value.Should().NotBeNull().And.BeAssignableTo<PalletResponseDto>();

        var dto = (PalletResponseDto?)value;
        dto.Should().NotBeNull();
        dto?.Length.Should().Be(5);
    }

}
