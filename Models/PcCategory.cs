using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_products_categories_relation")]
    public class PcCategory
    {
        [Key]
        [Column("pc_id")]
        public Int32 Id { get; set; }
        [Column("pc_product_category_name")]
        public string CategoryName { get; set; }
        [Column("pc_product_category_id")]
        public Int32 CategoryId { get; set; }
        [Column("pc_product_id")]
        public Int32 ProductId { get; set; }
    }
}
