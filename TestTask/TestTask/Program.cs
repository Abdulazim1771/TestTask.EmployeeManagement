using TestTask.Application.Extensions;
using TestTask.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterApplication(builder.Configuration)
    .ConfigureServices(builder.Configuration);

builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
