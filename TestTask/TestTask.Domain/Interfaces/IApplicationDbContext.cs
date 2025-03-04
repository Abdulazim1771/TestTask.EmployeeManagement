using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Entities;

namespace TestTask.Domain.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
