using Microsoft.EntityFrameworkCore;
using System;
using TodoList.Api.Entities;

namespace TodoList.Api.DbContexts
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            var ss = options;
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
