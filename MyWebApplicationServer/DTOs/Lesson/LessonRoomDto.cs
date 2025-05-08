using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTOs.Lesson
{
    /// <summary>
    /// Модель DTO для отображения в уроке
    /// </summary>
    public class LessonRoomDto
    {
        /// <summary>
        /// Кабинет
        /// </summary>
        [MaxLength(10)]
        public string RoomName { get; set; }
    }
}
