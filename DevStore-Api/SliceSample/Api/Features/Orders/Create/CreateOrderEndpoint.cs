using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Features.Orders.Create;

public static class CreateOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrderEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        // POST /orders
        group.MapPost("/", CreateAsync)
             .WithName("CreateOrder")
             .Produces<Created<object>>(201)
             .Produces<ValidationProblem>(400);

        return app;
    }

    // Handler do endpoint (Minimal API)
    private static async Task<IResult> CreateAsync(
        CreateOrderCommand command,
        ISender sender,
        IValidator<CreateOrderCommand> validator,
        CancellationToken ct)
    {
        // 1) Validação com FluentValidation (retorna 400 se inválido)
        var validation = await validator.ValidateAsync(command, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }

        // 2) Envia para o MediatR (seu CreateOrderHandler trata a persistência)
        //    ↳ Aqui assumimos que o handler retorna o Id (Guid) do pedido criado.
        var id = await sender.Send(command, ct);

        // 3) Retorna 201 Created com Location: /orders/{id}
        return Results.Created($"/orders/{id}", new { id });
    }
}
