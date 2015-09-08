namespace Lyra.Models.Format
{
    public abstract class FileFormat
    {
        public abstract string GetArtist();

        public abstract string GetAlbum();

        public abstract byte[] GetArtwork();

        public abstract int GetTrackNumber();

        public abstract string GetTitle();

        public abstract int GetDuration();

        public bool IsAvailableTags()
        {
            return this.GetTitle() != null && this.GetDuration() > 0;
        }
    }
}