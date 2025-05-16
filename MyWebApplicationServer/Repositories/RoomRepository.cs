using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Room;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public RoomRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить все кабинеты
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RoomDto>> GetAllRooms()
        {
            return await _context.Room
                .Select(r => new RoomDto
                {
                    Name = r.Name,
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
