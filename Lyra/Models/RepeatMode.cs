namespace Lyra.Models
{
    /// <summary>
    /// リピートモード
    /// </summary>
    public enum RepeatMode
    {
        /// <summary>
        /// リピート再生を行わずに、次の曲へ移動します。
        /// </summary>
        NoRepeat,

        /// <summary>
        /// 1回だけリピートを行い、次の曲へ移動します。
        /// </summary>
        RepeatOnce,

        /// <summary>
        /// 永久にリピート再生を行います。次の曲へ移動することはありません。
        /// </summary>
        RepeatInifinity
    }
}