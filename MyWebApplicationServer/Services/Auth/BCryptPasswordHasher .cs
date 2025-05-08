using MyWebApplicationServer.Interfaces;

namespace MyWebApplicationServer.Services.Auth
{
    /// <summary>
    /// Сервис для управления хешом пароля
    /// </summary>
    public class BCryptPasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Захешировать пароль
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        /// <summary>
        /// Проверить пароль с захешированым паролем
        /// </summary>
        /// <param name="hashedPassword">захешированный пароль</param>
        /// <param name="inputPassword">пароль</param>
        /// <returns></returns>
        public bool Verify(string inputPassword, string hashedPassword) => BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    }
}
