using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationServer.DTO.Student
{
    /// <summary>
    /// Модель DTO для отображения названий классов
    /// </summary>
    public class ClassDto
    {
        /// <summary>
        /// Название класса
        /// </summary>
        public string Name { get; set; }
    }
}
