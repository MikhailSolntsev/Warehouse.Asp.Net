using Autofac;
using Warehouse.EntityContext;

namespace Warehouse.EntityContext.Sqlite;

public static class EntityContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<WarehouseSqliteContext>().As<WarehouseContext>();

        return builder.Build();
    }
}
