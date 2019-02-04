using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auctions")]
    public class AllegroAuction
    {
        [Key]
        [Column("auction_id")]
        public string AuctionId { get; set; }
        [Column("product_id")]
        public Int32 ProductId { get; set; }
        [Column("auction_category")]
        public string Category { get; set; }
        [Column("auction_name")] // title used as an auction title;
        public string AuctionTitle { get; set; }
        [Column("auction_model_id")]
        public int ModelId { get; set; }
        [Column("auction_allegro_id")] 
        public string AllegroId { get; set; }
        [Column("auction_allegro_status")]
        public Boolean Status { get; set; }

    }
}
