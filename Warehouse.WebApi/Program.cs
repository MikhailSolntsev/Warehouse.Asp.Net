using Warehouse.Data;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using Warehouse.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseUrls("https://localhost:6002");


builder.Services.AddScoped<IScalableStorage, ScalableStorage>();
builder.Services.AddScoped<WarehouseContext, WarehouseSqliteContext>();

builder.Services.AddAutoMapper(typeof(DtoMappingProfile), typeof(ModelMappingProfile));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
