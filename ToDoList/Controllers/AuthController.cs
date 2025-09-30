using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Core.Dtos;
using ToDoList.Core.Serveces;
using ToDoList.EF.Serveces;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IAuthService _authServes;
        public AuthController(IAuthService authServes)
        {
            _authServes = authServes;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServes.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authServes.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        //[Authorize(Roles ="Admin")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authServes.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(model);

        }
    }
}
