using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTO.Grade
{
    public class CreateGradeDto
    {
        /// <summary>
        /// Уникальный идентификатор предмета
        /// </summary>
        [Required]
        public string SubjectName { get; set; }

        /// <summary>
        /// Уникальный идентификатор студента
        /// </summary>
        [Required]
        public string StudentName { get; set; }

        /// <summary>
        /// Оценка (1-5)
        /// </summary>
        [Range(1, 5, ErrorMessage = "Оценка должна быть в диапазоне от 1 до 5")]
        public int Value { get; set; }

        /// <summary>
        /// Дата оценки
        /// </summary>
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
