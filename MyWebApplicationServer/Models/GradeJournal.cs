using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Project.MyWebApplicationServer.Models;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Журнал
    /// </summary>
    public class GradeJournal
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GradeJournalId { get; set; }

        /// <summary>
        /// Уникальный индентификатор урока
        /// </summary>
        public Guid LessonId { get; set; }

        /// <summary>
        /// Уникальный индентификатор студента
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Оценка
        /// </summary>
        [Range(1, 5, ErrorMessage = "Оценка должна быть в диапазоне от 1 до 5")]
        public int? Grade { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Урок
        /// </summary>
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }

        /// <summary>
        /// Студент
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}
