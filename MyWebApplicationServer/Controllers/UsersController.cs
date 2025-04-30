using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.FinalGrade;
using MyWebApplicationServer.DTO.Student;
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
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Password = u.Password,
                    Name = u.Name,
                    Login = u.Login,
                    InActive = u.InActive,
                    Roles = _context.UserRoles
                        .Where(ur => ur.UserId == u.UserId)
                        .Select(ur => ur.Role.RoleName)
                        .ToList()
                })
                .ToListAsync();
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Users.AnyAsync(u => u.Login == userDto.Login))
            {
                return BadRequest(new
                {
                    message = "Логин уже занят"
                });
            }

            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest(new
                {
                    message = "Email уже занят"
                });
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
                    if (userDto.ClassName == null)
                    {
                        return BadRequest("Для студента требуется ClassName");
                    }

                    var classExists = await _context.Class
                        .FirstOrDefaultAsync(c => c.Name == userDto.ClassName);

                    if (classExists == null)
                    {
                        return BadRequest("Указанный класс не существует");
                    }

                    var student = new Student
                    {
                        UserId = user.UserId,
                        ClassId = classExists.ClassId,
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

                if (userDto.Roles.Contains("Завуч"))
                {
                    return BadRequest("Вы не можете создать завуча");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Пользователь успешно создан"
                });
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

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="UpdateUserDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> UpdateUser(Guid userId, [FromBody] UpdateUserDto UpdateUserDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound($"Пользователь не найден");
            }

            if (UpdateUserDto.Login != null && await _context.Users.AnyAsync(u => u.Login == UpdateUserDto.Login && u.UserId != userId))
            {
                return BadRequest("Логин уже занят");
            }

            if (UpdateUserDto.Email != null && await _context.Users.AnyAsync(u => u.Email == UpdateUserDto.Email && u.UserId != userId))
            {
                return BadRequest("Email уже занят");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (UpdateUserDto.Email != null)
                {
                    user.Email = UpdateUserDto.Email;
                }

                if (UpdateUserDto.Name != null)
                {
                    user.Name = UpdateUserDto.Name;
                }

                if (UpdateUserDto.Login != null)
                {
                    user.Login = UpdateUserDto.Login;
                }

                if (UpdateUserDto.Password != null)
                {
                    user.Password = UpdateUserDto.Password;
                }


                if (UpdateUserDto.InActive.HasValue)
                {
                    user.InActive = UpdateUserDto.InActive.Value;
                }

                // Если нужны будут роли
                // UpdateRoleForQuery(userId, UpdateuUserDto);

                if (UpdateUserDto.ClassName != null)
                {
                    var student = await _context.Student.FirstOrDefaultAsync(s => s.UserId == user.UserId);
                    if (student != null)
                    {
                        var classExists = await _context.Class
                            .Where(c => c.Name == UpdateUserDto.ClassName)
                            .FirstOrDefaultAsync();

                        if (classExists == null)
                        {
                            return BadRequest("Указанный класс не существует");
                        }

                        student.ClassId = classExists.ClassId;
                        _context.Student.Update(student);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(user);
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

        //private async Task UpdateRoleForQuery(Guid userId, UpdateUserDto UpdateuUserDto)
        //{
        //    if (UpdateuUserDto.Roles != null)
        //    {
        //        var currentRoles = await _context.UserRoles
        //            .Where(ur => ur.UserId == userId)
        //            .ToListAsync();
        //        _context.UserRoles.RemoveRange(currentRoles);

        //        foreach (var roleName in UpdateuUserDto.Roles)
        //        {
        //            var role = await _context.Role
        //                .FirstOrDefaultAsync(r => r.RoleName == roleName);

        //            if (role == null)
        //            {
        //                return BadRequest($"Роль '{roleName}' не существует");
        //            }

        //            _context.UserRoles.Add(new UserRole
        //            {
        //                UserId = userId,
        //                RoleId = role.RoleId
        //            });
        //        }
        //    }

        //    if (UpdateuUserDto.Roles.Contains("Студент"))
        //    {
        //        if (!UpdateuUserDto.ClassId.HasValue)
        //        {
        //            return BadRequest("Для студента требуется ClassId");
        //        }

        //        var classExists = await _context.Class
        //            .AnyAsync(c => c.ClassId == UpdateuUserDto.ClassId.Value);

        //        if (!classExists)
        //        {
        //            return BadRequest("Указанный класс не существует");
        //        }

        //        var student = new Student
        //        {
        //            UserId = user.UserId,
        //            ClassId = UpdateuUserDto.ClassId.Value
        //        };

        //        _context.Student.Add(student);
        //    }
        //    else
        //    {
        //        var student = await _context.Student.FirstOrDefaultAsync(s => s.UserId == user.UserId);
        //        _context.Student.Remove(student);
        //    }

        //    if (UpdateuUserDto.Roles.Contains("Учитель"))
        //    {
        //        _context.Teacher.Add(new Teacher
        //        {
        //            UserId = user.UserId
        //        });
        //    }
        //}


        /// <summary>
        /// Удалить пользователя по его ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("Такого пользователя не существует");
            }

            var zavuch = await _context.Zavuch.FirstOrDefaultAsync(s => s.UserId == userId);

            if (zavuch != null)
            {
                return BadRequest("Завуча удалить нельзя!");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userRoles = await _context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync();
                _context.UserRoles.RemoveRange(userRoles);

                var student = await _context.Student.FirstOrDefaultAsync(s => s.UserId == userId);
                if (student != null)
                {
                    _context.Student.Remove(student);
                }

                var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.UserId == userId);
                if (teacher != null)
                {
                    _context.Teacher.Remove(teacher);
                }

                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Пользователь успешно удален"
                });
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

        /// <summary>
        /// Получить общую информацию о пользователе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GeneralInfo/{userId}")]
        public async Task<ActionResult<IEnumerable<UserGeneralInfoDto>>> GetGeneralInfoStudent(Guid userId)
        {
            var student = await _context.Student
                .Where(s => s.User.UserId == userId)
                .Select(s => s.Class.Name)
                .FirstOrDefaultAsync();

            var user = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserGeneralInfoDto
                {
                    Name = u.Name,
                    Email = u.Email,
                    ClassName = student,
                })
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}

