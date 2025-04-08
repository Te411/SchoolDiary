using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Роль
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Уникальный идентификатор роли
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Название роли
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string RoleName { get; set; }
    }
}
