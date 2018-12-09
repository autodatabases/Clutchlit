using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_models")]
    public class Model
    {
        [Column("model_id")]
        public Int32 Id { get; set; }
        [Column("model_modelid")]
        public Int32 Tecdoc_id { get; set; }
        [Column("model_description")]
        public string Description { get; set; }
        [Column("model_full_description")]
        public string FullDescription { get; set; }
        [Column("model_constructioninterval")]
        public string ConstructionInterval { get; set; }
        public string SelectDesc { get { return this.Description + "[ " + this.ConstructionInterval + " ]"; } }
        [Column("model_manufacturer_id")]
        public Int32 Manufacturer_id { get; set; }
        
     
    }
}
