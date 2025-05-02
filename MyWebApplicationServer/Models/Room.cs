using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Кабинет
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoomId { get; set; }

        /// <summary>
        /// Название кабинета
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
