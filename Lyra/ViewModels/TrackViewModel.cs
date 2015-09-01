using System;

using Livet;

using Lyra.Models;
using Lyra.Models.Audio;

namespace Lyra.ViewModels
{
    public class TrackViewModel : ViewModel
    {
        public Track Track { get; }

        public string Duration
        {
            get
            {
                var timespan = TimeSpan.FromSeconds(this.Track.Duration / 1000D);
                if (timespan.Hours >= 1)
                    return $"{timespan.Hours:D2}:{timespan.Minutes:D2}:{timespan.Seconds:D2}";
                else
                    return $"{timespan.Minutes:D2}:{timespan.Seconds:D2}";
            }
        }

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
                RaisePropertyChanged();
            }
        }

        #endregion

        public TrackViewModel(Track track)
        {
            this.Track = track;
            this.PlayState = PlayState.Stopped;
        }
    }
}