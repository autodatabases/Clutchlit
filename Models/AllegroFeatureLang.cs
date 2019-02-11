using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_feature_lang")]
    public class AllegroFeatureLang
    {
        [Key]
        [Column("id_feature")]
        public int FeatureId { get; set; }
        [Column("id_lang")]
        public int LangId { get; set; }
        [Column("name")]
        public string Value { get; set; }
    }
}
