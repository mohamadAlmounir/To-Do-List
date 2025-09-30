using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Dtos
{
    public class TaskItemDto
    {
       
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
