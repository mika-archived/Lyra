using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;

using Livet;

using Lyra.Models.Database;

namespace Lyra.Models.Watchers
{
    public delegate void TracksChangedEventHandler(IReadOnlyCollection<Track> items, NotifyCollectionChangedEventArgs e);

    /// <summary>
    /// 一定時間ごとに、フォルダーなどを監視します。
    /// </summary>
    public abstract class Watcher : NotificationObject, IDisposable
    {
        public event TracksChangedEventHandler OnChanged;

        private IObservable<long> _timer;

        private IDisposable _subscriber;

        protected string Path { get; private set; }

        protected ObservableCollection<Track> Items { get; }

        protected AppDbContext Database { get; }

        protected Watcher(string path, int interval)
        {
            this.Path = path;
            this.Items = new ObservableCollection<Track>();
            this.Items.CollectionChanged += Items_CollectionChanged;

            this._timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(interval));

            var connection = DbProviderFactories.GetFactory(LyraApp.DatabaseProvider).CreateConnection();
            // ReSharper disable once PossibleNullReferenceException
            connection.ConnectionString = LyraApp.DatabaseConnectionString;

            this.Database = new AppDbContext(connection);
            if (this.Database.Locations.Any(w => w.Path == path))
                return;

            this.Database.Locations.Add(new Location { Path = path });
            this.Database.SaveChanges();
        }

        public void Start() => this._subscriber = this._timer.Subscribe(_ => this.Tick());

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TracksChangedEventHandler handler = OnChanged;
            handler?.Invoke(new ReadOnlyCollection<Track>(this.Items), e);
        }

        /// <summary>
        /// 指定した時間ごとに、Watcherから呼び出されます。
        /// </summary>
        protected abstract void Tick();

        public void Dispose()
        {
            this._subscriber.Dispose();
        }
    }
}