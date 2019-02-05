using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clutchlit.Data;
using Clutchlit.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clutchlit.Controllers
{
    public class AllegroAuctionsController : Controller
    {
        public static string Token = "";
        private static string SellerId = "sprzeglo-com-pl";
        private static string AccessToken = "";
        private IHostingEnvironment hostingEnv;
        private static string pathToApp = "http://clutchlit.trimfit.pl/";
        private readonly ApplicationDbContext _context;
        private readonly MysqlContext _contextShop;
        HttpClient client = new HttpClient();

        public List<Auction> list = new List<Auction>();

        public IActionResult Index()
        {
            return View();
        }

        public AllegroAuctionsController(IHostingEnvironment env, ApplicationDbContext context, MysqlContext contextShop)
        {
            _context = context;
            _contextShop = contextShop;
            client.BaseAddress = new Uri("https://api.allegro.pl/");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            this.hostingEnv = env;
        }
        public async Task<List<string>> GetAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            HttpResponseMessage response = await client.GetAsync("sale/offers", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<string>>();
            }
            return new List<string>();
        }
        public IActionResult getTokTest()
        {
            return Json(Token);
        }
        public string GetToken(string token)
        {
            string response = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://allegro.pl/auth/oauth/token?grant_type=authorization_code&code=" + token + "&redirect_uri=http://clutchlit.trimfit.pl/AllegroAuctions/GetList/api/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Basic NGZlMGU1ZTcxMTRjNDE1ZmI3ZjY5Y2JmZDFkYWFiMTY6dGtwWGF2WWFmTGVmRnNRQllBRDV6UDl0S2lXQU1RdW9hc05WMHhndTZJRzRXYU12ZmllWnpFNjNQU1k5RlNSRQ==");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = JObject.Parse(streamReader.ReadToEnd());
                foreach (var property in resource.Properties())
                {
                    if (property.Name == "access_token")
                    {
                        response = property.Value.ToString();
                        AccessToken = response;
                    }

                }

                return response;
            }

        }
        public void GetOffersList(string token)
        {
            list.Clear();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offers?limit=1000");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/vnd.allegro.beta.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var offers = x.offers;
                foreach (var offer in offers)
                {
                    Auction of = new Auction()
                    {
                        Id = offer.id,
                        Name = offer.name,
                        Category = offer.category.id,
                        PrimaryImage = offer.primaryImage.url,
                        Price = offer.sellingMode.price.amount + " PLN",
                        Watchers = offer.stats.watchersCount,
                        Visits = offer.stats.visitsCount,
                        Status = offer.publication.status,
                    };
                    list.Add(of);
                }
            }

        }
        public IActionResult EndOffer(string Id)
        {
            var uuid = Guid.NewGuid().ToString();
            string data = "{" +
  "\"offerCriteria\": [" +
    "{" +
      "\"offers\": [" +
        "{" +
          "\"id\": \"" + Id + "\"" +
        "}" +
      "]," +
      "\"type\": \"CONTAINS_OFFERS\"" +
    "}" +
  "]," +
  "\"publication\": {" +
    "\"action\": \"END\"" +
    "}" +
    "}";

            string response = "Coś poszło nie tak. Skontaktuj się z pokojem obok.";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offer-publication-commands/" + uuid + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            var resource = "";
            var httpWebRequest2 = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offer-publication-commands/" + uuid + "");
            httpWebRequest2.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest2.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest2.Method = "GET";
            httpWebRequest2.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();

            using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
            {
                resource = streamReader.ReadToEnd();
            }

            Response.StatusCode = 200;
            return new JsonResult("Aukcja została zakończona");
        }
        public IActionResult ActivateOffer(string Id)
        {
            var uuid = Guid.NewGuid().ToString();
            string data = "{" +
  "\"offerCriteria\": [" +
    "{" +
      "\"offers\": [" +
        "{" +
          "\"id\": \"" + Id + "\"" +
        "}" +
      "]," +
      "\"type\": \"CONTAINS_OFFERS\"" +
    "}" +
  "]," +
  "\"publication\": {" +
    "\"action\": \"ACTIVATE\"" +
    "}" +
    "}";

            string response = "Coś poszło nie tak. Skontaktuj się z pokojem obok.";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offer-publication-commands/" + uuid + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            var resource = "";
            var httpWebRequest2 = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offer-publication-commands/" + uuid + "");
            httpWebRequest2.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest2.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest2.Method = "GET";
            httpWebRequest2.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();

            using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
            {
                resource = streamReader.ReadToEnd();
            }

            Response.StatusCode = 200;
            return new JsonResult("Aukcja została zakończona");
        }
        [HttpGet("[controller]/[action]s/{id}/")]
        public IActionResult EditOffer(string id)
        { // edytujemy ofertę o ID

            return View();
        }
        [HttpGet("[controller]/[action]/")]
        public IActionResult GetList()
        {

            return View();
        }
        [HttpGet("[controller]/[action]/api/")]
        public IActionResult GetList([FromQuery(Name = "code")] string a_query)
        {

            var result = this.GetToken(a_query);
            ViewData["token"] = result;
            Token = result;
            ViewData["code"] = a_query;

            return View();
        }

        public IActionResult OffersList()
        {
            GetOffersList(Token);
            var data = list;
            return new JsonResult(new { data = data });


        }

        // dodawanie ręczne oferty

        // pobieramy metody dostawy
        public List<SelectListItem> GetDeliveryMethods()
        {
            List<SelectListItem> deliveryMethod = new List<SelectListItem>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/delivery-methods");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.deliveryMethods;
                foreach (var method in methods)
                {
                    deliveryMethod.Add(new SelectListItem { Selected = false, Text = method.name.ToString(), Value = method.id.ToString() });
                }
            }
            return deliveryMethod;
        }
        // pobieranie metod dostawy

        // pobieranie cennika dostaw
        public List<SelectListItem> GetShippingRates(string sellerId)
        {
            List<SelectListItem> shippingRates = new List<SelectListItem>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/shipping-rates?seller.id=" + sellerId + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.shippingRates;
                foreach (var method in methods)
                {
                    shippingRates.Add(new SelectListItem { Selected = false, Text = method.name.ToString(), Value = method.id.ToString() });
                }
            }
            return shippingRates;
        }
        //
        public List<SelectListItem> GetWarranties(string sellerId)
        {
            List<SelectListItem> warrenties = new List<SelectListItem>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/after-sales-service-conditions/warranties?seller.id=" + sellerId + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.warranties;
                foreach (var method in methods)
                {
                    warrenties.Add(new SelectListItem { Selected = false, Text = method.name.ToString(), Value = method.id.ToString() });
                }
            }
            return warrenties;
        }
        public List<SelectListItem> GetImpliedWarranties(string sellerId)
        {
            List<SelectListItem> warrenties = new List<SelectListItem>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/after-sales-service-conditions/implied-warranties?seller.id=" + sellerId + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.impliedWarranties;
                foreach (var method in methods)
                {
                    warrenties.Add(new SelectListItem { Selected = false, Text = method.name.ToString(), Value = method.id.ToString() });
                }
            }
            return warrenties;
        }
        public List<SelectListItem> GetReturnPolicy(string sellerId)
        {
            List<SelectListItem> warrenties = new List<SelectListItem>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/after-sales-service-conditions/return-policies?seller.id=" + sellerId + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.returnPolicies;
                foreach (var method in methods)
                {
                    warrenties.Add(new SelectListItem { Selected = false, Text = method.name.ToString(), Value = method.id.ToString() });
                }
            }
            return warrenties;
        }
        // OBSŁUGA KATEGORII
        public List<AllegroCategory> GetCategory()
        {
            List<AllegroCategory> categories = new List<AllegroCategory>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/categories");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.categories;
                foreach (var method in methods)
                {
                    categories.Add(new AllegroCategory { Id = method.id, Name = method.name });
                }
            }
            return categories;

        }
        public IActionResult GetChildCategories(string parent_id)
        {
            List<AllegroCategory> categories = new List<AllegroCategory>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/categories?parent.id=" + parent_id + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.categories;
                foreach (var method in methods)
                {
                    categories.Add(new AllegroCategory { Id = method.id, Name = method.name, ParentId = method.parent.id });
                }
            }
            Response.StatusCode = 200;
            return new JsonResult(new SelectList(categories, "Id", "Name"));

        }
        // OBSŁUGA KATEGORII

        // pobieranie parametrów dla wybranej kategorii
        public IActionResult GetParametersForCategory(string catId)
        {
            string categories = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/categories/" + catId + "/parameters");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var methods = x.parameters;
                foreach (var method in methods)
                {
                    string temp = "";
                    if (method.type == "dictionary" && method.restrictions.multipleChoices == false)
                    { // simple select form
                        temp += "<label for='" + method.id + "'><strong>" + method.name + "</strong></label>";
                        temp += "<select class='dictionary form-control' name='" + method.id + "'>";
                        var variants = method.dictionary;
                        foreach (var variant in variants)
                        {
                            temp += "<option value='" + variant.id + "'>" + variant.value + "</option>";
                        }
                        temp += "</select>";
                    }
                    else if (method.type == "dictionary" && method.restrictions.multipleChoices == true)
                    { // checkboxes
                        temp += "<label for='" + method.id + "'><strong>" + method.name + "</strong></label>";
                        temp += "<select multiple class='dictionary form-control' name='" + method.id + "'>";
                        var variants = method.dictionary;
                        foreach (var variant in variants)
                        {
                            temp += "<option value='" + variant.id + "'>" + variant.value + "</option>";
                        }
                        temp += "</select>";
                    }
                    else if (method.type == "string" || method.type == "float" || method.type == "integer")
                    { // input

                        temp += "<label for='" + method.id + "'><strong>" + method.name + "</strong></label>";
                        temp += "<input type='text' class='form-control' value='' name='" + method.id + "' />";
                    }
                    categories += temp;
                }
            }

            Response.StatusCode = 200;
            return new JsonResult(categories);
        }
        // pobieranie parametrów dla wybranej kategorii
        // przesyłanie plików zdjęć na serwer allegro

        public async Task<IActionResult> UploadPhotos(List<IFormFile> files)
        {
            var filesPath = $"{this.hostingEnv.WebRootPath}/images";
            string response = "";
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

                fileName = fileName.Contains("\\")
                    ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                    : fileName.Trim('"');

                var fullFilePath = Path.Combine(filesPath, fileName);
                if (file != null)
                {
                    using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            string image = Convert.ToBase64String(fileBytes); // obrazek w binarnej formie

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://upload.allegro.pl/sale/images");
                            httpWebRequest.ContentType = "image/jpeg";
                            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
                            httpWebRequest.Method = "POST";
                            httpWebRequest.Headers.Add("Accept-language", "pl-PL");
                            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

                            Stream requestStream = httpWebRequest.GetRequestStream();
                            requestStream.Write(fileBytes, 0, fileBytes.Length);
                            requestStream.Close();

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse(); // odczytujemy response od allegro

                            using (var readStream = new StreamReader(httpResponse.GetResponseStream(), Encoding.Default))
                            {
                                var resource = readStream.ReadToEnd();
                                dynamic x = JsonConvert.DeserializeObject(resource);
                                var location = x.location;
                                var expiresAt = x.expiresAt;
                                response += location + ";";
                            }
                        }
                        await file.CopyToAsync(stream);
                    }
                }
                else
                    return Json("Wystąpił błąd podczas dodawania!");
            }
            return Json(response);
        }


        // przesyłanie plików zdjęć na serwer
        public IActionResult AddAuction()
        {
            //ViewData["token"] = AccessToken;
            ViewData["Delivery"] = GetDeliveryMethods();
            ViewData["Shipping"] = GetShippingRates("45582318");
            ViewData["Warranty"] = GetWarranties("45582318");
            ViewData["ImpliesWarranty"] = GetImpliedWarranties("45582318");
            ViewData["ReturnPolicy"] = GetReturnPolicy("45582318");
            ViewData["MainCategories"] = GetCategory();
            return View();
        }
        [HttpGet("AllegroAuctions/GetValidationData/{id}")]
        public IActionResult GetValidationData(string id)
        {

            return Json(id);

        }
        [HttpGet("[controller]/[action]/")]
        public IActionResult MassiveAction()
        {
            return View();
        }
        ///[Produces("application/json")]
        public IActionResult GetAllegroAuctionsList(string FlagCategory, string FlagManufacturer)
        {
            var auctionsList = _context.AllegroAuction;
            var productsList = _context.Products;
            var List = Enumerable.Empty<AllegroAuction>().AsQueryable();

            // tylko filtrowanie
            if (FlagCategory == "0" && FlagManufacturer == "ALL")
            {
                List = (from auctions in auctionsList
                        select new AllegroAuction()
                        {
                            AuctionId = auctions.AuctionId,
                            AllegroId = auctions.AllegroId,
                            ProductId = auctions.ProductId,
                            AuctionTitle = auctions.AuctionTitle,
                            Category = auctions.Category,
                            Status = auctions.Status
                        });
            }
            else
            {
                if (FlagCategory == "0" && FlagManufacturer != "ALL")
                {
                    List = (from auctions in auctionsList
                            join products in productsList on auctions.ProductId equals products.Id
                            where products.Manufacturer_id == int.Parse(FlagManufacturer)
                            select new AllegroAuction()
                            {
                                AuctionId = auctions.AuctionId,
                                AllegroId = auctions.AllegroId,
                                ProductId = auctions.ProductId,
                                AuctionTitle = auctions.AuctionTitle,
                                Category = auctions.Category,
                                Status = auctions.Status
                            });
                }
                else if (FlagCategory != "0" && FlagManufacturer == "ALL")
                {
                    List = (from auctions in auctionsList
                            where auctions.Category == FlagCategory
                            select new AllegroAuction()
                            {
                                AuctionId = auctions.AuctionId,
                                AllegroId = auctions.AllegroId,
                                ProductId = auctions.ProductId,
                                AuctionTitle = auctions.AuctionTitle,
                                Category = auctions.Category,
                                Status = auctions.Status
                            });
                }
                else if (FlagCategory != "0" && FlagManufacturer != "ALL")
                {
                    List = (from auctions in auctionsList
                            join products in productsList on auctions.ProductId equals products.Id
                            where products.Manufacturer_id == int.Parse(FlagManufacturer) && auctions.Category == FlagCategory
                            select new AllegroAuction()
                            {
                                AuctionId = auctions.AuctionId,
                                AllegroId = auctions.AllegroId,
                                ProductId = auctions.ProductId,
                                AuctionTitle = auctions.AuctionTitle,
                                Category = auctions.Category,
                                Status = auctions.Status
                            });
                }
            }


            var count = List.Count();

            var concat = List;
            // porządkujemy liste
            var concat_sorted = concat.OrderByDescending(o => o.AuctionId);


            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            int skip = start != null ? Convert.ToInt32(start) : 0;

            int recordsTotal = 0;
            IQueryable<AllegroAuction> result = null;
            result = concat_sorted.AsQueryable();
            var customerData = result;
            //Sorting  

            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.AuctionTitle.ToUpper().Contains(searchValue.ToUpper()));
            }
            //Paging   
            recordsTotal = customerData.Count();
            //Paging   
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            Response.StatusCode = 200;
            return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

        }
        // konwertujemy plik na postać binarną
        private byte[] GetBinaryFile(string filename)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            return bytes;
        }
        // Massive action
        public IActionResult GetPhotoLink()
        {
            var photos = _context.AllegroPhotos.Where(p => p.ProductId == 42).Single(); // pobieramy kategorie do zdjęć.
            string path = "";
            string folderPath = hostingEnv.WebRootPath + "/images/allegro/6" + "/" + photos.CategoryId + "";
            string[] fileArray = Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories);

            foreach (string fileName in fileArray)
            {
                string absPaht = Path.Combine(folderPath, fileName);
                path = absPaht;
            }

            return Json(path);
        }
        public IActionResult TestPhotoUp()
        {
            string folderPath = hostingEnv.WebRootPath + "/images/allegro/" + "6" + "/" + "1607" + "";
            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
            List<string> fileLinks = new List<string>();
            string response = "";

            foreach (FileInfo fileName in Files)
            {
                string pathToFile = pathToApp + "images/allegro/" + "6/" + "1607/" + fileName.Name;


                string data = "{\"url\": \"" + pathToFile + "\"}";
                response += ";" + data;

                /*
                var httpWebRequestPhoto = (HttpWebRequest)WebRequest.Create("https://upload.allegro.pl/sale/images");
                httpWebRequestPhoto.ContentType = "application/vnd.allegro.public.v1+json";
                httpWebRequestPhoto.Accept = "application/vnd.allegro.public.v1+json";
                httpWebRequestPhoto.Method = "POST";
                httpWebRequestPhoto.Headers.Add("Authorization", "Bearer " + Token + "");

                using (var streamWriter = new StreamWriter(httpWebRequestPhoto.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequestPhoto.GetResponse();
                using (var readStream = new StreamReader(httpResponse.GetResponseStream(), Encoding.Default))
                {
                    var resource = readStream.ReadToEnd();
                    response = resource;
                    dynamic x = JsonConvert.DeserializeObject(resource);
                    var location = x.location;
                    var expiresAt = x.expiresAt;
                    fileLinks.Add(location);
                */

            }
            return Json(response);
        }
        public IActionResult PostAuction(string AuctionId, string Title, string Category, string CreatedAt, string UpdatedAt, string ValidatedAt)
        {

            var auctionData = _context.AllegroAuction.Where(m => m.AllegroId == AuctionId).Single();

            var product = _context.Products.Where(p => p.Id == auctionData.ProductId).Single();
            var manufacturer = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).Single();
            var usage = _context.AllegroAuctionUsage.Where(u => u.AuctionId == auctionData.AuctionId).ToList();
            var photos = _context.AllegroPhotos.Where(p => p.ProductId == product.Id).Single(); // pobieramy kategorie do zdjęć.

            string TitlePost = "";

            string folderPath = hostingEnv.WebRootPath + "/images/allegro/" + manufacturer.Tecdoc_id.ToString() + "/" + photos.CategoryId.ToString() + "";
            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
            List<string> fileLinks = new List<string>();
            string response = "";

            foreach (FileInfo fileName in Files)
            {
                string pathToFile = pathToApp + "images/allegro/" + manufacturer.Tecdoc_id.ToString() +"/" + photos.CategoryId.ToString() +"/" + fileName.Name;
               // string pathToFile = Path.Combine(pathToApp, "images/allegro", manufacturer.Tecdoc_id.ToString(), photos.CategoryId.ToString(), fileName);
                string data = "{\"url\": \"" + pathToFile + "\"}";

                var httpWebRequestPhoto = (HttpWebRequest)WebRequest.Create("https://upload.allegro.pl/sale/images");
                httpWebRequestPhoto.ContentType = "application/vnd.allegro.public.v1+json";
                httpWebRequestPhoto.Accept = "application/vnd.allegro.public.v1+json";
                httpWebRequestPhoto.Method = "POST";
                httpWebRequestPhoto.Headers.Add("Authorization", "Bearer " + Token + "");

                using (var streamWriter = new StreamWriter(httpWebRequestPhoto.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequestPhoto.GetResponse();
                using (var readStream = new StreamReader(httpResponse.GetResponseStream(), Encoding.Default))
                {
                    var resource = readStream.ReadToEnd();
                    dynamic x = JsonConvert.DeserializeObject(resource);
                    var location = Convert.ToString(x.location);
                    var expiresAt = x.expiresAt;
                    fileLinks.Add(location);
                }

            }


            if ((auctionData.AuctionTitle + " " + auctionData.Category + " " + manufacturer.Description).Length <= 49)
                TitlePost = auctionData.Category + " " + manufacturer.Description + " " + auctionData.AuctionTitle;
            else
                TitlePost = auctionData.Category + " " + auctionData.AuctionTitle;

            string productId = "SP-" + product.Id.ToString();

            string price = product.Gross_price.ToString();
            // tu będziemy pobierać dane dot. danego produktu do aukcji
            var auction = new AuctionToPost();
            auction.id = AuctionId;
            auction.name = TitlePost.ToUpper();
            auction.category.id = Category;


            auction.parameters.Add(new Parameters("11323", new string[] { }, new string[] { "11323_1" }));
            auction.parameters.Add(new Parameters("127417", new string[] { }, new string[] { "127417_2" }));
            auction.parameters.Add(new Parameters("129591", new string[] { }, new string[] { "129591_1", "129591_2" }));
            auction.parameters.Add(new Parameters("214434", new string[] { }, new string[] { "214434_266986" }));
            auction.parameters.Add(new Parameters("130531", new string[] { }, new string[] { "130531_1" }));


            auction.ean = null;
            // dodać description

            // PHOTOS
            foreach (string link in fileLinks)
            {
                auction.images.Add(new Images(link));
            }

            // PHOTOS
            //auction.images.Add(new Images("https://a.allegroimg.com/original/11af91/03b8f20345efa50bb520090e8b38"));
            //auction.images.Add(new Images("https://a.allegroimg.com/original/11df2f/d512915b4c9eb1a7d9cd042e5c1e"));

            foreach (var car in usage)
            {
                auction.FillListCompatible(_context.PassengerCars.Where(p => p.Ktype == car.PcId).Single().SelectDesc);
            }

            auction.sellingMode.format = "BUY_NOW";
            auction.sellingMode.price.amount = price;
            auction.sellingMode.price.currency = "PLN";
            auction.sellingMode.minimalPrice = null;
            auction.sellingMode.startingPrice = null;

            auction.stock.available = 1000;
            auction.stock.unit = "UNIT";

            auction.publication.duration = null;
            auction.publication.status = "ACTIVE";
            auction.publication.startingAt = null;
            auction.publication.endingAt = null;

            auction.delivery.shippingRates.id = "b25e1a2e-3f2d-4206-97de-234a9dbf91bf";
            auction.delivery.handlingTime = "PT24H";
            auction.delivery.additionalInfo = "Dodatkowe informacje";
            auction.delivery.shipmentDate = null;

            auction.payments.invoice = "VAT";

            auction.afterSalesServices.impliedWarranty.id = "c2683ac1-b36b-42a1-b0f5-b45bdaf55928";
            auction.afterSalesServices.returnPolicy.id = "eb7c8407-808c-4078-9250-9da488560634";
            auction.afterSalesServices.warranty.id = "0dd88048-8163-4eba-9c12-768551bf407d";

            auction.additionalServices = null;
            auction.sizeTable = null;
            auction.promotion.emphasized = false;
            auction.promotion.bold = false;
            auction.promotion.highlight = false;
            auction.promotion.emphasizedHighlightBoldPackage = false;
            auction.promotion.departmentPage = false;

            auction.location.countryCode = "PL";
            auction.location.province = "MAZOWIECKIE";
            auction.location.city = "Warszawa";
            auction.location.postCode = "00-132";

            auction.external.id = productId;
            auction.contact = null;

            auction.validation.validatedAt = null;
            auction.createdAt = null;
            auction.updatedAt = null;

            var section = new Section();
            //section.items.Add(new Item("TEXT", "<p>Zdjęcia zamieszczone w aukcji mają charakter poglądowy. W rzeczywistości, w zależności od modelu samochodu sprzęgła mogą się trochę różnić.</p>"));
            section.items.Add(new Item("TEXT", "<h1>Nie jesteś pewien czy sprzęgło będzie pasowało do Twojego samochodu?</h1><h1>Zadzwoń lub napisz, chętnie pomożemy!</h1><h1>Nr tel. / e-mail znajdziesz poniżej w zakładce [-- O sprzedającym --]</h1>"));

            auction.description.sections.Add(section);

            string outprint = JsonConvert.SerializeObject(auction, Formatting.Indented);

            // ------

            List<string> Errors = new List<string>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offers/" + AuctionId + "");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(outprint);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var readStream = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var resource = readStream.ReadToEnd();
                    dynamic x = JsonConvert.DeserializeObject(resource);

                    var errors = x.validation.errors;
                    foreach (var error in errors)
                    {
                        Errors.Add(Convert.ToString(error.message));
                    }
                }
            }
            catch
            {
                Response.StatusCode = 410;
            }
            Response.StatusCode = 200;
            return Json("OK");

        }

        public IActionResult PostDraftAuction(string id)
        {
            var auction_id = Convert.ToInt32(id);
            var auction = _context.AllegroAuction.Where(m => m.AuctionId == auction_id).Single();
            // dodać aktualizacje id aukcji allegro do bazy 
            var FirstTitle = auction.Category;
            var Title = auction.AuctionTitle;

            var ConcatTitle = FirstTitle + "" + Title;
            var Category = "50884";

            string outprint = "{" +
                "\"name\": \"" + Title + "\"," +
                "\"category\": " +
                "{\"id\": \"" + Category + "\"" +
                "}" +
                "}";

            List<string> OfferResponse = new List<string>();
            List<string> Errors = new List<string>(); // errors handler

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offers");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(outprint);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var readStream = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = readStream.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                OfferResponse.Add(Convert.ToString(x.id));
                OfferResponse.Add(Convert.ToString(x.validatedAt));
                OfferResponse.Add(Convert.ToString(x.createdAt));
                OfferResponse.Add(Convert.ToString(x.updatedAt));
                OfferResponse.Add(Convert.ToString(x.validation.validatedAt));
                var errors = x.validation.errors;
                foreach (var error in errors)
                {
                    Errors.Add(Convert.ToString(error.message));
                }
            }
            var errors_response = String.Join(", ", Errors.ToArray());

            auction.AllegroId = OfferResponse.ElementAt(0);
            _context.SaveChanges();

            var result = PostAuction(OfferResponse.ElementAt(0), Title, Category, OfferResponse.ElementAt(2), OfferResponse.ElementAt(3), OfferResponse.ElementAt(4)); // wystawiamy aukcję z draft'a
            // pośrednie błędy poniżej
            // return Json(errors_response + " \n " + OfferResponse.First() + result);

            return Json(result.ToString());
        }
    }
}