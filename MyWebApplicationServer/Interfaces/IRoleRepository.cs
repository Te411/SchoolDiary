using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IRoleRepository
    {
        Task Add(Role role);
        Task<IEnumerable<Role>> GetAll();

        Task<Role> GetById(Guid id);
    }
}
