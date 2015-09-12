using System;
using System.Text.RegularExpressions;

namespace Lyra.Models.Format
{
    /// <summary>
    /// タグ情報が読み取り不可能もしくは不正だった場合に使用されます。
    /// </summary>
    public class UnReadableFormat : FileFormat
    {
        private readonly string _artist;
        private readonly string _album;
        private readonly int _number;
        private readonly string _title;
        private readonly int _duration;

        // 現時点では、とりあえずファイルパスから取り出す
        public UnReadableFormat(FileFormat ff, string file)
        {
            var regex = new Regex(@"\\(?<artist>[\w\s]*)\\(?<album>[\w\s]*)\\(?<track>\d*)-(?<title>.*)\.(?<ext>.*)$");
            if (!regex.IsMatch(file) || ff.GetDuration() <= 0)
                throw new NotSupportedException($"ファイル {file} は、サポートされていない形式です。");

            var match = regex.Match(file);
            // 既にある FileFormat 派生から、有効なものがあれば取り出す。
            this._artist = ff.GetArtist() ?? match.Groups["artist"].Value;
            this._album = ff.GetAlbum() ?? match.Groups["album"].Value;
            this._number = ff.GetTrackNumber() <= 0 ? int.Parse(match.Groups["track"].Value) : ff.GetTrackNumber();
            this._title = (ff.GetTitle() == null || ff.GetTitle() == file) ? match.Groups["title"].Value : ff.GetTitle();
            this._duration = ff.GetDuration();
        }

        public override string GetArtist() => this._artist;

        public override string GetAlbum() => this._album;

        public override byte[] GetArtwork() => null;

        public override int GetTrackNumber() => this._number;

        public override string GetTitle() => this._title;

        public override int GetDuration() => this._duration;
    }
}