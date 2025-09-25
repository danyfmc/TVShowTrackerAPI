using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTrackerAPI.Data;
using TvShowTrackerAPI.Models;

namespace TvShowTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TvShowTrackerDbContext database;

        public UsersController(TvShowTrackerDbContext database)
        {
            this.database = database;
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = database.Users.Include(u => u.FavoriteTvShows).ToList();
            return Ok(users);
        }

        // GET: api/Users/1
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = database.Users.Include(u => u.FavoriteTvShows).FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            database.Users.Add(user);
            database.SaveChanges();
            return Ok(user);
        }

        // PUT: api/Users/1
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var existingUser = database.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            database.SaveChanges();
            return Ok(existingUser);
        }

        // DELETE: api/Users/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = database.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            
            database.Users.Remove(user);
            database.SaveChanges();
            return Ok("User deleted");
        }

        // GET: api/Users/1/favorites
        [HttpGet("{userId}/favorites")]
        public IActionResult GetUserFavorites(int userId)
        {
            var user = database.Users.Include(u => u.FavoriteTvShows).ThenInclude(t => t.Actors).FirstOrDefault(u => u.Id == userId);
            
            if (user == null)
            {
                return NotFound("User not found");
            }
            
            return Ok(user.FavoriteTvShows);
        }

        // POST: api/Users/1/favorites/1
        [HttpPost("{userId}/favorites/{tvShowId}")]
        public IActionResult AddFavorite(int userId, int tvShowId)
        {
            var user = database.Users.Include(u => u.FavoriteTvShows).FirstOrDefault(u => u.Id == userId);
            var tvShow = database.TvShows.Find(tvShowId);
            
            if (user == null || tvShow == null)
            {
                return NotFound("User or TV Show not found");
            }

            // Check if already favorite
            bool alreadyFavorite = false;
            foreach (var favorite in user.FavoriteTvShows)
            {
                if (favorite.Id == tvShowId)
                {
                    alreadyFavorite = true;
                    break;
                }
            }

            if (alreadyFavorite)
            {
                return Conflict("TV Show is already in favorites");
            }

            user.FavoriteTvShows.Add(tvShow);
            database.SaveChanges();
            
            return Ok("TV Show added to favorites");
        }

        // DELETE: api/Users/1/favorites/1
        [HttpDelete("{userId}/favorites/{tvShowId}")]
        public IActionResult RemoveFavorite(int userId, int tvShowId)
        {
            var user = database.Users.Include(u => u.FavoriteTvShows).FirstOrDefault(u => u.Id == userId);
            
            if (user == null)
            {
                return NotFound("User not found");
            }

            TvShow favoriteToRemove = null;
            foreach (var favorite in user.FavoriteTvShows)
            {
                if (favorite.Id == tvShowId)
                {
                    favoriteToRemove = favorite;
                    break;
                }
            }

            if (favoriteToRemove == null)
            {
                return NotFound("TV Show not in favorites");
            }

            user.FavoriteTvShows.Remove(favoriteToRemove);
            database.SaveChanges();
            return Ok("TV Show removed from favorites");
        }
    }
}
