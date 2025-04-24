using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.User;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly LibraryContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(LibraryContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/Users
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="request">Запрос на авторизацию с учетными данными пользователя.</param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> AuthenticateUser([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Логин и пароль обязательны");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == request.Login && u.Password == request.Password);

            if (user == null)
            {
                return NotFound("Пользователь с указанными логином и паролем не найден");
            }

            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Join(_context.Role,
                    ur => ur.RoleId,
                    r => r.RoleId,
                    (ur, r) => r.RoleName)
                .ToListAsync();

            user.InActive = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var student = await _context.Student
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.UserId == user.UserId);

            Guid? classId = null;
            if (student != null)
            {
                classId = student.ClassId;
            }

            return Ok(new AuthResponseDto
            {
                Message = "Пользователь успешно аутентифицирован",
                UserId = user.UserId,
                ClassId = classId,
                Roles = roles
            });
        }


        /// <summary>
        /// Добавление новых пользователй
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("CreateNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Login == userDto.Login))
            {
                return BadRequest("Логин уже занят");
            }

            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest("Email уже занят");
            }

            var user = new User
            {
                Email = userDto.Email,
                Password = userDto.Password,
                Name = userDto.Name,
                Login = userDto.Login,
                InActive = false
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                foreach (var roleName in userDto.Roles)
                {
                    var role = await _context.Role
                        .FirstOrDefaultAsync(r => r.RoleName == roleName);

                    if (role == null)
                    {
                        return BadRequest($"Роль '{roleName}' не существует");
                    }

                    _context.UserRoles.Add(new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = role.RoleId
                    });
                }

                if (userDto.Roles.Contains("Студент"))
                {
                    if (!userDto.ClassId.HasValue)
                    {
                        return BadRequest("Для студента требуется ClassId");
                    }
                    var classExists = await _context.Class
                        .AnyAsync(c => c.ClassId == userDto.ClassId.Value);

                    if (!classExists)
                    {
                        return BadRequest("Указанный класс не существует");
                    }

                    var student = new Student
                    {
                        UserId = user.UserId,
                        ClassId = userDto.ClassId.Value
                    };

                    _context.Student.Add(student);
                }

                if (userDto.Roles.Contains("Учитель"))
                {
                    _context.Teacher.Add(new Teacher
                    {
                        UserId = user.UserId
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetUsers), user);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ошибка при добавлении пользователя.",
                    Details = dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Непредвиденная ошибка!",
                    Details = ex.Message
                });
            }
        }
    }
}
