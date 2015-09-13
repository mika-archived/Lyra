using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using Livet;

using Lyra.Models;
using Lyra.Models.Database.Repositories;
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

            foreach (var location in new AppRepository().Locations.ToEnumerable())
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

        // Model？
        private void Watcher_OnChanged(IReadOnlyCollection<Track> items, NotifyCollectionChangedEventArgs e)
        {
            // TODO: マルチスレッドの闇
            try
            {
                foreach (var track in items)
                {
                    var viewModel = new TrackViewModel(track);
                    if (this.TrackList.Any(w => w.Track.Id == viewModel.Track.Id))
                        continue;
                    DispatcherHelper.UIDispatcher.Invoke(() => this.TrackList.Add(viewModel));
                }

                var value = this.TrackList.OrderBy(w => w.Track.Album.Title).ThenBy(w => w.Track.Number);
                DispatcherHelper.UIDispatcher.Invoke(
                    () => this.TrackList = new ObservableCollection<TrackViewModel>(value));
            }
            catch (InvalidOperationException)
            {
                //
            }
        }
    }
}