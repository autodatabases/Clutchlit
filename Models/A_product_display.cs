using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_product")]
    public class A_product_display
    {
        [Key]
        [Column("id_product")]
        public int ProductId { get; set; }
        [Column("reference")]
        public string Reference { get; set; }
    }
}
