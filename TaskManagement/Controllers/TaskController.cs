using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DAO;
using TaskManagement.Models.Models;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskDao _service;

        public TaskController(ITaskDao service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTree(Guid id)
        {
            var task = await _service.Get(id);
            return new OkObjectResult(task);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddMainTask(TaskModel task)
        {
            var updatedTask = await _service.Create(task);
            return new OkObjectResult(updatedTask);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskModel task)
        {
            var updatedTask = await _service.Update(task);
            return new OkObjectResult(updatedTask);
        }
        
    }
}