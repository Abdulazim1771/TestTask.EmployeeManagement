namespace TestTask.Domain.Entities;
public class Employee
{
    public int Id { get; set; }
    public required string PayrollNumber { get; set; }
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? Telephone { get; set; }
    public required string Mobile { get; set; }
    public required string Address { get; set; }
    public string? Address_2 { get; set; }
    public string? PostCode { get; set; }
    public required string Email { get; set; }
    public DateOnly StartDate { get; set; }
}
