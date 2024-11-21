using Microsoft.AspNetCore.Mvc;
using SoftWizard.Models;

namespace SoftWizard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository repo, ILogger<UsersController> logger) : ControllerBase
{
    private readonly IUserRepository _repo = repo;
    private readonly ILogger<UsersController> _logger = logger;


    // GET: api/users
    [HttpGet]
    public ActionResult<IEnumerable<User>>? GetUsers()
    {
        try
        {
            return _repo.GetUsers();

        }
        catch (Exception ex)
        {
            string expp = ex.Message;
            _logger.LogError(expp);
            return null;
        }

    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        User user = _repo.Get(id);
        if (user != null)
        {
            _logger.LogInformation(user.Id + " " + user.Email + " " + user.NameUser);
            return user;
        }

        return NotFound();
    }

    // POST: api/user-create
    [HttpPost]
    public ActionResult Post([FromBody] User user)
    {
        _repo.Create(user);
        return Ok();
    }

    // PUT: api/user-update/5
    [HttpPut]
    public ActionResult Put(int id, [FromBody] User user)
    {
        user.Id = id;
        _repo.Update(user);
        return Ok();
    }

    // DELETE: api/user-delete/5
    [HttpDelete]
    public ActionResult Delete(int id)
    {
        try
        {
            _repo.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            string expp = ex.Message;
            _logger.LogError(expp);
            return Ok();
        }
       
    }

    [NonAction]
    public ObjectResult SetError(Exception e)
    {
        return StatusCode(500, e.Message);
    }
}