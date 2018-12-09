using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_distributors")]
    public class Distributor
    {
        [Column("distributor_id")]
        public Int32 Id { get; set; }
        [Column("distributor_name")]
        public string Name { get; set; }
    }
}
