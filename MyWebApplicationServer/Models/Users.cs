using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Login { get; set; }

        /// <summary>
        /// Статус активности
        /// </summary>
        [DefaultValue(false)]
        public bool InActive { get; set; }
    }
}
