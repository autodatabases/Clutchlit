using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_products_distributors_prices")]
    public class PdPrices
    {
        [Key]
        [Column("pd_id")]
        public int Id { get; set; }
        [Column("pd_product_id")]
        public int ProductId { get; set; }
        [Column("pd_product_net_price")]
        public Double NetPrice { get; set; }
        [Column("pd_product_gross_price")]
        public Double GrossPrice { get; set; }
        [Column("pd_product_quantity")]
        public int Quantity { get; set; }
        [Column("pd_distributor_id")]
        public int DistributorId { get; set; }
        [Column("pd_distributor_warehouse_id")]
        public int DistributorWarehouseId { get; set; }
        [Column("pd_update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
