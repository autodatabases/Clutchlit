using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_products_ktypes_relation")]
    public class PcKtype
    {
        [Column("pk_id")]
        public Int32 Id { get; set; }
        [Column("pk_product_id")]
        public Int32 Product_id { get; set; }
        [Column("pk_ktype")]
        public Int32 Ktype { get; set; }
    }
}
