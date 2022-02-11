using Core.Dtos;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        public Task<List<BookingForCustomerDTO>> GetBookingsForCustomer(int customerId);
    }
}