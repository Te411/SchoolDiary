using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Урок
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LessonId { get; set; }

        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        [Required]
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Уникальный индентификатор учителя
        /// </summary>
        [Required]
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Начало урока
        /// </summary>
        [Required]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Конец урока
        /// </summary>
        [Required]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Домашнее задание
        /// </summary>
        [MaxLength(255)]
        public string? Homework { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public Guid? RoomId { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// Учитель
        /// </summary>
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }
    }
}
