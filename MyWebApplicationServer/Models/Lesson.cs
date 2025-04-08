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
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Уникальный индентификатор учителя
        /// </summary>
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Уникальный индентификатор класса
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Домашнее задание
        /// </summary>
        [MaxLength(255)]
        public string Homework { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        [MaxLength(10)]
        public string Room { get; set; }

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
        /// Класс
        /// </summary>
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
    }
}
