using Livet;

namespace Lyra.ViewModels
{
    public class SettingsWindowViewModel : ViewModel
    {
        #region CurrentPage変更通知プロパティ

        private ViewModel _CurrentPage;

        public ViewModel CurrentPage
        {
            get
            { return _CurrentPage; }
            set
            {
                if (_CurrentPage == value)
                    return;
                _CurrentPage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void Initialize()
        {
        }
    }
}