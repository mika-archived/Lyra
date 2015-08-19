using System.Collections.ObjectModel;

using Livet;

namespace Lyra.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public string Title => "Lyra";

        #region Tracks変更通知プロパティ

        private ReadOnlyCollection<TrackViewModel> _Tracks;

        public ReadOnlyCollection<TrackViewModel> Tracks
        {
            get
            { return _Tracks; }
            set
            {
                if (_Tracks == value)
                    return;
                _Tracks = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedTrack変更通知プロパティ

        private TrackViewModel _SelectedTrack;

        public TrackViewModel SelectedTrack
        {
            get
            { return _SelectedTrack; }
            set
            {
                if (_SelectedTrack == value)
                    return;
                _SelectedTrack = value;
                this.PlayerControlViewModel.SelectedTrack = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region PlayerControlViewModel変更通知プロパティ

        private PlayerControlViewModel _PlayerControlViewModel;

        public PlayerControlViewModel PlayerControlViewModel
        {
            get
            { return _PlayerControlViewModel; }
            set
            {
                if (_PlayerControlViewModel == value)
                    return;
                _PlayerControlViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region StatusBarViewModel変更通知プロパティ

        private StatusBarViewModel _StatusBarViewModel;

        public StatusBarViewModel StatusBarViewModel
        {
            get
            { return _StatusBarViewModel; }
            set
            {
                if (_StatusBarViewModel == value)
                    return;
                _StatusBarViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            this.PlayerControlViewModel = new PlayerControlViewModel();
        }

        public void Initialize()
        {
        }
    }
}