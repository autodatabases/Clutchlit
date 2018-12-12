using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{

    public class B_orders_display
    {

        public int Id_order { get; set; }
        public string Reference { get; set; }
        public int Id_carrier { get; set; }
        public int Id_cart { get; set; }
        public string Id_address_d { get; set; }
        public string Id_address_i { get; set; }
        public string Current_state { get; set; }
        public string Payment { get; set; }
        public decimal Total_paid { get; set; }
        public decimal Total_paid_products { get; set; }
        public decimal Total_shipping { get; set; }
        public DateTime Created { get; set; }
        public string Id_customer { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
