using Api.AppDb;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Orders.GetById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly AppDbContext _db;

    public GetOrderByIdHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _db.Orders
            .AsNoTracking()
            .Where(o => o.Id == request.Id)
            .Select(o => new OrderDto(o.Id, o.Code, o.CustomerName, o.TotalAmount, o.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
            throw new KeyNotFoundException($"Pedido {request.Id} não encontrado.");

        return order;
    }
}
