using System;
using System.Reactive.Linq;

using Livet;
using Livet.Commands;
using Lyra.Extensions;
using Lyra.Helpers;

namespace Lyra.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Title変更通知プロパティ

        private string _Title;

        public string Title
        {
            get
            { return _Title; }
            set
            {
                if (_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedTrack変更通知プロパティ

        private TrackViewModel _SelectedTrack;

        public TrackViewModel SelectedTrack
        {
            get { return _SelectedTrack; }
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
            get { return _PlayerControlViewModel; }
            set
            {
                if (_PlayerControlViewModel == value)
                    return;
                _PlayerControlViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region TrackListViewModel変更通知プロパティ

        private TrackListViewModel _TrackListViewModel;

        public TrackListViewModel TrackListViewModel
        {
            get
            { return _TrackListViewModel; }
            set
            {
                if (_TrackListViewModel == value)
                    return;
                _TrackListViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region StatusBarViewModel変更通知プロパティ

        private StatusBarViewModel _StatusBarViewModel;

        public StatusBarViewModel StatusBarViewModel
        {
            get { return _StatusBarViewModel; }
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
            this.PlayerControlViewModel = new PlayerControlViewModel(this).AddTo(this);
            this.TrackListViewModel = new TrackListViewModel().AddTo(this);
            this.StatusBarViewModel = new StatusBarViewModel().AddTo(this);

            this.Title = "Lyra";
            this.PlayerControlViewModel.PropertyChangedAsObservable(nameof(this.PlayerControlViewModel.PlayingTrack))
                .Where(_ => this.PlayerControlViewModel.PlayingTrack != null)
                .Subscribe(_ => this.Title = $"Lyra - {this.PlayerControlViewModel.PlayingTrack.Track.Title}")
                .AddTo(this);
        }

        public void Initialize()
        {
            AppInitializer.PostInitialize();
        }

        public void UnInitialize()
        {
            //
        }

        private void Play(TrackViewModel track)
        {
            this.PlayerControlViewModel.Play(track);
        }

        #region MouseDoubleClickCommand

        private ListenerCommand<TrackViewModel> _MouseDoubleClickCommand;

        public ListenerCommand<TrackViewModel> MouseDoubleClickCommand => _MouseDoubleClickCommand ??
                                                                          (_MouseDoubleClickCommand =
                                                                              new ListenerCommand<TrackViewModel>(
                                                                                  MouseDoubleClick));

        private void MouseDoubleClick(TrackViewModel parameter)
        {
            this.Play(parameter);
        }

        #endregion
    }
}