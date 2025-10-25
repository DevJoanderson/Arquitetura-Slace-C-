using Api.AppDb;
using Api.Features.Orders.GetById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Orders.List;

public sealed class ListOrdersHandler : IRequestHandler<ListOrdersQuery, PagedResult<OrderResponse>>
{
    private readonly AppDbContext _db;

    public ListOrdersHandler(AppDbContext db) => _db = db;

    public async Task<PagedResult<OrderResponse>> Handle(ListOrdersQuery req, CancellationToken ct)
    {
        var q = _db.Orders.AsNoTracking().AsQueryable();

        // Filtros simples (case-insensitive)
        if (!string.IsNullOrWhiteSpace(req.CustomerName))
        {
            var n = req.CustomerName.ToLower();
            q = q.Where(o => o.CustomerName.ToLower().Contains(n));
        }

        if (!string.IsNullOrWhiteSpace(req.Code))
        {
            var c = req.Code.ToLower();
            q = q.Where(o => o.Code.ToLower().Contains(c));
        }

        // Ordenação
        var sortBy = (req.SortBy ?? "createdAt").ToLower();
        var sortDir = (req.SortDir ?? "desc").ToLower();

        q = (sortBy, sortDir) switch
        {
            ("code", "asc") => q.OrderBy(o => o.Code),
            ("code", "desc") => q.OrderByDescending(o => o.Code),

            ("customername", "asc") => q.OrderBy(o => o.CustomerName),
            ("customername", "desc") => q.OrderByDescending(o => o.CustomerName),

            ("totalamount", "asc") => q.OrderBy(o => o.TotalAmount),
            ("totalamount", "desc") => q.OrderByDescending(o => o.TotalAmount),

            ("createdat", "asc") => q.OrderBy(o => o.CreatedAt),
            _ => q.OrderByDescending(o => o.CreatedAt)
        };

        // Total + paginação
        var total = await q.CountAsync(ct);
        var totalPages = (int)Math.Ceiling(total / (double)req.PageSize);
        var skip = (req.Page - 1) * req.PageSize;

        var items = await q
            .Skip(skip)
            .Take(req.PageSize)
            .Select(o => new OrderResponse(o.Id, o.Code, o.CustomerName, o.TotalAmount, o.CreatedAt))
            .ToListAsync(ct);

        return new PagedResult<OrderResponse>(req.Page, req.PageSize, total, totalPages, items);
    }
}
