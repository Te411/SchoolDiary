using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Project.MyWebApplicationServer.Models;

namespace Project.MyWebApplicationServer.Models
{
    /// <summary>
    /// Студент
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Уникальный индентификатор
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StudentId { get; set; }

        /// <summary>
        /// Уникальный идентификатор класса
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Класс
        /// </summary>
        [ForeignKey("ClassId")]
        public virtual Class Class{ get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
