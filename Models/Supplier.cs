using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_suppliers")]
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        public int Id { get; set; }
        [Column("supplier_tecdoc_id")]
        public int Tecdoc_id { get; set; }
        [Column("supplier_description")]
        public string Description { get; set; }
    }
}
