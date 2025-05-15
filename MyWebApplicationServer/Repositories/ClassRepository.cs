using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    /// <summary>
    /// Репозиторий для класса
    /// </summary>
    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public ClassRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить все классы
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Class>> GetAll()
        {
            return await _context.Class.ToListAsync();
        }
    }
}
