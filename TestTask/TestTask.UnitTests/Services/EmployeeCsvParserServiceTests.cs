using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using TestTask.Application.Services;

namespace TestTask.UnitTests.Services;
public class EmployeeCsvParserServiceTests
{
    private readonly EmployeeCsvParserService _parserService;

    public EmployeeCsvParserServiceTests()
    {
        _parserService = new EmployeeCsvParserService();
    }

    [Fact]
    public async Task ParseAsync_ValidCsv_ReturnsCorrectEmployees()
    {
        // Arrange
        var csvContent = "PayrollNumber,FirstName,Surname,BirthDate,Telephone,Mobile,Address,Address_2,PostCode,Email,StartDate\n" +
                         "12345,John,Doe,11/5/1974,123456789,987654321,Street 1, Apt 2,12345,john.doe@example.com,15/3/2020";

        var fileMock = CreateMockIFormFile(csvContent);

        // Act
        var employees = await _parserService.ParseAsync(fileMock.Object);

        // Assert
        employees.Should().HaveCount(1);
        employees[0].FirstName.Should().Be("John");
        employees[0].Surname.Should().Be("Doe");
        employees[0].PayrollNumber.Should().Be("12345");
        employees[0].BirthDate.Should().Be(new DateOnly(1974, 5, 11));
        employees[0].StartDate.Should().Be(new DateOnly(2020, 3, 15));
    }

    [Fact]
    public async Task ParseAsync_EmptyFile_ThrowsArgumentException()
    {
        // Arrange
        var fileMock = CreateMockIFormFile("");

        // Act
        Func<Task> act = async () => await _parserService.ParseAsync(fileMock.Object);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid file*");
    }

    [Fact]
    public async Task ParseAsync_InvalidDateFormat_ThrowsFormatException()
    {
        // Arrange
        var csvContent = "PayrollNumber,FirstName,Surname,BirthDate,Telephone,Mobile,Address,Address_2,PostCode,Email,StartDate\n" +
                         "12345,Jane,Doe,INVALID_DATE,123456789,987654321,Street 1, Apt 2,12345,jane.doe@example.com,15/03/2020";

        var fileMock = CreateMockIFormFile(csvContent);

        // Act
        Func<Task> act = async () => await _parserService.ParseAsync(fileMock.Object);

        // Assert
        await act.Should().ThrowAsync<FormatException>()
            .WithMessage("Invalid date format: INVALID_DATE");
    }

    [Fact]
    public async Task ParseAsync_MissingColumns_DoesNotThrowError()
    {
        // Arrange
        var csvContent = "PayrollNumber,FirstName,Surname,BirthDate,Telephone,Mobile,Address,Address_2,PostCode,Email,StartDate\n" +
                         "12345,Jane,Doe,11/5/1974,123456789,987654321,Street 1, Apt 2,12345,jane.doe@example.com,";

        var fileMock = CreateMockIFormFile(csvContent);

        // Act
        var employees = await _parserService.ParseAsync(fileMock.Object);

        // Assert
        employees.Should().HaveCount(1);
        employees[0].StartDate.Should().Be(default); // Default value for DateOnly (0001-01-01)
    }

    [Fact]
    public async Task ParseAsync_WhitespaceAndEmptyLines_IgnoresThem()
    {
        // Arrange
        var csvContent = "PayrollNumber,FirstName,Surname,BirthDate,Telephone,Mobile,Address,Address_2,PostCode,Email,StartDate\n" +
                         "\n" + // Empty line
                         "12345,John,Doe,11/5/1974,123456789,987654321,Street 1, Apt 2,12345,john.doe@example.com,15/3/2020\n" +
                         "   \n" + // Whitespace line
                         "67890,Jane,Smith,3/7/1980,987654321,123456789,Street 3, Apt 4,54321,jane.smith@example.com,1/12/2015";

        var fileMock = CreateMockIFormFile(csvContent);

        // Act
        var employees = await _parserService.ParseAsync(fileMock.Object);

        // Assert
        employees.Should().HaveCount(2);
        employees[0].FirstName.Should().Be("John");
        employees[1].FirstName.Should().Be("Jane");
    }

    private Mock<IFormFile> CreateMockIFormFile(string content)
    {
        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));

        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        return fileMock;
    }
}
