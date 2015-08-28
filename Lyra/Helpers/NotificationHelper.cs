using System;
using System.ComponentModel;
using System.Reactive.Linq;

using Livet;

namespace Lyra.Helpers
{
    // クラス名変えたほうがいいかも

    /// <summary>
    /// INotifyPropertyChanged に関する Helper クラスです。
    /// </summary>
    /// <seealso cref="http://nineworks2.blog.fc2.com/blog-entry-6.html"/>
    public static class NotificationHelper
    {
        public static IObservable<PropertyChangedEventArgs> PropertyChangedAsObservable(this NotificationObject notifyPropertyChanged, string propertyName)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => notifyPropertyChanged.PropertyChanged += handler,
                handler => notifyPropertyChanged.PropertyChanged -= handler)
                .Where(e => e.PropertyName == propertyName);
        }

        // どうせなら Subscribe まで一気に。
        public static IDisposable Subscribe(this NotificationObject notifyPropertyChanged, string propertyName, Action<PropertyChangedEventArgs> action)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => notifyPropertyChanged.PropertyChanged += handler,
                handler => notifyPropertyChanged.PropertyChanged -= handler)
                .Where(e => e.PropertyName == propertyName)
                .Subscribe(action.Invoke);
        }
    }
}