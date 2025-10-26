using MediatR;

namespace Api.Features.Orders.Delete;

public sealed record DeleteOrderCommand(Guid Id) : IRequest<bool>;
