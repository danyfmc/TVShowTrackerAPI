using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTrackerAPI.Data;
using TvShowTrackerAPI.Models;

namespace TvShowTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly TvShowTrackerDbContext database;

        public ActorsController(TvShowTrackerDbContext database)
        {
            this.database = database;
        }

        // GET: api/Actors
        [HttpGet]
        public IActionResult GetActors()
        {
            var actors = database.Actors.ToList();
            return Ok(actors);
        }

        // GET: api/Actors/1
        [HttpGet("{id}")]
        public IActionResult GetActor(int id)
        {
            var actor = database.Actors.FirstOrDefault(a => a.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return Ok(actor);
        }

        // POST: api/Actors
        [HttpPost]
        public IActionResult CreateActor(Actor actor)
        {
            database.Actors.Add(actor);
            database.SaveChanges();
            return Ok(actor);
        }

        // PUT: api/Actors/1
        [HttpPut("{id}")]
        public IActionResult UpdateActor(int id, Actor actor)
        {
            var existingActor = database.Actors.Find(id);
            if (existingActor == null)
            {
                return NotFound();
            }

            existingActor.Name = actor.Name;
            database.SaveChanges();
            return Ok(existingActor);
        }

        // DELETE: api/Actors/1
        [HttpDelete("{id}")]
        public IActionResult DeleteActor(int id)
        {
            var actor = database.Actors.Find(id);
            if (actor == null)
            {
                return NotFound();
            }

            database.Actors.Remove(actor);
            database.SaveChanges();
            return Ok("Actor deleted");
        }
    }
}