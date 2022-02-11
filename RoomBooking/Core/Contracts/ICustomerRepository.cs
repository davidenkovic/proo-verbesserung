using Core.Dtos;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<List<CustomerDTO>> GetCustomersForView();
    }
}
