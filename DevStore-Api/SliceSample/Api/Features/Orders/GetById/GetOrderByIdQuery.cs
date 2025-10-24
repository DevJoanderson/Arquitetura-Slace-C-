using MediatR;

namespace Api.Features.Orders.GetById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse?>;
