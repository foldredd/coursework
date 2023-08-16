using coursework.Controllers;
using coursework.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using coursework.DB.RequestRoad;

namespace coursework {
    public class Program {
        public static void Main(string[] args) {
            // Створення об'єкта для налаштування додатка
            var builder = WebApplication.CreateBuilder(args);

            // Додавання служби для підключення до бази даних (один екземпляр на всю програму)
            builder.Services.AddSingleton<DataBaseConnect>();

            // Додавання засідної служби виконавця для виконання дій над даними
            builder.Services.AddScoped<Performer>();

            // Додавання контролерів та представлень
            builder.Services.AddControllersWithViews();

            // Додавання налаштувань для роботи з сесіями
            builder.Services.AddSession(options =>
            {
                // Можливі налаштування сесій
            });

            // Створення об'єкта додатка на основі налаштованого білдера
            var app = builder.Build();

            // Використання сесій для збереження стану користувача
            app.UseSession();

            // Перевірка, чи додаток не знаходиться в режимі розробки
            if (!app.Environment.IsDevelopment())
            {
                // Встановлення обробника помилок для відображення сторінки помилок
                app.UseExceptionHandler("/Home/Error");

                // Використання HSTS (HTTP Strict Transport Security) для забезпечення безпеки з'єднання
                app.UseHsts();
            }

            // Автоматичний редирект з HTTP на HTTPS
            app.UseHttpsRedirection();

            // Використання статичних файлів (CSS, JavaScript тощо)
            app.UseStaticFiles();

            // Використання маршрутизації
            app.UseRouting();

            // Авторизація користувачів
            app.UseAuthorization();

            // Встановлення маршрутів для контролерів
            app.MapControllers();

            // Встановлення маршруту за замовчуванням
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Запуск додатка і очікування запитів
            app.Run();
        }
    }
}
