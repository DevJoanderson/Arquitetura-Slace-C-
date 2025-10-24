using Api.AppDb;
using Api.Features.Orders.Create;
using Api.Features.Orders.GetById;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Features.Orders.GetById;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// ====== Services (DI) ======
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

// FluentValidation (varre o assembly e registra validators)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// ====== Pipeline (Middleware) ======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ====== Endpoints Minimal API ======
var orders = app.MapGroup("/orders").WithTags("Orders");

// POST /orders  -> cria pedido
orders.MapPost("/", async (CreateOrderCommand cmd, ISender mediator) =>
{
    var id = await mediator.Send(cmd);
    return Results.Created($"/orders/{id}", new { id });
})
.Produces(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.WithName("CreateOrder");

// GET /orders/{id} -> busca por Id
orders.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
{
    var dto = await mediator.Send(new GetOrderByIdQuery(id));
    return Results.Ok(dto);
})
.Produces<OrderDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetOrderById");

app.Run();
