using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_passanger_cars")]
    public class PassengerCar
    {
        [Key]
        [Column("pc_id")]
        public Int32 Id { get; set; }
        [Column("pc_constructioninterval")]
        public string Constructioninterval { get; set; }
        [Column("pc_description")]
        public string Description { get; set; }
        [Column("pc_fulldescription")]
        public string Fulldescription { get; set; }
        [Column("pc_modelid")]
        public Int32 Modelid { get; set; }
        [Column("pc_ktype")]
        public Int32 Ktype { get; set; }
        public string SelectDesc { get { return this.Description + "[ " + this.Constructioninterval + " ]"; }  }

    }
}
