using MyWebApplicationServer.DTOs.Grade;
using MyWebApplicationServer.DTOs.Subject;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IGradeRepository : IBaseRepository<Grade>
    {
        new Task<IEnumerable<GradeDto>> GetAll();
        new Task<IEnumerable<GradeDto>> GetById(Guid id);
        Task<IEnumerable<GradeDto>> GetBySubjectName(string subjectName);
        Task<IEnumerable<GradeDto>> GetByClassIdAndSubjectId(Guid classId, Guid subjectId);
        Task<IEnumerable<SubjectForGradeDto>> GetByUserId(Guid userId);
    }
}
