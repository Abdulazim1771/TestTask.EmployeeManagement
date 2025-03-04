using Microsoft.AspNetCore.Http;
using TestTask.Application.Requests;
using TestTask.Application.ViewModels;

namespace TestTask.Application.Interfaces;
public interface IEmployeeService
{
    Task<List<EmployeeViewModel>> GetAll(string? search = null);
    Task<EmployeeViewModel> GetByIdAsync(int id);
    Task CreateAsync(EmployeeCreateRequest request);
    Task UpdateAsync(EmployeeViewModel model);
    Task DeleteAsync(int id);
    Task<int> ImportEmployeesFromCsv(IFormFile file);
}
