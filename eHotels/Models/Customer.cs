using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Booking = new HashSet<Booking>();
            Renting = new HashSet<Renting>();
        }

        public int Ssn { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Person SsnNavigation { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Renting> Renting { get; set; }
    }
}
