using Livet;

namespace Lyra.ViewModels
{
    public class StatusBarViewModel : ViewModel
    {
        #region StatusMessage変更通知プロパティ

        private string _StatusMessage;

        public string StatusMessage
        {
            get
            { return _StatusMessage; }
            set
            {
                if (_StatusMessage == value)
                    return;
                _StatusMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public StatusBarViewModel()
        {
            this.StatusMessage = "準備完了";
        }
    }
}