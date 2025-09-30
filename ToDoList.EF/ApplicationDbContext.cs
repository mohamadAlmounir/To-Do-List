using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;
using Task = ToDoList.Core.Models.TaskItem;

namespace ToDoList.EF
{
    public  class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskItem>()
            .HasOne(t => t.User)
            .WithMany(u => u.TaskItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            // علاقة Category -> Tasks
            builder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Work" },
            new Category { Id = 2, Name = "Personal" },
            new Category { Id = 3, Name = "Shopping" },
            new Category { Id = 4, Name = "Health" },
            new Category { Id = 5, Name = "Learning" },
            new Category { Id = 6, Name = "Others" }
    );

        }

        public DbSet<TaskItem>TaskItems { get; set; }
        public DbSet<Category>Categories { get; set; }

    }
}
