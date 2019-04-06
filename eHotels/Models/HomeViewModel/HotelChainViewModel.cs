using eHotels.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{

    public class HotelChainViewModel
    {
        public String Hcid { get; set; }

        [Required]
        [Display(Name = "Hotel Name")]
        public string HotelChainName { get; set; }

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
        [RegularExpression("[A-Z]{2}", ErrorMessage = "State should be exactly 2 capital letters")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        [RegularExpression("[0-9a-zA-Z]{5,6}", ErrorMessage = "Zip code should be  5 or 6 characters")]
        public string Zip { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneAdd { get; set; }

        [Display(Name = "Email")]
        public string EmailAdd { get; set; }
        
        public List<Hotelchainphone> phones { get; set; }
        public List<Hotelchainemail> emails { get; set; }

        public HotelChainViewModel() { }

        public HotelChainViewModel(ApplicationDbContext context, Hotelchain hotel)
        {
            initModel(context, hotel.Hcid);

            Hcid = hotel.Hcid.ToString();
            HotelChainName = hotel.HotelChainName;
            StreetNumber = hotel.StreetNumber;
            StreetName = hotel.StreetName;
            AptNumber = hotel.AptNumber;
            City = hotel.City;
            State = hotel.HcState;
            Zip = hotel.Zip;
        }

        public void initModel(ApplicationDbContext context, int hcid)
        {
            DBManipulation DB = new DBManipulation(context);
            phones = DB.getHotelChainPhones(hcid);
            emails = DB.getHotelChainEmails(hcid);
        }
    }
}
