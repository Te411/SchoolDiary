using MyWebApplicationServer.DTOs.Student;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<IEnumerable<StudentForRoleDto>> GetAllStudent();
        Task<IEnumerable<StudentForGradeDto>> GetByUserId(Guid userId);
        Task<IEnumerable<StudentForGradeDto>> GetByClassName(string className);
        Task<IEnumerable<StudentGeneralInfoDto>> GetGeneralInfoById(Guid id);
        Task<Student> GetByStudentName(string studentName);
    }
}
