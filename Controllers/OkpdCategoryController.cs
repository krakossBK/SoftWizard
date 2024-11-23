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
        public ILogger<OkpdCategoryController> _logger = logger;

        [HttpGet("/api/okpd")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await unitOfWork.OkpdCategory.GetAllAsync());
        }

        [HttpGet("/api/okpd/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.OkpdCategory.GetByIdAsync(id);
            return Ok(data != null ? data : "");
        }

        [HttpPost("/api/add-okpd")]
        public async Task<IActionResult> Add(OkpdCategory okpdCategory)
        {
            return Ok(await unitOfWork.OkpdCategory.AddAsync(okpdCategory));
        }

        [HttpDelete("/api/delete-okpd")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await unitOfWork.OkpdCategory.DeleteAsync(id));
        }

        [HttpPut("/api/update-okpd")]
        public async Task<IActionResult> Update(OkpdCategory okpdCategory)
        {
            return Ok(await unitOfWork.OkpdCategory.UpdateAsync(okpdCategory));
        }

        [NonAction]
        public ObjectResult SetError(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
