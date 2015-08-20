namespace Lyra.Models
{
    public class DummyTrack : Track
    {
        public DummyTrack()
        {
            this.Id = -1;
            this.Info = TrackInfo.Local;
            this.Number = 0;
            this.Title = "No Title";
            this.Artist = "No Title";
            this.Album = "No Title";
            this.Duration = 0;
            this.Jacket = null;
            this.Status = TrackState.Local;
            this.FilePath = null;
        }
    }
}