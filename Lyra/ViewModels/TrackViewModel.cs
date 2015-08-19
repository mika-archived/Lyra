using System;

using Livet;

using Lyra.Models;

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

        public TrackViewModel(Track track)
        {
            this.Track = track;
        }
    }
}