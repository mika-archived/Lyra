using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Lyra.Models.Format;

namespace Lyra.Models.Watchers
{
    /// <summary>
    /// 10秒ごとにディレクトリを監視し、その変更を DB 及び MainWindow に適用/通知します。
    /// </summary>
    public class DirectoryWatcher : Watcher
    {
        public DirectoryWatcher(string path) : base(path, 1000 * 10)
        {
        }

        /// <summary>
        /// 指定した時間ごとに、Watcherから呼び出されます。
        /// </summary>
        protected override void Tick()
        {
            if (!Directory.Exists(this.Path))
                return;

            var files = Directory.GetFiles(this.Path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var t = Database.Tracks.Include("Artist").Include("Album").SingleOrDefault(w => w.Path == file);
                if (t != null)
                {
                    if (this.Items.Contains(t))
                        continue;
                    this.Items.Add(t);
                    continue;
                }

                var ext = System.IO.Path.GetExtension(file);
                FileFormat ff = null;

                switch (ext)
                {
                    case ".mp3":
                        ff = new Mp3(file);
                        break;
                }

                // Do not support
                if (ff == null)
                    continue;

                if (ff.IsAvailableTags())
                    this.Insert(ff, file);
                else
                    this.Insert(file);

                t = Database.Tracks.Include("Album").Include("Artist").SingleOrDefault(w => w.Path == file);
                if (t != null)
                    this.Items.Add(t);
            }

            // Tick finished
        }

        private void Insert(FileFormat iif, string file)
        {
            int artistId;
            if (iif.GetArtist() != null)
            {
                var temp = iif.GetArtist();
                if (this.Database.Artists.Any(w => w.Name == temp))
                    artistId = this.Database.Artists.Single(w => w.Name == temp).Id;
                else
                    artistId = this.Database.Artists.Add(new Artist { Name = temp }).Id;
            }
            else
                artistId = LyraApp.DatabaseUnknownArtist;

            int albumId;
            if (iif.GetAlbum() != null)
            {
                var temp = iif.GetAlbum();
                if (this.Database.Albums.Any(w => w.Title == temp))
                    albumId = this.Database.Albums.Single(w => w.Title == temp).Id;
                else
                    albumId = this.Database.Albums.Add(new Album { Title = temp, Artwork = iif.GetArtwork() }).Id;
            }
            else
                albumId = LyraApp.DatabaseUnknownAlbum;

            var track = new Track
            {
                Path = file,
                Number = iif.GetTrackNumber(),
                Title = iif.GetTitle(),
                ArtistId = artistId,
                AlbumId = albumId,
                Duration = iif.GetDuration()
            };

            this.Database.Tracks.Add(track);
            this.Database.SaveChanges();
        }

        // タグから取れなかったので、ディレクトリ名などから取得を試みる。
        private void Insert(string file)
        {
            // ~\{Artist}\{Album}\{Track}-{Title}.{ext}
            var regex = new Regex(@"\\(?<artist>[\w\s]*)\\(?<album>[\w\s]*)\\(?<track>\d*)-(?<title>.*)\.(?<ext>.*)$");
        }
    }
}