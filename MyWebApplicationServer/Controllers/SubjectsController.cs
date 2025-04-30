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
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public SubjectsController(LibraryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все предметы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubject()
        {
            return await _context.Subject.ToListAsync();
        }
    }
}
