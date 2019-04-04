using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class ViewTwoViewModel
    {
        public List<ViewTwo> viewtwos { get; set; }

        public List<Hotel> hotels { get; set; }

        [Display(Name = "Select an hotel to display the capacity of all the rooms for this hotel.")]
        public int hotelSelected { get; set; }
    }
}
