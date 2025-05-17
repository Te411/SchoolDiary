using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.FinalGrade;
using MyWebApplicationServer.DTOs.Student;
using MyWebApplicationServer.Interfaces;
using MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    /// <summary>
    /// Репозиторий для итоговой оценки
    /// </summary>
    public class FinalGradeRepository : BaseRepository<FinalGrade>, IFinalGradeRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public FinalGradeRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить все итоговые оценки
        /// </summary>
        /// <returns></returns>
        async Task<IEnumerable<FinalGradeDto>> IFinalGradeRepository.GetAll()
        {
            return await _context.FinalGrade
               .Include(fg => fg.Subject)
               .Include(fg => fg.Student)
                   .ThenInclude(s => s.User)
               .Select(fg => new FinalGradeDto
               {
                   FinalGradeId = fg.FinalGradeId,
                   StudentId = fg.StudentId,
                   SubjectId = fg.SubjectId,
                   SubjectName = fg.Subject.Name,
                   PeriodType = fg.PeriodType,
                   Quarter = fg.Quarter,
                   GradeValue = fg.GradeValue,
                   Student = new StudentForGradeDto
                   {
                       StudentId = fg.Student.StudentId,
                       Name = fg.Student.User.Name,
                   }
               })
               .ToListAsync();
        }

        /// <summary>
        /// Получить итоговую оценку по её ID
        /// </summary>
        /// <param name="id">ID итоговой оценки</param>
        /// <returns></returns>
        async Task<FinalGradeDto?> IFinalGradeRepository.GetById(Guid id)
        {
            return await _context.FinalGrade
               .Where(fg => fg.FinalGradeId == id)
               .Include(fg => fg.Subject)
               .Include(fg => fg.Student)
                   .ThenInclude(s => s.User)
               .Select(fg => new FinalGradeDto
               {
                   FinalGradeId = fg.FinalGradeId,
                   StudentId = fg.StudentId,
                   SubjectId = fg.SubjectId,
                   SubjectName = fg.Subject.Name,
                   PeriodType = fg.PeriodType,
                   Quarter = fg.Quarter,
                   GradeValue = fg.GradeValue,
                   Student = new StudentForGradeDto
                   {
                       StudentId = fg.Student.StudentId,
                       Name = fg.Student.User.Name,
                   }
               })
               .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Получить итоговые оценки по ID студента
        /// </summary>
        /// <param name="studentId">ID студента</param>
        /// <returns></returns>
        public async Task<IEnumerable<FinalGradeDto>> GetByStudentId(Guid studentId)
        {
            return await _context.FinalGrade
               .Where(fg => fg.StudentId == studentId)
               .Include(fg => fg.Subject)
               .Include(fg => fg.Student)
                   .ThenInclude(s => s.User)
               .Select(fg => new FinalGradeDto
               {
                   FinalGradeId = fg.FinalGradeId,
                   StudentId = fg.StudentId,
                   SubjectId = fg.SubjectId,
                   SubjectName = fg.Subject.Name,
                   PeriodType = fg.PeriodType,
                   Quarter = fg.Quarter,
                   GradeValue = fg.GradeValue,
                   Student = new StudentForGradeDto
                   {
                       StudentId = fg.Student.StudentId,
                       Name = fg.Student.User.Name,
                   }
               })
               .ToListAsync();
        }

        /// <summary>
        /// Проверить существует ли оценка
        /// </summary>
        /// <param name="studentId">ID студента</param>
        /// <param name="subjectId">ID предмета</param>
        /// <param name="periodType">тип периода(год/четверть)</param>
        /// <param name="quarter">номер четверти</param>
        /// <returns></returns>
        public async Task<bool> CheckExistingGrade(Guid studentId, Guid subjectId, string periodType, int? quarter)
        {
            return await _context.FinalGrade
                .AnyAsync(fg =>
                    fg.StudentId == studentId &&
                    fg.SubjectId == subjectId &&
                    fg.PeriodType == periodType &&
                    fg.Quarter == quarter);
        }
    }
}
