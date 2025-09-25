namespace TvShowTrackerAPI.Models
{
    public class TvShow
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        public List<Actor> Actors { get; set; } = new List<Actor>();
    }
}