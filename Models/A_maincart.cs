using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_cart")]
    public class A_cart_main
    {
        [Key]
        [Column("id_cart")]
        public int Id_cart { get; set; }
    }
}
