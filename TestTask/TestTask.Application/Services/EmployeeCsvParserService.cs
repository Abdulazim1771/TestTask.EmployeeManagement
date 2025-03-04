using Microsoft.AspNetCore.Http;
using System.Globalization;
using TestTask.Application.Interfaces;
using TestTask.Domain.Entities;

namespace TestTask.Application.Services;
public sealed class EmployeeCsvParserService : ICsvParser<Employee>
{
    public async Task<List<Employee>> ParseAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file", nameof(file));
        }

        var employees = new List<Employee>();

        using (var stream = new StreamReader(file.OpenReadStream()))
        {
            await stream.ReadLineAsync(); // Skip header

            while (!stream.EndOfStream)
            {
                var line = await stream.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var values = line.Split(',');

                employees.Add(ParseEmployee(values));
            }
        }

        return employees;
    }

    private Employee ParseEmployee(string[] values)
    {
        return new Employee
        {
            PayrollNumber = values[0],
            FirstName = values[1],
            Surname = values[2],
            BirthDate = ParseDate(values[3]),
            Telephone = values[4],
            Mobile = values[5],
            Address = values[6],
            Address_2 = values[7],
            PostCode = values[8],
            Email = values[9],
            StartDate = string.IsNullOrWhiteSpace(values[10]) ? default : ParseDate(values[10]), // ✅ Handle missing date
        };
    }

    private DateOnly ParseDate(string input)
    {
        string[] dateFormats = { "d/M/yyyy", "dd/MM/yyyy", "M/d/yyyy", "MM/dd/yyyy" };

        if (DateOnly.TryParseExact(input, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
        {
            return parsedDate;
        }

        throw new FormatException($"Invalid date format: {input}");
    }
}
