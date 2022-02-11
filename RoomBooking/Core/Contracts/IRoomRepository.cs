using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Contracts
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<RoomDTO>> GetRoomsAsync();
    }
}
