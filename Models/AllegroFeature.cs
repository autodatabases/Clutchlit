using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_feature_product")]
    public class AllegroFeature
    {
        [Key]
        [Column("id_feature")]
        public int FeatureId { get; set; }
        [Column("id_product")]
        public int ProductId { get; set; }
        [Column("id_feature_value")]
        public int FeatureValueId { get; set; }
    }
}
