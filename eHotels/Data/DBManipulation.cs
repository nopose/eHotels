using eHotels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Data
{
    public class DBManipulation
    {
        private readonly ApplicationDbContext _context;

        public DBManipulation(ApplicationDbContext context)
        {
            _context = context;
        }

        public Person getCustomer(int ssn)
        {
            return _context.Person
                .Include(p => p.Customer)
                .Where(p => p.Customer != null && p.Ssn == ssn).First();
        }

        public List<Person> getCustomers()
        {
            return _context.Person
                .Include(p => p.Customer)
                .Where(p => p.Customer != null).ToList();
        }

        public Person getEmployee(int ssn)
        {
            return _context.Person
                .Include(p => p.Employee)
                .Where(p => p.Employee != null && p.Ssn == ssn).First();
        }

        public List<Person> getEmployees()
        {
            return _context.Person.Include(p => p.Employee).Where(p => p.Employee != null).ToList();
        }

        public List<Person> getEmployeesWithRoles()
        {
            return _context.Person
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Role)
                .Where(p => p.Employee != null).ToList();
        }

        public List<Hotelchain> getHotelChains()
        {
            return _context.Hotelchain.ToList();
        }

        public List<Hotelphone> getHotelPhones(int hid)
        {
            return _context.Hotelphone.FromSql("SELECT * FROM eHotel.hotelphone WHERE hid={0}", parameters: hid).ToList();
        }

        public List<Hotel> getHotels()
        {
            var hotelChains = getHotelChains();
            return _context.Hotel.ToList();
        }

        public List<Hotel> getHotelsFullNav()
        {
            return _context.Hotel
                .Include(h => h.Hc)
                .Include(h => h.ManagerNavigation)
                 .ThenInclude(m => m.SsnNavigation)
                .ToList();
        }

        public List<Room> getRooms()
        {
            return _context.Room
                .Include(r => r.Amenity)
                .Include(r => r.Damage)
                .ToList();
        }

        public List<Room> getRooms(int hid)
        {
            return _context.Room
                .Where(r => r.Hid == hid)
                .Include(r => r.Amenity)
                .Include(r => r.Damage)
                .ToList();
        }

        public List<Damage> getRoomDamage(int rid)
        {
            return _context.Damage.FromSql("SELECT * FROM eHotel.damage WHERE rid={0}", parameters: rid).ToList();
        }

        public List<Amenity> getRoomAmenities(int rid)
        {
            return _context.Amenity.FromSql("SELECT * FROM eHotel.amenity WHERE rid={0}", parameters: rid).ToList();
        }

        public List<string> getHotelStates()
        {
            return _context.Hotel.Select(h => h.HState).Distinct().ToList();
        }

        public List<Booking> getBookingsByDates(DateTime s, DateTime e)
        {
            return _context.Booking.FromSql("SELECT * FROM eHotel.booking WHERE (start_date <= {0} AND start_date >= {1}) OR (end_date >= {1} AND end_date <= {0})", e, s).ToList();
        }

        public List<Renting> getRentingsByDates(DateTime s, DateTime e)
        {
            return _context.Renting.FromSql("SELECT * FROM eHotel.renting WHERE (start_date <= {0} AND start_date >= {1}) OR (end_date >= {1} AND end_date <= {0})", e, s).ToList();
        }

        public Room getRoom(int rid)
        {
            return _context.Room
                .Include(r => r.H)
                    .ThenInclude(h => h.Hc)
                .Include(r => r.H)
                    .ThenInclude(h => h.Hotelphone)
                .Where(r => r.Rid == rid)
                .First();
        }

        public List<Room> getRoomForSearch(string qry)
        {
            return _context.Room.FromSql(qry).Include(r => r.H).Include(r => r.Amenity).ToList();
        }

        public List<Booking> getBookings()
        {
            return _context.Booking
                .Include(b => b.R)
                    .ThenInclude(r => r.H)
                .ToList();
        }

        public List<Booking> getBookings(int hid)
        {
            return _context.Booking
                .Include(b => b.R)
                    .ThenInclude(r => r.H)
                .Where(b => b.R.Hid == hid)
                .ToList();
        }

        public List<Booking> getPersonBookings(int ssn)
        {
            return _context.Booking
                .Include(b => b.R)
                    .ThenInclude(r => r.H)
                .Where(b => b.CustomerSsn == ssn)
                .ToList();
        }

        public List<Booking> getBookingsFullNav()
        {
            return _context.Booking
                .Include(b => b.R)
                    .ThenInclude(r => r.H)
                .Include(b => b.CustomerSsnNavigation)
                    .ThenInclude(c => c.SsnNavigation)
                .ToList();
        }

        public List<Booking> getBookingsFullNav(int hid)
        {
            return _context.Booking
                .Include(b => b.R)
                    .ThenInclude(r => r.H)
                .Include(b => b.CustomerSsnNavigation)
                    .ThenInclude(c => c.SsnNavigation)
                .Where(b => b.R.Hid == hid)
                .ToList();
        }

        public List<Hotelchainphone> getHotelChainPhones(int hcid)
        {
            return _context.Hotelchainphone.FromSql("SELECT * FROM eHotel.hotelchainphone WHERE hcid={0}", parameters: hcid).ToList();
        }

        public List<Hotelchainemail> getHotelChainEmails(int hcid)
        {
            return _context.Hotelchainemail.FromSql("SELECT * FROM eHotel.hotelchainemail WHERE hcid={0}", parameters: hcid).ToList();
        }

        public List<ViewOne> getViewOne()
        {
            return _context.ViewOne.FromSql("SELECT * FROM eHotel.num_hotel_area").ToList();
        }

        public List<ViewOne> getViewOne(string state)
        {
            return _context.ViewOne.FromSql("SELECT * FROM eHotel.num_hotel_area WHERE h_state={0}", state).ToList();
        }

        public List<ViewTwo> getViewTwo()
        {
            return _context.ViewTwo.FromSql("SELECT * FROM eHotel.capacity_room").ToList();
        }

        public List<ViewTwo> getViewTwo(int hid)
        {
            return _context.ViewTwo.FromSql("SELECT * FROM eHotel.capacity_room WHERE hid={0}", hid).ToList();
        }

        public int deleteBooking(int bid, int ssn)
        {
            try
            {
                if (_context.Customer.FirstOrDefault(c => c.Ssn == ssn) != null)
                {
                    _context.Booking.Remove(_context.Booking.Find(bid));
                    _context.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Npgsql.PostgresException)
            {
                return -1;
            }        
        }
    }
}
