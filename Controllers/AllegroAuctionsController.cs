using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Clutchlit.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clutchlit.Controllers
{
    public class AllegroAuctionsController : Controller
    {
        public static string Token = ""; 
        HttpClient client = new HttpClient();
        public List<Auction> list = new List<Auction>();
        
        public IActionResult Index()
        {
            return View();
        }
        public AllegroAuctionsController()
        {
            client.BaseAddress = new Uri("https://api.allegro.pl/");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
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
                        response = property.Value.ToString();
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
        
        public string EndOffer(string id)
        {
            string data = "{" +
  "\"offerCriteria\": ["+
    "{"+
      "\"offers\": ["+
        "{"+
          "\"id\": \""+id+"\""+
        "}"+
      "],"+
      "\"type\": \"CONTAINS_OFFERS\""+
    "}"+
  "],"+
  "\"publication\": {"+
    "\"action\": \"END\""+
    "}"+
    "}";
            
           
            return data; 
        }
        [HttpGet("[controller]/[action]/{id}/")]
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
    }
}