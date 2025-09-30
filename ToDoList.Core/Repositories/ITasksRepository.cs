using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Core.Models;

namespace ToDoList.Core.Repositories
{
    public interface ITasksRepository:IBaseRepository<TaskItem>
    {
        Task<bool> TaskExistsAsync(int TaskId);
        Task<IEnumerable<TaskItem>> GetAllAsync(string UserId);
        Task<IEnumerable<TaskItem>> GetAllByCategoryAsync(string UserId,int CategoryId);
        Task<TaskItem> AddAsync(TaskItemDto task);
        
        Task<TaskItem> UpdateAsync(int Id,TaskItemDto task);
        Task<TaskItem> DeleteAsync(int Id);
    }
}
