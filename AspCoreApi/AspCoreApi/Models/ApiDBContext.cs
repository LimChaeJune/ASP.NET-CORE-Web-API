using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreApi.Models
{
    public class ApiDBContext : DbContext
    { 
        public ApiDBContext(DbContextOptions<ApiDBContext> options) : base(options)
        {

        }
        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
