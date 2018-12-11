using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Clutchlit.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            return View();
        }
        public IActionResult GetUnrealized()
        {

            return View();
        }
        public IActionResult GetSpcom()
        {

            return View();
        }
        public IActionResult GetSp24()
        {

            return View();
        }
    }
}