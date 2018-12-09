using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("cl_manufacturers")]
    public class Manufacturer
    {
        [Column("manufacturer_id")]
        public Int32 Id { get; set; }
        [Column("manufacturer_tecdoc_id")]
        public Int32 Tecdoc_id { get; set; }
        [Column("manufacturer_name")]
        public string Name { get; set; }
        

        public Manufacturer(Int32 id, Int32 tecdoc_id, string name)
        {
            this.Id = id;
            this.Tecdoc_id = tecdoc_id;
            this.Name = name;
        }
    }
}
