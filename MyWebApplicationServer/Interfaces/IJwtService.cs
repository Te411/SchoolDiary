namespace MyWebApplicationServer.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Генерация JWT-токена
        /// </summary>
        /// <param name="userId">Уникальный индентификатор пользователя</param>
        /// <param name="roles">Роль пользователя</param>
        /// <returns></returns>
        string GenerateToken(Guid userId, List<string> roles);
    }
}
