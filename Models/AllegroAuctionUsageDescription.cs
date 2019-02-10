using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auction_usage_description")]
    public class AllegroAuctionUsageDescription
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("pc_id")]
        public int PcId { get; set; }
        [Column("description_desc")]
        public string Description_desc { get; set; }
        [Column("description_list")]
        public string Description_list { get; set; }
    }
}
