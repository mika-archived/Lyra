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
        /// 指定した時間ごとに、Watcher&lt;T&gt;から呼び出されます。
        /// </summary>
        protected override void Tick()
        {
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
                        var id3Tag = new Id3Tag(file);
                        // Do not support Various Artists
                        int artistId;
                        if (id3Tag.GetArtist() != null)
                        {
                            var temp = id3Tag.GetArtist();
                            if (Database.Artists.Any(w => w.Name == temp))
                                artistId = Database.Artists.Single(w => w.Name == temp).Id;
                            else
                                artistId = Database.Artists.Add(new Artist { Name = temp }).Id;
                        }
                        else
                            artistId = LyraApp.DatabaseUnknownArtist;

                        int albumId;
                        if (id3Tag.GetAlbum() != null)
                        {
                            var temp = id3Tag.GetAlbum();
                            if (Database.Albums.Any(w => w.Title == temp))
                                albumId = Database.Albums.Single(w => w.Title == temp).Id;
                            else
                                albumId = Database.Albums.Add(new Album { Title = temp }).Id;
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
                        Database.Tracks.Add(track);
                        // avoid "SQLite error (5): database is locked"
                        Database.SaveChanges();

                        break;

                        #endregion
                }

                t = Database.Tracks.Include("Album").Include("Artist").SingleOrDefault(w => w.Path == file);
                if (t != null)
                    this.Items.Add(t);
            }

            // Tick finished
        }
    }
}