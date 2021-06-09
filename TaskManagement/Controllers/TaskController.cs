using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskService _service;

        public TaskController(TaskService service)
        {
            _service = service;
        }

        // [HttpGet]
        // public async Task<ActionResult<List<TreeItemModel>> GetTree()
        // {
        //     
        // }
        
    }
}