using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
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

        HttpClient client = new HttpClient();

        public List<Auction> list = new List<Auction>();
        
        public IActionResult Index()
        {
            return View();
        }

        public AllegroAuctionsController(IHostingEnvironment env)
        {
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
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<string>>();
            }
            return new List<string>();
        }
        
        public string GetToken(string token)
        {
            string response = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://allegro.pl/auth/oauth/token?grant_type=authorization_code&code="+token+"&redirect_uri=http://clutchlit.trimfit.pl/AllegroAuctions/GetList/api/");
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
            httpWebRequest.Headers.Add("Authorization", "Bearer "+token+"");

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resource = streamReader.ReadToEnd();
                dynamic x = JsonConvert.DeserializeObject(resource);
                var offers = x.offers;
                foreach(var offer in offers)
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
  "\"offerCriteria\": ["+
    "{"+
      "\"offers\": ["+
        "{"+
          "\"id\": \""+Id+"\""+
        "}"+
      "],"+
      "\"type\": \"CONTAINS_OFFERS\""+
    "}"+
  "],"+
  "\"publication\": {"+
    "\"action\": \"END\""+
    "}"+
    "}";
           
            string response = "Coś poszło nie tak. Skontaktuj się z pokojem obok.";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offer-publication-commands/"+uuid+"");
            httpWebRequest.ContentType = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Accept = "application/vnd.allegro.public.v1+json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", "Bearer "+Token+"");

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
            var httpWebRequest2 = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/offer-publication-commands/"+uuid+"");
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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/shipping-rates?seller.id=" + sellerId+"");
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
                    categories.Add(new AllegroCategory { Id = method.id, Name = method.name});
                }
            }
            return categories;

        }
        public IActionResult GetChildCategories(string parent_id)
        {
            List<AllegroCategory> categories = new List<AllegroCategory>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/categories?parent.id="+parent_id+"");
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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.allegro.pl/sale/categories/"+catId+"/parameters");
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
                    if(method.type == "dictionary" && method.restrictions.multipleChoices == false )
                    { // simple select form
                        temp += "<label for='"+method.id+"'><strong>"+method.name+"</strong></label>";
                        temp += "<select class='dictionary form-control' name='" + method.id+"'>";
                        var variants = method.dictionary;
                        foreach(var variant in variants)
                        {
                            temp += "<option value='"+variant.id+"'>"+variant.value+"</option>";
                        }
                        temp += "</select>";
                    }
                    else if(method.type == "dictionary" && method.restrictions.multipleChoices == true)
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
                    else if(method.type == "string" || method.type == "float" || method.type == "integer")
                    { // input

                        temp += "<label for='" + method.id + "'><strong>" + method.name + "</strong></label>";
                        temp += "<input type='text' class='form-control' value='' name='"+method.id+"' />";
                    }
                    categories += temp;
                }
            }

            Response.StatusCode = 200;
            return new JsonResult(categories);
        }
        // pobieranie parametrów dla wybranej kategorii
        // przesyłanie plików zdjęć na serwer allegro
        public IActionResult UploadPhotos()
        {
            long size = 0;
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = hostingEnv.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            return Json(message);
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
    }
}