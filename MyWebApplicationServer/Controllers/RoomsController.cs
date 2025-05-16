using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.DTOs.Lesson;
using MyWebApplicationServer.DTOs.Room;
using MyWebApplicationServer.Interfaces;
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
        private readonly IRoomRepository _roomRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="roomRepository"></param>
        public RoomsController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Получить все кабинеты
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRoom()
        {
            try
            {
                var allRooms = await _roomRepository.GetAllRooms();

                if(allRooms == null)
                {
                    return NotFound();
                }

                return Ok(allRooms);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Непредвиденная ошибка",
                    Details = ex.Message
                });
            }
        }
    }
}
