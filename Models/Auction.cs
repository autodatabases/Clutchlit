using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    public class Auction
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string PrimaryImage { get; set; }
        public string Price { get; set; }
        public string Watchers { get; set; }
        public string Visits { get; set; }
        public string Status { get; set; }
        /*
        public string[] Category { get; set; }
        public string[] PrimaryImage { get; set; }
        public string[] SellingMode { get; set; }
        public string[] SaleInfo { get; set; }
        public string[] Stats { get; set; }
        public string[] Stock { get; set; }
        public string[] Publication { get; set; }
        public string[] AfterSalesServices { get; set; }
        public string[] AdditionalServices { get; set; }
        */

    }
}
