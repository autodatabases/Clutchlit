using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_product_lang")]
    public class A_product_lang
    {
        [Key]
        [Column("id_product")]
        public int ProductId { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
