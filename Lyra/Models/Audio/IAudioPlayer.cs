using System;

using Lyra.Events;

namespace Lyra.Models.Audio
{
    public delegate void PlayingStreamFinishedEvent(object sender, PlayingStreamFinishedEventArgs e);

    public interface IAudioPlayer : IDisposable
    {
        /// <summary>
        /// 現在再生中のトラックが終了した時に発生します。
        /// </summary>
        event PlayingStreamFinishedEvent OnPlayingStreamFinished;

        /// <summary>
        /// 音声ファイルを再生します。
        /// </summary>
        /// <param name="track">再生するトラック</param>
        void Play(Track track);

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