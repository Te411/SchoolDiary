using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Controllers
{
    [Route("api/Class")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ClassesController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все классы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClass()
        {
            return await _context.Class.ToListAsync();
        }
    }
}
