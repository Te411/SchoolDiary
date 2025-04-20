using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Неделя
    /// </summary>
    public class Week
    {
        /// <summary>
        /// Уникальный индентификатор недели
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeekId { get; set; }

        /// <summary>
        /// Дата начала недели
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата конца недели
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
