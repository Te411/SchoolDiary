namespace MyWebApplicationServer.DTO.Teacher
{
    /// <summary>
    /// Модель DTO для отображения в уроке
    /// </summary>
    public class TeacherForLessonDto
    {
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
    }
}
