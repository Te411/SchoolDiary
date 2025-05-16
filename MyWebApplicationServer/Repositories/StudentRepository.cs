using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.DTOs.User;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    /// <summary>
    /// Репозиторий для студента
    /// </summary>
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public StudentRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить всех студентов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StudentForRoleDto>> GetAllStudent()
        {
            return await _context.Student
               .Include(s => s.Class)
               .Include(s => s.User)
               .Select(s => new StudentForRoleDto
               {
                   StudentId = s.StudentId,
                   ClassId = s.ClassId,
                   UserId = s.UserId,
                   Class = new ClassDto { Name = s.Class.Name },
                   User = new UserDtoForRole
                   {
                       Email = s.User.Email,
                       Password = s.User.Password,
                       Name = s.User.Name,
                       Login = s.User.Login,
                       InActive = s.User.InActive
                   }
               })
               .ToListAsync();
        }

        /// <summary>
        /// Получить студента по ID пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns></returns>
        public async Task<IEnumerable<StudentForGradeDto>> GetByUserId(Guid userId)
        {
            return await _context.Student
                .Where(s => s.UserId == userId)
                .Include(s => s.User)
                .Select(s => new StudentForGradeDto
                {
                    StudentId = s.StudentId,
                    Name = s.User.Name
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить студентов по названию класса
        /// </summary>
        /// <param name="className">Название класса</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<StudentForGradeDto>> GetByClassName(string className)
        {
            return await _context.Student
                .Where(s => s.Class.Name == className)
                .Include(s => s.User)
                .Select(s => new StudentForGradeDto
                {
                    StudentId = s.StudentId,
                    Name = s.User.Name
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить общую информацию о студенте
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StudentGeneralInfoDto>> GetGeneralInfoById(Guid id)
        {
            return await _context.Student
                .Where(s => s.StudentId == id)
                .Include(s => s.User)
                .Include(s => s.Class)
                .Select(s => new StudentGeneralInfoDto
                {
                    Name = s.User.Name,
                    Email = s.User.Email,
                    ClassName = s.Class.Name,
                })
                .ToListAsync();
        }
    }
}
