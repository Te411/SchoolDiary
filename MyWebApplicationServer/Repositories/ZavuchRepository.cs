using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    public class ZavuchRepository : BaseRepository<Zavuch>, IZavuchRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public ZavuchRepository(LibraryContext context) : base(context) { }
    }
}
