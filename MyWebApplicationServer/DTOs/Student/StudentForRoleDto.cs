using MyWebApplicationServer.DTOs.User;

namespace MyWebApplicationServer.DTOs.Student
{
    /// <summary>
    /// Модель DTO для студента с ролью и классом
    /// </summary>
    public class StudentForRoleDto
    {
        /// <summary>
        /// Уникальный индетификатор студента
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Уникальный индетификатор класса
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Уникальный индетификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Класс
        /// </summary>
        public ClassDto Class { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public UserDtoForRole User { get; set; }
    }
}
