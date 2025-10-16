using MediatR;

namespace Api.Features.Orders.Create;

// Command = intenção de criar um pedido
// record = tipo imutável, perfeito pra comandos/queries
public record CreateOrderCommand(string Code, string CustomerName, decimal TotalAmount) : IRequest<Guid>;
