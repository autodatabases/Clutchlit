using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_tag")]
    public class AllegroTag
    {
        [Key]
        [Column("id_tag")]
        public int TagId { get; set; }
        [Column("id_lang")]
        public int LangId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("id_manufacturer")]
        public int ManufacturerId { get; set; }
        [NotMapped]
        public string Manufacturer { get; set; }
    }
}
