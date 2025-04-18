namespace MyWebApplicationServer.DTO.Lesson
{
    public class AddHomeworkLessonDto
    {
        /// <summary>
        /// Уникальный индентификатор урока
        /// </summary>
        public Guid LessonId { get; set; }

        /// <summary>
        /// Домашнее задание
        /// </summary>
        public string Homework { get; set; }
    }
}
