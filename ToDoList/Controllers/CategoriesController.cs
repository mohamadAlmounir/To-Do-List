using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using ToDoList.Core;
using ToDoList.Core.Dtos;

namespace ToDoList.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _categoryUniOfWork;
        public CategoriesController(IUnitOfWork categoryUniOfWork)
        {
            _categoryUniOfWork=categoryUniOfWork;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllAsync() 
        {
            var list = await _categoryUniOfWork.Categories.GetAllAsync();

            
            if(list != null)
                return Ok(list);
            return NotFound("No Categories available");


        }
    }
}
