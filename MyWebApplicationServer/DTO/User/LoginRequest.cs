namespace MyWebApplicationServer.DTO.User
{
    /// <summary>
    /// Модель DTO для формы авторизации
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
