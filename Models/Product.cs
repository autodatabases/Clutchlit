using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_products")]
    public class Product
    {
        //[Key]
        [Column("product_id")]
        public Int32 Id { get; set; }
        [Column("product_name")]
        public string Name { get; set; }
        [Column("product_reference")]
        public string Reference { get; set; }
        [Column("product_gross_price")]
        public double Gross_price { get; set; }
        [Column("product_net_price")]
        public double Net_price { get; set; }
        [Column("product_status")]
        public Boolean Status { get; set; }
        [Column("product_manufacturer_id")]
        public Int32 Manufacturer_id { get; set; }
        [Column("product_lowest_price")]
        public double LowestPrice { get; set; }
        [Column("product_lowest_price_distributor_id")]
        public Int32 DistributorId { get; set; }
        public double Markup { get  { return Math.Round(((Gross_price - LowestPrice) / Gross_price)*100,2);  } }
    }
}
