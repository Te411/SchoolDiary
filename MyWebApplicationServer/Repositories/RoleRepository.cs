using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public RoleRepository(LibraryContext context) : base(context) { }
    }
}
