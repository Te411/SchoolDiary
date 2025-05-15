using Microsoft.AspNetCore.Mvc;
using Project.MyWebApplicationServer.Models;

namespace MyWebApplicationServer.Interfaces
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAll();
    }
}
