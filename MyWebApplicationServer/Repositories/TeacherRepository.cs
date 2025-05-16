using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Teacher;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    /// <summary>
    /// Репозиторий для учителя
    /// </summary>
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public TeacherRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить учителя по ID пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns></returns>
        public async Task<TeacherGeneralInfoDto> GetTeacherInfoByUserIdAsync(Guid userId)
        {
            var teacher = await _context.Teacher
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .Select(t => new TeacherGeneralInfoDto
                {
                    Name = t.User.Name,
                    Email = t.User.Email,
                })
                .FirstOrDefaultAsync();

            return teacher;
        }
    }
}
