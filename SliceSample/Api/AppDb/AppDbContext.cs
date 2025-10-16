using Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.AppDb;

// DbContext do EF Core.
// Usaremos InMemory apenas para dev/teste e ter o Swagger rodando rápido.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Exemplo: setar CreatedAt automaticamente para novas entidades.
        var entries = ChangeTracker.Entries<Order>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in entries)
            entry.Entity.CreatedAt = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }
}
