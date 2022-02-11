
using Persistence;
using System;
using System.Threading.Tasks;

namespace ImportConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await using var uow = new UnitOfWork();
            Console.WriteLine("Datenbank migrieren");
            await uow.DeleteDatabaseAsync();
            await uow.CreateDatabaseAsync();


            Console.WriteLine("Daten generieren");

            var data = DemoDataGenerator.Generate();

            Console.WriteLine("Daten speichern");

            throw new NotImplementedException("TODO: Insert test data");

            var customersCount = await uow.Customers.CountAsync();
            var roomsCount = await uow.Rooms.CountAsync();
            var bookingsCount = await uow.Bookings.CountAsync();

            Console.WriteLine($"\n{customersCount} Hotelgäste, {roomsCount} Zimmer, {bookingsCount} Zimmerbuchungen wurden aus DB gelesen.");


        }

       
    }
}
