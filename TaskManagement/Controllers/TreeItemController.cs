using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DAO;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreeItemController
    {
        private readonly ITreeItemDao _service;

        public TreeItemController(ITreeItemDao service)
        {
            _service = service;
        }

        [HttpGet("Tree")]
        public async Task<IActionResult> GetTree()
        {
            var root = await _service.GetRoot();
            return root == null
                ? new ObjectResult(new {error = "Не удалось загузить список!"})
                : new OkObjectResult(root);
        }

        [HttpGet("Children")]
        public async Task<IActionResult> GetChildren(Guid id)
        {
            var root = await _service.GetChildren(id);
            return root == null
                ? new ObjectResult(new {error = "Не удалось загузить подзадачи!"})
                : new OkObjectResult(root);
        }
    }
}