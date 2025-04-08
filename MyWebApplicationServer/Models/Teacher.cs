using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Учитель
    /// </summary>
    public class Teacher
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Уникальный индентификатор пользователя
        /// </summary>
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }
    }
}
