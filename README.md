# TV Show Tracker API

A REST API for managing your favorite TV shows, built with ASP.NET Core and Entity Framework.

## Features

- Browse and search TV shows
- Manage show details (create, read, update, delete)
- Track your favorite shows
- Filter shows by genre
- Sort shows alphabetically or by genre
- Automatic show data seeding from external API

## Quick Start

1. Clone the repository
2. Run `dotnet run` 
3. Navigate to http://localhost:5291/api/tvshows

The SQLite database will be created automatically on first run.

## API Endpoints

### TV Shows
- `GET /api/tvshows` - Get all shows
- `GET /api/tvshows/{id}` - Get specific show
- `POST /api/tvshows` - Create new show
- `PUT /api/tvshows/{id}` - Update show
- `DELETE /api/tvshows/{id}` - Delete show

### Users & Favorites  
- `GET /api/users/{id}/favorites` - Get user's favorite shows
- `POST /api/users/{id}/favorites/{showId}` - Add show to favorites
- `DELETE /api/users/{id}/favorites/{showId}` - Remove from favorites

### Episodes & Actors
- `GET /api/episodes` - Get all episodes
- `GET /api/actors` - Get all actors

## Query Parameters

Filter by genre: `/api/tvshows?genre=Comedy`
Sort results: `/api/tvshows?sortBy=title`

## Tech Stack

- ASP.NET Core 8.0
- Entity Framework Core
- SQLite
