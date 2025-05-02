using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTO.Lesson;
using MyWebApplicationServer.DTO.Room;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Кабинет"
    /// </summary>
    [Route("api/Room")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly LibraryContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public RoomsController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все кабинеты
        /// </summary>
        /// <returns></returns>
        [HttpGet("Room")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRoom()
        {
            return await _context.Room
                .Select(l => new RoomDto
                {
                    Name = l.Name,
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
