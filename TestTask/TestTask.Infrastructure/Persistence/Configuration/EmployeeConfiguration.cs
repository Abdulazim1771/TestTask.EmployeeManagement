using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Domain.Entities;

namespace TestTask.Infrastructure.Persistence.Configuration;
internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(nameof(Employee));
        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.PayrollNumber)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.FirstName)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.Surname)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.BirthDate)
            .IsRequired();

        builder
            .Property(e => e.Telephone)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired(false);

        builder
            .Property(e => e.Mobile)
            .IsRequired();

        builder
            .Property(e => e.Address)
            .HasMaxLength(Constants.MAX_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.Address_2)
            .HasMaxLength(Constants.MAX_STRING_LENGTH)
            .IsRequired(false);

        builder
            .Property(e => e.PostCode)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired(false);

        builder
            .Property(e => e.Email)
            .HasMaxLength(Constants.DEFAULT_STRING_LENGTH)
            .IsRequired();

        builder
            .Property(e => e.StartDate)
            .IsRequired();
    }
}

