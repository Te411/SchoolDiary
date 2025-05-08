namespace MyWebApplicationServer.DTOs.Student
{
    /// <summary>
    /// Модель DTO для отображения студента в оценках
    /// </summary>
    public class StudentForGradeDto
    {
        /// <summary>
        /// Уникальный индетификатор студента
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
    }
}
