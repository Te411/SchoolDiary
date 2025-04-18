using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<ActionResult> AuthenticateUser([FromBody] LoginRequest request)
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

            return Ok(new
            {
                Message = "Пользователь успешно аутентифицирован",
                UserId = user.UserId,
                classId = classId
            });
        }
    }
}
