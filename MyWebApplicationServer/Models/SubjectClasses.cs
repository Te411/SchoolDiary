using Project.MyWebApplicationServer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Связь-таблица "предмет - класс"
    /// </summary>
    public class SubjectClasses
    {
        /// <summary>
        /// Уникальный идентификатор класса
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Уникальный идентификатор предмета
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Класс
        /// </summary>
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        /// <summary>
        /// Предмет
        /// </summary>
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
}
