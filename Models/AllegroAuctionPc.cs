using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_passanger_cars")]
    public class AllegroAuctionPc
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("passanger_car_id")]
        public int PcId { get; set; }
        [Column("passanger_car_active")]
        public Boolean Status { get; set; }
    }
}
