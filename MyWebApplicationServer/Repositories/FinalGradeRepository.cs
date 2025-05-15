using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    public class FinalGradeRepository : BaseRepository<FinalGrade>, IFinalGradeRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public FinalGradeRepository(LibraryContext context) : base(context) { }
    }
}
