using MyWebApplicationServer.DTOs.Room;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<IEnumerable<RoomDto>> GetAllRooms();
    }
}
