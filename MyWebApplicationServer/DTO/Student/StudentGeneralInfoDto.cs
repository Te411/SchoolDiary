namespace MyWebApplicationServer.DTO.Student
{
    /// <summary>
    /// Модель DTO для общей информации о студенте
    /// </summary>
    public class StudentGeneralInfoDto
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Класс
        /// </summary>
        public string ClassName { get; set; }
    }
}
