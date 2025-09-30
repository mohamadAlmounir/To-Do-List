using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Core.Models;
using ToDoList.Core.Repositories;

namespace ToDoList.EF.Repositories
{
    public class TasksRepository : BaseRepository<TaskItem>, ITasksRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TasksRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }


        public async Task<IEnumerable<TaskItem>> GetAllAsync(string UserId)
        {
            List<TaskItem> list = await base._context.TaskItems.Where(x=>x.UserId==UserId).ToListAsync();
            return list;
        }

        public async Task<TaskItem> AddAsync(TaskItemDto task)
        {
            //var userExists = await _userManager.FindByIdAsync(task.UserId) != null;
            //var categoryExists = await base._context.Categories.AnyAsync(d => d.Id == task.CategoryId);

            //if (!userExists || !categoryExists)
            //    return null;

            TaskItem newTask = new TaskItem
            {
                CategoryId = task.CategoryId,
                UserId = task.UserId,
                DueDate = task.DueDate,
                IsCompleted = false,
                Title = task.Title,
                Description = task.Description
            };

            await base._context.TaskItems.AddAsync(newTask);
            
            return newTask;
        }

        public async Task<TaskItem?> DeleteAsync(int id)
        {
            var existingTask = await base._context.TaskItems.FindAsync(id);
            if (existingTask == null)
                return null;

            base._context.TaskItems.Remove(existingTask);
            return existingTask;
        }

        public async Task<TaskItem?> UpdateAsync(int Id, TaskItemDto task)
        {
            var existingTask = await base._context.TaskItems.FindAsync(Id);
            if (existingTask == null)
                return null;

            //var categoryExists = await base._context.Categories.AnyAsync(d => d.Id == task.CategoryId);
            //var userExists = await _userManager.FindByIdAsync(task.UserId) != null;

            //if (!categoryExists || !userExists)
            //    return null;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.CategoryId = task.CategoryId;
            existingTask.IsCompleted = task.IsCompleted;

           
            return existingTask;
        }

        public async Task<IEnumerable<TaskItem>> GetAllByCategoryAsync(string UserId, int CategoryId)
        {
            List<TaskItem> list = await base._context.TaskItems.Where(x => x.UserId == UserId&&x.CategoryId==CategoryId).ToListAsync();
            return list;
        }

        public async Task<bool> TaskExistsAsync(int TaskId)
        {
            return await base._context.TaskItems.AnyAsync(x => x.Id == TaskId);
        }
    }
}
