using MediatR;

namespace Api.Features.Orders.GetById;

// Query = intenção de buscar um pedido por Id
public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;

// DTO de saída (não devolver entidade crua do domínio)
public record OrderDto(Guid Id, string Code, string CustomerName, decimal TotalAmount, DateTime CreatedAt);
