using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// День недели
    /// </summary>
    public class WeekDay
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeekDayId { get; set; }

        /// <summary>
        /// Название дня недели
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
