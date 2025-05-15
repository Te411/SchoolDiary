using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using MyWebApplicationServer.Interfaces;
using MyWebApplicationServer.Repositories;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Класс"
    /// </summary>
    [Route("api/Class")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _classRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="classRepository"></param>
        public ClassesController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        /// <summary>
        /// Получить все классы
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClass()
        {
            var classes = await _classRepository.GetAll();
            return Ok(classes);
        }
    }
}
