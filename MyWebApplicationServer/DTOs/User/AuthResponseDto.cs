namespace MyWebApplicationServer.DTOs.User
{
    /// <summary>
    /// Модель DTO для ответа по авторизации
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Уникальный индентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Уникальный индентификатор класса
        /// </summary>
        public Guid? ClassId { get; set; }

        /// <summary>
        /// Роли
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// JWT токен
        /// </summary>
        public string Token { get; set; }
    }
}
