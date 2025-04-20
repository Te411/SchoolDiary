using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Project.MyWebApplicationServer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.MyWebApplicationServer.Models
{
    public class Schedule
    {
        /// <summary>
        /// Уникальный индентификатор расписания
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// Уникальный индентификатор класса
        /// </summary>
        [Key]
        [Column(Order = 2)]
        public Guid ClassId { get; set; }

        /// <summary>
        /// Уникальный индентификатор дня недели
        /// </summary>
        [Key]
        [Column(Order = 3)]
        public int WeekDayId { get; set; }

        /// <summary>
        /// Уникальный индентификатор урока
        /// </summary>
        public Guid LessonId { get; set; }

        /// <summary>
        /// Порядок урока
        /// </summary>
        [Key]
        [Column(Order = 4)]
        public int LessonOrder { get; set; }

        /// <summary>
        /// Уникальный индентификатор недели
        /// </summary>
        [Key]
        [Column(Order = 5)]
        public int WeekId { get; set; }

        /// <summary>
        /// Класс
        /// </summary>
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        /// <summary>
        /// Дни недели
        /// </summary>
        [ForeignKey("WeekDayId")]
        public virtual WeekDay WeekDay { get; set; }

        /// <summary>
        /// Урок
        /// </summary>
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }

        /// <summary>
        /// Урок
        /// </summary>
        [ForeignKey("WeekId")]
        public virtual Week Week { get; set; }
    }
}