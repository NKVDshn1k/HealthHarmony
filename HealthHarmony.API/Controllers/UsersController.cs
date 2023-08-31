using HealthHarmony.API.Entity;
using HealthHarmony.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthHarmony.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IRepository<User> _repository;

        public UsersController(IRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<User>>> Get() =>
            Ok(await _repository.GetAsync());

        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(int id) =>
            Ok(await _repository.GetAsync(id));

        [HttpPost("add")]
        public async Task<ActionResult<User>> Add(User user)
        {
            await _repository.AddAsync(user);
            return Ok(user);
        }


        [HttpPut("edit")]
        public async Task<ActionResult<User>> Edit(User user)
        {
            if (user == null)
                return BadRequest();
            
            if (!_repository.Items.Any(x => x.Id == user.Id))
                return NotFound();

            await _repository.UpdateAsync(user);
            return Ok(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.RemoveAsync(id);
            return Ok();
        }
    }
}
