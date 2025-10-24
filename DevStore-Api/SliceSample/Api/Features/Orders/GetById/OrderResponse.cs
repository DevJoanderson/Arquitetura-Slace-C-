namespace Api.Features.Orders.GetById;

public sealed record OrderResponse(
    Guid Id,
    string Code,
    string CustomerName,
    decimal TotalAmount,
    DateTime CreatedAt
);
