using TagLib;

namespace Lyra.Utilities
{
    /// <summary>
    /// Read tagging info from  *.mp3 files.
    /// </summary>
    public class Id3Tag
    {
        private readonly File _file;

        public Id3Tag(string path)
        {
            this._file = File.Create(path);
            // Debug.WriteLine($"Read a file {{{path}}}. Tagging by {this._file.TagTypes}");
        }

        public string GetArtist()
        {
            if (this._file.Tag.Performers.Length > 0)
                return string.Join(", ", this._file.Tag.Performers);
            if (this._file.Tag.AlbumArtists.Length > 0)
                return string.Join(", ", this._file.Tag.AlbumArtists);
            if (!string.IsNullOrWhiteSpace(this._file.Tag.JoinedAlbumArtists))
                return this._file.Tag.JoinedAlbumArtists;
            return null;
        }

        public string GetAlbum()
        {
            if (!string.IsNullOrWhiteSpace(this._file.Tag.Album))
                return this._file.Tag.Album;
            return null;
        }

        public byte[] GetArtwork()
        {
            if (this._file.Tag.Pictures.Length > 0)
                return this._file.Tag.Pictures[0].Data.Data;
            return null;
        }

        public int GetTrackNumber()
        {
            return (int)this._file.Tag.Track;
        }

        public string GetTitle()
        {
            if (!string.IsNullOrWhiteSpace(this._file.Tag.Title))
                return this._file.Tag.Title;
            return this._file.Name;
        }

        public int GetDuration()
        {
            return (int)(this._file.Properties.Duration.TotalSeconds * 1000);
        }
    }
}