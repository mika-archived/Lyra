using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Lyra.Helpers
{
    // クラス名変えたほうがいいかも

    /// <summary>
    /// INotifyPropertyChanged に関する Helper クラスです。
    /// </summary>
    /// <seealso cref="http://nineworks2.blog.fc2.com/blog-entry-6.html"/>
    public static class NotificationHelper
    {
        public static IObservable<PropertyChangedEventArgs> OnPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => notifyPropertyChanged.PropertyChanged += handler,
                handler => notifyPropertyChanged.PropertyChanged -= handler)
                .Do(_ => Debug.WriteLine(_.PropertyName))
                .Where(e => e.PropertyName == propertyName);
        }
    }
}