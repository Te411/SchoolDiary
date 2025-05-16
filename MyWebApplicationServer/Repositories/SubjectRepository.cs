using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    /// <summary>
    /// Репозиторий для предмета 
    /// </summary>
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public SubjectRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить предмет по названию
        /// </summary>
        /// <param name="name">название</param>
        /// <returns></returns>
        public async Task<Subject> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Name == name);
        }
    }
}
