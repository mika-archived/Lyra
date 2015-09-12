namespace Lyra.Models.Format
{
    public abstract class FileFormat
    {
        protected string Path { get; }

        protected FileFormat()
        {
            this.Path = null;
        }

        protected FileFormat(string path)
        {
            this.Path = path;
        }

        public abstract string GetArtist();

        public abstract string GetAlbum();

        public abstract byte[] GetArtwork();

        public abstract int GetTrackNumber();

        public abstract string GetTitle();

        public abstract int GetDuration();

        public bool IsAvailableTags()
        {
            return this.GetTitle() != null && this.GetDuration() > 0 && this.GetTrackNumber() != 0;
        }
    }
}