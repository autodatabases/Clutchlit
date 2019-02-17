using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auctions_rescue_title")]
    public class AllegroTitle
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("auction_id")]
        public string AuctionId { get; set; }
        [Column("auction_title")]
        public string AuctionTitle { get; set; }
    }
}
