using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Grade;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.DTOs.Subject;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    public class GradeRepository : BaseRepository<Grade>, IGradeRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public GradeRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить все оценки
        /// </summary>
        /// <returns></returns>
        async Task<IEnumerable<GradeDto>> IGradeRepository.GetAll()
        {
            return await _context.Grade
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить оценку по ID
        /// </summary>
        /// <param name="id">ID оценки</param>
        /// <returns></returns>
        async Task<IEnumerable<GradeDto>> IGradeRepository.GetById(Guid id)
        {
            return await _context.Grade
                .Where(g => g.GradeId == id)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить оценки по названию предмета
        /// </summary>
        /// <param name="subjectName">Название предмета</param>
        /// <returns></returns>
        public async Task<IEnumerable<GradeDto>> GetBySubjectName(string subjectName)
        {
            return await _context.Grade
                .Where(g => g.Subject.Name == subjectName)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить оценку по ID класса и ID предмета
        /// </summary>
        /// <param name="classId">ID класса</param>
        /// <param name="subjectId">ID предмета</param>
        /// <returns></returns>
        public async Task<IEnumerable<GradeDto>> GetByClassIdAndSubjectId(Guid classId, Guid subjectId)
        {
            return await _context.Grade
                .Where(g => g.SubjectId == subjectId && g.Student.ClassId == classId)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .Include(g => g.Subject)
                .Select(g => new GradeDto
                {
                    GradeId = g.GradeId,
                    SubjectId = g.SubjectId,
                    SubjectName = g.Subject.Name,
                    Value = g.Value,
                    Data = g.Data,
                    Student = new StudentForGradeDto
                    {
                        StudentId = g.Student.StudentId,
                        Name = g.Student.User.Name,
                    }
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить оценки для пользователя по его ID
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns></returns>
        public async Task<IEnumerable<SubjectForGradeDto>> GetByUserId(Guid userId)
        {
            return await _context.Grade
                .Where(g => g.Student.UserId == userId)
                .Include(g => g.Subject)
                .Include(g => g.Student)
                    .ThenInclude(s => s.User)
                .GroupBy(g => new { g.SubjectId, g.Subject.Name })
                .Select(g => new SubjectForGradeDto
                {
                    Name = g.Key.Name,
                    SubjectId = g.Key.SubjectId,
                    grade = g.Select(g => new GradeDto
                    {
                        GradeId = g.GradeId,
                        Value = g.Value,
                        Data = g.Data,
                        Student = new StudentForGradeDto
                        {
                            StudentId = g.Student.StudentId,
                            Name = g.Student.User.Name,
                        }
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
