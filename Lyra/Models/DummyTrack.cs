using System.ComponentModel.DataAnnotations.Schema;

namespace Lyra.Models
{
    [NotMapped]
    public sealed class DummyTrack : Track
    {
        public DummyTrack()
        {
            this.Id = -1;
            this.Number = 0;
            this.Title = "No Title";
            this.Artist = new Artist { Id = -1, Name = "No Title" };
            this.Album = new Album { Id = -1, Title = "No Title", Artwork = null };
            this.Duration = 0;
            this.Path = null;
        }
    }
}