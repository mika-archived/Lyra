using System.Text;

using Livet;

using Lyra.Models.Audio;

namespace Lyra.ViewModels
{
    public class StatusBarViewModel : ViewModel
    {
        #region StatusMessage変更通知プロパティ

        private string _StatusMessage;

        public string StatusMessage
        {
            get
            { return _StatusMessage; }
            set
            {
                if (_StatusMessage == value)
                    return;
                _StatusMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public StatusBarViewModel()
        {
            this.StatusMessage = "準備完了";
        }

        public void SetStatusMessage(PlayerControlViewModel viewModel)
        {
            if (viewModel.PlayState == PlayState.Stopped)
            {
                this.StatusMessage = "準備完了";
                return;
            }

            var sb = new StringBuilder();
            if (viewModel.PlayState == PlayState.Playing)
                sb.Append("再生中");
            else
                sb.Append("一時停止中");
            sb.Append(" : ");
            sb.Append($"{viewModel.PlayingTrack.Track.Title} ({viewModel.PlayingTrack.Track.Artist.Name} / {viewModel.PlayingTrack.Track.Album.Title})");
            this.StatusMessage = sb.ToString();
        }
    }
}