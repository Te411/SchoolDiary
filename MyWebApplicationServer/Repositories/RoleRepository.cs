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

        public async Task Add(Role role)
        {
            _context.Role.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Role>> GetAll() => await base.GetAll();

        public new async Task<Role> GetById(Guid id) => await base.GetById(id);
    }
}
