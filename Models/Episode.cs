namespace TvShowTrackerAPI.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int TvShowId { get; set; }
    }
}