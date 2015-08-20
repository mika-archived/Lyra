using System;
using System.Reactive.Linq;

using Livet;
using Livet.Commands;

using Lyra.Models;
using Lyra.Models.Audio;

namespace Lyra.ViewModels
{
    public class PlayerControlViewModel : ViewModel
    {
        private readonly BassPlayer _player;

        private float _tempVol;

        #region SelectedTrack変更通知プロパティ

        private TrackViewModel _SelectedTrack;

        public TrackViewModel SelectedTrack
        {
            private get
            { return _SelectedTrack; }
            set
            {
                if (_SelectedTrack == value)
                    return;
                _SelectedTrack = value;
                this.PlayCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        #endregion

        #region PlayingTrack変更通知プロパティ

        private TrackViewModel _PlayingTrack;

        public TrackViewModel PlayingTrack
        {
            get
            { return _PlayingTrack; }
            private set
            {
                if (_PlayingTrack == value)
                    return;
                _PlayingTrack = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CurrentTime変更通知プロパティ

        private string _CurrentTime;

        public string CurrentTime
        {
            get
            { return _CurrentTime; }
            private set
            {
                if (_CurrentTime == value)
                    return;
                _CurrentTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CurrentDuration変更通知プロパティ

        private long _CurrentDuration;

        public long CurrentDuration
        {
            get
            { return _CurrentDuration; }
            private set
            {
                if (_CurrentDuration == value)
                    return;
                _CurrentDuration = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Volume変更通知プロパティ

        // Bass側の volume は 0.0f ~ 1.0f
        // 表示側は 0 ~ 100
        public float Volume
        {
            get
            { return this._player.Volume * 100; }
            set
            {
                if (Math.Abs(this._player.Volume - (value / 100)) <= 0)
                    return;
                this._player.Volume = value / 100;
                this._tempVol = value / 100;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region PlayState変更通知プロパティ

        private PlayState _PlayState;

        public PlayState PlayState
        {
            get
            { return _PlayState; }
            set
            {
                if (_PlayState == value)
                    return;
                _PlayState = value;
                this.StopCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        #endregion

        public PlayerControlViewModel()
        {
            this._player = new BassPlayer();

            // ダミー
            this.PlayingTrack = new TrackViewModel(new DummyTrack());

            // ボリュームは 0.5f (前回の設定から読み込むべき)
            this.Volume = 50;

            // タイマー開始
            var timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            var subscriber = timer.Subscribe(_ => this.UpdateTick());
            this.CompositeDisposable.Add(subscriber);
        }

        private void UpdateTick()
        {
            // 再生状態を取得
            this.PlayState = this._player.PlayState;
            if (this.PlayState == PlayState.Stopped)
            {
                // 停止してたらいらぬ
                this.CurrentDuration = 0;
                this.CurrentTime = "00:00";
                return;
            }

            this.CurrentDuration = this._player.CurrentTime;
            var timespan = TimeSpan.FromSeconds(this.CurrentDuration / 1000D);
            if (timespan.Hours >= 1)
                this.CurrentTime = $"{timespan.Hours:D2}:{timespan.Minutes:D2}:{timespan.Seconds:D2}";
            else
                this.CurrentTime = $"{timespan.Minutes:D2}:{timespan.Seconds:D2}";
        }

        #region NextCommand

        private ViewModelCommand _NextCommand;

        public ViewModelCommand NextCommand => _NextCommand ?? (_NextCommand = new ViewModelCommand(Next));

        private void Next()
        {
        }

        #endregion

        #region PlayCommand

        private ViewModelCommand _PlayCommand;

        public ViewModelCommand PlayCommand => _PlayCommand ?? (_PlayCommand = new ViewModelCommand(Play, CanPlay));

        private bool CanPlay()
        {
            return this.SelectedTrack != null;
        }

        private void Play()
        {
            // ポーズ
            if (this.PlayState == PlayState.Playing)
            {
                this._player.Pause();
                return;
            }

            this._player.Play(this.SelectedTrack.Track.FilePath);
            // ボリュームがリセットされるので再度
            this.Volume = this._tempVol * 100;
            this.PlayingTrack = this.SelectedTrack;
        }

        #endregion

        #region StopCommand

        private ViewModelCommand _StopCommand;

        public ViewModelCommand StopCommand => _StopCommand ?? (_StopCommand = new ViewModelCommand(Stop, CanStop));

        private bool CanStop()
        {
            return this._player.PlayState != PlayState.Stopped;
        }

        private void Stop()
        {
            this._player.Stop();
        }

        #endregion

        #region PreviousCommand

        private ViewModelCommand _PreviousCommand;

        public ViewModelCommand PreviousCommand => _PreviousCommand ?? (_PreviousCommand = new ViewModelCommand(Previous));

        private void Previous()
        {
        }

        #endregion
    }
}