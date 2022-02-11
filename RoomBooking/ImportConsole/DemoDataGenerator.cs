using Bogus;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportConsole
{

    public class DemoDataGenerator
    {
        private const int NUMBER_OF_ROOMS = 20;
        private const int NUMBER_OF_CUSTOMERS = 40;

        public static DemoData Generate()
        {
            var result = new DemoData();

            var customers = new Faker<Customer>("de_AT")
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.CreditCardNumber, (f, u) => f.Finance.CreditCardNumber());
            result.Customers = customers.Generate(NUMBER_OF_CUSTOMERS);


            var rooms = new Faker<Room>()
                .RuleFor(r => r.RoomNumber, f => $"R-{f.UniqueIndex}")
                .RuleFor(r => r.RoomType, f => f.PickRandom<RoomType>());
            result.Rooms = rooms.Generate(NUMBER_OF_ROOMS);


            var bookings = new Faker<Booking>()
                .RuleFor(r => r.From, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(-5)))
                .RuleFor(r => r.Customer, f => f.PickRandom(result.Customers))
                .RuleFor(r => r.Room, f => f.PickRandom(result.Rooms));
            var bookingsWithDuplicates = bookings.Generate(10);
            throw new NotImplementedException("TODO: Provide correct booking test data");

            return result;
        }
    }
}
