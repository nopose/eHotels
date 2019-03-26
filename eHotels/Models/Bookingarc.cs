using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Bookingarc
    {
        public int Baid { get; set; }
        public int Rid { get; set; }
        public int CustomerSsn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
