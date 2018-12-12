using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_order_state_lang")]
    public class B_order_state
    {
        [Column("id_order_state")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }

    }
}
