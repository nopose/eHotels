using eHotels.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class ManageBookingsViewModel
    {
        [Display(Name = "Select an hotel to filter the list")]
        public int SelectedHotel { get; set; }
        public List<Booking> bookings { get; set; }
        public List<Hotel> hotels { get; set; }

        public ManageBookingsViewModel() { }

        public ManageBookingsViewModel(ApplicationDbContext context)
        {
            initModel(context);
        }

        public void initModel(ApplicationDbContext context, int hid = 0)
        {
            DBManipulation DB = new DBManipulation(context);
            if (hid > 0)
            {
                bookings = DB.getBookingsFullNav(hid);
            }
            else
            {
                bookings = DB.getBookingsFullNav();
            }
            hotels = DB.getHotels();
            hotels.Insert(0, new Hotel
            {
                Hid = 0,
                Hcid = -1,
                HotelName = "All hotels",
                Manager = -1,
                Category = -1,
                NumRooms = -1,
                StreetNumber = -1,
                StreetName = "null",
                AptNumber = -1,
                City = "null",
                HState = "null",
                Zip = "null",
                Email = "null"
            });
        }
    }
}
