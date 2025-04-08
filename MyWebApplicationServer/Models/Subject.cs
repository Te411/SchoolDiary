using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Предмет
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Название предмета
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
