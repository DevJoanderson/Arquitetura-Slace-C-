using Api.Features.Orders.GetById;
using FluentValidation;
using MediatR;

namespace Api.Features.Orders.List;

public static class ListOrdersEndpoint
{
    public static IEndpointRouteBuilder MapListOrdersEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapGet("/", async (
            int page,
            int pageSize,
            string? customerName,
            string? code,
            string? sortBy,
            string? sortDir,
            ISender sender,
            IValidator<ListOrdersQuery> validator,
            CancellationToken ct) =>
        {
            var query = new ListOrdersQuery(page, pageSize, customerName, code, sortBy, sortDir);

            var validation = await validator.ValidateAsync(query, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            var result = await sender.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("ListOrders")
        .Produces<PagedResult<OrderResponse>>(StatusCodes.Status200OK)
        .ProducesValidationProblem();

        return app;
    }
}
