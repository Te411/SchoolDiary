namespace MyWebApplicationServer.DTO.Lesson
{
    public class LessonForScheduleByTeacherDto
    {
        /// <summary>
        /// Уникальный индетификатор урока
        /// </summary>
        public Guid LessonId { get; set; }

        /// <summary>
        /// Название класса
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Порядок урока
        /// </summary>
        public int LessonOrder { get; set; }

        /// <summary>
        /// Название предмета
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Начало урока
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Конец урока
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Домашнее задание
        /// </summary>
        public string? Homework { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string? RoomName { get; set; }
    }
}
