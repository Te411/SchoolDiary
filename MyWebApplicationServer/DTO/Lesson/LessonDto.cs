﻿using MyWebApplicationServer.DTO.Subject;
using MyWebApplicationServer.DTO.Teacher;

namespace MyWebApplicationServer.DTO.Lesson
{
    public class LessonDto
    {
        /// <summary>
        /// Уникальный индентификатор урока
        /// </summary>
        public Guid LessonId { get; set; }

        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Уникальный индентификатор учителя
        /// </summary>
        public Guid TeacherId { get; set; }

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
        public string Homework { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        public SubjectForLessonDto Subject { get; set; }

        /// <summary>
        /// Учитель
        /// </summary>
        public TeacherForLessonDto Teacher { get; set; }
    }
}
