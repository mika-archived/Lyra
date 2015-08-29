namespace Lyra.Models.Watchers
{
    /// <summary>
    /// http, https. ftpプロトコルでアクセスするクラウド上のリソースを監視します。
    /// クラウド上のリソースは 10分 ごとに更新などを検知します。
    /// </summary>
    public static class CloudWatcherProvider
    {
        public static Watcher Create(string path, int interval = 1000 * 60 * 60)
        {
            if (path.StartsWith("https://api.soundcloud.com"))
                return new SoundCloudWatcher(path, interval);
            return null;
        }
    }
}