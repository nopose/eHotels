using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Booking
    {
        public int Bid { get; set; }
        public int Rid { get; set; }
        public int CustomerSsn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Customer CustomerSsnNavigation { get; set; }
        public virtual Room R { get; set; }
    }
}
