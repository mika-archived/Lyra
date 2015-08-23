using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;

using Livet;
using Livet.Commands;

using Lyra.Models;
using Lyra.Models.Database;

namespace Lyra.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public string Title => "Lyra";

        #region Tracks変更通知プロパティ

        private ReadOnlyCollection<TrackViewModel> _Tracks;

        public ReadOnlyCollection<TrackViewModel> Tracks
        {
            get { return _Tracks; }
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
            this.PlayerControlViewModel = new PlayerControlViewModel();

            var tracks = new List<TrackViewModel>();

            // Temporary
            var connection = DbProviderFactories.GetFactory(LyraApp.DatabaseProvider).CreateConnection();
            connection.ConnectionString = LyraApp.DatabaseConnectionString;

            using (var dbcontext = new AppDbContext(connection))
            {
                // LINQ で回すと死ぬ, Include("Album"), Include("Artist") で先に読み込んでおかないと、 Binding 時に Track.Album.Name とかが null になる
                foreach (var track in dbcontext.Tracks.Include("Album").Include("Artist"))
                {
                    tracks.Add(new TrackViewModel(track));
                }
            }
            this.Tracks = tracks.AsReadOnly();
        }

        public void Initialize()
        {
            AppInitializer.PostInitialize();
        }

        public void UnInitialize()
        {
            //
        }

        #region MouseDoubleClickCommand

        private ListenerCommand<TrackViewModel> _MouseDoubleClickCommand;

        public ListenerCommand<TrackViewModel> MouseDoubleClickCommand => _MouseDoubleClickCommand ??
                                                                          (_MouseDoubleClickCommand =
                                                                              new ListenerCommand<TrackViewModel>(
                                                                                  MouseDoubleClick));

        private void MouseDoubleClick(TrackViewModel parameter)
        {
            this.PlayerControlViewModel.Play(parameter);
        }

        #endregion
    }
}