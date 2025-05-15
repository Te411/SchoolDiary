using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.User;
using MyWebApplicationServer.Interfaces;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Repositories
{
    /// <summary>
    /// Репозиторий для пользователя
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(LibraryContext context) : base(context) { }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Password = u.Password,
                    Name = u.Name,
                    Login = u.Login,
                    ClassName = _context.Student
                        .Where(s => s.UserId == u.UserId)
                        .Select(s => s.Class.Name)
                        .FirstOrDefault(),
                    InActive = u.InActive,
                    Roles = _context.UserRoles
                        .Where(ur => ur.UserId == u.UserId)
                        .Select(ur => ur.Role.RoleName)
                        .ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Получить пользователя по Email
        /// </summary>
        /// <param name="email">почта</param>
        /// <returns></returns>
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns></returns>
        public async Task<User> GetByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        /// <summary>
        /// Проверка на существование пользователя по логину
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns></returns>
        public async Task<bool> UserExistsByLogin(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        /// <summary>
        /// Проверка на существование пользователя по почте
        /// </summary>
        /// <param name="email">Логин</param>
        /// <returns></returns>
        public async Task<bool> UserExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Добавить нового пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить сущность пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserWithRolesAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDto> AuthenticateAsync(LoginRequest request, IPasswordHasher hasher, IJwtService jwtService)
        {
            throw new NotImplementedException();
        }
    }
}
