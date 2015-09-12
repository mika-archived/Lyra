using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Lyra.Extensions;
using Lyra.Models.Format;

namespace Lyra.Models.Watchers
{
    /// <summary>
    /// 10秒ごとにディレクトリを監視し、その変更を DB 及び MainWindow に適用/通知します。
    /// </summary>
    public class DirectoryWatcher : Watcher
    {
        // 置き換えよう正規表現
        private static Regex ReplaceChars => new Regex(@"[\p{M}\p{P}\p{S}\p{Z}\p{S}]*");

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
                    case ".3gp":
                        ff = new Audio3Gp(file);
                        break;

                    case ".asf":
                    case ".mp3":
                    case ".mp4":
                    case ".ogg":
                    case ".wma":
                        ff = new TagReader(file);
                        break;
                }

                // Do not support
                if (ff == null)
                    continue;

                if (ff.IsAvailableTags())
                    this.Insert(ff, file);
                else
                {
                    ff = new UnReadableFormat(ff, file);
                    if (!ff.IsAvailableTags())
                        throw new NotSupportedException("サポートされていない形式です。");
                    this.Insert(ff, file);
                }

                t = Database.Tracks.Include("Album").Include("Artist").SingleOrDefault(w => w.Path == file);
                if (t != null)
                    this.Items.Add(t);
            }

            // Tick finished
        }

        private void Insert(FileFormat iif, string file)
        {
            int artistId;
            string temp;
            if (iif.GetArtist() != null)
            {
                temp = iif.GetArtist();
                if (this.Database.Artists.Any(w => w.Name == temp))
                    artistId = this.Database.Artists.Single(w => w.Name == temp).Id;
                else
                {
                    if (iif is UnReadableFormat)
                    {
                        // 類似検索
                        var list =
                            this.Database.Artists.ToList().Select(w => new { Name = w.Name.Replace(ReplaceChars, ""), w.Id }).ToList();
                        if (list.Any(w => w.Name == temp.Replace(ReplaceChars, "")))
                            artistId = list.Single(w => w.Name == temp.Replace(ReplaceChars, "")).Id;
                        else
                            artistId = this.Database.Artists.Add(new Artist { Name = temp }).Id;
                    }
                    else
                        artistId = this.Database.Artists.Add(new Artist { Name = temp }).Id;
                }
            }
            else
                artistId = LyraApp.DatabaseUnknownArtist;

            int albumId;
            if (iif.GetAlbum() != null)
            {
                temp = iif.GetAlbum();
                if (this.Database.Albums.Any(w => w.Title == temp))
                    albumId = this.Database.Albums.Single(w => w.Title == temp).Id;
                else
                {
                    if (iif is UnReadableFormat)
                    {
                        // 類似検索
                        var list =
                            this.Database.Albums.ToList().Select(w => new { Title = w.Title.Replace(ReplaceChars, ""), w.Id }).ToList();
                        if (list.Any(w => w.Title == temp.Replace(ReplaceChars, "")))
                            albumId = list.Single(w => w.Title == temp.Replace(ReplaceChars, "")).Id;
                        else
                            albumId = this.Database.Albums.Add(new Album { Title = temp }).Id;
                    }
                    else
                        albumId = this.Database.Albums.Add(new Album { Title = temp }).Id;
                }
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
    }
}