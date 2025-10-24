using FluentValidation;
using MediatR;

namespace Api.Features.Orders.GetById;

public static class GetOrderByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetOrderByIdEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, IValidator<GetOrderByIdQuery> validator, CancellationToken ct) =>
        {
            var query = new GetOrderByIdQuery(id);

            var validation = await validator.ValidateAsync(query, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            var order = await sender.Send(query, ct);
            return order is null ? Results.NotFound() : Results.Ok(order);
        })
        .WithName("GetOrderById")
        .Produces<OrderResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        return app;
    }
}
