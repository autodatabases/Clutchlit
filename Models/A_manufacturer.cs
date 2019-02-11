using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_manufacturer")]
    public class A_manufacturer
    {
        [Key]
        [Column("id_manufacturer")]
        public int ManuId { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
