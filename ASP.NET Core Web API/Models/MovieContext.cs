using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Web_API.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; } = null;
    }
}
