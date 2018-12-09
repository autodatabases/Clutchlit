using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_distributors_warehouses")]
    public class DistributorWarehouse
    {
        [Key]
        [Column("dw_warehouse_id")]
        public Int32 Id { get; set; }
        [Column("dw_warehouse_name")]
        public string Name { get; set; }
        [Column("dw_distributor_id")]
        public Int32 DistributorId { get; set; }
        [Column("dw_warehouse_number")]
        public Int32 WarehouseNumber { get; set;  }
    }
}
