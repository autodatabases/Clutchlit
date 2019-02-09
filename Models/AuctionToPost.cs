using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    public class JustId
    {
        public string id { get; set; }
        public JustId()
        {
        }
        public JustId(string str)
        {
            this.id = str;
        }
    }
    public class JustInvoice
    {
        public string invoice { get; set; }
        public JustInvoice()
        {
        }
        public JustInvoice(string str)
        {
            this.invoice = str;
        }
    }
    public class Item
    {
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string content { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url {get;set;}
        public Item() { }
        public Item(string Type, string Content = null, string Url = null)
        {
            this.type = Type;
            this.content = Content;
            this.url = Url;
        }
    }

    public class Section
    {
        public List<Item> items = new List<Item>();

        public Section() { items.Clear(); }
    }
    public class Description
    {
        public List<Section> sections = new List<Section>();

        public Description() { sections.Clear(); }
    }
    public class AuctionToPost
    {
        
        public string additionalServices { get; set; }
        public AfterSalesServices afterSalesServices = new AfterSalesServices();
        public JustId category = new JustId();
        
        public string contact { get; set; }
        public string createdAt { get; set; }
        public Delivery delivery = new Delivery();
        public string ean { get; set; }
        public JustId external = new JustId();
        public string id { get; set; }
        public List<Images> images = new List<Images>();
        public Description description = new Description();
        public Location location = new Location();
        public string name { get; set; }
        public List<Parameters> parameters = new List<Parameters>();
        public JustInvoice payments = new JustInvoice();
        public Promotion promotion = new Promotion();
        public Publication publication = new Publication();
        public SellingMode sellingMode = new SellingMode();
        public CompatibleList compatibilityList = new CompatibleList();
        public string sizeTable { get; set; }
        public Stock stock = new Stock();
        public string updatedAt { get; set; }
        public Validation validation = new Validation();

        public void FillListCompatible(string car)
        {
            var Car = new Text(car);
            this.compatibilityList.items.Add(Car);
        }
    }
    public class Validation
    {
        public string validatedAt { get; set; }
        public List<ValidationError> errors = new List<ValidationError>();

        public Validation()
        {
            this.errors.Clear();
        }
    }
    public class ValidationError
    {
        public string code { get; set; }
        public string details { get; set; }
        public string message { get; set; }
        public string path { get; set; }
        public string userMessage { get; set; }
    }

    public class Stock
    {
        public Int32 available { get; set; }
        public string unit { get; set; }
    }

    public class SellingMode
    {
        public string format { get; set; }
        public Price minimalPrice { get; set; }
        public Price price = new Price();
        public Price startingPrice { get; set; }
    }
    public class Price
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }
    public class Publication
    {
        public string duration { get; set; }
        public string endingAt { get; set; }
        public string startingAt { get; set; }
        public string status { get; set; }
    }
    public class Promotion
    {
        public Boolean bold { get; set; }
        public Boolean departmentPage { get; set; }
        public Boolean emphasized { get; set; }
        public Boolean emphasizedHighlightBoldPackage { get; set; }
        public Boolean highlight { get; set; }
    }
    public class Parameters
    {
        public string id { get; set; }
        public string[] values { get; set; }
        public string[] valuesIds { get; set; }
        public ParameterRangeValue rangeValue { get; set; }

        public Parameters(string Id, string[] Values, string[] ValuesIds)
        {
            this.id = Id;
            this.values = Values;
            this.valuesIds = ValuesIds;
            this.rangeValue = null;
        }
    }
    public class ParameterRangeValue
    {
        public string from { get; set; }
        public string to { get; set; }
    }
    public class AfterSalesServices
    {
        public JustId impliedWarranty = new JustId();
        public JustId returnPolicy = new JustId();
        public JustId warranty = new JustId();
    }
    public class Images
    {
        public string url { get; set; }
        public Images(string Url)
        {
            url = Url;
        }
    }
    public class Text
    {
        public string text { get; set; }

        public Text(string textToInsert)
        {
            this.text = textToInsert;
        }
    }
    public class CompatibleList
    {
        public List<Text> items = new List<Text>();
    }

    public class Delivery
    {
        public string additionalInfo { get; set; }
        public string handlingTime { get; set; }
        public string shipmentDate { get; set; }
        public JustId shippingRates = new JustId();
    }
    public class Location
    {
        public string city { get; set; }
        public string countryCode { get; set; }
        public string postCode { get; set; }
        public string province { get; set; }
    }
}
