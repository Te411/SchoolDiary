using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyWebApplicationServer.DTOs.User
{
    /// <summary>
    /// Общая модель DTO
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Название класса
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Статус активности
        /// </summary>
        [DefaultValue(false)]
        public bool InActive { get; set; }

        /// <summary>
        /// Роли
        /// </summary>
        public List<string> Roles { get; set; }
    }
}
