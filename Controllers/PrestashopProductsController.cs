﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Clutchlit.Controllers
{
    public class PrestashopProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}