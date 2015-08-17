using System.Collections.ObjectModel;

using Livet;

using Lyra.Models;

namespace Lyra.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public string Title => "Lyra";

        #region Tracks変更通知プロパティ

        private ReadOnlyCollection<Track> _Tracks;

        public ReadOnlyCollection<Track> Tracks
        {
            get
            { return _Tracks; }
            set
            {
                if (_Tracks == value)
                    return;
                _Tracks = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public MainWindowViewModel()
        {
        }

        public void Initialize()
        {
        }
    }
}