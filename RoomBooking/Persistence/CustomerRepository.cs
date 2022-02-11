using Core.Contracts;
using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;

namespace Persistence
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CustomerDTO>> GetCustomersForView()
        {
            return await _dbContext.Customers.Select(c => new CustomerDTO
            {
                CustomerId = c.Id,
                LastName = c.LastName,
                FirstName = c.FirstName,
                CountBookings = _dbContext.Bookings.Where(b => b.CustomerId == c.Id).Count()
            }).ToListAsync();
        }
    }
}
