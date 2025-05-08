namespace MyWebApplicationServer.DTOs.User
{
    public class UserDtoForRole
    {
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Статус активности
        /// </summary>
        public bool InActive { get; set; }
    }
}
