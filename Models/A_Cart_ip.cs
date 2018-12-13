using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_oav_order_ip_log")]
    public class A_Cart_ip
    {
        [Key]
        [Column("id_cart")]
        public int Id_cart { get; set; }
        [Column("ip")]
        public string Ip_address { get; set; }
        [NotMapped]
        public string Additional { get; set; }
    }
}
