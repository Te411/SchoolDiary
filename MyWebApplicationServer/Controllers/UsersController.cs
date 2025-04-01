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

        public UsersController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// GET: api/Users/Login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("{login}")]
        public async Task<ActionResult<Users>> GetUserByLogin([FromRoute] string login)
        {
            var users = await _context.Users
                .FirstOrDefaultAsync(bg => bg.Login == login);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        [HttpPost("authenticate")]
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

            return Ok(new
            {
                Message = "Пользователь успешно аутентифицирован",
                Name = user.Name,
                Email = user.Email
            });
        }
    }
}
