using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Hotelchainemail
    {
        public string Email { get; set; }
        public int Hcid { get; set; }

        public virtual Hotelchain Hc { get; set; }
    }
}
