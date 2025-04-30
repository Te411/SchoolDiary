using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTO.User
{
    /// <summary>
    /// Модель DTO для создания пользователя
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Роли
        /// </summary>
        [Required]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Уникальный индентификатор класса
        /// </summary>
        public string? ClassName { get; set; }
    }
}
