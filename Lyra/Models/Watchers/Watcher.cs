using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

using Livet;
using Lyra.Models.Database.Repositories;

namespace Lyra.Models.Watchers
{
    public delegate void TracksChangedEventHandler(IReadOnlyCollection<Track> items, NotifyCollectionChangedEventArgs e);

    /// <summary>
    /// 一定時間ごとに、フォルダーなどを監視します。
    /// </summary>
    public abstract class Watcher : NotificationObject, IDisposable
    {
        /// <summary>
        /// Items に変更があった場合に１秒ごとに通知されます。
        /// </summary>
        public event TracksChangedEventHandler OnChanged;

        private readonly IObservable<long> _timer;

        private IDisposable _subscriber;

        protected string Path { get; private set; }

        protected ObservableCollection<Track> Items { get; }

        protected AppRepository Database { get; }

        protected Watcher(string path, int interval)
        {
            this.Path = path;
            this.Items = new ObservableCollection<Track>();
            Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(this.Items, "CollectionChanged")
                .Throttle(TimeSpan.FromSeconds(1.5))
                .Subscribe(w => this.Items_CollectionChanged(w.Sender, w.EventArgs));

            this._timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(interval));

            Database = new AppRepository();

            if (Database.Locations.Contains(w => w.Path == path))
                return;

            Database.Locations.Add(new Location { Path = path });
            Database.SaveChanges();
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