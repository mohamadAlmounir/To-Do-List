using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoList.Core.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
