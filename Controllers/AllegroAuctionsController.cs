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
        // [HttpGet("controller/action/{id}")]
        public IActionResult TestAuction(Int64 AuctionId)
        {
            var TitlePost = "";
            AuctionId = 394007;
            string FinalResponse = "";
            var auctionData = _context.AllegroAuction.Where(m => m.AuctionId == AuctionId).Single();

            var product = _context.Products.Where(p => p.Id == auctionData.ProductId).Single();
            var manufacturer = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).Single();
            var usage = _context.AllegroAuctionUsage.Where(u => u.AuctionId == auctionData.AuctionId).ToList();
            var photos = _context.AllegroPhotos.Where(p => p.ProductId == product.Id).Single(); // pobieramy kategorie do zdjęć.
            string r = "";
            foreach (var p in usage)
            {
                r += p.PcId;
                r += ",";
            }
            string price = product.Gross_price.ToString();
            if ((auctionData.AuctionTitle + " " + auctionData.Category + " " + manufacturer.Description).Length <= 49)
                TitlePost = auctionData.Category + " " + manufacturer.Description + " " + auctionData.AuctionTitle;
            else
                TitlePost = auctionData.Category + " " + auctionData.AuctionTitle;

            string productId = "SP-" + product.Id.ToString();

            return Json(productId + "" + price);
        }
        [HttpGet("get/auction/{id}")]
        public IActionResult GetAuction(string id)
        {
            string result = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offers/" + id + "");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token + "");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                result = resource;
            }
            return Json(result);
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
                    var location = Convert.ToString(x.location);
                    var expiresAt = x.expiresAt;
                    fileLinks.Add(location);
                }
            }
            return Json(String.Join(",", fileLinks.ToArray()));
        }

        // TEST ===========
        [HttpGet("controller/action/{id}")]
        public IActionResult PostAuctionA(string id)
        {
            string FinalResponse = "";

            // tu będziemy pobierać dane dot. danego produktu do aukcji
            var auction = new AuctionToPost();
            auction.id = id.ToString();
            auction.name = "Tytuł przk aukcji";
            auction.category.id = "50884";

            var FeatureList = Enumerable.Empty<AllegroFeatureValue>().AsQueryable();
            var feature = _contextShop.AllegroFeature.Where(f => f.ProductId == 42);
            var featureValue = _contextShop.AllegroFeatureValue;
            var featureLang = _contextShop.AllegroFeatureLang;

            FeatureList = (from features in feature
                           join featuresValue in featureValue on features.FeatureValueId equals featuresValue.FeatureValueId
                           where features.ProductId == 42
                           select new AllegroFeatureValue()
                           {
                               FeatureValueId = featuresValue.FeatureValueId,
                               LangId = featuresValue.LangId,
                               Value = featuresValue.Value,
                               FeatureId = features.FeatureId
                           });

            var radius = FeatureList.Where(f => f.FeatureId == 5002).SingleOrDefault().Value;
            var F_title = FeatureList.Where(f => f.FeatureId == 5000).SingleOrDefault().Value;
            string F_set = "<h2>W ZESTAWIE</h2><ul>";
            foreach (var result in FeatureList.Where(f => f.FeatureId == 5001))
            {
                F_set += "<li>" + result.Value + "</li>";
            }
            F_set += "</ul>";
            //FinalResponse += F_set;

            var TermsList = Enumerable.Empty<AllegroTermsOfUse>().AsQueryable();
            var Terms = _contextShop.AllegroTerms.Where(t => t.ProductId == 30185);

            var usage = _context.AllegroAuctionUsage.Where(u => u.AuctionId == 464127);
            var usageDesc = _context.AllegroUsage;
            var UsageList = Enumerable.Empty<AllegroAuctionUsageDescription>().AsQueryable();
            var UsageDescription = "<h1>Zastosowanie - informacje uzupełniające</h1><h2>W uwagach do zamówienia podaj dane Twojego samochodu i nr VIN - Sprawdzimy czy zamówione części na 100% będą pasowały.</h2><ul>";
            UsageList = (from u in usage
                         join ud in usageDesc on u.PcId equals ud.PcId
                         where u.ProductId == "30185"
                         select new AllegroAuctionUsageDescription()
                         {
                             Description_desc = ud.Description_desc,
                             Description_list = ud.Description_list,
                             Id = ud.Id,
                             PcId = ud.PcId
                         });
            foreach (var r in UsageList)
            {
                var ktype = r.PcId + 500000;
                TermsList = (from t in Terms
                             join f in featureLang on t.FeatureId equals f.FeatureId
                             where t.CategoryId == ktype
                             select new AllegroTermsOfUse()
                             {
                                 CategoryId = t.CategoryId,
                                 FeatureId = t.FeatureId,
                                 LangId = t.LangId,
                                 ProductId = t.ProductId,
                                 Value = t.Value,
                                 Name = f.Value
                             });

                UsageDescription += "<li><b>" + r.Description_desc + "</b> ";
                foreach (var singleterm in TermsList)
                {
                    UsageDescription += singleterm.Name + ":" + singleterm.Value + ", ";
                }
                UsageDescription += "</li>";
            }
            UsageDescription += "</ul>";

            // dodatkowe informacje do proudktu

            FinalResponse += UsageDescription;

            auction.parameters.Add(new Parameters("11323", new string[] { }, new string[] { "11323_1" }));
            auction.parameters.Add(new Parameters("127417", new string[] { }, new string[] { "127417_2" }));
            auction.parameters.Add(new Parameters("129591", new string[] { }, new string[] { "129591_1", "129591_2" }));
            auction.parameters.Add(new Parameters("214434", new string[] { }, new string[] { "214434_266986" }));
            auction.parameters.Add(new Parameters("130531", new string[] { }, new string[] { "130531_1" }));


            auction.ean = null;

            auction.images.Add(new Images("https://a.allegroimg.com/original/11af91/03b8f20345efa50bb520090e8b38"));
            auction.images.Add(new Images("https://a.allegroimg.com/original/11df2f/d512915b4c9eb1a7d9cd042e5c1e"));



            auction.sellingMode.format = "BUY_NOW";
            auction.sellingMode.price.amount = "123";
            auction.sellingMode.price.currency = "PLN";
            auction.sellingMode.minimalPrice = null;
            auction.sellingMode.startingPrice = null;

            auction.stock.available = 100;
            auction.stock.unit = "UNIT";

            auction.publication.duration = null;
            auction.publication.status = "INACTIVE";
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

            auction.external.id = "SPDSDS";
            auction.contact = null;

            auction.FillListCompatible("BMW 3 (E46) 330 d 204 KM / 150 kW 2993 ccm");
            auction.FillListCompatible("BMW 3 (E46) 330 i 231 KM / 170 kW 2979 ccm");

            auction.validation.validatedAt = null;
            auction.createdAt = null;
            auction.updatedAt = null;

            var section = new Section();
            section.items.Add(new Item("TEXT", "<p>Zdjęcia zamieszczone w aukcji mają charakter poglądowy. W rzeczywistości, w zależności od modelu samochodu sprzęgła mogą się trochę różnić.</p>", null));
            section.items.Add(new Item("IMAGE", null, "https://a.allegroimg.com/original/11df2f/d512915b4c9eb1a7d9cd042e5c1e"));

            auction.description.sections.Add(section);

            var outprint = JsonConvert.SerializeObject(auction);

            // ------

            return Json(FinalResponse);
        }

        // TEST ===========

        public IActionResult PostAuction(string AllegroId, string Title, string Category, string CreatedAt, string UpdatedAt, string ValidatedAt, Int64 InternalId)
        {
            string FinalResponse = "";
            var auctionData = _context.AllegroAuction.Where(m => m.AuctionId == InternalId).Single();

            var product = _context.Products.Where(p => p.Id == auctionData.ProductId).Single();
            var manufacturer = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).Single();
            var usage = _context.AllegroAuctionUsage.Where(u => u.AuctionId == auctionData.AuctionId).ToList();
            var photos = _context.AllegroPhotos.Where(p => p.ProductId == product.Id).Single(); // pobieramy kategorie do zdjęć.

            string TitlePost = "";

            string folderPath = hostingEnv.WebRootPath + "/images/allegro/" + manufacturer.Tecdoc_id.ToString() + "/" + photos.CategoryId.ToString() + "";
            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
            List<string> fileLinks = new List<string>();


            foreach (FileInfo fileName in Files)
            {
                string pathToFile = pathToApp + "images/allegro/" + manufacturer.Tecdoc_id.ToString() + "/" + photos.CategoryId.ToString() + "/" + fileName.Name;
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
                var httpResponseB = (HttpWebResponse)httpWebRequestPhoto.GetResponse();
                using (var readStream = new StreamReader(httpResponseB.GetResponseStream(), Encoding.Default))
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

            string productId = "SPC-" + product.Id.ToString();

            string price = product.Gross_price.ToString();
            // tu będziemy pobierać dane dot. danego produktu do aukcji
            var auction = new AuctionToPost();
            auction.id = AllegroId;
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
                auction.FillListCompatible(_context.PassengerCars.Where(p => p.Ktype == car.PcId).Single().Ktype.ToString());
            }

            auction.sellingMode.format = "BUY_NOW";
            auction.sellingMode.price.amount = price;
            auction.sellingMode.price.currency = "PLN";
            auction.sellingMode.minimalPrice = null;
            auction.sellingMode.startingPrice = null;

            auction.stock.available = 1000;
            auction.stock.unit = "UNIT";

            auction.publication.duration = null;
            auction.publication.status = "INACTIVE";
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
            section.items.Add(new Item("TEXT", "<p>Zdjęcia zamieszczone w aukcji mają charakter poglądowy. W rzeczywistości, w zależności od modelu samochodu sprzęgła mogą się trochę różnić.</p>"));
            //section.items.Add(new Item("TEXT", "<h1>Nie jesteś pewien czy sprzęgło będzie pasowało do Twojego samochodu?</h1><h1>Zadzwoń lub napisz, chętnie pomożemy!</h1><h1>Nr tel. / e-mail znajdziesz poniżej w zakładce [-- O sprzedającym --]</h1>"));

            auction.description.sections.Add(section);

            string outprint = JsonConvert.SerializeObject(auction, Formatting.Indented);

            // ------

            List<string> Errors = new List<string>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offers/" + AllegroId + "");
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
            /*
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var readStream = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = readStream.ReadToEnd();
                FinalResponse = resource;
                dynamic x = JsonConvert.DeserializeObject(resource);
            }
            */
            Response.StatusCode = 200;

            return Json(outprint);
        }

        public string PhotoUpload(string Url)
        {
            string PhotoLink = "";

            string dataA = "{\"url\": \"" + Url + "\"}";

            var httpWebRequestPhotoA = (HttpWebRequest)WebRequest.Create("https://upload.allegro.pl/sale/images");
            httpWebRequestPhotoA.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequestPhotoA.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequestPhotoA.Method = "POST";
            httpWebRequestPhotoA.Headers.Add("Authorization", "Bearer " + Token + "");

            using (var streamWriter = new StreamWriter(httpWebRequestPhotoA.GetRequestStream()))
            {
                streamWriter.Write(dataA);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponseBA = (HttpWebResponse)httpWebRequestPhotoA.GetResponse();
            using (var readStream = new StreamReader(httpResponseBA.GetResponseStream(), Encoding.Default))
            {
                var resource = readStream.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var location = Convert.ToString(x.location);
                var expiresAt = x.expiresAt;
                PhotoLink = location;
            }
            return PhotoLink;

        }
        // Poniższa metoda do wrzucania aukcji 
        public async Task<JsonResult> PostAuctionTest(string id)
        {
            string FinalResponse = "";
            string ResponseId = "";
            string crosy = "";

            var auction_id = Convert.ToInt32(id);
            var auctionData = _context.AllegroAuction.Where(m => m.AuctionId == auction_id).SingleOrDefault();
            var auctionParams = _context.AllegroParams.Where(p => p.AuctionId == auction_id).SingleOrDefault();

            var product = _context.Products.Where(p => p.Id == auctionData.ProductId).SingleOrDefault();
            var additionalInfo = _context.AllegroAdditional.Where(a => a.ProductId == product.Id.ToString()).SingleOrDefault();
            var manufacturer = _context.Suppliers.Where(m => m.Tecdoc_id == product.Manufacturer_id).SingleOrDefault();
           
            var allegroManufacturer = _context.AllegroManufacturers.Where(m => m.ManufacturerId == manufacturer.Tecdoc_id).SingleOrDefault();
            var productPrice = _contextShop.Products_prices_sp24.Where(p => p.Id_product == product.Id).SingleOrDefault();
            var productDisplay = _contextShop.ProductDisplay.Where(p=>p.ProductId == product.Id).SingleOrDefault();

            var tagProduct = _contextShop.TagProduct.Where(t => t.ProductId == product.Id);
            var tag = _contextShop.Tag;
            var TagList = Enumerable.Empty<AllegroTag>().AsQueryable();

            if(tagProduct != null)
            {
                TagList = (from tp in tagProduct
                           join t in tag on tp.TagId equals t.TagId
                           join m in _context.Manufacturers on t.ManufacturerId equals m.Tecdoc_id
                           select new AllegroTag()
                           {
                               LangId = t.LangId,
                               TagId = t.TagId,
                               Manufacturer = m.Name,
                               ManufacturerId = m.Id,
                               Name = t.Name
                           });
                if(TagList != null)
                {
                    foreach (var singletag in TagList)
                    {
                        crosy += singletag.Manufacturer + " " + singletag.Name + ",";
                    }
                }
            }

            // obsługujemy specyfikacje produktu

            var FeatureList = Enumerable.Empty<AllegroFeatureValue>().AsQueryable();
            var feature = Enumerable.Empty<AllegroFeature>().AsQueryable();
            var featureValue = Enumerable.Empty<AllegroFeatureValue>().AsQueryable();
            var featureLang = Enumerable.Empty<AllegroFeatureLang>().AsQueryable();

            var F_set = "<h2>W ZESTAWIE</h2><ul>";
            var F_title = "";
            var F_radius = "";
            var F_disk = "";

            var photos = _context.AllegroPhotos.Where(p => p.ProductId == product.Id).SingleOrDefault(); // pobieramy kategorie do zdjęć.
            
            if (auctionParams.AllegroCategory == "50884" || auctionParams.AllegroCategory == "255983" || auctionParams.AllegroCategory == "255984")
            {
                feature = _contextShop.AllegroFeature.Where(f => f.ProductId == product.Id);
                featureValue = _contextShop.AllegroFeatureValue;
                featureLang = _contextShop.AllegroFeatureLang;
                FeatureList = (from features in feature
                               join featuresValue in featureValue on features.FeatureValueId equals featuresValue.FeatureValueId
                               where features.ProductId == product.Id
                               select new AllegroFeatureValue()
                               {
                                   FeatureValueId = featuresValue.FeatureValueId,
                                   LangId = featuresValue.LangId,
                                   Value = featuresValue.Value,
                                   FeatureId = features.FeatureId
                               });

                foreach (var result in FeatureList.Where(f => f.FeatureId == 5001))
                {
                    F_set += "<li>" + result.Value + "</li>";
                }
                F_set += "<li>Oryginalne opakowanie</li>";
                F_set += "<li>Paragon / Faktura Vat</li>";
                F_set += "</ul>";

                F_title = FeatureList.Where(f => f.FeatureId == 5000).SingleOrDefault().Value;
                F_radius = FeatureList.Where(f => f.FeatureId == 5002).SingleOrDefault().Value;
                F_disk = FeatureList.Where(f => f.FeatureId == 5003).SingleOrDefault().Value;
            }
            else 
            {
                if (auctionParams.AllegroCategory == "255985")
                {
                    F_set += "<li>Koło zamachowe</li>";
                    F_title = "Koło zamachowe";
                }     
                else
                {
                    F_set += "<li>Koło dwumasowe</li>";
                    F_title = "Koło dwumasowe";
                }

                if (photos.CategoryId % 2 == 0)
                {
                    F_set += "<li>Zestaw śrub</li>";
                    F_set += "<li>Oryginalne opakowanie</li>";
                    F_set += "<li>Paragon / Faktura Vat</li>";
                    F_set += "</ul>";
                }
                else
                {
                    F_set += "<li>Oryginalne opakowanie</li>";
                    F_set += "<li>Paragon / Faktura Vat</li>";
                    F_set += "</ul>";
                }
            }
            

           
            // obsługujemy specyfikacje produktu

            var TermsList = Enumerable.Empty<AllegroTermsOfUse>().AsQueryable();
            var Terms = _contextShop.AllegroTerms.Where(t => t.ProductId == auctionData.ProductId);

            var usage = _context.AllegroAuctionUsage.Where(u => u.AuctionId == auctionData.AuctionId);
            var usageDesc = _context.AllegroUsage;
            var UsageList = Enumerable.Empty<AllegroAuctionUsageDescription>().AsQueryable();
            var UsageDescription = "<h1>Zastosowanie - informacje uzupełniające</h1><h2>W uwagach do zamówienia podaj dane Twojego samochodu i nr VIN - Sprawdzimy czy zamówione części na 100% będą pasowały.</h2><ul>";
            UsageList = (from u in usage
                         join ud in usageDesc on u.PcId equals ud.PcId
                         where u.ProductId == product.Id.ToString()
                         select new AllegroAuctionUsageDescription()
                         {
                             Description_desc = ud.Description_desc,
                             Description_list = ud.Description_list,
                             Id = ud.Id,
                             PcId = ud.PcId
                         });
            foreach (var r in UsageList)
            {
                var ktype = r.PcId + 500000;
                TermsList = (from t in Terms
                             join f in featureLang on t.FeatureId equals f.FeatureId
                             where t.CategoryId == ktype
                             select new AllegroTermsOfUse()
                             {
                                 CategoryId = t.CategoryId,
                                 FeatureId = t.FeatureId,
                                 LangId = t.LangId,
                                 ProductId = t.ProductId,
                                 Value = t.Value,
                                 Name = f.Value
                             });

                UsageDescription += "<li><b>" + r.Description_desc + "</b> ";
                foreach (var singleterm in TermsList)
                {
                    UsageDescription += singleterm.Name + " <b>" + singleterm.Value + "</b>, ";
                }
                UsageDescription += "</li>";
            }
            UsageDescription += "</ul>";

            // dodatkowe informacje do proudktu
            //



            string TitlePost = "";

            string folderPath = hostingEnv.WebRootPath + "/images/allegro/" + manufacturer.Tecdoc_id.ToString() + "/" + photos.CategoryId.ToString() + "";
            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files

            Array.Sort(Files, (f1, f2) => f1.Name.CompareTo(f2.Name)); // sortowanie plikow

            List<string> fileLinks = new List<string>();
            string ManufacturerPhotoLink = "";
            string ManufacturerCertLink = "";
            string ManufacturerLogo = "";
            int PhotoNumber = 0;
            foreach (FileInfo fileName in Files)
            {
                PhotoNumber++;
                string pathToFile = pathToApp + "images/allegro/" + manufacturer.Tecdoc_id.ToString() + "/" + photos.CategoryId.ToString() + "/" + fileName.Name;
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
                var httpResponseB = (HttpWebResponse)httpWebRequestPhoto.GetResponse();
                using (var readStream = new StreamReader(httpResponseB.GetResponseStream(), Encoding.Default))
                {
                    var resource = readStream.ReadToEnd();
                    dynamic x = JsonConvert.DeserializeObject(resource);
                    var location = Convert.ToString(x.location);
                    var expiresAt = x.expiresAt;
                    fileLinks.Add(location);
                }

            }
            string pathToPhoto = pathToApp + "images/allegro/" + manufacturer.Tecdoc_id.ToString() + ".png";
            string pathToPhotoC = pathToApp + "images/allegro/" + manufacturer.Tecdoc_id.ToString() + "c.jpg";
            string pathToLogo = pathToApp + "images/allegro/" + manufacturer.Tecdoc_id.ToString() + "l.png";
            ManufacturerPhotoLink = this.PhotoUpload(pathToPhoto);
            ManufacturerCertLink = this.PhotoUpload(pathToPhotoC);
            ManufacturerLogo = this.PhotoUpload(pathToLogo);
            // przesyłanie zdjęcia

            //przesyłanie zdjęcia
            // Generowanie tytułu
            if (additionalInfo.SecondTitle != "")
            {
                if ((additionalInfo.FirstTitle + " " + manufacturer.Description + " " + auctionData.AuctionTitle + " " + additionalInfo.SecondTitle).Length <= 50)
                    TitlePost = additionalInfo.FirstTitle + " " + manufacturer.Description + " " + auctionData.AuctionTitle + " " + additionalInfo.SecondTitle;
                else
                    TitlePost = additionalInfo.FirstTitle + " " + auctionData.AuctionTitle + " " + additionalInfo.SecondTitle;
            }
            else
            {
                if ((additionalInfo.FirstTitle + " " + manufacturer.Description + " " + auctionData.AuctionTitle).Length <= 50)
                    TitlePost = additionalInfo.FirstTitle + " " + manufacturer.Description + " " + auctionData.AuctionTitle;
                else
                    TitlePost = additionalInfo.FirstTitle + " " + auctionData.AuctionTitle;
            }
            //Generowanie tytułu

            string productId = "SP-" + product.Id.ToString();
            string price = Math.Round(decimal.ToDouble(productPrice.Price) * 1.23, 0).ToString();
            // tu będziemy pobierać dane dot. danego produktu do aukcji
            var auction = new AuctionToPost();
            //auction.id = AllegroId;
            auction.name = TitlePost.ToUpper();
            auction.category.id = auctionParams.AllegroCategory;

            auction.parameters.Add(new Parameters("11323", new string[] { }, new string[] { auctionParams.AllegroStatus })); // nowa / uzywana
            auction.parameters.Add(new Parameters("215858", new string[] { productDisplay.Reference }, new string[] { }));
            auction.parameters.Add(new Parameters("127417", new string[] { }, new string[] { allegroManufacturer.AllegroManufacturerId }));
            auction.parameters.Add(new Parameters("215941", new string[] { crosy }, new string[] { }));
            
            if (auctionParams.AllegroType.Replace(" ", "") == "Dostawcze")
                auction.parameters.Add(new Parameters("129591", new string[] { }, new string[] { "129591_2" }));
            else if (auctionParams.AllegroType.Replace(" ", "") == "Osobowe")
                auction.parameters.Add(new Parameters("129591", new string[] { }, new string[] { "129591_1" }));
            else
                auction.parameters.Add(new Parameters("129591", new string[] { }, new string[] { "129591_1", "129591_2" }));

            auction.parameters.Add(new Parameters("214434", new string[] { }, new string[] { auctionParams.AllegroQuality })); // jakosc czesci
            auction.parameters.Add(new Parameters("130531", new string[] { }, new string[] { auctionParams.AllegroEngine }));  // diesel / benzyna

            if (additionalInfo.Ean != null)
                auction.ean = additionalInfo.Ean;
            else
                auction.ean = null;

            // PHOTOS
            foreach (string link in fileLinks)
            {
                auction.images.Add(new Images(link));
            }
            auction.images.Add(new Images(ManufacturerPhotoLink));
            auction.images.Add(new Images(ManufacturerCertLink));
            auction.images.Add(new Images(ManufacturerLogo));

            //  auction.FillListCompatible("BMW 3 (E46) 330 i 231 KM / 170 kW 2979 ccm");
            // auction.FillListCompatible("BMW 2 (E46) 330 i 231 KM / 170 kW 2979 ccm");

            auction.sellingMode.format = "BUY_NOW";
            if (price == "0")
                auction.sellingMode.price.amount = "9999";
            else
                auction.sellingMode.price.amount = price;

            auction.sellingMode.price.currency = "PLN";
            auction.sellingMode.minimalPrice = null;
            auction.sellingMode.startingPrice = null;

            auction.stock.available = 1000;
            auction.stock.unit = "UNIT";

            auction.publication.duration = null;
            auction.publication.status = "INACTIVE";
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
            auction.location.postCode = "02-180";

            auction.external.id = productId;
            auction.contact = null;

            auction.validation.validatedAt = null;
            auction.createdAt = null;
            auction.updatedAt = null;

            var headerSection = new Section();
            headerSection.items.Add(new Item("IMAGE", null, ManufacturerPhotoLink));
            auction.description.sections.Add(headerSection);

            var section = new Section();
            section.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(0)));
            if (auctionParams.AllegroCategory == "50884" || auctionParams.AllegroCategory == "255983" || auctionParams.AllegroCategory == "255984")
            {
                section.items.Add(new Item("TEXT", "<h1>" + F_title + " do " + auctionData.AuctionTitle + "</h1><h2>SPECYFIKACJA</h2><ul><li>Producent: <b>" + manufacturer.Description + "</b></li><li>Nr katalogowy: <b>" + product.Reference.Replace(" ", "") + "</b></li><li>Średnica tarczy: <b>" + F_radius + " mm</b></li><li>Ilość zębów: <b>" + F_disk + "</b></li><li>Gwarancja producenta: <b>2 lata</b></li><li>Stan: <b>fabrycznie nowe części</b></li></ul>" + F_set + "", null));
            }
            else
            {
                section.items.Add(new Item("TEXT", "<h1>" + F_title + " do " + auctionData.AuctionTitle + "</h1><h2>SPECYFIKACJA</h2><ul><li>Producent: <b>" + manufacturer.Description + "</b></li><li>Nr katalogowy: <b>" + product.Reference.Replace(" ", "") + "</b></li><li>Gwarancja producenta: <b>2 lata</b></li><li>Stan: <b>fabrycznie nowe części</b></li></ul>" + F_set + "", null));
            }
            auction.description.sections.Add(section);

            var usageSection = new Section();
            usageSection.items.Add(new Item("TEXT", UsageDescription));
            auction.description.sections.Add(usageSection);

            var manuSection = new Section();
            manuSection.items.Add(new Item("IMAGE", null, ManufacturerCertLink));
            manuSection.items.Add(new Item("TEXT", "<h1>" + manufacturer.Description + " w Sprzegla24</h1>" + allegroManufacturer.AllegroDescription + "", null));
            auction.description.sections.Add(manuSection);

            if(PhotoNumber ==2)
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                auction.description.sections.Add(photoSection_1);
            }
            else if (PhotoNumber == 3) // mamy 2 zdjecia szczegolowe, nie liczymy glownego zdjecia
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(2)));
                auction.description.sections.Add(photoSection_1);
            }
            else if (PhotoNumber == 4) // mamy 3 zdjecia szczegolowe, nie liczymy glownego zdjecia
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(2)));
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(3)));
                auction.description.sections.Add(photoSection_1);

                var photoSection_2 = new Section();
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                auction.description.sections.Add(photoSection_2);
            }
            else if (PhotoNumber == 5) // mamy 4 zdjecia szczegolowe, nie liczymy glownego zdjecia
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(2)));
                auction.description.sections.Add(photoSection_1);

                var photoSection_2 = new Section();
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(3)));
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(4)));
                auction.description.sections.Add(photoSection_2);
            }
            else if (PhotoNumber == 6)
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(2)));
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(3)));
                auction.description.sections.Add(photoSection_1);

                var photoSection_2 = new Section();
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(4)));
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(5)));
                auction.description.sections.Add(photoSection_2);

                var photoSection_3 = new Section();
                photoSection_3.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                auction.description.sections.Add(photoSection_3);
            }
            else if(PhotoNumber == 7)
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(2)));
                auction.description.sections.Add(photoSection_1);

                var photoSection_2 = new Section();
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(3)));
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(4)));
                auction.description.sections.Add(photoSection_2);

                var photoSection_3 = new Section();
                photoSection_3.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(5)));
                photoSection_3.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(6)));
                auction.description.sections.Add(photoSection_3);
            }
            else if(PhotoNumber == 8)
            {
                var photoSection_1 = new Section();
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(2)));
                photoSection_1.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(3)));
                auction.description.sections.Add(photoSection_1);

                var photoSection_2 = new Section();
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(4)));
                photoSection_2.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(5)));
                auction.description.sections.Add(photoSection_2);

                var photoSection_3 = new Section();
                photoSection_3.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(6)));
                photoSection_3.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(7)));
                auction.description.sections.Add(photoSection_3);

                var photoSection_4 = new Section();
                photoSection_4.items.Add(new Item("IMAGE", null, fileLinks.ElementAt(1)));
                auction.description.sections.Add(photoSection_4);
            }

            


            var footerSection = new Section();
            footerSection.items.Add(new Item("TEXT", "<p>Zdjęcia zamieszczone w aukcji mają charakter poglądowy. W rzeczywistości, w zależności od modelu samochodu sprzęgła mogą się trochę różnić.</p><h1>Nie jesteś pewien czy sprzęgło będzie pasowało do Twojego samochodu?</h1><h1>Zadzwoń lub napisz, chętnie pomożemy!</h1><h1>Nr tel. / e-mail znajdziesz poniżej w zakładce [-- O sprzedającym --]</h1>", null));
            auction.description.sections.Add(footerSection);

            string outprint = JsonConvert.SerializeObject(auction, Formatting.Indented);

            // ------
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.allegro.pl");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.allegro.public.v1+json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token + "");
                var result = await client.PostAsync("/sale/offers", new StringContent(outprint, Encoding.UTF8, "application/vnd.allegro.public.v1+json"));
                string resultContent = await result.Content.ReadAsStringAsync();
                FinalResponse = resultContent;
                dynamic x = JsonConvert.DeserializeObject(resultContent);
                ResponseId = x.id;
            }

            auctionData.AllegroId = ResponseId; // aktualizujemy id
            _context.SaveChanges(); // aktualizujemy id 

            return new JsonResult(FinalResponse);
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

            var result = PostAuction(OfferResponse.ElementAt(0), Title, Category, OfferResponse.ElementAt(2), OfferResponse.ElementAt(3), OfferResponse.ElementAt(4), auction_id); // wystawiamy aukcję z draft'a
            // pośrednie błędy poniżej
            // return Json(errors_response + " \n " + OfferResponse.First() + result);

            return Json(result);
        }
    }
}