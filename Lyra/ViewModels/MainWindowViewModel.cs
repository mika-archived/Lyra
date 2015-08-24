﻿using Livet;
using Livet.Commands;

using Lyra.Extensions;

namespace Lyra.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public string Title => "Lyra";

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