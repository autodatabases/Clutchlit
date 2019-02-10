using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auctions_parameters")]
    public class AllegroAuctionParams
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("auction_id")]
        public int AuctionId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("allegro_category")]
        public string AllegroCategory { get; set; }
        [Column("product_reference")]
        public string ProductReference { get; set; }
        [Column("auction_status")]
        public string AllegroStatus { get; set; } // czy czesc nowa czy uzywana
        [Column("auction_type")]
        public string AllegroType { get; set; } // do jakich aut pasuje
        [Column("auction_quality")]
        public string AllegroQuality { get; set; }
        [Column("auction_engine")]
        public string AllegroEngine { get; set; } // diesel czy beznzyna
        [Column("auction_weight")]
        public string AllegroWeight { get; set; }

    }
}
