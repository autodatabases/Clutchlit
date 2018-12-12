using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_orders")]
    public class A_orders
    {
        [Key]
        [Column("id_order")]
        public int Id_order { get; set; }

        [Column("reference")]
        public string Reference { get; set; }

        [Column("id_carrier")]
        public int Id_carrier { get; set; }

        [Column("id_cart")]
        public int Id_cart { get; set; }

        [Column("id_address_delivery")]
        public int Id_address_d { get; set; }

        [Column("id_address_invoice")]
        public int Id_address_i { get; set; }

        [Column("current_state")]
        public int Current_state { get; set; }

        [Column("payment")]
        public string Payment { get; set; }

        [Column("total_paid")]
        public decimal Total_paid { get; set; }

        [Column("total_products_wt")]
        public decimal Total_paid_products { get; set; }

        [Column("total_shipping")]
        public decimal Total_shipping { get; set; }

        [Column("date_add")]
        public DateTime Created { get; set; }

        [Column("id_customer")]
        public int Id_customer { get; set; }
    }
}
