using coursework.DB;
using coursework.DB.RequestRoad;
using coursework.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using coursework.Valid;
using coursework.Models.TriggerModel;

namespace coursework.Controllers {
    public class ManagerController : Controller {
        // Виконавець для доступу до бази даних
        public Performer _performer;

        // Конструктор, який приймає виконавця
        public ManagerController(DataBaseConnect db, Performer performer) {
            _performer = performer;
        }

        // Дія для відображення профілю менеджера (GET-запит)
        [HttpGet]
        [Route("ManagerController/Profile")]
        public IActionResult Profile() {
            // Отримання даних менеджера з сесії та десеріалізація їх
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Manager manager = JsonSerializer.Deserialize<Manager>(usertJson);
            // Передача об'єкту менеджера до представлення
            return View(manager);
        }

        // Дія для відображення профілю менеджера (POST-запит)
        [HttpPost]
        [Route("ManagerController/Profile")]
        public IActionResult Profile(string button) {
            switch (button)
            {
                case "create":
                    // Перенаправлення на сторінку створення співробітника
                    return RedirectToAction("CreateEmployee");
                case "view":
                    // Перенаправлення на сторінку звітів
                    return RedirectToAction("Reports");
                case "add":
                    // Перенаправлення на сторінку додавання спеціалізації
                    return RedirectToAction("AddSpec");
            }
            return View();
        }

        // Дія для створення співробітника (GET-запит)
        [HttpGet]
        [Route("ManagerController/CreateEmployee")]
        public IActionResult CreateEmployee() {
            // Створення об'єкту для передачі даних на сторінку
            UserRegist userRegist = new UserRegist();
            string sql = "SELECT specialty FROM specialty";
            userRegist.Specialties = _performer.ListQueryFirstOrDefault(sql);
            return View(userRegist);
        }

        // Дія для створення співробітника (POST-запит)
        [HttpPost]
        [Route("ManagerController/CreateEmployee")]
        public IActionResult CreateEmployee(Employee employee) {
            employee.Specialty++;
            Console.WriteLine(employee.Specialty);
            UserRegist userRegist = Validation.CreateEmployee(employee, _performer);
            string sql = "SELECT specialty FROM specialty";
            userRegist.Specialties = _performer.ListQueryFirstOrDefault(sql);
            return View(userRegist);
        }

        // Дія для відображення сторінки звітів (GET-запит)
        [HttpGet]
        [Route("ManagerController/Reports")]
        public IActionResult Reports() {
            Console.WriteLine("Reportsget");
            return View();
        }

        // Дія для відображення сторінки звітів (POST-запит)
        [HttpPost]
        [Route("ManagerController/Reports")]
        public IActionResult Reports(string button) {
            Console.WriteLine("Reports");
            switch (button)
            {
                case "employee":
                    // Перенаправлення на сторінку звітів про співробітників
                    return RedirectToAction("EmployeeReports");
                case "orders":
                    // Перенаправлення на сторінку звітів про замовлення
                    return RedirectToAction("AllOrders");
                case "rating":
                    // Перенаправлення на сторінку популярності спеціалізацій
                    return RedirectToAction("PopularSpec");
                case "clientorder":
                    // Перенаправлення на сторінку перегляду замовлень клієнтів
                    return RedirectToAction("ClientOrderView");
            }
            return View();
        }

        // Дія для відображення звіту про співробітників (GET-запит)
        [HttpGet]
        [Route("ManagerController/EmployeeReports")]
        public IActionResult EmployeeReports() {
            // Отримання списку всіх співробітників
            List<Employee> employees = Validation.AllEmp(_performer);

            // Створення об'єкту для передачі даних на сторінку
            ReportEmpView viewModel = new ReportEmpView
            {
                Employees = employees
            };

            // Передача об'єкту до представлення
            return View(viewModel);
        }

        // Дія для відображення звіту про співробітників (POST-запит)
        [HttpPost]
        [Route("ManagerController/EmployeeReports")]
        public IActionResult EmployeeReports(int EmployeeId, DateTime StartDateTime, DateTime EndDateTime) {
            // Отримання результатів звіту
            CompliteOrdEmp result = Validation.ReportsEmpOrd(EmployeeId, StartDateTime, EndDateTime, _performer);

            // Отримання списку всіх співробітників
            List<Employee> employees = Validation.AllEmp(_performer);
            Console.WriteLine($"ORDER={result.TotalOrders}+{result.TotalRevenue}+{result.TenPercent}");

            // Створення об'єкту для передачі даних на сторінку
            ReportEmpView viewModel = new ReportEmpView
            {
                Employees = employees,
                ReportResult = result
            };

            // Передача об'єкту до представлення
            return View(viewModel);
        }

        // Дія для відображення звіту про всі замовлення (GET-запит)
        [HttpGet]
        [Route("ManagerController/AllOrders")]
        public IActionResult AllOrders() {
            return View();
        }

        // Дія для відображення звіту про всі замовлення (POST-запит)
        [HttpPost]
        [Route("ManagerController/AllOrders")]
        public IActionResult AllOrders(DateTime StartDateTime, DateTime EndDateTime) {
            // Отримання результатів звіту
            CompliteOrdEmp result = Validation.AllOrders(StartDateTime, EndDateTime, _performer);
            Console.WriteLine($"ORDER={result.TotalRevenue}+{result.TenPercent}");

            // Передача результатів до представлення
            return View(result);
        }

        // Дія для відображення списку популярних спеціалізацій (GET-запит)
        [HttpGet]
        [Route("ManagerController/PopularSpec")]
        public IActionResult PopularSpec() {
            // Отримання списку популярних спеціалізацій
            List<RatingSpec> ratingSpecs = Validation.GetRatingSpec(_performer);

            // Передача списку до представлення
            return View(ratingSpecs);
        }

        // Дія для відображення списку замовлень клієнтів (GET-запит)
        [HttpGet]
        [Route("ManagerController/ClientOrderView")]
        public IActionResult ClientOrderView() {
            // Отримання списку замовлень клієнтів
            List<ClientOrder> list = Validation.GetClientOrder(_performer);

            // Передача списку до представлення
            return View(list);
        }

        // Дія для відображення сторінки додавання спеціалізації (GET-запит)
        [HttpGet]
        [Route("ManagerController/AddSpec")]
        public IActionResult AddSpec() {
            return View();
        }

        // Дія для додавання спеціалізації (POST-запит)
        [HttpPost]
        [Route("ManagerController/AddSpec")]
        public IActionResult AddSpec(string spec) {
            Console.WriteLine(spec);
            // Додавання спеціалізації до бази даних
            Validation.AddSpec(spec, _performer);

            return View();
        }

    }
}
