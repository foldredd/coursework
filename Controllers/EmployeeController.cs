using Microsoft.AspNetCore.Mvc;
using coursework.DB.RequestRoad;
using coursework.DB;
using System.Text.Json;
using coursework.DB.RequestRoad;
using coursework.Models.DataModels;
using coursework.Valid;
namespace coursework.Controllers
{
    public class EmployeeController : Controller
    {
        DataBaseConnect _db;
        Performer _performer;

        public EmployeeController(DataBaseConnect db ,Performer performer) {
         _db= db;
         _performer= performer;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("EmployeeController/Profile")]
        public IActionResult Profile()
        {
            Console.WriteLine("getProf");
            
                var usertJson = HttpContext.Session.GetString("HomeClient");
                Employee employee = JsonSerializer.Deserialize<Employee>(usertJson);
                Console.WriteLine(employee.Id);
            Console.WriteLine($"Emp = {employee.Specialty}");
            return View("Profile", employee);
        }

        [HttpPost]
        [Route("EmployeeController/Profile")]
        public IActionResult Profile(string button)
        {
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Employee employee = JsonSerializer.Deserialize<Employee>(usertJson);
            
            switch (button)
            {
                case "take":  
                    return RedirectToAction("TakeJob");
                case "view":
                    return RedirectToAction("ViewJob");
                default: return View(employee);
            }
            
        }
        [HttpGet]
        [Route("EmployeeController/TakeJob")]
        public  IActionResult TakeJob(){
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Employee employee = JsonSerializer.Deserialize<Employee>(usertJson);
            Console.WriteLine($"Emp = {employee.Specialty}");
            List<Order> listorders = Validation.ValidateOrder(employee, _performer);

            return View(listorders);
        }

        [HttpPost]
        [Route("EmployeeController/TakeJob")]
        public IActionResult TakeJob(string button, int orderId)
        {
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Employee employee = JsonSerializer.Deserialize<Employee>(usertJson);
           
            switch (button)
            {
                case "take":
                    if (_performer == null)
                    {
                        Console.WriteLine("PERFORMER NULL");
                    }

                    Validation.TakeOrder(employee, _performer, orderId);
                    break;
                   
            }
            List<Order> listorders = Validation.ValidateOrder(employee, _performer);
            return View(listorders);
        }
        [HttpGet]
        [Route("EmployeeController/ViewJob")]
        public IActionResult ViewJob()
        {
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Employee employee = JsonSerializer.Deserialize<Employee>(usertJson);
            Console.WriteLine($"EMpID={employee.Id}");
            List<Order> listorders = Validation.GetYourOrder(employee, _performer);
            return View(listorders);
        }
        [HttpPost]
        [Route("EmployeeController/ViewJob")]
        public IActionResult ViewJob(double cost,string button,int orderId)
        {
            Order order = Validation.getOrderInProgress(orderId, _performer);
            Console.WriteLine(cost);

            if (Math.Abs(cost) > 0.001)
            {
                order.Cost = cost;
            }
            Console.WriteLine($"Cost={order.Cost}");
            switch (button)
            {
                case "true":
                    Console.WriteLine("TRUE");
                    Validation.CompleteOrder(order, _performer);
                    break;
            }
            var usertJson = HttpContext.Session.GetString("HomeClient");
            Employee employee = JsonSerializer.Deserialize<Employee>(usertJson);
            List<Order> listorders = Validation.GetYourOrder(employee, _performer);
            return View(listorders);

        }
    }
}
