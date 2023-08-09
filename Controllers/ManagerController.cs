using coursework.DB;
using coursework.DB.RequestRoad;
using coursework.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using coursework.Valid;
using coursework.Models.TriggerModel;

namespace coursework.Controllers
{
    public class ManagerController : Controller
    {
        public Performer _performer;
        public ManagerController(DataBaseConnect db,Performer performer) {
            _performer = performer;
        }
        [HttpGet]
        [Route("ManagerController/Profile")]
        public IActionResult Profile()
        {
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Manager manager = JsonSerializer.Deserialize<Manager>(usertJson);
            return View(manager);
        }
        [HttpPost]
        [Route("ManagerController/Profile")]
        public IActionResult Profile(string button)
        {
            switch (button)
            {
                case "create":
                    return RedirectToAction("CreateEmployee");
                case "view":
                    return RedirectToAction("Reports");
                case "add": return RedirectToAction("AddSpec");
            }
            return View();
        }
        [HttpGet]
        [Route("ManagerController/CreateEmployee")]
        public IActionResult CreateEmployee()
        {
            UserRegist userRegist = new UserRegist();
            string sql = "SELECT specialty FROM specialty";
           userRegist.Specialties = _performer.ListQueryFirstOrDefault(sql);
            return View(userRegist);
        }

        [HttpPost]
        [Route("ManagerController/CreateEmployee")]
        public IActionResult CreateEmployee(Employee employee)
        {
            RegistValid registValid = new RegistValid();
            employee.Specialty++;
            Console.WriteLine(employee.Specialty);
               UserRegist userRegist= Validation.CreateEmployee(employee, _performer);
            string sql = "SELECT specialty FROM specialty";
            userRegist.Specialties = _performer.ListQueryFirstOrDefault(sql);
            return View(userRegist);
        }
        [HttpGet]
        [Route("ManagerController/Reports")]
        public IActionResult Reports()
        {
            Console.WriteLine("Reportsget");
            return View();
        }

        [HttpPost]
        [Route("ManagerController/Reports")]
        public IActionResult Reports(string button)
        {
            Console.WriteLine("Reports");
            switch (button)
            {

                case "employee":
                    return RedirectToAction("EmployeeReports");
                case "orders":return RedirectToAction("AllOrders");
                case "rating":return RedirectToAction("PopularSpec");
                case "clientorder": return RedirectToAction("ClientOrderView");
            }
            return View();
        }
        [HttpGet]
        [Route("ManagerController/EmployeeReports")]
        public IActionResult EmployeeReports()
        {
            List<Employee> employees = Validation.AllEmp(_performer);
            ReportEmpView viewModel = new ReportEmpView
            {
                Employees = employees
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("ManagerController/EmployeeReports")]
        public IActionResult EmployeeReports(int EmployeeId, DateTime StartDateTime, DateTime EndDateTime)
        {
            CompliteOrdEmp result = Validation.ReportsEmpOrd(EmployeeId, StartDateTime, EndDateTime, _performer);

            List<Employee> employees = Validation.AllEmp(_performer);
            Console.WriteLine($"ORDER={result.TotalOrders}+{result.TotalRevenue}+{result.TenPercent}");
            ReportEmpView viewModel = new ReportEmpView
            {
                Employees = employees,
                ReportResult = result
            };

            return View(viewModel);
        }
        [HttpGet]
        [Route("ManagerController/AllOrders")]
        public IActionResult AllOrders()
        {
            
            return View();
        }
        [HttpPost]
        [Route("ManagerController/AllOrders")]
        public IActionResult AllOrders(DateTime StartDateTime, DateTime EndDateTime)
        {
            CompliteOrdEmp result = Validation.AllOrders( StartDateTime, EndDateTime, _performer);
            Console.WriteLine($"ORDER={result.TotalRevenue}+{result.TenPercent}");
            return View(result);
        }
        [HttpGet]
        [Route("ManagerController/PopularSpec")]
        public IActionResult PopularSpec()
        {
            List<RatingSpec> ratingSpecs = Validation.GetRatingSpec(_performer);
            
            return View(ratingSpecs);
        }
        [HttpGet]
        [Route("ManagerController/ClientOrderView")]
        public IActionResult ClientOrderView()
        {
            List<ClientOrder> list = Validation.GetClientOrder(_performer);
            return View(list);
        }
        [HttpGet]
        [Route("ManagerController/AddSpec")]
        public IActionResult AddSpec()
        {
            return View();
        }
        [HttpPost]
        [Route("ManagerController/AddSpec")]
        public IActionResult AddSpec(string spec)
        {
            Console.WriteLine(spec);
            Validation.AddSpec(spec, _performer);
            
            return View();
        }

    }
}
