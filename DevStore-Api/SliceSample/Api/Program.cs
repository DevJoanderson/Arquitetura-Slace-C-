using Api.AppDb;
using Api.Features.Orders.Create;
using Api.Features.Orders.GetById;
using Api.Features.Orders.List;
using Api.Features.Orders.Delete;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("AppDb"));

// MediatR (varre o assembly atual e registra handlers)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// FluentValidation (auto-validação + scan do assembly)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// ===== Pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ===== Endpoints (Orders) =====
app.MapCreateOrderEndpoint();
app.MapGetOrderByIdEndpoint();
app.MapListOrdersEndpoint();
app.MapDeleteOrderEndpoint();

// (Removido o exemplo /weatherforecast)

app.Run();
