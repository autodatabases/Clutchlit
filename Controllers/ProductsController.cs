using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        public string CheckAutodocBackground(string stock)
        {
            if (stock.Trim().Contains("Aktualnie"))
                return "red";
            else
                return "green";
        }
        public string CheckUcandoBackground(string stock)
        {
            if (stock.Trim().Contains("2-3 dni"))
                return "green";
            else
                return "red";
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
        public string ParseReference(string reference, int type)
        {
            string result = "";
            if (type == 6)
            {
                result = reference[0].ToString() + reference[1].ToString() + reference[2].ToString() + " " + reference[3].ToString() + reference[4].ToString() + reference[5].ToString() + reference[6].ToString() + " " + reference[7].ToString() + reference[8].ToString();
            }
            else if (type == 32)
            {
                result = reference[0].ToString() + reference[1].ToString() + reference[2].ToString() + reference[3].ToString() + " " + reference[4].ToString() + reference[5].ToString() + reference[6].ToString() + " " + reference[7].ToString() + reference[8].ToString() + reference[9].ToString();
            }
            else
                result = reference.Replace(" ", "").ToLower();
            return result;
        }
        public IActionResult GetOpponentsPrices(int Id)
        {
            string iparts_string = "";
            string ucando_string = "";
            string interCars_string = "";
            string autodoc_string = "";
            string ceneo_string = "";
            string czesci_string = "";
            //
            Product product = _context.Products.Where(m => m.Id == Id).Single();
            string manufacturer_name = "DUPA";
            manufacturer_name = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).First().Description.ToUpper().Replace(" ", "");

            // IPARTS
            Task<string> iparts = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
                string resultA = "";
                using (var response = client.GetAsync("https://www.iparts.pl/wyszukaj/art" + EncodeBase64(product.Reference) + ".html").Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
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
                                    if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                    {
                                        if (title.Contains(manufacturer_name))
                                        {
                                            var price = node.SelectSingleNode(".//div[@class=\"cena\"]").InnerText;
                                            var stock = node.SelectSingleNode(".//div[@class=\"katalog-akcje-kosz\"]").InnerText;
                                            resultA = resultA + "<tr class='" + CheckIpartsBackground(stock) + "'><td><b>IPARTS</b></td><td>" + price.Replace(" ", "").Replace("złzVAT", " PLN").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                }
                            }
                            result = resultA;
                        }
                        else
                        {
                            result = "<tr class='orange'><td><b>IPARTS</b></td><td colspan='2'>B/D</td></tr><";
                        }
                    }
                }
                return result;
            });
            // IPARTS

            // UCANDO
            Task<string> ucando = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
                string resultA = "";
                using (var response = client.GetAsync("https://www.ucando.pl/szukaj?text=" + product.Reference.Replace(" ", "") + "").Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
                        var document = new HtmlDocument();
                        document.LoadHtml(resultB);
                        var nodes = document.DocumentNode.SelectNodes("//product-item[@class=\"o-product-list__item c-product-item\"]");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node != null)
                                {
                                    var title = node.SelectSingleNode(".//h3[@class=\"c-product-item__name\"]").InnerText.ToUpper().Replace(" ", "");
                                    if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                    {
                                        if (title.Contains(manufacturer_name))
                                        {
                                            var price = node.SelectSingleNode(".//span[@class=\"c-price__current\"]").InnerText;
                                            var stock = node.SelectSingleNode(".//span[@class=\"c-price__delivery-note c-price__delivery-note--inStock\"]").InnerText;
                                            resultA = resultA + "<tr class='" + CheckUcandoBackground(stock) + "'><td><b>UCANDO</b></td><td>" + price.Replace(" ", "").Replace("zł", " PLN").Trim() + "</td><td>" + stock.Replace("Dostępny.", "").Trim() + "</td></tr>";
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                }
                            }
                            result = resultA;
                        }
                        else
                        {
                            result = "<tr class='orange'><td><b>UCANDO</b></td><td colspan='2'>B/D</td></tr>";
                        }
                    }
                }
                return result;
            });
            // UCANDO

            // INTER-CARS
            Task<string> interCars = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
                string resultA = "";
                using (var response = client.GetAsync("https://intercars.pl/szukaj/" + product.Reference.Replace(" ", "") + ".html").Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
                        var document = new HtmlDocument();
                        document.LoadHtml(resultB);
                        var nodes = document.DocumentNode.SelectNodes("//div[contains(@class, \"gtm-item\")]");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node != null)
                                {
                                    var title = node.SelectSingleNode(".//span[@class=\"prod-label cleared\"]").InnerText.ToUpper().Replace(" ", "");
                                    if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                    {
                                        if (title.Contains(manufacturer_name))
                                        {
                                            var price = node.SelectSingleNode(".//span[@class=\"current-price\"]").InnerText;
                                            var stock = "B/D";
                                            resultA = "<tr class='orange'><td><b>INTER-CARS</b></td><td>" + price.Replace(" ", "").Replace("zł", " PLN").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                }
                            }
                            result = resultA;
                        }
                        else
                        {
                            result = "<tr class='orange'><td><b>INTER-CARS</b></td><td colspan='2'>B/D</td></tr>";
                        }
                    }
                }
                return result;
            });
            // INTER-CARS

            // AUTO DOC
            Task<string> autodoc = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
                string resultA = "";
                using (var response = client.GetAsync("https://www.autodoc.pl/search?keyword=" + product.Reference.Replace(" ", "")).Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
                        var document = new HtmlDocument();
                        document.LoadHtml(resultB);
                        var nodes = document.DocumentNode.SelectNodes("//li[contains(@class, \"ovVisLi\")]");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node != null)
                                {
                                    var title = node.SelectSingleNode(".//span[@class=\"article_number\"]").InnerText.ToUpper().Replace(" ", "");
                                    if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                    {
                                        if (node.SelectSingleNode(".//div[@class=\"name\"]").InnerText.ToUpper().Replace(" ", "").Contains(manufacturer_name))
                                        {//delivery
                                            var price = node.SelectSingleNode(".//p[@class=\"actual_price small_price\"]").InnerText;
                                            var stock = node.SelectSingleNode(".//div[@class=\"delivery\"]").InnerText;

                                            resultA = resultA + "<tr class='" + CheckAutodocBackground(stock.Trim()) + "'><td><b>AUTODOC</b></td><td>" + price.Replace(" ", "").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                }
                            }
                            result = resultA;
                        }
                        else
                        {
                            result = "<tr class='orange'><td><b>AUTODOC</b></td><td colspan='2'>B/D</td></tr>";
                        }
                    }
                }
                return result;
            });
            // AUTO DOC

            // CENEO 
            Task<string> ceneo = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
                string resultA = "";
                Double lowestPrice = 100000;
                string partialResult = "";
                using (var response = client.GetAsync("https://www.ceneo.pl/Motoryzacja;szukaj-" + ParseReference(product.Reference, product.Manufacturer_id)).Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
                        var document = new HtmlDocument();
                        document.LoadHtml(resultB);
                        var nodes = document.DocumentNode.SelectNodes("//div[@class=\"cat-prod-row-body\"]");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node != null)
                                {
                                    var title = node.SelectSingleNode(".//strong[@class=\"cat-prod-row-name\"]").InnerText.ToUpper().Replace(" ", "");
                                    if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                    {
                                        if (title.Contains(manufacturer_name))
                                        {//delivery

                                            var price = node.SelectSingleNode(".//span[@class=\"price\"]").InnerText;
                                            var stock = "Dostępny";
                                            if (Double.Parse(price.Replace("zł", "").Trim()) < lowestPrice)
                                            {
                                                lowestPrice = Double.Parse(price.Replace("zł", "").Trim());
                                                partialResult = "<tr class='green'><td><b>CENEO</b></td><td>" + price.Replace(" ", "").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                }
                            }
                            resultA = resultA + partialResult;
                            result = resultA;
                        }
                        else
                        {
                            result = "<tr class='orange'><td><b>CENEO</b></td><td colspan='2'>B/D</td></tr>";
                        }
                    }
                }
                return result;
            });
            // CENEO

            // CZESCI AUTO
            Task<string> czesciauto = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0");
                string resultA = "";
                using (var response = client.GetAsync("https://www.czesciauto24.pl/search?keyword=" + product.Reference.Replace(" ", "")).Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
                        var document = new HtmlDocument();
                        document.LoadHtml(resultB);
                        var nodes = document.DocumentNode.SelectNodes("//div[@class=\"brand-products\"]");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node != null)
                                {
                                    var title = node.SelectSingleNode(".//div[@class=\"nr\"]").InnerText.ToUpper().Replace(" ", "");
                                    if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                    {
                                        if (node.SelectSingleNode(".//a[@class=\"ga-click prod_link\"]").InnerText.ToUpper().Replace(" ", "").Contains(manufacturer_name))
                                        {//vers_box 
                                            var price = node.SelectSingleNode(".//div[@class=\"price\"]").InnerText;
                                            var stock = node.SelectSingleNode(".//span[contains(@class, \"text_vers\")]").InnerText;

                                            resultA = resultA + "<tr class='" + CheckAutodocBackground(stock.Trim()) + "'><td><b>CZĘŚCIAUTO</b></td><td>" + price.Replace(" ", "").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                }
                            }
                            result = resultA;
                        }
                        else
                        {
                            result = "<tr class='orange'><td><b>CZĘŚCIAUTO</b></td><td colspan='2'>B/D</td></tr>";
                        }
                    }
                }
                return result;
            });
            // CZESCI AUTO

            ////////////////////////////////////
            iparts_string = iparts.Result;
            ucando_string = ucando.Result.Replace("<","");
            interCars_string = interCars.Result;
            autodoc_string = autodoc.Result;
            ceneo_string = ceneo.Result;
            czesci_string = czesciauto.Result;
            var res = iparts_string + ucando_string + interCars_string + autodoc_string + ceneo_string + czesci_string +"</table>";
            return Json(res);
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