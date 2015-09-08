using TagLib;

namespace Lyra.Models.Format
{
    /// <summary>
    /// Read tagging info.
    /// See also: https://github.com/mono/taglib-sharp/tree/master/src/TagLib
    /// </summary>
    public class TagReader : FileFormat
    {
        private readonly File _file;

        public TagReader(string path)
        {
            this._file = File.Create(path);
            // Debug.WriteLine($"Read a file {{{path}}}. Tagging by {this._file.TagTypes}");
        }

        public override string GetArtist()
        {
            if (this._file.Tag.Performers.Length > 0)
                return string.Join(", ", this._file.Tag.Performers);
            if (this._file.Tag.AlbumArtists.Length > 0)
                return string.Join(", ", this._file.Tag.AlbumArtists);
            if (!string.IsNullOrWhiteSpace(this._file.Tag.JoinedAlbumArtists))
                return this._file.Tag.JoinedAlbumArtists;
            return null;
        }

        public override string GetAlbum()
        {
            if (!string.IsNullOrWhiteSpace(this._file.Tag.Album))
                return this._file.Tag.Album;
            return null;
        }

        public override byte[] GetArtwork()
        {
            if (this._file.Tag.Pictures.Length > 0)
                return this._file.Tag.Pictures[0].Data.Data;
            return null;
        }

        public override int GetTrackNumber()
        {
            return (int)this._file.Tag.Track;
        }

        public override string GetTitle()
        {
            if (!string.IsNullOrWhiteSpace(this._file.Tag.Title))
                return this._file.Tag.Title;
            return this._file.Name;
        }

        public override int GetDuration()
        {
            if (this._file.Properties == null)
                return -1;
            return (int)(this._file.Properties.Duration.TotalSeconds * 1000);
        }
    }
}