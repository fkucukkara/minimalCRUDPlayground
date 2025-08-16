using Microsoft.EntityFrameworkCore;

namespace minimalCRUDPlayground;

public class TodoDBContext : DbContext
{
    public TodoDBContext(DbContextOptions<TodoDBContext> options) : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; } = null!;
}