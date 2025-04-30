using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTO.FinalGrade
{
    /// <summary>
    /// Модель DTO для формы создания итоговой оценки
    /// </summary>
    public class CreateFinalGradeDto
    {
        /// <summary>
        /// Уникальный индентификатор студента
        /// </summary>
        [Required]
        public Guid StudentId { get; set; }

        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        [Required]
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Тип итоговой оценки ("Четверть", "Год")
        /// </summary>
        [Required]
        [RegularExpression("Четверть|Год", ErrorMessage = "Недопустимый тип периода")]
        public string PeriodType { get; set; }

        /// <summary>
        /// Номер четверти ("1`","2","3","4")
        /// </summary>
        [Range(0, 4, ErrorMessage = "Номер четверти должен быть от 1 до 4")]
        public int Quarter { get; set; }

        /// <summary>
        /// Оценка
        /// </summary>
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int GradeValue { get; set; }
    }
}
