using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_feature_value_lang")]
    public class AllegroFeatureValue
    {
        [Key]
        [Column("id_feature_value")]
        public int FeatureValueId { get; set; }
        [Column("id_lang")]
        public int LangId { get; set; }
        [Column("value")]
        public string Value { get; set; }
        [NotMapped]
        public int FeatureId { get; set; }
    }
}
