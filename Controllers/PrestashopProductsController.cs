using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clutchlit.Data;
using Microsoft.AspNetCore.Mvc;

namespace Clutchlit.Controllers
{
    public class PrestashopProductsController : Controller
    {
        private readonly MysqlContext _contextSp24;
        private readonly AMysqlContext _contextSpC;

        public PrestashopProductsController(MysqlContext contextSp24, AMysqlContext contextSpC)
        {
            _contextSp24 = contextSp24;
            _contextSpC = contextSpC;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Edit(string id)
        {
            var product_title = _contextSp24.Products_sp24.Where(p => p.Id_product == int.Parse(id)).FirstOrDefault().Name; //ps_product_lang
            var product_net_price = _contextSp24.Products_prices_sp24.Where(p => p.Id_product == int.Parse(id)).FirstOrDefault().Price; //ps_product_shop
            var product_gross_price = Math.Round((double)product_net_price * 1.23, 0);
            var product_data = _contextSp24.ProductDisplay.Where(o => o.ProductId == int.Parse(id)).SingleOrDefault(); // ps_product

            var product_quantity = product_data.Quantity;

            ViewData["product_title"] = product_title;
            ViewData["product_gross_price"] = product_gross_price;
            ViewData["product_quantity"] = product_quantity;
            
            return View();
        }
    }
}