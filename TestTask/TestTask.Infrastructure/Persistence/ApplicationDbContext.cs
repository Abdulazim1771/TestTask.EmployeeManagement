using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces;

namespace TestTask.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public virtual DbSet<Employee> Employees { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
