using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using TestTask.Application.Interfaces;
using TestTask.Application.Services;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces;

namespace TestTask.UnitTests.Services;
public class EmployeeServiceTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<ICsvParser<Employee>> _mockCsvParser;
    private readonly EmployeeService _employeeService;

    public static readonly TheoryData<string?, int> SearchQueryParameters = new()
    {
        { "Jane", 1 },  // Searching by first name
        { "Doe", 1 },   // Searching by surname
        { "1003", 1 },  // Searching by payroll number
        { "NonExistent", 0 },  // No matches found
        { null, 3 },  // No search query, should return all employees
        { "", 3 },    // Empty search query, should return all employees
        { "   ", 3 }  // Whitespace search query, should return all employees
    };

    public EmployeeServiceTests()
    {
        _mockCsvParser = new Mock<ICsvParser<Employee>>();
        _mockContext = new Mock<IApplicationDbContext>();
        _employeeService = new EmployeeService(_mockContext.Object, _mockCsvParser.Object);
    }

    [Fact]
    public async Task ImportEmployeesFromCsv_ParsesCsvAndSavesToDatabase()
    {
        // Arrange
        var csvContent = "PayrollNumber,FirstName,Surname,BirthDate,Telephone,Mobile,Address,Address_2,PostCode,Email,StartDate\n" +
                         "12345,Jane,Doe,11/5/1974,123456789,987654321,Street 1, Apt 2,12345,jane.doe@example.com,2024-01-01";

        var fileMock = CreateMockIFormFile(csvContent);

        var employees = new List<Employee>
        {
            new Employee
            {
                PayrollNumber = "12345",
                FirstName = "Jane",
                Surname = "Doe",
                Mobile = "987654321",
                Address = "Street 1, Apt 2",
                Email = "jane.doe@example.com",
                BirthDate = new DateOnly(1974, 5, 11),
                StartDate = new DateOnly(2024, 1, 1)    // Provide a valid DateOnly value
            }
        };

        _mockCsvParser
            .Setup(parser => parser.ParseAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(employees);

        _mockContext
            .Setup(db => db.Employees.AddRangeAsync(It.IsAny<IEnumerable<Employee>>(), default))
            .Returns(Task.CompletedTask);

        _mockContext
            .Setup(db => db.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _employeeService.ImportEmployeesFromCsv(fileMock.Object);

        // Assert
        result.Should().Be(employees.Count);

        _mockCsvParser.Verify(parser => parser.ParseAsync(fileMock.Object), Times.Once);
        _mockContext.Verify(db => db.Employees.AddRangeAsync(employees, default), Times.Once);
        _mockContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    //[Theory]
    //[MemberData(nameof(SearchQueryParameters))]
    //public async Task GetAll_Search_ReturnsMatchingEmployees(string? searchQuery, int expectedCount)
    //{
    //    // Arrange
    //    var employees = GetTestEmployees();
    //    _mockContext.Setup(context => context.Employees)
    //            .ReturnsDbSet(employees);


    //    // Act
    //    var actual = await _employeeService.GetAll(searchQuery);

    //    // Assert
    //    actual.Should().HaveCount(expectedCount);
    //}

    private Mock<IFormFile> CreateMockIFormFile(string content)
    {
        var fileMock = new Mock<IFormFile>();
        var fileName = "employees.csv";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);

        return fileMock;
    }

    private static List<Employee> GetTestEmployees() =>
        [
            new Employee
            {
                Id = 1,
                PayrollNumber = "1001",
                FirstName = "John",
                Surname = "Doe",
                Mobile = "123456789",
                Address = "Street 1",
                Email = "john@example.com",
                BirthDate = new DateOnly(1974, 5, 11),
                StartDate = new DateOnly(2024, 1, 1)
            },
            new Employee
            {
                Id = 2,
                PayrollNumber = "1002",
                FirstName = "Jane",
                Surname = "Smith",
                Mobile = "987654321",
                Address = "Street 2",
                Email = "jane@example.com",
                BirthDate = new DateOnly(1974, 5, 11),
                StartDate = new DateOnly(2024, 1, 1)
            },
            new Employee
            {
                Id = 3,
                PayrollNumber = "1003",
                FirstName = "Mike",
                Surname = "Johnson",
                Mobile = "111222333",
                Address = "Street 3",
                Email = "mike@example.com",
                BirthDate = new DateOnly(1974, 5, 11),
                StartDate = new DateOnly(2024, 1, 1)
            }
        ];

    private static List<Employee> GetExpectedEmployee(string? search)
    {
        var query = GetTestEmployees().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string searchLower = search.ToLower();
            query = query.Where(e => e.FirstName.ToLower().Contains(searchLower) ||
                                     e.Surname.ToLower().Contains(searchLower) ||
                                     e.PayrollNumber.ToLower().Contains(searchLower));
        }

        return query.ToList();
    }
}
