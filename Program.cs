using coursework.Controllers;
using coursework.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using coursework.DB.RequestRoad;
namespace coursework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<DataBaseConnect>();
            builder.Services.AddScoped<Performer>();
            builder.Services.AddControllersWithViews();

            // Добавьте поддержку сессий
            builder.Services.AddSession(options =>
            {
                // Настройки сеансов, если необходимо
            });

            var app = builder.Build();
            app.UseSession(); // Разместите это перед app.UseAuthorization()

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
