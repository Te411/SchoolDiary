using MyWebApplicationServer.DTOs.Teacher;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        /// <summary>
        /// Получить учителя по ID пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TeacherGeneralInfoDto> GetTeacherInfoByUserIdAsync(Guid userId);
    }
}
