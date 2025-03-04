using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TestTask.Application.Interfaces;
using TestTask.Application.Mappings;
using TestTask.Application.Requests;
using TestTask.Application.ViewModels;
using TestTask.Domain.Entities;
using TestTask.Domain.Exceptions;
using TestTask.Domain.Interfaces;

namespace TestTask.Application.Services;
public sealed class EmployeeService : IEmployeeService
{
    private readonly IApplicationDbContext _context;
    private readonly ICsvParser<Employee> _csvParser;

    public EmployeeService(IApplicationDbContext context, ICsvParser<Employee> csvParser)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _csvParser = csvParser ?? throw new ArgumentNullException(nameof(csvParser));
    }

    public async Task<List<EmployeeViewModel>> GetAll(string? search = null)
    {
        var query = FilterEmployee(search);

        var viewModels = await query.Select(e => e.ToViewModel()).ToListAsync();

        return viewModels;
    }

    public async Task<EmployeeViewModel> GetByIdAsync(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null)
        {
            throw new EntityNotFoundException($"Employee with id: {id} is not found.");
        }

        return employee.ToViewModel(); ;
    }

    public async Task CreateAsync(EmployeeCreateRequest request)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(request));

        var employee = request.ToEntity();

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmployeeViewModel model)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == model.Id);

        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        var updatedModel = employee.ToUpdate(model);

        _context.Employees.Update(updatedModel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ImportEmployeesFromCsv(IFormFile file)
    {
        var employees = await _csvParser.ParseAsync(file);

        await _context.Employees.AddRangeAsync(employees);
        await _context.SaveChangesAsync();

        return employees.Count;
    }

    private IQueryable<Employee> FilterEmployee(string? search)
    {
        var query = _context.Employees
            .OrderBy(e => e.Surname)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e => e.FirstName.Contains(search) ||
                                     e.Surname.Contains(search) ||
                                     e.PayrollNumber.Contains(search));
        }

        return query;
    }
}
