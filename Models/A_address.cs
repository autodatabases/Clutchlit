using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    [Table("ps_address")]
    public class A_address
    {
        [Key]
        [Column("id_address")]
        public int Id_address { get; set; }
        [Column("company")]
        public string Company { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("address1")]
        public string Address1 { get; set; }
        [Column("address2")]
        public string Address2 { get; set; }
        [Column("postcode")]
        public string ZipCode { get; set; }
        [Column("city")]
        public string City { get; set; }
        [Column("phone")]
        public string PhoneNumber { get; set; }
        [Column("phone_mobile")]
        public string Phone { get; set; }
        [Column("vat_number")]
        public string Nip { get; set; }
        [Column("other")]
        public string AdditionalInfo { get; set; }
    }
}
