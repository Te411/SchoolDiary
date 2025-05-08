namespace MyWebApplicationServer.Interfaces
{
    /// <summary>
    /// Сервис для хеширования
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Преобразовать пароль в хеш
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string Hash(string password);

        /// <summary>
        /// Преобразовать хеш в пароль
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        bool Verify(string inputPassword, string hashedPassword);
    }
}
