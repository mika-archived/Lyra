using System;

namespace Lyra.Models.Watchers
{
    /// <summary>
    /// SoundCloud API を用いて、 stream や likes などを監視します。
    /// </summary>
    public class SoundCloudWatcher : Watcher
    {
        public SoundCloudWatcher(string path, int interval) : base(path, interval)
        {
        }

        /// <summary>
        /// 指定した時間ごとに、Watcher&lt;T&gt;から呼び出されます。
        /// </summary>
        protected override void Tick()
        {
            throw new NotImplementedException();
        }
    }
}