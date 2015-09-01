using System.IO;
using System.Linq;

using Lyra.Utilities;

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

                // ID3タグ、ディレクトリパスやファイル名などから、楽曲名を推測
                // それでもダメなら GracenoteとかFreeDBになげる
                var ext = System.IO.Path.GetExtension(file);

                switch (ext)
                {
                    #region *.mp3

                    case ".mp3":
                        this.InsertMp3(file);
                        break;

                        #endregion
                }

                t = Database.Tracks.Include("Album").Include("Artist").SingleOrDefault(w => w.Path == file);
                if (t != null)
                    this.Items.Add(t);
            }

            // Tick finished
        }

        private void InsertMp3(string file)
        {
            var id3Tag = new Id3Tag(file);
            int artistId;
            if (id3Tag.GetArtist() != null)
            {
                var temp = id3Tag.GetArtist();
                if (this.Database.Artists.Any(w => w.Name == temp))
                    artistId = this.Database.Artists.Single(w => w.Name == temp).Id;
                else
                    artistId = this.Database.Artists.Add(new Artist { Name = temp }).Id;
            }
            else
                artistId = LyraApp.DatabaseUnknownArtist;

            int albumId;
            if (id3Tag.GetAlbum() != null)
            {
                var temp = id3Tag.GetAlbum();
                if (this.Database.Albums.Any(w => w.Title == temp))
                    albumId = this.Database.Albums.Single(w => w.Title == temp).Id;
                else
                    albumId = this.Database.Albums.Add(new Album { Title = temp, Artwork = id3Tag.GetArtwork() }).Id;
            }
            else
                albumId = LyraApp.DatabaseUnknownAlbum;

            var track = new Track
            {
                Path = file,
                Number = id3Tag.GetTrackNumber(),
                Title = id3Tag.GetTitle(),
                ArtistId = artistId,
                AlbumId = albumId,
                Duration = id3Tag.GetDuration()
            };

            this.Database.Tracks.Add(track);
            this.Database.SaveChanges();
        }
    }
}