using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    public class PrestashopProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Photo { get; set; }
        public decimal GrossPrice { get; set; }
        public int Status { get; set; }
    }
}
