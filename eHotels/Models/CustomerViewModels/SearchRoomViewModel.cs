using eHotels.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class SearchRoomViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Want to filter by the room available for today?")]
        public bool isUsingTodaysRooms { get; set; }

        public List<Room> Rooms { get; set; }

        public List<Hotelchain> HotelChains { get; set; }
        [Display(Name = "Select an hotel chain to filter the results.")]
        public int? ChainSelected { get; set; }

        public List<int> CategoryOfHotel { get; set; }
        [Display(Name = "Select a hotel category to filter the results.")]
        public int? CategorySelected { get; set; }

        public List<string> States { get; set; }
        [Display(Name = "Select a state to filter the results.")]
        public string StateSelected { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
    
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Capacity")]
        public int? Capacity { get; set; }

        [Display(Name = "Number of Rooms")]
        public int? NumberOfRooms { get; set; }

        [Display(Name = "Price of Rooms")]
        public decimal? PriceOfRooms { get; set; }

        public SearchRoomViewModel() { }

        public SearchRoomViewModel(ApplicationDbContext _context) {
            Rooms = new List<Room>();
            initModel(_context);
        }

        public void initModel(ApplicationDbContext _context) {

            DBManipulation db = new DBManipulation(_context);

            HotelChains = db.getHotelChains();
            States = db.getHotelStates();
            CategoryOfHotel = new List<int>(
                new int[] { 1, 2, 3, 4, 5}
            );
        }
    }
}
