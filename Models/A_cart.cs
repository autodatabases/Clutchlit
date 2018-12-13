using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_cart_product")]
    public class A_cart
    {
        [Key]
        [Column("id_cart")]
        public int Id_cart { get; set; }
        [Column("id_product")]
        public int Id_product { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
