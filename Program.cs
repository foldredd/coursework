using coursework.Controllers;
using coursework.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using coursework.DB.RequestRoad;

namespace coursework {
    public class Program {
        public static void Main(string[] args) {
            // ��������� ��'���� ��� ������������ �������
            var builder = WebApplication.CreateBuilder(args);

            // ��������� ������ ��� ���������� �� ���� ����� (���� ��������� �� ��� ��������)
            builder.Services.AddSingleton<DataBaseConnect>();

            // ��������� ������ ������ ��������� ��� ��������� �� ��� ������
            builder.Services.AddScoped<Performer>();

            // ��������� ���������� �� ������������
            builder.Services.AddControllersWithViews();

            // ��������� ����������� ��� ������ � ������
            builder.Services.AddSession(options =>
            {
                // ������ ������������ ����
            });

            // ��������� ��'���� ������� �� ����� ������������� ������
            var app = builder.Build();

            // ������������ ���� ��� ���������� ����� �����������
            app.UseSession();

            // ��������, �� ������� �� ����������� � ����� ��������
            if (!app.Environment.IsDevelopment())
            {
                // ������������ ��������� ������� ��� ����������� ������� �������
                app.UseExceptionHandler("/Home/Error");

                // ������������ HSTS (HTTP Strict Transport Security) ��� ������������ ������� �'�������
                app.UseHsts();
            }

            // ������������ �������� � HTTP �� HTTPS
            app.UseHttpsRedirection();

            // ������������ ��������� ����� (CSS, JavaScript ����)
            app.UseStaticFiles();

            // ������������ �������������
            app.UseRouting();

            // ����������� ������������
            app.UseAuthorization();

            // ������������ �������� ��� ����������
            app.MapControllers();

            // ������������ �������� �� �������������
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ������ ������� � ���������� ������
            app.Run();
        }
    }
}
