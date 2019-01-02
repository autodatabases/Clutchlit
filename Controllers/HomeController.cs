using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Clutchlit.Models;
using Microsoft.AspNetCore.Authorization;
using Clutchlit.Data;

namespace Clutchlit.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MysqlContext _contextSp24;
        private readonly AMysqlContext _contextSp;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, MysqlContext contextSp24, AMysqlContext contextSp)
        {
            _contextSp24 = contextSp24;
            _contextSp = contextSp;
            _context = context;
        }
        public IActionResult Index()
        {
            int numberSp24 = _contextSp24.Orders_sp24.Where(p => p.Current_state == 1 || p.Current_state == 3 || p.Current_state == 16).Count();
            int numberSpcom = _contextSp.Orders_spcom.Where(p => p.Current_state == 1 || p.Current_state == 3 || p.Current_state == 16).Count();

            List<A_orders> sp24 = _contextSp24.Orders_sp24.OrderByDescending(c => c.Id_order).Take(5).ToList();
            List<B_orders> spCom = _contextSp.Orders_spcom.OrderByDescending(c => c.Id_order).Take(5).ToList();

            ViewData["products"] = sp24;
            ViewData["productsB"] = spCom;

            int disPrices = _context.PdPrices.Count();

            int customers = _contextSp24.Customers_sp24.Count() + _contextSp.Customers_spcom.Count();

            ViewData["newOrders"] = numberSp24 + numberSpcom;
            ViewData["distributorsPrices"] = disPrices;
            ViewData["customerAmount"] = customers;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
