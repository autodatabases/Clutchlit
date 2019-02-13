using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_stock_available")]
    public class A_stock
    {
        [Key]
        [Column("id_stock_available")]
        public int Id { get; set; }
        [Column("id_product")]
        public int ProductId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
