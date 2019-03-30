using eHotels.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class HotelViewModel
    {

        [Required]
        [Display(Name = "Hotel Name")]
        public string HotelName { get; set; }

        [Required]
        [Display(Name = "Hotel Chain Name")]
        public int HotelChainID { get; set; }

        [Required]
        [Display(Name = "Manager Name")]
        public int ManagerSSN { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Category should be between 1 and 5")]
        public int Category { get; set; }

        [Required]
        [Display(Name = "Street Number")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The street number should be higher than 0")]
        public int StreetNumber { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "Apartment Number")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The apartment number should be higher than 0")]
        public int? AptNumber { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        [RegularExpression("[A-Z]{2}", ErrorMessage = "SSN should be exactly 2 capital letters")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        [RegularExpression("[0-9a-zA-Z]{6}", ErrorMessage = "Zip code should be exactly 6 characters")]
        public string Zip { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Contact email")]
        public string Email { get; set; }

        public List<Person> employees { get; set; }
        public List<Hotelchain> chains { get; set; }

        public HotelViewModel() { }

        public HotelViewModel(ApplicationDbContext context)
        {
            DBManipulation DB = new DBManipulation(context);
            employees = DB.getEmployees();
            chains = DB.getHotelChains();
        }
    }
}
