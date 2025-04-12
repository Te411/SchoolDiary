using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Project.MyWebApplicationServer.Models;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Оценка
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GradeId { get; set; }

        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Уникальный индентификатор студента
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Оценка
        /// </summary>
        [Range(1, 5, ErrorMessage = "Оценка должна быть в диапазоне от 1 до 5")]
        public int Value { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Урок
        /// </summary>
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// Студент
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}
