using MyWebApplicationServer.DTO.Lesson;

namespace MyWebApplicationServer.DTO.Schedule
{
    public class AddScheduleNewLessonDto
    {
        /// <summary>
        /// Название класса
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Список уроков для заданного дня
        /// </summary>
        public List<NewLessonForAddScheduleDto> Lessons { get; set; } = new();
    }
}
