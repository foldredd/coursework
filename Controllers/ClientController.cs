using Microsoft.AspNetCore.Mvc;
using coursework.Models.DataModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using coursework.DB.RequestRoad;
using System.Collections.Generic;
using coursework.DB;

namespace coursework.Controllers
{
    public class ClientController : Controller
    {
        private  Client client;
        private static List<Order> orders;
        private readonly DataBaseConnect _db;
        private readonly Performer performer;
        private  static List<string> specialties;
        public ClientController(DataBaseConnect db,Performer _performer)
        {
            _db = db;
            performer = _performer;
           
        }

        [HttpGet]
        [Route("ClientController/Profile")]
        public IActionResult Profile()
        {
            Console.WriteLine("getProf");
            if (client == null) {
                var clientJson = HttpContext.Session.GetString("HomeClient");
                client = JsonSerializer.Deserialize<Client>(clientJson);
                Console.WriteLine(client.Id);
            }

            return View(client);
        }



        [HttpPost]
        [Route("ClientController/Profile")]
        public IActionResult Profile(string button)
        {
           
            var clientJson = HttpContext.Session.GetString("HomeClient");
            client = JsonSerializer.Deserialize<Client>(clientJson);
            Console.WriteLine(client.Id);
            
            
            switch (button)
            {
                case "Add":
                    return RedirectToAction("AddOrder");
                case "View":
                    return RedirectToAction("ViewOrders");
                default:
                    Console.WriteLine("def");
                    return View(client);
            }
        }


        [HttpGet]
        [Route("ClientController/AddOrder")]
        public IActionResult AddOrder(){
            string sql = "SELECT specialty FROM specialty";
            specialties = performer.ListQueryFirstOrDefault(sql);
            return View(specialties);
        }
        [HttpPost]
        [Route("ClientController/AddOrder")]
        public IActionResult AddOrder(string specialty, string button, string description)
        {
            var clientJson = HttpContext.Session.GetString("HomeClient");
            client = JsonSerializer.Deserialize<Client>(clientJson);
            if (!string.IsNullOrEmpty(specialty))
            {
                int i = 0;
                foreach (var spec in specialties)
                {
                    i++;
                    if (spec == specialty)
                    {
                        break;
                    }
                }

                string sql = "INSERT INTO `Order` (id_client, specialty, description,`date`) VALUES (@IdClient, @Specialty, @Description,@Date)";
                performer.QueryFirstOrDefault(sql, new { IdClient = client.Id, Specialty = i, Description = description,Date=DateTime.Now});
                return RedirectToAction("Profile");
            }
            return View();
        }

        [HttpGet]
        [Route("ClientController/ViewOrders")]
        public IActionResult ViewOrders()
        {
             orders = new List<Order>();
            var clientJson = HttpContext.Session.GetString("HomeClient");
            client = JsonSerializer.Deserialize<Client>(clientJson);
            Console.WriteLine(client.Id);
            string sql = "SELECT * FROM `order` WHERE id_client=@client";
            orders.AddRange(performer.ListOrder(sql, new {client=client.Id}));
            sql = "SELECT * FROM orders_in_progress WHERE id_client=@client";
            orders.AddRange(performer.ListOrder(sql, new {client = client.Id }));
            return View(orders);
        }
    }
}
