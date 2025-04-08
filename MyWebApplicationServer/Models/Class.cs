using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Класс
    /// </summary>
    public class Class
    {
        /// <summary>
        /// Уникальный индентификатор класса
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ClassId { get; set; }

        /// <summary>
        /// Название класса
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
