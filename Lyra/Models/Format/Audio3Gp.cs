using System.Linq;
using System.Text;

namespace Lyra.Models.Format
{
    /// <summary>
    /// Read tagginf info from *.3gp
    /// </summary>
    public class Audio3Gp : TagReader
    {
        public Audio3Gp(string path) : base(path, "audio/mp4")
        {
        }

        public override string GetArtist()
        {
            if (base.GetArtist() == null)
                return null;

            // 基本的には、TagLib#でaudio/mp4で読みこめばいいけど、
            // いらない(0x00)文字が何故か混ざっているので、除去しておく
            // 通常、一番最後に0x00が入っているっぽいけど、念のため
            var sb = new StringBuilder();
            foreach (var c in base.GetArtist().Where(c => c != 0x00))
                sb.Append(c);
            return sb.ToString();
        }

        public override string GetAlbum()
        {
            if (base.GetAlbum() == null)
                return null;

            var sb = new StringBuilder();
            foreach (var c in base.GetAlbum().Where(c => c != 0x00))
                sb.Append(c);
            return sb.ToString();
        }

        public override string GetTitle()
        {
            if (base.GetTitle() == null)
                return null;

            var sb = new StringBuilder();
            foreach (var c in base.GetTitle().Where(c => c != 0x00))
                sb.Append(c);
            return sb.ToString();
        }
    }
}