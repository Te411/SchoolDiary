using MyWebApplicationServer.DTOs.Student;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplicationServer.DTOs.FinalGrade
{
    /// <summary>
    /// Общая модель DTO для отображения итоговой оценки
    /// </summary>
    public class FinalGradeDto
    {
        /// <summary>
        /// Уникальный индентификатор итоговой оценки
        /// </summary>
        public Guid FinalGradeId { get; set; }

        /// <summary>
        /// Уникальный индентификатор студента
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Название предмета
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Тип итоговой оценки ("Четверть", "Год")
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// Номер четверти ("1`","2","3","4")
        /// </summary>
        public int Quarter { get; set; }

        /// <summary>
        /// Оценка
        /// </summary>
        public int GradeValue { get; set; }

        /// <summary>
        /// Студент
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual StudentForGradeDto Student { get; set; }
    }
}
