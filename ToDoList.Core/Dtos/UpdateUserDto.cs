using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        public string Id { get; set; }
        [Required ,MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string UserName { get; set; }
        
    }
}
