namespace Lyra.Models
{
    /// <summary>
    /// TrackInfoがRemoteのものの、現在のトラックの状態を示す。
    /// Localのものの場合はLocalになる。
    /// </summary>
    public enum TrackState
    {
        /// <summary>
        /// TrackInfoがRemoteではない。
        /// </summary>
        Local,

        /// <summary>
        /// リモートに存在している = キャッシュしていない
        /// </summary>
        Remote,

        /// <summary>
        /// ストリーム再生中
        /// </summary>
        Streaming,

        /// <summary>
        /// キャッシュ済み
        /// </summary>
        Cached
    }
}