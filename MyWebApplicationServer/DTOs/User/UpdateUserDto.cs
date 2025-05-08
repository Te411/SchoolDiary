using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTOs.User
{
    /// <summary>
    /// Модель DTO для обновления пользователя
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Почта
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Статус активности
        /// </summary>
        public bool? InActive { get; set; }

        /// <summary>
        /// Уникальный индентификатор класса
        /// </summary>
        public string? ClassName { get; set; }
    }
}
