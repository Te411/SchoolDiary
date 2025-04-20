namespace MyWebApplicationServer.DTO.User
{
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
    }
}
