using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auctions_manufacturers")]
    public class AllegroManufacturers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("manufacturer_id")]
        public int ManufacturerId { get; set; }
        [Column("allegro_manufacturer_id")]
        public string AllegroManufacturerId { get; set; }
        [Column("manufacturer_description")]
        public string AllegroDescription { get; set; }
    }
}
