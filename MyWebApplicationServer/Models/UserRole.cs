using Project.MyWebApplicationServer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Связь роль-пользователь
    /// </summary>
    [Table("UserRoles")]
    public class UserRole
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Уникальный идентификатор роли
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
