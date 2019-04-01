using eHotels.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class RoomViewModel
    {
        public String Rid { get; set; }

        [Required]
        [Display(Name = "Hotel Name")]
        public int HotelID { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        [Range(100, Int32.MaxValue, ErrorMessage = "The room number should be minimum 100")]
        public int RoomNum { get; set; }

        [Required]
        [Display(Name = "Price ($)")]
        [Range(0, 100000000, ErrorMessage = "The price should be a positive number")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        [Range(1, 20, ErrorMessage = "The capacity should be between 1 and  20")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Landscape")]
        public int Landscape { get; set; }

        [Required]
        [Display(Name = "Extandable")]
        public bool Isextandable { get; set; }

        public List<Hotel> hotels { get; set; }
        public List<NameValuePair> landscapes { get; set; } = new List<NameValuePair> { new NameValuePair(0,"Sea View"), new NameValuePair(1,"Mountain View") };

        public RoomViewModel() { }

        public RoomViewModel(ApplicationDbContext context)
        {
            initModel(context);
        }

        public RoomViewModel(ApplicationDbContext context, Room room)
        {
            initModel(context);

            HotelID = room.Hid;
            RoomNum = room.RoomNum;
            Price = room.Price;
            Capacity = room.Capacity;
            Landscape = room.Landscape;
            Isextandable = room.Isextandable;
        }

        public void initModel(ApplicationDbContext context)
        {
            DBManipulation DB = new DBManipulation(context);
            hotels = DB.getHotels();
        }
    }

    public class NameValuePair
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public NameValuePair(int value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}
