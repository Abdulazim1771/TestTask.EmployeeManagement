using TestTask.Application.Requests;
using TestTask.Application.ViewModels;
using TestTask.Domain.Entities;

namespace TestTask.Application.Mappings;
public static class EmployeeMappings
{
    public static EmployeeViewModel ToViewModel(this Employee employee)
    {
        return new EmployeeViewModel
        {
            Id = employee.Id,
            PayrollNumber = employee.PayrollNumber,
            FirstName = employee.FirstName,
            Surname = employee.Surname,
            BirthDate = employee.BirthDate,
            Telephone = employee.Telephone,
            Mobile = employee.Mobile,
            Address = employee.Address,
            Address_2 = employee.Address_2,
            PostCode = employee.PostCode,
            Email = employee.Email,
            StartDate = employee.StartDate
        };
    }

    public static Employee ToEntity(this EmployeeCreateRequest request)
    {
        return new Employee
        {
            PayrollNumber = request.PayrollNumber,
            FirstName = request.FirstName,
            Surname = request.Surname,
            BirthDate = request.BirthDate,
            Telephone = request.Telephone,
            Mobile = request.Mobile,
            Address = request.Address,
            Address_2 = request.Address_2,
            PostCode = request.PostCode,
            Email = request.Email,
            StartDate = request.StartDate
        };
    }

    public static Employee ToUpdate(this Employee employee, EmployeeViewModel viewModel)
    {
        employee.PayrollNumber = viewModel.PayrollNumber;
        employee.FirstName = viewModel.FirstName;
        employee.Surname = viewModel.Surname;
        employee.BirthDate = viewModel.BirthDate;
        employee.Telephone = viewModel.Telephone;
        employee.Mobile = viewModel.Mobile;
        employee.Address = viewModel.Address;
        employee.Address_2 = viewModel.Address_2;
        employee.PostCode = viewModel.PostCode;
        employee.Email = viewModel.Email;
        employee.StartDate = viewModel.StartDate;

        return employee;
    }
}
