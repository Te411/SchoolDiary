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
    /// Контроллер для таблицы "Роль"
    /// </summary>
    [Route("api/Role")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="roleRepository"></param>
        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Получить все роли
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var classes = await _roleRepository.GetAll();
            return Ok(classes);
        }

        /// <summary>
        /// Получить роль по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(Guid id)
        {
            var role = await _roleRepository.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        /// <summary>
        /// Добавить новую роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Authorize(Roles = "Завуч")]
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            await _roleRepository.Add(role);
            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }
    }
}
