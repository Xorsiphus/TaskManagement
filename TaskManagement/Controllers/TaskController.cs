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
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _service.Get(id);
            if (task == null)
                return new ObjectResult(new { error = "Невозможно получить объект!" });
            return new OkObjectResult(task);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddTask(TaskModel task)
        {
            var updatedTask = await _service.Create(task);
            if (updatedTask == null)
                return new ObjectResult(new { error = "Невозможно создать объект!" });
            return new OkObjectResult(updatedTask);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskModel task)
        {
            var updatedTask = await _service.Update(task);
            if (updatedTask == null)
                return new ObjectResult(new { error = "Невозможно обновить статус!" });
            return new OkObjectResult(updatedTask);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteTask([FromBody]Guid id)
        {
            var status = await _service.Delete(id);
            return new OkObjectResult(status);
        }
        
    }
}