using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext? _dbContext;
        public UnitOfWork() : this(new ApplicationDbContext())
        { }
        public UnitOfWork(ApplicationDbContext dBContext)
        {
            _dbContext = dBContext;
            Rooms = new RoomRepository(_dbContext);
            Customers = new CustomerRepository(_dbContext);
            Bookings = new BookingRepository(_dbContext);
        }

        public ICustomerRepository Customers { get; }
        public IRoomRepository Rooms { get; }
        public IBookingRepository Bookings { get; }


        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext!.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln

            // Allfällige Validierungen der geänderten Entities durchführen
            foreach (var entity in entities)
            {
                await ValidateEntityAsync(entity);
            }

            return await _dbContext.SaveChangesAsync();
        }

        private async Task ValidateEntityAsync(object entity)
        {
            
            Validator.ValidateObject(entity, new ValidationContext(entity), true);

            //if (entity is Booking booking)
            //{
            //    var bookingsForRoom = await _dbContext!.Bookings.Include(b=>b.Customer).Where(b => b.RoomId == booking.RoomId).ToArrayAsync();
            //    var bookingFrom = booking.From;
            //    var bookingTo = booking.To;
            //    foreach (var bookingForRoom in bookingsForRoom)
            //    {
            //        var bookingForRoomFrom = bookingForRoom.From;
            //        var bookingForRoomTo = bookingForRoom.To;
            //        if (bookingFrom >= bookingForRoomFrom && bookingFrom <= bookingForRoomTo)
            //        {
            //            throw new ValidationException($"Es gibt schon eine Buchung von {bookingForRoom!.Customer!.LastName} von {bookingForRoom.From} bis {bookingForRoom.To} ", null, "From");
            //        }
            //        if (bookingTo >= bookingForRoomFrom && bookingTo <= bookingForRoomTo)
            //        {
            //            throw new ValidationException($"Es gibt schon eine Buchung von {bookingForRoom!.Customer!.LastName} von {bookingForRoom.From} bis {bookingForRoom.To} ", null, "To");
            //        }
            //    }
            //}         

        }

        public async Task DeleteDatabaseAsync() => await _dbContext!.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext!.Database.MigrateAsync();
        public async Task CreateDatabaseAsync() => await _dbContext!.Database.EnsureCreatedAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (_dbContext != null)
            {
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
            }
            _dbContext = null;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
