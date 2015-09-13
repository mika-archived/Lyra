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
                var t = Database.Tracks.Find(w => w.Path == file).FirstOrDefault();
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
                this.Database.SaveChanges();
                t = Database.Tracks.Find(w => w.Path == file).FirstOrDefault();
                if (t != null)
                    this.Items.Add(t);
            }
            // Tick finished
        }

        private void Insert(FileFormat iif, string file)
        {
            Artist artist;
            if (iif.GetArtist() != null)
            {
                if (this.Database.Artists.Contains(w => w.Name == iif.GetArtist()))
                    artist = this.Database.Artists.Find(w => w.Name == iif.GetArtist()).First();
                else
                {
                    if (iif is UnReadableFormat)
                    {
                        // 類似検索
                        var list =
                            this.Database.Artists.ToEnumerable().ToList();
                        if (list.Any(w => w.Name.Replace(ReplaceChars, "") == iif.GetArtist().Replace(ReplaceChars, "")))
                            artist = list.Single(w => w.Name.Replace(ReplaceChars, "") == iif.GetArtist().Replace(ReplaceChars, ""));
                        else
                            artist = this.Database.Artists.Add(new Artist { Name = iif.GetArtist() });
                    }
                    else
                        artist = this.Database.Artists.Add(new Artist { Name = iif.GetArtist() });
                }
            }
            else
                artist = LyraApp.DatabaseUnknownArtist;

            Album album;
            if (iif.GetAlbum() != null)
            {
                if (this.Database.Albums.Contains(w => w.Title == iif.GetAlbum()))
                    album = this.Database.Albums.Find(w => w.Title == iif.GetAlbum()).First();
                else
                {
                    if (iif is UnReadableFormat)
                    {
                        // 類似検索
                        var list =
                            this.Database.Albums.ToEnumerable().ToList();
                        if (list.Any(w => w.Title.Replace(ReplaceChars, "") == iif.GetAlbum().Replace(ReplaceChars, "")))
                            album = list.Single(w => w.Title.Replace(ReplaceChars, "") == iif.GetAlbum().Replace(ReplaceChars, ""));
                        else
                            album = this.Database.Albums.Add(new Album { Title = iif.GetAlbum(), Artwork = iif.GetArtwork() });
                    }
                    else
                        album = this.Database.Albums.Add(new Album { Title = iif.GetAlbum(), Artwork = iif.GetArtwork() });
                }
            }
            else
                album = LyraApp.DatabaseUnknownAlbum;

            var track = new Track
            {
                Path = file,
                Number = iif.GetTrackNumber(),
                Title = iif.GetTitle(),
                Artist = artist,
                Album = album,
                Duration = iif.GetDuration()
            };

            this.Database.Tracks.Add(track);
        }
    }
}