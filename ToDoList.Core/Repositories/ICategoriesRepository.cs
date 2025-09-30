using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Core.Models;

namespace ToDoList.Core.Repositories
{
    public interface ICategoriesRepository:IBaseRepository<Category>
    {
        Task<IEnumerable<CategoryDto>>GetAllAsync();

        Task<bool> CategoryExistsAsync(int CategoryId);
    }
}
