using System;
using System.IO;

using Un4seen.Bass;

namespace Lyra.Models.Audio
{
    /// <summary>
    /// BASS for .NET
    /// 参考：http://akabeko.me/blog/2010/01/c-%E3%81%A7%E9%9F%B3%E6%A5%BD%E5%86%8D%E7%94%9F-3/
    /// </summary>
    public class BassPlayer : IAudioPlayer
    {
        private int _handle;

        public BassPlayer()
        {
            // SplashScreen 出るとかいうクソ仕様
            BassNet.Registration(LyraApp.BassNetMailAddress, LyraApp.BassNetRegistrationKey);

            // 初期化
            if (!Bass.LoadMe(LyraApp.BassNetModuleDir))
                throw new Exception("Bass.NET の初期化に失敗しました。");

            // デフォを使う
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
                throw new Exception("デバイスの初期化に失敗しました。");

            // Add-on 周り
            Bass.BASS_PluginLoadDirectory(LyraApp.BassNetModuleDir);

            // ボリュームはとりあえず半分
            this.Volume = 0.5f;

            this.PlayState = PlayState.Stopped;
        }

        /// <summary>
        /// 音声ファイルを再生します。
        /// ポーズから再開する場合は、pathはnullにします。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        public void Play(string path = null)
        {
            if (this.PlayState == PlayState.Playing)
                return;

            // 再生不可
            if (path == null && this.PlayState != PlayState.Paused)
                return;

            // handler
            if (path != null)
            {
                if (!File.Exists(path))
                    return;

                this._handle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
                if (this._handle == 0)
                    throw new Exception("ファイルを再生できません。");
            }

            this.PlayState = PlayState.Playing;
            Bass.BASS_ChannelPlay(this._handle, this.PlayState == PlayState.Paused);
        }

        public void Pause()
        {
            if (this.PlayState != PlayState.Playing)
                return;

            this.PlayState = PlayState.Paused;
            Bass.BASS_ChannelPause(this._handle);
        }

        public void Stop()
        {
            if (this.PlayState == PlayState.Stopped)
                return;

            this.PlayState = PlayState.Stopped;
            Bass.BASS_ChannelStop(this._handle);

            // 再生箇所を初期化
            Bass.BASS_ChannelSetPosition(this._handle, 0.0);

            // ファイルを開放
            Bass.BASS_StreamFree(this._handle);

            this._handle = 0;
        }

        /// <summary>
        /// 現在の再生状況を取得します。
        /// </summary>
        public PlayState PlayState { get; private set; }

        //
        private float _volume;

        /// <summary>
        /// 現在のボリュームを取得/設定します。
        /// なお、音声が再生されていない場合は最後に設定した値が取得されます。
        /// </summary>
        public float Volume
        {
            get
            {
                var volume = this._volume;
                Bass.BASS_ChannelGetAttribute(this._handle, BASSAttribute.BASS_ATTRIB_VOL, ref volume);
                return volume;
            }
            set
            {
                var b = Bass.BASS_ChannelSetAttribute(this._handle, BASSAttribute.BASS_ATTRIB_VOL, value);
                if (!b)
                    this._volume = value;
            }
        }

        /// <summary>
        /// 現在再生中の時間を取得/設定します。
        /// </summary>
        public long CurrentTime
        {
            get
            {
                var pos = Bass.BASS_ChannelGetPosition(this._handle);
                return (long)(Bass.BASS_ChannelBytes2Seconds(this._handle, pos) * 1000);
            }
            set
            {
                var pos = Bass.BASS_ChannelSeconds2Bytes(this._handle, value / 1000f);
                Bass.BASS_ChannelSetPosition(this._handle, pos);
            }
        }

        public void Dispose()
        {
            if (this._handle != 0)
                this.Stop();

            Bass.BASS_PluginFree(0);
            Bass.FreeMe();
        }
    }
}