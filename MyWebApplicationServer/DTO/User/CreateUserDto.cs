using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTO.User
{
    public class CreateUserDto
    {
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [EmailAddress]
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
        public Guid? ClassId { get; set; }
    }
}
