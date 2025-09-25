using Microsoft.EntityFrameworkCore;
using TvShowTrackerAPI.Models;

namespace TvShowTrackerAPI.Data
{
    public class TvShowTrackerDbContext : DbContext
    {
        public TvShowTrackerDbContext(DbContextOptions<TvShowTrackerDbContext> options) : base(options) 
        { 
        }

        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TvShow>()
                .HasMany(t => t.Actors)
                .WithMany()
                .UsingEntity(j => j.ToTable("TvShowActors")); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteTvShows)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFavoriteTvShows")); 

            // Seed TV Shows
            modelBuilder.Entity<TvShow>().HasData(
                new TvShow { Id = 1, Title = "Breaking Bad", Genre = "Crime" },
                new TvShow { Id = 2, Title = "Game of Thrones", Genre = "Fantasy" },
                new TvShow { Id = 3, Title = "Stranger Things", Genre = "Science Fiction" },
                new TvShow { Id = 4, Title = "Friends", Genre = "Comedy" },
                new TvShow { Id = 5, Title = "The Office", Genre = "Comedy" }
            );

            // Seed Actors
            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Bryan Cranston" },
                new Actor { Id = 2, Name = "Emilia Clarke" },
                new Actor { Id = 3, Name = "Millie Bobby Brown" },
                new Actor { Id = 4, Name = "Jennifer Aniston" },
                new Actor { Id = 5, Name = "Steve Carell" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "Regina", Email = "regina@gmail.com" },
                new User { Id = 2, UserName = "Carolina", Email = "carolina@gmail.com" },
                new User { Id = 3, UserName = "Daniel", Email = "daniel@hotmail.com" }
            );

            // Seed Episodes
            modelBuilder.Entity<Episode>().HasData(
                new Episode { Id = 1, Title = "Pilot", ReleaseDate = new DateTime(2008, 1, 20), TvShowId = 1 },
                new Episode { Id = 2, Title = "Winter Is Coming", ReleaseDate = new DateTime(2011, 4, 17), TvShowId = 2 }
            );

            // Seed the many-to-many relationships between TvShows and Actors
            // The correct property names are based on the navigation property names
            modelBuilder.Entity("ActorTvShow").HasData(
                new { ActorsId = 1, TvShowId = 1 }, // Bryan Cranston in Breaking Bad
                new { ActorsId = 2, TvShowId = 2 }, // Emilia Clarke in Game of Thrones
                new { ActorsId = 3, TvShowId = 3 }, // Millie Bobby Brown in Stranger Things
                new { ActorsId = 4, TvShowId = 4 }, // Jennifer Aniston in Friends
                new { ActorsId = 5, TvShowId = 5 }  // Steve Carell in The Office
            );

            // Seed the many-to-many relationships between Users and their favorite TvShows
            modelBuilder.Entity("TvShowUser").HasData(
                new { FavoriteTvShowsId = 1, UserId = 1 }, // Regina likes Breaking Bad
                new { FavoriteTvShowsId = 4, UserId = 1 }, // Regina likes Friends
                new { FavoriteTvShowsId = 2, UserId = 2 }, // Carolina likes Game of Thrones
                new { FavoriteTvShowsId = 3, UserId = 3 }  // Daniel likes Stranger Things
            );
        }
    }
}