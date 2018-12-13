using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_product_lang")]
    public class A_products
    {
        [Key]
        [Column("id_product")]
        public int Id_product { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [NotMapped]
        public int Cart_Quantity { get; set; }
        [NotMapped]
        public decimal Cart_price { get; set; }
    }
}
