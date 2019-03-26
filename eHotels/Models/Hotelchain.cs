using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Hotelchain
    {
        public Hotelchain()
        {
            Hotel = new HashSet<Hotel>();
            Hotelchainemail = new HashSet<Hotelchainemail>();
            Hotelchainphone = new HashSet<Hotelchainphone>();
        }

        public int Hcid { get; set; }
        public string HotelChainName { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public int? AptNumber { get; set; }
        public string City { get; set; }
        public string HcState { get; set; }
        public string Zip { get; set; }
        public int NumHotels { get; set; }

        public virtual ICollection<Hotel> Hotel { get; set; }
        public virtual ICollection<Hotelchainemail> Hotelchainemail { get; set; }
        public virtual ICollection<Hotelchainphone> Hotelchainphone { get; set; }
    }
}
