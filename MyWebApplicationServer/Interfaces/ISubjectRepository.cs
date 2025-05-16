using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface ISubjectRepository : IBaseRepository<Subject>
    {
        Task<Subject> GetByNameAsync(string name);
    }
}
