namespace Lyra.Models
{
    /// <summary>
    /// トラックの存在情報
    /// </summary>
    public enum TrackInfo
    {
        /// <summary>
        /// 再生中
        /// 再生中を示すアイコンを表示する
        /// </summary>
        Playing,

        /// <summary>
        /// ローカルディスク上に存在している
        /// 特に何も表示しない
        /// </summary>
        Local,

        /// <summary>
        /// リモートに存在している
        /// それっぽいアイコン(雲だとか雷とか)を表示する
        /// </summary>
        Remote,

        /// <summary>
        /// そんなものは無い
        /// 警告を示すアイコンを表示する
        /// </summary>
        Nil
    }
}