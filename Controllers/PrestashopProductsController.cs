using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clutchlit.Data;
using Clutchlit.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clutchlit.Controllers
{
    public class PrestashopProductsController : Controller
    {
        private readonly MysqlContext _contextSp24;
        private readonly AMysqlContext _contextSpC;
        private readonly AllegroAuctionsController _allegroAuctionsController;

        public PrestashopProductsController(MysqlContext contextSp24, AMysqlContext contextSpC, AllegroAuctionsController allegroAuctionsController)
        {
            _contextSp24 = contextSp24;
            _contextSpC = contextSpC;
            _allegroAuctionsController = allegroAuctionsController;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult GetAllProducts()
        {
            var products = _contextSp24.ProductDisplay; // products
            var manufacturers = _contextSp24.ShopManufacturer; // producenci
            var productsName = _contextSp24.Products_sp24;

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;
            IQueryable<PrestashopProduct> result = null;

            result = (from p in products
                      join m in manufacturers on p.ManufacturerId equals m.ManuId
                      join pn in productsName on p.ProductId equals pn.Id_product
                      select new PrestashopProduct {
                          Id = p.ProductId,
                          Name = pn.Name,
                          Reference = p.Reference,
                          GrossPrice = Math.Round((decimal)((double)p.NetPrice*1.23)),
                          Status = p.Active
                      });

            var customerData = result.OrderBy(o=>o.Id);

            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.Name.ToUpper().Contains(searchValue.ToUpper())).OrderBy(o=>o.Id);
            }
            //Paging   
            recordsTotal = customerData.Count();
            //Paging   
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            Response.StatusCode = 200;
            return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Edit(string id)
        {
            var product_title = _contextSp24.Products_sp24.Where(p => p.Id_product == int.Parse(id)).FirstOrDefault().Name; //ps_product_lang
            var product_net_price = _contextSp24.Products_prices_sp24.Where(p => p.Id_product == int.Parse(id)).FirstOrDefault().Price; //ps_product_shop
            var product_gross_price = Math.Round((double)product_net_price * 1.23, 0);
            var product_data = _contextSp24.ProductDisplay.Where(o => o.ProductId == int.Parse(id)).SingleOrDefault(); // ps_product

            var product_quantity = product_data.Quantity;

            ViewData["product_id"] = product_data.ProductId;
            ViewData["product_title"] = product_title;
            ViewData["product_gross_price"] = product_gross_price;
            ViewData["product_quantity"] = product_quantity;
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(string inputId, string inputTitle, string inputGrossPrice, string inputQuantity, string inputStatus)
        {
            int product_id = int.Parse(inputId);
            double netPrice = Math.Round(Double.Parse(inputGrossPrice) / 1.23, 6);

            var product_Sp24 = _contextSp24.ProductDisplay.Where(p => p.ProductId == product_id).SingleOrDefault();
            var product_shop_Sp24 = _contextSp24.Products_prices_sp24.Where(p => p.Id_product == product_id).SingleOrDefault();
            var product_stockSp24 = _contextSp24.ProductsStock.Where(p => p.ProductId == product_id).SingleOrDefault();

            product_Sp24.Quantity = int.Parse(inputQuantity);
            product_Sp24.NetPrice = (decimal)netPrice;
            product_shop_Sp24.Price = (decimal)netPrice;
            product_Sp24.Active = int.Parse(inputStatus);
            product_shop_Sp24.Active = int.Parse(inputStatus);
            product_stockSp24.Quantity = int.Parse(inputQuantity);

            var product_SpC = _contextSpC.ProductDisplay.Where(p => p.ProductId == product_id).SingleOrDefault();
            var product_shop_SpC = _contextSpC.Products_prices_spcom.Where(p => p.Id_product == product_id).SingleOrDefault();
            var product_stockSpC = _contextSpC.ProductsStock.Where(p => p.ProductId == product_id).SingleOrDefault();

            product_SpC.Quantity = int.Parse(inputQuantity);
            product_SpC.NetPrice = (decimal)netPrice;
            product_shop_SpC.Price = (decimal)netPrice;
            product_SpC.Active = int.Parse(inputStatus);
            product_shop_SpC.Active = int.Parse(inputStatus);
            product_stockSpC.Quantity = int.Parse(inputQuantity);

            _contextSpC.SaveChanges();
            _contextSp24.SaveChanges();

            

            if (inputStatus == "1")
                ViewData["status"] = "Włączony";
            else
                ViewData["status"] = "Wyłączony";

            ViewData["quantity"] = inputQuantity;
            ViewData["product_id"] = product_id;
            ViewData["price"] = Math.Round(netPrice*1.23).ToString() + "(" + netPrice + ")";

            // coś takiego będzie trzeba tutaj dodać celem aktualizacji cen w aukcjach allegro
            // RedirectToAction("UpdateAuctionPrice","AllegroAuctionsController");
            if (inputQuantity == "0" || inputStatus == "0")
                await _allegroAuctionsController.TurnOffAuction(product_id.ToString());
            else if(product_SpC.Quantity > 0 && product_shop_SpC.Active == 1)
            {
                await _allegroAuctionsController.TurnOnAuction(product_id.ToString());
                System.Threading.Thread.Sleep(2000); // staramy sie zmniejszyć szanse na brak aktualizacji ceny. 
            }

            await _allegroAuctionsController.UpdateAuctionPrice(product_id.ToString());

            return View();
        }
    }
}