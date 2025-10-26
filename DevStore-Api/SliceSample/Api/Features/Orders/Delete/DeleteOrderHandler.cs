using Api.AppDb;
using MediatR;

namespace Api.Features.Orders.Delete;

public sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly AppDbContext _db;
    public DeleteOrderHandler(AppDbContext db) => _db = db;

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken ct)
    {
        var order = await _db.Orders.FindAsync([request.Id], ct);
        if (order is null) return false;

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
