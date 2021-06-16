using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TaskManagement.Models.DAO;
using TaskManagement.Models.Models;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskDao _service;
        private readonly IStringLocalizer<TaskController> _localizer;

        public TaskController(ITaskDao service, IStringLocalizer<TaskController> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _service.Get(id);
            return task == null
                ? new ObjectResult(new {error = _localizer["ControllerGetTaskError"] })
                : new OkObjectResult(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskModel task)
        {
            var updatedTask = await _service.Create(task);
            return updatedTask == null
                ? new ObjectResult(new {error = _localizer["ControllerAddTaskError"] })
                : new OkObjectResult(updatedTask);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskModel task)
        {
            var updatedTask = await _service.Update(task);
            return updatedTask == null
                ? new ObjectResult(new {error = _localizer["ControllerUpdateTaskError"] })
                : new OkObjectResult(updatedTask);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask([FromBody] Guid id)
        {
            var status = await _service.Delete(id);
            return new OkObjectResult(status);
        }
    }
}