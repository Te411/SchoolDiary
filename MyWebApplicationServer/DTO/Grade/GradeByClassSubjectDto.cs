using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTO.Grade
{
    public class GradeByClassSubjectDto
    {
        /// <summary>
        /// Уникальный идентификатор класса
        /// </summary>
        [Required]
        public Guid ClassId { get; set; }

        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        [Required]
        public Guid SubjectId { get; set; }
    }
}
