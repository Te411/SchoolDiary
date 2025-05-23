﻿namespace MyWebApplicationServer.DTOs.User
{
    /// <summary>
    /// Модель DTO для общей информации о пользователе
    /// </summary>
    public class UserGeneralInfoDto
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
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Класс
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Роли
        /// </summary>
        public List<string> Roles { get; set; }
    }
}
