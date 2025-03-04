using Microsoft.AspNetCore.Http;

namespace TestTask.Application.Interfaces;
public interface ICsvParser<T>
{
    Task<List<T>> ParseAsync(IFormFile file);
}
