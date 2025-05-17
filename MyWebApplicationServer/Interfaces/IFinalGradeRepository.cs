using MyWebApplicationServer.DTOs.FinalGrade;
using MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IFinalGradeRepository : IBaseRepository<FinalGrade>
    {
        new Task<IEnumerable<FinalGradeDto>> GetAll();
        new Task<FinalGradeDto> GetById(Guid id);
        new Task<IEnumerable<FinalGradeDto>> GetByStudentId(Guid studentId);
        Task<bool> CheckExistingGrade(Guid studentId, Guid subjectId, string periodType, int? quarter);
    }
}
