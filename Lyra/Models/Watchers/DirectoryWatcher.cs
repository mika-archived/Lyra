using System.IO;
using System.Linq;

namespace Lyra.Models.Watchers
{
    /// <summary>
    /// ディレクトリを監視し、その変更を DB 及び MainWindow に適用/通知します。
    /// </summary>
    public class DirectoryWatcher : Watcher<Track>
    {
        public DirectoryWatcher(string path) : base(path, 1000)
        {
        }

        /// <summary>
        /// 指定した時間ごとに、Watcher&lt;T&gt;から呼び出されます。
        /// </summary>
        protected override void Tick()
        {
            var fl = Directory.GetFiles(this.Path).AsEnumerable();
        }
    }
}