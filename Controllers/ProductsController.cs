using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Clutchlit.Data;
using Clutchlit.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clutchlit.Controllers
{


    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult List()
        {
            List<Manufacturer> list = new List<Manufacturer>();
            list = _context.Manufacturers.ToList();
            return View(list);
        }
        public IActionResult ModelsList(int id)
        {
            
            List<Model> list = new List<Model>();
            list = _context.Models.Where(m => m.Manufacturer_id == id).ToList();
            Response.StatusCode = 200;

            return new JsonResult(new SelectList(list, "Tecdoc_id", "SelectDesc"));
        }
        public IActionResult PcList(int id)
        {
           
            List<PassengerCar> list = new List<PassengerCar>();
            list = _context.PassengerCars.Where(p => p.Modelid == id).ToList();
            Response.StatusCode = 200;
            return new JsonResult(new SelectList(list, "Ktype", "SelectDesc"));
        }
        public string GetDistributorName(int id)
        {
            string name =  _context.Distributors.Single(d=>d.Id == id).Name;
            return name;
        }
        public string GetDistributorWarehouseName(int id, int disid)
        {
            string name = _context.Warehouses.Single(d => d.WarehouseNumber == id && d.DistributorId == disid).Name;
            return name;
        }
        public string CheckIpartsBackground(string stock)
        {
            if (stock.Trim() == "Dodaj do koszyka")
                return "green";
            else if (stock.Trim().Contains("Zapytaj o"))
                return "orange";
            else
                return "red";
        }
        public string CheckBackground(int quantity)
        {
            if (quantity == 0)
                return "red";
            else
                return "green";
            
        }
        public string EncodeBase64(string reference)
        {
            
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(reference+"|0|0");
            return Convert.ToBase64String(plainTextBytes);
        }

        public async Task<string> GetSm(int Id)
        {
            await Task.Delay(500);
            string ta = "adasd";
            return await Task.FromResult(ta);
        }
        public async Task<string> TakeUcando(string reference, string manufacturer)
        {
            var result = "";
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
            string resultA = "";
            using (var response = await client.GetAsync("https://www.iparts.pl/wyszukaj/art" + EncodeBase64(reference) + ".html"))
            {
                using (var content = response.Content)
                {
                    // read answer in non-blocking way
                    var resultB = await content.ReadAsStringAsync();
                    var document = new HtmlDocument();
                    document.LoadHtml(resultB);
                    var nodes = document.DocumentNode.SelectNodes("//div[@class=\"small-12 medium-12 columns\"]");
                    if (nodes != null)
                    {
                        foreach (HtmlNode node in nodes)
                        {
                            if (node != null)
                            {
                                var title = node.SelectSingleNode(".//h2[@class=\"nazwa naglowek\"]").InnerText.ToUpper().Replace(" ", "");
                                if (title.Contains(reference.ToUpper().Replace(" ", "")))
                                {
                                    if (title.Contains(manufacturer))
                                    {
                                        var price = node.SelectSingleNode(".//div[@class=\"cena\"]").InnerText; ;
                                        var stock = node.SelectSingleNode(".//div[@class=\"katalog-akcje-kosz\"]").InnerText; ;
                                        resultA = resultA + "<tr class='" + CheckIpartsBackground(stock) + "'><td><b>IPARTS</b></td><td>" + price.Replace(" ", "").Replace("złzVAT", " PLN").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                    }
                                    break;
                                }
                            }
                            else
                            {
                            }
                        }
                        resultA = resultA + "</table>";
                        result = resultA;
                    }
                    else
                    {
                        result = "<tr class='orange'><td><b>IPARTS</b></td><td colspan='2'>B/D</td></tr></table>";
                    }
                    //Some work with page....

                }
            }
            return await Task.FromResult(result);
        }
        public async Task<string> GetOpponentsPrices(int Id)
        {
            string result = "";
            Product product = _context.Products.Where(m => m.Id == Id).Single();
            //string reference = product.Reference;
            string manufacturer_name = "DUPA";
            manufacturer_name = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).First().Description.ToUpper().Replace(" ","");
            
            HttpClient client = new HttpClient();
           // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
            string resultA = "";
            using (var response = await client.GetAsync("https://www.iparts.pl/wyszukaj/art" + EncodeBase64(product.Reference) + ".html"))
            {
                using (var content = response.Content)
                {
                    // read answer in non-blocking way
                    var resultB = await content.ReadAsStringAsync();
                    var document = new HtmlDocument();
                    document.LoadHtml(resultB);
                    var nodes = document.DocumentNode.SelectNodes("//div[@class=\"small-12 medium-12 columns\"]");
                    if(nodes != null)
                    {
                        foreach (HtmlNode node in nodes)
                        {
                            if(node !=null)
                            {
                                var title = node.SelectSingleNode(".//h2[@class=\"nazwa naglowek\"]").InnerText.ToUpper().Replace(" ","");
                                if (title.Contains(product.Reference.ToUpper().Replace(" ","")))
                                {
                                    if (title.Contains(manufacturer_name))
                                    {
                                        var price = node.SelectSingleNode(".//div[@class=\"cena\"]").InnerText; ;
                                        var stock = node.SelectSingleNode(".//div[@class=\"katalog-akcje-kosz\"]").InnerText; ;
                                        resultA = resultA + "<tr class='"+CheckIpartsBackground(stock)+"'><td><b>IPARTS</b></td><td>" + price.Replace(" ","").Replace("złzVAT"," PLN").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                    }
                                    break;
                                }
                            }
                            else
                            {
                            }
                        }
                        resultA = resultA + "</table>";
                        result = resultA;
                    }
                    else
                    {
                        result = "<tr class='orange'><td><b>IPARTS</b></td><td colspan='2'>B/D</td></tr></table>";
                    }
                    //Some work with page....
                    
                }
            }

            return await Task.FromResult(result);
        }
        [HttpPost]
        public IActionResult GetDistributorsPrices(int Id)
        {
            string result = "";
            List<PdPrices> list = new List<PdPrices>();
            list = _context.PdPrices.Where(d => d.ProductId == Id).ToList();
            if(list == null)
            {
                result = "Brak danych :(";
            }
            else
            {
                
                foreach (PdPrices s in list)
                {
                    result = result + "<tr class="+CheckBackground(s.Quantity)+"><td><b>" + GetDistributorName(s.DistributorId) + "</b></td><td>"+GetDistributorWarehouseName(s.DistributorWarehouseId, s.DistributorId)+"</td><td>" + s.GrossPrice + "</td><td>"+s.Quantity+"</td></tr>";
                }
                result = result + "</table>";
            }
            

            Response.StatusCode = 200;

            return new JsonResult(result); 
        }
        public IActionResult ProductsList(string engine, int category_id)
        { // dodać obsługę Ktype samochodu     
            //Int32[] categories = Int32.Parse(category_id.Split(","));
            var all_k = _context.PcKtypes.Where(p => p.Ktype == int.Parse(engine));
            var all_p = _context.Products;
            var all_c = _context.PcCategories;

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
     
            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;
            IQueryable<Product> result = null;
            if (category_id == 0)
            {
                result = from k in all_k
                         join p in all_p on k.Product_id equals p.Id
                         select p;
            }
            else
            {
               result = from k in all_k
                        join p in all_p on k.Product_id equals p.Id
                        join c in all_c on p.Id equals c.ProductId
                        where c.CategoryId == category_id
                        select p;
            }
            var customerData = result;
            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.Name.ToUpper().Contains(searchValue.ToUpper()));
            }
            //Paging   
            recordsTotal = customerData.Count();
            //Paging   
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            Response.StatusCode = 200;
            return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        public IActionResult History()
        {
            return View();
        }
    }
}