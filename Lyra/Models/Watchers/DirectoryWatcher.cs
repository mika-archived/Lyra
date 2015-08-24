using System.IO;
using System.Linq;

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
                var t = this.Database.Tracks.Include("Artist").Include("Album").SingleOrDefault(w => w.Path == file);
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
                        var tag = TagLib.File.Create(file);
                        // Do not support Various Artists
                        int artistId;
                        if (tag.Tag.AlbumArtists.Length > 0)
                        {
                            // タグあり
                            var temp = tag.Tag.AlbumArtists[0];
                            if (this.Database.Artists.Any(w => w.Name == temp))
                                artistId = this.Database.Artists.Single(w => w.Name == temp).Id;
                            else
                                artistId = this.Database.Artists.Add(new Artist { Name = temp }).Id;
                        }
                        else
                            artistId = LyraApp.DatabaseUnknownArtist;

                        int albumId;
                        if (tag.Tag.Album != null)
                        {
                            if (this.Database.Albums.Any(w => w.Title == tag.Tag.Album))
                                albumId = this.Database.Albums.Single(w => w.Title == tag.Tag.Album).Id;
                            else
                                albumId = this.Database.Albums.Add(new Album { Title = tag.Tag.Album }).Id;
                        }
                        else
                            albumId = LyraApp.DatabaseUnknownAlbum;

                        var track = new Track
                        {
                            Path = file,
                            Number = (int)tag.Tag.Track,
                            Title = tag.Tag.Title,
                            ArtistId = artistId,
                            AlbumId = albumId,
                            Duration = (int)(tag.Properties.Duration.TotalSeconds * 1000)
                        };
                        this.Database.Tracks.Add(track);
                        break;

                        #endregion
                }

                t = this.Database.Tracks.Include("Album").Include("Artist").SingleOrDefault(w => w.Path == file);
                if (t != null)
                    this.Items.Add(t);

                this.Database.SaveChanges();
            }
        }
    }
}