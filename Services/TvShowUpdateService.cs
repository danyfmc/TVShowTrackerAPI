using TvShowTrackerAPI.Data;
using TvShowTrackerAPI.Models;
using System.Text.Json;

namespace TvShowTrackerAPI.Services
{
    public class TvShowUpdateService : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private ILogger<TvShowUpdateService> _logger;
        private IHttpClientFactory _httpClientFactory;

        public TvShowUpdateService(IServiceProvider serviceProvider, ILogger<TvShowUpdateService> logger, IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // This runs once when the app starts
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            FetchShowsFromApi();
            return Task.CompletedTask;
        }

        // Get shows from API and save to database
        private void FetchShowsFromApi()
        {
            _logger.LogInformation("Getting TV shows from API...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<TvShowTrackerDbContext>();

                // Step 1: Get data from API
                string apiData = GetApiData();

                // Step 2: Convert API data to our TV shows
                List<TvShow> newShows = ConvertApiDataToTvShows(apiData);

                // Step 3: Save to database
                SaveShowsToDatabase(database, newShows);
            }
        }

        // Call the API and get JSON data
        private string GetApiData()
        {
            var client = _httpClientFactory.CreateClient();
            string jsonData = "";

            try
            {
                string apiUrl = "https://www.episodate.com/api/most-popular?page=1";
                jsonData = client.GetStringAsync(apiUrl).Result;
                _logger.LogInformation("Got data from API");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling API: {ex.Message}");
            }

            return jsonData;
        }

        // Convert API JSON into own TvShow objects
        private List<TvShow> ConvertApiDataToTvShows(string jsonData)
        {
            List<TvShow> tvShows = new List<TvShow>();

            try
            {
                // Parse the JSON
                using (JsonDocument document = JsonDocument.Parse(jsonData))
                {
                    JsonElement root = document.RootElement;
                    JsonElement tvShowsArray = root.GetProperty("tv_shows");

                    foreach (JsonElement showElement in tvShowsArray.EnumerateArray())
                    {
                        // Extract data from API
                        string name = showElement.GetProperty("name").GetString();

                        // Convert to our TvShow object
                        TvShow tvShow = new TvShow();
                        tvShow.Title = name;
                       

                        tvShows.Add(tvShow);

                        // Implement a limit
                        if (tvShows.Count >= 5)
                            break;
                    }
                }

                _logger.LogInformation($"Converted {tvShows.Count} shows from API");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error converting API data: {ex.Message}");
            }

            return tvShows;
        }

        // Verify if a TV show already exists in the database
        private bool IsTvShowAlreadyInDatabase(TvShowTrackerDbContext database, string title)
        {
            try
            {
                // Check by title only (case-insensitive) - using imperative style
                List<TvShow> existingShows = database.TvShows.ToList();
                for (int i = 0; i < existingShows.Count; i++)
                {
                    if (existingShows[i].Title.ToLower() == title.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking if TV show exists: {ex.Message}");
                return false;
            }
        }

        // Save the TV shows to database
        private void SaveShowsToDatabase(TvShowTrackerDbContext database, List<TvShow> newShows)
        {
            for (int i = 0; i < newShows.Count; i++)
            {
                TvShow newShow = newShows[i];
                
                // Check if the show already exists before adding
                if (!IsTvShowAlreadyInDatabase(database, newShow.Title))
                {
                    database.TvShows.Add(newShow);
                    _logger.LogInformation($"Adding: {newShow.Title}");
                }
                else
                {
                    _logger.LogInformation($"Skipping duplicate: {newShow.Title}");
                }
            }
            
            // Save all changes at once instead of per show
            database.SaveChanges();
        }
    }
}