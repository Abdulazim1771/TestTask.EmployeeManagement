using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.Requests;
using TestTask.Application.ViewModels;

namespace TestTask.Controllers;
public class HomeController : Controller
{
    private readonly IEmployeeService _employeeService;

    public HomeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAll();

        return View(employees);
    }

    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Json(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, errors });
        }

        await _employeeService.CreateAsync(request);

        return Json(new { success = true, message = "Employee created successfully!" });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EmployeeViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, errors });
        }

        await _employeeService.UpdateAsync(viewModel);

        return Json(new { success = true, message = "Employee updated successfully!" });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _employeeService.DeleteAsync(id);

        return Ok();
    }

    [HttpPost]
    [Route("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Please select a CSV file.";
            return RedirectToAction("Index");
        }

        var count = await _employeeService.ImportEmployeesFromCsv(file);

        return Ok($"{count} employees imported successfully.");
    }

    /// <summary>
    /// Filters employees
    /// </summary>
    /// <param name="search"></param>
    /// <returns>List of filtered employees</returns>
    [HttpGet]
    [Route("getEmployees")]
    public async Task<IActionResult> GetEmployees(string search)
    {
        var result = await _employeeService.GetAll(search);

        return Ok(result);
    }
}
