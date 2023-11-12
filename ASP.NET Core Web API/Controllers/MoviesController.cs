using ASP.NET_Core_Web_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly MovieContext _dbContext;

        public MoviesController(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if(_dbContext.Movies == null)
            {
                return NotFound();
            }
            return await _dbContext.Movies.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }
            var movie = await _dbContext.Movies.FindAsync(id);
            if(movie == null)
            {
                return NotFound();
            }
            return movie;
        }
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if( id != movie.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(movie).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) 
            {
                if(!MovieExists(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_dbContext.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }
            var movie = await _dbContext.Movies.FindAsync(id);
            if(movie == null)
            {
                return NotFound();
            }
            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
