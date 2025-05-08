namespace MyWebApplicationServer.Services.JWT
{
    /// <summary>
    /// Настройки JWT токена
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Ключ для шифрования
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Издатель
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Потребитель 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Время жизни токена
        /// </summary>
        public int ExpiryInMinutes { get; set; }
    }
}
