using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DAO;

namespace TaskManagement.Controllers
{
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
            return new OkObjectResult(root);
        }
        
        [HttpGet("Children")]
        public async Task<IActionResult> GetChildren(Guid id)
        {
            var root = await _service.GetChildren(id);
            return new OkObjectResult(root);
        }
        
    }
}