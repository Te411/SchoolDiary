namespace MyWebApplicationServer.DTO.Lesson
{
    public class LessonForAddScheduleDto
    {
        /// <summary>
        /// Уникальный идентификатор урока
        /// </summary>
        public Guid LessonId { get; set; }

        /// <summary>
        /// Порядковый номер урока
        /// </summary>
        public int LessonOrder { get; set; }
    }
}
