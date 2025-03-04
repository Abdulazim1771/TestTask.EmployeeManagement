namespace TestTask.Application.Requests;
public sealed record EmployeeCreateRequest(
    string PayrollNumber,
    string FirstName,
    string Surname,
    DateOnly BirthDate,
    string Telephone,
    string Mobile,
    string Address,
    string Address_2,
    string PostCode,
    string Email,
    DateOnly StartDate);