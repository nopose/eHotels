using eHotels.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class ManageRoomViewModel
    {
        [Display(Name = "Select an hotel to filter the list")]
        public int SelectedHotel { get; set; }
        public List<Room> rooms { get; set; }
        public List<Hotel> hotels { get; set; }

        public ManageRoomViewModel() { }

        public ManageRoomViewModel(ApplicationDbContext context) {
            initModel(context);
        }

        public void initModel(ApplicationDbContext context, int hid = 0)
        {
            DBManipulation DB = new DBManipulation(context);
            if(hid > 0)
            {
                rooms = DB.getRooms(hid);
            }
            else
            {
                rooms = DB.getRooms();
            }
            hotels = DB.getHotels();
        }
    }
}
