using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Contracts;
using Core.Entities;
using Core.Dtos;

namespace Persistence
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RoomDTO>> GetRoomsAsync()
        {
            return await _dbContext.Rooms.Include(r => r.Bookings).ThenInclude(b => b.Customer).Select(r => new RoomDTO()
            {
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                From = r.Bookings != null ? r.Bookings.OrderByDescending(b => b.From).First().From : null,
                To = r.Bookings != null ? r.Bookings.OrderByDescending(b => b.From).First().To : null,
                IsEmpty = r.Bookings != null ? CheckIsEmpty(r.Bookings.OrderByDescending(b => b.From).First()) : true,
                CurrentBooking = r.Bookings != null ? r.Bookings.OrderByDescending(b => b.From).First() : null
            })
                .ToListAsync();

        }

        public static bool CheckIsEmpty(Booking booking)
        {
            if (booking.To == null || booking.To > DateTime.Now)
            {
                if (booking.From < DateTime.Now)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
            else
            {
                return true;
            }
        }
    }
}

