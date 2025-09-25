using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTrackerAPI.Data;
using TvShowTrackerAPI.Models;

namespace TvShowTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : ControllerBase
    {
        private readonly TvShowTrackerDbContext database;

        public EpisodesController(TvShowTrackerDbContext context)
        {
            database = context;
        }

        // GET: api/Episodes
        [HttpGet]
        public IActionResult GetAllEpisodes()
        {
            var episodes = database.Episodes.ToList();
            return Ok(episodes);
        }

        // GET: api/Episodes/1
        [HttpGet("{id}")]
        public IActionResult GetEpisode(int id)
        {
            var episode = database.Episodes.Find(id);
            if (episode == null)
            {
                return NotFound();
            }
            return Ok(episode);
        }


        // POST: api/Episodes
        [HttpPost]
        public IActionResult CreateEpisode(Episode episode)
        {
            database.Episodes.Add(episode);
            database.SaveChanges();
            return Ok(episode);
        }

        // PUT: api/Episodes/5
        [HttpPut("{id}")]
        public IActionResult UpdateEpisode(int id, Episode episode)
        {
            var existingEpisode = database.Episodes.Find(id);
            if (existingEpisode == null)
            {
                return NotFound();
            }

            existingEpisode.Title = episode.Title;
            existingEpisode.ReleaseDate = episode.ReleaseDate;
            existingEpisode.TvShowId = episode.TvShowId;
            database.SaveChanges();
            return Ok(existingEpisode);
        }

        // DELETE: api/Episodes/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEpisode(int id)
        {
            var episode = database.Episodes.Find(id);
            if (episode == null)
            {
                return NotFound();
            }

            database.Episodes.Remove(episode);
            database.SaveChanges();
            return Ok("Episode deleted");
        }
    }
}