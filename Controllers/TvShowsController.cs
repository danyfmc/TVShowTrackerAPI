using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTrackerAPI.Data;
using TvShowTrackerAPI.Models;

namespace TvShowTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TvShowsController : ControllerBase
    {
        private readonly TvShowTrackerDbContext database;

        public TvShowsController(TvShowTrackerDbContext database)
        {
            this.database = database;
        }

        // GET: api/TvShows
        // You can filter by genre example: /api/TvShows?genre=Comedy
        // You can sort by title (alphabetically) example: /api/TvShows?sortBy=title

        [HttpGet]
        public IActionResult GetTvShows(string? genre = null, string? sortBy = null)
        {
            var query = database.TvShows.Include(t => t.Episodes).Include(t => t.Actors).AsQueryable();

            // Filter by genre if provided
            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(t => t.Genre.ToLower() == genre.ToLower());
            }

            // Sort if requested
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "title" => query.OrderBy(t => t.Title),
                    "genre" => query.OrderBy(t => t.Genre),
                    _ => query
                };
            }

            var tvShows = query.ToList();
            return Ok(tvShows);
        }

        // GET: api/TvShows/1
        [HttpGet("{id}")]
        public IActionResult GetTvShow(int id)
        {
            var tvShow = database.TvShows.Include(t => t.Episodes).Include(t => t.Actors).FirstOrDefault(t => t.Id == id);
            if (tvShow == null)
            {
                return NotFound();
            }
            return Ok(tvShow);
        }

        // POST: api/TvShows
        [HttpPost]
        public IActionResult CreateTvShow(TvShow tvShow)
        {
            database.TvShows.Add(tvShow);
            database.SaveChanges();
            return Ok(tvShow);
        }

        // PUT: api/TvShows/1
        [HttpPut("{id}")]
        public IActionResult UpdateTvShow(int id, TvShow tvShow)
        {
            var existingShow = database.TvShows.Find(id);
            if (existingShow == null)
            {
                return NotFound();
            }
            
            existingShow.Title = tvShow.Title;
            existingShow.Genre = tvShow.Genre;
            database.SaveChanges();
            return Ok(existingShow);
        }

        // DELETE: api/TvShows/1
        [HttpDelete("{id}")]
        public IActionResult DeleteTvShow(int id)
        {
            var tvShow = database.TvShows.Find(id);
            if (tvShow == null)
            {
                return NotFound();
            }
            
            database.TvShows.Remove(tvShow);
            database.SaveChanges();
            return Ok("TV Show deleted");
        }
    }
}