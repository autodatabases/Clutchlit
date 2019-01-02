using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clutchlit.Data;
using Clutchlit.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clutchlit.Controllers
{
    public class DistributorsController : Controller
    {
        private readonly MysqlContext _contextSp24;
        private readonly AMysqlContext _contextSp;
        private readonly ApplicationDbContext _context;

        public DistributorsController(ApplicationDbContext context, MysqlContext contextSp24, AMysqlContext contextSp)
        {
            _contextSp24 = contextSp24;
            _contextSp = contextSp;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DistributorsList()
        {
            List<Distributor> dis = _context.Distributors.ToList();
            return View(dis);
        }
        public IActionResult DistributorDetails()
        {
            return View();
        }
        public IActionResult DistributorEdit()
        {
            return View();
        }
    }
}