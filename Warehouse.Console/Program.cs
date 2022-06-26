using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
//using Automapper;

//string fileName = "../Warehouse.db";

WarehouseContext context = new WarehouseSqliteContext();

//ScalableStorage storage = new(context);

//Pallet pallet = new(29, 37, 47, 3);
//storage.AddPalletAsync(pallet);

Console.WriteLine("I made it!");
