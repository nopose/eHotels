using System;
using System.Collections.Generic;
using System.Collections;

namespace eHotels.Models
{
    public partial class Room
    {
        public Room()
        {
            Amenity = new HashSet<Amenity>();
            Booking = new HashSet<Booking>();
            Damage = new HashSet<Damage>();
            Renting = new HashSet<Renting>();
        }

        public int Rid { get; set; }
        public int RoomNum { get; set; }
        public int Hid { get; set; }
        public decimal Price { get; set; }
        public short Capacity { get; set; }
        public BitArray Landscape { get; set; }
        public bool Isextandable { get; set; }

        public virtual Hotel H { get; set; }
        public virtual ICollection<Amenity> Amenity { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Damage> Damage { get; set; }
        public virtual ICollection<Renting> Renting { get; set; }
    }
}
