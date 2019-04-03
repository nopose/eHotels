using eHotels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            //FINDQUERY
            return _context.Person
                .Include(p => p.Customer)
                .Where(p => p.Customer != null && p.Ssn == ssn).First();
        }

        public Person getEmployee(int ssn)
        {
            //FINDQUERY
            //EF Generated query
            return _context.Person
                .Include(p => p.Employee)
                .Where(p => p.Employee != null && p.Ssn == ssn).First();
        }

        public List<Person> getEmployees()
        {
            //FINDQUERY
            //EF Generated query
            //SELECT p.ssn, p.apt_number, p.city, p.full_name, p.p_state, p.street_name, p.street_number, p.zip, "p.Employee".ssn, "p.Employee".date_of_employment, "p.Employee".password, "p.Employee".username
            //FROM ehotel.person AS p
            //LEFT JOIN ehotel.employee AS "p.Employee" ON p.ssn = "p.Employee".ssn
            //WHERE "p.Employee".ssn IS NOT NULL
            return _context.Person.Include(p => p.Employee).Where(p => p.Employee != null).ToList();
        }

        public List<Person> getEmployeesWithRoles()
        {
            //FINDQUERY
            //EF Generated query
            
            return _context.Person
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Role)
                .Where(p => p.Employee != null).ToList();
        }

        public List<Hotelchain> getHotelChains()
        {
            //FINDQUERY
            //EF Generated query
            //SET search_path = "eHotel";
            //SELECT h.hcid, h.apt_number, h.city, h.hc_state, h.hotel_chain_name, h.num_hotels, h.street_name, h.street_number, h.zip
            //FROM ehotel.hotelchain AS h
            return _context.Hotelchain.ToList();
        }

        public List<Hotelphone> getHotelPhones(int hid)
        {
            //FINDQUERY
            return _context.Hotelphone.FromSql("SELECT * FROM eHotel.hotelphone WHERE hid={0}", parameters: hid).ToList();
        }

        public List<Hotel> getHotels()
        {
            var hotelChains = getHotelChains();
            return _context.Hotel.ToList();
        }

        public List<Hotel> getHotelsFullNav()
        {
            //FINDQUERY
            //EF Generated query
            //SET search_path = "eHotel";
            //SELECT h.hid, h.apt_number, h.category, h.city, h.email, h.h_state, h.hcid, h.hotel_name, h.manager, h.num_rooms, h.street_name, h.street_number, h.zip, "h.ManagerNavigation".ssn, "h.ManagerNavigation".date_of_employment, "h.ManagerNavigation".password, "h.ManagerNavigation".username, "h.ManagerNavigation.SsnNavigation".ssn, "h.ManagerNavigation.SsnNavigation".apt_number, "h.ManagerNavigation.SsnNavigation".city, "h.ManagerNavigation.SsnNavigation".full_name, "h.ManagerNavigation.SsnNavigation".p_state, "h.ManagerNavigation.SsnNavigation".street_name, "h.ManagerNavigation.SsnNavigation".street_number, "h.ManagerNavigation.SsnNavigation".zip, "h.Hc".hcid, "h.Hc".apt_number, "h.Hc".city, "h.Hc".hc_state, "h.Hc".hotel_chain_name, "h.Hc".num_hotels, "h.Hc".street_name, "h.Hc".street_number, "h.Hc".zip
            //FROM ehotel.hotel AS h
            //INNER JOIN ehotel.employee AS "h.ManagerNavigation" ON h.manager = "h.ManagerNavigation".ssn
            //INNER JOIN ehotel.person AS "h.ManagerNavigation.SsnNavigation" ON "h.ManagerNavigation".ssn = "h.ManagerNavigation.SsnNavigation".ssn
            //INNER JOIN ehotel.hotelchain AS "h.Hc" ON h.hcid = "h.Hc".hcid
            return _context.Hotel
                .Include(h => h.Hc)
                .Include(h => h.ManagerNavigation)
                 .ThenInclude(m => m.SsnNavigation)
                .ToList();
        }

        public List<Room> getRooms()
        {
            //FINDQUERY
            //EF Generated query
            return _context.Room
                .Include(r => r.Amenity)
                .Include(r => r.Damage)
                .ToList();
        }

        public List<Room> getRooms(int hid)
        {
            //FINDQUERY
            //EF Generated query
            return _context.Room
                .Where(r => r.Hid == hid)
                .Include(r => r.Amenity)
                .Include(r => r.Damage)
                .ToList();
        }

        public List<Damage> getRoomDamage(int rid)
        {
            //FINDQUERY
            return _context.Damage.FromSql("SELECT * FROM eHotel.damage WHERE rid={0}", parameters: rid).ToList();
        }

        public List<Amenity> getRoomAmenities(int rid)
        {
            //FINDQUERY
            return _context.Amenity.FromSql("SELECT * FROM eHotel.amenity WHERE rid={0}", parameters: rid).ToList();
        }


        public List<string> getHotelCities()
        {
            //FINDQUERY
            return _context.Hotel.Select(h => h.City).Distinct().ToList();
        }

        public List<string> getHotelStates()
        {
            //FINDQUERY
            return _context.Hotel.Select(h => h.HState).Distinct().ToList();
        }

        public List<Booking> getBookingsByDates(DateTime s, DateTime e)
        {
            //FINDQUERY
            return _context.Booking.FromSql("SELECT * FROM eHotel.booking WHERE (start_date <= {0} AND start_date >= {1}) OR (end_date >= {1} AND end_date <= {0})", e, s).ToList();
        }

        public List<Renting> getRentingsByDates(DateTime s, DateTime e)
        {
            //FINDQUERY
            return _context.Renting.FromSql("SELECT * FROM eHotel.renting WHERE (start_date <= {0} AND start_date >= {1}) OR (end_date >= {1} AND end_date <= {0})", e, s).ToList();
        }

        public Room getRoom(int rid)
        {
            //FINDQUERY
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
            //FINDQUERY
            /*
                SELECT "r.Amenity".aid, "r.Amenity".amenity, "r.Amenity".rid
                FROM ehotel.amenity AS "r.Amenity"
                INNER JOIN (
                    SELECT DISTINCT r0.rid
                    FROM (
                        SELECT rid, room_num, room.hid, price, capacity, isExtandable, landscape FROM eHotel.room INNER JOIN eHotel.hotel ON room.hid = hotel.hid WHERE 0=0 AND capacity >= 5 AND rid NOT IN (17, 17)
                    ) AS r0
                    INNER JOIN ehotel.hotel AS "r.H0" ON r0.hid = "r.H0".hid
                ) AS t ON "r.Amenity".rid = t.rid
                ORDER BY t.rid
             */
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
    }
}
