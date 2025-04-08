using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Завуч
    /// </summary>
    public class Zavuch
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ZavuchId { get; set; }

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
