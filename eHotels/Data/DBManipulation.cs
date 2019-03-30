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

        public List<Person> getEmployees()
        {
            var employees = _context.Employee.ToList();
            var persons = _context.Person.FromSql("SELECT * FROM eHotel.person").ToList();
            return persons.FindAll(x => x.Employee != null);
        }

        public List<Hotelchain> getHotelChains()
        {
            return _context.Hotelchain.ToList();
        }

        public List<Hotel> getHotels()
        {
            return _context.Hotel.ToList();
        }
    }
}
