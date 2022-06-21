﻿using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;

namespace Warehouse.IntegrationTests
{
    public class ScalableStorageEntityContextTests
    {
        private ScalableStorage storage;
        private WarehouseContext context;

        public ScalableStorageEntityContextTests()
        {
            string fileName = Path.GetRandomFileName();
            context = new WarehouseSqliteContext(fileName);
            storage = new ScalableStorage(context);
        }

        [Fact(DisplayName = "Storage can add pallet")]
        public async Task CanAddPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);

            // Act
            await storage.AddPalletAsync(pallet);

            //Assert
            context.Pallets.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public async Task CanModifyPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            // Act
            pallet = new(13, 5, 7, 11);
            await storage.UpdatePalletAsync(pallet);

            //Assert
            context.Pallets?.Find(pallet.Id)?.Length.Should().Be(13);
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public async Task CanDeletePallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            // Act
            await storage.DeletePalletAsync(pallet);

            //Assert
            context.Pallets?.Find(pallet.Id).Should().BeNull();
        }

        [Fact(DisplayName = "Can add box to pallet")]
        public async Task CanAddBoxToPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            // Act
            await storage.AddBoxToPalletAsync(box, pallet);

            //Assert
            context.Boxes?.Find(pallet.Id).Should().BeNull();
        }
    }
}