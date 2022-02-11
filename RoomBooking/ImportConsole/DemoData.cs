using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportConsole
{
    public class DemoData
    {
        public List<Customer> Customers { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();

        public List<Booking> Bookings { get; set; } = new();
    }
}
