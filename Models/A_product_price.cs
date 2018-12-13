using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_product_shop")]
    public class A_product_price
    {
        [Key]
        [Column("id_product")]
        public int Id_product { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
    }
}
