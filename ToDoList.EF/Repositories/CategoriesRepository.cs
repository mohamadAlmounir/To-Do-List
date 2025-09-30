using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Dtos;
using ToDoList.Core.Models;
using ToDoList.Core.Repositories;

namespace ToDoList.EF.Repositories
{
    public class CategoriesRepository : BaseRepository<Category>, ICategoriesRepository
    {

        private readonly ApplicationDbContext _context;
        public CategoriesRepository(ApplicationDbContext context):base(context) 
        {
            
        }

        public async Task<bool> CategoryExistsAsync(int CategoryId)
        {
            return await base._context.Categories.AnyAsync(x=>x.Id==CategoryId);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var list = await base._context.Categories.ToListAsync();
            var dtoList = list.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
            return dtoList;
        }
    }
}
