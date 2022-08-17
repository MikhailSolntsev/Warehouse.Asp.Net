using Warehouse.Data;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using Warehouse.Web.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Web.Api.Infrastructure.Mapping;
using Warehouse.Web.Api.Infrastructure.Validators;
using Warehouse.Web.Api.Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:6001");

builder.Services.AddValidatorsFromAssemblyContaining<PalletCreateValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidationService, ValidationService>();

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>();
    options.Filters.Add<OperationCancelledExceptionFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
