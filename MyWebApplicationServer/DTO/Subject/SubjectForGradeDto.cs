using MyWebApplicationServer.DTO.Grade;

namespace MyWebApplicationServer.DTO.Subject
{
    public class SubjectForGradeDto
    {
        /// <summary>
        /// Уникальный индентификатор предмета
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Оценка
        /// </summary>
        public List<GradeDto> grade { get; set; }
    }
}
