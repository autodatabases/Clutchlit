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
        public Boolean Leaf { get; set; }
        public Boolean Advertisement { get; set; }
        public Boolean AdvertisementPriceOptional { get; set; }
    }
}
