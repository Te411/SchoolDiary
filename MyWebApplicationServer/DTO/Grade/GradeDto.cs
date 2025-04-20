using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyWebApplicationServer.DTO.Student;

namespace MyWebApplicationServer.DTO.Grade
{
    public class GradeDto
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
        /// Название предмета
        /// </summary>
        public string SubjectName { get; set; }

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
        /// Студент
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual StudentForGradeDto Student { get; set; }
    }
}
