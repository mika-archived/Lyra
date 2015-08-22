using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Livet;

namespace Lyra.Models.Watchers
{
    /// <summary>
    /// 一定時間ごとに、フォルダーなどを監視します。
    /// </summary>
    public abstract class Watcher<T> : NotificationObject, IDisposable
    {
        private readonly IDisposable _subscriber;

        protected string Path { get; private set; }

        #region Items変更通知プロパティ

        private ObservableCollection<T> _Items;

        public ObservableCollection<T> Items
        {
            get
            { return _Items; }
            set
            {
                if (_Items == value)
                    return;
                _Items = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        protected Watcher(string path, int interval)
        {
            this.Path = path;
            this.Items = new ObservableCollection<T>();
            var timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(interval));
            this._subscriber = timer.Subscribe(_ => this.Tick());
        }

        /// <summary>
        /// 指定した時間ごとに、Watcher&lt;T&gt;から呼び出されます。
        /// </summary>
        protected abstract void Tick();

        public void Dispose()
        {
            this._subscriber.Dispose();
        }
    }
}