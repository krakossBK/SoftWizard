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
            try
            {
                var data = await unitOfWork.OkpdCategory.GetAllAsync();
                return Ok(data);

            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return Ok("");
            }

        }

        [HttpGet("/api/okpd/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {

                var data = await unitOfWork.OkpdCategory.GetByIdAsync(id);
                var dataId = data.Id;

                return Ok(data != null ? data : "");
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return Ok("");
            }
        }

        [HttpPost("/api/add-okpd")]
        public async Task<IActionResult> Add(OkpdCategory okpdCategory)
        {
            try
            {

                var data = await unitOfWork.OkpdCategory.AddAsync(okpdCategory);
                return Ok(data);
            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return Ok("");
            }
        }

        [HttpDelete("/api/delete-okpd")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var data = await unitOfWork.OkpdCategory.DeleteAsync(id);
                return Ok(data);

            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return Ok("");
            }
        }

        [HttpPut("/api/update-okpd")]
        public async Task<IActionResult> Update(OkpdCategory okpdCategory)
        {
            try
            {
                var data = await unitOfWork.OkpdCategory.UpdateAsync(okpdCategory);
                return Ok(data);

            }
            catch (Exception ex)
            {
                string expp = ex.Message;
                _logger.LogError(expp);
                return Ok("");
            }
        }

        [NonAction]
        public ObjectResult SetError(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
