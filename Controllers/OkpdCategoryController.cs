using Microsoft.AspNetCore.Mvc;
using SoftWizard.Models;
using SoftWizard.Services;

namespace SoftWizard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OkpdCategoryController(IUnitOfWork unitOfWork, ILogger<OkpdCategoryController> logger) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ILogger<OkpdCategoryController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.OkpdCategory.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.OkpdCategory.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OkpdCategory okpdCategory)
        {
            var data = await unitOfWork.OkpdCategory.AddAsync(okpdCategory);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.OkpdCategory.DeleteAsync(id);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(OkpdCategory okpdCategory)
        {
            var data = await unitOfWork.OkpdCategory.UpdateAsync(okpdCategory);
            return Ok(data);
        }

        [NonAction]
        public ObjectResult SetError(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
