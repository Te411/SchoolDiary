﻿using MyWebApplicationServer.DTOs.Lesson;

namespace MyWebApplicationServer.DTOs.Schedule
{
    public class AddScheduleDto
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
        /// Список уроков для заданного дня
        /// </summary>
        public List<LessonForAddScheduleDto> Lessons { get; set; } = new();
    }
}
