using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_allegro_auctions_additional")]
    public class AllegroAdditionalInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("product_id")]
        public string ProductId { get; set; }
        [Column("title_id")]
        public int TitleId { get; set; }
        [Column("first_title")]
        public string FirstTitle { get; set; }
        [Column("second_title")]
        public string SecondTitle { get; set; }
        [Column("ean_number")]
        public string Ean { get; set; }
    
    }
}
