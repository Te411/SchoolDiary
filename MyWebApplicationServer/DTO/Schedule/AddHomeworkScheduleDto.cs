namespace MyWebApplicationServer.DTO.Schedule
{
    /// <summary>
    /// Модель DTO для формы заполнения домашнего задания через расписание
    /// </summary>
    public class AddHomeworkScheduleDto
    {
        /// <summary>
        /// Название класса
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Уникальный индентификатор недели
        /// </summary>
        public int WeekId { get; set; }

        /// <summary>
        /// Уникальный индентификатор дня недели
        /// </summary>
        public int WeekDayId { get; set; }

        /// <summary>
        /// Порядковый номер урока
        /// </summary>
        public int LessonOrder { get; set; }

        /// <summary>
        /// Домашнее задание
        /// </summary>
        public string Homework { get; set; }
    }
}
