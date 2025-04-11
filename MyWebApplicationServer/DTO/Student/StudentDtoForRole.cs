using MyWebApplicationServer.DTO.User;

namespace MyWebApplicationServer.DTO.Student
{
    public class StudentDtoForRole
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
