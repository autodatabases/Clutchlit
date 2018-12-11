using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_passanger_cars_attributes")]
    public class PcAttributes
    {
        [Key]
        [Column("pca_id")]
        public int Id { get; set; }
        [Column("pca_pc_id")]
        public int Pc_id { get; set; }
        [Column("pca_description")]
        public string Description { get; set; }
    }
}
