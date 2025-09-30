using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Core;
using ToDoList.Core.Dtos;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public TasksController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("GetAllTasks/{id}")]
        public async Task<IActionResult> GetAllTasksAsync(string id)
        {
            if(await _userManager.FindByIdAsync(id)==null)
                return NotFound($"No tasks found for user with id {id}.");

            var tasksList = await _unitOfWork.TaskItems.GetAllAsync(id);

                return Ok(tasksList);

        }
        [HttpGet("GetAllTasksByCategory/{userId}/{categoryId}")]
        public async Task<IActionResult> GetAllTasksByCategoryAsync(string userId,int CategoryId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                return NotFound($"User with this {userId} is not existed");

            if (! await _unitOfWork.Categories.CategoryExistsAsync(CategoryId))
                return NotFound($"category with this {CategoryId} is not existed");
            var tasksList = await _unitOfWork.TaskItems.GetAllByCategoryAsync(userId,CategoryId);

            return Ok(tasksList);
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody]TaskItemDto task) 
        {
            if (await _userManager.FindByIdAsync(task.UserId) == null)
                return NotFound($"User with this {task.UserId} is not existed");
            if(! await _unitOfWork.Categories.CategoryExistsAsync(task.CategoryId))
                return NotFound($"category with this {task.CategoryId} is not existed");

            var newTask= await _unitOfWork.TaskItems.AddAsync(task);

            try
            {
                await _unitOfWork.CompleteAsync(); // كل try/catch موجودة جوّا
                return Ok(newTask);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"DB Error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }

        }

        [HttpPut("UpdateTask/{Id}")]

        public async Task<IActionResult> UpdateTask(int Id,[FromBody] TaskItemDto task)
        {
            if (!await _unitOfWork.TaskItems.TaskExistsAsync(Id))
                return NotFound($"Task with Id {Id} is not existed ");

            if (await _userManager.FindByIdAsync(task.UserId) == null)
                return NotFound($"User with this {task.UserId} is not existed");

            if (!await _unitOfWork.Categories.CategoryExistsAsync(task.CategoryId))
                return NotFound($"category with this {task.CategoryId} is not existed");

            var updatedTask = await _unitOfWork.TaskItems.UpdateAsync(Id, task);

            try
            {
                await _unitOfWork.CompleteAsync(); // كل try/catch موجودة جوّا
                return Ok(updatedTask);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"DB Error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }

        }

        [HttpDelete("DeleteTask/{Id}")]
        public async Task<IActionResult> DeleteTask(int Id)
        {
            var deletedTask= await _unitOfWork.TaskItems.DeleteAsync(Id);

            if (deletedTask == null)
                 return NotFound($"Task with Id {Id} is not existed ");

            try
            {
                await _unitOfWork.CompleteAsync(); // كل try/catch موجودة جوّا
                return Ok(deletedTask);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"DB Error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
    }
}
