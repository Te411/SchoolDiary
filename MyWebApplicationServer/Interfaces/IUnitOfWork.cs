using MyWebApplicationServer.Repositories;

namespace MyWebApplicationServer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IStudentRepository Students { get; }
        IScheduleRepository Schedules { get; }
        IGradeRepository Grades { get; }
        ITeacherRepository Teacher { get; }
        ILessonRepository Lesson { get; }
        IZavuchRepository Zavuch { get; }
        
        // Добавить по мере необходимости

        Task<int> CompleteAsync();
    }
}
