using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_product_tag")]
    public class AllegroTagProduct
    {
        [Key]
        [Column("id_product")]
        public int ProductId { get; set; }
        [Column("id_tag")]
        public int TagId { get; set; }
    }
}
