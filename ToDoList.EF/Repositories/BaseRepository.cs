using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Repositories;

namespace ToDoList.EF.Repositories
{
    public class BaseRepository<T>:IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
    }
}
