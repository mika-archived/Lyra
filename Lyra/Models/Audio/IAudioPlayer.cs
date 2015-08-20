using System;

namespace Lyra.Models.Audio
{
    public interface IAudioPlayer : IDisposable
    {
        /// <summary>
        /// 音声ファイルを再生します。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        void Play(string path);

        /// <summary>
        /// 現在再生中のファイルをポーズします。
        /// 再度再生する場合は、Playを呼び出してください。
        /// </summary>
        void Pause();

        /// <summary>
        /// 現在再生中のファイルをストップします。
        /// </summary>
        void Stop();

        /// <summary>
        /// 現在のボリュームを取得/設定します。
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// 現在再生中の時間を取得/設定します。
        /// </summary>
        long CurrentTime { get; set; }
    }

    public enum PlayState
    {
        /// <summary>
        /// 再生中
        /// </summary>
        Playing,

        /// <summary>
        /// 一時停止中
        /// </summary>
        Paused,

        /// <summary>
        /// 停止
        /// </summary>
        Stopped
    }
}