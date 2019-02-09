using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    public class AllegroCategory
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public string ParentId { get; set; }
        public string Leaf { get; set; }
        public string Advertisement { get; set; }
        public string AdvertisementPriceOptional { get; set; } 
    }
}
