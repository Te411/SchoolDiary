using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;

namespace MyWebApplicationServer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Пользователь
        /// </summary>
        public IUserRepository Users { get; }

        /// <summary>
        /// Студент
        /// </summary>
        public IStudentRepository Students { get; }

        /// <summary>
        /// Расписание
        /// </summary>
        public IScheduleRepository Schedules { get; }

        /// <summary>
        /// Оценка
        /// </summary>
        public IGradeRepository Grades { get; }

        /// <summary>
        /// Учитель
        /// </summary>
        public ITeacherRepository Teacher { get; }

        /// <summary>
        /// Урок
        /// </summary>
        public ILessonRepository Lesson { get; }

        /// <summary>
        /// Завуч
        /// </summary>
        public IZavuchRepository Zavuch { get; }

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <returns></returns>
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
