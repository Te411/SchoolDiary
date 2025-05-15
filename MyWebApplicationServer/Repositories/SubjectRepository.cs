using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public SubjectRepository(LibraryContext context) : base(context) { }
    }
}
