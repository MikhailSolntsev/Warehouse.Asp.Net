using Warehouse.Data;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using Warehouse.Web.Models;
using Warehouse.Web.Api.Infrastructure;
using FluentValidation;
using FluentValidation.Results;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:6002");

builder.Services.AddValidatorsFromAssemblyContaining<PalletValidator>();

builder.Services.AddScoped<IScalableStorage, ScalableStorage>();
builder.Services.AddScoped<IBoxStorage, BoxStorage>();
builder.Services.AddScoped<IPalletStorage, PalletStorage>();

builder.Services.AddScoped<IWarehouseContext, WarehouseSqliteContext>();

builder.Services.AddAutoMapper(typeof(DtoMappingProfile), typeof(ModelMappingProfile));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new()
    {
            Title = "Warehouse Service API",
            Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
