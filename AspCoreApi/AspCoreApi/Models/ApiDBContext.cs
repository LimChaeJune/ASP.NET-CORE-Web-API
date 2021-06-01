using Microsoft.EntityFrameworkCore;

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
