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
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    /// <summary>
    /// Контроллер для таблицы "Предмет"
    /// </summary>
    [Route("api/Subjects")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public SubjectsController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        /// <summary>
        /// Получить все предметы
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubject()
        {
            return Ok(await _subjectRepository.GetAll());
        }
    }
}
