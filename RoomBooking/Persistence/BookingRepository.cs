using Core.Contracts;
using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;

namespace Persistence
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BookingForCustomerDTO>> GetBookingsForCustomer(int customerId)
        {
            return await _dbContext.Bookings
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Select(b => new BookingForCustomerDTO
                {
                    From = b.From,
                    To = b.To,
                    RoomNumber = b.Room.RoomNumber,
                    RoomType = b.Room.RoomType.ToString(),
                    DaysSleeped = b.To != null ? Math.Round((b.To - b.From).Value.TotalDays,0) : -1
                }).ToListAsync();
        }



    }
}