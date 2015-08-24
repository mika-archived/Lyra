using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;

using Livet;

using Lyra.Models;
using Lyra.Models.Database;
using Lyra.Models.Watchers;

namespace Lyra.ViewModels
{
    /// <summary>
    /// MainWindow に TrackList を提供したり
    /// </summary>
    public class TrackListViewModel : ViewModel
    {
        private List<Watcher> _watchers;

        #region TrackList変更通知プロパティ

        private ObservableCollection<TrackViewModel> _TrackList;

        public ObservableCollection<TrackViewModel> TrackList
        {
            get
            { return _TrackList; }
            set
            {
                if (_TrackList == value)
                    return;
                _TrackList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public TrackListViewModel()
        {
            this._watchers = new List<Watcher>();
            this.TrackList = new ObservableCollection<TrackViewModel>();

            // ここもModelでやった方がいいかも？
            var connection = DbProviderFactories.GetFactory(LyraApp.DatabaseProvider).CreateConnection();
            // ReSharper disable once PossibleNullReferenceException
            connection.ConnectionString = LyraApp.DatabaseConnectionString;

            using (var dbContext = new AppDbContext(connection))
            {
                foreach (var location in dbContext.Locations)
                {
                    Watcher watcher;
                    if (location.Path.StartsWith("http://") || location.Path.StartsWith("https://") ||
                        location.Path.StartsWith("ftp://"))
                        watcher = CloudWatcherProvider.Create(location.Path);
                    else
                        watcher = new DirectoryWatcher(location.Path);
                    watcher.OnChanged += Watcher_OnChanged;
                    watcher.Start();
                    this.CompositeDisposable.Add(watcher);
                    this._watchers.Add(watcher);
                }
            }
        }

        // Model？
        private void Watcher_OnChanged(IReadOnlyCollection<Track> items, NotifyCollectionChangedEventArgs e)
        {
            foreach (var track in items)
            {
                var viewModel = new TrackViewModel(track);
                if (this.TrackList.Any(w => w.Track.Id == viewModel.Track.Id))
                    continue;
                DispatcherHelper.UIDispatcher.Invoke(() => this.TrackList.Add(viewModel));
            }
        }
    }
}