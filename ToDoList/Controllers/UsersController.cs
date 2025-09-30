using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Core.Dtos;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser>_userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with id {id} is not existed");
            }

            if (user.UserName == User.Identity?.Name)
            {
                return BadRequest("Admin cannot delete their own account.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok($"User with Id {id} has been deleted successfully.");
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var currentUserId = User.IsInRole("Admin")
                ? userDto.Id
                : User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("You are not authorized to update this user.");

            var existingUser = await _userManager.FindByIdAsync(currentUserId);
            if (existingUser == null)
                return NotFound($"User with id {currentUserId} does not exist.");

         
            var emailOwner = await _userManager.FindByEmailAsync(userDto.Email);
            if (emailOwner != null && emailOwner.Id != existingUser.Id)
                return BadRequest("This email is already taken by another user.");

           
            var userNameOwner = await _userManager.FindByNameAsync(userDto.UserName);
            if (userNameOwner != null && userNameOwner.Id != existingUser.Id)
                return BadRequest("This username is already taken by another user.");

            
            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;

            var setEmailResult = await _userManager.SetEmailAsync(existingUser, userDto.Email);
            if (!setEmailResult.Succeeded)
                return BadRequest(setEmailResult.Errors);

            var setUserNameResult = await _userManager.SetUserNameAsync(existingUser, userDto.UserName);
            if (!setUserNameResult.Succeeded)
                return BadRequest(setUserNameResult.Errors);

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            return Ok(new
            {
                Message = "User updated successfully.",
                User = new
                {
                    existingUser.Id,
                    existingUser.FirstName,
                    existingUser.LastName,
                    existingUser.Email,
                    existingUser.UserName
                }
            });
        }
    }
}
