namespace MyWebApplicationServer.DTO.Student
{
    public class StudentForGradeDto
    {
        /// <summary>
        /// Уникальный индетификатор студента
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
    }
}
