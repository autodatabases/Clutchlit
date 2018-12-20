using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clutchlit.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string getFirstName()
        {
            return this.FirstName;
        }
        public string getLastName()
        {
            return this.LastName;
        }
    }
}
