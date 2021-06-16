using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TaskManagement.Models.DAO;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreeItemController
    {
        private readonly ITreeItemDao _service;
        private readonly IStringLocalizer<TreeItemController> _localizer; 

        public TreeItemController(ITreeItemDao service, IStringLocalizer<TreeItemController> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet("Tree")]
        public async Task<IActionResult> GetTree()
        {
            var root = await _service.GetRoot();
            return root == null
                ? new ObjectResult(new {error = _localizer["ControllerGetTreeError"] })
                : new OkObjectResult(root);
        }

        [HttpGet("Children")]
        public async Task<IActionResult> GetChildren(Guid id)
        {
            var root = await _service.GetChildren(id);
            return root == null
                ? new ObjectResult(new {error = _localizer["ControllerGetTreeChildrenError"] })
                : new OkObjectResult(root);
        }
    }
}