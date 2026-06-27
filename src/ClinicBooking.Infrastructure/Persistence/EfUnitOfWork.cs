using ClinicBooking.Application.Abstractions;

namespace ClinicBooking.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly ClinicDbContext _context;
    public EfUnitOfWork(ClinicDbContext context)
    {
        _context = context;
    }
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}