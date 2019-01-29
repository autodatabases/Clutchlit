using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Clutchlit.Data;
using Clutchlit.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clutchlit.Controllers
{
   
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static List<string> busketList = new List<string>();

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
        public IActionResult AddToCart(int id)
        {
            string key = "product_id";
            busketList.Add(id.ToString());
            string data = string.Join(",", busketList.ToArray());

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(20);
            
            Response.Cookies.Append(key, data, option);

            // aktualizujemy koszyk
            ViewData["busketItemsNumber"] = "ass";
            //

            // czytamy 
            string cookieValueFromReq = Request.Cookies["product_id"];
            return Json("Dodano do koszyka");

        }
        [HttpGet("/Products/ListProducts/")]
        public IActionResult ListProducts([FromQuery]string q)
        {
            var search_string = System.Web.HttpUtility.HtmlEncode(q);
        

            // lista produktów
            var all_p = _context.Products;
            IQueryable<Product> result = null;
            result = from p in all_p
                     where p.Reference.ToUpper().Contains(search_string.Replace(" ","").ToUpper())
            select p;
            
            var customerData = result.ToList();
            //
            ViewData["products"] = customerData;
            ViewData["search_string"] = q;
            return View();
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
           
            var list = _context.PassengerCars.Where(p => p.Modelid == id);
            var list_a = _context.PcAttributes;
            List<PassengerCar> result = null;

            result = (from p in list
                     join a in list_a on p.Ktype equals a.Pc_id
                     select new PassengerCar { Ktype = p.Ktype, Description = p.Description, Constructioninterval = p.Constructioninterval, Id = p.Id, Modelid = p.Modelid, Fulldescription = a.Description }).OrderBy(c => c.Description).ToList();
            
            Response.StatusCode = 200;
            return new JsonResult(new SelectList(result, "Ktype", "SelectDesc"));
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

        public string CheckCzesciBackground(string stock)
        {
            if (stock.Trim().Contains("Niedost"))
                return "red";
            else
                return "green";
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
                result = reference[0].ToString() + reference[1].ToString() + reference[2].ToString() + "+" + reference[3].ToString() + reference[4].ToString() + reference[5].ToString() + reference[6].ToString() + "+" + reference[7].ToString() + reference[8].ToString();
            }
            else if (type == 32)
            {
                result = reference[0].ToString() + reference[1].ToString() + reference[2].ToString() + reference[3].ToString() + "+" + reference[4].ToString() + reference[5].ToString() + reference[6].ToString() + "+" + reference[7].ToString() + reference[8].ToString() + reference[9].ToString();
            }
            else
            {
                result = reference.Replace(" ", "").ToLower();
            }
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
            string ebay_string = "";

            //
            Product product = _context.Products.Where(m => m.Id == Id).SingleOrDefault();
            string manufacturer_name = "DUPA";
            if(product != null)
            {
                if(_context.Suppliers.Where(p => p.Tecdoc_id == product.Manufacturer_id) != null)
                {
                    manufacturer_name = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).FirstOrDefault().Description.ToUpper().Replace(" ", "");
                }
            }
            else
            {

            }
            
            // EBAY
            Task<string> ebay = Task<string>.Factory.StartNew(() =>
            {
            string result = "";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
            string resultA = "";
            Double lowestPrice = 100000;
            string partialResult = "";

            try
            {
                using (var response = client.GetAsync("https://www.ebay.com/sch/i.html?_from=R40&_trksid=m570.l1313&_nkw=+" + product.Reference.ToUpper().Replace(" ", "") + "&_sacat=0").Result)
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var resultB = content.ReadAsStringAsync().Result;
                        var document = new HtmlDocument();
                        document.LoadHtml(resultB);

                        var nodes = document.DocumentNode.SelectNodes("//li[@class=\"s-item  \"]");
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node != null)
                                {
                                    var titleA = node.SelectSingleNode(".//h3[@class=\"s-item__title\"]");
                                        if(titleA != null)
                                        {
                                            var title = titleA.InnerText.ToUpper().Replace(" ", "");
                                            if (title.Contains(product.Reference.ToUpper().Replace(" ", "")))
                                            {
                                                if (title.Contains(manufacturer_name.ToUpper()))
                                                {//delivery
                                                    var price = node.SelectSingleNode(".//span[@class=\"s-item__price\"]").InnerText;
                                                    var stock = "Dostępny";

                                                    string shippingPrice = "";
                                                    var shipprice = node.SelectSingleNode(".//span[@class=\"s-item__shipping s-item__logisticsCost\"]").InnerText;
                                                    if ( shipprice != null)
                                                    {
                                                        shippingPrice +=  shipprice.Replace(".", ",");
                                                    }

                                                    double TotalPrice = Double.Parse(price.Replace("$", "").Replace(".", ",").Trim());

                                                    if (TotalPrice < lowestPrice)
                                                    {
                                                        lowestPrice = TotalPrice;
                                                        string pr = Math.Round((lowestPrice * 3.76),2).ToString();
                                                        partialResult = "<tr class='green'><td><b>EBAY</b></td><td>" + pr + " PLN "+ shipprice+"</td><td>" + stock.Trim() + "</td></tr>";
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                    
                                }
                                resultA = resultA + partialResult;
                                result = resultA;
                            }
                            else
                            {
                                result = "<tr class='orange'><td><b>EBAY</b></td><td colspan='2'>B/D</td></tr>";
                            }
                        }
                    }
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                return result;
            });

            // EBAY
            // IPARTS
            Task<string> iparts = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
                string resultA = "";
                try
                {
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
                                result = "<tr class='orange'><td><b>IPARTS</b></td><td colspan='2'>B/D</td></tr>";
                            }
                        }
                    }
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                
                return result;
            });
            // IPARTS

            // UCANDO
            Task<string> ucando = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
                string resultA = "";

                try
                {
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
                                            if (manufacturer_name != "" && manufacturer_name != null && manufacturer_name != "DUPA")
                                            {
                                                if (title.Contains(manufacturer_name))
                                                {
                                                    var price = node.SelectSingleNode(".//span[@class=\"c-price__current\"]").InnerText;
                                                    var stock = node.SelectSingleNode(".//span[@class=\"c-price__delivery-note c-price__delivery-note--inStock\"]").InnerText;
                                                    resultA = resultA + "<tr class='" + CheckUcandoBackground(stock) + "'><td><b>UCANDO</b></td><td>" + price.Replace(" ", "").Replace("zł", " PLN").Trim() + "</td><td>" + stock.Replace("Dostępny.", "").Trim() + "</td></tr>";
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
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
                    
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                return result;
            });
            // UCANDO

            // INTER-CARS
            Task<string> interCars = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
                string resultA = "";
                try
                {
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
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                
                return result;
            });
            // INTER-CARS

            // AUTO DOC
            Task<string> autodoc = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
                string resultA = "";
                try
                {
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
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                
                return result;
            });
            // AUTO DOC

            // CENEO 
            Task<string> ceneo = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
                string resultA = "";
                Double lowestPrice = 100000;
                string partialResult = "";

                try
                {
                    using (var response = client.GetAsync("https://www.ceneo.pl/Czesci_samochodowe;szukaj-" + ParseReference(product.Reference.Trim(), product.Manufacturer_id).Trim()).Result)
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
                                                    partialResult = "<tr class='green'><td><b>CENEO</b></td><td>" + price.Replace(" ", "").Trim() + " PLN</td><td>" + stock.Trim() + "</td></tr>";
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
                }
                catch(AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                return result;
            });
            // CENEO

            // CZESCI AUTO
            Task<string> czesciauto = Task<string>.Factory.StartNew(() =>
            {
                string result = "";
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
                string resultA = "";
                try
                {
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
                                            if (node.SelectSingleNode(".//*[contains(@class, \"prod_link\")]").InnerText.ToUpper().Replace(" ", "").Contains(manufacturer_name))
                                            {//vers_box 
                                                var price = node.SelectSingleNode(".//div[@class=\"price\"]").InnerText;
                                                var stock = node.SelectSingleNode(".//span[contains(@class, \"text_vers\")]").InnerText;

                                                resultA = resultA + "<tr class='" + CheckCzesciBackground(stock.Trim()) + "'><td><b>CZĘŚCIAUTO</b></td><td>" + price.Replace(" ", "").Trim() + "</td><td>" + stock.Trim() + "</td></tr>";
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
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                
                return result;
            });
            // CZESCI AUTO

            ////////////////////////////////////
            if (iparts.Result != null)
                iparts_string = iparts.Result;
            if(ucando.Result != null)
                ucando_string = ucando.Result;
            if (interCars.Result != null)
                interCars_string = interCars.Result;
            if (autodoc.Result != null)
                autodoc_string = autodoc.Result;
            if (ceneo.Result != null)
                ceneo_string = ceneo.Result;
            if (czesciauto.Result != null)
                czesci_string = czesciauto.Result;
            if(ebay != null)
                ebay_string = ebay.Result;

            var res = iparts_string + ucando_string + interCars_string + autodoc_string + ceneo_string + czesci_string + ebay_string +"</table>";
            return Json(res);
     
        }
        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://katalog.adpolska.pl/ws/api?wsdl");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
        public string Api_elit_call(string Reference , int Manufacturer)
        {
            Reference = "826705";
            Manufacturer = 21;
            string result = "";
            string Prefix = "";
            string Ref = "";
            Ref = Reference.Replace(" ", "").ToUpper();
            switch (Manufacturer)
            {
                case 6:
                    {
                        Prefix = "L";
                        break;
                    }
                case 15:
                    {
                        Prefix = "NGK";
                        break;
                    }
                case 21:
                    {
                        Prefix = "V";
                        break;
                    }
                case 32:
                    {
                        Prefix = "SCH";
                        break;
                    }
            }
            Ref = Prefix + " " +Ref;

            string query = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns1=\"http://katalog.adpolska.pl/ws/api/1.0/\">" +
                "<SOAP-ENV:Header>" +
                "<api_key>c86d4779cd53fae70b16d047973caf4f42441e57</api_key>" +
                "<kh_kod>066140</kh_kod>" +
                "<personal_key>6f41ed68678895eae1df0f0fae28c8a3445e01ca</personal_key>" +
                "</SOAP-ENV:Header>" +
                "<SOAP-ENV:Body>" +
                "<ns1:getProductPrice>" +
                "<id>" +
                "<item>" +
                "<index>" + Ref + "</index>" +
                "</item>" +
                "</id>" +
                "</ns1:getProductPrice>" +
                "</SOAP-ENV:Body>" +
                "</SOAP-ENV:Envelope>";

            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@query);

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    result += soapResult;
                }
            }
            return result;
        }
        [HttpPost]
        public IActionResult GetDistributorsPrices(int Id)
        {
            string result = "";
            //
            Product product = new Product();
            product = _context.Products.Where(d => d.Id == Id).SingleOrDefault();
            string reference = "";
            //
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
            // pobieramy API ELIT
            var thread_elit = new Thread(start =>
            {
                // zadanie do wykonania
            });
            thread_elit.Start();
            // API ELIT
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