using devchat3.Models;
using Microsoft.EntityFrameworkCore;

namespace devchat3.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
