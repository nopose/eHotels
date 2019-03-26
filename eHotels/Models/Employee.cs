using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Hotel = new HashSet<Hotel>();
            Renting = new HashSet<Renting>();
            Role = new HashSet<Role>();
        }

        public int Ssn { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Person SsnNavigation { get; set; }
        public virtual ICollection<Hotel> Hotel { get; set; }
        public virtual ICollection<Renting> Renting { get; set; }
        public virtual ICollection<Role> Role { get; set; }
    }
}
