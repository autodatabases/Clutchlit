using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_message")]
    public class B_message
    {
        [Key]
        [Column("id_message")]
        public int Id_message { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("id_order")]
        public int Id_order { get; set; }
    }
}
