using FluentValidation;
using MediatR;

namespace Api.Features.Orders.Delete;

public static class DeleteOrderEndpoint
{
    public static IEndpointRouteBuilder MapDeleteOrderEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender,
            IValidator<DeleteOrderCommand> validator,
            CancellationToken ct) =>
        {
            var cmd = new DeleteOrderCommand(id);

            var validation = await validator.ValidateAsync(cmd, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }

            var ok = await sender.Send(cmd, ct);
            return ok ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteOrder")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        return app;
    }
}
