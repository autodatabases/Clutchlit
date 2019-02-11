using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_TermOfUseFromTecDoc")]
    public class AllegroTermsOfUse
    {
        [Key]
        [Column("id_category")]
        public int CategoryId { get; set; }
        [Column("id_product")]
        public int ProductId { get; set; }
        [Column("id_lang")]
        public int LangId { get; set; }
        [Column("id_feature")]
        public int FeatureId { get; set; }
        [Column("value")]
        public string Value { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }
}
