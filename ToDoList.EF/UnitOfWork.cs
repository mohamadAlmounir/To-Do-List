using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core;
using ToDoList.Core.Models;
using ToDoList.Core.Repositories;
using ToDoList.EF.Repositories;

namespace ToDoList.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ITasksRepository TaskItems { get; private set;}
        public ICategoriesRepository Categories { get; private set;}
        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Categories= new CategoriesRepository(_context);
            TaskItems= new TasksRepository(_context,_userManager);

        }
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // خطأ بسبب التزامن (سجل اتعدل أو انمسح من شخص آخر)
                Console.WriteLine($"Concurrency error: {ex.Message}");
                throw; // ممكن ترجع خطأ مخصص بدالك
            }
            catch (DbUpdateException ex)
            {
                // خطأ في قواعد البيانات (FK, Unique...)
                Console.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // أي خطأ غير متوقع
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
