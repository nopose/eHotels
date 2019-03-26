using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            Hotelphone = new HashSet<Hotelphone>();
            Room = new HashSet<Room>();
        }

        public int Hid { get; set; }
        public int Hcid { get; set; }
        public string HotelName { get; set; }
        public int Manager { get; set; }
        public int Category { get; set; }
        public int NumRooms { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public int? AptNumber { get; set; }
        public string City { get; set; }
        public string HState { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }

        public virtual Hotelchain Hc { get; set; }
        public virtual Employee ManagerNavigation { get; set; }
        public virtual ICollection<Hotelphone> Hotelphone { get; set; }
        public virtual ICollection<Room> Room { get; set; }
    }
}
