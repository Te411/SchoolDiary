using MyWebApplicationServer.DTOs.User;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync();
        Task<User> GetByLoginAsync(string login);
        Task<User> GetByEmailAsync(string email);
        Task<bool> UserExistsByLogin(string login);
        Task<bool> UserExistsByEmail(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task UpdateUserWithRolesAsync(Guid userId, UpdateUserDto updateUserDto);
        Task DeleteAsync(Guid id);
        Task<AuthResponseDto> AuthenticateAsync(LoginRequest request, IPasswordHasher hasher, IJwtService jwtService);
    }
}
