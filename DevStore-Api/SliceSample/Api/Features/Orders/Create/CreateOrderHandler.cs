using Api.AppDb;
using Api.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Orders.Create;

// Handler: recebe o Command, aplica regras e persiste no banco.
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly AppDbContext _db;

    public CreateOrderHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Exemplo de regra simples: Code deve ser único
        var exists = await _db.Orders.AnyAsync(o => o.Code == request.Code, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Já existe um pedido com Code '{request.Code}'.");

        var order = new Order
        {
            Code = request.Code,
            CustomerName = request.CustomerName,
            TotalAmount = request.TotalAmount
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
