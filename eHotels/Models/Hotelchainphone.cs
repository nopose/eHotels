using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Hotelchainphone
    {
        public string PhoneNumber { get; set; }
        public int Hcid { get; set; }

        public virtual Hotelchain Hc { get; set; }
    }
}
