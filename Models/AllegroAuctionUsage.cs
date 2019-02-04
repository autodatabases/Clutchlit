using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auction_usage")]
    public class AllegroAuctionUsage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("auction_id")]
        public int AuctionId { get; set; }
        [Column("auction_name")]
        public string AuctionTitle { get; set; }
        [Column("passanger_car_id")]
        public int PcId { get; set; }
        [Column("temp_product")]
        public string ProductId { get; set; }
    }
}
